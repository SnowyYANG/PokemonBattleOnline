Imports PokemonBattle.BattleNetwork
Imports PokemonBattle.PokemonData
Public Class Team
    Public mPokemons As List(Of Pokemon)
    Public WithEvents ground As BattleGround
    Private WithEvents mSelectedPokemon As List(Of Pokemon)
    Private player As String()

    Public log As ImgTxt
    Public opponentTeam As Team
    Public random As Random

    Private mReflect, mLightScreen As Boolean
    Private reflectCounter, lightScreenCounter As Integer
    Private mSafeguard, mMist, mLuckyChant As Boolean
    Private safeguardCounter, mistCounter, luckyChantCounter As Integer
    Private mWithWind As Boolean
    Private withWindCounter As Integer
    Public spikesCounter, hideRockCounter, poisonSpikesCounter As Integer
    Public quickDefence As Boolean
    Public wideDefence As Boolean

    Public moor As Boolean
    Private moorCounter As Integer
    Public rainbow As Boolean
    Private rainbowCounter As Integer
    Public flameSea As Boolean
    Private flameSeaCounter As Integer

    Private rockType As PokemonType

    Public Event PokemonDied As EventHandler
    Public Event PokemonHPChanged As EventHandler
    Public Event PokemonStateChanged As EventHandler
    Public Event PokemonChanged As EventHandler
    Public Event PokemonImageChanged As EventHandler

    Public Sub New(ByVal playerName As String(), ByVal txt As ImgTxt, _
        ByVal randomValue As Random, ByVal mode As BattleMode, ByVal ppUp As Boolean, ByVal ParamArray infos As TeamData())
        mPokemons = New List(Of Pokemon)
        mSelectedPokemon = New List(Of Pokemon)
        random = randomValue
        player = playerName
        log = txt
        For Each info As TeamData In infos
            For Each pm As PokemonCustomInfo In info.Pokemons
                Dim pmData As PokemonData = BattleData.GetPokemon(pm.Identity)
                If pmData IsNot Nothing Then
                    Dim newPM As New Pokemon(pmData, pm, Me, ppUp)
                    mPokemons.Add(newPM)
                Else
                    mPokemons.Add(Nothing)
                End If
            Next
        Next
        rockType = BattleData.GetTypeData("岩")
        mSelectedPokemon.Add(mPokemons(0))
        Select Case mode
            Case BattleMode.Double
                mSelectedPokemon.Add(mPokemons(1))
            Case BattleMode.Double_4P
                mSelectedPokemon.Add(mPokemons(6))
        End Select
        For Each pm As Pokemon In SelectedPokemon
            pm.BattleState = New PokemonBattleState
            AddPokemonHandler(pm)
        Next
    End Sub

    Public ReadOnly Property SelectedPokemon() As List(Of Pokemon)
        Get
            Return mSelectedPokemon
        End Get
    End Property
    Public ReadOnly Property Pokemon() As List(Of Pokemon)
        Get
            Return mPokemons
        End Get
    End Property

    Public Function CanChange(ByVal pm As Pokemon) As Boolean
        If mPokemons.Count > 6 Then
            Return CanChange(SelectedPokemon.IndexOf(pm))
        Else
            Return CanChange(0)
        End If
    End Function
    Private Function CanChange(ByVal index As Integer) As Boolean
        Dim start As Integer = 1
        If index = 1 Then start += 6
        For i As Integer = start To start + 4
            If Not mSelectedPokemon.Contains(mPokemons(i)) AndAlso mPokemons(i) IsNot Nothing AndAlso mPokemons(i).HP <> 0 Then
                Return True
            End If
        Next
        Return False
    End Function


    Public Sub ChangePokemon(ByVal pm As Pokemon, ByVal changeTo As Integer, ByVal pass As Boolean)
        For i As Integer = 0 To mPokemons.Count - 1
            If mPokemons(i) Is pm Then
                If i > 1 Then changeTo += 6
                ChangePM(i, changeTo - 1, pass)
                Return
            End If
        Next
    End Sub
    Private Sub ChangePM(ByVal index1 As Integer, ByVal index2 As Integer, ByVal pass As Boolean)
        Dim withdraw As Pokemon = mPokemons(index1)
        If pass AndAlso withdraw.SelTrait = Trait.吸盘 Then pass = False

        log.AddText(GetPlayerName(withdraw) & "收回了" & withdraw.Nickname & "(" & withdraw.NameBase & ")！")

        With withdraw
            .lastMoveIndex = 0
            If .SelTrait = Trait.自然恢复 Then .RaiseTrait()
            If .UsedItem = Item.专爱头巾 OrElse .UsedItem = Item.专爱围巾 OrElse .UsedItem = Item.专爱眼镜 _
                Then .UsedItem = Item.无
            For i As Integer = 1 To 4
                If .SelMove(i) IsNot Nothing Then
                    .SelMove(i).EverUsed = False
                End If
            Next
        End With
        '跟我来 
        If ground.followPokemon Is withdraw Then ground.followPokemon = Nothing

        If Not pass OrElse withdraw.BattleState.uTurn Then
            ground.waterSport = False
            ground.mudSport = False
        End If
        '接力无法接束缚
        For Each pm As Pokemon In opponentTeam.SelectedPokemon
            If pm.BattleState.constraint Then
                If pm.BattleState.constraintBy = PokemonIndex.Pokemon1OfTeam1 _
                    OrElse pm.BattleState.constraintBy = PokemonIndex.Pokemon1OfTeam2 Then
                    If withdraw Is SelectedPokemon(0) Then
                        pm.BattleState.constraint = False
                    End If
                ElseIf withdraw Is SelectedPokemon(1) Then
                    pm.BattleState.constraint = False
                End If
            End If
        Next
        For Each pm As Pokemon In opponentTeam.SelectedPokemon
            If pm.BattleState.captivated AndAlso pm.BattleState.captivateTarget Is withdraw Then
                pm.BattleState.captivated = False
                pm.BattleState.captivateTarget = Nothing
            End If
        Next

        Dim battleState As New PokemonBattleState
        With withdraw.BattleState
            If .wished AndAlso .wishTurn <> 1 Then
                battleState.wished = True
                battleState.wishTurn = .wishTurn
                battleState.wishRecovery = .wishRecovery
            End If
            If .futureAtk AndAlso .futureCounter <> 3 Then
                battleState.futureAtk = True
                battleState.futureCounter = .futureCounter
                battleState.futureAtkAcc = .futureAtkAcc
                battleState.futureAtkValue = .futureAtkValue
                battleState.futureAtkType = .futureAtkType
                battleState.futureAtkMoveType = .futureAtkMoveType
                battleState.futureAtkDamageRevision = .futureAtkDamageRevision
            End If
            battleState.healwish = .healwish
            battleState.lunarDance = .lunarDance
        End With
        '接力
        If pass AndAlso Not withdraw.BattleState.uTurn Then
            battleState.PassCopy(withdraw.BattleState)
        End If
        '睡眠计数器重置
        If withdraw.sleepTurn <> 0 Then
            withdraw.sleepCounter = 0
            If withdraw.hypnosis Then
                withdraw.sleepCounter = random.Next(Battle.MinSleepTurn, Battle.MaxSleepTurn + 1)
            End If
        End If

        withdraw.BattleState = Nothing

        Dim temp As Pokemon = withdraw
        mPokemons(index1) = mPokemons(index2)
        mPokemons(index2) = temp
        ChangeSelectedPokemon(temp, mPokemons(index1))

        ShowPokemon(mPokemons(index1), battleState)
    End Sub
    Private Sub ChangeSelectedPokemon(ByVal pm As Pokemon, ByVal pm2 As Pokemon)
        RemovePokemonHandler(pm)
        For i As Integer = 0 To mSelectedPokemon.Count - 1
            If mSelectedPokemon(i) Is pm Then
                mSelectedPokemon(i) = pm2
                Exit For
            End If
        Next
        AddPokemonHandler(pm2)
    End Sub
    Private Sub ShowPokemon(ByVal pm As Pokemon, ByVal battleState As PokemonBattleState)
        pm.BattleState = battleState

        log.AddText(GetPlayerName(pm) & "换上了" & pm.Nickname & "(" & pm.NameBase & ")！")
        RaiseEvent PokemonChanged(pm, EventArgs.Empty)

        ground.ChangeWeather(pm)

        If pm.SelTrait <> Trait.魔法守护 AndAlso pm.HP > 0 Then
            If Not (pm.HaveType("飞行") OrElse pm.SelTrait = Trait.浮游 OrElse _
                    pm.BattleState.suspension) OrElse ground.Gravity OrElse pm.Item = Item.黑铁球 Then
                If poisonSpikesCounter > 0 Then
                    If pm.HaveType("毒") Then
                        poisonSpikesCounter = 0
                    ElseIf Not pm.HaveType("钢") Then
                        Select Case poisonSpikesCounter
                            Case 1
                                pm.SetState(PokemonState.Poison, , False, True)
                            Case 2
                                pm.SetState(PokemonState.Toxin, , False, True)
                        End Select
                    End If
                End If

                If spikesCounter > 0 Then
                    log.AddText(pm.Nickname & "受到了钉子的伤害")
                    Select Case spikesCounter
                        Case 1
                            pm.GetHurt(pm.MAXHP * 0.125)
                        Case 2
                            pm.GetHurt(pm.MAXHP / 6)
                        Case 3
                            pm.GetHurt(pm.MAXHP * 0.25)
                    End Select
                End If
            End If
            If pm.HP = 0 Then Return
            If hideRockCounter = 1 Then
                log.AddText(pm.Nickname & "受到了隐蔽沙砾的伤害")
                Dim typeEff As Double = rockType.TypeEffects(pm.Type1.Identity)
                If pm.Type2 IsNot Nothing Then typeEff *= rockType.TypeEffects(pm.Type2.Identity)
                Select Case typeEff
                    Case 0.25
                        pm.GetHurt(pm.MAXHP * 0.03125)
                    Case 4
                        pm.GetHurt(pm.MAXHP * 0.5)
                    Case 0.5
                        pm.GetHurt(pm.MAXHP * 0.0625)
                    Case 2
                        pm.GetHurt(pm.MAXHP * 0.25)
                    Case Else
                        pm.GetHurt(pm.MAXHP * 0.125)
                End Select
            End If
        End If
        If pm.HP = 0 Then Return

        If battleState.healwish Then
            log.AddText(pm.Nickname & "恢复了HP和状态！")
            pm.HPRecover(pm.MAXHP)
            pm.State = PokemonState.No
            battleState.healwish = False
        End If
        If battleState.lunarDance Then
            log.AddText(pm.Nickname & "恢复了HP,PP和状态！")
            pm.HPRecover(pm.MAXHP)
            pm.State = PokemonState.No
            battleState.lunarDance = False
            For i As Integer = 1 To 4
                If pm.SelMove(i) IsNot Nothing Then pm.SelMove(i).PPRecover(pm.SelMove(i).MAXPP)
            Next
        End If
        pm.Showed()
        battleState.moved = True
        battleState.swaped = True
    End Sub

    Public Function NoPM() As Boolean
        For Each pm As Pokemon In Pokemon
            If pm IsNot Nothing AndAlso pm.HP <> 0 Then Return False
        Next
        Return True
    End Function
    Public ReadOnly Property AliveSelectedPokemonCount() As Integer
        Get
            Dim count As Integer
            For Each pm As Pokemon In mSelectedPokemon
                If pm IsNot Nothing AndAlso pm.HP <> 0 Then count += 1
            Next
            Return count
        End Get
    End Property
    Public ReadOnly Property AlivePokemonCount() As Integer
        Get
            Dim count As Integer
            For Each pm As Pokemon In mPokemons
                If pm IsNot Nothing AndAlso pm.HP <> 0 Then count += 1
            Next
            Return count
        End Get
    End Property
    Public ReadOnly Property AlivePokemon() As Pokemon()
        Get
            Dim list As New List(Of Pokemon)
            For Each pm As Pokemon In SelectedPokemon
                If pm.HP <> 0 Then list.Add(pm)
            Next
            Return list.ToArray
        End Get
    End Property

    Public Function GetAllMove(ByVal except As Pokemon) As List(Of Integer)
        Dim newArray As New List(Of Integer)
        For Each pm As Pokemon In Pokemon
            If pm IsNot Nothing AndAlso pm IsNot except Then
                For i As Integer = 1 To 4
                    Dim move As Move = pm.SelMove(i)
                    If move IsNot Nothing AndAlso Not newArray.Contains(move.Identity) Then _
                        newArray.Add(move.Identity)
                Next
            End If
        Next
        Return newArray
    End Function

    Public Sub EndTurnCount()
        If Reflect Then reflectCounter -= 1
        If LightScreen Then lightScreenCounter -= 1
        If Safeguard Then safeguardCounter -= 1
        If Mist Then mistCounter -= 1
        If WithWind Then withWindCounter -= 1
        If LuckyChant Then luckyChantCounter -= 1

        If moor Then moorCounter -= 1
        If flameSea Then flameSeaCounter -= 1
        If rainbow Then rainbowCounter -= 1

        quickDefence = False
        wideDefence = False

        If Reflect AndAlso reflectCounter = 0 Then
            mReflect = False
            log.AddText(PlayerName & "队伍的反射盾消失了！")
        End If
        If LightScreen AndAlso lightScreenCounter = 0 Then
            mLightScreen = False
            log.AddText(PlayerName & "队伍的光墙消失了！")
        End If
        If Safeguard AndAlso safeguardCounter = 0 Then
            mSafeguard = False
            log.AddText(PlayerName & "队伍的保护罩消失了！")
        End If
        If Mist AndAlso mistCounter = 0 Then
            mMist = False
            log.AddText("覆盖在" & PlayerName & "队伍上的雾消失了！")
        End If
        If WithWind AndAlso withWindCounter = 0 Then
            mWithWind = False
            log.AddText(PlayerName & "队伍的顺风解除了！")
        End If
        If LuckyChant AndAlso luckyChantCounter = 0 Then
            mLuckyChant = False
            log.AddText(PlayerName & "队伍的祈福无效了！")
        End If
        For Each pm As Pokemon In SelectedPokemon
            If pm.BattleState.wished Then pm.BattleState.wishTurn += 1
            If pm.BattleState.futureAtk Then pm.BattleState.futureCounter += 1
        Next

        If moor AndAlso moorCounter = 0 Then
            moor = False
            log.AddText(PlayerName & "队伍场上的湿原消失了")
        End If
        If rainbow AndAlso rainbowCounter = 0 Then
            rainbow = False
            log.AddText(PlayerName & "队伍场上的彩虹消失了")
        End If
        If flameSea AndAlso flameSeaCounter = 0 Then
            flameSea = False
            log.AddText(PlayerName & "队伍场上的火海消失了")
        End If

        IsPokemonDiedLastTurn = IsPokemonDiedThisTurn
        IsPokemonDiedThisTurn = False
    End Sub

    Private Sub AddPokemonHandler(ByVal pm As Pokemon)
        AddHandler pm.Died, AddressOf PokemonDie
        AddHandler pm.HPChange, AddressOf PokemonHPChange
        AddHandler pm.StateChange, AddressOf PokemonStateChange
        AddHandler pm.ImageChange, AddressOf PokemonImageChange
    End Sub
    Private Sub RemovePokemonHandler(ByVal pm As Pokemon)
        RemoveHandler pm.Died, AddressOf PokemonDie
        RemoveHandler pm.HPChange, AddressOf PokemonHPChange
        RemoveHandler pm.StateChange, AddressOf PokemonStateChange
        RemoveHandler pm.ImageChange, AddressOf PokemonImageChange
    End Sub
    Private Sub PokemonDie(ByVal sender As Object, ByVal e As EventArgs)
        IsPokemonDiedThisTurn = True
        RaiseEvent PokemonDied(sender, EventArgs.Empty)
    End Sub
    Private Sub PokemonHPChange(ByVal sender As Object, ByVal e As EventArgs)
        RaiseEvent PokemonHPChanged(sender, EventArgs.Empty)
    End Sub
    Private Sub PokemonStateChange(ByVal sender As Object, ByVal e As EventArgs)
        RaiseEvent PokemonStateChanged(sender, EventArgs.Empty)
    End Sub
    Private Sub PokemonImageChange(ByVal sender As Object, ByVal e As System.EventArgs)
        RaiseEvent PokemonImageChanged(sender, e)
    End Sub

    Public ReadOnly Property LightScreen() As Boolean
        Get
            Return mLightScreen
        End Get
    End Property
    Public Function MakeLightScreen(ByVal maker As Pokemon) As Boolean
        If LightScreen Then Return False
        mLightScreen = True
        lightScreenCounter = 5
        If maker.Item = Item.光土 Then lightScreenCounter = 8
        log.AddText(PlayerName & "的队伍受到了光墙保护！")
        Return True
    End Function

    Public ReadOnly Property Reflect() As Boolean
        Get
            Return mReflect
        End Get
    End Property
    Public Function MakeReflect(ByVal maker As Pokemon) As Boolean
        If Reflect Then Return False
        mReflect = True
        reflectCounter = 5
        If maker.Item = Item.光土 Then reflectCounter = 8
        log.AddText(PlayerName & "的队伍受到了反射盾保护！")
        Return True
    End Function

    Public Sub BreakWall()
        If LightScreen Then mLightScreen = False : log.AddText(PlayerName & "队伍的光墙消失了！")
        If Reflect Then mReflect = False : log.AddText(PlayerName & "队伍的反射盾消失了！")
    End Sub

    Public ReadOnly Property Safeguard() As Boolean
        Get
            Return mSafeguard
        End Get
    End Property
    Public Function MakeSafeguard() As Boolean
        If Safeguard Then Return False
        mSafeguard = True
        safeguardCounter = 5
        log.AddText(PlayerName & "的队伍覆盖上了一层防护罩")
        Return True
    End Function

    Public ReadOnly Property Mist() As Boolean
        Get
            Return mMist
        End Get
    End Property
    Public Function MakeMist() As Boolean
        If Mist Then Return False
        mMist = True
        mistCounter = 5
        log.AddText(PlayerName & "的队伍隐藏在雾中")
        Return True
    End Function

    Public ReadOnly Property LuckyChant() As Boolean
        Get
            Return mLuckyChant
        End Get
    End Property
    Public Function MakeLuckyChant() As Boolean
        If LuckyChant Then Return False
        mLuckyChant = True
        luckyChantCounter = 5
        log.AddText(PlayerName & "的队伍5回合内不会被命中要害")
        Return True
    End Function

    Public Sub BreakMist()
        BreakWall()
        mSafeguard = False
        mMist = False
        spikesCounter = 0
        poisonSpikesCounter = 0
        hideRockCounter = 0
    End Sub

    Public ReadOnly Property WithWind() As Boolean
        Get
            Return mWithWind
        End Get
    End Property
    Public Function MakeWithWind() As Boolean
        If WithWind Then Return False
        mWithWind = True
        withWindCounter = Battle.WithwindTurn
        log.AddText(PlayerName & "队伍顺风而行！")
        Return True
    End Function

    Public Function MakeQuickDefence() As Boolean
        If Not quickDefence Then
            quickDefence = True
            Return True
        Else
            Return False
        End If
    End Function

    Public Function MakeWideDefence() As Boolean
        If Not wideDefence Then
            wideDefence = True
            Return True
        Else
            Return False
        End If
    End Function

    Public Function MakeMoor() As Boolean
        If Not moor Then
            moor = True
            moorCounter = 4
            log.AddText(PlayerName & "队伍的场地变成了湿原")
        Else
            Return False
        End If
    End Function

    Public Function MakeFlameSea() As Boolean
        If Not flameSea Then
            flameSea = True
            flameSeaCounter = 4
            log.AddText(PlayerName & "队伍的场上出现了彩虹")
        Else
            Return False
        End If
    End Function

    Public Function MakeRainbow() As Boolean
        If Not rainbow Then
            rainbow = True
            rainbowCounter = 4
            log.AddText(PlayerName & "队伍的场地变成了火海")
        Else
            Return False
        End If
    End Function


    Private _playerName As String
    Public ReadOnly Property PlayerName() As String
        Get
            If (String.IsNullOrEmpty(_playerName)) Then
                Dim str As New System.Text.StringBuilder
                For i As Integer = 0 To player.Length - 1
                    If i > 0 Then str.Append("&")
                    str.Append(player(i))
                Next
                _playerName = str.ToString
            End If
            Return _playerName
        End Get
    End Property
    Public Function GetPlayerName(ByVal pm As Pokemon) As String
        If Pokemon.IndexOf(pm) < 6 Then
            Return player(0)
        Else
            Return player(1)
        End If
    End Function

    Public Function SwapPokemonPosition() As Boolean
        If SelectedPokemon.Count = 1 Then
            Return False
        End If
        If SelectedPokemon.Exists(Function(pm) pm.HP = 0) Then
            Return False
        End If
        Dim temp As Pokemon = SelectedPokemon(0)
        SelectedPokemon(0) = SelectedPokemon(1)
        SelectedPokemon(1) = temp
        PokemonImageChange(SelectedPokemon(0), EventArgs.Empty)
        PokemonImageChange(SelectedPokemon(1), EventArgs.Empty)

        Return True
    End Function

    Public Property IsPokemonDiedThisTurn As Boolean
    Public Property IsPokemonDiedLastTurn As Boolean
End Class