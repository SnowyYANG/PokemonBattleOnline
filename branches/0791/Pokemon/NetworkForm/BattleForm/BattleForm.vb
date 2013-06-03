Imports PokemonBattle.BattleNetwork
Public Class BattleForm
    Protected imgtxt As ImgTxt

    Private Delegate Sub UpdateTextDelegate(ByVal txt As String)
    Private Delegate Sub UpdateProgressDelegate(ByVal value As Integer)

    Private _battle As Battle
    Protected _myTeamData As TeamData

    Protected _identity As String
    Protected _myPosition As Byte
    Protected _battleMode As BattleMode
    Private _battleInfo As BattleInfo
    Private _locker As Object = New Object()

    Protected ScreenUp As ScreenUpPainter
    Protected WithEvents ScreenDown As ScreenDownPainter
    Public Event OnBattleStarted As NetworkLib.VoidFunctionDelegate
    Public Sub New(ByVal identity As String, ByVal indexValue As Byte, ByVal teamInfo As TeamData, _
        ByVal mode As BattleMode)

        ' 此调用是 Windows 窗体设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。 
        Me.Icon = My.Resources.PokemonBall
        _myPosition = indexValue
        _battleMode = mode
        teamInfo.CheckData()
        _myTeamData = teamInfo.Clone()
        _identity = identity

        imgtxt = New ImgTxt(Convert.ToInt32(My.Settings.LogLineCount))
        _battle = New Battle(imgtxt, mode)
        SetPlayer(_myPosition, _identity)
        Initialize()
    End Sub

    Private Sub Initialize()
        If Not InvokeRequired Then
            Dim logLine As Integer = Convert.ToInt32(My.Settings.LogLineCount)
            If logLine > 6 Then
                picText.Size = New Size(256, 7 + logLine * 13)
            End If
            If My.Settings.LogPosition = 0 Then
                If logLine > 6 Then Me.Size = New Size(394, 439 + logLine * 13)
            Else
                Me.Size = New Size(650, 432)
                picText.Location = New Point(376, 45)
            End If
        Else
            Invoke(New WorkDelegate(AddressOf Initialize))
        End If
    End Sub
    Protected Sub SetCaption(ByVal txt As String)
        If Not InvokeRequired Then
            Text = txt
        Else
            Invoke(New UpdateTextDelegate(AddressOf SetCaption), txt)
        End If
    End Sub
    Protected Sub SetProgressText(ByVal txt As String)
        If Not InvokeRequired Then
            lblProgress.Text = txt
        Else
            Invoke(New UpdateTextDelegate(AddressOf SetProgressText), txt)
        End If
    End Sub
    Protected Sub SetProgress(ByVal value As Integer)
        If Not InvokeRequired Then
            pgbSendData.Value = value
        Else
            Invoke(New UpdateProgressDelegate(AddressOf SetProgress), value)
        End If
    End Sub
    Protected Sub HideProgress()
        If Not InvokeRequired Then
            lblProgress.Hide()
            pgbSendData.Hide()
        Else
            Invoke(New WorkDelegate(AddressOf HideProgress))
        End If
    End Sub

    Protected Sub PlayerMoved(ByVal moveValue As PlayerBattleMove) Handles ScreenDown.PlayerMoved
        Dim move As New PlayerMove()
        move.Move = moveValue.Move
        move.MoveIndex = moveValue.MoveIndex
        move.Pokemon = moveValue.PMIndex
        move.Target = moveValue.TargetIndex
        move.Player = _identity
        'PlayerMoved(move)
        ThreadPool.QueueUserWorkItem(AddressOf RaiseMoved, move)
    End Sub
    Private Sub RaiseMoved(ByVal obj As Object)
        PlayerMoved(CType(obj, PlayerMove))
    End Sub
    Protected Overridable Sub PlayerMoved(ByVal moveValue As PlayerMove)

    End Sub

    Public Sub SetMove(ByVal move As PlayerMove)
        SyncLock _locker
            _battle.SetMove(move)

            If (_battle.MoveReady) Then
                Dim result As TurnResult = _battle.NextResult()
                ScreenDown.SetTurnResult(result, _battle.GetPokemons(_myPosition))
                If _battle.Ended Then OnBattleEnd()
            End If
            If ScreenDown.Moved = False Then
                OnWaitMove()
            End If
        End SyncLock
    End Sub
    Protected Overridable Sub OnWaitMove()

    End Sub

    Public Sub SetRandomSeed(ByVal seed As Integer)
        _battle.SetSeed(seed)
    End Sub
    Public Sub SetRules(ByVal rules As List(Of BattleRule))
        _battle.SetRules(rules)
    End Sub
    Public Sub SetPlayer(ByVal position As Byte, ByVal identity As String)
        _battle.SetPlayer(position, identity)
    End Sub
    Public ReadOnly Property PlayerReady() As Boolean
        Get
            Return _battle.PlayerReady()
        End Get
    End Property

    Public Sub StartBattle(ByVal teamInfo As Dictionary(Of Byte, TeamData))
        _battle.SetTeams(teamInfo)
        _battle.InitializeBattle()
        RaiseEvent OnBattleStarted()

        For Each key As Byte In teamInfo.Keys
            If key <> _myPosition Then PokemonCount(teamInfo(key))
        Next

        Dim result As TurnResult = _battle.StartBattle()

        AddHandler Me.KeyDown, AddressOf KeyDownTask
        ScreenUp = New ScreenUpPainter(picScreenUp, picText, _battle.ground.Terrain, imgtxt, MyTeam, OpponentTeam, _battleMode)
        ScreenDown = New ScreenDownPainter(picScreenDown, _battle, _myPosition)
        AddHandler imgtxt.Roll, AddressOf imgtxt_Roll
        AddHandler imgtxt.UpdateScreen, AddressOf imgtxt_UpdateScreen
        SetCaption(_battle.GetCaption)
        ScreenUp.Paint()

        ScreenDown.SetTurnResult(result, _battle.GetPokemons(_myPosition))
        OnWaitMove()
    End Sub
    Protected Function GetSnapshot() As BattleSnapshot
        Dim snapshot As New BattleSnapshot()
        snapshot.NewText = imgtxt.lastText
        snapshot.TextColor = imgtxt.lastColor.ToArgb
        snapshot.Pokemons = New PokemonSnapshotSequence
        Dim pokemonList As New List(Of PokemonSnapshot)
        For Each pm As Pokemon In _battle.team1.SelectedPokemon
            pokemonList.Add(GetPokemonSnap(pm))
        Next
        For Each pm As Pokemon In _battle.team2.SelectedPokemon
            pokemonList.Add(GetPokemonSnap(pm))
        Next
        snapshot.Pokemons.Elements.AddRange(pokemonList)
        Return snapshot
    End Function
    Private Function GetPokemonSnap(ByVal pm As Pokemon) As PokemonSnapshot
        Dim pmSnap As PokemonSnapshot = New PokemonSnapshot()
        pmSnap.Identity = pm.Identity
        If pm.BattleState.transfrom OrElse pm.BattleState.shapeShift Then pmSnap.Identity = pm.BattleState.transTemp.Identity
        pmSnap.Hp = Convert.ToInt16(pm.HP)
        pmSnap.MaxHp = Convert.ToInt16(pm.MAXHP)
        pmSnap.Nickname = pm.Nickname
        pmSnap.State = CByte(pm.State)
        pmSnap.Substituded = pm.BattleState.substituted
        pmSnap.Hid = pm.BattleState.Hide
        pmSnap.Lv = pm.LV
        pmSnap.Gender = CByte(pm.Gender)
        Return pmSnap
    End Function
    Protected Function GetBattleInfo() As BattleInfo
        If Not _battle.BattleReady Then Return Nothing
        If _battleInfo Is Nothing Then
            _battleInfo = New BattleInfo
            _battleInfo.Mode = _battleMode
            _battleInfo.Caption = Text
            _battleInfo.CustomDataHash = _myTeamData.CustomInfo.DataHash
            _battleInfo.Terrain = _battle.ground.Terrain
        End If
        Return _battleInfo
    End Function

    Public ReadOnly Property MyTeam() As Team
        Get
            If _battle Is Nothing Then Return Nothing
            Select Case _myPosition
                Case 1
                    Return _battle.team1
                Case 2
                    If _battleMode = BattleMode.Double_4P Then
                        Return _battle.team1
                    Else
                        Return _battle.team2
                    End If
                Case Else
                    Return _battle.team2
            End Select
            Return Nothing
        End Get
    End Property
    Public ReadOnly Property OpponentTeam() As Team
        Get
            If _battle Is Nothing Then Return Nothing
            Select Case _myPosition
                Case 1
                    Return _battle.team2
                Case 2
                    If _battleMode = BattleMode.Double_4P Then
                        Return _battle.team2
                    Else
                        Return _battle.team1
                    End If
                Case Else
                    Return _battle.team1
            End Select
            Return Nothing
        End Get
    End Property

    Private Sub imgtxt_Roll(ByVal sender As Object, ByVal e As System.EventArgs)
        ScreenUp.PaintImgTxt()
    End Sub
    Private Sub imgtxt_UpdateScreen(ByVal sender As Object, ByVal e As System.EventArgs)
        ScreenUp.PaintImgTxt()
    End Sub
    Private Sub KeyDownTask(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.PageUp Then
            imgtxt.RollUp()
        ElseIf e.KeyCode = Keys.PageDown Then
            imgtxt.RollDown()
        End If
    End Sub

#Region "ScreenDownEvent"

    Private Sub ScreenDown_Escape(ByVal sender As Object, ByVal e As System.EventArgs) Handles ScreenDown.Escape
        Close()
    End Sub

    Private Sub ScreenDown_Tie(ByVal sender As Object, ByVal e As System.EventArgs) Handles ScreenDown.Tie
        SendTie()
    End Sub

    Private Sub ScreenDown_StartShake(ByVal sender As Object, ByVal e As System.EventArgs) Handles ScreenDown.StartShake
        ScreenUp.StartShake(ScreenDown.SelectedPokemon)
    End Sub

    Private Sub ScreenDown_StopShake(ByVal sender As Object, ByVal e As System.EventArgs) Handles ScreenDown.StopShake
        ScreenUp.StopShake()
    End Sub

    Private Sub ScreenDown_ShowPokemonInfo(ByVal pokemon As Pokemon) Handles ScreenDown.ShowPokemonInfo
        Dim text As New System.Text.StringBuilder()
        text.AppendLine(pokemon.Nickname)
        text.AppendLine(String.Format("LV : {0}", pokemon.LV))
        text.AppendLine(String.Format("HP : {0}/{1}", pokemon.HP, pokemon.MAXHP))
        text.AppendLine("物攻 : " & pokemon.AttackValue)
        text.AppendLine("物防 : " & pokemon.DefenceValue)
        text.AppendLine("特攻 : " & pokemon.SpAttackValue)
        text.AppendLine("特防 : " & pokemon.SpDefenceValue)
        text.AppendLine("速度 : " & pokemon.SpeedValue)
        text.AppendLine(" - " & pokemon.SelMoveName(1))
        text.AppendLine(" - " & pokemon.SelMoveName(2))
        text.AppendLine(" - " & pokemon.SelMoveName(3))
        text.AppendLine(" - " & pokemon.SelMoveName(4))
        text.AppendLine("道具 : " & pokemon.Item.ToString)
        text.AppendLine("特性 : " & pokemon.SelTrait.ToString)
        PokemonLabel.Text = text.ToString()
        PokemonIcon.Image = pokemon.Icon
    End Sub

    Private Sub ScreenDown_HidePokemonInfo(ByVal sender As Object, ByVal e As System.EventArgs) Handles ScreenDown.HidePokemonInfo
        PokemonLabel.Text = ""
        PokemonLabel.Refresh()
        PokemonIcon.Image = Nothing
        PokemonIcon.Refresh()
    End Sub

#End Region

    Public Sub PlayerExit(ByVal identity As String)
        _battle.ExitGame(identity)
        BattleEnd()
        ScreenDown.Page = SelectedPage.Waiting
    End Sub
    Private Sub SendTie()
        Dim result As DialogResult = MessageBox.Show("即将想对手发送平手请求，是否确定？", "提示", _
            MessageBoxButtons.YesNo, MessageBoxIcon.Information)
        If result = Windows.Forms.DialogResult.Yes Then
            ScreenDown.Moved = True
            ScreenDown.Page = SelectedPage.Waiting
            SendTieMessage(TieMessage.TieRequest)
        End If
    End Sub
    Public Sub Tied()
        _battle.Tie()
        BattleEnd()
    End Sub
    Public Sub GetTie(ByVal player As String, ByVal message As TieMessage)
        If message = TieMessage.TieRequest Then
            Dim result As DialogResult = ShowInformation(Me, player & "发出平手请求,是否同意", "提示", _
                MessageBoxButtons.YesNo, MessageBoxIcon.Information)
            If result = Windows.Forms.DialogResult.Yes Then
                SendTieMessage(TieMessage.AgreeTie)
            ElseIf result = Windows.Forms.DialogResult.No Then
                SendTieMessage(TieMessage.RefuseTie)
            End If
        ElseIf message = TieMessage.RefuseTie Then
            ShowInformation(Me, "平手请求被" & player & "拒绝,对战继续", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ScreenDown.Moved = False
            ScreenDown.Page = SelectedPage.Index
        ElseIf message = TieMessage.AgreeTie Then
            Tied()
        ElseIf message = TieMessage.Fail Then
            ShowInformation(Me, "当前无法提出平手请求", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ScreenDown.Moved = False
            ScreenDown.Page = SelectedPage.Index
        End If
    End Sub
    Protected Overridable Sub SendTieMessage(ByVal message As TieMessage)

    End Sub

    Private Sub BattleEnd()
        OnBattleEnd()
        ScreenDown.Moved = True
    End Sub
    Protected Overridable Sub OnBattleEnd()
    End Sub
    Protected Sub OnError(ByVal message As String)
        ShowInformation(Me, message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Invoke(New NetworkLib.VoidFunctionDelegate(AddressOf Close))
    End Sub

    Private Sub BattleForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        OnEscape(e)
    End Sub
    Protected Overridable Sub OnEscape(ByVal e As System.Windows.Forms.FormClosingEventArgs)
        If e.CloseReason = CloseReason.UserClosing AndAlso _battle IsNot Nothing Then
            Dim result As DialogResult = MessageBox.Show("是否退出对战?", "提示", _
                MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            If result = Windows.Forms.DialogResult.Cancel Then
                e.Cancel = True
                Return
            End If
        End If
        If imgtxt IsNot Nothing Then
            imgtxt.SaveLog(_battle._players)
            RemoveHandler imgtxt.Roll, AddressOf imgtxt_Roll
            RemoveHandler imgtxt.UpdateScreen, AddressOf imgtxt_UpdateScreen
            If _battle IsNot Nothing AndAlso _battle.Replay IsNot Nothing Then _battle.Replay.SaveReplay()
        End If
        If ScreenUp IsNot Nothing Then ScreenUp.Dispose()
        RemoveHandler Me.KeyDown, AddressOf KeyDownTask
    End Sub

    Private Sub PokemonCount(ByVal team As TeamData)
        For Each pm As PokemonCustomInfo In team.Pokemons
            PokemonCounter.Count(pm.Identity)
        Next
    End Sub

    Private Sub RepaintMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RepaintMenu.Click
        ScreenUp.Reset()
        ScreenDown.Reset()
    End Sub
End Class