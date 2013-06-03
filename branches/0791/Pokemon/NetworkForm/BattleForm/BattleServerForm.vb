Imports System.Runtime.Serialization.Formatters.Binary
Imports PokemonBattle.BattleNetwork
Imports NetworkLib
Public Class BattleServerForm

    Private _battleRules As List(Of BattleRule)
    Private _seed As Integer = New Random().Next()

    Private _server As PokemonBattleServer

    Private _teams As Dictionary(Of Byte, PlayerInfo) = New Dictionary(Of Byte, PlayerInfo)()
    Private _agreeTieCounter As Integer
    Private _tieing As Boolean

    Private _locker As Object = New Object()
    Public Sub New(ByVal myTeamValue As TeamData, ByVal name As String, _
        ByVal battleMode As BattleMode, ByVal rules As List(Of BattleRule))

        MyBase.New(name, 1, myTeamValue, battleMode)
        ' 此调用是 Windows 窗体设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。

        _myTeamData = myTeamValue
        _battleRules = rules

        SetCaption("对战窗口")
    End Sub
    Private Sub BattleServerForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InitializeServer()
        RunServer()
    End Sub

    Private Sub InitializeServer()
        SetProgressText("正在初始化")

        _server = New PokemonBattleServer()
        AddHandler _server.OnBattleClientConnected, AddressOf VerifyClient
        AddHandler _server.OnSetTeam, AddressOf ReceiveClientTeam
        AddHandler _server.OnSetMove, AddressOf OnGetMove
        AddHandler _server.OnPlayerExit, AddressOf OnExit
        AddHandler _server.OnTie, AddressOf OnTie
        AddHandler _server.OnRequestBattleInfo, AddressOf GetBattleInfo

        SetRules(_battleRules)
        SetRandomSeed(_seed)

        Dim bytes As New ByteSequence()
        bytes.Elements.AddRange(_myTeamData.ToBytes())
        _teams(_myPosition) = New PlayerInfo(_identity, bytes)
    End Sub
    Protected Overridable Sub RunServer()
        If _server.Initialize() Then
            _server.RunThread()
            SetProgressText("正在等待连接")
        Else
            ShowInformation(Me, "服务器初始化失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub
    Protected Overridable Function VerifyClient(ByVal identity As String, ByVal mode As BattleMode, _
                                  ByVal versionInfo As String, ByRef message As String) As Boolean
        If mode <> _battleMode Then
            message = "对战模式不同"
            Return False
        End If
        If versionInfo <> GetVersionInfo() Then
            message = "版本不同"
            Return False
        End If
        SyncLock _locker
            If PlayerReady Then
                message = "人数已满"
                Return False
            End If
        End SyncLock
        Return True
    End Function
    Protected Overridable Sub ReceiveClientTeam(ByVal position As Byte, ByVal identity As String, ByVal team As ByteSequence)
        _teams(position) = New PlayerInfo(identity, team)

        SyncLock _locker
            SetPlayer(position, identity)
            If PlayerReady Then
                InitializeBattle()
            End If
        End SyncLock
    End Sub

    Protected Sub InitializeBattle()
        SetProgressText("正在发送数据")
        SetProgress(50)
        For Each key As Byte In _teams.Keys
            _server.SendTeam(key, _teams(key).Name, _teams(key).Team)
        Next

        Dim rules As New BattleRuleSequence
        rules.Elements.AddRange(_battleRules)
        _server.SendRules(rules)
        _server.SendRandomSeed(_seed)

        SetProgressText("正在初始化对战")
        SetProgress(80)

        OnStartBattle()

        SetProgress(100)
        SetProgressText("初始化完毕")
        HideProgress()
    End Sub
    Protected Overridable Sub OnStartBattle()
        Dim teams As New Dictionary(Of Byte, TeamData)
        For Each key As Byte In _teams.Keys
            teams(key) = TeamData.FromBytes(_teams(key).Team.Elements.ToArray())
        Next
        AddHandler OnBattleStarted, AddressOf BattleStarted
        StartBattle(teams)
    End Sub
    Private Sub BattleStarted()
        _server.SendBattleInfo(GetBattleInfo())
        AddHandler imgtxt.UpdateScreen, AddressOf ImageText_Update
    End Sub
    Private Sub ImageText_Update(ByVal sender As Object, ByVal e As System.EventArgs)
        _server.SendBattleSnapshot(GetSnapshot())
    End Sub

    Private Sub OnGetMove(ByVal move As PlayerMove)
        If (_server IsNot Nothing) Then
            _server.SendMove(move)
            SetMove(move)
        End If
    End Sub
    Private Sub OnTie(ByVal identity As Integer, ByVal player As String, ByVal message As TieMessage)
        If _server Is Nothing Then Return

        If message = TieMessage.TieRequest Then
            If _tieing Then
                _server.TieRequestFail(identity)
                Return
            Else
                _tieing = True
            End If
        End If

        If Not _tieing Then Return
        If message = TieMessage.AgreeTie Then
            _agreeTieCounter += 1
            If Not AllAgreeTie Then Return
        ElseIf message = TieMessage.RefuseTie Then
            _tieing = False
            _agreeTieCounter = 0
        End If

        If message = TieMessage.AgreeTie Then
            _server.Tie(player, message)
        Else
            _server.TieExcept(identity, player, message)
        End If
        GetTie(player, message)
    End Sub
    Private Sub OnExit(ByVal identity As String)
        _server.Exit(identity)
        PlayerExit(identity)
    End Sub

    Protected Overrides Sub PlayerMoved(ByVal moveValue As PlayerMove)
        OnGetMove(moveValue)
    End Sub
    Protected Overrides Sub SendTieMessage(ByVal message As PokemonBattle.BattleNetwork.TieMessage)
        If _server Is Nothing Then Return

        If message = TieMessage.TieRequest Then
            If _tieing Then
                GetTie("", TieMessage.Fail)
                Return
            Else
                _tieing = True
            End If
        End If

        If Not _tieing Then Return
        If message = TieMessage.AgreeTie Then
            _agreeTieCounter += 1
            If Not AllAgreeTie Then Return
        ElseIf message = TieMessage.RefuseTie Then
            _tieing = False
            _agreeTieCounter = 0
        End If

        _server.Tie(_identity, message)
        If message = TieMessage.AgreeTie Then GetTie(_identity, message)
    End Sub


    Private ReadOnly Property AllAgreeTie() As Boolean
        Get
            If _battleMode = BattleMode.Double_4P Then
                If _agreeTieCounter = 3 Then Return True
            ElseIf _agreeTieCounter = 1 Then
                Return True
            End If
            Return False
        End Get
    End Property

    Protected Overrides Sub OnEscape(ByVal e As System.Windows.Forms.FormClosingEventArgs)
        MyBase.OnEscape(e)
        If e.Cancel Then Return
        If _server IsNot Nothing Then
            _server.Stop()
            _server = Nothing
        End If
    End Sub

    Protected Overrides Sub OnBattleEnd()
        MyBase.OnBattleEnd()
        If _server IsNot Nothing Then
            _server.Stop()
            _server = Nothing
        End If
    End Sub

#Region "Embeded Class"
    Private Class PlayerInfo
        Private _name As String
        Private _team As ByteSequence

        Public Sub New(ByVal name As String, ByVal team As ByteSequence)
            _name = name
            _team = team
        End Sub

        Public ReadOnly Property Name() As String
            Get
                Return _name
            End Get
        End Property
        Public ReadOnly Property Team() As ByteSequence
            Get
                Return _team
            End Get
        End Property

    End Class
#End Region

End Class