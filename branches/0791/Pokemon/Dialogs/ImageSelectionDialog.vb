Imports System.Windows.Forms

Public Class ImageSelectionDialog

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        lstImages.SelectedIndices.Clear()
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
    Public ReadOnly Property SelectedIndex() As Integer
        Get
            If lstImages.SelectedIndices.Count = 0 Then Return -1
            Return lstImages.SelectedIndices(0)
        End Get
    End Property

    Public Sub New(ByVal list As Image(), ByVal message As String, ByVal imageSize As Size, Optional ByVal title As String = Nothing)

        ' �˵����� Windows ���������������ġ�
        InitializeComponent()

        ' �� InitializeComponent() ����֮�������κγ�ʼ����
        lblMsg.Text = message
        If title Is Nothing Then
            Me.Text = My.Application.Info.Title
        Else
            Me.Text = title
        End If
        lstImages.LargeImageList = ImageList
        ImageList.ImageSize = imageSize
        ImageList.Images.AddRange(list)
        For i As Integer = 0 To list.Length - 1
            lstImages.Items.Add("", i)
        Next
    End Sub
End Class