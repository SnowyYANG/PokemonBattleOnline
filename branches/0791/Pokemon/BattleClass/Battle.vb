Imports PokemonBattle.BattleNetwork
Imports PokemonBattle.PokemonData
Public Class Battle
    Public team1, team2 As Team
    Public ground As BattleGround

    Private _turn As Integer = 1
    Private log As ImgTxt
    Private _random As Random
    Private _randomSeed As Integer

    Private _replay As BattleReplay

    Private _mode As BattleMode
    Private _ended As Boolean
    Private _ready As Boolean

    Private _rules As List(Of BattleRule)
    Public _players As New Dictionary(Of Byte, String)
    Private _teamDatas As New Dictionary(Of Byte, TeamData)

    Public Sub New(ByVal textValue As ImgTxt, ByVal mode As BattleMode)
        log = textValue
        _mode = mode

        If _mode = BattleMode.Double_4P Then
            _waitPlayerCount = 4
        Else
            _waitPlayerCount = 2
        End If
    End Sub

    Private _waitPlayerCount As Integer

    Public Sub SetPlayer(ByVal position As Byte, ByVal identity As String)
        _players(position) = identity
        _waitPlayerCount -= 1
    End Sub
    Public Sub SetSeed(ByVal seed As Integer)
        _randomSeed = seed
    End Sub
    Public Sub SetRules(ByVal rules As List(Of BattleRule))
        _rules = rules
    End Sub
    Public Sub SetTeams(ByVal teams As Dictionary(Of Byte, TeamData))
        For Each key As Byte In teams.Keys
            _teamDatas(key) = teams(key)
        Next
    End Sub

    Public Function PlayerReady() As Boolean
        Return _waitPlayerCount = 0
    End Function

    Public Sub InitializeBattle()
        _random = New Random(_randomSeed)
        If _rules.Contains(BattleRule.Random) Then
            For i As Integer = 1 To _teamDatas.Keys.Count
                _teamDatas(Convert.ToByte(i)) = BattleData.GetRandomTeam(_random)
            Next
        End If
        If _mode = BattleMode.Double_4P Then
            team1 = New Team(New String() {_players(1), _players(2)}, log, _random, Mode, _
                             Rules.Contains(BattleRule.PPUp), _teamDatas(1), _teamDatas(2))
            team2 = New Team(New String() {_players(3), _players(4)}, log, _random, Mode, _
                             Rules.Contains(BattleRule.PPUp), _teamDatas(3), _teamDatas(4))
        Else
            team1 = New Team(New String() {_players(1)}, log, _random, Mode, _
                             Rules.Contains(BattleRule.PPUp), _teamDatas(1))
            team2 = New Team(New String() {_players(2)}, log, _random, Mode, _
                             Rules.Contains(BattleRule.PPUp), _teamDatas(2))
        End If

        ground = New BattleGround(_random, team1, team2)
        team1.ground = ground
        team2.ground = ground
        team1.opponentTeam = team2
        team2.opponentTeam = team1

        _replay = New BattleReplay(_teamDatas, _randomSeed, _players, _mode, _rules)
        _ready = True
    End Sub
    Public Function StartBattle() As TurnResult
        log.AddText("战斗开始！")
        If CType(Rules.Contains(BattleRule.PPUp), Boolean) Then log.AddText("可选规则: PP上限提升")
        If CType(Rules.Contains(BattleRule.Random), Boolean) Then log.AddText("可选规则: 随机队伍")
        ShowPokemon()
        log.AddLog(" ")
        If Mode = BattleMode.Single Then
            waitMoveCount = 2
        Else
            waitMoveCount = 4
        End If

        Return New TurnResult(True)
    End Function
    Private Sub ShowPokemon()
        Dim pokemons As New List(Of Pokemon)()
        pokemons.AddRange(team1.SelectedPokemon)
        pokemons.AddRange(team2.SelectedPokemon)


        For Each pm As Pokemon In pokemons
            log.AddText(pm.myTeam.GetPlayerName(pm) & "放出了" & pm.Nickname & "(" & pm.NameBase & ")！")
        Next

        pokemons.Sort(AddressOf ComparePokemonSpeed)

        For Each pm As Pokemon In pokemons
            pm.Showed()
        Next
    End Sub
    Private Function ComparePokemonSpeed(ByVal pokemon1 As Pokemon, ByVal pokemon2 As Pokemon) As Integer
        Return pokemon2.Speed.CompareTo(pokemon1.Speed)
    End Function

    Private changeList As List(Of PlayerBattleMove) = New List(Of PlayerBattleMove)
    Private atkList As List(Of PlayerBattleMove) = New List(Of PlayerBattleMove)
    Private lastMove As Integer = MoveData.InvalidId

    Private waitMoveCount As Integer
    Private nextTurnMove As BattleTurnMove = BattleTurnMove.NextTurn
    Private moveList As List(Of PlayerMove) = New List(Of PlayerMove)

    Public Sub SetMove(ByVal move As PlayerMove)
        moveList.Add(move)
        waitMoveCount -= 1
    End Sub
    Public ReadOnly Property MoveReady() As Boolean
        Get
            Return waitMoveCount = 0
        End Get
    End Property

    Private Function ComparePokemonIndex(ByVal move1 As PlayerMove, ByVal move2 As PlayerMove) As Integer
        Dim pokemons As PokemonIndex() = New PokemonIndex() _
            {PokemonIndex.Pokemon1OfTeam1, PokemonIndex.Pokemon2OfTeam1, PokemonIndex.Pokemon1OfTeam2, PokemonIndex.Pokemon2OfTeam2}

        Return Array.IndexOf(pokemons, move1.Pokemon).CompareTo(Array.IndexOf(pokemons, move2.Pokemon))
    End Function

    Public Function NextResult() As TurnResult
        moveList.Sort(AddressOf ComparePokemonIndex)

        RecordMoves(moveList)

        Dim list As New List(Of PlayerBattleMove)
        For Each move As PlayerMove In moveList
            list.Add(New PlayerBattleMove(move))
        Next
        moveList.Clear()

        If nextTurnMove = BattleTurnMove.Pass Then
            Return PassChange(list(0))
        ElseIf nextTurnMove = BattleTurnMove.Death Then
            Return DeathChange(list)
        Else
            Return NextTurn(list)
        End If
    End Function
    Private Sub RecordMoves(ByVal moves As List(Of PlayerMove))
        _replay.AddMove(moves)
    End Sub

    Private Function NextTurn(ByVal moves As List(Of PlayerBattleMove)) As TurnResult
        BeginTurn()
        SortPlayerMoves(moves)
        Return NextTurn()
    End Function
    Private Function NextTurn() As TurnResult
        Do Until changeList.Count = 0
            Dim move As PlayerBattleMove = changeList(0)
            changeList.Remove(move)
            Dim pursuit As PlayerBattleMove = GetPursuitMove(move.PM, atkList)
            If pursuit IsNot Nothing Then
                atkList.Remove(pursuit)
                pursuit.PM.BattleState.pursuit = True
                NextMove(pursuit)
                pursuit.PM.BattleState.pursuit = False
                AddLine()
            End If
            If move.PM.HP <> 0 Then
                move.PM.ChangePM(move.MoveIndex, False)
                AddLine()
            End If
        Loop
        Do Until atkList.Count = 0
            Dim move As PlayerBattleMove = atkList(0)
            atkList.Remove(move)
            If move.PM.HP <> 0 AndAlso move.PM.myTeam.SelectedPokemon.Contains(move.PM) Then
                NextMove(move)
                If move.PM.BattleState.pass Then
                    Dim result As New TurnResult(False)
                    result.PassPokemon = move.PM
                    nextTurnMove = BattleTurnMove.Pass
                    waitMoveCount = 1
                    Return result
                ElseIf move.PM.BattleState.healwish OrElse move.PM.BattleState.lunarDance Then
                    If move.PM.myTeam.CanChange(move.PM) Then
                        Dim result As New TurnResult(False)
                        result.DeadPokemons.Add(move.PM)
                        nextTurnMove = BattleTurnMove.Death
                        waitMoveCount = 1
                        Return result
                    End If
                End If
                AddLine()
            End If
        Loop


        If team1.NoPM OrElse team2.NoPM Then
            GameOver()
        Else
            Dim deadList As List(Of Pokemon) = EndTurn()
            If team1.NoPM OrElse team2.NoPM Then
                GameOver()
            Else
                If deadList.Count > 0 Then
                    Dim result As New TurnResult(False)
                    result.DeadPokemons.AddRange(deadList)
                    nextTurnMove = BattleTurnMove.Death
                    waitMoveCount = deadList.Count
                    Return result
                End If
            End If
        End If
        nextTurnMove = BattleTurnMove.NextTurn
        waitMoveCount = team1.AliveSelectedPokemonCount + team2.AliveSelectedPokemonCount
        Return New TurnResult(True)
    End Function

    Private Function BringToAttackListFront(ByVal pm As Pokemon) As Boolean
        Dim attackMove As PlayerBattleMove = atkList.Find(Function(m) m.PM Is pm)
        If attackMove IsNot Nothing Then
            atkList.Remove(attackMove)
            atkList.Insert(0, attackMove)
            Return True
        Else
            Return False
        End If
    End Function

    Private Function BringToAttackListBack(ByVal pm As Pokemon) As Boolean
        Dim attackMove As PlayerBattleMove = atkList.Find(Function(m) m.PM Is pm)
        If attackMove IsNot Nothing Then
            atkList.Remove(attackMove)
            atkList.Add(attackMove)
            Return True
        Else
            Return False
        End If
    End Function

    Private Function DeathChange(ByVal moves As List(Of PlayerBattleMove)) As TurnResult
        For Each move As PlayerBattleMove In moves
            move.SetPM(team1, team2)
            move.PM.ChangePM(move.MoveIndex, False)
        Next
        Return NextTurn()
    End Function
    Private Function PassChange(ByVal move As PlayerBattleMove) As TurnResult

        move.SetPM(team1, team2)
        If move.PM.BattleState.uTurn Then
            Dim pursuit As PlayerBattleMove = GetPursuitMove(move.PM, atkList)
            If pursuit IsNot Nothing AndAlso pursuit.PM.HP > 0 Then
                atkList.Remove(pursuit)
                pursuit.PM.BattleState.pursuit = True
                NextMove(pursuit)
                pursuit.PM.BattleState.pursuit = False
            End If
        End If
        If move.PM.HP <> 0 Then move.PM.ChangePM(move.MoveIndex, True)
        AddLine()
        Return NextTurn()
    End Function
    Private Function GetPursuitMove(ByVal changePM As Pokemon, ByVal list As List(Of PlayerBattleMove)) As PlayerBattleMove
        For Each atk As PlayerBattleMove In list
            If atk.Pursuit AndAlso atk.GetCurrentTarget(Me, _random).Contains(changePM) AndAlso _
                Not changePM.myTeam.SelectedPokemon.Contains(atk.PM) Then
                Return atk
            End If
        Next
        Return Nothing
    End Function


    Private Sub BeginTurn()
        log.AddText(" ")
        log.AddText("开始回合 #" & _turn)
        BeginTurn(team1)
        BeginTurn(team2)
        ground.followPokemon = Nothing
        AddLine()
    End Sub
    Private Sub BeginTurn(ByVal team As Team)
        For Each pm As Pokemon In team.SelectedPokemon
            pm.BattleState.moved = False
            pm.BattleState.swaped = False
            pm.turnHurt.Clear()
            pm.turnHurtMove.Clear()
            pm.turnHurtBy.Clear()
            pm.attackSuccessed = False
            If pm.BattleState.stayOneTurn Then pm.BattleState.firstTurnAtk = False
            pm.BattleState.stayOneTurn = True
            pm.CheckTrait()
        Next

    End Sub

    Private EndTurnCount As Boolean

    Private Function EndTurn() As List(Of Pokemon)
        If Not EndTurnCount Then
            team1.EndTurnCount()
            team2.EndTurnCount()
            ground.CountOfTurnEnd(log)

            EndTurnCountPokemon(team1)

            EndTurnCountPokemon(team2)

            EndTurnCount = True
        End If
        Dim list As New List(Of Pokemon)

        If team1.NoPM OrElse team2.NoPM Then Return list

        list.AddRange(GetDeathSwapPokemons(team1))
        list.AddRange(GetDeathSwapPokemons(team2))

        If List.Count > 0 Then Return List

        log.AddText("结束回合 #" & _turn)
        For Each pm As Pokemon In team1.SelectedPokemon
            If pm.HP <> 0 Then log.AddLog(pm.GetNameString & "(" & pm.NameBase & "):" & pm.HP & " HP" & GetStateString(pm.State))
        Next
        For Each pm As Pokemon In team2.SelectedPokemon
            If pm.HP <> 0 Then log.AddLog(pm.GetNameString & "(" & pm.NameBase & "):" & pm.HP & " HP" & GetStateString(pm.State))
        Next
        _turn += 1
        EndTurnCount = False
        Return List
    End Function

    Public Sub EndTurnCountPokemon(ByVal team As Team)
        For Each pm As Pokemon In team.SelectedPokemon
            If pm.HP <> 0 Then
                pm.BattleState.CountOfEndTurn(pm, ground, log)
                pm.CountOfEndTurn(_random, Me)
            End If
        Next
    End Sub

    Private Function GetDeathSwapPokemons(ByVal team As Team) As List(Of Pokemon)
        Dim list As New List(Of Pokemon)
        For i As Integer = 0 To team.SelectedPokemon.Count - 1
            Dim pm As Pokemon = team.SelectedPokemon(i)
            If pm.HP = 0 AndAlso team.CanChange(pm) AndAlso team.AlivePokemonCount > list.Count Then
                list.Add(pm)
            End If
        Next
        Return list
    End Function

    Public ReadOnly Property Mode() As BattleMode
        Get
            Return _mode
        End Get
    End Property
    Public ReadOnly Property Rules() As List(Of BattleRule)
        Get
            Return _rules
        End Get
    End Property
    Public ReadOnly Property Replay() As BattleReplay
        Get
            Return _replay
        End Get
    End Property
    Public ReadOnly Property Ended() As Boolean
        Get
            Return _ended
        End Get
    End Property
    Public ReadOnly Property BattleReady() As Boolean
        Get
            Return _ready
        End Get
    End Property
    Public Function GetCaption() As String
        If _mode <> BattleMode.Double_4P Then
            Return _players(1) & " VS " & _players(2)
        Else
            Return _players(1) & " & " & _players(2) & " VS " & _players(3) & " & " & _players(4)
        End If
    End Function
    Private Function CountPokemon() As Integer
        Dim team1count As Integer, team2count As Integer
        For Each pm As Pokemon In team1.Pokemon
            If pm IsNot Nothing AndAlso pm.HP <> 0 Then team1count += 1
        Next
        For Each pm As Pokemon In team2.Pokemon
            If pm IsNot Nothing AndAlso pm.HP <> 0 Then team2count += 1
        Next
        log.AddText(team1count & "  :  " & team2count)
        Return team1count.CompareTo(team2count)
    End Function
    Private Sub GameOver()
        log.AddText("战斗结束！")
        Dim result As Integer = CountPokemon()
        If result > 0 Then
            log.AddText(team1.PlayerName & "获得了胜利！")
        ElseIf result < 0 Then
            log.AddText(team2.PlayerName & "获得了胜利！")
        Else
            log.AddText(team1.PlayerName & "与" & team2.PlayerName & "打成了平手！")
        End If
        _ended = True
    End Sub
    Public Sub Tie()
        log.AddText("战斗结束！")
        CountPokemon()
        log.AddText(team1.PlayerName & "与" & team2.PlayerName & "打成了平手！")
        _ended = True
    End Sub
    Public Sub ExitGame(ByVal player As String)
        log.AddText(player & "离开了游戏")
        CountPokemon()
        _ended = True
    End Sub

    Public Function GetPokemons(ByVal playerIndex As Integer) As List(Of Pokemon)
        Dim list As New List(Of Pokemon)
        If Mode = BattleMode.Double_4P Then
            If playerIndex = 1 Then
                If team1.SelectedPokemon(0).HP > 0 OrElse team1.CanChange(team1.SelectedPokemon(0)) Then list.Add(team1.SelectedPokemon(0))
            ElseIf playerIndex = 2 Then
                If team1.SelectedPokemon(1).HP > 0 OrElse team1.CanChange(team1.SelectedPokemon(1)) Then list.Add(team1.SelectedPokemon(1))
            ElseIf playerIndex = 3 Then
                If team2.SelectedPokemon(0).HP > 0 OrElse team2.CanChange(team2.SelectedPokemon(0)) Then list.Add(team2.SelectedPokemon(0))
            ElseIf playerIndex = 4 Then
                If team2.SelectedPokemon(1).HP > 0 OrElse team2.CanChange(team2.SelectedPokemon(1)) Then list.Add(team2.SelectedPokemon(1))
            End If
        Else
            If playerIndex = 1 Then
                For Each pm As Pokemon In team1.SelectedPokemon
                    If pm.HP > 0 OrElse (team1.CanChange(pm) AndAlso list.Count < team1.AlivePokemonCount) Then list.Add(pm)
                Next
            Else
                For Each pm As Pokemon In team2.SelectedPokemon
                    If pm.HP > 0 OrElse (team2.CanChange(pm) AndAlso list.Count < team2.AlivePokemonCount) Then list.Add(pm)
                Next
            End If
        End If
        Return list
    End Function
    Public Function GetPokemonIndex(ByVal pm As Pokemon) As PokemonIndex
        Dim index1 As Integer = team1.SelectedPokemon.IndexOf(pm)
        Dim index2 As Integer = team2.SelectedPokemon.IndexOf(pm)
        If index1 = 0 Then
            Return PokemonIndex.Pokemon1OfTeam1
        ElseIf index1 = 1 Then
            Return PokemonIndex.Pokemon2OfTeam1
        ElseIf index2 = 0 Then
            Return PokemonIndex.Pokemon1OfTeam2
        ElseIf index2 = 1 Then
            Return PokemonIndex.Pokemon2OfTeam2
        End If
    End Function
    Public Function GetPokemonFromIndex(ByVal index As PokemonIndex) As Pokemon
        Select Case index
            Case PokemonIndex.Pokemon1OfTeam1
                Return team1.SelectedPokemon(0)
            Case PokemonIndex.Pokemon1OfTeam2
                Return team2.SelectedPokemon(0)
            Case PokemonIndex.Pokemon2OfTeam1
                Return team1.SelectedPokemon(1)
            Case PokemonIndex.Pokemon2OfTeam2
                Return team2.SelectedPokemon(1)
        End Select
        Return Nothing
    End Function
    Public Function GetTeam(ByVal playerIndex As Integer) As Team
        If Mode = BattleMode.Double_4P Then
            If playerIndex <= 2 Then
                Return team1
            Else
                Return team2
            End If
        Else
            If playerIndex = 1 Then
                Return team1
            Else
                Return team2
            End If
        End If
    End Function
    Public Function GetTargetsFromIndex(ByVal pmValue As Pokemon, ByVal move As Move, ByVal targetIndex As TargetIndex) As List(Of Pokemon)
        Dim list As List(Of Pokemon)
        Dim myTeam As Team = pmValue.myTeam
        Dim opponentTeam As Team
        If myTeam Is team1 Then
            opponentTeam = team2
        Else
            opponentTeam = team1
        End If
        list = New List(Of Pokemon)
        Select Case targetIndex
            Case targetIndex.DefaultTarget
                If pmValue.BattleState.bide Then Exit Select
                Select Case move.Target
                    Case MoveTarget.二体
                        list.AddRange(opponentTeam.SelectedPokemon)
                    Case MoveTarget.全体
                        list.AddRange(team1.SelectedPokemon)
                        list.AddRange(team2.SelectedPokemon)
                        list.Remove(pmValue)

                    Case MoveTarget.己场
                    Case MoveTarget.敌场
                    Case MoveTarget.全场
                    Case MoveTarget.最近
                    Case MoveTarget.技能
                End Select
            Case targetIndex.Opponent1
                list.Add(opponentTeam.SelectedPokemon(0))
            Case targetIndex.Opponent2
                list.Add(opponentTeam.SelectedPokemon(1))
            Case targetIndex.Random
                list.Add(opponentTeam.SelectedPokemon(_random.Next(0, opponentTeam.SelectedPokemon.Count)))
            Case targetIndex.Self
                list.Add(pmValue)
            Case targetIndex.TeamFriend
                If pmValue.Friend IsNot Nothing Then list.Add(pmValue.Friend)
        End Select

        Return list
    End Function

