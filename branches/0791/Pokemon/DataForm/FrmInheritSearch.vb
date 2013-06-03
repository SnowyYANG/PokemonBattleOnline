Friend Class FrmInheritSearch

    Private gamedata As GameData
    Private dexForm As FrmDataDex
    Private pm As PokemonData

    Private pmSorter As ListSorter
    Public Sub New(ByVal pmValue As PokemonData, ByVal dex As FrmDataDex)

        ' 此调用是 Windows 窗体设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        Me.Icon = My.Resources.PokemonBall

        gamedata = CType(BattleData.DataProvider, PokemonBattleOnline.GameData)
        dexForm = dex
        pm = pmValue
        UpdateMe()
    End Sub

    Public Sub UpdateMe()
        For Each move As MoveLearnData In pm.Moves
            If move.LearnBy.Contains("遗传") Then
                cboMove1.Items.Add(BattleData.GetMoveName(move.MoveId))
                cboMove2.Items.Add(BattleData.GetMoveName(move.MoveId))
                cboMove3.Items.Add(BattleData.GetMoveName(move.MoveId))
                cboMove4.Items.Add(BattleData.GetMoveName(move.MoveId))
            End If
        Next

        pmSorter = New ListSorter()
        lstResult.ListViewItemSorter = pmSorter
        Text &= " - " & pm.Name
    End Sub

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Me.Close()
    End Sub

    Private Sub lstResult_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lstResult.ColumnClick
        If pmSorter IsNot Nothing Then pmSorter.SetColumn(e.Column) : lstResult.Sort()
    End Sub

    Private Sub lstResult_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstResult.SelectedIndexChanged
        If lstResult.SelectedItems.Count > 0 Then
            dexForm.SetPMIndex(Convert.ToInt32(lstResult.SelectedItems(0).Text))
        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        lstResult.Items.Clear()
        lstResult.ListViewItemSorter = Nothing

        Dim moves As New List(Of Integer)
        If cboMove1.SelectedIndex <> -1 Then
            moves.Add(BattleData.GetMove(cboMove1.SelectedItem.ToString).Identity)
        End If
        If cboMove2.SelectedIndex > 0 Then
            Dim id As Integer = BattleData.GetMove(cboMove2.SelectedItem.ToString).Identity
            If Not moves.Contains(id) Then moves.Add(id)
        End If
        If cboMove3.SelectedIndex > 0 Then
            Dim id As Integer = BattleData.GetMove(cboMove3.SelectedItem.ToString).Identity
            If Not moves.Contains(id) Then moves.Add(id)
        End If
        If cboMove4.SelectedIndex > 0 Then
            Dim id As Integer = BattleData.GetMove(cboMove4.SelectedItem.ToString).Identity
            If Not moves.Contains(id) Then moves.Add(id)
        End If
        If moves.Count > 0 Then
            Dim list As New List(Of PokemonData)

            Dim except As New List(Of EggGroup)
            Dim list1 As List(Of PokemonData) = gamedata.GetPMs(moves, gamedata.GetEggGroup(pm.eggGroup1, except))
            except.Add(pm.eggGroup1)
            For Each data As PokemonData In list1
                If gamedata.CheckMoves(moves, data) Then list.Add(data)
            Next

            Dim list2 As List(Of PokemonData)
            If pm.eggGroup2 <> EggGroup.无 Then
                list2 = gamedata.GetPMs(moves, gamedata.GetEggGroup(pm.eggGroup2, except))
                For Each data As PokemonData In list2
                    If gamedata.CheckMoves(moves, data) Then list.Add(data)
                Next
            End If

            For Each data As PokemonData In list
                Dim type As String = BattleData.GetTypeName(data.Type1)
                If data.Type2 <> PokemonType.InvalidId Then type &= "/" & BattleData.GetTypeName(data.Type2)
                Dim item As New ListViewItem(New String() _
                    {data.Number.ToString, data.Name, type, data.HPBase.ToString, data.AttackBase.ToString, data.DefenceBase.ToString, _
                    data.SpeedBase.ToString, data.SpAttackBase.ToString, data.SpDefenceBase.ToString})
                lstResult.Items.Add(item)
            Next

        End If

        pmSorter = New ListSorter
        lstResult.ListViewItemSorter = pmSorter
    End Sub

    Private Sub FrmInheritSearch_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        dexForm.cmdInherit.Enabled = True
    End Sub
End Class