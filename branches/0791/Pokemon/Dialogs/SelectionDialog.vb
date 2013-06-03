Imports System.Windows.Forms

Public Class SelectionDialog

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        lstSelection.SelectedIndex = -1
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Public Sub New(ByVal list As String(), ByVal message As String, Optional ByVal title As String = Nothing)

        ' 此调用是 Windows 窗体设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        lstSelection.Items.AddRange(list)
        lblMsg.Text = message
        If title Is Nothing Then
            Me.Text = My.Application.Info.Title
        Else
            Me.Text = title
        End If
    End Sub
    Public ReadOnly Property SelectedIndex() As Integer
        Get
            Return lstSelection.SelectedIndex
        End Get
    End Property

    Private Sub txtSearch_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSearch.TextChanged
        If txtSearch.Text = "" Then Return
        Dim index As Integer = lstSelection.FindString(txtSearch.Text)
        If index <> -1 Then
            lstSelection.SelectedIndex = index
        End If
    End Sub
End Class
