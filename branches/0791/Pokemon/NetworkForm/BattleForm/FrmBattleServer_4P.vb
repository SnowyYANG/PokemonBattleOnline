Friend Class FrmBattleServer_4P

    Private clients As NetworkClient()
    Private clientNames As String() 
    Private teams As TeamInfo()
  
    Public Sub New(ByVal myTeamValue As TeamInfo, ByVal name As String, _
        ByVal data As GameData, ByVal rules As BattleRules)
        MyBase.New(myTeamValue, name, data, battleMode.Double_4P, rules)
        ' 此调用是 Windows 窗体设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
    End Sub
    Protected Overrides Sub HandleClientException(ByVal sender As NetworkClient, ByVal ex As System.Exception)
        If clients Is Nothing Then Return 
        Dim index As Integer = Array.IndexOf(Of NetworkClient)(clients, sender) + 1
        PlayerExit(index)
    End Sub

    Protected Overrides Sub GetClient()
        clients = New NetworkClient(3) {}
        clientNames = New String(3) {}
        teams = New TeamInfo(3) {}
        clientNames(0) = myName
        teams(0) = myTeamInfo

        SetProgress(20)
        SetProgressText("正在接收数据")
        Dim seed As Integer = New Random().Next(0, 500)
        For i As Integer = 1 To 3
            AddPlayer(seed)
        Next
        SetProgress(40)
        SetProgressText("正在发送数据")
        For i As Integer = 1 To 3
            For j As Integer = 0 To 3
                If i <> j Then
                    clients(i).Send(clientNames(j))
                End If
            Next
        Next
        If Convert.ToBoolean(battleRules And battleRules.Random) Then
            Dim random As New Random(seed)
            For i As Integer = 0 To 3
                teams(i) = GameData.RandomTeam(random)
            Next
        Else
            For i As Integer = 1 To 3
                For j As Integer = 0 To 3
                    If i <> j Then
                        clients(i).Serialize(teams(j))
                    End If
                Next
            Next
        End If

        SetProgress(80)
        SetProgressText("正在初始化对战")
        StartBattle(teams, seed, clientNames, MyBase.battleRules)

        SetProgress(100)
        SetProgressText("初始化完毕")
        HideProgress()
        For Each client As NetworkClient In clients
            If client IsNot Nothing Then client.Run(AddressOf GetPlayerMessage, AddressOf HandleClientException)
        Next
    End Sub
    Private Sub AddPlayer(ByVal seed As Integer)
        Dim client As New NetworkClient(listener.AcceptTcpClient)
        Dim index As Integer = Convert.ToInt32(client.ReadString)

        clients(index - 1) = client
        clientNames(index - 1) = client.ReadString
        client.Send(MyBase.battleRules)
        client.Send(seed)

        If Convert.ToBoolean(battleRules And battleRules.Random) = False Then teams(index - 1) = CType(client.Deserialize, TeamInfo)
    End Sub
    Private Sub GetPlayerMessage(ByVal sender As NetworkClient, ByVal message As String) 
        For Each client As NetworkClient In clients
            If client IsNot Nothing AndAlso client IsNot sender Then
                client.Send(message)
            End If
        Next

        MyBase.GetMessage(sender, message)
    End Sub

    Protected Overrides Sub Send(ByVal data As String)
        For Each client As NetworkClient In clients
            If client IsNot Nothing AndAlso client.Connected Then client.Send(data)
        Next
    End Sub
    Private Sub PlayerExit(ByVal index As Integer)
        If battle IsNot Nothing Then GetMessage(clients(index - 1), "Exit" & index)

        Send("Exit" & index)
        Thread.Sleep(100)
        For Each client As NetworkClient In clients
            If client IsNot Nothing Then client.Dispose()
        Next
        clients = Nothing
        If watchers IsNot Nothing Then  
            watchers.ForEach(Of String)(AddressOf NetworkClient.SendToClient, "Exit" & index)
            Thread.Sleep(100)
            watchers.ForEach(AddressOf NetworkClient.CloseClient) 
            watchers.Clear()
        End If
    End Sub

    Protected Overrides Sub SendTie()

    End Sub
    Protected Overrides Sub Tied(ByVal message As String)

    End Sub
    Protected Overrides Sub GetTie(ByVal message As String)

    End Sub
    Protected Overrides Sub RefuseTie(ByVal message As String)

    End Sub
 
    Protected Overrides Sub OnEscape(ByVal e As System.Windows.Forms.FormClosingEventArgs)
        MyBase.OnEscape(e)
        If Not e.Cancel Then
            If clients IsNot Nothing Then
                For Each client As NetworkClient In clients
                    If client IsNot Nothing Then client.Dispose()
                Next
            End If
        End If
    End Sub
End Class
