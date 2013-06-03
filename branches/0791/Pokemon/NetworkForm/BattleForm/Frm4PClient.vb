Friend Class Frm4PClient

    Private host As String
    Private port As Integer
    Private myName As String
    Private myIndex As Integer
    Private tempIndex As Integer

    Private myTeam As TeamInfo
    Private gameData As GameData

    Private client As NetworkClient
    Private escape As Boolean

    Private roomIndex As Integer
    Private main As FrmUserMain
    Public Sub New(ByVal address As String, ByVal name As String, ByVal team As TeamInfo, _
        ByVal data As GameData)
        Me.New(address, 10021, name, team, data)
        roomIndex = -1
        ThreadPool.QueueUserWorkItem(AddressOf Connect)
    End Sub
    Public Sub New(ByVal address As String, ByVal portValue As Integer, ByVal index As Integer, ByVal name As String, ByVal team As TeamInfo, _
        ByVal data As GameData, ByVal mainValue As FrmUserMain)
        Me.New(address, portValue, name, team, data)
        roomIndex = index
        main = mainValue
        ThreadPool.QueueUserWorkItem(AddressOf Connect)
    End Sub
    Private Sub New(ByVal address As String, ByVal portValue As Integer, ByVal name As String, ByVal team As TeamInfo, _
        ByVal data As GameData)

        ' 此调用是 Windows 窗体设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        Me.Icon = My.Resources.PokemonBall
        host = address
        myName = name
        myTeam = team
        gameData = data
        port = portValue
    End Sub
    Private Sub Connect(ByVal state As Object)
        Dim tcpConnection As TcpClient = New TcpClient()
        Try 
            tcpConnection.Connect(host, port)
            client = New NetworkClient(tcpConnection)
            If roomIndex <> -1 Then client.Send(roomIndex)
            client.Send(myName)
            SetButton(btn1P, True, "Player1")
            SetButton(btn2P, True, "Player2")
            SetButton(btn3P, True, "Player3")
            SetButton(btn4P, True, "Player4")
            client.Run(AddressOf GetMessage, AddressOf HandleExcetion)
        Catch ex As Exception
            If escape Then Return
            ShowInformation(Me, ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
            tcpConnection.Close()
            Invoke(New WorkDelegate(AddressOf Close))
        End Try
    End Sub
    Private Sub GetMessage(ByVal sender As NetworkClient, ByVal message As String)
        If message.StartsWith("AddPlayer") Then
            Dim msg As String() = SplitString(message.Substring(9), SeparateChar(0))
            For Each obj As Control In Me.Controls
                If obj.Name = "btn" & msg(1) & "P" Then 
                    SetButton(CType(obj, Button), False, msg(1) & "P " & msg(0))
                End If
            Next
        ElseIf message.StartsWith("RemovePlayer") Then
            Dim msg As String = message.Substring(12)
            For Each obj As Control In Me.Controls
                If obj.Name = "btn" & msg & "P" Then
                    SetButton(CType(obj, Button), True, "Player" & msg)
                End If
            Next
        ElseIf message = "Successed" Then
            myIndex = tempIndex
        ElseIf message = "Failed" Then
            ShowInformation(Me, "该位置已经有人", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
            tempIndex = 0
        ElseIf message = "BattleStarted" Then
            If myIndex <> 0 Then
                StartBattle()
            Else
                ShowInformation(Me, "人数已满", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            Invoke(New WorkDelegate(AddressOf Close))
        End If
    End Sub
    Private Sub HandleExcetion(ByVal sender As NetworkClient, ByVal ex As Exception)
        If Not escape Then
            ShowInformation(Me, "与主机断开了连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Invoke(New WorkDelegate(AddressOf Close))
        End If
    End Sub

    Private Sub StartBattle()
        If InvokeRequired Then
            Invoke(New WorkDelegate(AddressOf StartBattle))
        Else
            If roomIndex = -1 Then
                Dim form As Form
                form = New FrmBattleClient_4P(host, myName, myIndex, myTeam, gameData)
                form.MdiParent = Me.MdiParent
                form.Show()
            Else
                main.StartBattle(myIndex, BattleMode.Double_4P, BattleRules.None)
            End If
        End If
    End Sub

    Private Sub Frm4PClient_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        escape = True
        If client IsNot Nothing Then client.Dispose()
    End Sub

    Private Sub Button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn1P.Click, btn2P.Click, btn3P.Click, btn4P.Click
        tempIndex = Convert.ToInt32(CType(sender, Button).Name.Substring(3, 1))
        client.Send(tempIndex)
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
End Class