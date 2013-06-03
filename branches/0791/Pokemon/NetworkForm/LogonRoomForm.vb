Imports System.Text.RegularExpressions
Imports PokemonBattle.BattleRoom.Client
Friend Class LogonRoomForm

    Public Sub New()

        ' 此调用是 Windows 窗体设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。

        Me.Icon = My.Resources.PokemonBall
        MyImage.Image = UserImages.Images(ImageIndex)
    End Sub

    Private Sub LogonButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LogonButton.Click
        If Regex.IsMatch(NameText.Text, "^\s*$") Then Return
        My.Settings.AutoLoginUser = AutoLogonCheck.Checked
        My.Settings.UserName = NameText.Text
        DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub ImageUpnButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImageUpnButton.Click
        If ImageIndex < UserImages.Images.Count - 1 Then
            ImageIndex += 1
            MyImage.Image = UserImages.Images(ImageIndex)
        End If
    End Sub

    Private Sub ImageDownButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImageDownButton.Click
        If ImageIndex > 0 Then
            ImageIndex -= 1
            MyImage.Image = UserImages.Images(ImageIndex)
        End If
    End Sub

    Public Property ImageIndex() As Integer
        Get
            Return My.Settings.ImageIndex
        End Get
        Set(ByVal value As Integer)
            My.Settings.ImageIndex = Convert.ToByte(value)
        End Set
    End Property

    Public ReadOnly Property Advanced() As Boolean
        Get
            Return AdvanceLinkButton.Checked
        End Get
    End Property

    Public ReadOnly Property UserInfo() As User
        Get
            Dim info As New User()
            info.ImageKey = Convert.ToByte(ImageIndex)
            info.Name = NameText.Text
            Return info
        End Get
    End Property
End Class