Friend Class FrmExpertEditor
    Private data As PokemonData
    Private info As PokemonCustomInfo
    Private TempInfo As PokemonCustomInfo

    Private usedEV As Integer
    Public Sub New(ByVal dataVal As PokemonData, ByVal infoVal As PokemonCustomInfo)

        ' 此调用是 Windows 窗体设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        Me.Icon = My.Resources.PokemonBall
        data = dataVal
        info = infoVal
        TempInfo = CType(info.Clone, PokemonCustomInfo)
        updatePM()
    End Sub
    Private Sub updatePM()
        Dim newPm As New Pokemon(data, info, Nothing, False)
        lblHPVal.Text = newPm.MAXHP.ToString
        lblAtkVal.Text = newPm.AttackValue.ToString
        lblDefVal.Text = newPm.DefenceValue.ToString
        lblSAtkVal.Text = newPm.SpAttackValue.ToString
        lblSDefVal.Text = newPm.SpDefenceValue.ToString
        lblSpeedVal.Text = newPm.SpeedValue.ToString

        nudHPEV.Value = info.HPEV
        nudHPIV.Value = info.HPIV

        nudSpeedEV.Value = info.SpeedEV
        nudSpeedIV.Value = info.SpeedIV

        nudAtkEV.Value = info.AttackEV
        nudAtkIV.Value = info.AttackIV

        nudDefEV.Value = info.DefenceEV
        nudDefIV.Value = info.DefenceIV

        nudSAtkEV.Value = info.SpAttackEV
        nudSAtkIV.Value = info.SpAttackIV

        nudSDefEV.Value = info.SpDefenceEV
        nudSDefIV.Value = info.SpDefenceIV

        updateEV(nudHPEV)
        updateEV(nudSpeedEV)
        updateEV(nudAtkEV)
        updateEV(nudDefEV)
        updateEV(nudSAtkEV)
        updateEV(nudSDefIV)

        cboNature.SelectedIndex = info.Character
        Select Case data.GenderRestriction
            Case PokemonGenderRestriction.No
                radNoGender.Enabled = False
            Case PokemonGenderRestriction.FemaleOnly
                radMale.Enabled = False
                radNoGender.Enabled = False
            Case PokemonGenderRestriction.MaleOnly
                radFemale.Enabled = False
                radNoGender.Enabled = False
            Case PokemonGenderRestriction.NoGender
                radMale.Enabled = False
                radFemale.Enabled = False
        End Select

        Select Case info.Gender
            Case PokemonGender.Male
                radMale.Checked = True
            Case PokemonGender.Female
                radFemale.Checked = True
            Case PokemonGender.No
                radNoGender.Checked = True
        End Select

        If data.trait2 = Trait.无 Then radTrait2.Enabled = False
        radTrait1.Text = data.trait1.ToString
        If radTrait2.Enabled = True Then radTrait2.Text = data.trait2.ToString
        If info.SelectedTrait = 0 Then info.SelectedTrait = 1
        If info.SelectedTrait = 1 Then
            radTrait1.Checked = True
        Else
            radTrait2.Checked = True
        End If
        usedEV = Convert.ToInt32(info.HPEV) + info.AttackEV + info.DefenceEV + info.SpAttackEV + info.SpDefenceEV + info.SpeedEV
        lblEVRest.Text = (510 - usedEV).ToString
    End Sub
    Private Sub updateEV(ByVal sender As NumericUpDown)
        Dim addPoint As Integer
        If sender.Value > sender.Maximum Then sender.Value = sender.Maximum
        Select Case sender.Name
            Case nudHPEV.Name
                addPoint = Convert.ToInt32(sender.Value - info.HPEV)
            Case nudAtkEV.Name
                addPoint = Convert.ToInt32(sender.Value - info.AttackEV)
            Case nudDefEV.Name
                addPoint = Convert.ToInt32(sender.Value - info.DefenceEV)
            Case nudSpeedEV.Name
                addPoint = Convert.ToInt32(sender.Value - info.SpeedEV)
            Case nudSAtkEV.Name
                addPoint = Convert.ToInt32(sender.Value - info.SpAttackEV)
            Case nudSDefEV.Name
                addPoint = Convert.ToInt32(sender.Value - info.SpDefenceEV)
        End Select
        usedEV += addPoint
        For Each ctrl As Object In grpEV.Controls
            If TypeOf ctrl Is NumericUpDown Then
                Dim nud As NumericUpDown = CType(ctrl, NumericUpDown)
                If nud.Value + (510 - usedEV) > 255 Then
                    nud.Maximum = 255
                Else
                    nud.Maximum = nud.Value + (510 - usedEV)
                End If
            End If
        Next
        lblEVRest.Text = (510 - usedEV).ToString
    End Sub

    Private Sub nudHPIV_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudHPIV.ValueChanged
        info.HPIV = Convert.ToByte(nudHPIV.Value)
        updatePM()
    End Sub

    Private Sub nudAtkIV_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudAtkIV.ValueChanged
        info.AttackIV = Convert.ToByte(nudAtkIV.Value)
        updatePM()
    End Sub

    Private Sub nudDefIV_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudDefIV.ValueChanged
        info.DefenceIV = Convert.ToByte(nudDefIV.Value)
        updatePM()
    End Sub

    Private Sub nudSpeedIV_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudSpeedIV.ValueChanged
        info.SpeedIV = Convert.ToByte(nudSpeedIV.Value)
        updatePM()
    End Sub

    Private Sub nudSAtkIV_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudSAtkIV.ValueChanged
        info.SpAttackIV = Convert.ToByte(nudSAtkIV.Value)
        updatePM()
    End Sub

    Private Sub nudSDefIV_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudSDefIV.ValueChanged
        info.SpDefenceIV = Convert.ToByte(nudSDefIV.Value)
        updatePM()
    End Sub

    Private Sub nudHPEV_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudHPEV.ValueChanged
        updateEV(nudHPEV)
        info.HPEV = Convert.ToByte(nudHPEV.Value)
        updatePM()
    End Sub

    Private Sub nudAtkEV_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudAtkEV.ValueChanged
        updateEV(nudAtkEV)
        info.AttackEV = Convert.ToByte(nudAtkEV.Value)
        updatePM()
    End Sub

    Private Sub nudDefEV_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudDefEV.ValueChanged
        updateEV(nudDefEV)
        info.DefenceEV = Convert.ToByte(nudDefEV.Value)
        updatePM()
    End Sub

    Private Sub nudSpeedEV_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudSpeedEV.ValueChanged
        updateEV(nudSpeedEV)
        info.SpeedEV = Convert.ToByte(nudSpeedEV.Value)
        updatePM()
    End Sub

    Private Sub nudSAtkEV_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudSAtkEV.ValueChanged
        updateEV(nudSAtkEV)
        info.SpAttackEV = Convert.ToByte(nudSAtkEV.Value)
        updatePM()
    End Sub

    Private Sub nudSDefEV_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudSDefEV.ValueChanged
        updateEV(nudSDefEV)
        info.SpDefenceEV = Convert.ToByte(nudSDefEV.Value)
        updatePM()
    End Sub

    Private Sub radGender_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles radMale.CheckedChanged, radFemale.CheckedChanged, radNoGender.CheckedChanged

        If radMale.Checked Then
            info.Gender = PokemonGender.Male
        ElseIf radFemale.Checked Then
            info.Gender = PokemonGender.Female
        ElseIf radNoGender.Checked Then
            info.Gender = PokemonGender.No
        End If
    End Sub

    Private Sub cboNature_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboNature.SelectedIndexChanged
        If cboNature.SelectedIndex = -1 Then Return
        info.Character = CType(cboNature.SelectedIndex, PokemonCharacter)
        updatePM()
    End Sub

    Private Sub radTrait1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radTrait1.CheckedChanged
        If radTrait1.Checked Then
            Dim trait As Trait = data.trait1
            If CheckTrait(trait) Then
                info.SelectedTrait = 1
            Else
                radTrait2.Checked = True
            End If
        End If
    End Sub

    Private Sub radTrait2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radTrait2.CheckedChanged
        If radTrait2.Checked Then
            Dim trait As Trait = data.trait2
            If CheckTrait(trait) Then
                info.SelectedTrait = 2
            Else
                radTrait1.Checked = True
            End If
        End If
    End Sub
    Private Function CheckTrait(ByVal trait As Trait) As Boolean
        For Each moveId As Integer In info.SelectedMoves
            Dim move As MoveLearnData = data.GetMove(moveId)
            If move IsNot Nothing AndAlso move.WithoutTrait = trait Then
                MessageBox.Show(String.Format("{0}与{1}特性无法共存",
                                BattleData.GetMoveName(moveId), trait.ToString), "错误", _
                                MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If
        Next
        Return True
    End Function

    Private Sub cmdClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClear.Click
        nudHPEV.Value = 0
        nudAtkEV.Value = 0
        nudSAtkEV.Value = 0
        nudDefEV.Value = 0
        nudSDefEV.Value = 0
        nudSpeedEV.Value = 0
    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Close()
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        info = TempInfo
        Close()
    End Sub
End Class