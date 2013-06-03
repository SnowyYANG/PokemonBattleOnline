Imports PokemonBattle.BattleNetwork
Public Class ScreenDownPainter
    Private battle As Battle

    Private Image As Bitmap
    Private WithEvents picturebox As PictureBox

    Private mPage As SelectedPage
    Private mSelectedPMIndex As Integer = 1
    Private mSelectedIndex As Integer
    Private mClickable As Boolean
    Private mMoved As Boolean

    Private playerIndex As Integer

    Public Event PlayerMoved(ByVal move As PokemonBattleOnline.PlayerBattleMove)
    Public Event Escape As EventHandler
    Public Event Tie As EventHandler
    Public Event StartShake As EventHandler
    Public Event StopShake As EventHandler
    Public Event ShowPokemonInfo(ByVal pokemon As Pokemon)
    Public Event HidePokemonInfo As EventHandler

    Public Sub New(ByVal pictureboxValue As PictureBox, ByVal battleValue As Battle, _
        ByVal index As Integer)
        picturebox = pictureboxValue
        battle = battleValue
        playerIndex = index

        picturebox.Image = New Bitmap(256, 192)
        Image = CType(picturebox.Image, Bitmap)

        mPage = SelectedPage.Waiting
        Moved = True

    End Sub

    Public Sub Reset()
        picturebox.Image = New Bitmap(256, 192)
        Image = CType(picturebox.Image, Bitmap)
        Paint()
    End Sub
    Public Sub Paint()
        If picturebox.InvokeRequired Then
            picturebox.Invoke(New WorkDelegate(AddressOf Update))
        Else
            Try
                Update()
            Catch ex As Exception
                If picturebox.Image Is Nothing Then Reset()
            End Try
        End If
    End Sub
    Private Sub Update()
        Dim graph As Graphics = Graphics.FromImage(Image)
        Try
            Dim opponentTeam As Team = MyTeam.opponentTeam
            Select Case Page
                Case SelectedPage.Index
                    graph.DrawImageUnscaled(My.Resources.Index, 0, 0)
                    graph.DrawImageUnscaled(GetIndexImage(MyTeam, opponentTeam), 0, 0)

                    graph.DrawImageUnscaledAndClipped(SelectedPokemon.Icon, New Rectangle(71, 65, 32, 32))
                Case SelectedPage.SelectingMove
                    graph.DrawImageUnscaled(My.Resources.SelectMove, 0, 0)
                    For i As Integer = 1 To 4
                        If SelectedPokemon.SelMove(i) IsNot Nothing Then
                            graph.DrawImageUnscaled(SelectedPokemon.SelMove(i).Image, MovePoint(i - 1))
                        End If
                    Next

                    If SelectedIndex <> 0 Then graph.DrawImageUnscaled(My.Resources.MoveFrame, MovePoint(SelectedIndex - 1))
                Case SelectedPage.SelectingPM
                    graph.DrawImageUnscaled(My.Resources.SelectPokemon, 0, 0)
                    Dim start As Integer = 0
                    If MyTeam.Pokemon.Count > 6 AndAlso SelectedPokemon Is MyTeam.SelectedPokemon(1) Then start = 6
                    For i As Integer = 0 To 5
                        Dim pm As Pokemon
                        pm = MyTeam.Pokemon(i + start)
                        If pm Is Nothing Then Exit For
                        If i > 0 Then
                            graph.DrawImageUnscaled(My.Resources.Pokemon, PokemonPoint(i))
                        End If
                        graph.DrawImageUnscaled(GetPMImage(pm), PokemonPoint(i))
                    Next

                    If SelectedIndex <> 0 Then graph.DrawImageUnscaled(My.Resources.PokemonFrame, PokemonPoint(SelectedIndex - 1))
                Case SelectedPage.Waiting
                    graph.DrawImageUnscaled(My.Resources.back, 0, 0)
            End Select
        Finally
            graph.Dispose()
        End Try
        picturebox.Refresh()
    End Sub

    Public Property Page() As SelectedPage
        Get
            Return mPage
        End Get
        Set(ByVal value As SelectedPage)
            If mPage = SelectedPage.SelectingPM AndAlso value <> SelectedPage.SelectingPM Then
                RaiseEvent HidePokemonInfo(Nothing, EventArgs.Empty)
            End If
            mPage = value
            mSelectedIndex = 0
            Paint()
        End Set
    End Property
    Public Property SelectedIndex() As Integer
        Get
            Return mSelectedIndex
        End Get
        Private Set(ByVal value As Integer)
            If value >= 0 Then
                mSelectedIndex = value
                Paint()
            End If
        End Set
    End Property
    Public Property Moved() As Boolean
        Get
            Return mMoved
        End Get
        Set(ByVal value As Boolean)
            mMoved = value
            mClickable = Not mMoved
            If Moved = True Then
                RaiseEvent StopShake(Me, EventArgs.Empty)
                Page = SelectedPage.Waiting
            End If
        End Set
    End Property

    Private Sub picturebox_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) _
        Handles picturebox.MouseClick, picturebox.MouseDoubleClick
        If e.Button = MouseButtons.Left AndAlso mClickable Then
            HandleClick(e)
        End If
    End Sub
    Private Sub HandleClick(ByVal e As System.Windows.Forms.MouseEventArgs)
        Select Case mPage
            Case SelectedPage.Index
                Dim move As Rectangle = New Rectangle(23, 43, 211, 81)
                Dim pm As Rectangle = New Rectangle(179, 149, 74, 40)
                Dim escape As Rectangle = New Rectangle(91, 157, 73, 35)
                Dim tie As Rectangle = New Rectangle(2, 147, 74, 40)

                If move.Contains(e.Location) Then
                    If Not Struggle() Then
                        If SelectedPokemon.BattleState.encore Then
                            Dim target As TargetIndex = TargetIndex.DefaultTarget
                            Select Case SelectedPokemon.LastMove.Target
                                Case MoveTarget.随机
                                    target = TargetIndex.Random
                                Case MoveTarget.选一
                                    target = TargetIndex.Random
                                Case MoveTarget.队友
                                    target = TargetIndex.TeamFriend
                                Case MoveTarget.自身
                                    target = TargetIndex.Self
                                Case MoveTarget.任选
                                    target = TargetIndex.Self
                            End Select
                            SetPlayerMove(BattleMove.Attack, SelectedPokemon.lastMoveIndex, target)
                        Else
                            Page = SelectedPage.SelectingMove
                        End If
                    End If
                ElseIf pm.Contains(e.Location) Then
                    Page = SelectedPage.SelectingPM
                ElseIf escape.Contains(e.Location) Then
                    RaiseEvent Escape(Me, EventArgs.Empty)
                ElseIf tie.Contains(e.Location) Then
                    RaiseEvent Tie(Me, EventArgs.Empty)
                End If
            Case SelectedPage.SelectingMove
                Dim cancel As New Rectangle(10, 157, 236, 35)
                Dim rect1 As New Rectangle(MovePoint(0), My.Resources.Move.Size)
                Dim rect2 As New Rectangle(MovePoint(1), My.Resources.Move.Size)
                Dim rect3 As New Rectangle(MovePoint(2), My.Resources.Move.Size)
                Dim rect4 As New Rectangle(MovePoint(3), My.Resources.Move.Size)

                If cancel.Contains(e.Location) Then
                    Page = SelectedPage.Index
                    Return
                ElseIf rect1.Contains(e.Location) Then
                    If Move(1) Then Return
                ElseIf rect2.Contains(e.Location) Then
                    If Move(2) Then Return
                ElseIf rect3.Contains(e.Location) Then
                    If Move(3) Then Return
                ElseIf rect4.Contains(e.Location) Then
                    If Move(4) Then Return
                Else
                    Return
                End If
                If Not CanMove(SelectedIndex) Then SelectedIndex = 0
            Case SelectedPage.SelectingPM
                Dim cancel As New Rectangle(218, 155, 36, 35)
                Dim rect0 As New Rectangle(PokemonPoint(0), My.Resources.Pokemon.Size)
                Dim rect1 As New Rectangle(PokemonPoint(1), My.Resources.Pokemon.Size)
                Dim rect2 As New Rectangle(PokemonPoint(2), My.Resources.Pokemon.Size)
                Dim rect3 As New Rectangle(PokemonPoint(3), My.Resources.Pokemon.Size)
                Dim rect4 As New Rectangle(PokemonPoint(4), My.Resources.Pokemon.Size)
                Dim rect5 As New Rectangle(PokemonPoint(5), My.Resources.Pokemon.Size)

                Dim pm As Pokemon = Nothing
                If rect0.Contains(e.Location) Then
                    pm = GetPokemon(1)
                ElseIf rect1.Contains(e.Location) Then
                    pm = GetPokemon(2)
                ElseIf rect2.Contains(e.Location) Then
                    pm = GetPokemon(3)
                ElseIf rect3.Contains(e.Location) Then
                    pm = GetPokemon(4)
                ElseIf rect4.Contains(e.Location) Then
                    pm = GetPokemon(5)
                ElseIf rect5.Contains(e.Location) Then
                    pm = GetPokemon(6)
                End If
                If pm IsNot Nothing Then RaiseEvent ShowPokemonInfo(pm)

                If cancel.Contains(e.Location) Then
                    If MustChange() Then Return
                    Page = SelectedPage.Index
                ElseIf rect1.Contains(e.Location) Then
                    ChangePokemon(2)
                ElseIf rect2.Contains(e.Location) Then
                    ChangePokemon(3)
                ElseIf rect3.Contains(e.Location) Then
                    ChangePokemon(4)
                ElseIf rect4.Contains(e.Location) Then
                    ChangePokemon(5)
                ElseIf rect5.Contains(e.Location) Then
                    ChangePokemon(6)
                End If
        End Select
    End Sub

    Private Function CanChange() As Boolean
        If SelectedPokemon.BattleState.pass OrElse SelectedPokemon.HP = 0 Then Return True
        If SelectedPokemon.Item <> Item.绮丽外壳 Then
            If SelectedPokemon.BattleState.constraint Then
                MessageBox.Show("你的PM被束缚,现在无法交换", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            ElseIf SelectedPokemon.BattleState.ingrain Then
                MessageBox.Show("你的PM已经扎根,现在无法交换", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            Else
                For Each pm As Pokemon In MyTeam.opponentTeam.SelectedPokemon
                    If pm.HP = 0 Then Continue For
                    If (pm.SelTrait = Trait.踩影子 AndAlso SelectedPokemon.SelTrait <> Trait.踩影子) OrElse _
                        (pm.SelTrait = Trait.活地狱 AndAlso _
                        ((Not SelectedPokemon.IsAboveGroud _
                        AndAlso Not SelectedPokemon.HaveType("飞行")) OrElse battle.ground.Gravity)) OrElse _
                        (pm.SelTrait = Trait.磁力 AndAlso SelectedPokemon.HaveType("钢")) Then
                        MessageBox.Show("你的PM被对手的特性所束缚,现在无法交换", "提示", _
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return False
                    End If
                Next
            End If
        End If
        Return True
    End Function
    Private Function GetPokemon(ByVal index As Byte) As Pokemon
        Dim index2 As Integer = index
        If battle.Mode = BattleMode.Double_4P AndAlso playerIndex Mod 2 = 0 Then index2 += 6
        Return MyTeam.Pokemon(index2 - 1)
    End Function
    Private Sub ChangePokemon(ByVal index As Byte)
        Dim pm As Pokemon = GetPokemon(index)

        If MyTeam.SelectedPokemon.Contains(pm) Then Return
        If pm Is Nothing OrElse pm.HP = 0 Then Return

        If _swapPokemon.Contains(index) Then Return

        If SelectedIndex = index Then
            If Not CanChange() Then Return
            If SelectedPokemon.HP = 0 Then
                SetPlayerMove(BattleMove.DeathSwap, index, TargetIndex.DefaultTarget)
            ElseIf SelectedPokemon.BattleState.pass Then
                SetPlayerMove(BattleMove.PassSwap, index, TargetIndex.DefaultTarget)
            Else
                SetPlayerMove(BattleMove.SwapPokemon, index, TargetIndex.DefaultTarget)
            End If
        Else
            SelectedIndex = index
        End If
    End Sub
    Private Function MustChange() As Boolean
        Return SelectedPokemon.HP = 0 OrElse SelectedPokemon.BattleState.pass
    End Function

    Private Function CanMove(ByVal index As Integer) As Boolean
        If SelectedPokemon.BattleState.taunt AndAlso SelectedPokemon.SelMove(index).MoveType = MoveType.其他 Then
            MessageBox.Show(SelectedPokemon.Nickname & "被挑拨不能使用辅助技能", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
        End If
        If SelectedPokemon.BattleState.torment AndAlso SelectedPokemon.lastMoveIndex = index Then
            MessageBox.Show(SelectedPokemon.Nickname & _
                "被寻衅不能连续使用相同技能", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
        End If
        If (SelectedPokemon.UsedItem = Item.专爱头巾 OrElse SelectedPokemon.UsedItem = Item.专爱围巾 _
            OrElse SelectedPokemon.UsedItem = Item.专爱眼镜) AndAlso SelectedPokemon.lastMoveIndex <> index Then
            MessageBox.Show(SelectedPokemon.Nickname & "携带了专爱道具只能使用相同技能", "提示", _
                MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
        End If
        If battle.ground.Gravity AndAlso (SelectedPokemon.SelMove(index).Effect = MoveEffect.电磁浮游 OrElse _
                                          SelectedPokemon.SelMove(index).AddEff2 = MoveAdditionalEffect.飞天后攻击 OrElse SelectedPokemon.SelMove(index).AddEff2 = MoveAdditionalEffect.跳起后攻击) Then
            MessageBox.Show("此技能重力下无法使用", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
        End If
        For Each pm As Pokemon In MyTeam.opponentTeam.SelectedPokemon
            If (pm.BattleState.imprison AndAlso pm.HaveMove(SelectedPokemon.SelMoveName(index))) _
                OrElse SelectedPokemon.BattleState.disableIndex = index Then
                MessageBox.Show("此技能被封印不能使用", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If
        Next
        Return True
    End Function
    Private Function Move(ByVal index As Byte) As Boolean
        If SelectedPokemon.SelMove(index) Is Nothing OrElse SelectedPokemon.SelMove(index).PP = 0 Then Return True

        If SelectedIndex = index Then
            Dim target As TargetIndex
            If GetTarget(index, target) Then SetPlayerMove(BattleMove.Attack, index, target)
            Return True
        Else
            SelectedIndex = index
            Return False
        End If
    End Function
    Private Function Struggle() As Boolean
        If SelectedPokemon.NoMoveCanUse Then
            Dim result As DialogResult = MessageBox.Show _
                ("你的PM没有可使用的技能,是否使用拼命", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
            If result = Windows.Forms.DialogResult.Yes Then
                Dim target As TargetIndex
                If GetTarget(battle.StruggleIndex, target) Then
                    SetPlayerMove(BattleMove.Attack, battle.StruggleIndex, target)
                End If
            End If
            Return True
        Else
            Return False
        End If
    End Function
    Private Function GetTarget(ByVal index As Integer, ByRef target As TargetIndex) As Boolean
        If index = battle.StruggleIndex OrElse SelectedPokemon.SelMove(index).Target = MoveTarget.选一 _
            OrElse (SelectedPokemon.SelMove(index).Target = MoveTarget.任选 AndAlso MyTeam.SelectedPokemon.Count > 1) Then
            If MyTeam.SelectedPokemon.Count = 1 Then target = TargetIndex.Opponent1 : Return True

            Dim selfEff As Boolean
            If index <> battle.StruggleIndex Then selfEff = SelectedPokemon.SelMove(index).Target = MoveTarget.任选
            Dim targetDialog As New SelectTargetForm(MyTeam, MyTeam.opponentTeam, SelectedPokemon, selfEff)
            Dim result As DialogResult = targetDialog.ShowDialog
            If result = DialogResult.OK Then
                target = targetDialog.Target
                Return True
            Else
                Return False
            End If
        Else
            Select Case SelectedPokemon.SelMove(index).Target
                Case MoveTarget.随机
                    target = TargetIndex.Random
                Case MoveTarget.队友
                    target = TargetIndex.TeamFriend
                Case MoveTarget.自身
                    target = TargetIndex.Self
                Case MoveTarget.任选
                    target = TargetIndex.Self
                Case Else
                    target = TargetIndex.DefaultTarget
            End Select
            Return True
        End If
    End Function
    Private Sub SetPlayerMove(ByVal moveValue As BattleMove, ByVal index As Byte, ByVal target As TargetIndex)
        Dim newMove As New PlayerBattleMove(SelectedPokemonIndex, moveValue, index, target)

        _waitMoveCount -= 1
        If _waitMoveCount = 0 Then
            Moved = True
            _swapPokemon.Clear()
        Else
            If moveValue = BattleMove.DeathSwap OrElse moveValue = BattleMove.SwapPokemon Then
                _swapPokemon.Add(index)
            End If
            mSelectedPMIndex += 1
            If moveValue = BattleMove.DeathSwap Then
                Page = SelectedPage.SelectingPM
            Else
                Page = SelectedPage.Index
            End If

        End If
        RaiseEvent PlayerMoved(newMove)
        If Not Moved Then
            If Not SelectedPokemon.CanChooseMove Then
                SetPlayerMove(BattleMove.Attack, 0, TargetIndex.DefaultTarget)
            Else
                RaiseEvent StartShake(Me, EventArgs.Empty)
            End If
        End If
    End Sub

    Private _selectedPokemons As List(Of Pokemon)
    Private ReadOnly Property SelectedPokemonIndex() As PokemonIndex
        Get
            Return battle.GetPokemonIndex(SelectedPokemon)
        End Get
    End Property
    Public ReadOnly Property SelectedPokemon() As Pokemon
        Get
            Return _selectedPokemons(mSelectedPMIndex - 1)
        End Get
    End Property

    Private _waitMoveCount As Integer
    Private _swapPokemon As List(Of Integer) = New List(Of Integer)

    Public Sub SetTurnResult(ByVal result As TurnResult, ByVal pokemons As List(Of Pokemon))
        _selectedPokemons = pokemons
        If battle.Ended Then
            battle = Nothing
            Return
        End If
        If _selectedPokemons.Count = 0 Then Return
        mSelectedPMIndex = 1
        mSelectedIndex = 0
        Moved = False
        If Not result.TurnComplete Then
            If result.DeadPokemons.Count > 0 Then
                Dim deadList As List(Of Pokemon) = result.DeadPokemons.FindAll(Function(pm As Pokemon) _selectedPokemons.Contains(pm))
                If deadList.Count > 0 Then
                    If Not SelectedPokemon.HP = 0 Then
                        mSelectedPMIndex = 2
                    End If
                    _waitMoveCount = Math.Min(deadList.Count, _selectedPokemons.Count)
                    Page = SelectedPage.SelectingPM
                Else
                    Moved = True
                End If
            Else
                If _selectedPokemons.Contains(result.PassPokemon) Then
                    If Not SelectedPokemon.BattleState.pass Then
                        mSelectedPMIndex = 2
                    End If
                    Page = SelectedPage.SelectingPM
                    _waitMoveCount = 1
                    RaiseEvent StartShake(Me, EventArgs.Empty)
                Else
                    Moved = True
                End If
            End If
        Else
            _waitMoveCount = _selectedPokemons.Count
            Page = SelectedPage.Index
            If Not SelectedPokemon.CanChooseMove Then
                SetPlayerMove(BattleMove.Attack, 0, TargetIndex.DefaultTarget)
            Else
                RaiseEvent StartShake(Me, EventArgs.Empty)
            End If
        End If

    End Sub

    Public ReadOnly Property MyTeam() As Team
        Get
            Return battle.GetTeam(playerIndex)
        End Get
    End Property

    Public Shared MovePoint As Point() = {New Point(2, 26), New Point(130, 26), New Point(2, 90), New Point(130, 90)}
    Public Shared PokemonPoint As Point() = _
        {New Point(1, 2), New Point(129, 9), New Point(1, 50), New Point(129, 57), New Point(1, 98), New Point(129, 105)}
End Class
