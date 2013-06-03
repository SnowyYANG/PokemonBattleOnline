Imports PokemonBattle.RoomList.Client
Imports PokemonBattle.RoomListClient
Imports NetworkLib
Imports PokemonBattle.BattleRoom.Client
Imports PokemonBattle.RoomClient
Public Class RoomListForm


    Private _client As RoomListClient
    Private _userInfo As User
    Public Sub New(ByVal user As User)

        ' 此调用是 Windows 窗体设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        Icon = My.Resources.PokemonBall
        RoomList.ListViewItemSorter = New ListSorter(1)
        _userInfo = user
        Connect()
    End Sub

    Private Sub lstRooms_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RoomList.SelectedIndexChanged
        If RoomList.SelectedItems.Count > 0 Then
            Dim info As Room = _client.GetRoom(CInt(RoomList.SelectedItems(0).Name))
            InfoText.Text = info.Description
            EnterButton.Enabled = True
        Else
            EnterButton.Enabled = False
            InfoText.Clear()
        End If
    End Sub

    Private Sub Connect()
        _client = New RoomListClient(My.Settings.ServerAddress)
        AddHandler _client.OnUserAddRooms, AddressOf UpdateList
        AddHandler _client.OnDisconnected, AddressOf OnDisconnected
        AddHandler _client.OnConnectFail, AddressOf OnConnectFail
        AddHandler _client.OnAddRoomInfo, AddressOf AddRoom
        AddHandler _client.OnUpdateRoomInfo, AddressOf UpdateRoom
        AddHandler _client.OnRemoveRoomInfo, AddressOf RemoveRoom

        _client.Initialize()
        _client.RunThread()
    End Sub

    Private Sub UpdateList(ByVal rooms As List(Of Room))
        If Not InvokeRequired Then

            RoomList.BeginUpdate()
            RoomList.Items.Clear()

            For i As Integer = 0 To rooms.Count - 1
                Dim roomInfo As Room = rooms(i)
                Dim item As New ListViewItem(New String(2) {roomInfo.Name, roomInfo.UserCount.ToString, roomInfo.MaxUser.ToString})
                item.Name = roomInfo.Identity.ToString()
                RoomList.Items.Add(item)
            Next
            RoomList.EndUpdate()
        Else
            Invoke(New RoomListDelegate(AddressOf UpdateList), rooms)
        End If
    End Sub
    Private Sub AddRoom(ByVal info As Room)
        If Not InvokeRequired Then
            Dim item As New ListViewItem(New String(2) {info.Name, info.UserCount.ToString, info.MaxUser.ToString})
            item.Name = info.Identity.ToString()
            RoomList.Items.Add(item)
        Else
            Invoke(New RoomDelegate(AddressOf AddRoom), info)
        End If
    End Sub
    Private Sub UpdateRoom(ByVal info As Room)
        If Not InvokeRequired Then
            Dim item As ListViewItem() = RoomList.Items.Find(info.Identity.ToString, False)
            If item.Length > 0 Then
                item(0).SubItems(0).Text = info.Name
                item(0).SubItems(1).Text = info.UserCount.ToString()
                item(0).SubItems(2).Text = info.MaxUser.ToString()
            End If
        Else
            Invoke(New RoomDelegate(AddressOf UpdateRoom), info)
        End If
    End Sub
    Private Sub RemoveRoom(ByVal identity As Integer)
        If Not InvokeRequired Then
            RoomList.Items.RemoveByKey(identity.ToString)
        Else
            Invoke(New RemoveRoomDelegate(AddressOf RemoveRoom), identity)
        End If
    End Sub

    Private Sub OnConnectFail(ByVal exception As NetworkException)
        ShowInformation(Me, "无法连接到服务器", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ShowAdvancedOption()
    End Sub

    Private Sub OnDisconnected()
        ShowInformation(Me, "与服务器断开了连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ShowAdvancedOption()
    End Sub

    Private Sub btnEnter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles EnterButton.Click
        If RoomList.SelectedIndices.Count = 0 Then Return
        Dim info As Room = _client.GetRoom(CInt(RoomList.SelectedItems(0).Name))
        CType(MdiParent, MainForm).BuildRoomUserForm(_userInfo, info.Address, info.Name).Show()
        Close()
    End Sub

    Private _address As String
    Private _roomName As String

    Private Sub lstRooms_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles RoomList.DoubleClick
        EnterButton.PerformClick()
    End Sub

    Private Sub ShowAdvancedOption()
        If InvokeRequired Then
            Invoke(New WorkDelegate(AddressOf ShowAdvancedOption))
        Else
            Dim advanceForm As New AdvanceLinkForm(_userInfo)
            advanceForm.MdiParent = Me.MdiParent
            advanceForm.Show()
            Close()
        End If
    End Sub

    Private Sub Disconnect()
        If _client IsNot Nothing Then
            _client.Stop()
            _client = Nothing
        End If
    End Sub

    Private Sub FrmRoomList_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Disconnect()
    End Sub

    Private Sub btnAdvance_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AdvanceButton.Click
        ShowAdvancedOption()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Close()
    End Sub
End Class