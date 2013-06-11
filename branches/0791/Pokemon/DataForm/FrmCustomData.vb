Imports PokemonBattle.PokemonData
Imports PokemonBattle.PokemonData.Custom
Friend Class FrmCustomData

    Private data As CustomGameData
    Private gameData As GameData
    Private tempData As CustomGameData
    Public Sub New(ByVal gd As GameData)

        ' �˵����� Windows ���������������ġ�
        InitializeComponent()

        ' �� InitializeComponent() ����֮�������κγ�ʼ����
        Icon = My.Resources.PokemonBall
        gameData = gd
        For Each type As PokemonType In BattleData.GetAllTypes()
            If type.Name <> "��" Then
                cboType1.Items.Add(type.Name)
                cboType2.Items.Add(type.Name)
                cboNewType1.Items.Add(type.Name)
                cboNewType2.Items.Add(type.Name)
            End If
        Next
        If My.Settings.CustomDataPath <> "" Then
            If Not File.Exists(My.Settings.CustomDataPath) Then
                My.Settings.CustomDataPath = ""
            Else
                OpenData(My.Settings.CustomDataPath)
            End If
        End If
    End Sub

#Region "NewPokemon"
    Private selNew As CustomPokemonData
    Private updatingNew As Boolean
    Private Sub lstNewPM_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstNewPM.SelectedIndexChanged
        If lstNewPM.SelectedIndex <> -1 Then
            updatingNew = True
            If txtName.Enabled = False Then
                txtName.Enabled = True
                cboType1.Enabled = True
                cboType2.Enabled = True
                cboTrait1.Enabled = True
                cboTrait2.Enabled = True
                nudHP.Enabled = True
                nudAtk.Enabled = True
                nudDef.Enabled = True
                nudSAtk.Enabled = True
                nudSDef.Enabled = True
                nudSpeed.Enabled = True
                nudWeight.Enabled = True
                cboEg1.Enabled = True
                cboEg2.Enabled = True
                cboGenderRS.Enabled = True
                btnImage.Enabled = True
                btnAddMove.Enabled = True
                btnRemoveMove.Enabled = True
            End If
            selNew = data.CustomPokemons(lstNewPM.SelectedIndex)
            txtName.Text = selNew.NameBase
            cboType1.Text = BattleData.GetTypeName(selNew.Type1)
            If selNew.Type2 <> PokemonType.InvalidId Then
                cboType2.Text = BattleData.GetTypeName(selNew.Type2)
            Else
                cboType2.SelectedIndex = 0
            End If
            cboTrait1.SelectedIndex = selNew.Trait1 - 1
            cboTrait2.SelectedIndex = selNew.Trait2
            nudHP.Value = selNew.HPBase
            nudAtk.Value = selNew.AttackBase
            nudDef.Value = selNew.DefenceBase
            nudSAtk.Value = selNew.SpAttackBase
            nudSDef.Value = selNew.SpDefenceBase
            nudSpeed.Value = selNew.SpeedBase
            nudWeight.Value = Convert.ToDecimal(selNew.Weight)
            cboEg1.SelectedIndex = selNew.EggGroup1 - 1
            cboEg2.SelectedIndex = selNew.EggGroup2
            cboGenderRS.SelectedIndex = selNew.GenderRestriction
            txtNumber.Text = selNew.Number.ToString

            lstMoves.Items.Clear()
            For Each move As String In selNew.Moves
                lstMoves.Items.Add(move)
            Next
            updatingNew = False
        Else
            txtName.Enabled = False
            cboType1.Enabled = False
            cboType2.Enabled = False
            cboTrait1.Enabled = False
            cboTrait2.Enabled = False
            nudHP.Enabled = False
            nudAtk.Enabled = False
            nudDef.Enabled = False
            nudSAtk.Enabled = False
            nudSDef.Enabled = False
            nudSpeed.Enabled = False
            nudWeight.Enabled = False
            cboEg1.Enabled = False
            cboEg2.Enabled = False
            cboGenderRS.Enabled = False
            btnImage.Enabled = False
            btnAddMove.Enabled = False
            btnRemoveMove.Enabled = False
        End If
    End Sub

    Private Sub cboType1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboType1.SelectedIndexChanged
        If cboType1.SelectedIndex = -1 Then Return
        If updatingNew Then Return
        selNew.Type1 = BattleData.GetTypeData(cboType1.SelectedItem.ToString).Identity
        If selNew.Type1 = selNew.Type2 Then cboType2.SelectedIndex = 0
    End Sub
    Private Sub cboType2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboType2.SelectedIndexChanged
        If cboType2.SelectedIndex = -1 Then Return
        If updatingNew Then Return
        If cboType2.SelectedIndex = 0 Then
            selNew.Type2 = PokemonType.InvalidId
        Else
            selNew.Type2 = BattleData.GetTypeData(cboType2.SelectedItem.ToString).Identity
            If selNew.Type1 = selNew.Type2 Then cboType2.SelectedIndex = 0
        End If
    End Sub
    Private Sub cboTrait1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboTrait1.SelectedIndexChanged
        If cboTrait1.SelectedIndex = -1 Then Return
        If updatingNew Then Return
        selNew.Trait1 = CType(cboTrait1.SelectedIndex + 1, Trait)
        If selNew.Trait1 = selNew.Trait2 Then cboTrait2.SelectedIndex = 0
    End Sub
    Private Sub cboTrait2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboTrait2.SelectedIndexChanged
        If cboTrait2.SelectedIndex = -1 Then Return
        selNew.Trait2 = CType(cboTrait2.SelectedIndex, Trait)
        If selNew.Trait1 = selNew.Trait2 Then cboTrait2.SelectedIndex = 0
    End Sub
    Private Sub cboEg1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboEg1.SelectedIndexChanged
        If cboEg1.SelectedIndex = -1 Then Return
        If updatingNew Then Return
        selNew.EggGroup1 = CType(cboEg1.SelectedIndex + 1, EggGroup)
        If selNew.EggGroup1 = selNew.EggGroup2 Then cboEg2.SelectedIndex = 0
    End Sub
    Private Sub cboEg2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboEg2.SelectedIndexChanged
        If cboEg2.SelectedIndex = -1 Then Return
        If updatingNew Then Return
        selNew.EggGroup2 = CType(cboEg2.SelectedIndex, EggGroup)
        If selNew.EggGroup1 = selNew.EggGroup2 Then cboEg2.SelectedIndex = 0
    End Sub

    Private Sub nudNew_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudWeight.ValueChanged, _
         nudHP.ValueChanged, nudAtk.ValueChanged, nudDef.ValueChanged, nudSAtk.ValueChanged, nudSDef.ValueChanged, nudSpeed.ValueChanged
        If updatingNew Then Return
        If selNew Is Nothing Then Return
        If sender Is nudWeight Then
            selNew.Weight = nudWeight.Value
        ElseIf sender Is nudHP Then
            selNew.HPBase = Convert.ToByte(nudHP.Value)
        ElseIf sender Is nudAtk Then
            selNew.AttackBase = Convert.ToByte(nudAtk.Value)
        ElseIf sender Is nudDef Then
            selNew.DefenceBase = Convert.ToByte(nudDef.Value)
        ElseIf sender Is nudSAtk Then
            selNew.SpAttackBase = Convert.ToByte(nudSAtk.Value)
        ElseIf sender Is nudSDef Then
            selNew.SpDefenceBase = Convert.ToByte(nudSDef.Value)
        ElseIf sender Is nudSpeed Then
            selNew.SpeedBase = Convert.ToByte(nudSpeed.Value)
        End If
    End Sub

    Private Sub cboGenderRS_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboGenderRS.SelectedIndexChanged
        If cboGenderRS.SelectedIndex = -1 Then Return
        If updatingNew Then Return
        selNew.GenderRestriction = CType(cboGenderRS.SelectedIndex, PokemonGenderRestriction)
    End Sub
    Private Sub txtName_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtName.Validating
        If updatingNew Then Return
        If selNew Is Nothing Then Return
        If Not System.Text.RegularExpressions.Regex.IsMatch(txtName.Text, "\^s*$") Then
            selNew.NameBase = txtName.Text
            lstNewPM.Items(lstNewPM.SelectedIndex) = selNew.NameBase
        Else
            e.Cancel = True
        End If
    End Sub

    Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        Dim pmName As String = GetInput("�������¾��������", "���Ӿ���", 4)
        If pmName = "" Then Return
        If gameData.ContainsPokemon(pmName) Then
            MessageBox.Show("�þ������Ѵ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        If lstNewPM.Items.Contains(pmName) Then
            MessageBox.Show("�þ������Ѵ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        Dim newPokemon As CustomPokemonData = data.AddNewPokemon(pmName)
        lstNewPM.Items.Add(pmName)
    End Sub
    Private Sub btnDelNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelNew.Click
        If lstNewPM.SelectedIndex = -1 Then Return
        Dim remove As CustomPokemonData = data.CustomPokemons(lstNewPM.SelectedIndex)
        data.RemoveCustomPokemon(remove)
        lstNewPM.Items.RemoveAt(lstNewPM.SelectedIndex)
    End Sub
    Private Sub btnAddMove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddMove.Click
        Dim dict As Dictionary(Of String, Integer) = GetMoveList()
        Dim names As String() = New List(Of String)(dict.Keys).ToArray

        Dim index As Integer = GetSelection(names, "��ѡ��Ҫ���ӵļ���", "���Ӽ���")
        If index <> -1 Then
            Dim moveName As String = names(index)
            Dim moveId As Integer = dict(moveName)

            If lstMoves.Items.Contains(moveName) Then
                MessageBox.Show("�ü����Ѵ������б���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                selNew.Moves.Add(moveId)
                lstMoves.Items.Add(moveName)
            End If
        End If
    End Sub
    Private Sub btnRemoveMove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveMove.Click
        If lstMoves.SelectedIndex = -1 Then Return
        selNew.Moves.Remove(BattleData.GetMove(lstMoves.SelectedItem.ToString).Identity)
        lstMoves.Items.RemoveAt(lstMoves.SelectedIndex)
    End Sub
    Private Sub btnImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImage.Click
        Dim editor As New ImageEditorForm(selNew, data)
        editor.ShowDialog()
    End Sub
#End Region

#Region "UpdatePokemon"
    Private selUpdate As UpdatePokemonData
    Private updatingUpdate As Boolean
    Private Sub lstUpdate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstUpdate.SelectedIndexChanged
        If lstUpdate.SelectedIndex <> -1 Then
            updatingUpdate = True
            If cboNewType1.Enabled = False Then
                cboNewType1.Enabled = True
                cboNewType2.Enabled = True
                cboNewTrait1.Enabled = True
                cboNewTrait2.Enabled = True
                nudNewHP.Enabled = True
                nudNewAtk.Enabled = True
                nudNewDef.Enabled = True
                nudNewSAtk.Enabled = True
                nudNewSDef.Enabled = True
                nudNewSpeed.Enabled = True
                nudNewWeight.Enabled = True
                btnDelRM.Enabled = True
                btnAddRM.Enabled = True
                btnDelAM.Enabled = True
                btnAddAM.Enabled = True
            End If
            Dim pm As UpdatePokemonData = data.UpdatePokemons(lstUpdate.SelectedIndex)
            txtNameBase.Text = pm.NameBase
            If pm.Type1 <> PokemonType.InvalidId Then
                cboNewType1.Text = BattleData.GetTypeName(pm.Type1)
            Else
                cboNewType1.SelectedIndex = 0
            End If
            If pm.Type2 <> PokemonType.InvalidId Then
                cboNewType2.Text = BattleData.GetTypeName(pm.Type2)
            Else
                cboNewType2.SelectedIndex = 0
            End If
            cboNewTrait1.SelectedIndex = pm.Trait1
            cboNewTrait2.SelectedIndex = pm.Trait2
            nudNewHP.Value = pm.HPBase
            nudNewAtk.Value = pm.AttackBase
            nudNewDef.Value = pm.DefenceBase
            nudNewSAtk.Value = pm.SpAttackBase
            nudNewSDef.Value = pm.SpDefenceBase
            nudNewSpeed.Value = pm.SpeedBase
            nudNewWeight.Value = Convert.ToDecimal(pm.Weight)
            lstAddMove.Items.Clear()
            For Each addMove As String In pm.AddMoves
                lstAddMove.Items.Add(addMove)
            Next
            lstRemoveMove.Items.Clear()
            For Each removeMove As String In pm.RemoveMoves
                lstRemoveMove.Items.Add(removeMove)
            Next
            selUpdate = pm
            updatingUpdate = False
        Else
            txtNameBase.Enabled = False
            cboNewType1.Enabled = False
            cboNewType2.Enabled = False
            cboNewTrait1.Enabled = False
            cboNewTrait2.Enabled = False
            nudNewHP.Enabled = False
            nudNewAtk.Enabled = False
            nudNewDef.Enabled = False
            nudNewSAtk.Enabled = False
            nudNewSDef.Enabled = False
            nudNewSpeed.Enabled = False
            nudNewWeight.Enabled = False
            btnDelRM.Enabled = False
            btnAddRM.Enabled = False
            btnDelAM.Enabled = False
            btnAddAM.Enabled = False
        End If
    End Sub

    Private Sub nudUpdate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudNewWeight.ValueChanged, _
         nudNewHP.ValueChanged, nudNewAtk.ValueChanged, nudNewDef.ValueChanged, nudNewSAtk.ValueChanged, _
         nudNewSDef.ValueChanged, nudNewSpeed.ValueChanged
        If updatingUpdate Then Return
        If sender Is nudNewWeight Then
            selUpdate.Weight = nudNewWeight.Value
        ElseIf sender Is nudNewHP Then
            selUpdate.HPBase = Convert.ToByte(nudNewHP.Value)
        ElseIf sender Is nudNewAtk Then
            selUpdate.AttackBase = Convert.ToByte(nudNewAtk.Value)
        ElseIf sender Is nudNewDef Then
            selUpdate.DefenceBase = Convert.ToByte(nudNewDef.Value)
        ElseIf sender Is nudNewSAtk Then
            selUpdate.SpAttackBase = Convert.ToByte(nudNewSAtk.Value)
        ElseIf sender Is nudNewSDef Then
            selUpdate.SpDefenceBase = Convert.ToByte(nudNewSDef.Value)
        ElseIf sender Is nudNewSpeed Then
            selUpdate.SpeedBase = Convert.ToByte(nudNewSpeed.Value)
        End If
    End Sub

    Private Sub cboNewType1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboNewType1.SelectedIndexChanged
        If cboNewType1.SelectedIndex = -1 Then Return
        If updatingUpdate Then Return
        If cboNewType1.SelectedIndex = 0 Then
            selUpdate.Type1 = PokemonType.InvalidId
        Else
            selUpdate.Type1 = BattleData.GetTypeData(cboNewType1.SelectedItem.ToString).Identity
        End If
    End Sub
    Private Sub cboNewType2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboNewType2.SelectedIndexChanged
        If cboNewType2.SelectedIndex = -1 Then Return
        If updatingUpdate Then Return
        If cboNewType2.SelectedIndex = 0 Then
            selUpdate.Type2 = PokemonType.InvalidId
        Else
            selUpdate.Type2 = BattleData.GetTypeData(cboNewType2.SelectedItem.ToString).Identity
        End If
    End Sub
    Private Sub cboNewTrait2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboNewTrait2.SelectedIndexChanged
        If updatingUpdate Then Return
        If cboNewTrait2.SelectedIndex = -1 Then Return
        selUpdate.Trait2 = CType(cboNewTrait2.SelectedIndex, Trait)
    End Sub
    Private Sub cboNewTrait1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboNewTrait1.SelectedIndexChanged
        If updatingUpdate Then Return
        If cboNewTrait1.SelectedIndex = -1 Then Return
        selUpdate.Trait1 = CType(cboNewTrait1.SelectedIndex, Trait)
    End Sub

    Private Sub btnAddRM_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddRM.Click
        Dim dict As New Dictionary(Of String, Integer)

        For Each move As MoveLearnData In BattleData.GetPokemon(selUpdate.Identity).Moves
            dict.Add(BattleData.GetMoveName(move.MoveId), move.MoveId)
        Next
        Dim names As String() = New List(Of String)(dict.Keys).ToArray

        Dim index As Integer = GetSelection(names, "��ѡ��Ҫ���ӵļ���", "���Ӽ���")

        If index <> -1 Then
            Dim moveName As String = names(index)
            Dim moveId As Integer = dict(moveName)

            If lstRemoveMove.Items.Contains(moveName) Then
                MessageBox.Show("�ü����Ѵ������б���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                selUpdate.RemoveMoves.Add(moveId)
                lstRemoveMove.Items.Add(moveName)
            End If
        End If
    End Sub
    Private Sub btnAddAM_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddAM.Click
        Dim dict As Dictionary(Of String, Integer) = GetMoveList()
        Dim names As String() = New List(Of String)(dict.Keys).ToArray
        Dim index As Integer = GetSelection(names, "��ѡ��Ҫ���ӵļ���", "���Ӽ���")
        If index <> -1 Then
            Dim moveName As String = names(index)
            Dim moveId As Integer = dict(moveName)
            If lstAddMove.Items.Contains(moveName) OrElse BattleData.GetPokemon(selUpdate.Identity).GetMove(moveId) IsNot Nothing Then
                MessageBox.Show("�ü����Ѵ����ھ���ļ����б���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                selUpdate.AddMoves.Add(moveId)
                lstAddMove.Items.Add(moveName)
            End If
        End If
    End Sub

    Private Sub btnDelRM_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelRM.Click
        If lstRemoveMove.SelectedIndex = -1 Then Return
        selUpdate.RemoveMoves.Remove(BattleData.GetMove(lstRemoveMove.SelectedItem.ToString).Identity)
        lstRemoveMove.Items.RemoveAt(lstRemoveMove.SelectedIndex)
    End Sub
    Private Sub btnDelAM_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelAM.Click
        If lstAddMove.SelectedIndex = -1 Then Return
        selUpdate.AddMoves.Remove(BattleData.GetMove(lstAddMove.SelectedItem.ToString).Identity)
        lstAddMove.Items.RemoveAt(lstAddMove.SelectedIndex)
    End Sub

    Private Sub btnAddUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddUpdate.Click
        Dim pmName As String
        Dim list As String() = GetPokemonList()
        Dim pmList As PokemonData() = BattleData.GetAllPokemons()
        Dim index As Integer = GetSelection(list, "��ѡ��Ҫ�����޸ĵľ���", "�޸ľ���")
        If index <> -1 Then
            pmName = list(index)
            If lstUpdate.Items.Contains(pmName) Then
                MessageBox.Show("�þ����Ѵ������б���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                Dim newPokemon As UpdatePokemonData = data.AddUpdatePokemon(pmList(index).Identity, pmName)
                lstUpdate.Items.Add(pmName)
            End If
        End If
    End Sub

    Private Sub btnDelUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelUpdate.Click
        If lstUpdate.SelectedIndex = -1 Then Return
        data.UpdatePokemons.RemoveAt(lstUpdate.SelectedIndex)
        lstUpdate.Items.RemoveAt(lstUpdate.SelectedIndex)
    End Sub
#End Region

    Private Sub btnAddRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddRemove.Click
        Dim pmName As String
        Dim list As String() = GetPokemonList()
        Dim index As Integer = GetSelection(list, "��ѡ��Ҫ���εľ���", "���ξ���")
        If index <> -1 Then
            pmName = list(index)
            If lstRemovePM.Items.Contains(pmName) Then
                MessageBox.Show("�þ����Ѵ������б���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                data.RemovePokemons.Add(BattleData.GetAllPokemons()(index).Identity)
                lstRemovePM.Items.Add(pmName)
            End If
        End If
    End Sub
    Private Sub btnDelRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelRemove.Click
        If lstRemovePM.SelectedIndex = -1 Then Return
        data.RemovePokemons.RemoveAt(lstRemovePM.SelectedIndex)
        lstRemovePM.Items.RemoveAt(lstRemovePM.SelectedIndex)
    End Sub

    Private Sub btnAddImg_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddImg.Click
        Dim dialog As New OpenFileDialog
        dialog.Title = "����ͼ����Դ"
        dialog.Filter = "λͼ(*.gif,*.png,*.jpg,*.jpeg)|*.gif;*.png;*.jpg;*.jpeg"
        dialog.Multiselect = True
        Dim result As DialogResult = dialog.ShowDialog()
        If result = Windows.Forms.DialogResult.OK Then
            Try
                For Each filename As String In dialog.FileNames
                    Dim image As Bitmap = CType(Bitmap.FromFile(filename), Bitmap)
                    data.Images.Add(image)
                    ImageList.Images.Add(image)
                    lstImages.Items.Add("", ImageList.Images.Count - 1)
                Next
            Catch ex As Exception
                MessageBox.Show(ex.Message, "����", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub
    Private Sub btnDelImg_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelImg.Click
        For i As Integer = lstImages.SelectedIndices.Count - 1 To 0 Step -1
            Dim index As Integer = lstImages.SelectedIndices(i)
            If index <> lstImages.Items.Count Then
                For j As Integer = index + 1 To lstImages.Items.Count - 1
                    Dim item As ListViewItem = lstImages.Items(j)
                    item.ImageIndex -= 1
                Next
            End If
            For Each pm As CustomPokemonData In data.CustomPokemons
                If pm.FrontImage = index Then pm.FrontImage = -1
                If pm.FrontImageF = index Then pm.FrontImageF = -1
                If pm.BackImage = index Then pm.BackImage = -1
                If pm.BackImageF = index Then pm.BackImageF = -1
                If pm.Frame = index Then pm.Frame = -1
                If pm.FrameF = index Then pm.FrameF = -1
                If pm.Icon = index Then pm.Icon = -1
            Next
            data.Images.RemoveAt(index)
            ImageList.Images.RemoveAt(index)
            lstImages.Items.RemoveAt(index)
        Next
    End Sub

    Private Function GetMoveList() As Dictionary(Of String, Integer)
        Dim movedatas As MoveData() = BattleData.GetAllMoves
        Dim moves As Dictionary(Of String, Integer) = New Dictionary(Of String, Integer)
        For Each item As MoveData In movedatas
            If item.Name <> "ƴ��" Then
                moves.Add(item.Name, item.Identity)
            End If
        Next
        Return moves
    End Function
    Private Function GetPokemonList() As String()
        Dim pokemons As New List(Of String)
        Dim array As PokemonData() = BattleData.GetAllPokemons()
        For Each pm As PokemonData In array
            pokemons.Add(pm.Name)
        Next
        Return pokemons.ToArray()
    End Function

    Private Sub OpenData(ByVal fileName As String)
        Try
            Using stream As New FileStream(fileName, FileMode.Open)
                Dim formatter As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
                data = CType(formatter.Deserialize(stream), CustomGameData)
                tempData = data.Clone
                My.Settings.CustomDataPath = fileName
                OpenData()
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message, "����", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub OpenData()
        Me.Text = data.Name & " - �Զ���������Ŀ"
        For Each add As CustomPokemonData In data.CustomPokemons
            lstNewPM.Items.Add(add.NameBase)
        Next
        For Each update As UpdatePokemonData In data.UpdatePokemons
            lstUpdate.Items.Add(update.NameBase)
        Next
        For Each remove As Integer In data.RemovePokemons
            lstRemovePM.Items.Add(BattleData.GetPokemon(remove).Name)
        Next
        ImageList.Images.AddRange(data.Images.ToArray)
        For i As Integer = 0 To ImageList.Images.Count - 1
            lstImages.Items.Add("", i)
        Next
        btnAddNew.Enabled = True
        btnDelNew.Enabled = True
        btnAddUpdate.Enabled = True
        btnDelUpdate.Enabled = True
        btnAddRemove.Enabled = True
        btnDelRemove.Enabled = True
        btnAddImg.Enabled = True
        btnDelImg.Enabled = True
    End Sub
    Private Function CloseData() As Boolean
        If data Is Nothing Then Return True
        If Not data.Equals(tempData) Then
            Dim result As DialogResult = MessageBox.Show("�Ƿ񱣴�Ķ�?", "��ʾ", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation)
            Select Case result
                Case Windows.Forms.DialogResult.Yes
                    If My.Settings.CustomDataPath = "" Then
                        SaveTo()
                    Else
                        SaveData()
                    End If
                Case Windows.Forms.DialogResult.Cancel
                    Return False
            End Select
        End If
        selNew = Nothing
        selUpdate = Nothing
        lstNewPM.SelectedIndex = -1
        lstUpdate.SelectedIndex = -1
        lstNewPM.Items.Clear()
        lstUpdate.Items.Clear()
        ImageList.Images.Clear()
        lstImages.Clear()
        lstRemovePM.Items.Clear()
        Return True
    End Function
    Private Sub SaveData()
        If data Is Nothing Then Return
        Try
            Using stream As New FileStream(My.Settings.CustomDataPath, FileMode.Create)
                Dim formatter As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
                formatter.Serialize(stream, data)
                tempData = data.Clone
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message, "����", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub SaveTo()
        If data Is Nothing Then Return
        Dim fileDialog As New SaveFileDialog()
        fileDialog.Title = "����PBO�Զ���������Ŀ"
        fileDialog.Filter = "PBO�Զ���������Ŀ(*.pcp)|*.pcp"
        Dim result As DialogResult = fileDialog.ShowDialog()
        If result = Windows.Forms.DialogResult.OK OrElse result = Windows.Forms.DialogResult.Yes Then
            My.Settings.CustomDataPath = fileDialog.FileName
            SaveData()
        End If
    End Sub
    Private Sub �½�NToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles �½�NToolStripMenuItem.Click
        If Not CloseData() Then Return
        Dim dataName As String = GetInput("�������µ��Զ���������Ŀ������", "�½��Զ���������Ŀ", 20)
        If dataName = "" Then Return
        data = New CustomGameData(dataName)
        tempData = Nothing
        My.Settings.CustomDataPath = ""
        OpenData()
    End Sub
    Private Sub ��OToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ��OToolStripMenuItem.Click
        If Not CloseData() Then Return
        Dim dialog As New OpenFileDialog
        dialog.Title = "��PBO�Զ���������Ŀ"
        dialog.Filter = "PBO�Զ���������Ŀ(*.pcp)|*.pcp"
        Dim result As DialogResult = dialog.ShowDialog()
        If result = Windows.Forms.DialogResult.OK Then OpenData(dialog.FileName)
    End Sub
    Private Sub ����SToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ����SToolStripMenuItem.Click
        If My.Settings.CustomDataPath = "" Then
            SaveTo()
        Else
            SaveData()
        End If
    End Sub
    Private Sub ����ΪAToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ����ΪAToolStripMenuItem.Click
        SaveTo()
    End Sub
    Private Sub ��������BToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ��������BToolStripMenuItem.Click
        If data Is Nothing Then Return
        Dim cdPath As String = Path.Combine(Application.StartupPath, "CustomData")
        If Not Directory.Exists(cdPath) Then Directory.CreateDirectory(cdPath)
        Dim fileName As String = Path.Combine(cdPath, data.Name & ".pcd")
        If File.Exists(fileName) Then
            Dim result As DialogResult = MessageBox.Show("�ļ��Ѵ���,�Ƿ񸲸�", "��������", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = Windows.Forms.DialogResult.No Then Return
        End If
        Try
            data.Save(fileName)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "����", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub �˳�XToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles �˳�XToolStripMenuItem.Click
        Close()
    End Sub

    Private Sub FrmCustomData_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If Not CloseData() Then e.Cancel = True : Return
    End Sub

    Private Sub ������Ŀ��RToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ������Ŀ��RToolStripMenuItem.Click
        If data Is Nothing Then Return
        Dim dataName As String = GetInput("�������µ��Զ���������Ŀ����", "�����Զ���������Ŀ����", 20, False, data.Name)
        If dataName = "" Then Return
        data.Name = dataName
    End Sub
End Class