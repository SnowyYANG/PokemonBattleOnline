Imports PokemonBattle.BattleNetwork
<Serializable()> _
Public Class PlayerBattleMove
    Private _pokemon As Pokemon
    Private _target As List(Of Pokemon)

    Private _move As BattleMove
    Private _moveIndex As Byte
    Private mPMIndex As PokemonIndex
    Private mTargetIndex As TargetIndex

    Private _priority As Double = -1
    Private _randomPriority As Integer

    Public Sub New(ByVal move As PlayerMove)
        _move = move.Move
        _moveIndex = move.MoveIndex
        mTargetIndex = move.Target
        mPMIndex = move.Pokemon
    End Sub
    Public Sub New(ByVal pmValue As PokemonIndex, ByVal moveValue As BattleMove, ByVal index As Byte, _
        ByVal targetValue As TargetIndex)
        mPMIndex = pmValue
        _move = moveValue
        _moveIndex = index
        mTargetIndex = targetValue
    End Sub

    Public ReadOnly Property PMIndex() As PokemonIndex
        Get
            Return mPMIndex
        End Get
    End Property
    Public ReadOnly Property TargetIndex() As TargetIndex
        Get
            Return mTargetIndex
        End Get
    End Property
    Public ReadOnly Property PM() As Pokemon
        Get
            Return _pokemon
        End Get
    End Property
    Public ReadOnly Property Move() As BattleMove
        Get
            Return _move
        End Get
    End Property
    Public ReadOnly Property MoveIndex() As Byte
        Get
            Return _moveIndex
        End Get
    End Property
    Public ReadOnly Property Target() As List(Of Pokemon)
        Get
            Return _target
        End Get
    End Property
    Public Sub SetTarget(ByVal battle As Battle, ByVal random As Random)
        If _move <> BattleMove.Attack Then Return
        If Not _pokemon.CanChooseMove Then
            If _pokemon.BattleState.nextMove Is Nothing Then
                Return
            ElseIf _pokemon.BattleState.nextMove.Target = MoveTarget.随机 Then
                mTargetIndex = TargetIndex.Random
            Else
                mTargetIndex = _pokemon.lastTarget
            End If
        End If
        _pokemon.lastTarget = mTargetIndex
        _target = GetCurrentTarget(battle, random)
    End Sub
    Public Sub SetPM(ByVal team1 As Team, ByVal team2 As Team)
        Select Case mPMIndex
            Case PokemonIndex.Pokemon1OfTeam1
                _pokemon = team1.SelectedPokemon(0)
            Case PokemonIndex.Pokemon1OfTeam2
                _pokemon = team2.SelectedPokemon(0)
            Case PokemonIndex.Pokemon2OfTeam1
                _pokemon = team1.SelectedPokemon(1)
            Case PokemonIndex.Pokemon2OfTeam2
                _pokemon = team2.SelectedPokemon(1)
        End Select
    End Sub

    Public Function GetCurrentTarget(ByVal battle As Battle, ByVal random As Random) As List(Of Pokemon)
        Dim selMove As Move
        If _moveIndex = 0 Then
            If Not _pokemon.CanChooseMove Then
                selMove = _pokemon.BattleState.nextMove
            Else
                selMove = Nothing
            End If
        ElseIf _moveIndex = battle.StruggleIndex Then
            selMove = Nothing
        Else
            selMove = _pokemon.SelMove(_moveIndex)
        End If

        Return battle.GetTargetsFromIndex(_pokemon, selMove, mTargetIndex)
    End Function

    Public ReadOnly Property Pursuit() As Boolean
        Get
            If Not _pokemon.CanChooseMove Then Return False
            If _move = BattleMove.Attack AndAlso _moveIndex <> Battle.StruggleIndex _
                AndAlso _pokemon.SelMove(_moveIndex).AddEff2 = MoveAdditionalEffect.追击 Then
                Return True
            End If
            Return False
        End Get
    End Property
    Public ReadOnly Property FocusPunch() As Boolean
        Get
            If Not _pokemon.CanChooseMove Then Return False
            If _move = BattleMove.Attack AndAlso _moveIndex <> Battle.StruggleIndex _
                AndAlso _pokemon.SelMove(_moveIndex).Name = "气合拳" AndAlso _
                Not (_pokemon.SelTrait = Trait.偷懒 AndAlso _pokemon.BattleState.traitRaised) _
                AndAlso Not _pokemon.State = PokemonState.Sleep Then
                Return True
            End If
        End Get
    End Property
    Public ReadOnly Property Struggle() As Boolean
        Get
            If Not _pokemon.CanChooseMove Then Return False
            Return _move = BattleMove.Attack AndAlso _moveIndex = Battle.StruggleIndex
        End Get
    End Property
    Public ReadOnly Property AttackMove() As Boolean
        Get
            If _move = BattleMove.Attack AndAlso _
                (_moveIndex = 0 OrElse _moveIndex = 5 OrElse _pokemon.SelMove(_moveIndex).MoveType <> MoveType.其他) Then
                Return True
            End If
        End Get
    End Property

    Public Sub SetPriority(ByVal random As Random)
        _priority = Battle.CountAtkPriority(_pokemon, _moveIndex)

        If _pokemon.Item = Item.先制之爪 Then
            If random.NextDouble < 0.2 Then _priority += 0.5
        ElseIf _pokemon.BattleState.quickBerry Then
            _priority += 0.5
            _pokemon.BattleState.quickBerry = False
        ElseIf _pokemon.Item = Item.后攻尾 OrElse _pokemon.Item = Item.满腹香炉 Then
            _priority -= 0.5
        End If
    End Sub
    Public Function GetPriority() As Double
        Return _priority
    End Function
    Public Property RandomPriority() As Integer
        Get
            Return _randomPriority
        End Get
        Set(ByVal value As Integer)
            _randomPriority = value
        End Set
    End Property
End Class
