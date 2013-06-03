Imports System.Runtime.Serialization.Formatters.Binary
Imports PokemonBattle.BattleNetwork
Imports NetworkLib
Public Class BattleClientForm

    Private _teams As Dictionary(Of Byte, TeamData) = New Dictionary(Of Byte, TeamData)()

    Protected _serverAddress As String
    Protected _client As PokemonBattleClient

    Public Sub New(ByVal address As String, ByVal name As String, ByVal myTeamValue As TeamData, _
        ByVal battleMode As BattleMode)

        Me.New(address, name, myTeamValue, battleMode, 2)
    End Sub
    Public Sub New(ByVal address As String, ByVal name As String, ByVal myTeamValue As TeamData, _
        ByVal battleMode As BattleMode, ByVal myPosition As Byte)

        MyBase.New(name, myPosition, myTeamValue, battleMode)
        ' 此调用是 Windows 窗体设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        _serverAddress = address
        _myTeamData = myTeamValue
        SetCaption("对战窗口")
    End Sub
    Private Sub BattleClientForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InitializeClient()
        ConenctServer()
    End Sub

    Private Sub InitializeClient()
        SetProgressText("正在初始化")
        _client = New PokemonBattleClient(_serverAddress)

        AddHandler _client.OnConnected, AddressOf OnConnected
        AddHandler _client.OnLogoned, AddressOf OnLogoned
        AddHandler _client.OnSetTeam, AddressOf OnGetTeam
        AddHandler _client.OnSetSeed, AddressOf OnGetSeed
        AddHandler _client.OnSetRules, AddressOf OnGetRules
        AddHandler _client.OnSetMove, AddressOf OnGetMove
        AddHandler _client.OnPlayerExit, AddressOf OnExit
        AddHandler _client.OnDisconnected, AddressOf OnDisconnected
        AddHandler _client.OnTie, AddressOf OnTie
        AddHandler _client.OnConnectFail, AddressOf OnConnectFail
        AddHandler _client.OnLogonFailed, AddressOf OnLogonFail
        SetProgressText("正在连接")
    End Sub

    Protected Overridable Sub ConenctServer()
        _client.Initialize()
        _client.RunThread()
    End Sub

    Protected Overridable Sub OnConnected()
        If _client IsNot Nothing Then
            _client.Logon(_identity, _battleMode, GetVersionInfo())
        End If
    End Sub

    Protected Overridable Sub OnLogoned()
        If _client IsNot Nothing Then
            SetProgressText("正在发送数据")
            SetProgress(30)
            Dim bytes As New ByteSequence()
            bytes.Elements.AddRange(_myTeamData.ToBytes())
            _client.SendTeam(_myPosition, _identity, bytes)
            SetProgressText("正在接收数据")
            SetProgress(60)
        End If
    End Sub

    Protected Overridable Sub OnConnectFail(ByVal ex As NetworkException)
        OnError("无法连接到主机")
    End Sub
    Protected Overridable Sub OnLogonFail(ByVal message As String)
        OnError(String.Format("无法登录到主机 : {0}", message))
    End Sub

    Protected Overridable Sub OnGetTeam(ByVal position As Byte, ByVal identity As String, ByVal team As ByteSequence)
        _teams(position) = TeamData.FromBytes(team.Elements.ToArray())
        SetPlayer(position, identity)
    End Sub
    Private Sub OnGetRules(ByVal rules As BattleRuleSequence)
        SetRules(rules.Elements)
    End Sub
    Private Sub OnGetSeed(ByVal seed As Integer)
        SetRandomSeed(seed)
        SetProgressText("正在初始化对战")
        SetProgress(80)

        If Not _teams.ContainsKey(_myPosition) Then _teams(_myPosition) = _myTeamData
        OnStartBattle()

        SetProgress(100)
        SetProgressText("初始化完毕")
        HideProgress()
    End Sub
    Protected Overridable Sub OnStartBattle()
        StartBattle(_teams)
    End Sub

    Protected Overridable Sub OnDisconnected()
        imgtxt.AddText("与主机断开了连接", Color.Red)
    End Sub

    Private Sub OnExit(ByVal identity As String)
        PlayerExit(identity)
    End Sub
    Private Sub OnTie(ByVal player As String, ByVal message As TieMessage)
        GetTie(player, message)
    End Sub
    Private Sub OnGetMove(ByVal move As PlayerMove)
        SetMove(move)
    End Sub

    Protected Overrides Sub PlayerMoved(ByVal move As PokemonBattle.BattleNetwork.PlayerMove)
        If _client IsNot Nothing Then
            _client.SendMove(move)
        End If
    End Sub
    Protected Overrides Sub SendTieMessage(ByVal message As PokemonBattle.BattleNetwork.TieMessage)
        If _client IsNot Nothing Then
            _client.Tie(_identity, message)
        End If
    End Sub
    Protected Overrides Sub OnBattleEnd()
        MyBase.OnBattleEnd()
        If _client IsNot Nothing Then
            _client.Stop()
            _client = Nothing
        End If
    End Sub

    Protected Overrides Sub OnEscape(ByVal e As System.Windows.Forms.FormClosingEventArgs)
        MyBase.OnEscape(e)
        If e.Cancel Then Return
        If _client IsNot Nothing Then
            _client.Stop()
            _client = Nothing
        End If
    End Sub

End Class