Imports PokemonBattle.BattleNetwork
Public Class FourPlayerServerForm
    Private Delegate Sub SetText(ByVal control As Control, ByVal txt As String)

    Private _myName As String
    Private _rules As List(Of BattleRule)

    Private _server As FourPlayerServer
    Public Sub New(ByVal name As String, ByVal rules As List(Of BattleRule))

        ' 此调用是 Windows 窗体设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        Me.Icon = My.Resources.PokemonBall
        _myName = name
        _rules = rules
        RunServer()
    End Sub

    Private Sub RunServer()
        Player1Button.Text = _myName

        _server = New FourPlayerServer(_myName)
        AddHandler _server.OnPositionSet, AddressOf SetPosition
        If _server.Initialize() Then
            _server.RunThread()
        Else
            ShowInformation(Me, "服务器初始化失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub
    Private Sub SetPosition(ByVal position As Byte, ByVal player As String)
        SetButton(Controls.Find(String.Format("Player{0}Button", position), False)(0), player)
    End Sub
    Private Sub StartBattle()
        Dim form As Form = CType(MdiParent, MainForm).BuildBattleServerForm(_myName, BattleMode.Double_4P, _rules)
        form.Show()
    End Sub

    Private Sub SetButton(ByVal control As Control, ByVal txt As String)
        If InvokeRequired Then
            Invoke(New SetText(AddressOf SetButton), control, txt)
        Else
            control.Text = txt
            StartButton.Enabled = FullPlayer
        End If
    End Sub

    Private Sub Frm4PServer_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If _server IsNot Nothing Then
            _server.Stop()
            _server = Nothing
        End If
    End Sub

    Private Sub StartButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StartButton.Click
        StartBattle()
        _server.StartBattle()
        Thread.Sleep(500)
        Close()
    End Sub

    Private ReadOnly Property FullPlayer() As Boolean
        Get
            If Player2Button.Text.Length = 0 Then Return False
            If Player3Button.Text.Length = 0 Then Return False
            If Player4Button.Text.Length = 0 Then Return False
            Return True
        End Get
    End Property
End Class