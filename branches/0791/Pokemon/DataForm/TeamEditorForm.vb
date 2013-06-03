Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Text.RegularExpressions
Imports PokemonBattle.PokemonData
Friend Class TeamEditorForm
    Private _gameData As GameData
    Private _team As TeamData
    Private tempTeam As TeamData

    Private boxes As List(Of List(Of PokemonCustomInfo))

    Private selInfo As PokemonCustomInfo
    Private MoveSorter As ListSorter

    Private updating As Boolean
    Private lstUpdating As Boolean

    Public Sub New(ByVal teamData As TeamData, ByVal gameDataValue As GameData)

        ' 此调用是 Windows 窗体设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        Me.Text = "队伍编辑器"
        Me.Icon = My.Resources.PokemonBall
        UpdateData(gameDataValue)
        For Each data As PokemonType In BattleData.GetAllTypes()
            If data.Image IsNot Nothing Then imgTypes.Images.Add(data.Name, data.Image)
        Next
        MoveSorter = New ListSorter()
        MoveList.ListViewItemSorter = MoveSorter
        If teamData Is Nothing Then
            NewTeam()
        Else
            OpenTeam(teamData, True)
        End If
    End Sub

    Private Sub FrmTeamEditor_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ExpertButton.Enabled = False
        ItemCombo.Enabled = False
        LVNumberic.Enabled = False
        NickNameText.Enabled = False
        GetBox()
        HideBox()
    End Sub

    Private Sub FrmTeamEditor_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If e.CloseReason <> CloseReason.MdiFormClosing Then
            e.Cancel = True
        End If
        If _team.Pokemons(0).Identity = 0 Then
            MessageBox.Show("首发PM不得为空！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        For i As Integer = 0 To 5
            If _team.Pokemons(i).Identity <> 0 AndAlso _team.Pokemons(i).SelectedMoves(0) = MoveData.InvalidId Then
                MessageBox.Show(_team.Pokemons(i).Nickname & "没有技能！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
        Next
        If Not CloseTeam() Then Return
        SaveBox()
        PokemonList.SelectedIndices.Clear()
        MotherForm.Team = _team.Clone()
        Hide()
    End Sub

#Region "BattleData"
    Public Sub UpdateData(ByVal dataValue As GameData)
        _gameData = dataValue
        PokemonCombo.Items.Clear()
        For Each pm As PokemonData In BattleData.GetAllPokemons
            If Not BattleData.PokemonIsRemoved(pm.Identity) Then PokemonCombo.Items.Add(New IdentityNamePair(pm.Name, pm.Identity))
        Next
        CheckTeam()
    End Sub

    Private Sub CheckTeam()
        If _team IsNot Nothing Then
            _team.CustomInfo.DataName = BattleData.CustomData.DataName
            _team.CustomInfo.DataHash = BattleData.CustomData.DataHash
            For i As Integer = 0 To _team.Pokemons.Length - 1
                If _team.Pokemons(i).Identity = 0 Then Continue For
                If Not BattleData.CheckPokemon(_team.Pokemons(i)) Then _team.Pokemons(i) = New PokemonCustomInfo
            Next
            Dim emptyCount As Integer
            For i As Integer = 0 To _team.Pokemons.Length - 1
                If _team.Pokemons(i).Identity = 0 Then
                    emptyCount += 1
                Else
                    If emptyCount <> 0 Then SwapPM(i, i - emptyCount, False)
                End If
            Next
            PokemonList.SelectedIndices.Clear()
            UpdateTeam()
            ClearText()
        End If
    End Sub
#End Region

#Region "Open/Close Method"
    Public Shared Function OpenTeam(ByVal path As String) As TeamData
        Dim file As New FileStream(path, FileMode.Open, FileAccess.Read)
        Try
            Using ms As MemoryStream = Decrypt(file)
                ms.Seek(0, SeekOrigin.Begin)

                My.Settings.teamPath = path
                If My.Settings.history.IndexOf(path) = -1 Then My.Settings.history.Add(path)

                Return TeamData.FormStream(ms)
            End Using
        Catch ex As Exception
            My.Settings.teamPath = ""
            MessageBox.Show("队伍打开错误", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            file.Close()
        End Try
        Return Nothing
    End Function

    Public Sub SaveTeam()
        Try
            Using ms As New MemoryStream
                _team.Save(ms)
                Encrypt(ms, My.Settings.teamPath)
            End Using
            tempTeam = _team.Clone()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Sub OpenTeam(ByVal teamValue As TeamData, ByVal constructing As Boolean)
        If teamValue Is Nothing Then Return
        If Not constructing AndAlso Not MotherForm.Battling AndAlso teamValue.CustomInfo.DataHash <> BattleData.CustomData.DataHash Then

            Dim result As DialogResult = _
                MessageBox.Show("你所打开的队伍所使用的自定义数据与当前使用的自定义数据不同,是否导入该自定义数据?", _
                "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = Windows.Forms.DialogResult.Yes Then
                _team = Nothing
                If Not MotherForm.LoadCustom(teamValue.CustomInfo.DataName & ".pcd") Then
                    NewTeam()
                    Return
                End If
            ElseIf result = Windows.Forms.DialogResult.No Then
                Return
            End If

        End If
        _team = teamValue
        If _team Is Nothing Then
            _team = New TeamData()
            _team.CustomInfo.DataName = BattleData.CustomData.DataName
            _team.CustomInfo.DataHash = BattleData.CustomData.DataHash
        End If
        tempTeam = _team.Clone()
        UpdateTeam()
        CheckTeam()
        ClearText()
    End Sub

    Public Sub NewTeam()
        tempTeam = Nothing
        _team = New TeamData()
        _team.CustomInfo.DataName = BattleData.CustomData.DataName
        _team.CustomInfo.DataHash = BattleData.CustomData.DataHash

        selInfo = Nothing
        My.Settings.teamPath = ""
        UpdateTeam()
        ClearText()
    End Sub

    Private Function CloseTeam() As Boolean
        If _team.Pokemons(0).Identity = 0 Then Return True
        Dim changed As Boolean = Not _team.Equals(tempTeam)
        If changed Then
            Dim result As DialogResult = MessageBox.Show("是否保存改动?", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation)
            Select Case result
                Case Windows.Forms.DialogResult.Yes
                    cmdSave.PerformClick()
                Case Windows.Forms.DialogResult.Cancel
                    Return False
            End Select
        End If
        selInfo = Nothing
        Return True
    End Function

    Private Sub SetSelectedPokemon(ByVal newPM As PokemonCustomInfo)
        _team.Pokemons(PokemonList.SelectedIndices(0)) = newPM
        If PokemonList.SelectedIndices(0) = 0 Then Return
        For i As Integer = 0 To PokemonList.SelectedIndices(0) - 1
            If _team.Pokemons(i).Identity = 0 Then
                SwapPM(PokemonList.SelectedIndices(0), i)
                Return
            End If
        Next
    End Sub

    Private Sub cmdNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNew.Click
        If Not CloseTeam() Then Return
        NewTeam()
    End Sub

    Private Sub cmdOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOpen.Click
        If Not CloseTeam() Then Return
        Dim file As New OpenFileDialog()
        file.Title = "打开队伍"
        file.Multiselect = False
        file.Filter = "PBO队伍文件(*.ptd)|*.ptd"
        Dim result As DialogResult = file.ShowDialog()
        If result = Windows.Forms.DialogResult.OK AndAlso file.FileName <> "" Then
            OpenTeam(OpenTeam(file.FileName), False)
        End If
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        If My.Settings.teamPath <> "" Then
            SaveTeam()
        Else
            cmdSaveTo.PerformClick()
        End If
    End Sub

    Private Sub cmdSaveTo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSaveTo.Click
        Dim fileDialog As New SaveFileDialog()
        fileDialog.Filter = "PBO队伍文件(*.ptd)|*.ptd"
        Dim result As DialogResult = fileDialog.ShowDialog()
        If result = Windows.Forms.DialogResult.OK OrElse result = Windows.Forms.DialogResult.Yes Then
            My.Settings.teamPath = fileDialog.FileName
            SaveTeam()
            If My.Settings.history.IndexOf(My.Settings.teamPath) = -1 Then _
                My.Settings.history.Add(fileDialog.FileName)
        End If
    End Sub
#End Region

    Private Sub UpdateTeam()
        PokemonList.Items.Clear()
        For i As Integer = 0 To 5
            If _team.Pokemons(i).Nickname <> "" Then
                If Not imgPM.Images.ContainsKey(_team.Pokemons(i).Identity.ToString) Then
                    Dim data As PokemonData = BattleData.GetPokemon(_team.Pokemons(i).Identity)
                    imgPM.Images.Add(data.Identity.ToString, BattleData.GetImage(data.Identity, data.Icon))
                End If
                PokemonList.Items.Add(_team.Pokemons(i).Nickname, _team.Pokemons(i).Identity.ToString)
            Else
                PokemonList.Items.Add("无")
            End If
        Next
    End Sub

#Region "PokemonEdit"
    Private Sub lstPM_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PokemonList.SelectedIndexChanged
        If PokemonList.SelectedIndices.Count = 0 Then
            ClearText()
            selInfo = Nothing
            Return
        End If
        If selInfo Is _team.Pokemons(PokemonList.SelectedIndices(0)) Then Return
        selInfo = _team.Pokemons(PokemonList.SelectedIndices(0))
        If selInfo.Nickname <> "" Then
            UpdatePM(selInfo)
            cmdUp.Enabled = True
            cmdDown.Enabled = True
            ExpertButton.Enabled = True
            LVNumberic.Enabled = True
            NickNameText.Enabled = True
        Else
            ClearText()
        End If
        PokemonCombo.Enabled = True
    End Sub

    Private Sub ClearText()
        PokemonCombo.Text = "(请选择)"
        NickNameText.Clear()
        lblHPVal.Text = ""
        lblAtkVal.Text = ""
        lblDefVal.Text = ""
        lblSAtkVal.Text = ""
        lblSDefVal.Text = ""
        lblSpeedVal.Text = ""
        lblType1Name.Text = ""
        lblType2Name.Text = ""
        MoveList.Items.Clear()
        MoveText1.Clear()
        MoveText2.Clear()
        MoveText3.Clear()
        MoveText4.Clear()
        picPM.Image = Nothing
        ExpertButton.Enabled = False
        ItemCombo.Enabled = False
        LVNumberic.Enabled = False
        cmdUp.Enabled = False
        cmdDown.Enabled = False
        SwitchButton.Enabled = False
        PokemonCombo.Enabled = False
        NickNameText.Enabled = False
    End Sub

    Private Sub UpdatePM(ByVal pm As PokemonCustomInfo, Optional ByVal updateMoveList As Boolean = True)
        updating = True
        PokemonCombo.Text = BattleData.GetPokemon(pm.Identity).Name
        NickNameText.Text = pm.Nickname

        Dim data As PokemonData = BattleData.GetPokemon(pm.Identity)
        Dim newPM As New Pokemon(data, pm, Nothing, False)
        lblHPVal.Text = newPM.MAXHP.ToString
        lblAtkVal.Text = newPM.AttackValue.ToString
        lblDefVal.Text = newPM.DefenceValue.ToString
        lblSAtkVal.Text = newPM.SpAttackValue.ToString
        lblSDefVal.Text = newPM.SpDefenceValue.ToString
        lblSpeedVal.Text = newPM.SpeedValue.ToString
        lblType1Name.Text = newPM.Type1.Name
        picPM.Image = newPM.FrontImage

        If newPM.Type2 IsNot Nothing Then
            lblType2Name.Text = newPM.Type2.Name
        Else
            lblType2Name.Text = "无"
        End If
        If updateMoveList Then
            SetMoveText(MoveText1, selInfo.SelectedMoves(0))
            SetMoveText(MoveText2, selInfo.SelectedMoves(1))
            SetMoveText(MoveText3, selInfo.SelectedMoves(2))
            SetMoveText(MoveText4, selInfo.SelectedMoves(3))

            lstUpdating = True

            MoveList.Items.Clear()

            For Each moveData As MoveLearnData In data.Moves
                Dim move As MoveData = BattleData.GetMove(moveData.MoveId)
                If move Is Nothing Then Continue For
                Dim power As String = move.Power.ToString
                If power = "0" Or power = "9999" Then power = "-"
                Dim acc As String = move.Accuracy * 100 & "%"
                If acc = "9999%" Then acc = "-"

                Dim newItem As New ListViewItem(New String() _
                    {move.Name, power, move.PP.ToString, acc, move.MoveType.ToString, moveData.LearnBy}, move.Type - 1) _
                    With {.Tag = moveData.MoveId}
                newItem.Name = move.Name
                MoveList.Items.Add(newItem)
            Next
            For i As Integer = 0 To 3
                If pm.SelectedMoves(i) = MoveData.InvalidId Then Continue For
                MoveList.FindItemWithText(BattleData.GetMoveName(pm.SelectedMoves(i))).Checked = True
            Next
            MoveList.ListViewItemSorter = MoveSorter

            lstUpdating = False
        End If

        LVNumberic.Value = pm.LV
        ItemCombo.SelectedIndex = selInfo.Item
        If (data.ItemRestriction <> Item.无) Then
            ItemCombo.SelectedIndex = data.ItemRestriction
            ItemCombo.Enabled = False
        Else
            ItemCombo.Enabled = True
        End If
        Select Case data.GenderRestriction
            Case PokemonGenderRestriction.No
                If pm.Gender = PokemonGender.No Then pm.Gender = PokemonGender.Male
            Case PokemonGenderRestriction.NoGender
                If pm.Gender <> PokemonGender.No Then pm.Gender = PokemonGender.No
            Case PokemonGenderRestriction.FemaleOnly
                If pm.Gender <> PokemonGender.Female Then pm.Gender = PokemonGender.Female
            Case PokemonGenderRestriction.MaleOnly
                If pm.Gender <> PokemonGender.Male Then pm.Gender = PokemonGender.Male
        End Select
        updating = False
    End Sub

    Private Sub lstMoves_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles MoveList.ColumnClick
        If MoveSorter IsNot Nothing Then MoveSorter.SetColumn(e.Column) : MoveList.Sort()
    End Sub

    Private Sub lstMoves_ItemChecked(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles MoveList.ItemChecked
        If lstUpdating Then Return

        If e.Item.Checked Then
            If MoveText1.Text <> "" AndAlso MoveText2.Text <> "" AndAlso MoveText3.Text <> "" AndAlso MoveText4.Text <> "" Then
                lstUpdating = True
                e.Item.Checked = False
                lstUpdating = False
                MessageBox.Show("技巧已满", "错误", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If
            With BattleData.GetPokemon(selInfo.Identity)
                Dim selTrait As Trait
                If selInfo.SelectedTrait = 1 Then
                    selTrait = .Trait1
                Else
                    selTrait = .Trait2
                End If
                If .GetMove(CInt(e.Item.Tag)).WithoutTrait = Convert.ToByte(selTrait) Then
                    lstUpdating = True
                    e.Item.Checked = False
                    lstUpdating = False
                    MessageBox.Show(e.Item.Text & "与" & selTrait.ToString & "特性无法共存", "错误", _
                                    MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return
                End If
            End With
            If MoveText1.Text = "" Then
                selInfo.SelectedMoves(0) = CInt(e.Item.Tag)
                MoveText1.Text = e.Item.Text
            ElseIf MoveText2.Text = "" Then
                selInfo.SelectedMoves(1) = CInt(e.Item.Tag)
                MoveText2.Text = e.Item.Text
            ElseIf MoveText3.Text = "" Then
                selInfo.SelectedMoves(2) = CInt(e.Item.Tag)
                MoveText3.Text = e.Item.Text
            ElseIf MoveText4.Text = "" Then
                selInfo.SelectedMoves(3) = CInt(e.Item.Tag)
                MoveText4.Text = e.Item.Text
            Else
                lstUpdating = True
                e.Item.Checked = False
                lstUpdating = False
            End If
            If e.Item.Checked Then
                Dim array As New List(Of Integer)
                For Each move As Integer In selInfo.SelectedMoves
                    If move <> MoveData.InvalidId Then array.Add(move)
                Next
                If array.Count > 1 Then
                    If Not _gameData.CheckMoves(array, BattleData.GetPokemon(selInfo.Identity)) Then
                        e.Item.Checked = False
                        MessageBox.Show(_gameData.FalseMove1 & "与" & _gameData.FalseMove2 & "不可共存", _
                            "错误", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        _gameData.FalseMove1 = ""
                        _gameData.FalseMove2 = ""
                    End If
                End If
            End If
        Else
            Select Case e.Item.Text
                Case MoveText1.Text
                    selInfo.SelectedMoves(0) = MoveData.InvalidId
                Case MoveText2.Text
                    selInfo.SelectedMoves(1) = MoveData.InvalidId
                Case MoveText3.Text
                    selInfo.SelectedMoves(2) = MoveData.InvalidId
                Case MoveText4.Text
                    selInfo.SelectedMoves(3) = MoveData.InvalidId
            End Select

            Dim array As New List(Of Integer)
            For Each move As Integer In selInfo.SelectedMoves
                If move <> MoveData.InvalidId Then array.Add(move)
            Next

            For i As Integer = 0 To 3
                If array.Count >= i + 1 Then
                    selInfo.SelectedMoves(i) = array(i)
                Else
                    selInfo.SelectedMoves(i) = MoveData.InvalidId
                End If
            Next

            SetMoveText(MoveText1, selInfo.SelectedMoves(0))
            SetMoveText(MoveText2, selInfo.SelectedMoves(1))
            SetMoveText(MoveText3, selInfo.SelectedMoves(2))
            SetMoveText(MoveText4, selInfo.SelectedMoves(3))
        End If

    End Sub

    Private Sub SetMoveText(ByVal textbox As TextBox, ByVal moveId As Integer)
        If moveId = MoveData.InvalidId Then
            textbox.Text = String.Empty
        Else
            textbox.Text = BattleData.GetMoveName(moveId)
        End If
    End Sub

    Private Sub cmdExpert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExpertButton.Click
        If selInfo Is Nothing Then Return

        Dim expert As New FrmExpertEditor(BattleData.GetPokemon(selInfo.Identity), selInfo)
        expert.ShowDialog()
        UpdatePM(selInfo, False)

    End Sub

    Private Sub cboPM_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles PokemonCombo.KeyUp
        If e.KeyCode = Keys.Enter Then
            Dim regex As New Regex("^\d+$")
            If regex.IsMatch(PokemonCombo.Text) Then
                Dim index As Integer = Convert.ToInt32(PokemonCombo.Text)
                If 0 < index AndAlso index < 387 Then
                    PokemonCombo.SelectedIndex = index - 1
                    e.SuppressKeyPress = True
                ElseIf index < 494 Then
                    PokemonCombo.SelectedIndex = index + 2
                End If
            End If
        End If
    End Sub

    Private Sub cboPM_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PokemonCombo.SelectedIndexChanged
        If selInfo Is Nothing Or updating Then Return

        SwitchButton.Enabled = True
    End Sub

    Private Sub nudLV_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LVNumberic.ValueChanged
        If selInfo Is Nothing Then Return
        selInfo.LV = Convert.ToByte(LVNumberic.Value)
        UpdatePM(selInfo, False)
    End Sub

    Private Sub cmdSwitch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SwitchButton.Click
        selInfo = New PokemonCustomInfo()
        SetSelectedPokemon(selInfo)

        Dim data As PokemonData = BattleData.GetPokemon(CType(PokemonCombo.SelectedItem, IdentityNamePair).Identity)

        selInfo.Identity = data.Identity
        selInfo.Nickname = data.Name
        selInfo.LV = 100
        If data.GenderRestriction = PokemonGenderRestriction.NoGender Then
            selInfo.Gender = PokemonGender.No
        ElseIf data.GenderRestriction = PokemonGenderRestriction.FemaleOnly Then
            selInfo.Gender = PokemonGender.Female
        Else
            selInfo.Gender = PokemonGender.Male
        End If
        For i As Integer = 0 To 5
            If _team.Pokemons(i) Is selInfo Then
                If Not imgPM.Images.ContainsKey(CStr(selInfo.Identity)) Then
                    imgPM.Images.Add(selInfo.Identity.ToString, BattleData.GetImage(data.Identity, data.Icon))
                End If
                PokemonList.Items(i).ImageKey = CStr(selInfo.Identity)
                PokemonList.Items(i).Text = selInfo.Nickname
            End If
        Next

        selInfo.HPIV = 31
        selInfo.AttackIV = 31
        selInfo.DefenceIV = 31
        selInfo.SpAttackIV = 31
        selInfo.SpDefenceIV = 31
        selInfo.SpeedIV = 31

        selInfo.HPEV = 85
        selInfo.AttackEV = 85
        selInfo.DefenceEV = 85
        selInfo.SpAttackEV = 85
        selInfo.SpDefenceEV = 85
        selInfo.SpeedEV = 85

        selInfo.Character = PokemonCharacter.Hardy
        selInfo.SelectedTrait = 1

        SwitchButton.Enabled = False
        ExpertButton.Enabled = True
        LVNumberic.Enabled = True

        If selInfo.LV <> 100 Then Return

        UpdatePM(selInfo)
        cmdUp.Enabled = True
        cmdDown.Enabled = True
        If data.ItemRestriction = Item.无 Then ItemCombo.Enabled = True
        NickNameText.Enabled = True
    End Sub

    Private Sub txtNickName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles NickNameText.LostFocus
        If selInfo Is Nothing OrElse PokemonList.SelectedItems.Count = 0 Then Return
        If NickNameText.Text <> "" AndAlso Not New Regex("\s").Match(NickNameText.Text).Success Then
            selInfo.Nickname = NickNameText.Text
            PokemonList.SelectedItems(0).Text = selInfo.Nickname
        Else
            NickNameText.Text = selInfo.Nickname
        End If
    End Sub

    Private Sub txtMoves_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles MoveText1.GotFocus, _
        MoveText2.GotFocus, MoveText3.GotFocus, MoveText4.GotFocus
        If selInfo Is Nothing Then Return
        If CType(sender, TextBox).Text = "" Then Return
        MoveList.FindItemWithText(CType(sender, TextBox).Text).Selected = True
        MoveList.EnsureVisible(MoveList.SelectedIndices(0))
        MoveList.Focus()
    End Sub

    Private Sub SwapPM(ByVal index1 As Integer, ByVal index2 As Integer, Optional ByVal ListSelect As Boolean = True)
        Dim pm1 As PokemonCustomInfo = _team.Pokemons(index1)
        Dim pm2 As PokemonCustomInfo = _team.Pokemons(index2)
        _team.Pokemons(index1) = pm2
        _team.Pokemons(index2) = pm1
        UpdateTeam()
        If Not ListSelect Then Return
        PokemonList.SelectedIndices.Add(index2)
        PokemonList.Focus()
    End Sub

    Private Sub cboItem_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemCombo.SelectedIndexChanged
        If selInfo Is Nothing Then Return
        If ItemCombo.SelectedIndex = -1 Then Return
        selInfo.Item = CType(ItemCombo.SelectedIndex, Item)
    End Sub

    Private Sub lstMoves_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MoveList.SelectedIndexChanged
        If MoveList.SelectedItems.Count > 0 Then
            lblMoveInfo.Text = BattleData.GetMove(CInt(MoveList.SelectedItems(0).Tag)).Info
        End If
    End Sub

    Private Sub cboItem_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ItemCombo.TextChanged
        If selInfo Is Nothing OrElse updating Then Return
        ItemCombo.SelectedItem = ItemCombo.Text
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SearchButton.Click
        If SearchText.Text <> "" Then
            Dim item As ListViewItem = MoveList.FindItemWithText(SearchText.Text)
            If item IsNot Nothing Then
                item.Selected = True
                MoveList.EnsureVisible(item.Index)
                MoveList.Focus()
            End If
        End If
    End Sub
#End Region

    Private Sub cmdUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUp.Click
        If PokemonList.SelectedIndices(0) = 0 Then Return
        SwapPM(PokemonList.SelectedIndices(0), PokemonList.SelectedIndices(0) - 1)
    End Sub

    Private Sub cmdDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDown.Click
        If PokemonList.SelectedIndices(0) = 5 Then Return
        If _team.Pokemons(PokemonList.SelectedIndices(0) + 1).Identity = 0 Then Return
        SwapPM(PokemonList.SelectedIndices(0), PokemonList.SelectedIndices(0) + 1)
    End Sub

#Region "Boxes"
    Private Sub cmdBox_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBox.Click
        If Me.Width = 649 Then
            ShowBox()
        Else
            HideBox()
        End If
    End Sub

    Private Sub ShowBox()
        scTeam.Panel2Collapsed = False
        Me.Size = New Size(825, Me.Height)
    End Sub

    Private Sub HideBox()
        Me.Size = New Size(649, Me.Height)
        scTeam.Panel2Collapsed = True
    End Sub

    Private Sub GetBox()
        boxes = New List(Of List(Of PokemonCustomInfo))
        If File.Exists(Application.StartupPath & "\box.pbd") Then
            Dim stream As FileStream
            stream = New FileStream(Application.StartupPath & "\box.pbd", FileMode.Open)
            Dim reader As New BinaryReader(stream)
            Try
                For listCounter As Integer = 0 To 9
                    Dim list As New List(Of PokemonCustomInfo)()
                    Dim pmCount As Integer = reader.ReadInt32()
                    For pmCounter As Integer = 1 To pmCount
                        list.Add(PokemonCustomInfo.FormStream(stream))
                    Next
                    boxes.Add(list)
                Next
            Catch ex As Exception
                For i As Integer = 0 To 9
                    boxes.Add(New List(Of PokemonCustomInfo)(10))
                Next
            Finally
                stream.Close()
            End Try
        Else
            For i As Integer = 0 To 9
                boxes.Add(New List(Of PokemonCustomInfo)(10))
            Next
        End If
        cboPMBox.SelectedIndex = 0
    End Sub

    Private Sub SaveBox()
        Dim stream As FileStream
        stream = New FileStream(Application.StartupPath & "\box.pbd", FileMode.Create)
        Dim writer As New BinaryWriter(stream)
        Try
            For Each list As List(Of PokemonCustomInfo) In boxes
                writer.Write(list.Count)
                For Each info As PokemonCustomInfo In list
                    info.Save(stream)
                Next
            Next
        Catch ex As Exception
        Finally
            stream.Close()
        End Try
    End Sub

    Private Sub cboPMBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPMBox.SelectedIndexChanged
        lstPMBox.Items.Clear()
        If cboPMBox.SelectedIndex <> -1 Then
            cmdIn.Enabled = True
            cmdOut.Enabled = True
            cmdRemove.Enabled = True
            For Each pm As PokemonCustomInfo In boxes(cboPMBox.SelectedIndex)
                lstPMBox.Items.Add(pm.Nickname)
            Next
            If lblBoxName.Text <> "" Then
                picBox.Image = Nothing
                lblBoxName.Text = ""
                lblBoxHP.Text = ""
                lblBoxAtk.Text = ""
                lblBoxDef.Text = ""
                lblBoxSAtk.Text = ""
                lblBoxSDef.Text = ""
                lblBoxSp.Text = ""
                lblBoxMove1.Text = ""
                lblBoxMove2.Text = ""
                lblBoxMove3.Text = ""
                lblBoxMove4.Text = ""
                lblBoxItem.Text = ""
            End If
        End If
    End Sub

    Private Sub cmdIn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdIn.Click
        If cboPMBox.SelectedIndex <> -1 AndAlso PokemonList.SelectedIndices.Count <> 0 AndAlso selInfo.Nickname <> "" _
            AndAlso Not selInfo.SelectedMoves(0) = MoveData.InvalidId  Then

            If boxes(cboPMBox.SelectedIndex).Count < 20 Then
                boxes(cboPMBox.SelectedIndex).Add(CType(selInfo.Clone, PokemonCustomInfo))
                lstPMBox.Items.Add(selInfo.Nickname)
            Else
                MessageBox.Show("该箱子已满", "提示", MessageBoxButtons.OK)
            End If
        End If
    End Sub

    Private Sub cmdRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRemove.Click
        If lstPMBox.SelectedIndex <> -1 Then
            Dim result As DialogResult = MessageBox.Show("是否删除选中PM？", "提示", MessageBoxButtons.YesNo)
            If result = Windows.Forms.DialogResult.Yes Then
                boxes(cboPMBox.SelectedIndex).RemoveAt(lstPMBox.SelectedIndex)
                lstPMBox.Items.RemoveAt(lstPMBox.SelectedIndex)
            End If
        End If
    End Sub

    Private Sub cmdOut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOut.Click
        If lstPMBox.SelectedIndex <> -1 AndAlso PokemonList.SelectedIndices.Count > 0 Then
            If BattleData.CheckPokemon(boxes(cboPMBox.SelectedIndex)(lstPMBox.SelectedIndex)) Then
                Dim result As DialogResult = MessageBox.Show("是否导入所选PM？", "提示", MessageBoxButtons.YesNo)
                If result = Windows.Forms.DialogResult.Yes Then
                    SetSelectedPokemon(CType(boxes(cboPMBox.SelectedIndex)(lstPMBox.SelectedIndex).Clone, PokemonCustomInfo))
                    selInfo = _team.Pokemons(PokemonList.SelectedIndices(0))
                    PokemonList.SelectedItems(0).Text = selInfo.Nickname
                    PokemonList.SelectedItems(0).ImageKey = CStr(selInfo.Identity)
                    UpdatePM(selInfo)
                End If
            End If
        End If
    End Sub

    Private Sub lstPMBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstPMBox.SelectedIndexChanged
        If lstPMBox.SelectedIndex <> -1 Then
            Dim pm As PokemonCustomInfo = boxes(cboPMBox.SelectedIndex)(lstPMBox.SelectedIndex)
            If BattleData.CheckPokemon(pm) Then
                If Not imgPM.Images.ContainsKey(CStr(pm.Identity)) Then
                    Dim data As PokemonData = BattleData.GetPokemon(pm.Identity)
                    imgPM.Images.Add(data.Identity.ToString(), BattleData.GetImage(data.Identity, data.Icon))
                End If
                picBox.Image = imgPM.Images(pm.Identity.ToString())
                lblBoxName.Text = pm.Nickname
                Dim newPM As New Pokemon(BattleData.GetPokemon(pm.Identity), pm, Nothing, False)
                lblBoxHP.Text = "HP : " & newPM.MAXHP
                lblBoxAtk.Text = "物攻 : " & newPM.AttackValue
                lblBoxDef.Text = "物防 : " & newPM.DefenceValue
                lblBoxSAtk.Text = "特攻 : " & newPM.SpAttackValue
                lblBoxSDef.Text = "特防 : " & newPM.SpDefenceValue
                lblBoxSp.Text = "速度 : " & newPM.SpeedValue
                lblBoxMove1.Text = BattleData.GetMoveName(pm.SelectedMoves(0))
                lblBoxMove2.Text = BattleData.GetMoveName(pm.SelectedMoves(1))
                lblBoxMove3.Text = BattleData.GetMoveName(pm.SelectedMoves(2))
                lblBoxMove4.Text = BattleData.GetMoveName(pm.SelectedMoves(3))
                lblBoxItem.Text = pm.Item.ToString
            Else
                lblBoxName.Text = "该精灵的数据与当前自" & vbCrLf & "定义数据不符,暂时无" & vbCrLf & "法使用"
                picBox.Image = Nothing
                lblBoxHP.Text = ""
                lblBoxAtk.Text = ""
                lblBoxDef.Text = ""
                lblBoxSAtk.Text = ""
                lblBoxSDef.Text = ""
                lblBoxSp.Text = ""
                lblBoxMove1.Text = ""
                lblBoxMove2.Text = ""
                lblBoxMove3.Text = ""
                lblBoxMove4.Text = ""
                lblBoxItem.Text = ""
            End If
        End If
    End Sub

#End Region

    Public ReadOnly Property Team() As TeamData
        Get
            Return _team
        End Get
    End Property
    Private ReadOnly Property MotherForm() As MainForm
        Get
            Return CType(MdiParent, MainForm)
        End Get
    End Property

    Private Sub FrmTeamEditor_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.VisibleChanged
        If Visible Then
            If MoveList.Items.Count = 0 Then lblMoveInfo.Text = ""
            MotherForm.toolStripSeparator.Visible = True
            MotherForm.保存SToolStripMenuItem.Visible = True
            MotherForm.另存为AToolStripMenuItem.Visible = True
            MotherForm.EditTeamMenuItem.Visible = False
        Else
            MotherForm.toolStripSeparator.Visible = False
            MotherForm.保存SToolStripMenuItem.Visible = False
            MotherForm.另存为AToolStripMenuItem.Visible = False
            MotherForm.EditTeamMenuItem.Visible = True
        End If
    End Sub

    Private Sub txtSearch_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles SearchText.KeyDown
        If e.KeyCode = Keys.Enter Then
            SearchButton.PerformClick()
            e.SuppressKeyPress = True
        End If
    End Sub

#Region "Embeded Class"
    Protected Class IdentityNamePair
        Public NameBase As String
        Public Identity As Integer
        Public Sub New(ByVal name As String, ByVal id As Integer)
            NameBase = name
            Identity = id
        End Sub
        Public Overrides Function ToString() As String
            Return NameBase
        End Function
    End Class
#End Region
End Class