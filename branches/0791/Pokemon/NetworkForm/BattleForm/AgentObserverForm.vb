Imports PokemonBattle.BattleNetwork
Public Class AgentObserverForm

    Private _identity As Integer
    Public Sub New(ByVal identity As Integer, ByVal address As String, ByVal position As Byte)
        MyBase.New(address, position)

        InitializeComponent()
        _identity = identity
    End Sub
    Protected Overrides Sub ConnectServer()
        AddHandler _client.OnPlayerExit, AddressOf PlayerExit
        _client.SetPort(ServerPort.BattleAgentServerPort)
        MyBase.ConnectServer()
    End Sub
    Protected Overrides Sub ConnectedServer()
        _client.RegistObserver(_identity)
    End Sub
    Private Sub PlayerExit(ByVal name As String)
        imgtxt.AddText(String.Format("{0}离开了游戏.", name))
    End Sub
End Class