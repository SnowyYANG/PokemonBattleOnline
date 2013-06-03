Imports System.Windows.Forms

Public Class InputDialog

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        txtInput.Text = ""
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Public ReadOnly Property InputText() As String
        Get
            Return txtInput.Text.Trim
        End Get
    End Property

    Public Sub New(ByVal prompt As String, Optional ByVal title As String = Nothing, _
        Optional ByVal textLength As Int32 = 0, Optional ByVal muiltLine As Boolean = False, Optional ByVal defaultResponse As String = Nothing)

        ' 此调用是 Windows 窗体设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        lblMsg.Text = prompt
        If title Is Nothing Then
            Me.Text = My.Application.Info.Title
        Else
            Me.Text = title
        End If
        If defaultResponse IsNot Nothing Then
            txtInput.Text = defaultResponse
        End If
        If textLength <> 0 Then txtInput.MaxLength = textLength
        If muiltLine Then
            txtInput.Multiline = True
            Me.Size = New Size(Me.Width, Me.Height + txtInput.Height * 2)
            txtInput.Height *= 3
        End If
    End Sub
End Class
