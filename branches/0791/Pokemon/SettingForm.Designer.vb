<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SettingForm
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.SettingTips = New System.Windows.Forms.ToolTip(Me.components)
        Me.lblRepTtile = New System.Windows.Forms.Label
        Me.lblLogTitle = New System.Windows.Forms.Label
        Me.txtRepTitle = New System.Windows.Forms.TextBox
        Me.txtLogTitle = New System.Windows.Forms.TextBox
        Me.chkAutoSaveLog = New System.Windows.Forms.CheckBox
        Me.chkAutoSaveReplay = New System.Windows.Forms.CheckBox
        Me.chkAutoLoginUser = New System.Windows.Forms.CheckBox
        Me.grpRoom = New System.Windows.Forms.GroupBox
        Me.grpBattle = New System.Windows.Forms.GroupBox
        Me.lblLogLine = New System.Windows.Forms.Label
        Me.NumericUpDown1 = New System.Windows.Forms.NumericUpDown
        Me.cboLogPos = New System.Windows.Forms.ComboBox
        Me.lblLogPos = New System.Windows.Forms.Label
        Me.cboPMName = New System.Windows.Forms.ComboBox
        Me.lblPMName = New System.Windows.Forms.Label
        Me.lblRepPath = New System.Windows.Forms.Label
        Me.btnRepPath = New System.Windows.Forms.Button
        Me.txtRepPath = New System.Windows.Forms.TextBox
        Me.lblLogPath = New System.Windows.Forms.Label
        Me.btnLogPath = New System.Windows.Forms.Button
        Me.txtLogPath = New System.Windows.Forms.TextBox
        Me.grpRoom.SuspendLayout()
        Me.grpBattle.SuspendLayout()
        CType(Me.NumericUpDown1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SettingTips
        '
        Me.SettingTips.AutoPopDelay = 5000
        Me.SettingTips.InitialDelay = 200
        Me.SettingTips.ReshowDelay = 100
        '
        'lblRepTtile
        '
        Me.lblRepTtile.AutoSize = True
        Me.lblRepTtile.Location = New System.Drawing.Point(6, 118)
        Me.lblRepTtile.Name = "lblRepTtile"
        Me.lblRepTtile.Size = New System.Drawing.Size(53, 12)
        Me.lblRepTtile.TabIndex = 12
        Me.lblRepTtile.Text = "保存标题"
        Me.SettingTips.SetToolTip(Me.lblRepTtile, "\Player1表示1P名称(4P时为1P与2P),\Player2表示2P名称(4P时为2P与3P)," & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "\Time表示时间,\Date表示日期")
        '
        'lblLogTitle
        '
        Me.lblLogTitle.AutoSize = True
        Me.lblLogTitle.Location = New System.Drawing.Point(6, 43)
        Me.lblLogTitle.Name = "lblLogTitle"
        Me.lblLogTitle.Size = New System.Drawing.Size(53, 12)
        Me.lblLogTitle.TabIndex = 7
        Me.lblLogTitle.Text = "保存标题"
        Me.SettingTips.SetToolTip(Me.lblLogTitle, "\Player1表示1P名称(4P时为1P与2P),\Player2表示2P名称(4P时为2P与3P)," & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "\Time表示时间,\Date表示日期")
        '
        'txtRepTitle
        '
        Me.txtRepTitle.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.PokemonBattleOnline.My.MySettings.Default, "RepTitle", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.txtRepTitle.Enabled = False
        Me.txtRepTitle.Location = New System.Drawing.Point(65, 115)
        Me.txtRepTitle.Name = "txtRepTitle"
        Me.txtRepTitle.Size = New System.Drawing.Size(214, 21)
        Me.txtRepTitle.TabIndex = 10
        Me.txtRepTitle.Text = Global.PokemonBattleOnline.My.MySettings.Default.RepTitle
        Me.SettingTips.SetToolTip(Me.txtRepTitle, "\Player1表示1P名称(4P时为1P与2P),\Player2表示2P名称(4P时为2P与3P)," & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "\Time表示时间,\Date表示日期")
        '
        'txtLogTitle
        '
        Me.txtLogTitle.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.PokemonBattleOnline.My.MySettings.Default, "LogTitle", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.txtLogTitle.Enabled = False
        Me.txtLogTitle.Location = New System.Drawing.Point(65, 40)
        Me.txtLogTitle.Name = "txtLogTitle"
        Me.txtLogTitle.Size = New System.Drawing.Size(214, 21)
        Me.txtLogTitle.TabIndex = 5
        Me.txtLogTitle.Text = Global.PokemonBattleOnline.My.MySettings.Default.LogTitle
        Me.SettingTips.SetToolTip(Me.txtLogTitle, "\Player1表示1P名称(4P时为1P与2P),\Player2表示2P名称(4P时为2P与3P)," & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "\Time表示时间,\Date表示日期")
        '
        'chkAutoSaveLog
        '
        Me.chkAutoSaveLog.AutoSize = True
        Me.chkAutoSaveLog.Checked = Global.PokemonBattleOnline.My.MySettings.Default.AutoSaveLog
        Me.chkAutoSaveLog.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Global.PokemonBattleOnline.My.MySettings.Default, "AutoSaveLog", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.chkAutoSaveLog.Location = New System.Drawing.Point(6, 20)
        Me.chkAutoSaveLog.Name = "chkAutoSaveLog"
        Me.chkAutoSaveLog.Size = New System.Drawing.Size(96, 16)
        Me.chkAutoSaveLog.TabIndex = 2
        Me.chkAutoSaveLog.Text = "自动保存战报"
        Me.chkAutoSaveLog.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.SettingTips.SetToolTip(Me.chkAutoSaveLog, "该选项选中后战斗结束时会按照已设置的战报标题" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "及保存目录自动保存战报")
        Me.chkAutoSaveLog.UseVisualStyleBackColor = True
        '
        'chkAutoSaveReplay
        '
        Me.chkAutoSaveReplay.AutoSize = True
        Me.chkAutoSaveReplay.Checked = Global.PokemonBattleOnline.My.MySettings.Default.AutoSaveReplay
        Me.chkAutoSaveReplay.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Global.PokemonBattleOnline.My.MySettings.Default, "AutoSaveReplay", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.chkAutoSaveReplay.Location = New System.Drawing.Point(8, 96)
        Me.chkAutoSaveReplay.Name = "chkAutoSaveReplay"
        Me.chkAutoSaveReplay.Size = New System.Drawing.Size(96, 16)
        Me.chkAutoSaveReplay.TabIndex = 3
        Me.chkAutoSaveReplay.Text = "自动保存录象"
        Me.chkAutoSaveReplay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.SettingTips.SetToolTip(Me.chkAutoSaveReplay, "该选项选中后战斗结束时会按照已设置的录象标题" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "及保存目录自动保存录象")
        Me.chkAutoSaveReplay.UseVisualStyleBackColor = True
        '
        'chkAutoLoginUser
        '
        Me.chkAutoLoginUser.AutoSize = True
        Me.chkAutoLoginUser.Checked = Global.PokemonBattleOnline.My.MySettings.Default.AutoLoginUser
        Me.chkAutoLoginUser.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Global.PokemonBattleOnline.My.MySettings.Default, "AutoLoginUser", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.chkAutoLoginUser.Location = New System.Drawing.Point(6, 20)
        Me.chkAutoLoginUser.Name = "chkAutoLoginUser"
        Me.chkAutoLoginUser.Size = New System.Drawing.Size(96, 16)
        Me.chkAutoLoginUser.TabIndex = 0
        Me.chkAutoLoginUser.Text = "用户自动登录"
        Me.chkAutoLoginUser.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.SettingTips.SetToolTip(Me.chkAutoLoginUser, "该选项选中后登录房间时会按照已设置的名称及头像" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "直接进入房间列表界面")
        Me.chkAutoLoginUser.UseVisualStyleBackColor = True
        '
        'grpRoom
        '
        Me.grpRoom.Controls.Add(Me.chkAutoLoginUser)
        Me.grpRoom.Location = New System.Drawing.Point(12, 12)
        Me.grpRoom.Name = "grpRoom"
        Me.grpRoom.Size = New System.Drawing.Size(325, 51)
        Me.grpRoom.TabIndex = 4
        Me.grpRoom.TabStop = False
        Me.grpRoom.Text = "房间相关"
        '
        'grpBattle
        '
        Me.grpBattle.Controls.Add(Me.lblLogLine)
        Me.grpBattle.Controls.Add(Me.NumericUpDown1)
        Me.grpBattle.Controls.Add(Me.cboLogPos)
        Me.grpBattle.Controls.Add(Me.lblLogPos)
        Me.grpBattle.Controls.Add(Me.cboPMName)
        Me.grpBattle.Controls.Add(Me.lblPMName)
        Me.grpBattle.Controls.Add(Me.lblRepPath)
        Me.grpBattle.Controls.Add(Me.lblRepTtile)
        Me.grpBattle.Controls.Add(Me.btnRepPath)
        Me.grpBattle.Controls.Add(Me.txtRepTitle)
        Me.grpBattle.Controls.Add(Me.txtRepPath)
        Me.grpBattle.Controls.Add(Me.lblLogPath)
        Me.grpBattle.Controls.Add(Me.lblLogTitle)
        Me.grpBattle.Controls.Add(Me.btnLogPath)
        Me.grpBattle.Controls.Add(Me.txtLogTitle)
        Me.grpBattle.Controls.Add(Me.txtLogPath)
        Me.grpBattle.Controls.Add(Me.chkAutoSaveLog)
        Me.grpBattle.Controls.Add(Me.chkAutoSaveReplay)
        Me.grpBattle.Location = New System.Drawing.Point(12, 69)
        Me.grpBattle.Name = "grpBattle"
        Me.grpBattle.Size = New System.Drawing.Size(325, 231)
        Me.grpBattle.TabIndex = 5
        Me.grpBattle.TabStop = False
        Me.grpBattle.Text = "对战相关"
        '
        'lblLogLine
        '
        Me.lblLogLine.AutoSize = True
        Me.lblLogLine.Location = New System.Drawing.Point(219, 201)
        Me.lblLogLine.Name = "lblLogLine"
        Me.lblLogLine.Size = New System.Drawing.Size(53, 12)
        Me.lblLogLine.TabIndex = 6
        Me.lblLogLine.Text = "战报行数"
        '
        'NumericUpDown1
        '
        Me.NumericUpDown1.DataBindings.Add(New System.Windows.Forms.Binding("Value", Global.PokemonBattleOnline.My.MySettings.Default, "LogLineCount", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.NumericUpDown1.Location = New System.Drawing.Point(278, 197)
        Me.NumericUpDown1.Maximum = New Decimal(New Integer() {26, 0, 0, 0})
        Me.NumericUpDown1.Minimum = New Decimal(New Integer() {6, 0, 0, 0})
        Me.NumericUpDown1.Name = "NumericUpDown1"
        Me.NumericUpDown1.Size = New System.Drawing.Size(41, 21)
        Me.NumericUpDown1.TabIndex = 7
        Me.NumericUpDown1.Value = Global.PokemonBattleOnline.My.MySettings.Default.LogLineCount
        '
        'cboLogPos
        '
        Me.cboLogPos.FormattingEnabled = True
        Me.cboLogPos.Items.AddRange(New Object() {"中间", "右边"})
        Me.cboLogPos.Location = New System.Drawing.Point(111, 198)
        Me.cboLogPos.Name = "cboLogPos"
        Me.cboLogPos.Size = New System.Drawing.Size(77, 20)
        Me.cboLogPos.TabIndex = 17
        '
        'lblLogPos
        '
        Me.lblLogPos.AutoSize = True
        Me.lblLogPos.Location = New System.Drawing.Point(6, 201)
        Me.lblLogPos.Name = "lblLogPos"
        Me.lblLogPos.Size = New System.Drawing.Size(77, 12)
        Me.lblLogPos.TabIndex = 16
        Me.lblLogPos.Text = "战报显示位置"
        Me.lblLogPos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cboPMName
        '
        Me.cboPMName.FormattingEnabled = True
        Me.cboPMName.Items.AddRange(New Object() {"昵称", "种类名"})
        Me.cboPMName.Location = New System.Drawing.Point(111, 172)
        Me.cboPMName.Name = "cboPMName"
        Me.cboPMName.Size = New System.Drawing.Size(77, 20)
        Me.cboPMName.TabIndex = 15
        '
        'lblPMName
        '
        Me.lblPMName.AutoSize = True
        Me.lblPMName.Location = New System.Drawing.Point(6, 175)
        Me.lblPMName.Name = "lblPMName"
        Me.lblPMName.Size = New System.Drawing.Size(101, 12)
        Me.lblPMName.TabIndex = 14
        Me.lblPMName.Text = "精灵名称显示设置"
        Me.lblPMName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblRepPath
        '
        Me.lblRepPath.AutoSize = True
        Me.lblRepPath.Location = New System.Drawing.Point(6, 145)
        Me.lblRepPath.Name = "lblRepPath"
        Me.lblRepPath.Size = New System.Drawing.Size(53, 12)
        Me.lblRepPath.TabIndex = 13
        Me.lblRepPath.Text = "保存路径"
        Me.lblRepPath.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnRepPath
        '
        Me.btnRepPath.Enabled = False
        Me.btnRepPath.Location = New System.Drawing.Point(285, 140)
        Me.btnRepPath.Name = "btnRepPath"
        Me.btnRepPath.Size = New System.Drawing.Size(34, 23)
        Me.btnRepPath.TabIndex = 11
        Me.btnRepPath.Text = "..."
        Me.btnRepPath.UseVisualStyleBackColor = True
        '
        'txtRepPath
        '
        Me.txtRepPath.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.PokemonBattleOnline.My.MySettings.Default, "RepPath", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.txtRepPath.Enabled = False
        Me.txtRepPath.Location = New System.Drawing.Point(65, 142)
        Me.txtRepPath.Name = "txtRepPath"
        Me.txtRepPath.ReadOnly = True
        Me.txtRepPath.Size = New System.Drawing.Size(214, 21)
        Me.txtRepPath.TabIndex = 9
        Me.txtRepPath.Text = Global.PokemonBattleOnline.My.MySettings.Default.RepPath
        '
        'lblLogPath
        '
        Me.lblLogPath.AutoSize = True
        Me.lblLogPath.Location = New System.Drawing.Point(6, 70)
        Me.lblLogPath.Name = "lblLogPath"
        Me.lblLogPath.Size = New System.Drawing.Size(53, 12)
        Me.lblLogPath.TabIndex = 8
        Me.lblLogPath.Text = "保存路径"
        Me.lblLogPath.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnLogPath
        '
        Me.btnLogPath.Enabled = False
        Me.btnLogPath.Location = New System.Drawing.Point(285, 65)
        Me.btnLogPath.Name = "btnLogPath"
        Me.btnLogPath.Size = New System.Drawing.Size(34, 23)
        Me.btnLogPath.TabIndex = 6
        Me.btnLogPath.Text = "..."
        Me.btnLogPath.UseVisualStyleBackColor = True
        '
        'txtLogPath
        '
        Me.txtLogPath.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.PokemonBattleOnline.My.MySettings.Default, "LogPath", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.txtLogPath.Enabled = False
        Me.txtLogPath.Location = New System.Drawing.Point(65, 67)
        Me.txtLogPath.Name = "txtLogPath"
        Me.txtLogPath.ReadOnly = True
        Me.txtLogPath.Size = New System.Drawing.Size(214, 21)
        Me.txtLogPath.TabIndex = 4
        Me.txtLogPath.Text = Global.PokemonBattleOnline.My.MySettings.Default.LogPath
        '
        'SettingForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(349, 313)
        Me.Controls.Add(Me.grpBattle)
        Me.Controls.Add(Me.grpRoom)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "SettingForm"
        Me.ShowInTaskbar = False
        Me.Text = "设置"
        Me.grpRoom.ResumeLayout(False)
        Me.grpRoom.PerformLayout()
        Me.grpBattle.ResumeLayout(False)
        Me.grpBattle.PerformLayout()
        CType(Me.NumericUpDown1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents chkAutoLoginUser As System.Windows.Forms.CheckBox
    Friend WithEvents chkAutoSaveLog As System.Windows.Forms.CheckBox
    Friend WithEvents chkAutoSaveReplay As System.Windows.Forms.CheckBox
    Friend WithEvents SettingTips As System.Windows.Forms.ToolTip
    Friend WithEvents grpRoom As System.Windows.Forms.GroupBox
    Friend WithEvents grpBattle As System.Windows.Forms.GroupBox
    Friend WithEvents lblRepPath As System.Windows.Forms.Label
    Friend WithEvents lblRepTtile As System.Windows.Forms.Label
    Friend WithEvents btnRepPath As System.Windows.Forms.Button
    Friend WithEvents txtRepTitle As System.Windows.Forms.TextBox
    Friend WithEvents txtRepPath As System.Windows.Forms.TextBox
    Friend WithEvents lblLogPath As System.Windows.Forms.Label
    Friend WithEvents lblLogTitle As System.Windows.Forms.Label
    Friend WithEvents btnLogPath As System.Windows.Forms.Button
    Friend WithEvents txtLogTitle As System.Windows.Forms.TextBox
    Friend WithEvents txtLogPath As System.Windows.Forms.TextBox
    Friend WithEvents cboPMName As System.Windows.Forms.ComboBox
    Friend WithEvents lblPMName As System.Windows.Forms.Label
    Friend WithEvents lblLogLine As System.Windows.Forms.Label
    Friend WithEvents NumericUpDown1 As System.Windows.Forms.NumericUpDown
    Friend WithEvents cboLogPos As System.Windows.Forms.ComboBox
    Friend WithEvents lblLogPos As System.Windows.Forms.Label
End Class
