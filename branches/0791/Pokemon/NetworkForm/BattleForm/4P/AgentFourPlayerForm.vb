Imports PokemonBattle.BattleNetwork
Imports PokemonBattle.RoomClient
Public Class AgentFourPlayerForm

    Private _identity As Integer
    Private _asHost As Boolean
    Private _callback As FourPlayerFormCallback
    Public Sub New(ByVal identity As Integer, ByVal address As String, ByVal name As String, _
                   ByVal asHost As Boolean, ByVal callback As FourPlayerFormCallback)
        MyBase.New(address, name)
        InitializeComponent()

        _identity = identity
        _asHost = asHost
        _callback = callback
        If asHost Then
            Player1Button.Enabled = False
            Player2Button.Enabled = False
            Player3Button.Enabled = False
            Player4Button.Enabled = False
            _myPosition = 1
            Me.ClientSize = New System.Drawing.Size(274, 183)
        Else
            StartButton.Hide()
        End If
    End Sub
    Protected Overrides Sub ConnectServer()
        AddHandler _client.OnServerClosed, AddressOf ServerClosed
        _client.SetPort(ServerPort.FourPlayerAgentServerPort)
        MyBase.ConnectServer()
    End Sub
    Protected Overrides Sub Connected()
        _client.Logon(_identity)
    End Sub
    Protected Overrides Sub SetPosition(ByVal position As Byte, ByVal player As String)
        Dim enable As Boolean = (player = "")
        If _asHost Then enable = False
        SetButton(Controls.Find(String.Format("Player{0}Button", position), False)(0), enable, player)
    End Sub
    Protected Overrides Sub OnSetPosition()
        If _asHost Then StartButton.Enabled = FullPlayer
    End Sub
    Protected Overrides Sub StartBattle(ByVal identity As Integer)
        If _myPosition <> 0 Then
            _callback(identity, _myPosition)
            ServerClosed()
        End If
    End Sub

    Private Sub ServerClosed()
        Invoke(New NetworkLib.VoidFunctionDelegate(AddressOf Close))
    End Sub

    Private Sub StartButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StartButton.Click
        _client.StartBattle()
    End Sub

    Private ReadOnly Property FullPlayer() As Boolean
        Get
            If Player2Button.Text.Length = 0 Then Return False
            If Player3Button.Text.Length = 0 Then Return False
            If Player4Button.Text.Length = 0 Then Return False
            Return True
        End Get
    End Property

    Protected Overrides Sub OnEscape()
        If _asHost Then _client.Close()
    End Sub
End Class