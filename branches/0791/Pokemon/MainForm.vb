Imports System.Runtime.Serialization.Formatters.Binary
Imports PokemonBattle.BattleNetwork
Imports PokemonBattle.BattleRoom.Client
Imports PokemonBattle.RoomClient
Imports PokemonBattle.PokemonData.Custom

Friend Class MainForm

    Private _index As IndexForm
    Private WithEvents _teamEditor As TeamEditorForm
    Private _dataDex As FrmDataDex
    Private _settingForm As SettingForm
    Private _data As GameData
    Private _team As TeamData
    Private _roomForm As RoomClientForm

    Public Event TeamFormHided As EventHandler
    Public Event TeamFormShowed As EventHandler

    Public Sub New()

        ' 此调用是 Windows 窗体设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        Me.Text = "Pokemon Battle Online"
        Me.MinimumSize = Me.Size

        LoadData()
        If Not File.Exists(Application.StartupPath & "\images.pgd") Then
            MessageBox.Show("数据读取错误，请确保程序目录下的images.pgd文件没有被删除或改动", _
               "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Close()
        Else
            ImageManager.LoadData(Path.Combine(Application.StartupPath, "images.pgd"))
        End If
    End Sub

    Private Sub LoadData()
        _data = New GameData
        Using stream As New FileStream(Path.Combine(Application.StartupPath, "data.pgd"), FileMode.Open)
            _data.LoadData(stream)
        End Using
        BattleData.DataProvider = _data

        Dim i As Integer
        Dim j As Integer
        For Each pm As PokemonData In _data.GetAllPokemons()
            For Each move As MoveLearnData In pm.Moves
                If move.Info = "GC" Then
                    i += 1
                ElseIf move.Info = "DSOnly" Then
                    j += 1
                End If
            Next
        Next
        'UpdateData()
        'Using stream As New FileStream(Path.Combine(Application.StartupPath, "data.pgd"), FileMode.Open)
        '_data.Save(Stream)
        'End Using
    End Sub


    Private Sub UpdateData()

        Using writer As New StreamWriter("D:\PM\attack\pmName.txt")
            For i As Integer = 1 To 493
                Dim pm As PokemonData = _data.GetPokemonDataByNumber(i)(0)
                writer.WriteLine(pm.Name)
            Next
        End Using
    End Sub

    Private Sub FrmPBO_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _index = New IndexForm()
        _index.MdiParent = Me
        _index.Show()

        If My.Settings.history Is Nothing Then
            LoadOldSettings()
        End If

        If File.Exists(My.Settings.teamPath) Then
            _team = TeamEditorForm.OpenTeam(My.Settings.teamPath)
            If _team IsNot Nothing AndAlso _team.CustomInfo.DataName <> "" Then
                If Not LoadCustom(_team.CustomInfo.DataName & ".pcd") Then
                    _team = Nothing
                    My.Settings.teamPath = ""
                End If
            End If
            _index.UpdateTeamPath()
        Else
            My.Settings.teamPath = ""
        End If

        Dim counterPath As String = Path.Combine(Application.StartupPath, "PokemonCounter.pgd")
        If File.Exists(counterPath) Then
            PokemonCounter.LoadDict(counterPath)
        Else
            PokemonCounter.CreateDict(counterPath, _data.GetAllPokemons.Length)
        End If
        cboCustomData.SelectedIndex = 0
    End Sub

    Private Sub FrmPBO_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        ImageManager.Close()
        My.Settings.Save()
        PokemonCounter.SaveDict(Path.Combine(Application.StartupPath, "PokemonCounter.pgd"))
        Environment.Exit(Environment.ExitCode)
    End Sub

#Region "History"

    Private Sub mnuHistory_DropDownOpening(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuHistory.DropDownOpening
        UpdateHistory()
    End Sub
    Private Sub OpenHistory(ByVal sender As System.Object, ByVal e As System.EventArgs)
        TeamEditor.OpenTeam(TeamEditorForm.OpenTeam(CType(sender, ToolStripMenuItem).Text), False)
        ShowTeamEditor()
    End Sub
    Private Sub UpdateHistory()
        For Each menu As ToolStripMenuItem In mnuHistory.DropDownItems
            If menu.Text <> "无" Then
                RemoveHandler menu.Click, AddressOf OpenHistory
            End If
        Next
        mnuHistory.DropDownItems.Clear()
        If My.Settings.history.Count = 0 Then
            Return
        Else
            Dim index As Integer
            Do
                Dim path As String = My.Settings.history(index)

                If File.Exists(path) Then
                    Dim newMenu As New ToolStripMenuItem(path)
                    AddHandler newMenu.Click, AddressOf OpenHistory
                    mnuHistory.DropDownItems.Add(newMenu)
                    index += 1
                Else
                    My.Settings.history.RemoveAt(index)
                End If
            Loop Until index > My.Settings.history.Count - 1
        End If
    End Sub

#End Region

    Private Sub 精灵资料PToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 精灵资料PToolStripMenuItem.Click
        DataDexForm.tabDexs.SelectedIndex = 0
        DataDexForm.Show()
    End Sub

    Private Sub 技能ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 技能ToolStripMenuItem.Click
        DataDexForm.tabDexs.SelectedIndex = 1
        DataDexForm.Show()
    End Sub

    Private Sub 关于AToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles 关于AToolStripMenuItem.Click
        AboutForm.Show()
    End Sub

    Private Sub 观看录象RToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 观看录象RToolStripMenuItem.Click
        Dim replay As New BattleReplayForm(_data)
        replay.MdiParent = Me
        replay.Show()
    End Sub

    Private Sub mnuExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExit.Click
        Close()
    End Sub

    Public ReadOnly Property TeamEditor() As TeamEditorForm
        Get
            If _teamEditor Is Nothing Then
                _teamEditor = New TeamEditorForm(_team, _data)
                _teamEditor.MdiParent = Me
            End If
            Return _teamEditor
        End Get
    End Property
    Public ReadOnly Property TeamEditorVisible() As Boolean
        Get
            If _teamEditor Is Nothing OrElse _teamEditor.Visible = False Then Return False
            Return True
        End Get
    End Property
    Public ReadOnly Property SettingForm() As SettingForm
        Get
            Return New SettingForm()
        End Get
    End Property
    Public ReadOnly Property DataDexForm() As FrmDataDex
        Get
            If _dataDex Is Nothing Then
                _dataDex = New FrmDataDex()
                _dataDex.MdiParent = Me
            End If
            Return _dataDex
        End Get
    End Property

    Public Property Team() As TeamData
        Get
            Return _team
        End Get
        Set(ByVal value As TeamData)
            _team = value
        End Set
    End Property

#Region "Team Editor"
    Private Sub EditTeamMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles EditTeamMenuItem.Click
        ShowTeamEditor()
    End Sub

    Private Sub mnuNewTeam_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewTeam.Click
        NewTeam()
    End Sub

    Private Sub mnuOpenTeam_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuOpenTeam.Click
        ShowTeamEditor()
        TeamEditor.cmdOpen.PerformClick()
    End Sub

    Private Sub NewTeam()
        TeamEditor.NewTeam()
        ShowTeamEditor()
    End Sub

    Public Sub ShowTeamEditor()
        TeamEditor.Show()
        If _index.Visible Then
            _index.Hide()
            AddHandler Me.TeamFormHided, AddressOf _index.TeamFormHided
        End If
    End Sub

    Private Sub 保存SToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 保存SToolStripMenuItem.Click
        TeamEditor.cmdSave.PerformClick()
    End Sub

    Private Sub 另存为AToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 另存为AToolStripMenuItem.Click
        TeamEditor.cmdSaveTo.PerformClick()
    End Sub

    Private Sub 选项OToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 选项OToolStripMenuItem.Click
        SettingForm.ShowDialog()
    End Sub

    Private Sub mTeamEditor_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _teamEditor.VisibleChanged
        If Not _teamEditor.Visible Then
            If _roomForm IsNot Nothing Then _roomForm.EndEditTeam()
            RaiseEvent TeamFormHided(Me, EventArgs.Empty)
        Else
            If _roomForm IsNot Nothing Then _roomForm.BeginEditTeam()
            RaiseEvent TeamFormShowed(Me, EventArgs.Empty)
        End If
    End Sub
#End Region

    Private Sub LoadOldSettings()
        My.Settings.Upgrade()
        If My.Settings.history Is Nothing Then My.Settings.history = New Collections.Specialized.StringCollection
    End Sub

#Region "Custom"
    Public Function LoadCustom(ByVal fileName As String) As Boolean
        If fileName = ".pcd" Then
            ImageManager.CloseCustom()
            LoadData()
            UpdateCustomData()
            Return True
        Else
            Dim cdPath As String = Path.Combine(Application.StartupPath, "CustomData")
            fileName = Path.Combine(cdPath, fileName)
            If Not File.Exists(fileName) Then
                MessageBox.Show("找不到指定的自定义数据", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

            ImageManager.CloseCustom()
            If BattleData.CustomData.DataName <> "" Then LoadData()
            Dim hash As String
            Using stream As New FileStream(fileName, FileMode.Open)
                hash = ComputeHash(stream)
            End Using
            Try
                Dim customData As CustomGameData = CustomGameData.FromFile(fileName)
                _data.LoadCustomData(customData, hash)
                ImageManager.LoadCustomData(fileName, customData.ImageOffset)
                UpdateCustomData()
                Return True
            Catch ex As Exception
                MessageBox.Show("导入自定义数据错误", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
            Return False
        End If
    End Function
    Private Sub UpdateCustomData()
        If _teamEditor IsNot Nothing Then _teamEditor.UpdateData(_data)
        If _dataDex IsNot Nothing Then _dataDex.UpdateData()
        If _roomForm IsNot Nothing Then _roomForm.UpdateCustomData(_data.CustomData.DataName, _data.CustomData.DataHash)
    End Sub

    Public Sub UpdateCustom()
        Dim cdPath As String = Path.Combine(Application.StartupPath, "CustomData")
        If Not Directory.Exists(cdPath) Then Directory.CreateDirectory(cdPath)

        Dim array As Object() = New Object(cboCustomData.Items.Count - 1) {}
        cboCustomData.Items.CopyTo(array, 0)
        For Each item As String In array
            If item <> "不使用" AndAlso Not File.Exists(Path.Combine(cdPath, item)) Then cboCustomData.Items.Remove(item)
        Next

        Dim customInfo As DirectoryInfo = New DirectoryInfo(cdPath)
        For Each file As FileInfo In customInfo.GetFiles("*.pcd")
            If Not cboCustomData.Items.Contains(file.Name) Then
                cboCustomData.Items.Add(file.Name)
            End If
        Next

        If cboCustomData.SelectedItem.ToString <> BattleData.CustomData.DataName & ".pcd" Then
            If BattleData.CustomData.DataName = "" Then
                cboCustomData.SelectedIndex = 0
            Else
                cboCustomData.SelectedItem = BattleData.CustomData.DataName & ".pcd"
            End If
        End If
    End Sub

    Private Sub EditCustomMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EditCustomMenuItem.Click
        Dim form As New FrmCustomData(_data)
        form.MdiParent = Me
        form.Show()
    End Sub

    Private Sub cboCustomData_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboCustomData.SelectedIndexChanged
        If cboCustomData.SelectedIndex = -1 Then Return
        If cboCustomData.SelectedIndex = 0 Then
            If cboCustomData.Items.Count = 1 Then Return
            If BattleData.CustomData.DataName = "" Then Return
            LoadCustom(".pcd")
            If _team IsNot Nothing Then ShowTeamEditor()
        Else
            If BattleData.CustomData.DataName & ".pcd" = cboCustomData.SelectedItem.ToString Then Return
            If Not LoadCustom(cboCustomData.SelectedItem.ToString) Then
                If BattleData.CustomData.DataName = "" Then
                    cboCustomData.SelectedIndex = 0
                Else
                    cboCustomData.SelectedItem = BattleData.CustomData.DataName & ".pcd"
                End If
            Else
                If _team IsNot Nothing Then ShowTeamEditor()
            End If
        End If
    End Sub

    Private Sub mnuLoadCData_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuLoadCData.DropDownOpening
        If Battling Then
            cboCustomData.Enabled = False
        Else
            cboCustomData.Enabled = True
            UpdateCustom()
        End If
    End Sub
#End Region

    Public ReadOnly Property Battling() As Boolean
        Get
            For Each form As Form In Me.MdiChildren
                If form.GetType.IsSubclassOf(GetType(BattleForm)) Then
                    Return True
                End If
            Next
            Return False
        End Get
    End Property

#Region "BuildBattleForm"
    Public Function BuildRoomUserForm(ByVal user As User, ByVal address As String, ByVal roomName As String) As RoomClientForm
        user.CustomDataInfo = Team.CustomInfo.DataName
        user.CustomDataHash = Team.CustomInfo.DataHash
        Dim form As RoomClientForm = New RoomClientForm(user, address, roomName, GetVersionInfo())
        form.MdiParent = Me
        form.Icon = My.Resources.PokemonBall
        AddHandler form.OnBuildBattleAgentForm, AddressOf BuildBattleAgentForm
        AddHandler form.OnBuildBattleServerForm, AddressOf BuildBattleServerForm
        AddHandler form.OnBuildBattleClientForm, AddressOf BuildBattleClientForm
        AddHandler form.OnBuildBattleObserverForm, AddressOf BuildBattleObserverForm
        AddHandler form.OnBuildFourPlayerForm, AddressOf BuildAgent4PClientForm
        AddHandler form.FormClosed, AddressOf RoomForm_FormClosed
        _roomForm = form
        Return form
    End Function
    Private Sub RoomForm_FormClosed(ByVal sender As Object, ByVal e As EventArgs)
        _roomForm = Nothing
    End Sub

    Public Function BuildBattleServerForm(ByVal name As String, ByVal mode As BattleMode, ByVal rules As List(Of BattleRule)) As BattleServerForm
        Dim form As New BattleServerForm(_team, name, mode, rules)
        form.MdiParent = Me
        Return form
    End Function
    Public Function BuildBattleClientForm(ByVal address As String, ByVal position As Byte, ByVal name As String, _
                                          ByVal mode As BattleMode) As BattleClientForm
        Dim form As New BattleClientForm(address, name, _team, mode, position)
        form.MdiParent = Me
        Return form
    End Function
    Public Function BuildBattleAgentForm(ByVal battleInfo As AgentBattleInfo) As AgentClientForm
        Dim form As New AgentClientForm(battleInfo, _team)
        form.MdiParent = Me
        Return form
    End Function

    Public Function BuildBattleObserverForm(ByVal address As String, ByVal position As Byte) As BattleObserverForm
        Dim form As New BattleObserverForm(address, position)
        form.MdiParent = Me
        Return form
    End Function
    Public Function BuildBattleObserverForm(ByVal identity As Integer, ByVal address As String, ByVal position As Byte) As BattleObserverForm
        Dim form As BattleObserverForm
        If identity = -1 Then
            form = New BattleObserverForm(address, position)
        Else
            form = New AgentObserverForm(identity, address, position)
        End If
        form.MdiParent = Me
        Return form
    End Function

    Public Function Build4PServerForm(ByVal name As String, ByVal rules As List(Of BattleRule)) As FourPlayerServerForm
        Dim form As New FourPlayerServerForm(name, rules)
        form.MdiParent = Me
        Return form
    End Function
    Public Function Build4PClientForm(ByVal address As String, ByVal name As String) As FourPlayerClientForm
        Dim form As New FourPlayerClientForm(address, name)
        form.MdiParent = Me
        Return form
    End Function
    Public Function BuildAgent4PClientForm(ByVal identity As Integer, ByVal address As String, ByVal name As String, _
                                           ByVal asHost As Boolean, ByVal callback As FourPlayerFormCallback) As FourPlayerClientForm
        Dim form As New AgentFourPlayerForm(identity, address, name, asHost, callback)
        form.MdiParent = Me
        Return form
    End Function

#End Region
End Class