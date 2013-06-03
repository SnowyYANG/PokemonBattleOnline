Imports PokemonBattle.PokemonData
Friend Class FrmDataDex
    Private pmIndex As Integer = 1

    Private SearchForm As FrmSearchPM

    Private changeTimes As Integer

    Private dicTraits As Dictionary(Of String, String)
    Private dicItems As Dictionary(Of String, String)

    Private _moves As MoveData()
    Private _pokemons As PokemonData()
    Public Sub New()

        ' 此调用是 Windows 窗体设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。

        Me.Icon = My.Resources.PokemonBall
        Me.Text = "PokemonBattle资料集合"
        _moves = BattleData.GetAllMoves()
        _pokemons = BattleData.GetAllPokemons()

        For Each data As PokemonType In BattleData.GetAllTypes()
            If data.Image IsNot Nothing Then imgType.Images.Add(data.Name, data.Image)
        Next

        For Each data As MoveData In _moves
            lstMoves.Items.Add(New ListViewItem(data.Name, data.Type - 1) With {.Tag = data.Identity})
        Next
        Try
            Using stream As New FileStream(Path.Combine(Application.StartupPath, TAIPath), FileMode.Open)
                Using reader As New StreamReader(stream)
                    dicTraits = New Dictionary(Of String, String)
                    dicItems = New Dictionary(Of String, String)
                    Dim dic As Dictionary(Of String, String) = dicTraits
                    Dim list As ListBox = lstTraits
                    Do Until reader.EndOfStream
                        Dim line As String = reader.ReadLine
                        If line = "" Then
                            dic = dicItems
                            list = lstItems
                        Else
                            Dim info() As String = SplitString(line, SeparateChar(0))
                            If info.Length = 2 Then
                                dic.Add(info(0), info(1))
                            Else
                                dic.Add(info(0), "")
                            End If
                            list.Items.Add(info(0))
                        End If
                    Loop
                End Using
            End Using
        Catch ex As Exception
        End Try
        lstMoves.MultiSelect = False
    End Sub

    Private Sub FrmDataDex_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If e.CloseReason <> CloseReason.MdiFormClosing Then
            e.Cancel = True
            Hide()
        End If
    End Sub

    Private Sub FrmDataDex_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        UpdatePM()

    End Sub

    Private Sub UpdatePM()
        If _pokemons Is Nothing Then Return
        Dim pmData As PokemonData
        pmData = Array.Find(_pokemons, Function(pm As PokemonData) pm.Number = pmIndex)

        If pmIndex = 386 OrElse pmIndex = 487 OrElse pmIndex = 492 OrElse pmIndex = 479 Then
            cmdChange.Visible = True
            changeTimes = 0
        Else
            cmdChange.Visible = False
        End If
        UpdatePM(pmData)
    End Sub

    Private Sub UpdatePM(ByVal pm As PokemonData)
        picBase.Image = GetBaseImage(pm)
        SetPMImage(pm)
        lblPMName.Text = "名称 :" & pm.Name
        lblPMType.Text = "属性 :" & BattleData.GetTypeName(pm.Type1)
        If pm.Type2 <> PokemonType.InvalidId Then lblPMType.Text &= "/" & BattleData.GetTypeName(pm.Type2)
        lblTrait.Text = "特性 :" & pm.Trait1.ToString
        If pm.Trait2 <> Trait.无 Then lblTrait.Text &= "/" & pm.Trait2.ToString
        lblWeight.Text = "体重 :" & pm.Weight & "kg"
        lblGender.Text = "性别限制 :"
        Select Case pm.GenderRestriction
            Case PokemonGenderRestriction.No
                lblGender.Text &= "无"
            Case PokemonGenderRestriction.MaleOnly
                lblGender.Text &= "公"
            Case PokemonGenderRestriction.FemaleOnly
                lblGender.Text &= "母"
            Case PokemonGenderRestriction.NoGender
                lblGender.Text &= "无性别"
        End Select
        lblEggGroup.Text = "生蛋组别 :"
        lblEggGroup.Text &= pm.EggGroup1.ToString
        If pm.EggGroup2 <> EggGroup.无 Then lblEggGroup.Text &= "/" & pm.EggGroup2.ToString

        lblCount.Text = "对战统计 :" & PokemonCounter.GetCount(pm.Identity)

        lstPMMoves.Items.Clear()
        Dim move As MoveData
        For Each movedata As MoveLearnData In pm.Moves
            move = BattleData.GetMove(movedata.MoveId)
            lstPMMoves.Items.Add(New ListViewItem(New String() {move.Name, move.MoveType.ToString, movedata.LearnBy}, move.Type - 1) _
                                 With {.Tag = move.Identity})
        Next

        Dim identity As Integer = pm.Identity
        If pm.BeforeEvolution <> 0 Then
            identity = pm.BeforeEvolution
            Dim before As PokemonData = BattleData.GetPokemon(identity)
            If before.BeforeEvolution <> 0 Then
                identity = before.BeforeEvolution
            End If
        End If
        SetTree(identity)
    End Sub

    Private Function GetBaseImage(ByVal data As PokemonData) As Image
        Dim newImg As New Bitmap(325, 160)
        Dim graph As Graphics = Graphics.FromImage(newImg)
        Dim hp As New Rectangle(25, 140 - data.HPBase \ 2, 25, data.HPBase \ 2)
        Dim atk As New Rectangle(75, 140 - data.AttackBase \ 2, 25, data.AttackBase \ 2)
        Dim def As New Rectangle(125, 140 - data.DefenceBase \ 2, 25, data.DefenceBase \ 2)
        Dim speed As New Rectangle(175, 140 - data.SpeedBase \ 2, 25, data.SpeedBase \ 2)
        Dim satk As New Rectangle(225, 140 - data.SpAttackBase \ 2, 25, data.SpAttackBase \ 2)
        Dim sdef As New Rectangle(275, 140 - data.SpDefenceBase \ 2, 25, data.SpDefenceBase \ 2)
        Dim rects As Rectangle() = New Rectangle() {hp, atk, def, speed, satk, sdef}
        graph.FillRectangles(Brushes.DarkRed, rects)
        Dim font As New Font("宋体", 12, FontStyle.Regular, GraphicsUnit.Pixel)
        graph.DrawString(data.HPBase.ToString, font, Brushes.Black, 30, 128 - hp.Height)
        graph.DrawString(data.AttackBase.ToString, font, Brushes.Black, 80, 128 - atk.Height)
        graph.DrawString(data.DefenceBase.ToString, font, Brushes.Black, 130, 128 - def.Height)
        graph.DrawString(data.SpeedBase.ToString, font, Brushes.Black, 180, 128 - speed.Height)
        graph.DrawString(data.SpAttackBase.ToString, font, Brushes.Black, 230, 128 - satk.Height)
        graph.DrawString(data.SpDefenceBase.ToString, font, Brushes.Black, 280, 128 - sdef.Height)

        graph.DrawString("HP", font, Brushes.Black, 30, 148)
        graph.DrawString("物攻", font, Brushes.Black, 75, 148)
        graph.DrawString("物防", font, Brushes.Black, 125, 148)
        graph.DrawString("速度", font, Brushes.Black, 175, 148)
        graph.DrawString("特攻", font, Brushes.Black, 225, 148)
        graph.DrawString("特防", font, Brushes.Black, 275, 148)

        Return newImg
    End Function
    Private Sub SetPMImage(ByVal data As PokemonData)
        picFront1.Image = BattleData.GetImage(data.Identity, data.FrontImage)
        picFrame1.Image = BattleData.GetImage(data.Identity, data.Frame)
        picBack1.Image = BattleData.GetImage(data.Identity, data.BackImage)
        picFront2.Image = BattleData.GetImage(data.Identity, data.FrontImageF)
        picFrame2.Image = BattleData.GetImage(data.Identity, data.FrameF)
        picBack2.Image = BattleData.GetImage(data.Identity, data.BackImageF)
    End Sub

    Private Sub nudNo_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles nudNo.ValueChanged
        pmIndex = Convert.ToInt32(nudNo.Value)
        UpdatePM()
    End Sub

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Hide()
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        SearchForm = New FrmSearchPM(imgType, Me)
        SearchForm.MdiParent = Me.MdiParent
        SearchForm.Show()
    End Sub

    Public Sub SetPMIndex(ByVal index As Integer)
        nudNo.Value = index
    End Sub

    Private Sub cmdChange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdChange.Click
        If pmIndex = 386 Then
            Select Case changeTimes
                Case 0
                    UpdatePM(_pokemons(386))
                    changeTimes += 1
                Case 1
                    UpdatePM(_pokemons(387))
                    changeTimes += 1
                Case 2
                    UpdatePM(_pokemons(388))
                    changeTimes += 1
                Case 3
                    UpdatePM(_pokemons(385))
                    changeTimes = 0
            End Select
        ElseIf pmIndex = 492 Then
            Select Case changeTimes
                Case 0
                    UpdatePM(_pokemons(496))
                    changeTimes += 1
                Case 1
                    UpdatePM(_pokemons(494))
                    changeTimes = 0
            End Select
        ElseIf pmIndex = 487 Then
            Select Case changeTimes
                Case 0
                    UpdatePM(_pokemons(497))
                    changeTimes += 1
                Case 1
                    UpdatePM(_pokemons(489))
                    changeTimes = 0
            End Select
        ElseIf pmIndex = 479 Then
            Select Case changeTimes
                Case 0
                    UpdatePM(_pokemons(498))
                    changeTimes += 1
                Case 1
                    UpdatePM(_pokemons(499))
                    changeTimes += 1
                Case 2
                    UpdatePM(_pokemons(500))
                    changeTimes += 1
                Case 3
                    UpdatePM(_pokemons(501))
                    changeTimes += 1
                Case 4
                    UpdatePM(_pokemons(502))
                    changeTimes += 1
                Case 5
                    UpdatePM(_pokemons(481))
                    changeTimes = 0
            End Select

        End If
    End Sub

    Private Sub lstMoves_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstMoves.SelectedIndexChanged
        If lstMoves.SelectedItems.Count > 0 Then
            UpdateMove(BattleData.GetMove(CInt(lstMoves.SelectedItems(0).Tag)))
        End If
    End Sub

    Public Sub UpdateMove(ByVal move As MoveData)
        lblMoveName.Text = "名称 : " & move.Name
        lblType.Text = "属性 : " & BattleData.GetTypeName(move.Type)
        lblPower.Text = "威力 : " & move.Power
        lblTarget.Text = "对象 : " & move.Target.ToString
        If move.Power = 0 OrElse move.Power = 9999 Then
            lblPower.Text = "威力 : -"
        End If
        lblAcc.Text = "命中率 : " & move.Accuracy * 100 & "%"
        If move.Accuracy > 1 Then
            lblAcc.Text = "命中率 : -"
        End If
        lblMoveType.Text = "类型 : " & move.MoveType.ToString
        lblPP.Text = "PP : " & move.PP
        txtInfo.Text = move.Info
        chkContact.Checked = False
        chkSound.Checked = False
        chkPuch.Checked = False
        chkKR.Checked = False
        chkBP.Checked = False
        If move.Contact Then chkContact.Checked = True
        If move.Sound Then chkSound.Checked = True
        If move.KingRock Then chkKR.Checked = True
        If move.Snatchable Then chkBP.Checked = True
        If move.Punch Then chkPuch.Checked = True
    End Sub

    Private Sub cmdSearchMove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearchMove.Click
        If txtSearch.Text <> "" Then
            Dim item As ListViewItem = lstMoves.FindItemWithText(txtSearch.Text)
            If item IsNot Nothing Then
                item.Selected = True
                lstMoves.EnsureVisible(item.Index)
                lstMoves.Focus()
            End If
        End If
    End Sub

    Public Sub SetTree(ByVal identity As Integer)
        Dim data As PokemonData = BattleData.GetPokemon(identity)
        If treEvolution.Nodes.Count > 0 AndAlso data.Name = treEvolution.Nodes(0).Text Then Return
        treEvolution.Nodes.Clear()

        Dim newNode As New TreeNode(data.Name)
        newNode.Tag = data.Identity
        treEvolution.Nodes.Add(newNode)

        For Each no As Integer In data.AfterEvolution
            SetNodes(no, treEvolution.Nodes(0))
        Next

        treEvolution.ExpandAll()
    End Sub

    Public Sub SetNodes(ByVal identity As Integer, ByVal node As TreeNode)
        Dim data As PokemonData = BattleData.GetPokemon(identity)
        Dim newNode As New TreeNode(data.Name)
        newNode.Tag = data.Identity
        node.Nodes.Add(newNode)

        For Each no As Integer In data.AfterEvolution
            SetNodes(no, newNode)
        Next
    End Sub

    Private Sub treEvolution_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) _
        Handles treEvolution.AfterSelect
        nudNo.Value = BattleData.GetPokemon(CInt(e.Node.Tag)).Number
    End Sub

    Private Sub cmdInherit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdInherit.Click
        Dim pmData As PokemonData
        pmData = Array.Find(_pokemons, Function(pm As PokemonData) pm.Number = pmIndex)
        Dim InheritForm As New FrmInheritSearch(pmData, Me)
        InheritForm.MdiParent = Me.MdiParent
        InheritForm.Show()
        cmdInherit.Enabled = False
    End Sub

    Public Sub UpdateData()
        _moves = BattleData.GetAllMoves
        _pokemons = BattleData.GetAllPokemons()
        nudNo.Maximum = _pokemons.Length - 3
        UpdatePM()
    End Sub

    Private Sub txtTrait_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTrait.TextChanged
        If txtTrait.Text <> "" Then
            Dim index As Integer = lstTraits.FindString(txtTrait.Text)
            If index <> -1 Then lstTraits.SelectedIndex = index : lstTraits.Focus()
        End If
    End Sub

    Private Sub txtItem_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtItem.TextChanged
        If txtItem.Text <> "" Then
            Dim index As Integer = lstItems.FindString(txtItem.Text)
            If index <> -1 Then lstItems.SelectedIndex = index : lstItems.Focus()
        End If
    End Sub

    Private Sub lstTraits_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstTraits.SelectedIndexChanged
        If lstTraits.SelectedIndex <> -1 Then txtTraitInfo.Text = dicTraits(CStr(lstTraits.SelectedItem))
    End Sub

    Private Sub lstItems_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstItems.SelectedIndexChanged
        If lstItems.SelectedIndex <> -1 Then txtItemInfo.Text = dicItems(CStr(lstItems.SelectedItem))
    End Sub

    Private Sub txtSearch_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            cmdSearchMove.PerformClick()
        End If
    End Sub

    Private Sub CounterButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CounterButton.Click
        Dim dialog As SaveFileDialog = New SaveFileDialog()
        dialog.Title = "导出PM统计"
        dialog.Filter = "文本文件(*.txt)|*.txt"
        Dim result As DialogResult = dialog.ShowDialog()
        If result = DialogResult.OK OrElse result = DialogResult.Yes Then
            Using writer As New StreamWriter(dialog.FileName)
                For Each pm As PokemonData In _pokemons
                    writer.WriteLine(pm.Number.ToString() & vbTab & pm.Name & vbTab & PokemonCounter.GetCount(pm.Identity))
                Next
            End Using
        End If
    End Sub
End Class