#Region "SortPlayerMove"
    Private Sub SortPlayerMoves(ByVal moves As List(Of PlayerBattleMove))
        SetRandomPriority(moves)

        For Each move As PlayerBattleMove In moves
            move.SetPM(team1, team2)
            If move.Move = BattleMove.SwapPokemon Then
                changeList.Add(move)
            ElseIf move.Move = BattleMove.Attack Then
                atkList.Add(move)
            End If
        Next
        For Each move As PlayerBattleMove In atkList
            With move.PM
                If move.FocusPunch AndAlso .State <> PokemonState.Sleep AndAlso .State <> PokemonState.Freeze _
                    AndAlso Not (.SelTrait = Trait.偷懒 AndAlso .BattleState.traitRaised) Then log.AddText(.GetNameString & "正在蓄力")
                If .Item = Item.果实62 AndAlso atkList.Count > 1 Then .RaiseItem()
            End With
        Next

        SortChangeList(changeList)
        SortAttackList(atkList)
    End Sub
    Private Sub SetRandomPriority(ByVal moves As List(Of PlayerBattleMove))
        Dim randomList As New List(Of Integer)
        For i As Integer = 1 To moves.Count
            randomList.Add(i)
        Next
        For Each move As PlayerBattleMove In moves
            Dim randomIndex As Integer = _random.Next(0, randomList.Count)
            move.RandomPriority = randomList(randomIndex)
            randomList.RemoveAt(randomIndex)
        Next
    End Sub

    Private Sub SortChangeList(ByVal list As List(Of PlayerBattleMove))
        list.Sort(AddressOf CompareChange)
    End Sub
    Private Sub SortAttackList(ByVal list As List(Of PlayerBattleMove))
        For Each playerMove As PlayerBattleMove In list
            playerMove.SetPriority(_random)
        Next
        list.Sort(AddressOf CompareAttack)
    End Sub

    Private Function CompareChange(ByVal move1 As PlayerBattleMove, ByVal move2 As PlayerBattleMove) As Integer
        If move1 Is Nothing AndAlso move2 Is Nothing Then Return 0
        If move1 Is Nothing Then Return 1
        If move2 Is Nothing Then Return -1
        If move1 Is move2 Then Return 0
        Return move1.RandomPriority.CompareTo(move2.RandomPriority)
    End Function
    Private Function CompareAttack(ByVal move1 As PlayerBattleMove, ByVal move2 As PlayerBattleMove) As Integer
        If move1 Is Nothing AndAlso move2 Is Nothing Then Return 0
        If move1 Is Nothing Then Return 1
        If move2 Is Nothing Then Return -1
        If move1 Is move2 Then Return 0

        Dim priority1, priority2 As Double

        priority1 = move1.GetPriority()
        priority2 = move2.GetPriority()

        If priority1 <> priority2 Then
            Return priority2.CompareTo(priority1)
        End If
        If move1.PM.SelTrait = Trait.后手打击 AndAlso move2.PM.SelTrait = Trait.后手打击 Then
            Return move1.PM.Speed.CompareTo(move2.PM.Speed)
        ElseIf move1.PM.SelTrait = Trait.后手打击 Then
            Return 1
        ElseIf move2.PM.SelTrait = Trait.后手打击 Then
            Return -1
        End If
        If move1.PM.Speed <> move2.PM.Speed Then
            If ground.TrickRoom Then Return move1.PM.Speed.CompareTo(move2.PM.Speed)
            Return move2.PM.Speed.CompareTo(move1.PM.Speed)
        Else
            Return move1.RandomPriority.CompareTo(move2.RandomPriority)
        End If
    End Function

    Public Shared Function CountAtkPriority(ByVal pm As Pokemon, ByVal moveIndex As Integer) As Integer
        Dim priority As Integer
        If moveIndex <> StruggleIndex AndAlso pm.CanChooseMove Then priority = pm.SelMove(moveIndex).Priority
        Return priority
    End Function
#End Region

