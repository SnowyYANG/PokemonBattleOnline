Imports System.Runtime.Serialization.Formatters.Binary
Imports PokemonBattle.BattleNetwork
Imports PokemonBattle.BattleNetwork.Client

Public Class BattleObserverForm
    Private Delegate Sub UpdateTextDelegate(ByVal txt As String)

    Protected WithEvents imgtxt As New ImgTxt(Convert.ToInt32(My.Settings.LogLineCount), False)

    Private _serverAddress As String
    Private _observePosition As Byte

    Private _terrainImage As Image
    Private _image As New Bitmap(256, 144)

    Private _pokemonRects As List(Of Rectangle)
    Private _pokemonStateRects As List(Of Rectangle)
    Private _positions As List(Of Integer)
    Private _indices As List(Of Integer)

    Protected _client As PokemonBattleClient
    Private _snapshot As BattleSnapshot
    Private _mode As BattleMode
    Public Sub New(ByVal address As String, ByVal observePosition As Byte)

        ' 此调用是 Windows 窗体设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        Me.Icon = My.Resources.PokemonBall
        _serverAddress = address
        _observePosition = observePosition

        picText.Image = imgtxt.Image
        picScreen.Image = _image
    End Sub

    Private Sub InitializeClient()
        _client = New PokemonBattleClient(_serverAddress)
        AddHandler _client.OnConnected, AddressOf ConnectedServer
        AddHandler _client.OnSetBattleInfo, AddressOf ReceiveBattleInfo
        AddHandler _client.OnSetSnapshot, AddressOf ReceiveSnapshot
        AddHandler _client.OnDisconnected, AddressOf OnDisconnected
    End Sub
    Protected Overridable Sub ConnectServer()
        _client.Initialize()
        _client.RunThread()
    End Sub

    Protected Overridable Sub ConnectedServer()
        _client.RegistObserver(0)
    End Sub
    Private Sub ReceiveBattleInfo(ByVal info As BattleInfo)
        CheckCustomData(info.CustomDataHash)
        _terrainImage = GetTerrainImage(info.Terrain)
        _mode = info.Mode
        SetCaption(info.Caption)
    End Sub
    Private Sub CheckCustomData(ByVal hash As String)
        If BattleData.CustomData.DataHash <> hash Then imgtxt.AddText("当前使用的自定义数据与对战不同,观战时可能会出现错误")
    End Sub
    Private Sub ReceiveSnapshot(ByVal snapshot As BattleSnapshot)
        _snapshot = snapshot
        Dim textColor As Color = Color.FromArgb(snapshot.TextColor)
        imgtxt.AddText(snapshot.NewText, textColor)
    End Sub
    Protected Overridable Sub OnDisconnected()
        imgtxt.AddText("与主机断开了连接", Color.Red)
    End Sub

    Private Sub SetCaption(ByVal txt As String)
        If Not InvokeRequired Then
            Text = txt
        Else
            Invoke(New UpdateTextDelegate(AddressOf SetCaption), txt)
        End If
    End Sub

    Private Sub UpdateScreen()
        Dim pokemons As List(Of PokemonSnapshot) = _snapshot.Pokemons.Elements

        If _positions Is Nothing Then
            _positions = New List(Of Integer)
            _indices = New List(Of Integer)
            If _mode = BattleMode.Double_4P OrElse _mode = BattleMode.Double Then
                If _observePosition > 2 Then
                    _positions.AddRange(New Integer() {2, 3, 0, 1})
                    _indices.AddRange(New Integer() {2, 2, 1, 1})
                Else
                    _positions.AddRange(New Integer() {0, 1, 2, 3})
                    _indices.AddRange(New Integer() {1, 1, 2, 2})
                End If
            ElseIf _observePosition = 2 Then
                _positions.AddRange(New Integer() {1, 0})
                _indices.AddRange(New Integer() {2, 1})
            Else
                _positions.AddRange(New Integer() {0, 1})
                _indices.AddRange(New Integer() {1, 2})
            End If
        End If

        If _pokemonRects Is Nothing Then
            _pokemonRects = New List(Of Rectangle)
            _pokemonStateRects = New List(Of Rectangle)
            If pokemons.Count = 2 Then
                _pokemonRects.Add(New Rectangle(20, 64, 80, 80))
                _pokemonStateRects.Add(New Rectangle(136, 95, 120, 38))

                _pokemonRects.Add(New Rectangle(155, 13, 80, 80))
                _pokemonStateRects.Add(New Rectangle(-6, 27, 126, 30))
            Else
                _pokemonRects.Add(New Rectangle(0, 64, 80, 80))
                _pokemonRects.Add(New Rectangle(50, 64, 80, 80))
                _pokemonStateRects.Add(New Rectangle(130, 87, 126, 30))
                _pokemonStateRects.Add(New Rectangle(136, 116, 126, 30))

                _pokemonRects.Add(New Rectangle(126, 3, 80, 80))
                _pokemonRects.Add(New Rectangle(175, 7, 80, 80))
                _pokemonStateRects.Add(New Rectangle(0, 1, 126, 30))
                _pokemonStateRects.Add(New Rectangle(-6, 29, 126, 30))
            End If
        End If
        Using graph As Graphics = Graphics.FromImage(_image)
            graph.DrawImageUnscaled(_terrainImage, 0, 0)
            For i As Integer = 0 To pokemons.Count \ 2 - 1
                Dim position As Integer = _positions(i)

                Dim stateImage As Image = GetPMStateImg(_indices(i), pokemons(position))
                If stateImage IsNot Nothing Then graph.DrawImageUnscaled(stateImage, _pokemonStateRects(i))
                If pokemons(position).Hp > 0 AndAlso Not pokemons(position).Hid Then
                    Dim pmImage As Image
                    If pokemons(position).Substituded Then
                        pmImage = My.Resources.SubBack
                    Else
                        Dim pm As PokemonData = BattleData.GetPokemon(pokemons(position).Identity)
                        If pokemons(position).Gender = PokemonGender.Female AndAlso pm.BackImageF <> -1 Then
                            pmImage = _
                                BattleData.GetImage(pokemons(position).Identity, pm.BackImageF)
                        Else
                            pmImage = _
                                BattleData.GetImage(pokemons(position).Identity, pm.BackImage)
                        End If
                    End If
                    graph.DrawImageUnscaledAndClipped(pmImage, _pokemonRects(i))
                End If
            Next
            For i As Integer = pokemons.Count \ 2 To pokemons.Count - 1
                Dim position As Integer = _positions(i)
                Dim stateImage As Image = GetPMStateImg(_indices(i), pokemons(position))
                If stateImage IsNot Nothing Then graph.DrawImageUnscaled(stateImage, _pokemonStateRects(i))

                If pokemons(position).Hp > 0 AndAlso Not pokemons(position).Hid Then
                    Dim pmImage As Image
                    If (pokemons(position).Substituded) Then
                        pmImage = My.Resources.Substitute
                    Else
                        Dim pm As PokemonData = BattleData.GetPokemon(pokemons(position).Identity)
                        If pokemons(position).Gender = PokemonGender.Female AndAlso pm.FrontImageF <> -1 Then
                            pmImage = _
                                BattleData.GetImage(pokemons(position).Identity, pm.FrontImageF)
                        Else
                            pmImage = _
                                BattleData.GetImage(pokemons(position).Identity, pm.FrontImage)
                        End If
                    End If
                    graph.DrawImageUnscaledAndClipped(pmImage, _pokemonRects(i))
                End If
            Next
        End Using
        SetImage()
    End Sub
    Private Sub SetImage()
        If InvokeRequired Then
            Invoke(New WorkDelegate(AddressOf SetImage))
        Else
            picScreen.Refresh()
            picText.Refresh()
        End If
    End Sub

    Private Sub picScreen_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles picScreen.MouseClick
        If Not e.Button = Windows.Forms.MouseButtons.Left Then Return
        imgtxt.OnClick(e.Location)
    End Sub

    Private Sub FrmBattleWatcher_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.PageUp Then
            imgtxt.RollUp()
        ElseIf e.KeyCode = Keys.PageDown Then
            imgtxt.RollDown()
        End If
    End Sub

    Private Sub imgtxt_Roll(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgtxt.Roll
        SetImage()
    End Sub

    Private Sub imgtxt_UpdateScreen(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgtxt.UpdateScreen
        If _terrainImage IsNot Nothing Then UpdateScreen()
    End Sub

    Private Sub BattleObserverForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim logLine As Integer = Convert.ToInt32(My.Settings.LogLineCount)
        If logLine > 6 Then
            picText.Size = New Size(256, 7 + logLine * 13)
            Me.Size = New Size(291, 206 + logLine * 13)
        End If
        InitializeClient()
        ConnectServer()
    End Sub

    Private Sub BattleObserverForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If _client IsNot Nothing Then
            _client.Stop()
            _client = Nothing
        End If
    End Sub
End Class