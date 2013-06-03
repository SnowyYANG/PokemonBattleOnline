Friend Class Frm4PServer

    Private player As String
    Private myTeam As TeamInfo
    Private gameData As GameData
    Private WithEvents manager As FourPlayerServer
    Private battleRules As BattleRules

    Private listener As TcpListener
    Private runThread As Thread
    Private escape As Boolean
    Public Sub New(ByVal name As String, ByVal team As TeamInfo, ByVal data As GameData, ByVal rules As BattleRules)

        ' 此调用是 Windows 窗体设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        Me.Icon = My.Resources.PokemonBall
        player = name
        myTeam = team
        gameData = data
        battleRules = rules

        btn1P.Text = "1P " & player
        btn1P.Enabled = False
        btn2P.Enabled = False
        btn3P.Enabled = False
        btn4P.Enabled = False
        runThread = New Thread(AddressOf Run)
        runThread.Start()
    End Sub
    Private Sub Run()
        Try
            listener = New TcpListener(IPAddress.Any, 10021)
            listener.Start()
            manager = New FourPlayerServer
            Do
                Dim client As New NetworkClient(listener.AcceptTcpClient)
                If escape Then client.Dispose() : Return
                AddClient(client)
            Loop
        Catch ex As Exception
            If Not escape Then
                ShowInformation(Me, ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Invoke(New WorkDelegate(AddressOf Close))
            End If
        Finally
            If listener IsNot Nothing Then listener.Stop()
        End Try
    End Sub
    Private Sub AddClient(ByVal client As NetworkClient) 
        client.Send("AddPlayer" & player & SeparateChar(0) & 1)
        manager.GetClient(client)
    End Sub 
    Private Sub StartBattle()
        If InvokeRequired Then
            Invoke(New WorkDelegate(AddressOf StartBattle))
        Else
            Dim form As New FrmBattleServer_4P(myTeam, player, gameData, battleRules)
            form.MdiParent = Me.MdiParent
            form.Show()
            manager.StartBattle()
            Thread.Sleep(200)
            Close()
        End If
    End Sub

    Private Sub Frm4PServer_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        escape = True
        If listener IsNot Nothing Then listener.Stop()
        If manager IsNot Nothing Then manager.Dispose()
    End Sub
    Private Sub SetButton(ByVal button As Button, ByVal enable As Boolean, ByVal txt As String)
        If InvokeRequired Then
            Invoke(New SetText(AddressOf SetButton), button, enable, txt)
        Else
            button.Enabled = enable
            button.Text = txt
        End If
    End Sub
    Private Delegate Sub SetText(ByVal button As Button, ByVal enable As Boolean, ByVal txt As String)

    Private Sub manager_AddedPlayer(ByVal name As String, ByVal index As Integer) Handles manager.AddedPlayer
        For Each obj As Control In Me.Controls
            If obj.Name = "btn" & index & "P" Then
                SetButton(CType(obj, Button), False, index & "P " & name)
            End If
        Next
    End Sub

    Private Sub manager_CheckCount(ByVal sender As Object, ByVal e As System.EventArgs) Handles manager.CheckCount
        If manager.ClientCount = 3 Then StartBattle()
    End Sub

    Private Sub manager_RemovedPlayer(ByVal index As Integer) Handles manager.RemovedPlayer
        For Each obj As Control In Me.Controls
            If obj.Name = "btn" & index & "P" Then
                SetButton(CType(obj, Button), False, "Player" & index)
            End If
        Next
    End Sub
End Class