#Region "BattleLogic"
    Private Function GetStateString(ByVal stateValue As PokemonState) As String
        Select Case stateValue
            Case PokemonState.Burn
                Return "(烧伤)"
            Case PokemonState.Freeze
                Return "(冻结)"
            Case PokemonState.Paralysis
                Return "(麻痹)"
            Case PokemonState.Sleep
                Return "(睡眠)"
            Case PokemonState.Toxin
                Return "(中毒)"
            Case PokemonState.Poison
                Return "(中毒)"
        End Select
        Return ""
    End Function

    Private Sub NextMove(ByVal move As PlayerBattleMove)
        move.SetTarget(Me, _random)
        Dim moveValue As Move
        Dim pm As Pokemon = move.PM
        Dim moveIndex As Integer = move.MoveIndex
        Dim targets As List(Of Pokemon) = move.Target

        pm.BattleState.moved = True
        pm.BattleState.destinyBond = False
        pm.BattleState.rage = False
        pm.BattleState.grudge = False

        If pm.BattleState.nextTurnCantMove = True Then
            log.AddText(pm.GetNameString & "因反作用力而不能行动")
            pm.BattleState.nextTurnCantMove = False
            Return
        Else
            moveValue = GetPokemonMove(move)
            If moveValue Is Nothing Then Return
        End If

        If Not CanMove(pm, moveValue, moveIndex) Then
            If pm.BattleState.Hide OrElse pm.BattleState.prepare Then Prepare(moveValue, pm)
            Return
        End If
        If Not Prepare(moveValue, pm) Then Return
        UseSkill(pm, moveValue, targets, False)
    End Sub

    Private Function GetPokemonMove(ByVal move As PlayerBattleMove) As Move
        Dim moveValue As Move
        Dim pm As Pokemon = move.PM
        Dim moveIndex As Integer = move.MoveIndex

        If pm.BattleState.CrazyAttack OrElse pm.BattleState.Hide OrElse pm.BattleState.prepare Then
            moveValue = pm.BattleState.nextMove
        ElseIf pm.BattleState.bide Then
            If pm.BattleState.bideCounter = 1 Then Return Nothing
            moveValue = pm.BattleState.nextMove
            pm.BattleState.nextMove = Nothing
        ElseIf move.Struggle Then
            moveValue = New Move(BattleData.GetMove("拼命"), False)
        Else
            If pm.BattleState.encore Then
                moveValue = pm.LastMove
                If moveValue.Target <> pm.SelMove(moveIndex).Target Then
                    Dim targetIndex As TargetIndex = targetIndex.DefaultTarget
                    Select Case moveValue.Target
                        Case MoveTarget.选一, MoveTarget.任选, MoveTarget.随机
                            targetIndex = targetIndex.Random
                        Case MoveTarget.队友
                            targetIndex = targetIndex.TeamFriend
                        Case MoveTarget.自身, MoveTarget.任选
                            targetIndex = targetIndex.Self
                    End Select
                    move.Target.Clear()
                    move.Target.AddRange(GetTargetsFromIndex(pm, moveValue, targetIndex))
                End If
            Else
                moveValue = pm.SelMove(moveIndex)
            End If
        End If

        Return moveValue
    End Function

    Private Function UseSkillSuccessed(ByVal move As Move, ByVal user As Pokemon, ByVal targets As List(Of Pokemon), _
        ByVal moveCalled As Boolean) As Boolean
        Dim failed As Boolean = MoveFail(move, user, targets)
        If Not user.BattleState.bide Then
            If Not moveCalled Then
                If user.BattleState.nextMove IsNot move Then
                    move.Used()
                    If (move.AttackAtTarget OrElse move.Target = MoveTarget.敌场 OrElse move.Target = MoveTarget.全场 _
                        OrElse move.AddEff2 = MoveAdditionalEffect.二回合后伤害) _
                        AndAlso move.PP > 0 Then
                        If move.Target = MoveTarget.敌场 OrElse move.Target = MoveTarget.全场 Then _
                            targets = user.myTeam.opponentTeam.SelectedPokemon
                        For Each target As Pokemon In targets
                            If target.SelTrait = Trait.压力 AndAlso target.HP <> 0 Then move.Used() : Exit For
                        Next
                    End If
                    If move.Name = "拼命" Then
                        user.lastMoveIndex = StruggleIndex
                    Else
                        For i As Byte = 1 To 4
                            If user.SelMove(i) Is move Then
                                user.lastMoveIndex = i
                                Exit For
                            End If
                        Next
                    End If
                End If
                If user.ChoiceItem Then user.UsedItem = user.Item
            End If
            log.AddText(user.GetNameString & "使用了" & move.Name & "！")

            If move.AddEff2 = MoveAdditionalEffect.震级 Then
                Dim rand As Double = _random.NextDouble
                If rand <= 0.05 Then
                    log.AddText("震级4！")
                    move.SetPower(10)
                ElseIf rand <= 0.15 Then
                    log.AddText("震级5！")
                    move.SetPower(30)
                ElseIf rand <= 0.35 Then
                    log.AddText("震级6！")
                    move.SetPower(50)
                ElseIf rand <= 0.65 Then
                    log.AddText("震级7！")
                    move.SetPower(70)
                ElseIf rand <= 0.85 Then
                    log.AddText("震级8！")
                    move.SetPower(90)
                ElseIf rand <= 0.95 Then
                    log.AddText("震级9！")
                    move.SetPower(110)
                Else
                    log.AddText("震级10！")
                    move.SetPower(150)
                End If
            End If
        End If
        If failed Then Fail() : Return False
        Return True
    End Function

    Private Function CanMove(ByVal pm As Pokemon, ByVal move As Move, ByVal index As Integer) As Boolean
        If pm.State = PokemonState.Sleep Then
            If pm.sleepCounter = pm.sleepTurn Then
                log.AddText(pm.GetNameString & "醒了过来！")
                pm.State = PokemonState.No
            Else
                pm.sleepCounter += 1
                log.AddText(pm.GetNameString & "正在睡觉")
                If move.Effect <> MoveEffect.梦话 AndAlso move.AddEff2 <> MoveAdditionalEffect.自身睡眠攻击有效 Then Return False
            End If
        End If

        If pm.SelTrait = Trait.偷懒 AndAlso pm.BattleState.traitRaised Then
            log.AddText(pm.GetNameString & "正在偷懒！")
            Return False
        End If
        If pm.State = PokemonState.Freeze Then
            If _random.NextDouble > 0.25 AndAlso move.AddEff2 <> MoveAdditionalEffect.解除冻结 Then
                log.AddText(pm.GetNameString & "因冻结而不能行动")
                Return False
            Else
                pm.State = PokemonState.No
                log.AddText(pm.GetNameString & "解冻了！")
            End If
        End If
        If pm.BattleState.afraid Then
            log.AddText(pm.GetNameString & "因害怕而不能行动")
            Return False
        End If
        If pm.BattleState.confused Then
            If pm.BattleState.confusedCounter = pm.BattleState.confusedTurn Then
                log.AddText(pm.GetNameString & "解除了混乱！")
                pm.BattleState.confused = False
            Else
                pm.BattleState.confusedCounter += 1
                log.AddText(pm.GetNameString & "混乱了")
                If _random.NextDouble < 0.5 Then
                    log.AddText(pm.CustomName & "攻击了自己")
                    Dim damage As Double = ((pm.LV * 0.4 + 2) * pm.Attack * 40 / pm.Defence * 0.02 + 2) * _random.Next(85, 101) * 0.01
                    pm.GetHurt(damage)
                    Return False
                End If
            End If
        End If
        If pm.BattleState.captivated Then
            log.AddText(pm.GetNameString & "着迷了")
            If _random.NextDouble < 0.5 Then
                log.AddText(pm.CustomName & "爱上了" & pm.BattleState.captivateTarget.GetNameString)
                Return False
            End If
        End If
        If pm.State = PokemonState.Paralysis AndAlso pm.SelTrait <> Trait.魔法守护 AndAlso _random.NextDouble < 0.25 Then
            log.AddText(pm.GetNameString & "因麻痹而不能行动")
            Return False
        End If
        If index <> StruggleIndex AndAlso pm.CanChooseMove AndAlso Not pm.CanUseMove(index, True) Then Return False
        Return True
    End Function

    Private Function Prepare(ByVal selMove As Move, ByVal user As Pokemon) As Boolean
        Dim prepareCount As Boolean = False
        If user.BattleState.prepare OrElse user.BattleState.Hide Then prepareCount = True
        Select Case selMove.AddEff2
            Case MoveAdditionalEffect.准备后攻击
                If prepareCount Then
                    user.BattleState.prepare = False
                    user.BattleState.nextMove = Nothing
                ElseIf user.Item = Item.力量草药 Then
                    user.RaiseItem()
                Else
                    user.BattleState.prepare = True
                    user.BattleState.nextMove = selMove
                    log.AddText(user.GetNameString & "正在准备！")
                    Return False
                End If
            Case MoveAdditionalEffect.火箭头槌
                If prepareCount Then
                    user.BattleState.prepare = False
                    user.BattleState.nextMove = Nothing
                ElseIf user.Item = Item.力量草药 Then
                    user.RaiseItem()
                    user.DefLVUp(1, False)
                Else
                    user.BattleState.prepare = True
                    user.BattleState.nextMove = selMove
                    log.AddText(user.GetNameString & "正在准备！")
                    user.DefLVUp(1, False)
                    Return False
                End If
            Case MoveAdditionalEffect.太阳光线
                If prepareCount Then
                    user.BattleState.prepare = False
                    user.BattleState.nextMove = Nothing
                ElseIf ground.Weather <> Weather.Sunny Then
                    If user.Item = Item.力量草药 Then
                        user.RaiseItem()
                    Else
                        user.BattleState.prepare = True
                        user.BattleState.nextMove = selMove
                        log.AddText(user.GetNameString & "正在准备！")
                        Return False
                    End If
                End If
            Case MoveAdditionalEffect.潜水后攻击
                Return user.Dive(prepareCount, selMove)
            Case MoveAdditionalEffect.挖洞后攻击
                Return user.Dig(prepareCount, selMove)
            Case MoveAdditionalEffect.跳起后攻击
                Return user.Jump(prepareCount, selMove)
            Case MoveAdditionalEffect.飞天后攻击
                Return user.Fly(prepareCount, selMove)
            Case MoveAdditionalEffect.消失后攻击
                Return user.Disappear(prepareCount, selMove)
        End Select
        Return True
    End Function

    Private Function MoveFail(ByVal selMove As Move, ByVal user As Pokemon, ByVal target As List(Of Pokemon)) As Boolean
        Dim fail As Boolean
        Select Case selMove.AddEff2
            Case MoveAdditionalEffect.对象睡眠攻击有效
                If target IsNot Nothing AndAlso Not target(0).State = PokemonState.Sleep Then
                    fail = True
                End If
            Case MoveAdditionalEffect.自身睡眠攻击有效
                If Not user.State = PokemonState.Sleep Then fail = True
            Case MoveAdditionalEffect.受到伤害攻击失败
                If user.turnHurt.Count > 0 Then fail = True
            Case MoveAdditionalEffect.呕吐
                If user.BattleState.stockpileCounter = 0 Then fail = True
            Case MoveAdditionalEffect.二回合后伤害
                If target IsNot Nothing AndAlso target(0).BattleState.futureAtk Then
                    fail = True
                End If
            Case MoveAdditionalEffect.下马威
                If Not user.BattleState.firstTurnAtk Then fail = True
            Case MoveAdditionalEffect.自然恩惠
                If Not (0 < user.Item(True) AndAlso user.Item(True) < 65) Then
                    fail = True
                End If
            Case MoveAdditionalEffect.其他招式用过后可使用
                If selMove Is user.SelMove(1) AndAlso user.SelMove(2) Is Nothing Then
                    fail = True
                Else
                    For i As Integer = 1 To 4
                        If user.SelMove(i) IsNot Nothing AndAlso user.SelMove(i).AddEff2 <> MoveAdditionalEffect.其他招式用过后可使用 Then
                            If Not user.SelMove(i).EverUsed Then fail = True
                        End If
                    Next
                End If
            Case MoveAdditionalEffect.对手攻击技巧有效
                fail = True
                If target IsNot Nothing Then
                    For Each move As PlayerBattleMove In atkList
                        If move.PM Is target(0) AndAlso move.AttackMove Then
                            fail = False
                        End If
                    Next
                End If
        End Select
        If selMove.AddEff1 = MoveAdditionalEffect.自爆 AndAlso (ground.湿气 OrElse user.SelTrait = Trait.破格) Then fail = True
        Return fail
    End Function

    Private Function CanAttackTarget(ByVal move As Move, ByVal target As Pokemon, ByVal user As Pokemon) As Boolean
        If target.HP = 0 Then
            log.AddText(user.CustomName & "的技能没有对象！")
            Return False
        End If
        If move.AddEff2 = MoveAdditionalEffect.同调噪音 Then
            If user.Type2 Is Nothing Then
                If target.Type2 IsNot Nothing OrElse Not target.HaveType(user.Type1.Name) Then
                    Return False
                End If
            Else
                If Not target.HaveType(user.Type1.Name) OrElse Not target.HaveType(user.Type2.Name) Then
                    Return False
                End If
            End If
        End If
        If target.BattleState.protect AndAlso move.Protectable Then
            log.AddText(target.GetNameString & "保护了自己！")
            Return False
        End If
        If target.myTeam.quickDefence AndAlso move.Priority > 0 AndAlso move.Protectable Then
            log.AddText(target.GetNameString & "受到保护！")
            Return False
        End If
        If target.myTeam.wideDefence AndAlso move.Protectable Then
            log.AddText(target.GetNameString & "受到保护！")
            Return False
        End If
        If ((target.BattleState.dive AndAlso move.AddEff2 <> MoveAdditionalEffect.对象潜水伤害加倍 AndAlso _
            move.AddEff2 <> MoveAdditionalEffect.击中潜水对象) OrElse _
            (target.BattleState.dig AndAlso move.AddEff2 <> MoveAdditionalEffect.对象挖洞伤害加倍) OrElse _
            ((target.BattleState.fly OrElse target.BattleState.jump) AndAlso move.AddEff2 <> MoveAdditionalEffect.对象空中伤害加倍 _
             AndAlso move.AddEff2 <> MoveAdditionalEffect.雷电 AndAlso move.AddEff2 <> MoveAdditionalEffect.击坠) _
            OrElse target.BattleState.disappear) _
            AndAlso (move.AttackAtTarget AndAlso user.SelTrait <> Trait.不要防守 AndAlso target.SelTrait <> Trait.不要防守) Then
            log.AddText(target.GetNameString & "不在攻击范围！")
            Return False
        End If

        Dim selMoveType As PokemonType = GetAttackMoveType(move, user)

        If Not user.SelTrait = Trait.破格 AndAlso move.AttackAtTarget Then
            Dim traitEffect As Boolean = False

            If (target.SelTrait = Trait.蓄水 OrElse target.SelTrait = Trait.吸水 OrElse target.SelTrait = Trait.干燥皮肤) _
                AndAlso selMoveType.Name = "水" Then
                traitEffect = True
            End If

            If (target.SelTrait = Trait.蓄电 OrElse target.SelTrait = Trait.避雷针 OrElse target.SelTrait = Trait.电力引擎) _
                AndAlso selMoveType.Name = "电" Then
                traitEffect = True
            End If

            If target.SelTrait = Trait.引火 AndAlso selMoveType.Name = "火" AndAlso target.State <> PokemonState.Freeze Then _
               traitEffect = True

            If target.SelTrait = Trait.电力引擎 AndAlso selMoveType.Name = "电" Then _
               traitEffect = True

            If traitEffect Then
                target.RaiseTrait()
                Return False
            End If

            If target.IsAboveGroud _
                AndAlso selMoveType.Name = "地面" AndAlso _
                Not (move.MoveType = MoveType.其他 OrElse ground.Gravity OrElse target.Item = Item.黑铁球 OrElse target.BattleState.ingrain) Then _
                log.AddText(target.GetNameString & "浮在空中不受技能影响！") : Return False

            If target.SelTrait = Trait.隔音 AndAlso move.Sound _
                Then log.AddText(target.GetNameString & "的隔音特性使得技能无效！") : Return False
        End If
        Return True
    End Function

    Private Function CalculateAcc(ByVal user As Pokemon, ByVal target As Pokemon, ByVal selMove As Move) As Double
        Dim acc As Double = 1
        If selMove.MoveType = MoveType.其他 AndAlso (selMove.Target = MoveTarget.自身 OrElse selMove.Target = MoveTarget.己场 _
            OrElse selMove.Target = MoveTarget.技能 OrElse selMove.Target = MoveTarget.全场 OrElse selMove.Target = MoveTarget.敌场 _
            OrElse selMove.Target = MoveTarget.全体 OrElse selMove.Target = MoveTarget.队友) Then
            Return 1
        End If
        If selMove.AddEff2 = MoveAdditionalEffect.必杀 Then
            If user.LV < target.LV Then Return -1
        Else
            If target.BattleState.telekinesis Then Return 1
        End If
        If selMove.Acc > 1 Then
            Return 1
        End If

        If selMove.AddEff2 = MoveAdditionalEffect.必中 Then
            Return 1
        End If
        If (selMove.AddEff2 = MoveAdditionalEffect.雷电 OrElse selMove.AddEff2 = MoveAdditionalEffect.雨天必中) _
            AndAlso ground.Weather = Weather.Rainy Then
            Return 1
        End If
        If target.BattleState.lockOn Then
            Return 1
        End If
        If selMove.AddEff2 = MoveAdditionalEffect.暴风雪 AndAlso ground.Weather = Weather.HailStorm Then
            Return 1
        End If
        If user.BattleState.pursuit Then
            Return 1
        End If
        If user.SelTrait = Trait.不要防守 OrElse target.SelTrait = Trait.不要防守 Then
            Return 1
        End If

        If selMove.AddEff2 = MoveAdditionalEffect.必杀 Then
            If user.LV < target.LV Then
                Return -1
            Else
                acc = (30 + user.LV - target.LV) / 100
            End If
        Else
            acc = selMove.Acc
            If selMove.AddEff2 = MoveAdditionalEffect.雷电 AndAlso ground.Weather = Weather.Sunny Then acc = 0.5
            acc *= target.Evasion(user, selMove.AddEff2 = MoveAdditionalEffect.无视对象能力提升)

            If ground.Gravity Then acc *= 5 / 3

            If Not selMove.AddEff2 = MoveAdditionalEffect.必杀 Then
                If Not user.SelTrait = Trait.破格 AndAlso _
                    (target.SelTrait = Trait.沙隐术 AndAlso ground.Weather = Weather.SandStorm) OrElse _
                    (target.SelTrait = Trait.雪遁 AndAlso ground.Weather = Weather.HailStorm) OrElse _
                    (target.SelTrait = Trait.千鸟足 AndAlso target.BattleState.confused) Then acc *= 0.8
                If user.SelTrait = Trait.复眼 Then acc *= 1.3
                If user.SelTrait = Trait.紧张 AndAlso selMove.MoveType = MoveType.物理 Then acc *= 0.8
                If user.Item = Item.放大镜 Then
                    acc *= 1.1
                ElseIf user.Item = Item.瞄准镜 AndAlso target.BattleState.moved AndAlso Not target.BattleState.swaped Then
                    acc *= 1.2
                End If
            End If
            If target.Item = Item.光粉 Then acc *= 0.9
        End If

        Return acc
    End Function


    Private Sub UseSkill(ByVal user As Pokemon, ByVal move As Move, ByVal targets As List(Of Pokemon), ByVal moveCall As Boolean)
        If Not UseSkillSuccessed(move, user, targets, moveCall) Then Return
        If targets.Count = 0 Then
            UseNoTargetSkill(user, move)
        ElseIf targets.Count = 1 Then
            If targets(0) Is user OrElse move.Target = MoveTarget.队友 Then
                UseNoTargetSkill(user, move)
            Else
                If ground.followPokemon IsNot Nothing AndAlso ground.followPokemon.HP <> 0 AndAlso ground.followPokemon IsNot user.Friend Then
                    targets(0) = ground.followPokemon
                End If
                If targets(0).HP = 0 AndAlso targets(0).Friend IsNot Nothing AndAlso targets(0).Friend IsNot user Then
                    targets(0) = targets(0).Friend
                End If
                If move.MoveType = MoveType.其他 Then
                    AssistTarget(move, user, targets(0), moveCall)
                Else
                    UseAttackSkill(move, user, targets(0), moveCall, False)
                End If
            End If
        Else
            Dim index As Integer
            Do
                If targets(index).HP = 0 Then
                    targets.RemoveAt(index)
                Else
                    index += 1
                End If
            Loop Until index > targets.Count - 1

            If targets.Count = 0 Then
                log.AddText(user.CustomName & "的技能没有对象！")
            Else
                For Each target As Pokemon In targets
                    If move.MoveType = MoveType.其他 Then
                        AssistTarget(move, user, target, moveCall)
                    Else
                        UseAttackSkill(move, user, target, moveCall, targets.Count > 1)
                    End If
                Next
            End If
        End If
        user.BattleState.previousMove = move.Identity

        If move.Effect = MoveEffect.临别礼物 Then
            user.GetHurt(user.HP)
        ElseIf (move.AddEff1 = MoveAdditionalEffect.自爆 OrElse move.AddEff2 = MoveAdditionalEffect.豁命攻击) _
            AndAlso user.HP <> 0 Then
            user.GetHurt(user.HP)
        ElseIf move.AddEff2 = MoveAdditionalEffect.震级 Then
            move.SetPower(0)
        ElseIf move.AddEff2 = MoveAdditionalEffect.轮唱 Then
            ground.SingARound = True
        ElseIf move.AddEff2 = MoveAdditionalEffect.古代之歌 AndAlso user.HP <> 0 Then
            user.ShapeShift(BattleData.GetPokemon(667))
        End If

        If user.Item = Item.生命玉 AndAlso user.HP <> 0 AndAlso user.attackSuccessed AndAlso user.SelTrait <> Trait.魔法守护 _
            AndAlso Not moveCall Then
            log.AddText(user.GetNameString() & "因为生命玉而受到了伤害！")
            user.GetHurt(user.MAXHP * 0.1)
        End If
    End Sub

    Private Sub UseNoTargetSkill(ByVal user As Pokemon, ByVal move As Move)
        If move.MoveType = MoveType.其他 Then
            If user.BattleState.snatched AndAlso move.Snatchable AndAlso user.BattleState.snatchBy.HP <> 0 Then
                log.AddText(user.GetNameString & "的技能效果被" & user.BattleState.snatchBy.GetNameString & "抢夺了！")
                user = user.BattleState.snatchBy
            End If

            If move.Target = MoveTarget.己场 Then
                AssistUserTeam(move, user.myTeam, user)
            ElseIf move.Target = MoveTarget.敌场 Then
                AssistTargetTeam(move, user.myTeam.opponentTeam)
            ElseIf move.Target = MoveTarget.全场 Then
                AssistAllCourt(move, user)
            ElseIf move.Target = MoveTarget.技能 Then
                AssistMoveCall(move, user)
            ElseIf move.Target = MoveTarget.自身 OrElse move.Target = MoveTarget.任选 Then
                AssistUser(move, user)
            ElseIf move.Target = MoveTarget.队友 Then
                If user.Friend IsNot Nothing Then AssistFriend(move, user.Friend)
            End If
        Else
            Dim target As Pokemon
            Dim damage As Integer
            Select Case move.AddEff2
                Case MoveAdditionalEffect.金属爆破
                    If user.turnHurt.Count > 0 Then
                        For i As Integer = user.turnHurt.Count - 1 To 0 Step -1
                            target = GetPokemonFromIndex(user.turnHurtBy(i))
                            damage = Convert.ToInt32(Math.Truncate(user.turnHurt(i) * 1.5))
                            NoTargetAttack(move, user, target, damage)
                            Exit For
                        Next
                    Else
                        Fail() : Return
                    End If
                Case MoveAdditionalEffect.镜返
                    If user.turnHurt.Count > 0 Then
                        For i As Integer = user.turnHurt.Count - 1 To 0 Step -1
                            If BattleData.GetMove(user.turnHurtMove(i)).MoveType = MoveType.特殊 Then
                                target = GetPokemonFromIndex(user.turnHurtBy(i))
                                damage = user.turnHurt(i) * 2
                                NoTargetAttack(move, user, target, damage)
                                Exit For
                            End If
                        Next
                    Else
                        Fail() : Return
                    End If
                Case MoveAdditionalEffect.反击
                    If user.turnHurt.Count > 0 Then
                        For i As Integer = user.turnHurt.Count - 1 To 0 Step -1
                            If BattleData.GetMove(user.turnHurtMove(i)).MoveType = MoveType.物理 Then
                                target = GetPokemonFromIndex(user.turnHurtBy(i))
                                damage = user.turnHurt(i) * 2
                                NoTargetAttack(move, user, target, damage)
                                Exit For
                            End If
                        Next
                    Else
                        Fail() : Return
                    End If
                Case MoveAdditionalEffect.克制
                    If Not user.BattleState.bide Then
                        user.BattleState.bide = True
                        user.BattleState.bideCounter = 0
                        user.BattleState.bideHurt = 0
                        user.BattleState.nextMove = move
                        Return
                    End If
                    If user.BattleState.bideHurt > 0 Then
                        target = GetPokemonFromIndex(user.BattleState.bideTarget)
                        damage = user.BattleState.bideHurt * 2
                        user.BattleState.bide = False
                        NoTargetAttack(move, user, target, damage)
                    Else
                        log.AddText(user.GetNameString & "的克制失败了！") : Return
                    End If
            End Select
        End If
    End Sub

    Private Sub NoTargetAttack(ByVal move As Move, ByVal user As Pokemon, ByVal target As Pokemon, ByVal damage As Integer)
        If Not CanAttackTarget(move, target, user) Then Return
        Dim selMoveType As PokemonType = GetAttackMoveType(move, user)

        If CalculateAttackTypeEffect(user, target, selMoveType) = 0 Then
            log.AddText("这对" & target.GetNameString & "没有效果...")
            Return
        End If
        If move.AddEff2 = MoveAdditionalEffect.克制 Then log.AddText(user.GetNameString & "将受到的伤害加倍奉还！")

        HurtTarget(user, target, damage, move, False, target.HP = target.MAXHP)
        EndAttack(user, move, target)
    End Sub

    Private Sub UseAttackSkill(ByVal selMove As Move, ByVal user As Pokemon, ByVal target As Pokemon, _
        ByVal moveCall As Boolean, ByVal multiTarget As Boolean)
        Select Case selMove.AddEff2
            Case MoveAdditionalEffect.保护取消
                If target.BattleState.protect Then
                    log.AddText(target.GetNameString & "的保护无效了！")
                    target.BattleState.protect = False
                    target.BattleState.protectCancel = True
                End If
            Case MoveAdditionalEffect.二回合后伤害
                target.BattleState.futureAtk = True

                Dim atkValue As Double = 1
                If selMove.MoveType = MoveType.物理 Then
                    atkValue = user.Attack
                Else
                    atkValue = user.SpAttack
                End If
                atkValue = Math.Truncate(atkValue)
                target.BattleState.futureAtkValue = (user.LV * 0.4 + 2) * atkValue * selMove.Power
                target.BattleState.futureAtkMoveType = selMove.MoveType

                If user.SelTrait = Trait.普通皮肤 Then
                    target.BattleState.futureAtkType = BattleData.GetTypeData("普通")
                Else
                    target.BattleState.futureAtkType = selMove.Type
                End If

                target.BattleState.futureAtkAcc = selMove.Acc
                target.BattleState.futureAtkDamageRevision = CalculateAttackerTypeRevision(1, target.BattleState.futureAtkType, user)
                Return
            Case MoveAdditionalEffect.愤怒
                user.BattleState.rage = True
            Case MoveAdditionalEffect.回音
                ground.Echo = True
        End Select
        Dim success As Boolean = False
        If CanAttackTarget(selMove, target, user) Then
            success = True
        End If

        If success Then
            success = BeginAttack(selMove, user, target, moveCall, multiTarget)
            If success Then
                Select Case selMove.AddEff2
                    Case MoveAdditionalEffect.自然恩惠
                        user.SetItem(Item.无)
                End Select
            End If
        End If
        If Not success AndAlso selMove.AddEff2 = MoveAdditionalEffect.攻击失败反弹一半 _
            AndAlso user.SelTrait <> Trait.魔法守护 Then
            log.AddText(user.CustomName & "受到了伤害的反弹！")
            user.GetHurt(user.HP \ 2)
            user.BattleState.successContinAttackCounter = 0
        End If
    End Sub


    Private Function BeginAttack(ByVal selMove As Move, ByVal user As Pokemon, ByVal target As Pokemon, _
        ByVal moveCall As Boolean, ByVal multiTarget As Boolean) As Boolean

        Dim selMoveType As PokemonType = GetAttackMoveType(selMove, user)

        '避雷针,吸水
        If target.Friend IsNot Nothing AndAlso target.Friend.HP <> 0 AndAlso _
            ((selMoveType.Name = "电" AndAlso target.Friend.SelTrait = Trait.避雷针 AndAlso target.SelTrait <> Trait.避雷针) OrElse _
             (selMoveType.Name = "水" AndAlso target.Friend.SelTrait = Trait.吸水 AndAlso target.SelTrait <> Trait.吸水)) Then
            target.Friend.RaiseTrait()
            Return False
        End If

        Dim typeEff As Double = CalculateAttackTypeEffect(user, target, selMoveType)
        If typeEff = 0 Then
            log.AddText("这对" & target.GetNameString & "没有效果...", Color.Red)
            Return False
        End If
        If typeEff <= 1 AndAlso target.SelTrait = Trait.属性之盾 AndAlso Not user.SelTrait = Trait.破格 AndAlso selMoveType.Name <> "无" Then
            log.AddText(target.GetNameString & "的属性之盾特性使得攻击无效！")
            Return False
        End If

        Dim acc As Double = CalculateAcc(user, target, selMove)
        If acc < 0 Then
            Fail()
            Return False
        End If
        If _random.NextDouble > acc Then
            log.AddText(user.CustomName & "的技能没有命中...")
            Return False
        Else
            Attack(user, target, selMove, typeEff, moveCall, multiTarget)
        End If
        Return True
    End Function

    Private Function CalculateDamage(ByVal selMove As Move, ByVal typeEff As Double, _
        ByVal user As Pokemon, ByVal target As Pokemon, _
        ByVal multiTarget As Boolean, Optional ByVal hit As Integer = 1) As Integer
        Dim selMoveType As PokemonType = GetAttackMoveType(selMove, user)

        Dim damage As Integer
        Select Case selMove.AddEff2
            Case MoveAdditionalEffect.必杀
                If target.SelTrait = Trait.坚硬 AndAlso user.SelTrait <> Trait.破格 Then
                    Fail() : Return 0
                Else
                    If target.BattleState.substituted Then
                        damage = target.BattleState.subHP
                    Else
                        damage = target.HP
                        log.AddText("这是一击必杀！")
                    End If
                End If
            Case MoveAdditionalEffect.对象HP二分
                If target.HP <> 1 Then
                    damage = target.HP \ 2
                Else
                    damage = 1
                End If
            Case MoveAdditionalEffect.莽撞
                If user.HP < target.HP Then
                    damage = target.HP - user.HP
                Else
                    Fail()
                    Return 0
                End If
            Case MoveAdditionalEffect.刀背击
                If damage >= target.HP AndAlso Not target.BattleState.substituted Then
                    damage = target.HP - 1
                End If
            Case MoveAdditionalEffect.固定伤害40
                damage = 40
            Case MoveAdditionalEffect.固定伤害20
                damage = 20
            Case MoveAdditionalEffect.自身等级伤害
                damage = user.LV
            Case MoveAdditionalEffect.对象等级伤害
                damage = target.LV
            Case MoveAdditionalEffect.幻象波浪
                damage = Convert.ToInt32(Math.Truncate(user.LV * 0.5 * _random.Next(1, 4)))
            Case MoveAdditionalEffect.豁命攻击
                damage = user.HP
            Case Else
                Select Case typeEff
                    Case Is < 1
                        log.AddText("这不是很有效...", Color.Red)
                    Case Is > 1
                        log.AddText("这非常有效！", Color.Red)
                End Select

                Dim attackerTypeEffect As Double = CalculateAttackerTypeRevision(typeEff, selMoveType, user)
                Dim defencerTypeEffect As Double = CalculateDefencerTypeRevision(typeEff, target, user.SelTrait = Trait.破格)

                typeEff *= attackerTypeEffect * defencerTypeEffect

                Dim weaEff As Double = selMoveType.WeatherEffects(ground.Weather)
                Dim other As Double = 1
                If Not user.SelTrait = Trait.破格 Then
                    If target.SelTrait = Trait.厚脂肪 AndAlso (selMoveType.Name = "冰" OrElse selMoveType.Name = "火") Then _
                        other *= 0.5
                    If target.SelTrait = Trait.耐热 AndAlso selMoveType.Name = "火" Then other *= 0.5
                    If target.SelTrait = Trait.干燥皮肤 AndAlso selMoveType.Name = "火" Then other *= 1.25
                End If

                Dim power As Double
                power = CalculateMovePower(user, target, selMove, selMoveType)
                If selMove.AddEff2 = MoveAdditionalEffect.三连踢 Then power *= hit
                If multiTarget AndAlso selMove.Target = MoveTarget.二体 Then power *= 0.75

                Dim CTValue As Integer = 1
                Dim ct As Boolean = False
                If power <> 0 Then
                    If selMove.AddEff2 = MoveAdditionalEffect.必定命中要害 Then
                        ct = True
                    Else
                        Dim ctOdds As Double
                        ctOdds = user.CTOdds(selMove.AddEff2 = MoveAdditionalEffect.易CT OrElse selMove.AddEff1 = MoveAdditionalEffect.易CT)

                        If Not target.myTeam.LuckyChant AndAlso Not _
                            ((target.SelTrait = Trait.化石盔甲 OrElse target.SelTrait = Trait.贝壳装甲) AndAlso user.SelTrait <> Trait.破格) _
                            AndAlso _random.NextDouble < ctOdds Then
                            ct = True
                        End If
                    End If
                    If ct Then
                        CTValue = 2
                        If user.SelTrait = Trait.狙击手 Then CTValue = 3
                        log.AddText("会心一击！", Color.Red)
                        If target.SelTrait = Trait.愤怒神经 AndAlso Not target.BattleState.substituted Then target.RaiseTrait()
                    End If
                End If

                Dim atkValue As Double = 1
                Dim defValue As Double = 1
                Dim ignoreDefenceLvUp As Boolean = selMove.AddEff2 = MoveAdditionalEffect.无视对象能力提升
                If selMove.MoveType = MoveType.物理 Then
                    If selMove.AddEff2 = MoveAdditionalEffect.诈骗 Then
                        atkValue = target.Attack(ct)
                    Else
                        atkValue = user.Attack(ct)
                    End If
                    defValue = target.Defence(target, ct OrElse ignoreDefenceLvUp)

                    If (user.SelTrait = Trait.花朵礼物 OrElse (user.Friend IsNot Nothing AndAlso _
                        user.Friend.SelTrait = Trait.花朵礼物 AndAlso user.Friend.HP <> 0)) _
                        AndAlso ground.Weather = Weather.Sunny Then
                        atkValue *= 1.5
                    End If
                    If user.SelTrait = Trait.好斗 AndAlso user.Gender <> PokemonGender.No AndAlso target.Gender <> PokemonGender.No Then
                        If user.Gender = target.Gender Then
                            atkValue *= 1.25
                        Else
                            atkValue *= 0.75
                        End If
                    End If
                Else
                    If selMove.AddEff2 = MoveAdditionalEffect.诈骗 Then
                        atkValue = target.SpAttack(ct)
                    Else
                        atkValue = user.SpAttack(ct)
                    End If
                    If selMove.AddEff2 = MoveAdditionalEffect.造成物理伤害 Then
                        defValue = target.Defence(user, ct OrElse ignoreDefenceLvUp)
                    Else
                        defValue = target.SpDefence(user, ct OrElse ignoreDefenceLvUp)
                    End If
                    If ground.Weather = Weather.SandStorm AndAlso target.HaveType("岩") Then defValue *= 1.5

                    If (target.SelTrait = Trait.花朵礼物 OrElse (target.Friend IsNot Nothing AndAlso _
                        target.Friend.SelTrait = Trait.花朵礼物 AndAlso target.Friend.HP <> 0)) _
                        AndAlso ground.Weather = Weather.Sunny Then
                        defValue *= 1.5
                    End If
                End If

                Dim screen As Double = 1
                If target.myTeam.Reflect AndAlso selMove.MoveType = MoveType.物理 AndAlso CTValue = 1 Then
                    If Mode = BattleMode.Single Then
                        screen = 0.5
                    Else
                        screen = 2 / 3
                    End If
                ElseIf target.myTeam.LightScreen AndAlso selMove.MoveType = MoveType.特殊 AndAlso CTValue = 1 Then
                    If Mode = BattleMode.Single Then
                        screen = 0.5
                    Else
                        screen = 2 / 3
                    End If
                End If
                If user.BattleState.assisted = True Then screen *= 1.5

                Select Case selMove.AddEff2
                    Case MoveAdditionalEffect.对象睡眠伤害加倍
                        If target.State = PokemonState.Sleep Then other *= 2
                    Case MoveAdditionalEffect.对象麻痹伤害加倍
                        If target.State = PokemonState.Paralysis Then other *= 2
                    Case MoveAdditionalEffect.对象中毒伤害加倍
                        If target.State = PokemonState.Toxin OrElse target.State = PokemonState.Poison Then
                            other *= 2
                        End If
                    Case MoveAdditionalEffect.对象异常状态伤害加倍
                        If target.State <> PokemonState.No Then
                            other *= 2
                        End If
                    Case MoveAdditionalEffect.礼物
                        Dim rand As Double = _random.NextDouble
                        If rand < 0.4 Then
                            power = 40
                        ElseIf rand < 0.7 Then
                            power = 80
                        ElseIf rand < 0.8 Then
                            power = 120
                        Else
                            target.HPRecover(target.HP * 0.25)
                            log.AddText(target.GetNameString & "恢复了HP！")
                            Return 0
                        End If
                    Case MoveAdditionalEffect.围攻
                        log.AddText(user.CustomName & "攻击了！")
                        For Each pm As Pokemon In user.myTeam.Pokemon
                            If pm IsNot Nothing AndAlso pm.HP <> 0 AndAlso pm IsNot user Then
                                atkValue += pm.Attack
                                log.AddText(pm.CustomName & "攻击了！")
                            End If
                        Next
                End Select

                Dim randValue As Double = _random.Next(85, 101) / 100
                atkValue = Math.Truncate(atkValue)
                defValue = Math.Truncate(defValue)
                power = Math.Truncate(power)
                damage = Convert.ToInt32(Math.Truncate(((user.LV * 0.4 + 2) * atkValue * power / defValue / 50 + 2) _
                    * typeEff * CTValue * screen * weaEff * other * randValue))
                If damage = 0 Then damage = 1
        End Select
        Return damage
    End Function

    Private Function CalculateMovePower(ByVal user As Pokemon, ByVal target As Pokemon, ByVal selMove As Move, _
        ByVal selMoveType As PokemonType) As Double
        Dim power As Double = Convert.ToDouble(selMove.Power)
        If power = 9999 Then power = 0
        Select Case selMove.AddEff2
            Case MoveAdditionalEffect.螺旋球
                power = 25 * target.Speed / user.Speed
                If power < 2 Then power = 2
                If power > 150 Then power = 150
            Case MoveAdditionalEffect.威力随对象体重变化
                Select Case target.Weight()
                    Case Is < 10
                        power = 20
                    Case Is < 25
                        power = 40
                    Case Is < 50
                        power = 60
                    Case Is < 100
                        power = 80
                    Case Is < 200
                        power = 100
                    Case Else
                        power = 120
                End Select
            Case MoveAdditionalEffect.责罚
                Dim targetLvCount As Integer = 0
                If target.AttackLV > 0 Then targetLvCount += target.AttackLV
                If target.SpAttackLV > 0 Then targetLvCount += target.SpAttackLV
                If target.DefenceLV > 0 Then targetLvCount += target.DefenceLV
                If target.SpDefenceLV > 0 Then targetLvCount += target.SpDefenceLV
                If target.SpeedLV > 0 Then targetLvCount += target.SpeedLV
                power = 60 + 20 * targetLvCount
                If power > 200 Then power = 200
            Case MoveAdditionalEffect.威力随剩余PP变化
                Select Case selMove.PP
                    Case Is >= 5
                        power = 40
                    Case 4
                        power = 50
                    Case 3
                        power = 50
                    Case 2
                        power = 80
                    Case 1
                        power = 200
                End Select
            Case MoveAdditionalEffect.伤害随对象HP变化
                power = 120 * target.HP / target.MAXHP
            Case MoveAdditionalEffect.掷物
                log.AddText("该技能具体效果目前不明...")
                Return 0
            Case MoveAdditionalEffect.挣扎
                Select Case user.HP / user.MAXHP
                    Case Is < 0.042
                        power = 200
                    Case Is < 0.105
                        power = 150
                    Case Is < 0.209
                        power = 100
                    Case Is < 0.355
                        power = 80
                    Case Is < 0.688
                        power = 40
                    Case Else
                        power = 20
                End Select
            Case MoveAdditionalEffect.威力随HP变化
                power = 150 * user.HP / user.MAXHP
            Case MoveAdditionalEffect.对象潜水伤害加倍
                If target.BattleState.dive Then power *= 2
            Case MoveAdditionalEffect.对象变小伤害加倍
                If target.BattleState.minimize Then power *= 2
            Case MoveAdditionalEffect.对象挖洞伤害加倍
                If target.BattleState.dig Then power *= 2
            Case MoveAdditionalEffect.对象空中伤害加倍
                If target.BattleState.fly Then power *= 2
            Case MoveAdditionalEffect.雷电
                If target.BattleState.fly Then power *= 2
            Case MoveAdditionalEffect.自身受到伤害威力加倍
                If user.turnHurt.Count > 0 Then power *= 2
            Case MoveAdditionalEffect.连续使用威力加倍
                If user.BattleState.previousMove <> selMove.Identity Then
                    user.BattleState.successContinAttackCounter = 0
                Else
                    user.BattleState.successContinAttackCounter += 1
                End If
                power *= Math.Pow(2, user.BattleState.successContinAttackCounter)

            Case MoveAdditionalEffect.较后攻击威力加倍
                If target.BattleState.moved AndAlso Not target.BattleState.swaped Then power *= 2
            Case MoveAdditionalEffect.对象受到伤害威力加倍
                If target.turnHurt.Count > 0 Then power *= 2
            Case MoveAdditionalEffect.对象HP低一半威力加倍
                If target.HP < target.MAXHP / 2 Then power *= 2
            Case MoveAdditionalEffect.自身异常威力加倍
                If user.State = PokemonState.Burn OrElse user.State = PokemonState.Paralysis OrElse user.State = PokemonState.Poison _
                    OrElse user.State = PokemonState.Toxin Then power *= 2
            Case MoveAdditionalEffect.天气球
                If ground.Weather <> Weather.No Then power *= 2

            Case MoveAdditionalEffect.呕吐
                power *= user.BattleState.stockpileCounter
                With user
                    .BattleState.stockpileCounter = 0
                    .DefLVDown(user.BattleState.stockpileDefCounter, True)
                    .BattleState.stockpileDefCounter = 0
                    .SDefLVDown(user.BattleState.stockpileSDefCounter, True)
                    .BattleState.stockpileSDefCounter = 0
                End With
            Case MoveAdditionalEffect.震级
                If target.BattleState.dig Then power *= 2
            Case MoveAdditionalEffect.滚动
                power = 30 * Math.Pow(2, user.BattleState.RollCounter - 1)
                If user.BattleState.defenseCurl Then power *= 2
            Case MoveAdditionalEffect.太阳光线
                If ground.Weather = Weather.Rainy OrElse ground.Weather = Weather.SandStorm OrElse _
                    ground.Weather = Weather.HailStorm Then
                    power *= 0.5
                End If
            Case MoveAdditionalEffect.追击
                If user.BattleState.pursuit Then power *= 2
            Case MoveAdditionalEffect.无道具威力加倍
                If user.Item(True) = Item.无 Then
                    power *= 2
                End If
            Case MoveAdditionalEffect.重量炸弹
                Select Case user.Weight / target.Weight
                    Case Is < 2
                        power = 40
                    Case Is <= 3
                        power = 60
                    Case Is <= 4
                        power = 80
                    Case Is <= 5
                        power = 100
                    Case Else
                        power = 120
                End Select
            Case MoveAdditionalEffect.电球
                Select Case user.Speed / target.Speed
                    Case Is < 2
                        power = 60
                    Case Is < 3
                        power = 80
                    Case Is < 4
                        power = 120
                    Case Else
                        power = 150
                End Select
            Case MoveAdditionalEffect.援助力量
                Dim userLvCounter As Integer = 0
                If user.AttackLV > 0 Then userLvCounter += user.AttackLV
                If user.SpAttackLV > 0 Then userLvCounter += user.SpAttackLV
                If user.DefenceLV > 0 Then userLvCounter += user.DefenceLV
                If user.SpDefenceLV > 0 Then userLvCounter += user.SpDefenceLV
                If user.SpeedLV > 0 Then userLvCounter += user.SpeedLV
                If user.BattleState.evasionLV > 0 Then userLvCounter += user.BattleState.evasionLV
                If user.BattleState.AccLV > 0 Then userLvCounter += user.BattleState.AccLV

                power = 20 + 20 * userLvCounter
            Case MoveAdditionalEffect.回音
                power = power * (1 + ground.EchoCounter)
            Case MoveAdditionalEffect.轮唱
                If ground.SingARound Then
                    power *= 2
                End If
            Case MoveAdditionalEffect.交织火焰
                If target.BattleState.crossThunder Then
                    power *= 2
                End If
            Case MoveAdditionalEffect.交织闪电
                If target.BattleState.crossFire Then
                    power *= 2
                End If
            Case MoveAdditionalEffect.报仇
                If user.myTeam.IsPokemonDiedLastTurn Then
                    power *= 2
                End If
            Case MoveAdditionalEffect.水之誓
                If user.BattleState.comboMoor OrElse user.BattleState.comboRainbow Then
                    power = 200
                End If
            Case MoveAdditionalEffect.草之誓
                If user.BattleState.comboMoor OrElse user.BattleState.comboFlameSea Then
                    power = 200
                End If
            Case MoveAdditionalEffect.火之誓
                If user.BattleState.comboFlameSea OrElse user.BattleState.comboRainbow Then
                    power = 200
                End If

        End Select

        If (user.SelTrait = Trait.引火 AndAlso user.BattleState.traitRaised AndAlso selMoveType.Name = "火") OrElse _
            (user.SelTrait = Trait.激流 AndAlso user.HP < user.MAXHP / 3 AndAlso selMoveType.Name = "水") OrElse _
            (user.SelTrait = Trait.发芽 AndAlso user.HP < user.MAXHP / 3 AndAlso selMoveType.Name = "草") OrElse _
            (user.SelTrait = Trait.虫族报告 AndAlso user.HP < user.MAXHP / 3 AndAlso selMoveType.Name = "虫") OrElse _
            (user.SelTrait = Trait.烈火 AndAlso user.HP < user.MAXHP / 3 AndAlso selMoveType.Name = "火") Then
            power *= 1.5
        ElseIf (user.SelTrait = Trait.铁拳 AndAlso selMove.Punch) OrElse _
            (user.SelTrait = Trait.舍身 AndAlso (selMove.AddEff2 = MoveAdditionalEffect.伤害反弹三分一 _
            OrElse selMove.AddEff2 = MoveAdditionalEffect.伤害反弹四分一 OrElse selMove.AddEff2 = MoveAdditionalEffect.双刃头槌 _
            OrElse selMove.AddEff2 = MoveAdditionalEffect.攻击失败反弹一半)) Then
            power *= 1.2
        ElseIf user.SelTrait = Trait.技师 AndAlso power <= 60 Then
            power *= 1.5
        End If
        If user.Item = Item.生命玉 Then
            power *= 1.3
        ElseIf user.Item = Item.节拍器 AndAlso user.BattleState.previousMove = selMove.Identity Then
            power *= 1.1
        End If
        'If selMove.AddEff1 = MoveAdditionalEffect.自爆 Then power *= 2
        If user.BattleState.meFirst Then power *= 1.5
        If ground.waterSport AndAlso selMoveType.Name = "火" Then power *= 0.5
        If ground.mudSport AndAlso selMoveType.Name = "电" Then power *= 0.5
        If user.BattleState.charge AndAlso selMoveType.Name = "电" Then power *= 2
        If ((user.Item = Item.黑色眼镜 OrElse user.Item = Item.恶属性石板) AndAlso selMoveType.Name = "恶") OrElse _
            ((user.Item = Item.黑带 OrElse user.Item = Item.格斗属性石板) AndAlso selMoveType.Name = "格斗") OrElse _
            ((user.Item = Item.银粉 OrElse user.Item = Item.虫属性石板) AndAlso selMoveType.Name = "虫") OrElse _
            ((user.Item = Item.弯曲汤匙 OrElse user.Item = Item.超能属性石板) AndAlso selMoveType.Name = "超能") OrElse _
            ((user.Item = Item.坚硬岩石 OrElse user.Item = Item.岩属性石板) AndAlso selMoveType.Name = "岩") OrElse _
            ((user.Item = Item.毒针 OrElse user.Item = Item.毒属性石板) AndAlso selMoveType.Name = "毒") OrElse _
            ((user.Item = Item.锐利鸟喙 OrElse user.Item = Item.飞行属性石板) AndAlso selMoveType.Name = "飞行") OrElse _
            ((user.Item = Item.不融冰 OrElse user.Item = Item.冰属性石板) AndAlso selMoveType.Name = "冰") OrElse _
            ((user.Item = Item.柔软沙子 OrElse user.Item = Item.地面属性石板) AndAlso selMoveType.Name = "地面") OrElse _
            ((user.Item = Item.咒符 OrElse user.Item = Item.鬼属性石板) AndAlso selMoveType.Name = "鬼") OrElse _
            ((user.Item = Item.磁铁 OrElse user.Item = Item.电属性石板) AndAlso selMoveType.Name = "电") OrElse _
            ((user.Item = Item.神秘水滴 OrElse user.Item = Item.水属性石板) AndAlso selMoveType.Name = "水") OrElse _
            ((user.Item = Item.奇迹种子 OrElse user.Item = Item.草属性石板) AndAlso selMoveType.Name = "草") OrElse _
            ((user.Item = Item.金属外套 OrElse user.Item = Item.钢属性石板) AndAlso selMoveType.Name = "钢") OrElse _
            ((user.Item = Item.木炭 OrElse user.Item = Item.火属性石板) AndAlso selMoveType.Name = "火") OrElse _
            ((user.Item = Item.龙牙 OrElse user.Item = Item.龙属性石板) AndAlso selMoveType.Name = "龙") OrElse _
            (user.Item = Item.丝巾 AndAlso selMoveType.Name = "普通") OrElse _
            (user.Item = Item.金刚石 AndAlso user.NO = 483 AndAlso (selMoveType.Name = "龙" OrElse selMoveType.Name = "钢")) OrElse _
            (user.Item = Item.白玉 AndAlso user.NO = 484 AndAlso (selMoveType.Name = "龙" OrElse selMoveType.Name = "水")) OrElse _
            (user.Item = Item.白金玉 AndAlso user.NO = 487 AndAlso (selMoveType.Name = "龙" OrElse selMoveType.Name = "鬼")) Then

            power *= 1.2
        ElseIf (user.Item = Item.力量头巾 AndAlso selMove.MoveType = MoveType.物理) OrElse _
            (user.Item = Item.智慧眼镜 AndAlso selMove.MoveType = MoveType.特殊) Then
            power *= 1.1
        End If
        Return power
    End Function


    Public Shared Function CalculateAttackTypeEffect(ByVal attacker As Pokemon, ByVal defencer As Pokemon, ByVal selMoveType As PokemonType) As Double
        Dim typeEff As Double
        Dim typeEff1 As Double = CalculateTypeEffect(attacker, defencer, selMoveType, defencer.Type1)
        If typeEff1 = 0 Then
            Return 0
        End If

        Dim typeEff2 As Double = 1
        If defencer.Type2 IsNot Nothing Then
            typeEff2 = CalculateTypeEffect(attacker, defencer, selMoveType, defencer.Type2)
            If typeEff2 = 0 Then
                Return 0
            End If
        End If
        typeEff = typeEff1 * typeEff2
        Return typeEff
    End Function

    Private Shared Function CalculateTypeEffect(ByVal attacker As Pokemon, ByVal defencer As Pokemon, _
                                         ByVal attackType As PokemonType, ByVal defenceType As PokemonType) As Double
        Dim typeEff1 As Double = attackType.TypeEffects(defenceType.Identity)
        If typeEff1 = 0 Then
            Dim ground As BattleGround = attacker.myTeam.ground
            If (ground.Gravity OrElse defencer.Item = Item.黑铁球 OrElse defencer.BattleState.ingrain) _
                AndAlso attackType.Name = "地面" Then
                typeEff1 = 1
            ElseIf defencer.BattleState.miracleEye AndAlso attackType.Name = "超能" Then
                typeEff1 = 1
            ElseIf (defencer.BattleState.foresight OrElse (attacker IsNot Nothing AndAlso attacker.SelTrait = Trait.气魄)) AndAlso _
                (attackType.Name = "普通" OrElse attackType.Name = "格斗") Then
                typeEff1 = 1
            End If
        End If
        Return typeEff1
    End Function

    Public Shared Function CalculateAttackerTypeRevision(ByVal typeEffect As Double, ByVal moveType As PokemonType, ByVal attacker As Pokemon) As Double
        Dim result As Double = 1

        If attacker.HaveType(moveType.Name) Then
            If attacker.SelTrait = Trait.适应力 Then
                result *= 2
            Else
                result *= 1.5
            End If
        End If

        If typeEffect < 1 Then
            If attacker.SelTrait = Trait.有色眼镜 Then result *= 2
        ElseIf typeEffect > 1 Then
            If attacker.Item = Item.达人腰带 Then result *= 1.2
        End If

        Return result
    End Function

    Public Shared Function CalculateDefencerTypeRevision(ByVal typeEffect As Double, ByVal defencer As Pokemon, ByVal moldBreak As Boolean) As Double
        Dim result As Double = 1

        If typeEffect > 1 Then
            If Not moldBreak Then
                If defencer.SelTrait = Trait.坚硬岩石 Then result *= 3 / 4
                If defencer.SelTrait = Trait.过滤器 Then result *= 3 / 4
            End If
        End If

        Return result
    End Function

    Public Shared Function GetAttackMoveType(ByVal move As Move, ByVal attacker As Pokemon) As PokemonType
        If attacker.SelTrait = Trait.普通皮肤 Then Return BattleData.GetTypeData("普通")
        If attacker.BattleState.comboMoor Then
            Return BattleData.GetTypeData("草")
        ElseIf attacker.BattleState.comboFlameSea Then
            Return BattleData.GetTypeData("火")
        ElseIf attacker.BattleState.comboRainbow Then
            Return BattleData.GetTypeData("水")
        End If
        Return move.Type
    End Function


    Private Sub Attack(ByVal user As Pokemon, ByVal target As Pokemon, ByVal selMove As Move, ByVal typeEff As Double, _
        ByVal moveCall As Boolean, ByVal multiTarget As Boolean)

        Dim selMoveType As PokemonType = GetAttackMoveType(selMove, user)

        Dim hit As Integer = 1
        Select Case selMove.AddEff2
            Case MoveAdditionalEffect.二连击
                hit = 2
            Case MoveAdditionalEffect.三连踢
                hit = 3
            Case MoveAdditionalEffect.二至五连击
                If user.SelTrait = Trait.技巧连接 Then hit = 5 : Exit Select
                Dim rand As Double = _random.NextDouble
                If rand <= 0.375 Then
                    hit = 2
                ElseIf rand <= 0.75 Then
                    hit = 3
                ElseIf rand <= 0.875 Then
                    hit = 4
                Else
                    hit = 5
                End If
        End Select

        If selMove.AddEff2 = MoveAdditionalEffect.破墙 Then
            target.myTeam.BreakWall()
        End If

        Dim damage As Integer
        Dim hpMax As Boolean = (target.HP = target.MAXHP)
        For i As Integer = 1 To hit
            If hit > 1 Then log.AddText(i & "连击！")
            damage = CalculateDamage(selMove, typeEff, user, target, multiTarget, i)
            If damage = 0 Then Return
            HurtTarget(user, target, damage, selMove, moveCall, hpMax)
            If target.HP = 0 OrElse user.HP = 0 Then Exit For
        Next

        If user.HP <> 0 Then
            Dim MoveAdditionalEffectOdds As Double = selMove.AddEffOdds
            If user.SelTrait = Trait.天之恩赐 OrElse user.myTeam.rainbow Then MoveAdditionalEffectOdds *= 2
            Select Case selMove.AddEff1
                Case MoveAdditionalEffect.一定几率自身物攻加一
                    If _random.NextDouble < MoveAdditionalEffectOdds Then user.AtkLVUp(1, False)
                Case MoveAdditionalEffect.一定几率自身特攻加一
                    If _random.NextDouble < MoveAdditionalEffectOdds Then user.SAtkLVUp(1, False)
                Case MoveAdditionalEffect.一定几率自身物防加一
                    If _random.NextDouble < MoveAdditionalEffectOdds Then user.DefLVUp(1, False)
                Case MoveAdditionalEffect.一定几率自身速度加一
                    If _random.NextDouble < MoveAdditionalEffectOdds Then user.SpeedLVUp(1, False)
                Case MoveAdditionalEffect.一定几率所有能力加一
                    If _random.NextDouble < MoveAdditionalEffectOdds Then
                        user.AtkLVUp(1, False)
                        user.DefLVUp(1, False)
                        user.SAtkLVUp(1, False)
                        user.SDefLVUp(1, False)
                        user.SpeedLVUp(1, False)
                    End If
                Case MoveAdditionalEffect.自身特攻减二
                    user.SAtkLVDown(2, True)
                Case MoveAdditionalEffect.自身物理攻防减一
                    user.AtkLVDown(1, True)
                    user.DefLVDown(1, True)
                Case MoveAdditionalEffect.自身速度减一
                    user.SpeedLVDown(1, True, False)
                Case MoveAdditionalEffect.自身双防减一
                    user.DefLVDown(1, True)
                    user.SDefLVDown(1, True)
                Case MoveAdditionalEffect.自身双防速度减一
                    user.DefLVDown(1, True)
                    user.SDefLVDown(1, True)
                    user.SpeedLVDown(1, True, False)

                Case MoveAdditionalEffect.次回合不能行动
                    user.BattleState.nextTurnCantMove = True
                Case MoveAdditionalEffect.吸收一半伤害
                    If user.HP <> 0 Then
                        If target.SelTrait = Trait.淤泥 AndAlso selMove.Name <> "食梦" _
                            AndAlso user.SelTrait <> Trait.魔法守护 Then
                            log.AddText(target.GetNameString & "的淤泥特性使" & user.CustomName & "受到了伤害！")
                            user.GetHurt(damage * 0.5)
                        ElseIf user.Item = Item.大树根 AndAlso selMove.Name <> "食梦" Then
                            user.HPRecover(damage / 3 * 2)
                            log.AddText(user.CustomName & "吸收了对手的伤害！")
                        Else
                            user.HPRecover(damage * 0.5)
                            log.AddText(user.CustomName & "吸收了对手的伤害！")
                        End If
                    End If
            End Select
            Select Case selMove.AddEff2
                Case MoveAdditionalEffect.伤害反弹三分一
                    If user.SelTrait <> Trait.石头脑袋 AndAlso user.SelTrait <> Trait.魔法守护 AndAlso damage <> 0 _
                        Then log.AddText(user.CustomName & "受到了伤害反弹！") : user.GetHurt(damage / 3)
                Case MoveAdditionalEffect.伤害反弹四分一
                    If user.SelTrait <> Trait.石头脑袋 AndAlso user.SelTrait <> Trait.魔法守护 AndAlso damage <> 0 _
                        Then log.AddText(user.CustomName & "受到了伤害反弹！") : user.GetHurt(damage * 0.25)
                Case MoveAdditionalEffect.双刃头槌
                    If user.SelTrait <> Trait.石头脑袋 AndAlso user.SelTrait <> Trait.魔法守护 AndAlso damage <> 0 _
                        Then log.AddText(user.CustomName & "受到了伤害反弹！") : user.GetHurt(damage * 0.5)
                Case MoveAdditionalEffect.绝境反击
                    log.AddText(user.CustomName & "受到了伤害反弹！") : user.GetHurt(user.MAXHP * 0.25)
                Case MoveAdditionalEffect.蜻蜓返
                    If user.myTeam.CanChange(user) AndAlso user.HP <> 0 Then
                        user.BattleState.uTurn = True
                        user.BattleState.pass = True
                    End If
                Case MoveAdditionalEffect.滚动
                    user.BattleState.Roll(selMove)
                Case MoveAdditionalEffect.二至三回合攻击后混乱
                    If Not user.BattleState.CrazyAttack Then
                        user.BattleState.BeginCrazyAttack(_random, selMove)
                    End If
                Case MoveAdditionalEffect.吵闹
                    If Not user.BattleState.CrazyAttack Then
                        user.BattleState.BeginUproar(_random, selMove)
                    End If
            End Select
        End If

        If selMove.AddEff1 = MoveAdditionalEffect.高旋 Then
            If user.BattleState.constraint AndAlso user.BattleState.constraintTurn <> -1 Then
                user.BattleState.constraint = False
                log.AddText(user.CustomName & "挣脱了束缚！")
            End If
            user.BattleState.seed = False
            user.BattleState.seedTarget = PokemonIndex.WrongInput
            user.myTeam.spikesCounter = 0
            user.myTeam.poisonSpikesCounter = 0
            user.myTeam.hideRockCounter = 0
            log.AddText(user.myTeam.PlayerName & "队伍周围的障碍被清除了！")
        End If
        If user.BattleState.comboMoor Then
            target.myTeam.MakeMoor()
        ElseIf user.BattleState.comboRainbow Then
            user.myTeam.MakeRainbow()
        ElseIf user.BattleState.comboFlameSea Then
            target.myTeam.MakeFlameSea()
        End If

        Select Case selMove.AddEff2
            Case MoveAdditionalEffect.溅射攻击
                If target.Friend IsNot Nothing Then
                    target.Friend.GetHurt(target.Friend.MAXHP / 16)
                    log.AddText(target.Friend.CustomName & "受到了溅射伤害！")
                End If
            Case MoveAdditionalEffect.交织火焰
                If target.HP <> 0 Then
                    target.BattleState.crossFire = True
                End If
            Case MoveAdditionalEffect.交织闪电
                If target.HP <> 0 Then
                    target.BattleState.crossThunder = True
                End If
        End Select
        EndAttack(user, selMove, target)
    End Sub

    Private Sub EndAttack(ByVal user As Pokemon, ByVal selMove As Move, ByVal target As Pokemon)
        If target.HP = 0 Then
            If target.BattleState.destinyBond Then
                log.AddText(user.CustomName & "与" & target.CustomName & "同行")
                user.GetHurt(user.HP)
            ElseIf target.BattleState.grudge Then
                For i As Integer = 1 To 4
                    If user.SelMove(i) Is selMove Then
                        selMove.SetPP(0)
                        log.AddText(target.CustomName & "的怨念使" & user.CustomName & "的" & selMove.Name & "技能PP变成了0")
                    End If
                Next
            End If
        End If
    End Sub

    Private Sub HurtTarget(ByVal user As Pokemon, ByVal target As Pokemon, ByRef damage As Integer, _
        ByVal selMove As Move, ByVal moveCall As Boolean, ByVal hpMax As Boolean)

        Dim selMoveType As PokemonType = GetAttackMoveType(selMove, user)

        If target.BattleState.substituted Then
            If damage > target.BattleState.subHP Then damage = target.BattleState.subHP
            log.AddText(target.GetNameString & "的替身代替它受到攻击！")
            target.SubGetHurt(Convert.ToInt32(damage))

            If selMove.AddEff1 = MoveAdditionalEffect.自爆 OrElse selMove.AddEff2 = MoveAdditionalEffect.豁命攻击 Then
                user.GetHurt(user.HP)
            End If
            If user.Item = Item.空贝铃 AndAlso user.HP <> user.MAXHP AndAlso user.HP <> 0 Then _
                user.HPRecover(damage * 0.125) : log.AddText(user.CustomName & "的空贝铃使它回复了HP！")
            If Not target.BattleState.substituted Then
                log.AddText(target.GetNameString & "的替身被打消了！")
                If selMove.AddEff1 = MoveAdditionalEffect.暂时束缚 Then
                    target.BattleState.constraint = True
                    If user.Item = Item.镰刀爪 Then
                        target.BattleState.constraintTurn = 8
                    Else
                        target.BattleState.constraintTurn = _random.Next(5, 7)
                    End If
                    target.BattleState.constraintCounter = 0
                    target.BattleState.constraintBy = GetPokemonIndex(user)
                    log.AddText(target.CustomName & "不能逃走了！")
                End If
            End If
        Else
            If target.BattleState.endure AndAlso damage >= target.HP Then
                damage = target.HP - 1
            End If
            If target.Item = Item.气息头巾 AndAlso damage >= target.HP _
                AndAlso _random.NextDouble < 0.1 Then
                damage = target.HP - 1
                log.AddText(target.GetNameString & "的气息头巾使它留下了1HP！")
            ElseIf (target.Item = Item.气息腰带 OrElse (target.SelTrait = Trait.坚硬 AndAlso user.SelTrait <> Trait.破格)) _
                 AndAlso hpMax AndAlso damage >= target.HP Then
                damage = target.HP - 1
                log.AddText(target.GetNameString() & "的气息腰带使它留下了1HP！")
                target.UseItem()
            ElseIf (target.Item = Item.果实36 AndAlso selMoveType.Name = "火") OrElse _
                (target.Item = Item.果实37 AndAlso selMoveType.Name = "水") OrElse _
                (target.Item = Item.果实38 AndAlso selMoveType.Name = "电") OrElse _
                (target.Item = Item.果实39 AndAlso selMoveType.Name = "草") OrElse _
                (target.Item = Item.果实40 AndAlso selMoveType.Name = "冰") OrElse _
                (target.Item = Item.果实41 AndAlso selMoveType.Name = "格斗") OrElse _
                (target.Item = Item.果实42 AndAlso selMoveType.Name = "毒") OrElse _
                (target.Item = Item.果实43 AndAlso selMoveType.Name = "地面") OrElse _
                (target.Item = Item.果实44 AndAlso selMoveType.Name = "飞行") OrElse _
                (target.Item = Item.果实45 AndAlso selMoveType.Name = "超能") OrElse _
                (target.Item = Item.果实46 AndAlso selMoveType.Name = "虫") OrElse _
                (target.Item = Item.果实47 AndAlso selMoveType.Name = "岩") OrElse _
                (target.Item = Item.果实48 AndAlso selMoveType.Name = "鬼") OrElse _
                (target.Item = Item.果实49 AndAlso selMoveType.Name = "龙") OrElse _
                (target.Item = Item.果实50 AndAlso selMoveType.Name = "恶") OrElse _
                (target.Item = Item.果实51 AndAlso selMoveType.Name = "钢") OrElse _
                (target.Item = Item.果实52 AndAlso selMoveType.Name = "普通") Then
                Dim typeEff As Double = CalculateAttackTypeEffect(user, target, selMoveType)
                If typeEff >= 2 Then
                    damage = Convert.ToInt32(damage * 0.5)
                    target.RaiseItem()
                End If
            End If
            If damage > target.HP Then damage = target.HP
            log.AddLog("(对" & target.GetNameString & "造成" & damage & "伤害)")
            target.GetHurt(damage)
            target.turnHurt.Add(damage)
            target.turnHurtMove.Add(selMove.Identity)
            target.turnHurtBy.Add(GetPokemonIndex(user))
            If target.BattleState.bide Then
                target.BattleState.bideHurt += damage
                target.BattleState.bideTarget = GetPokemonIndex(user)
            End If
            user.attackSuccessed = True

            If selMove.AddEff1 = MoveAdditionalEffect.自爆 OrElse selMove.AddEff2 = MoveAdditionalEffect.豁命攻击 Then
                user.GetHurt(user.HP)
            End If
            If user.Item = Item.空贝铃 AndAlso user.HP <> user.MAXHP AndAlso user.HP <> 0 Then _
                user.HPRecover(damage * 0.125) : log.AddText(user.CustomName & "的空贝铃使它回复了HP！")

            If target.SelTrait = Trait.变色 AndAlso target.HP <> 0 AndAlso target.Type1.Name <> selMoveType.Name Then _
                target.Type1 = selMoveType : log.AddText(target.CustomName & "的属性变成了" & selMoveType.Name & "！")

            If user.HP <> 0 AndAlso selMove.Contact Then
                Select Case target.SelTrait
                    Case Trait.引爆
                        If target.HP = 0 AndAlso Not ground.湿气 AndAlso user.SelTrait <> Trait.魔法守护 Then
                            log.AddText(target.CustomName & "的引爆特性使" & user.CustomName & "受到了伤害！")
                            user.GetHurt(user.MAXHP * 0.25)
                        End If
                    Case Trait.鲨鱼皮
                        If user.SelTrait <> Trait.魔法守护 Then _
                            log.AddText(target.CustomName & "的鲨鱼皮特性使" & user.CustomName & "受到了伤害！") _
                            : user.GetHurt(user.MAXHP * 0.125)
                    Case Trait.静电
                        If _random.NextDouble < 0.3 AndAlso user.SetState(PokemonState.Paralysis, , False) Then
                            log.AddText(target.CustomName & "的静电特性使" & user.CustomName & "麻痹了！")
                        End If
                    Case Trait.毒针
                        If _random.NextDouble < 0.3 AndAlso user.SetState(PokemonState.Poison, , False) Then
                            log.AddText(target.CustomName & "的毒针特性使" & user.CustomName & "中毒了！")
                        End If
                    Case Trait.孢子
                        Dim rand As Double = _random.Next
                        If rand < 0.1 Then
                            If user.SetState(PokemonState.Poison, , False) Then _
                                log.AddText(target.CustomName & "的孢子特性使" & user.CustomName & "中毒了！")
                        ElseIf rand < 0.2 Then
                            If user.SetState(PokemonState.Paralysis, , False) Then _
                                log.AddText(target.CustomName & "的孢子特性使" & user.CustomName & "麻痹了！")
                        ElseIf rand < 0.3 Then
                            If user.SetState(PokemonState.Sleep, , False) Then
                                log.AddText(target.CustomName & "的孢子特性使" & user.CustomName & "睡着了！")
                            End If
                        End If
                    Case Trait.火焰之身
                        If _random.NextDouble < 0.3 AndAlso user.SetState(PokemonState.Burn, , False) Then
                            log.AddText(target.CustomName & "的火焰之身特性使" & user.CustomName & "烧伤了！")
                        End If
                    Case Trait.魅惑之身
                        If _random.NextDouble < 0.3 Then user.captivate(target, True)
                End Select
                If target.Item = Item.附针 AndAlso user.Item = Item.无 Then
                    user.Item = Item.附针
                    target.SetItem(Item.无)
                    log.AddText(target.CustomName & "的附针转移到了" & user.CustomName & "身上！")
                End If
            End If

            If target.HP <> 0 Then
                If (user.Item = Item.王者证明 OrElse user.Item = Item.尖锐牙) _
                    AndAlso selMove.KingRock AndAlso _random.NextDouble < 0.1 _
                    Then target.terrify(user)

                If target.Item = Item.果实60 Then
                    Dim typeEff As Double = CalculateAttackTypeEffect(user, target, selMoveType)
                    If typeEff >= 2 Then target.RaiseItem()
                End If
                If target.BattleState.rage Then
                    target.AtkLVUp(1, False)
                End If
            End If
            If target.Item = Item.果实63 AndAlso selMove.MoveType = MoveType.物理 Then
                log.AddText(target.CustomName & "的63号果实使" & user.CustomName & "受到了伤害！")
                user.GetHurt(damage * 0.125)
                target.UseItem()
            ElseIf target.Item = Item.果实64 AndAlso selMove.MoveType = MoveType.特殊 Then
                log.AddText(target.CustomName & "的64号果实使" & user.CustomName & "受到了伤害！")
                user.GetHurt(damage * 0.125)
                target.UseItem()
            End If
            CalculateMoveEffect(user, target, selMove, moveCall)
        End If
    End Sub

    Private Sub CalculateMoveEffect(ByVal user As Pokemon, ByVal target As Pokemon, ByVal selMove As Move, _
        ByVal moveCall As Boolean)
        Dim selMoveType As PokemonType = GetAttackMoveType(selMove, user)
        If target.HP = 0 Then
            Return
        End If
        Dim moveAdditionalEffectOdds As Double = selMove.AddEffOdds
        If user.SelTrait = Trait.天之恩赐 Then moveAdditionalEffectOdds *= 2

        If Not (target.SelTrait = Trait.鳞粉 AndAlso user.SelTrait <> Trait.破格) Then
            Select Case selMove.AddEff1
                Case MoveAdditionalEffect.一定几率烧伤
                    If _random.NextDouble < moveAdditionalEffectOdds Then
                        target.SetState(PokemonState.Burn, user)
                    End If
                Case MoveAdditionalEffect.一定几率麻痹
                    If _random.NextDouble < moveAdditionalEffectOdds Then
                        target.SetState(PokemonState.Paralysis, user)
                    End If
                Case MoveAdditionalEffect.一定几率冻结
                    If _random.NextDouble < moveAdditionalEffectOdds Then
                        target.SetState(PokemonState.Freeze, user)
                    End If
                Case MoveAdditionalEffect.一定几率中毒
                    If _random.NextDouble < moveAdditionalEffectOdds Then
                        target.SetState(PokemonState.Poison, user)
                    End If
                Case MoveAdditionalEffect.一定几率剧毒
                    If _random.NextDouble < moveAdditionalEffectOdds Then
                        target.SetState(PokemonState.Toxin, user)
                    End If
                Case MoveAdditionalEffect.一定几率睡眠
                    If _random.NextDouble < moveAdditionalEffectOdds Then
                        target.SetState(PokemonState.Sleep, user)
                    End If
                Case MoveAdditionalEffect.一定几率烧伤或害怕
                    If _random.NextDouble < moveAdditionalEffectOdds Then
                        target.SetState(PokemonState.Burn, user)
                    End If
                    If _random.NextDouble < moveAdditionalEffectOdds Then target.terrify(user)
                Case MoveAdditionalEffect.一定几率麻痹或害怕
                    If _random.NextDouble < moveAdditionalEffectOdds Then
                        target.SetState(PokemonState.Paralysis, user)
                    End If
                    If _random.NextDouble < moveAdditionalEffectOdds Then target.terrify(user)

                Case MoveAdditionalEffect.一定几率冻结或害怕
                    If _random.NextDouble < moveAdditionalEffectOdds Then
                        target.SetState(PokemonState.Freeze, user)
                    End If
                    If _random.NextDouble < moveAdditionalEffectOdds Then target.terrify(user)

                Case MoveAdditionalEffect.三角攻击
                    Dim rand As Double = _random.NextDouble
                    If rand < moveAdditionalEffectOdds / 3 Then
                        target.SetState(PokemonState.Burn, user)
                    ElseIf rand < moveAdditionalEffectOdds * 2 / 3 Then
                        target.SetState(PokemonState.Paralysis, user)
                    ElseIf rand < moveAdditionalEffectOdds Then
                        target.SetState(PokemonState.Freeze, user)
                    End If
                Case MoveAdditionalEffect.一定几率混乱
                    If _random.NextDouble < moveAdditionalEffectOdds Then
                        target.confuse(moveCall, user)
                    End If
                Case MoveAdditionalEffect.一定几率害怕
                    If _random.NextDouble < moveAdditionalEffectOdds Then target.terrify(user)
                Case MoveAdditionalEffect.一定几率速度减一
                    If _random.NextDouble < moveAdditionalEffectOdds Then
                        target.SpeedLVDown(1, user)
                    End If
                Case MoveAdditionalEffect.一定几率特防减一
                    If _random.NextDouble < moveAdditionalEffectOdds Then
                        target.SDefLVDown(1, user)
                    End If
                Case MoveAdditionalEffect.一定几率特防减二
                    If _random.NextDouble < moveAdditionalEffectOdds Then
                        target.SDefLVDown(2, user)
                    End If
                Case MoveAdditionalEffect.一定几率物攻减一
                    If _random.NextDouble < moveAdditionalEffectOdds Then
                        target.AtkLVDown(1, user)
                    End If
                Case MoveAdditionalEffect.一定几率物防减一
                    If _random.NextDouble < moveAdditionalEffectOdds Then
                        target.DefLVDown(1, user)
                    End If
                Case MoveAdditionalEffect.一定几率命中减一
                    If _random.NextDouble < moveAdditionalEffectOdds Then
                        target.AccLVDown(1, user, False)
                    End If
                Case MoveAdditionalEffect.一定几率特攻减一
                    If _random.NextDouble < moveAdditionalEffectOdds Then
                        target.SAtkLVDown(1, user, False)
                    End If
                Case MoveAdditionalEffect.秘密之力
                    If _random.NextDouble < moveAdditionalEffectOdds Then
                        Select Case ground.Terrain
                            Case BattleTerrain.Stadium
                                target.SetState(PokemonState.Paralysis, user)
                            Case BattleTerrain.Grass
                                target.SetState(PokemonState.Sleep, user)
                            Case BattleTerrain.Flat
                                target.AccLVDown(1, user, False)
                            Case BattleTerrain.Sand
                                target.AccLVDown(1, user, False)
                            Case BattleTerrain.Mountain
                                target.terrify(user)
                            Case BattleTerrain.Cave
                                target.terrify(user)
                            Case BattleTerrain.Water
                                target.AtkLVDown(1, user)
                            Case BattleTerrain.SnowField
                                target.SetState(PokemonState.Freeze, user)
                        End Select
                    End If
            End Select
            Select Case selMove.AddEff2
                Case MoveAdditionalEffect.雷电
                    If _random.NextDouble < 0.3 Then
                        target.SetState(PokemonState.Paralysis, user)
                    End If
                Case MoveAdditionalEffect.跳起后攻击
                    If _random.NextDouble < moveAdditionalEffectOdds Then
                        target.SetState(PokemonState.Paralysis, user)
                    End If
                Case MoveAdditionalEffect.暴风雪
                    If _random.NextDouble < 0.1 Then
                        target.SetState(PokemonState.Freeze, user)
                    End If
                Case MoveAdditionalEffect.下马威
                    target.terrify(user)
            End Select
            '恶臭
            If selMove.Contact AndAlso user.SelTrait = Trait.恶臭 AndAlso target.SelTrait <> Trait.破格 Then
                If _random.NextDouble < 0.1 Then target.terrify(user)
            End If
        End If

        If target.State = PokemonState.Freeze AndAlso selMoveType.Name = "火" Then _
            target.State = PokemonState.No : log.AddText(target.CustomName & "解冻了！")

        Select Case selMove.AddEff1
            Case MoveAdditionalEffect.夺取果实
                If Item.果实1 <= target.Item(True) AndAlso target.Item(True) <= Item.果实64 _
                    AndAlso (target.SelTrait <> Trait.粘着 OrElse user.SelTrait = Trait.破格) Then
                    log.AddText(user.CustomName & "夺取了" & target.CustomName & "的果实！")
                    user.SetItem(target.Item(True))
                    user.RaiseItem()
                    target.SetItem(Item.无)
                End If
            Case MoveAdditionalEffect.燃尽果实
                If Item.果实1 <= target.Item(True) AndAlso target.Item(True) <= Item.果实64 Then
                    target.SetItem(Item.无)
                    log.AddText(String.Format("{0}的果实被烧掉了", target.CustomName))
                End If

            Case MoveAdditionalEffect.夺取道具
                If user.Item(True) = Item.无 AndAlso target.Item(True) <> Item.无 AndAlso _
                    (target.SelTrait <> Trait.粘着 OrElse user.SelTrait = Trait.破格) Then
                    log.AddText(user.CustomName & "夺取了" & target.CustomName & "的道具！")
                    user.SetItem(target.Item(True))
                    target.SetItem(Item.无)
                End If
            Case MoveAdditionalEffect.打落
                If target.Item(True) <> Item.无 AndAlso _
                    (target.SelTrait <> Trait.粘着 OrElse user.SelTrait = Trait.破格) Then
                    log.AddText(target.CustomName & "的" & target.Item(True).ToString & "被打落了！")
                    target.SetItem(Item.无)
                End If
            Case MoveAdditionalEffect.暂时束缚
                If Not target.BattleState.constraint Then
                    target.BattleState.constraint = True
                    If user.Item = Item.镰刀爪 Then
                        target.BattleState.constraintTurn = 5
                    Else
                        target.BattleState.constraintTurn = _random.Next(2, 6)
                    End If
                    target.BattleState.constraintCounter = 0
                    target.BattleState.constraintBy = GetPokemonIndex(user)
                    log.AddText(target.CustomName & "不能逃走了！")
                End If
            Case MoveAdditionalEffect.净化之雾
                target.Haze()
                log.AddText(target.CustomName & "的能力变化无效了！")
            Case MoveAdditionalEffect.强制下场
                target.MakedToSwap(user)
        End Select
        Select Case selMove.AddEff2
            Case MoveAdditionalEffect.对象睡眠伤害加倍
                If target.State = PokemonState.Sleep Then target.State = PokemonState.No : log.AddText(target.CustomName & "醒过来了！")
            Case MoveAdditionalEffect.对象麻痹伤害加倍
                If target.State = PokemonState.Paralysis Then target.State = PokemonState.No : log.AddText(target.CustomName & "解除了麻痹！")
        End Select
    End Sub


    Private Sub AssistTarget(ByVal selMove As Move, ByVal user As Pokemon, ByVal target As Pokemon, _
         ByVal moveCall As Boolean)
        If Not CanAttackTarget(selMove, target, user) Then Return
        Dim moveType As PokemonType = selMove.Type
        If user.SelTrait = Trait.普通皮肤 Then moveType = BattleData.GetTypeData("普通")

        If selMove.Name = "电磁波" AndAlso CalculateAttackTypeEffect(user, target, moveType) = 0 Then
            log.AddText("这对" & target.GetNameString & "没有效果...", Color.Red)
            Return
        End If
        If target.SelTrait = Trait.隔音 AndAlso selMove.Sound _
            Then log.AddText(target.GetNameString & "的隔音特性使得技能无效！") : Return

        If target.BattleState.substituted AndAlso selMove.Substitute Then
            log.AddText(target.GetNameString & "的替身使" & selMove.Name & "无效了！")
            Return
        End If
        Dim acc As Double = CalculateAcc(user, target, selMove)
        If _random.NextDouble > acc Then
            log.AddText(user.CustomName & "的技能没有命中...")
            Return
        End If
        If target.BattleState.magicCoat AndAlso selMove.Effect <= MoveEffect.种子 Then
            log.AddText(user.CustomName & "的技能效果被反弹回来！")
            target = user
        End If
        If selMove.Snatchable AndAlso user.BattleState.snatched AndAlso user.BattleState.snatchBy.HP <> 0 Then
            log.AddText(user.GetNameString & "的技能效果被" & user.BattleState.snatchBy.GetNameString & "抢夺了！")
            If selMove.Effect = MoveEffect.自我暗示 Then
                target = user
                user = user.BattleState.snatchBy
            ElseIf selMove.Effect = MoveEffect.点穴 Then
                target = user.BattleState.snatchBy
            End If
        End If
        Select Case selMove.Effect
            Case MoveEffect.中毒
                If Not target.SetState(PokemonState.Poison, user, , , , , True) Then Fail()
            Case MoveEffect.麻痹
                If Not target.SetState(PokemonState.Paralysis, user, , , , , True) Then Fail()
            Case MoveEffect.烧伤
                If Not target.SetState(PokemonState.Burn, user, , , , , True) Then Fail()
            Case MoveEffect.冻结
                If Not target.SetState(PokemonState.Freeze, user, , , , , True) Then Fail()
            Case MoveEffect.睡眠
                If Not target.SetState(PokemonState.Sleep, user, , , , , True) Then Fail()
            Case MoveEffect.剧毒
                If Not target.SetState(PokemonState.Toxin, user, , , , , True) Then Fail()
            Case MoveEffect.混乱
                If Not target.confuse(moveCall, user) Then Fail()
            Case MoveEffect.种子
                target.BeSeeded(GetPokemonIndex(user))

            Case MoveEffect.着迷
                If Not target.captivate(user) Then Fail()
            Case MoveEffect.束缚
                target.BattleState.constraint = True
                target.BattleState.constraintTurn = -1
                target.BattleState.constraintBy = GetPokemonIndex(user)
                log.AddText(target.GetNameString & "不能逃走了！")
            Case MoveEffect.诅咒
                If target.BattleState.curse = False Then
                    target.BattleState.curse = True
                    log.AddText(target.GetNameString & "被诅咒了！")
                    user.GetHurt(user.MAXHP / 2)
                Else
                    Fail()
                End If
            Case MoveEffect.噩梦
                If target.State = PokemonState.Sleep Then
                    If target.BattleState.nightmare = False Then
                        target.BattleState.nightmare = True
                        log.AddText(target.GetNameString & "被噩梦困扰！")
                    Else
                        log.AddText(target.GetNameString & "已经处于噩梦状态")
                    End If
                Else
                    Fail()
                End If
            Case MoveEffect.鼓掌
                If Not target.Encore() Then
                    Fail()
                End If

            Case MoveEffect.挑拨
                target.Taunt()

            Case MoveEffect.锁定
                target.BattleState.lockOn = True
                target.BattleState.lockOnCounter = 2
                log.AddText(target.GetNameString & "无法回避！")
            Case MoveEffect.精神控制
                If Not target.LastMoveIsValid OrElse target.BattleState.disableIndex <> 0 Then
                    Fail()
                Else
                    target.BattleState.disableIndex = target.lastMoveIndex
                    target.BattleState.disableTurn = _random.Next(2, 6)
                    target.BattleState.disableCounter = 0
                    log.AddText(target.GetNameString & "的" & target.SelMove(target.BattleState.disableIndex).Name & "技能不能使用了！")
                End If
            Case MoveEffect.强制交换
                If Not target.MakedToSwap(user) Then Fail()
            Case MoveEffect.哈欠
                If Not target.BeYawned Then Fail()
            Case MoveEffect.临别礼物
                target.AtkLVDown(2, user)
                target.SAtkLVDown(2, user)
            Case MoveEffect.念力挪移
                If user.State <> PokemonState.No AndAlso target.SetState(user.State, user, False) Then
                    user.State = PokemonState.No
                    log.AddText(user.GetNameString & "恢复了状态！")
                Else
                    Fail()
                End If
            Case MoveEffect.恨
                If target.LastMoveIsValid Then
                    Dim targetMove As Move = target.LastMove
                    If targetMove.PP > 4 Then
                        targetMove.SetPP(targetMove.PP - 4)
                    Else
                        targetMove.SetPP(0)
                    End If
                    log.AddText(target.GetNameString & "的" & targetMove.Name & "技能PP减少了")
                End If
            Case MoveEffect.分担痛苦
                Dim hp As Integer = user.HP + target.HP
                user.SetHP(hp \ 2)
                target.SetHP(hp \ 2)
                log.AddText("双方的HP平分了")
            Case MoveEffect.寻衅
                If target.BattleState.torment Then
                    Fail()
                Else
                    target.BattleState.torment = True
                    log.AddText(target.GetNameString & "的技能不能连续使用！")
                End If
            Case MoveEffect.煽动
                target.SAtkLVUp(1)
                target.confuse(moveCall, user)
            Case MoveEffect.虚张声势
                target.AtkLVUp(2)
                target.confuse(moveCall, user)
            Case MoveEffect.识破
                target.BattleState.foresight = True
                log.AddText(target.GetNameString & "被识破了！")
            Case MoveEffect.回复封印
                If target.BattleState.healBlock Then
                    Fail()
                Else
                    target.BattleState.healBlock = True
                    target.BattleState.healBlockCounter = 5
                    log.AddText(target.GetNameString & "禁止回复！")
                End If
            Case MoveEffect.力量交换
                Dim temp As Integer
                temp = user.AttackLV
                user.BattleState.AttackLV = target.AttackLV
                target.BattleState.AttackLV = temp
                temp = user.SpAttackLV
                user.BattleState.SpAttackLV = target.SpAttackLV
                target.BattleState.SpAttackLV = temp
                log.AddText(user.CustomName & "和" & target.GetNameString & "交换了攻击等级！")
            Case MoveEffect.防御交换
                Dim temp As Integer
                temp = user.DefenceLV
                user.BattleState.DefenceLV = target.DefenceLV
                target.BattleState.DefenceLV = temp
                temp = user.SpDefenceLV
                user.BattleState.SpDefenceLV = target.SpDefenceLV
                target.BattleState.SpDefenceLV = temp
                log.AddText(user.CustomName & "和" & target.GetNameString & "交换了防御等级！")
            Case MoveEffect.心灵交换
                Dim temp As Integer() = New Integer() {user.AttackLV, user.DefenceLV, user.SpAttackLV, user.SpDefenceLV, _
                    user.SpeedLV, user.BattleState.evasionLV, user.BattleState.AccLV}
                user.BattleState.AttackLV = target.AttackLV
                user.BattleState.DefenceLV = target.DefenceLV
                user.BattleState.SpAttackLV = target.SpAttackLV
                user.BattleState.SpDefenceLV = target.SpDefenceLV
                user.BattleState.SpeedLV = target.SpeedLV
                user.BattleState.evasionLV = target.BattleState.evasionLV
                user.BattleState.AccLV = target.BattleState.AccLV
                target.BattleState.AttackLV = temp(0)
                target.BattleState.DefenceLV = temp(1)
                target.BattleState.SpAttackLV = temp(2)
                target.BattleState.SpDefenceLV = temp(3)
                target.BattleState.SpeedLV = temp(4)
                target.BattleState.evasionLV = temp(5)
                target.BattleState.AccLV = temp(6)
                log.AddText(user.CustomName & "和" & target.GetNameString & "交换了能力等级！")
            Case MoveEffect.换装
                If target.SelTrait(True) = Trait.属性之盾 OrElse target.SelTrait(True) = Trait.气象台 Then
                    Fail()
                Else
                    log.AddText(user.CustomName & "的特性变成了" & target.SelTrait(True).ToString & "！")
                    user.SetTrait(target.SelTrait(True))
                End If
            Case MoveEffect.戏法
                If (user.Item(True) = Item.无 AndAlso target.Item(True) = Item.无) OrElse _
                    (target.SelTrait = Trait.粘着 AndAlso user.SelTrait <> Trait.破格) _
                    OrElse (user.SelTrait = Trait.粘着 AndAlso target.SelTrait <> Trait.破格) Then
                    Fail()
                Else
                    log.AddText(user.CustomName & "和" & target.GetNameString & "交换了道具！")
                    Dim temp As Item = user.Item(True)
                    user.SetItem(target.Item(True))
                    target.SetItem(temp)
                End If
            Case MoveEffect.扣押
                If target.BattleState.detain Then
                    Fail()
                Else
                    target.BattleState.detain = True
                    target.BattleState.detainCounter = 5
                    log.AddText(target.GetNameString & "的道具无效了！")
                End If
            Case MoveEffect.抢夺
                target.BattleState.snatched = True
                target.BattleState.snatchBy = user
            Case MoveEffect.胃液
                If target.BattleState.gastricJuice = False Then
                    target.BattleState.worrySeed = False
                    target.BattleState.gastricJuice = True
                    log.AddText(target.GetNameString & "的特性无效了")
                Else
                    Fail()
                End If

            Case MoveEffect.素描
                If target.LastMoveIsValid _
                    AndAlso Not user.HaveMove(target.SelMoveName(target.lastMoveIndex)) Then
                    For i As Integer = 1 To 4
                        If user.SelMove(i) Is selMove Then
                            Dim move As Move = New Move(BattleData.GetMove(target.LastMove.Identity), False)
                            user.SetMove(i, move)
                            user.lastMoveIndex = 0
                            Exit For
                        End If
                    Next
                Else
                    Fail()
                End If
            Case MoveEffect.模仿
                If target.LastMoveIsValid _
                    AndAlso Not user.HaveMove(target.SelMoveName(target.lastMoveIndex)) Then
                    For i As Integer = 1 To 4
                        If user.SelMove(i) Is selMove Then
                            Dim move As Move = New Move(BattleData.GetMove(target.LastMove.Identity), False)
                            move.SetPP(5)
                            user.SetTempMove(i, move)
                            user.lastMoveIndex = 0
                            Exit For
                        End If
                    Next
                Else
                    Fail()
                End If

            Case MoveEffect.变身
                If Not target.BattleState.transfrom Then
                    log.AddText(user.CustomName & "变成了" & target.NameBase & "！")
                    user.Transfrom(target)
                    user.lastMoveIndex = 0
                Else
                    Fail()
                End If
            Case MoveEffect.回避减一
                target.EvasionLVDown(1, user, True)
            Case MoveEffect.命中减一
                target.AccLVDown(1, user, True)
            Case MoveEffect.速度减一
                target.SpeedLVDown(1, user, True)
            Case MoveEffect.速度减二
                target.SpeedLVDown(2, user, True)
            Case MoveEffect.物攻减一
                target.AtkLVDown(1, user, True)
            Case MoveEffect.物攻减二
                target.AtkLVDown(2, user, True)
            Case MoveEffect.物防减一
                target.DefLVDown(1, user, True)
            Case MoveEffect.物防减二
                target.DefLVDown(2, user, True)
            Case MoveEffect.特攻减一
                target.SAtkLVDown(1, user, True)
            Case MoveEffect.特攻减二
                target.SAtkLVDown(2, user, True)
            Case MoveEffect.诱惑
                If target.Gender <> PokemonGender.No AndAlso user.Gender <> PokemonGender.No AndAlso target.Gender <> user.Gender Then
                    target.SAtkLVDown(2, user, True)
                Else
                    Fail()
                End If
            Case MoveEffect.特防减一
                target.SDefLVDown(1, user, True)
            Case MoveEffect.特防减二
                target.SDefLVDown(2, user, True)
            Case MoveEffect.物理攻防减一
                target.AtkLVDown(1, user, True)
                target.DefLVDown(1, user, True)
            Case MoveEffect.自我暗示
                user.BattleState.AttackLV = target.BattleState.AttackLV
                user.BattleState.SpAttackLV = target.BattleState.SpAttackLV
                user.BattleState.DefenceLV = target.BattleState.DefenceLV
                user.BattleState.SpDefenceLV = target.BattleState.SpDefenceLV
                user.BattleState.SpeedLV = target.BattleState.SpeedLV
                user.BattleState.evasionLV = target.BattleState.evasionLV
                user.BattleState.AccLV = target.BattleState.AccLV
                log.AddText(user.CustomName & "复制了" & target.GetNameString & "的能力等级！")
            Case MoveEffect.奇迹之眼
                target.BattleState.miracleEye = True
            Case MoveEffect.点穴
                Dim value As Integer() = New Integer() _
                    {target.SpeedLV, target.AttackLV, target.SpAttackLV, target.DefenceLV, target.SpDefenceLV, _
                    target.BattleState.AccLV, target.BattleState.evasionLV}
                Dim rand As Integer = _random.Next(0, 7)
                Do While value(rand) = 6
                    rand = _random.Next(0, 7)
                Loop
                Select Case rand
                    Case 0
                        target.SpeedLVUp(2)
                    Case 1
                        target.AtkLVUp(2)
                    Case 2
                        target.SAtkLVUp(2)
                    Case 3
                        target.DefLVUp(2)
                    Case 4
                        target.SDefLVUp(2)
                    Case 5
                        target.AccuracyLVUp(2)
                    Case 6
                        target.EvasionLVUp(2, False)
                End Select
            Case MoveEffect.除雾
                target.EvasionLVDown(1, user, True)
                target.myTeam.BreakMist()
            Case MoveEffect.先取
                If Not target.BattleState.moved Then
                    Dim move As Move
                    For Each item As PlayerBattleMove In atkList
                        If item.PM Is target Then
                            move = GetPokemonMove(item)
                            If move.MoveType = PokemonBattle.PokemonData.MoveType.其他 Then Return
                            log.AddText(target.GetNameString & "的技能被先取了！")
                            user.BattleState.meFirst = True
                            Me.MoveCall(move, user)
                            user.BattleState.meFirst = False
                            Exit For
                        End If
                    Next
                Else
                    Fail()
                End If
            Case MoveEffect.眩晕之舞
                target.confuse(False, user)

            Case MoveEffect.属性切换2
                If target.LastMoveIsValid Then
                    Dim array As New List(Of Integer)
                    Dim type As PokemonType = target.LastMove.Type
                    For Each data As PokemonType In BattleData.GetAllTypes
                        If type.TypeEffects(data.Identity) < 1 Then
                            array.Add(data.Identity)
                        End If
                    Next
                    Dim rand As Integer = _random.Next(0, array.Count)
                    user.Type1 = BattleData.GetTypeData(array(rand))
                    array = Nothing
                    type = Nothing
                    log.AddText(user.CustomName & "的属性变成了" & user.Type1.Name)
                Else
                    Fail()
                End If

            Case MoveEffect.鹦鹉学舌
                If target.LastMoveIsValid Then
                    Dim move As Move = New Move(BattleData.GetMove(target.LastMove.Identity), False)
                    Me.MoveCall(move, user)
                Else
                    Fail()
                End If
            Case MoveEffect.特性交换
                If target.SelTrait(True) = Trait.属性之盾 OrElse user.SelTrait(True) = Trait.属性之盾 _
                    OrElse target.SelTrait(True) = Trait.气象台 OrElse user.SelTrait(True) = Trait.气象台 Then
                    Fail()
                Else
                    log.AddText(user.CustomName & "和" & target.GetNameString & "交换了特性！")
                    Dim temp As Trait = user.SelTrait(True)
                    user.SetTrait(target.SelTrait(True))
                    target.SetTrait(temp)
                End If
            Case MoveEffect.单纯光线
                If target.SelTrait = Trait.偷懒 Then
                    Fail()
                Else
                    target.SetTrait(Trait.单纯)
                    log.AddText(target.GetNameString & "的特性变成了单纯！")
                End If
            Case MoveEffect.烦恼之种
                If target.BattleState.worrySeed OrElse target.SelTrait = Trait.偷懒 Then
                    Fail()
                Else
                    target.BattleState.worrySeed = True
                    log.AddText(target.GetNameString & "的特性变成了不眠！")
                    target.RaiseTrait()
                End If
            Case MoveEffect.变为同伴
                target.SetTrait(user.SelTrait(True))
                log.AddText(String.Format("{0}的特性变成了{1}！", target.GetNameString, user.SelTrait(True).ToString()))

            Case MoveEffect.HP一半恢复
                RecoverPokemonHP(target, target.MAXHP >> 1)

            Case MoveEffect.镜面属性
                user.Type1 = target.Type1
                user.Type2 = target.Type2

                log.AddText(String.Format("{0}复制了{1}的属性", user.CustomName, target.CustomName))
            Case MoveEffect.浸水
                Dim type As PokemonType = BattleData.GetTypeData("水")
                target.Type1 = type
                target.Type2 = Nothing
                log.AddText(target.CustomName & "的属性变成了" & type.Name & "！")
            Case MoveEffect.传递礼物
                If target.Item(True) = Item.无 AndAlso user.Item(True) <> Item.无 Then
                    target.SetItem(user.Item(True))
                    user.SetItem(Item.无)
                    log.AddText(String.Format("{0}将道具送给了{1}", user.CustomName, target.CustomName))
                End If
            Case MoveEffect.您先请
                If Not BringToAttackListFront(target) Then
                    Fail()
                End If
            Case MoveEffect.押后
                If BringToAttackListBack(target) Then
                    log.AddText(String.Format("{0}的行动被押后了", target.CustomName))
                Else
                    Fail()
                End If
            Case MoveEffect.念动力
                If target.BeTelekinesis() Then
                    log.AddText(String.Format("{0}浮到了空中", target.CustomName))
                Else
                    Fail()
                End If
            Case MoveEffect.防御平分
                Dim total As Integer
                total = user.DefenceValue + target.DefenceValue
                user.DefenceValue = total \ 2
                target.DefenceValue = total \ 2
                total = user.SpDefenceValue + target.SpDefenceValue
                user.SpDefenceValue = total \ 2
                target.SpDefenceValue = total \ 2
                log.AddText("双方的防御平分了")

            Case MoveEffect.力量平分
                Dim total As Integer
                total = user.AttackValue + target.AttackValue
                user.AttackValue = total \ 2
                target.AttackValue = total \ 2
                total = user.SpAttackValue + target.SpAttackValue
                user.SpAttackValue = total \ 2
                target.SpAttackValue = total \ 2
                log.AddText("双方的攻击平分了")

        End Select
    End Sub

    Private Sub AssistFriend(ByVal move As Move, ByVal pm As Pokemon)
        Select Case move.Effect
            Case MoveEffect.帮手
                pm.BattleState.assisted = True
        End Select
    End Sub

    Private Sub AssistAllCourt(ByVal selMove As Move, ByVal user As Pokemon)
        Select Case selMove.Effect
            Case MoveEffect.玩水
                log.AddText("火属性攻击的伤害减少了！")
                ground.waterSport = True
            Case MoveEffect.玩泥
                log.AddText("电属性攻击的伤害减少了！")
                ground.mudSport = True
            Case MoveEffect.欺骗空间
                ground.TrickRoomChange(log)
            Case MoveEffect.冰雹
                If Not ground.NewWeather(Weather.HailStorm, user, log) Then
                    Fail()
                End If
            Case MoveEffect.放晴
                If Not ground.NewWeather(Weather.Sunny, user, log) Then
                    Fail()
                End If
            Case MoveEffect.沙尘暴
                If Not ground.NewWeather(Weather.SandStorm, user, log) Then
                    Fail()
                End If
            Case MoveEffect.求雨
                If Not ground.NewWeather(Weather.Rainy, user, log) Then
                    Fail()
                End If
            Case MoveEffect.重力
                If ground.Gravity Then
                    Fail()
                Else
                    ground.Gravitate(log)
                End If
            Case MoveEffect.黑雾
                log.AddText("所有能力等级恢复了原状！")
                For Each pm As Pokemon In team1.SelectedPokemon
                    pm.Haze()
                Next
                For Each pm As Pokemon In team2.SelectedPokemon
                    pm.Haze()
                Next
            Case MoveEffect.灭歌
                For Each pm As Pokemon In team1.SelectedPokemon
                    If pm.SelTrait <> Trait.隔音 Then pm.PerishSong()
                Next
                For Each pm As Pokemon In team2.SelectedPokemon
                    If pm.SelTrait <> Trait.隔音 Then pm.PerishSong()
                Next
                log.AddText("场上的PM听到了灭亡歌！")
            Case MoveEffect.魔术空间
                ground.MagicRoomChange(log)
            Case MoveEffect.神奇空间
                ground.MiracleRoomChange(log)
        End Select
    End Sub

    Private Sub AssistTargetTeam(ByVal selMove As Move, ByVal targetTeam As Team)

        Select Case selMove.Effect
            Case MoveEffect.撒菱
                If targetTeam.spikesCounter < 3 Then
                    targetTeam.spikesCounter += 1
                End If
                log.AddText(targetTeam.PlayerName & "的队伍被钉子包围了")
            Case MoveEffect.秘密岩石
                targetTeam.hideRockCounter = 1
                log.AddText(targetTeam.PlayerName & "的队伍被碎石包围了")
            Case MoveEffect.毒菱
                If targetTeam.poisonSpikesCounter < 2 Then
                    targetTeam.poisonSpikesCounter += 1
                End If
                log.AddText(targetTeam.PlayerName & "的队伍被毒菱包围了")
        End Select
    End Sub

    Private Sub AssistUserTeam(ByVal selMove As Move, ByVal userTeam As Team, ByVal user As Pokemon)
        Select Case selMove.Effect
            Case MoveEffect.反射盾
                If Not userTeam.MakeReflect(user) Then
                    Fail()
                End If
            Case MoveEffect.光壁
                If Not userTeam.MakeLightScreen(user) Then
                    Fail()
                End If
            Case MoveEffect.队伍状态恢复
                For Each pm As Pokemon In userTeam.Pokemon
                    If pm IsNot Nothing AndAlso Not (selMove.Sound AndAlso pm.SelTrait = Trait.隔音) Then pm.State = PokemonState.No
                Next
                log.AddText("全队的状态都恢复了！")
            Case MoveEffect.神秘守护
                If Not userTeam.MakeSafeguard Then
                    Fail()
                End If
            Case MoveEffect.白雾
                If Not userTeam.MakeMist() Then
                    Fail()
                End If
            Case MoveEffect.祈福
                If Not userTeam.MakeLuckyChant Then
                    Fail()
                End If
            Case MoveEffect.顺风
                If Not userTeam.MakeWithWind Then
                    Fail()
                End If
            Case MoveEffect.广域防御
                If user.BattleState.protectCounter < 6 _
                    AndAlso _random.NextDouble < 1 / Math.Pow(2, user.BattleState.protectCounter) AndAlso _
                    userTeam.MakeWideDefence() Then
                    user.BattleState.protectCounter += 1
                Else
                    user.BattleState.protectCounter = 0
                    Fail()
                End If
            Case MoveEffect.快速防御
                If user.BattleState.protectCounter < 6 _
                    AndAlso _random.NextDouble < 1 / Math.Pow(2, user.BattleState.protectCounter) AndAlso _
                    userTeam.MakeQuickDefence() Then
                    user.BattleState.protectCounter += 1
                Else
                    user.BattleState.protectCounter = 0
                    Fail()
                End If
        End Select
    End Sub

    Private Sub AssistUser(ByVal selMove As Move, ByVal user As Pokemon)
        Select Case selMove.Effect
            Case MoveEffect.诅咒
                user.AtkLVUp(1)
                user.DefLVUp(1)
                user.SpeedLVDown(1, True, True)
            Case MoveEffect.殊途同归
                user.BattleState.destinyBond = True
            Case MoveEffect.保护
                If atkList.Count <> 0 AndAlso (user.BattleState.protectCounter < 6 _
                    AndAlso _random.NextDouble < 1 / Math.Pow(2, user.BattleState.protectCounter)) Then
                    user.BattleState.protect = True
                    log.AddText(user.CustomName & "进入了保护范围！")
                    user.BattleState.protectCounter += 1
                Else
                    user.BattleState.protectCounter = 0
                    Fail()
                End If
            Case MoveEffect.忍耐
                If user.BattleState.protectCounter < 6 _
                    AndAlso _random.NextDouble < 1 / Math.Pow(2, user.BattleState.protectCounter) Then
                    user.BattleState.endure = True
                    log.AddText(user.CustomName & "忍耐了！")
                    user.BattleState.protectCounter += 1
                Else
                    user.BattleState.protectCounter = 0
                    Fail()
                End If
            Case MoveEffect.魔装反射
                user.BattleState.magicCoat = True
            Case MoveEffect.休息

                If user.State = PokemonState.Sleep OrElse ground.Uproar _
                    OrElse user.SelTrait = Trait.不眠 OrElse user.SelTrait = Trait.活跃 Then
                    Fail()
                    Return
                End If
                If Not user.CheckIfHPRecoveryIsEnabled() Then
                    Return
                End If
                If RecoverPokemonHP(user, user.MAXHP) Then
                    user.State = PokemonState.Sleep
                    log.AddText(user.CustomName & "睡着了！")
                    user.sleepTurn = RestTurn
                    If user.SelTrait = Trait.早起 Then user.sleepTurn = RestTurn \ 2
                    user.sleepCounter = 0
                    If user.Item = Item.果实2 OrElse user.Item = Item.果实9 Then
                        user.RaiseItem()
                    End If
                End If
            Case MoveEffect.HP一半恢复
                RecoverPokemonHP(user, user.MAXHP >> 1)
            Case MoveEffect.鸟栖
                If RecoverPokemonHP(user, user.MAXHP >> 1) Then
                    If user.HaveType("飞行") Then
                        user.BattleState.roost = True
                        If user.Type2 Is Nothing Then
                            user.BattleState.roostTemp = BattleData.GetTypeData("普通")
                        Else
                            user.BattleState.roostTemp = BattleData.GetTypeData("无")
                        End If
                        log.AddText(user.CustomName & "的飞行属性暂时消失了")
                    End If
                End If
            Case MoveEffect.HP天气恢复
                Dim recovery As Integer
                Select Case ground.Weather
                    Case Weather.No
                        recovery = user.MAXHP >> 1
                    Case Weather.Sunny
                        recovery = user.MAXHP * 2 \ 3
                    Case Else
                        recovery = user.MAXHP >> 2
                End Select
                RecoverPokemonHP(user, recovery)
            Case MoveEffect.许愿
                If Not user.CheckIfHPRecoveryIsEnabled() Then
                    Return
                End If
                If Not user.BattleState.wished Then
                    user.BattleState.wished = True
                    user.BattleState.wishTurn = -1
                    user.BattleState.wishRecovery = user.MAXHP \ 2
                    log.AddText(user.CustomName & "许下愿望！")
                Else
                    Fail()
                End If
            Case MoveEffect.接力
                If Not user.Pass Then Fail()
            Case MoveEffect.自身状态恢复
                If user.State = PokemonState.Burn OrElse user.State = PokemonState.Paralysis _
                    OrElse user.State = PokemonState.Poison OrElse user.State = PokemonState.Toxin Then
                    user.State = PokemonState.No
                    log.AddText(user.CustomName & "的状态恢复了！")
                End If
            Case MoveEffect.回避加一
                user.EvasionLVUp(1)
            Case MoveEffect.变小
                user.BattleState.minimize = True
                user.EvasionLVUp(2)

            Case MoveEffect.蓄力
                If user.BattleState.focusEngery = False Then
                    user.CTLVUp(2)
                    user.BattleState.focusEngery = True
                    log.AddText(user.CustomName & "在积蓄力量！")
                Else
                    Fail()
                End If
            Case MoveEffect.速度加一
                user.SpeedLVUp(1)
            Case MoveEffect.速度加二
                user.SpeedLVUp(2)

            Case MoveEffect.物攻加一
                user.AtkLVUp(1)
            Case MoveEffect.成长
                Dim up As Integer = 1
                If ground.Weather = Weather.Sunny Then up = 2

                user.AtkLVUp(up)
                user.SAtkLVUp(up)
            Case MoveEffect.物攻加二
                user.AtkLVUp(2)
            Case MoveEffect.物防加一
                user.DefLVUp(1)
            Case MoveEffect.物防加二
                user.DefLVUp(2)

            Case MoveEffect.特攻加一
                user.SAtkLVUp(1)
            Case MoveEffect.特攻加二
                user.SAtkLVUp(2)
            Case MoveEffect.特攻加三
                user.SAtkLVUp(3)
            Case MoveEffect.特防加一
                user.SDefLVUp(1)
            Case MoveEffect.特防加二
                user.SDefLVUp(2)
            Case MoveEffect.特殊攻防加一
                user.SAtkLVUp(1)
                user.SDefLVUp(1)
            Case MoveEffect.物理攻防加一
                user.AtkLVUp(1)
                user.DefLVUp(1)
            Case MoveEffect.物攻速度加一
                user.AtkLVUp(1)
                user.SpeedLVUp(1)
            Case MoveEffect.双防加一
                user.DefLVUp(1)
                user.SDefLVUp(1)
            Case MoveEffect.变圆
                user.BattleState.defenseCurl = True
                log.AddText(user.CustomName & "变圆了！")
                user.DefLVUp(1)
            Case MoveEffect.治愈之愿
                user.BattleState.healwish = True
                log.AddText(user.CustomName & "许下了治愈之愿！")
                user.GetHurt(user.HP)
            Case MoveEffect.腹鼓
                If user.HP > user.MAXHP / 2 Then
                    user.GetHurt(user.MAXHP / 2)
                    log.AddText(user.CustomName & "用HP换来了物攻的提升！")
                    user.AtkLVUp(12, False)
                Else
                    Fail()
                End If
            Case MoveEffect.液体圈
                If user.BattleState.aquaRing Then
                    Fail()
                Else
                    user.BattleState.aquaRing = True
                    log.AddText(user.CustomName & "被液体包围起来了！")
                End If
            Case MoveEffect.电磁浮游
                If user.BattleState.suspension OrElse ground.Gravity Then
                    Fail()
                Else
                    user.BattleState.suspension = True
                    user.BattleState.suspensionCounter = 5
                    log.AddText(user.CustomName & "的特性变成了浮游！")
                End If
            Case MoveEffect.充电
                user.SDefLVUp(1)
                user.BattleState.charge = True
                user.BattleState.chargeCounter = 2
                log.AddText(user.CustomName & "积蓄了电力！")
            Case MoveEffect.扎根
                If user.BattleState.ingrain Then
                    Fail()
                Else
                    user.BattleState.ingrain = True
                    log.AddText(user.CustomName & "长出了根！")
                End If
            Case MoveEffect.力量欺骗
                user.BattleState.powerTrick = Not user.BattleState.powerTrick
                log.AddText(user.CustomName & "交换了物攻和物防！")
            Case MoveEffect.新月之舞
                user.BattleState.lunarDance = True
                user.GetHurt(user.HP)
            Case MoveEffect.属性切换
                Dim rand As Integer
                Do
                    rand = _random.Next(1, 5)
                Loop Until user.SelMoveName(rand) <> ""
                Dim type As PokemonType = user.SelMove(rand).Type
                user.Type1 = type
                log.AddText(user.CustomName & "的属性变成了" & type.Name)
            Case MoveEffect.怨念
                user.BattleState.grudge = True
            Case MoveEffect.替身
                If Not user.MakeSubstitute Then Fail()
            Case MoveEffect.能量储存
                If user.BattleState.stockpileCounter < 3 Then
                    log.AddText(user.CustomName & "储存了能量！")
                    user.BattleState.stockpileCounter += 1
                    If user.DefLVUp(1, False) Then user.BattleState.stockpileDefCounter += 1
                    If user.SDefLVUp(1, False) Then user.BattleState.stockpileSDefCounter += 1
                Else
                    Fail()
                End If
            Case MoveEffect.能量吸入
                If Not user.CheckIfHPRecoveryIsEnabled() Then
                    Return
                End If
                If user.BattleState.stockpileCounter <> 0 AndAlso user.HP <> user.MAXHP Then
                    Select Case user.BattleState.stockpileCounter
                        Case 1
                            user.HPRecover(user.MAXHP * 0.25)
                        Case 2
                            user.HPRecover(user.MAXHP * 0.5)
                        Case 3
                            user.HPRecover(user.MAXHP)
                    End Select
                    With user
                        .BattleState.stockpileCounter = 0
                        .DefLVDown(user.BattleState.stockpileDefCounter, True)
                        .BattleState.stockpileDefCounter = 0
                        .SDefLVDown(user.BattleState.stockpileSDefCounter, True)
                        .BattleState.stockpileSDefCounter = 0
                    End With
                Else
                    Fail()
                End If
            Case MoveEffect.道具回收
                If user.Item <> Item.无 OrElse user.UsedItem = Item.无 OrElse (112 < user.UsedItem AndAlso user.UsedItem < 116) Then
                    Fail()
                Else
                    log.AddText(user.CustomName & "回收了道具！")
                    user.SetItem(user.UsedItem)
                    user.UsedItem = Item.无
                    If (user.Item >= Item.果实1 AndAlso user.Item <= Item.果实15) OrElse _
                        (user.Item >= Item.果实53 AndAlso user.Item <= Item.果实59) OrElse user.Item = Item.果实61 _
                        OrElse user.Item = Item.白色香草 OrElse user.Item = Item.精神香草 Then
                        user.RaiseItem()
                    End If
                End If
            Case MoveEffect.保护色
                Dim name As String
                Select Case ground.Terrain
                    Case BattleTerrain.Stadium
                        name = "普通"
                    Case BattleTerrain.Grass
                        name = "草"
                    Case BattleTerrain.Flat
                        name = "普通"
                    Case BattleTerrain.Sand
                        name = "地面"
                    Case BattleTerrain.Mountain
                        name = "岩"
                    Case BattleTerrain.Cave
                        name = "岩"
                    Case BattleTerrain.Water
                        name = "水"
                    Case Else
                        name = "冰"
                End Select
                Dim type As PokemonType = BattleData.GetTypeData(name)
                user.Type1 = type
                user.Type2 = Nothing
                log.AddText(user.CustomName & "的属性变成了" & type.Name & "！")
            Case MoveEffect.封印
                If user.BattleState.imprison Then
                    Fail()
                Else
                    user.BattleState.imprison = True
                End If
            Case MoveEffect.跟我来
                If Mode <> BattleMode.Single Then ground.followPokemon = user
            Case MoveEffect.攻速度加二防减一
                user.DefLVDown(1, True)
                user.SDefLVDown(1, True)
                user.AtkLVUp(2)
                user.SAtkLVUp(2)
                user.SpeedLVUp(2)
            Case MoveEffect.双攻加一
                user.AtkLVUp(1)
                user.SAtkLVUp(1)

            Case MoveEffect.物理攻防命中加一
                user.AtkLVUp(1)
                user.DefLVUp(1)
                user.AccuracyLVUp(1)

            Case MoveEffect.特殊攻防速度加一
                user.SAtkLVUp(1)
                user.SDefLVUp(1)
                user.SpeedLVUp(1)

            Case MoveEffect.物攻加一速度加二
                user.AtkLVUp(1)
                user.SpeedLVUp(2)

            Case MoveEffect.物攻命中加一
                user.AtkLVUp(1)
                user.AccuracyLVUp(1)

            Case MoveEffect.位置交换
                If user.myTeam.SwapPokemonPosition() Then
                    log.AddText(String.Format("{0}与队友交换了位置", user.CustomName))
                Else
                    Fail()
                End If
            Case MoveEffect.身体净化
                If user.SpeedLVUp(2) Then
                    If user.Weight > 100 Then
                        user.Weight -= 100
                    Else
                        user.Weight = 0.1
                    End If
                Else
                    Fail()
                End If
        End Select
    End Sub

    Private Function RecoverPokemonHP(ByVal pm As Pokemon, ByVal recovery As Integer) As Boolean
        If Not pm.CheckIfHPRecoveryIsEnabled() Then
            Return False
        End If
        If pm.HP <> pm.MAXHP Then
            pm.HPRecover(recovery)
            log.AddText(pm.CustomName & "回复了HP！")
            Return True
        Else
            log.AddText(pm.CustomName & "的HP已经满了")
            Return False
        End If
    End Function


    Private Sub AssistMoveCall(ByVal selMove As Move, ByVal user As Pokemon)
        Select Case selMove.Effect
            Case MoveEffect.梦话
                If user.ChoiceItem AndAlso user.BattleState.choiceSleepTalked Then
                    Fail() : Return
                End If
                If user.State = PokemonState.Sleep Then
                    For i As Integer = 1 To 4
                        If user.SelMove(i) IsNot Nothing AndAlso user.SelMove(i) IsNot selMove Then
                            Dim move As Move = user.SelMove(i)
                            If CanSleepTalk(move) Then Exit For
                        End If
                        If i = 4 Then Fail() : Return
                    Next
                    Dim newIndex As Integer
                    Do
                        newIndex = _random.Next(1, 5)
                    Loop While user.SelMove(newIndex) Is Nothing OrElse user.SelMove(newIndex) Is selMove OrElse _
                        Not CanSleepTalk(user.SelMove(newIndex))
                    If user.ChoiceItem Then user.BattleState.choiceSleepTalked = True
                    user.BattleState.sleepTalking = True
                    MoveCall(user.SelMove(newIndex), user)
                Else
                    Fail()
                End If
            Case MoveEffect.猫手
                Dim array As List(Of Integer) = user.myTeam.GetAllMove(user)
                array.RemoveAll(Function(moveId)
                                    Dim newMove As MoveData = BattleData.GetMove(moveId)
                                    Return Not CanCallMove(newMove)
                                End Function)

                If array.Count = 0 Then Fail() : Return

                Dim move As Move
                move = New Move(BattleData.GetMove(array(_random.Next(0, array.Count))), False)
                MoveCall(move, user)
            Case MoveEffect.摇手指
                Dim MoveIndex As Integer
                Dim move As Move
                Dim data As MoveData
                Dim moves As MoveData() = BattleData.GetAllMoves
                Do
                    MoveIndex = _random.Next(0, moves.Length)
                    data = moves(MoveIndex)
                Loop Until CanCallMove(data)
                move = New Move(data, False)
                MoveCall(move, user)
            Case MoveEffect.效仿
                If lastMove <> MoveData.InvalidId Then
                    Dim move As Move = New Move(BattleData.GetMove(lastMove), False)
                    MoveCall(move, user)
                Else
                    Fail()
                End If
            Case MoveEffect.自然力量
                Dim name As String
                Select Case ground.Terrain
                    Case BattleTerrain.Stadium
                        name = "三角攻击"
                    Case BattleTerrain.Grass
                        name = "种子爆弹"
                    Case BattleTerrain.Flat
                        name = "地震"
                    Case BattleTerrain.Sand
                        name = "地震"
                    Case BattleTerrain.Mountain
                        name = "岩崩"
                    Case BattleTerrain.Cave
                        name = "岩崩"
                    Case BattleTerrain.Water
                        name = "水压"
                    Case Else
                        name = "暴风雪"
                End Select
                Dim move As Move = New Move(BattleData.GetMove(name), False)
                MoveCall(move, user)
        End Select
    End Sub

    Private Sub MoveCall(ByVal selMove As Move, ByVal user As Pokemon)
        If selMove.AddEff2 = MoveAdditionalEffect.觉醒力量 OrElse selMove.AddEff2 = MoveAdditionalEffect.制裁飞石 OrElse _
            selMove.AddEff2 = MoveAdditionalEffect.自然恩惠 OrElse selMove.AddEff2 = MoveAdditionalEffect.科技爆破 _
            OrElse selMove.Effect = MoveEffect.诅咒 Then
            user.UpdateMove(selMove)
        End If
        Dim targetIndex As TargetIndex = targetIndex.DefaultTarget
        Select Case selMove.Target
            Case MoveTarget.选一, MoveTarget.随机
                targetIndex = targetIndex.Random
            Case MoveTarget.队友
                targetIndex = targetIndex.TeamFriend
            Case MoveTarget.自身, MoveTarget.任选
                targetIndex = targetIndex.Self
        End Select
        user.lastTarget = targetIndex
        Dim targets As List(Of Pokemon) = GetTargetsFromIndex(user, selMove, targetIndex)
        UseSkill(user, selMove, targets, True)
    End Sub


    Private Shared Function CanSleepTalk(ByVal data As Move) As Boolean
        With data
            If data.PP = 0 Then Return False
            If (MoveAdditionalEffect.准备后攻击 <= .AddEff2 AndAlso .AddEff2 <= MoveAdditionalEffect.消失后攻击) _
                OrElse .AddEff1 = MoveAdditionalEffect.次回合不能行动 _
                OrElse .AddEff2 = MoveAdditionalEffect.吵闹 _
                OrElse .AddEff2 = MoveAdditionalEffect.克制 _
                OrElse .Target = MoveTarget.技能 _
                OrElse .Effect = MoveEffect.先取 _
                OrElse .Effect = MoveEffect.模仿 _
                OrElse .Name = "气合拳" _
                OrElse .Effect = MoveEffect.鹦鹉学舌 _
                OrElse .Name = "喋喋不休" Then _
                Return False
        End With
        Return True
    End Function

    Private Shared Function CanCallMove(ByVal data As MoveData) As Boolean
        With data
            If .Target = MoveTarget.技能 _
                OrElse .Effect = MoveEffect.保护 _
                OrElse .Effect = MoveEffect.忍耐 _
                OrElse .Effect = MoveEffect.素描 _
                OrElse .Effect = MoveEffect.模仿 _
                OrElse .Name = "气合拳" _
                OrElse .Effect = MoveEffect.抢夺 _
                OrElse .AddEffect2 = MoveAdditionalEffect.镜返 _
                OrElse .AddEffect2 = MoveAdditionalEffect.反击 _
                OrElse .Effect = MoveEffect.殊途同归 _
                 OrElse .Effect = MoveEffect.帮手 _
                OrElse .Effect = MoveEffect.跟我来 _
                OrElse .AddEffect1 = MoveAdditionalEffect.夺取道具 _
                OrElse .Effect = MoveEffect.鹦鹉学舌 _
                OrElse .Effect = MoveEffect.戏法 _
                OrElse .Effect = MoveEffect.先取 _
                OrElse .Name = "喋喋不休" _
                OrElse .Name = "拼命" _
                OrElse .Name = "佯攻" Then _
                Return False
        End With
        Return True
    End Function


    Private Sub Fail()
        log.AddText("但它失败了！")
    End Sub

    Private Sub AddLine()
        log.AddLog("---------------------------------")
    End Sub


#End Region

    Public Const StruggleIndex As Integer = 5
    Public Const TauntTurn As Integer = 3
    Public Const UproarTurn As Integer = 3
    Public Const MaxSleepTurn As Integer = 3
    Public Const MinSleepTurn As Integer = 1
    Public Const RestTurn As Integer = 2
    Public Const EncoreTurn As Integer = 3
    Public Const WithwindTurn As Integer = 4

    Public Enum BattleTurnMove
        NextTurn
        Pass
        Death
    End Enum
End Class
