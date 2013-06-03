Imports System.Runtime.Serialization.Formatters.Binary
Friend Class IndexForm

    Private mdiForm As MainForm

    Public Sub New()

        ' 此调用是 Windows 窗体设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        Me.Icon = My.Resources.PokemonBall
        Me.Text = "Quick Start"
    End Sub

    Private Sub FrmIndex_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        If mdiForm IsNot Nothing Then
            mdiForm.Close()
            mdiForm = Nothing
        End If
    End Sub

    Private Sub FrmIndex_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        mdiForm = CType(Me.MdiParent, MainForm)
    End Sub

    Private Sub cmdTeam_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTeam.Click
        mdiForm.ShowTeamEditor()
    End Sub

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        mdiForm.Close()
    End Sub

    Private Sub cmdBattle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBattle.Click
        Dim selector As New SelectorForm()
        selector.MdiParent = Me.MdiParent
        Hide()
        selector.Show()
        AddHandler selector.FormClosed, AddressOf BattleFormClosed
    End Sub

    Public Sub UpdateTeamPath()
        lblTeam.Text = My.Settings.teamPath
    End Sub
    Public Sub TeamFormHided(ByVal sender As Object, ByVal e As System.EventArgs)
        RemoveHandler CType(sender, MainForm).TeamFormHided, AddressOf TeamFormHided
        UpdateTeamPath()
        Show()
    End Sub
    Public Sub BattleFormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs)
        RemoveHandler CType(sender, Form).FormClosed, AddressOf BattleFormClosed
        Show()
    End Sub

    Private Sub FrmIndex_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.VisibleChanged
        If Me.Visible Then
            UpdateTeamPath()
        End If
    End Sub
End Class