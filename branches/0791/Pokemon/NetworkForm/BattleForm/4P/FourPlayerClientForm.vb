Imports PokemonBattle.BattleNetwork
Imports NetworkLib
Public Class FourPlayerClientForm
    Private Delegate Sub SetText(ByVal control As Control, ByVal enable As Boolean, ByVal txt As String)

    Private _serverAddress As String
    Private _myName As String
    Protected _myPosition As Byte

    Protected _client As FourPlayerClient
    Public Sub New(ByVal address As String, ByVal name As String)

        ' 此调用是 Windows 窗体设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        Me.Icon = My.Resources.PokemonBall
        _serverAddress = address
        _myName = name
    End Sub
    Private Sub InitializeClient()
        _client = New FourPlayerClient(_serverAddress)
        AddHandler _client.OnConnected, AddressOf Connected
        AddHandler _client.OnPositionSet, AddressOf SetPosition
        AddHandler _client.OnBattleReady, AddressOf StartBattle
        AddHandler _client.OnDisconnected, AddressOf Disconnected
        AddHandler _client.OnConnectFail, AddressOf ConnectFail
        AddHandler _client.OnMyPositionSet, AddressOf SetMyPosition
    End Sub
    Protected Overridable Sub ConnectServer()
        _client.Initialize()
        _client.RunThread()
    End Sub

    Protected Overridable Sub Connected()
        _client.Logon(0)
    End Sub
    Private Sub ConnectFail(ByVal exception As NetworkException)
        ShowInformation(Me, "无法连接到主机", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Invoke(New VoidFunctionDelegate(AddressOf Close))
    End Sub
    Private Sub Disconnected()
        ShowInformation(Me, "与主机断开了连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Invoke(New VoidFunctionDelegate(AddressOf Close))
    End Sub
    Protected Overridable Sub SetPosition(ByVal position As Byte, ByVal player As String)
        SetButton(Controls.Find(String.Format("Player{0}Button", position), False)(0), player = "", player)
    End Sub
    Private Sub SetMyPosition(ByVal position As Byte)
        _myPosition = position
    End Sub

    Protected Overridable Sub StartBattle(ByVal identity As Integer)
        If (Not InvokeRequired) Then
            RemoveHandler _client.OnDisconnected, AddressOf Disconnected
            If _myPosition <> 0 Then
                Dim form As Form = CType(MdiParent, MainForm).BuildBattleClientForm(_serverAddress, _myPosition, _myName, BattleMode.Double_4P)
                form.Show()
            End If
            Close()
        Else
            Invoke(New IdentityDelegate(AddressOf StartBattle), identity)
        End If
    End Sub

    Protected Sub SetButton(ByVal control As Control, ByVal enable As Boolean, ByVal txt As String)
        If InvokeRequired Then
            Invoke(New SetText(AddressOf SetButton), control, enable, txt)
        Else
            control.Enabled = enable
            control.Text = txt
            OnSetPosition()
        End If
    End Sub
    Protected Overridable Sub OnSetPosition()

    End Sub

    Private Sub Player2Button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Player2Button.Click
        _client.SetPosition(2, _myName)
    End Sub

    Private Sub Player3Button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Player3Button.Click
        _client.SetPosition(3, _myName)
    End Sub

    Private Sub Player4Button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Player4Button.Click
        _client.SetPosition(4, _myName)
    End Sub

    Private Sub FourPClientForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        OnEscape()
        If _client IsNot Nothing Then
            _client.Stop()
            _client = Nothing
        End If
    End Sub
    Protected Overridable Sub OnEscape()

    End Sub

    Private Sub FourPClientForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InitializeClient()
        ConnectServer()
    End Sub
End Class