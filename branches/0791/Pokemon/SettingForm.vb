Public Class SettingForm

    Public Sub New()

        ' 此调用是 Windows 窗体设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        Me.Icon = My.Resources.PokemonBall
        cboPMName.SelectedIndex = My.Settings.CustomName
        cboLogPos.SelectedIndex = My.Settings.LogPosition
    End Sub

    Private Sub chkAutoSaveLog_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAutoSaveLog.CheckedChanged
        txtLogTitle.Enabled = chkAutoSaveLog.Checked
        btnLogPath.Enabled = chkAutoSaveLog.Checked
    End Sub

    Private Sub chkAutoSaveReplay_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAutoSaveReplay.CheckedChanged
        txtRepTitle.Enabled = chkAutoSaveReplay.Checked
        btnRepPath.Enabled = chkAutoSaveReplay.Checked
    End Sub

    Private Sub btnLogPath_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogPath.Click
        Dim dialog As New FolderBrowserDialog()
        Dim result As DialogResult = dialog.ShowDialog
        If result = Windows.Forms.DialogResult.OK OrElse result = Windows.Forms.DialogResult.Yes Then
            If dialog.SelectedPath <> "" Then txtLogPath.Text = dialog.SelectedPath
        End If
    End Sub

    Private Sub btnRepPath_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRepPath.Click
        Dim dialog As New FolderBrowserDialog()
        Dim result As DialogResult = dialog.ShowDialog
        If result = Windows.Forms.DialogResult.OK OrElse result = Windows.Forms.DialogResult.Yes Then
            If dialog.SelectedPath <> "" Then txtRepPath.Text = dialog.SelectedPath
        End If
    End Sub

    Private Sub cboPMName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboPMName.SelectedIndexChanged
        If cboPMName.SelectedIndex <> -1 Then My.Settings.CustomName = cboPMName.SelectedIndex
    End Sub

    Private Sub cboLogPos_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboLogPos.SelectedIndexChanged
        If cboLogPos.SelectedIndex <> -1 Then My.Settings.LogPosition = cboLogPos.SelectedIndex
    End Sub
End Class