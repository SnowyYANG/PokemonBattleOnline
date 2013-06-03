
Imports PokemonBattle.PokemonData
Friend Class FrmSearchPM

    Private gamedata As GameData
    Private dexForm As FrmDataDex

    Private pmSorter As ListSorter
    Public Sub New(ByVal imglst As ImageList, ByVal dex As FrmDataDex)

        ' 此调用是 Windows 窗体设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        Me.Icon = My.Resources.PokemonBall

        lstMove.SmallImageList = imglst
        gamedata = CType(BattleData.DataProvider, PokemonBattleOnline.GameData)
        dexForm = dex
        UpdateMe()
    End Sub

    Public Sub UpdateMe()
        cboType.Items.Clear()
        cboType2.Items.Clear()
        cboType2.Items.Add("无")
        For Each type As PokemonType In BattleData.GetAllTypes()
            If type.Name <> "无" Then cboType.Items.Add(type.Name) _
                : cboType2.Items.Add(type.Name)
        Next
        lstMove.Items.Clear()
        For Each move As MoveData In BattleData.GetAllMoves()
            lstMove.Items.Add(New ListViewItem(move.Name, move.Type - 1) With {.Tag = move.Identity})
        Next
        pmSorter = New ListSorter()
        lstResult.ListViewItemSorter = pmSorter
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        lstResult.Items.Clear()
        lstResult.ListViewItemSorter = Nothing
        Dim result As New List(Of PokemonData)
        result.AddRange(BattleData.GetAllPokemons())

        If chkType.Checked AndAlso cboType.SelectedIndex <> -1 Then result = SearchByType(cboType.SelectedItem.ToString, result)
        If chkType2.Checked AndAlso cboType2.SelectedIndex <> -1 Then result = SearchByType(cboType2.SelectedItem.ToString, result)
        If chkTrait.Checked AndAlso cboTrait.SelectedIndex <> -1 Then result = SearchByTrait(CType(cboTrait.SelectedIndex + 1, Trait), result)
        If chkMove.Checked AndAlso lstMove.CheckedItems.Count > 0 Then
            For Each move As ListViewItem In lstMove.CheckedItems
                result = SearchByMove(CInt(move.Tag), result)
            Next
        End If
        For Each pm As PokemonData In result
            Dim type As String = BattleData.GetTypeName(pm.Type1)
            If pm.Type2 <> PokemonType.InvalidId Then type &= "/" & BattleData.GetTypeName(pm.Type2)
            Dim item As New ListViewItem(New String() _
                {pm.Number.ToString, pm.Name, type, pm.HPBase.ToString, pm.AttackBase.ToString, _
                pm.DefenceBase.ToString, pm.SpeedBase.ToString, pm.SpAttackBase.ToString, pm.SpDefenceBase.ToString})
            lstResult.Items.Add(item)
        Next
        pmSorter = New ListSorter
        lstResult.ListViewItemSorter = pmSorter
    End Sub

    Private Sub chkType_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkType.CheckedChanged
        If chkType.Checked OrElse chkType2.Checked OrElse chkTrait.Checked OrElse chkMove.Checked Then
            cmdSearch.Enabled = True
        Else
            cmdSearch.Enabled = False
        End If
    End Sub

    Private Sub chkType2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkType2.CheckedChanged
        If chkType.Checked OrElse chkType2.Checked OrElse chkTrait.Checked OrElse chkMove.Checked Then
            cmdSearch.Enabled = True
        Else
            cmdSearch.Enabled = False
        End If
    End Sub

    Private Sub chkTrait_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkTrait.CheckedChanged
        If chkType.Checked OrElse chkType2.Checked OrElse chkTrait.Checked OrElse chkMove.Checked Then
            cmdSearch.Enabled = True
        Else
            cmdSearch.Enabled = False
        End If
    End Sub

    Private Sub chkMove_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMove.CheckedChanged
        If chkType.Checked OrElse chkType2.Checked OrElse chkTrait.Checked OrElse chkMove.Checked Then
            cmdSearch.Enabled = True
        Else
            cmdSearch.Enabled = False
        End If
    End Sub

    Private Function SearchByType(ByVal type As String, ByVal PMList As List(Of PokemonData)) As List(Of PokemonData)
        Dim newList As New List(Of PokemonData)
        For Each pm As PokemonData In PMList
            If type = "无" Then
                If pm.Type2 = PokemonType.InvalidId Then newList.Add(pm)
            Else
                Dim id As Integer = BattleData.GetTypeData(type).Identity
                If pm.Type1 = id OrElse pm.Type2 = id Then newList.Add(pm)
            End If
        Next
        Return newList
    End Function

    Private Function SearchByTrait(ByVal trait As Trait, ByVal PMList As List(Of PokemonData)) As List(Of PokemonData)
        Dim newList As New List(Of PokemonData)
        For Each pm As PokemonData In PMList
            If pm.trait1 = trait OrElse pm.trait2 = trait Then newList.Add(pm)
        Next
        Return newList
    End Function

    Private Function SearchByMove(ByVal move As Integer, ByVal PMList As List(Of PokemonData)) As List(Of PokemonData)
        Dim newList As New List(Of PokemonData)
        For Each pm As PokemonData In PMList
            If pm.GetMove(move) IsNot Nothing Then newList.Add(pm)
        Next
        Return newList
    End Function

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Hide()
    End Sub

    Private Sub lstResult_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lstResult.ColumnClick
        If pmSorter IsNot Nothing Then pmSorter.SetColumn(e.Column) : lstResult.Sort()
    End Sub

    Private Sub lstResult_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstResult.SelectedIndexChanged
        If lstResult.SelectedItems.Count > 0 Then
            dexForm.SetPMIndex(Convert.ToInt32(lstResult.SelectedItems(0).Text))
        End If
    End Sub
End Class