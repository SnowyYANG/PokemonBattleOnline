Imports System.Text.RegularExpressions
Imports PokemonBattle.BattleRoom.Client
Imports PokemonBattle.RoomClient
Public Class AdvanceLinkForm

    Private _userInfo As User
    Public Sub New(ByVal user As User)

        ' 此调用是 Windows 窗体设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        Me.Icon = My.Resources.PokemonBall
        If My.Settings.RoomAddress Is Nothing Then
            My.Settings.RoomAddress = New System.Collections.Specialized.StringCollection
        End If
        For Each item As String In My.Settings.RoomAddress
            AddressCombo.Items.Add(item)
        Next
        _userInfo = user
    End Sub

    Private Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles OKButton.Click
        If Not New Regex("^\s*$").IsMatch(AddressCombo.Text) Then
            If Not My.Settings.RoomAddress.Contains(AddressCombo.Text) Then
                My.Settings.RoomAddress.Add(AddressCombo.Text)
                If My.Settings.RoomAddress.Count > 10 Then My.Settings.RoomAddress.RemoveAt(0)
            End If
            CType(MdiParent, MainForm).BuildRoomUserForm(_userInfo, AddressCombo.Text, AddressCombo.Text).Show()
            Close()
        End If
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Close()
    End Sub
End Class