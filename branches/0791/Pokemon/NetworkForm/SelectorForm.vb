Public Class SelectorForm

    Private WithEvents motherForm As MainForm
    Public Sub New()

        ' 此调用是 Windows 窗体设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        Me.Icon = My.Resources.PokemonBall
    End Sub

    Private Sub PersonalButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PersonalButton.Click
        Dim perBattle As New DirectBattleForm()
        perBattle.MdiParent = Me.MdiParent
        perBattle.Show()
        PersonalButton.Enabled = False
        AddHandler perBattle.FormClosed, AddressOf PersonalBattleForm_Closed
    End Sub

    Private Sub PersonalBattleForm_Closed(ByVal sender As Object, ByVal e As FormClosedEventArgs)
        RemoveHandler CType(sender, Form).FormClosed, AddressOf PersonalBattleForm_Closed
        PersonalButton.Enabled = True
    End Sub

    Private Sub LinkRoomButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LinkRoomButton.Click
        If My.Settings.AutoLoginUser AndAlso Not String.IsNullOrEmpty(My.Settings.UserName) Then
            Dim user As New PokemonBattle.BattleRoom.Client.User
            user.Name = My.Settings.UserName
            user.ImageKey = My.Settings.ImageIndex
            Logon(user, False)
        Else
            Dim logonForm As New LogonRoomForm()
            If logonForm.ShowDialog() = Windows.Forms.DialogResult.OK Then
                Logon(logonForm.UserInfo, logonForm.Advanced)
            End If
        End If
    End Sub
    Private Sub Logon(ByVal user As PokemonBattle.BattleRoom.Client.User, ByVal advance As Boolean)
        Dim linkForm As Form
        If advance Then
            linkForm = New AdvanceLinkForm(user)
        Else
            linkForm = New RoomListForm(user)
        End If
        linkForm.MdiParent = Me.MdiParent
        linkForm.Show()
    End Sub

    Private Sub SelectorForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        motherForm = CType(MdiParent, MainForm)
        If motherForm.Team Is Nothing Then
            LinkRoomButton.Enabled = False
            PersonalButton.Enabled = False
        End If
    End Sub

    Private Sub mdiForm_TeamFormHided(ByVal sender As Object, ByVal e As System.EventArgs) Handles motherForm.TeamFormHided
        If LinkRoomButton.Enabled = False AndAlso motherForm.Team IsNot Nothing Then
            LinkRoomButton.Enabled = True
            PersonalButton.Enabled = True
        End If
    End Sub
End Class