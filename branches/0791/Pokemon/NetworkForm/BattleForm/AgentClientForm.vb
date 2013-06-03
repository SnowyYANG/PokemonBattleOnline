Imports System.Runtime.Serialization.Formatters.Binary
Imports PokemonBattle.BattleNetwork
Imports PokemonBattle.RoomClient

Public Class AgentClientForm
    Inherits BattleClientForm

    Private _agentIdentity As Integer
    Private _teams As Dictionary(Of Integer, TeamData) = New Dictionary(Of Integer, TeamData)()

    Private _moveInterval As Integer
    Private _timeCounter As Integer
    Private WithEvents _moveTimer As Windows.Forms.Timer

    Public Sub New(ByVal battleInfo As AgentBattleInfo, ByVal team As TeamData)
        MyBase.New(battleInfo.ServerAddress, battleInfo.UserName, team, battleInfo.BattleMode, battleInfo.Position)
        ' 此调用是 Windows 窗体设计器所必需的。
        InitializeComponent()

        _moveInterval = battleInfo.MoveInterval
        _agentIdentity = battleInfo.AgentID
        SetCaption("对战窗口")
    End Sub

    Protected Overrides Sub ConenctServer()
        AddHandler _client.OnRegistObserver, AddressOf RegistObserver
        AddHandler _client.OnPlayerTimeUp, AddressOf OnPlayerTimerUp
        _client.SetPort(ServerPort.BattleAgentServerPort)
        MyBase.ConenctServer()
    End Sub
    Protected Overrides Sub OnConnected()
        If (_client IsNot Nothing) Then
            _client.Logon(_agentIdentity.ToString(), _battleMode, GetVersionInfo())
        End If
    End Sub

    Private Sub RegistObserver()
        If _client Is Nothing Then Return
        Dim info As BattleInfo = GetBattleInfo()
        If info IsNot Nothing Then _client.SendBattleInfo(info)
        AddHandler imgtxt.UpdateScreen, AddressOf ImageText_Update
        AddHandler OnBattleStarted, AddressOf BattleStarted
    End Sub

    Private Sub BattleStarted()
        If _client IsNot Nothing Then _client.SendBattleInfo(GetBattleInfo())
    End Sub

    Private Sub ImageText_Update(ByVal sender As Object, ByVal e As System.EventArgs)
        If _client IsNot Nothing Then _client.SendBattleSnapshot(GetSnapshot())
    End Sub

    Private Sub OnPlayerTimerUp(ByVal identity As String)
        imgtxt.AddText(String.Format("{0}行动超时.", identity), Color.Red)
    End Sub

    Protected Overrides Sub PlayerMoved(ByVal move As PokemonBattle.BattleNetwork.PlayerMove)
        If (_moveInterval <> 0) Then
            _timeCounter = 0
            Invoke(New NetworkLib.VoidFunctionDelegate(AddressOf _moveTimer.Stop))
        End If
        MyBase.PlayerMoved(move)
    End Sub
    Protected Overrides Sub OnWaitMove()
        If (_moveInterval <> 0) Then Invoke(New NetworkLib.VoidFunctionDelegate(AddressOf _moveTimer.Start))
        MyBase.OnWaitMove()
    End Sub
    Private Sub Timer_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles _moveTimer.Tick
        _timeCounter += 1
        If _timeCounter = 1 Then
            imgtxt.AddText(String.Format("目前还有{0}秒的等待行动时间", Math.Truncate(_moveInterval * 0.75)), Color.Red)
        ElseIf _timeCounter = 2 Then
            imgtxt.AddText(String.Format("目前还有{0}秒的等待行动时间", Math.Truncate(_moveInterval * 0.5)), Color.Red)
        ElseIf _timeCounter = 3 Then
            imgtxt.AddText(String.Format("目前还有{0}秒的等待行动时间,请尽快行动", Math.Truncate(_moveInterval * 0.25)), Color.Red)
        ElseIf _timeCounter = 4 Then
            If _client IsNot Nothing Then
                _client.TimeUp(_identity)
                imgtxt.AddText("行动超时,已通知服务器", Color.Red)
            End If
        End If
    End Sub

    Private Sub AgentClientForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (_moveInterval <> 0) Then
            _moveTimer = New Windows.Forms.Timer()
            _moveTimer.Interval = _moveInterval * 250
            _moveTimer.Stop()
        End If
    End Sub
End Class