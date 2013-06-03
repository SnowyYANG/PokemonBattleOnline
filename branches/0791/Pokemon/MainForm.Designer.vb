<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.MainMenu = New System.Windows.Forms.MenuStrip
        Me.文件FToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuNewTeam = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuOpenTeam = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuHistory = New System.Windows.Forms.ToolStripMenuItem
        Me.无ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.toolStripSeparator = New System.Windows.Forms.ToolStripSeparator
        Me.保存SToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.另存为AToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.toolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuExit = New System.Windows.Forms.ToolStripMenuItem
        Me.BattleMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.EditTeamMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.观看录象RToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.自定义数据ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuLoadCData = New System.Windows.Forms.ToolStripMenuItem
        Me.cboCustomData = New System.Windows.Forms.ToolStripComboBox
        Me.EditCustomMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.资料集合ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.精灵资料PToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.技能ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.工具TToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.选项OToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.帮助HToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.关于AToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.MainMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'MainMenu
        '
        Me.MainMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.文件FToolStripMenuItem1, Me.BattleMenuItem, Me.自定义数据ToolStripMenuItem, Me.资料集合ToolStripMenuItem, Me.工具TToolStripMenuItem, Me.帮助HToolStripMenuItem})
        Me.MainMenu.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu.Name = "MainMenu"
        Me.MainMenu.Size = New System.Drawing.Size(645, 24)
        Me.MainMenu.TabIndex = 1
        Me.MainMenu.Text = "MenuStrip1"
        '
        '文件FToolStripMenuItem1
        '
        Me.文件FToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuNewTeam, Me.mnuOpenTeam, Me.mnuHistory, Me.toolStripSeparator, Me.保存SToolStripMenuItem, Me.另存为AToolStripMenuItem, Me.toolStripSeparator2, Me.mnuExit})
        Me.文件FToolStripMenuItem1.Name = "文件FToolStripMenuItem1"
        Me.文件FToolStripMenuItem1.Size = New System.Drawing.Size(59, 20)
        Me.文件FToolStripMenuItem1.Text = "文件(&F)"
        '
        'mnuNewTeam
        '
        Me.mnuNewTeam.Image = CType(resources.GetObject("mnuNewTeam.Image"), System.Drawing.Image)
        Me.mnuNewTeam.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.mnuNewTeam.Name = "mnuNewTeam"
        Me.mnuNewTeam.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.N), System.Windows.Forms.Keys)
        Me.mnuNewTeam.Size = New System.Drawing.Size(153, 22)
        Me.mnuNewTeam.Text = "新建(&N)"
        '
        'mnuOpenTeam
        '
        Me.mnuOpenTeam.Image = CType(resources.GetObject("mnuOpenTeam.Image"), System.Drawing.Image)
        Me.mnuOpenTeam.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.mnuOpenTeam.Name = "mnuOpenTeam"
        Me.mnuOpenTeam.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.mnuOpenTeam.Size = New System.Drawing.Size(153, 22)
        Me.mnuOpenTeam.Text = "打开(&O)"
        '
        'mnuHistory
        '
        Me.mnuHistory.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.无ToolStripMenuItem1})
        Me.mnuHistory.Name = "mnuHistory"
        Me.mnuHistory.Size = New System.Drawing.Size(153, 22)
        Me.mnuHistory.Text = "过去打开(&H)"
        '
        '无ToolStripMenuItem1
        '
        Me.无ToolStripMenuItem1.Name = "无ToolStripMenuItem1"
        Me.无ToolStripMenuItem1.Size = New System.Drawing.Size(152, 22)
        Me.无ToolStripMenuItem1.Text = "无"
        '
        'toolStripSeparator
        '
        Me.toolStripSeparator.Name = "toolStripSeparator"
        Me.toolStripSeparator.Size = New System.Drawing.Size(150, 6)
        Me.toolStripSeparator.Visible = False
        '
        '保存SToolStripMenuItem
        '
        Me.保存SToolStripMenuItem.Image = CType(resources.GetObject("保存SToolStripMenuItem.Image"), System.Drawing.Image)
        Me.保存SToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.保存SToolStripMenuItem.Name = "保存SToolStripMenuItem"
        Me.保存SToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.保存SToolStripMenuItem.Size = New System.Drawing.Size(153, 22)
        Me.保存SToolStripMenuItem.Text = "保存(&S)"
        Me.保存SToolStripMenuItem.Visible = False
        '
        '另存为AToolStripMenuItem
        '
        Me.另存为AToolStripMenuItem.Name = "另存为AToolStripMenuItem"
        Me.另存为AToolStripMenuItem.Size = New System.Drawing.Size(153, 22)
        Me.另存为AToolStripMenuItem.Text = "另存为(&A)"
        Me.另存为AToolStripMenuItem.Visible = False
        '
        'toolStripSeparator2
        '
        Me.toolStripSeparator2.Name = "toolStripSeparator2"
        Me.toolStripSeparator2.Size = New System.Drawing.Size(150, 6)
        '
        'mnuExit
        '
        Me.mnuExit.Name = "mnuExit"
        Me.mnuExit.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.X), System.Windows.Forms.Keys)
        Me.mnuExit.Size = New System.Drawing.Size(153, 22)
        Me.mnuExit.Text = "退出(&X)"
        '
        'BattleMenuItem
        '
        Me.BattleMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EditTeamMenuItem, Me.观看录象RToolStripMenuItem})
        Me.BattleMenuItem.Name = "BattleMenuItem"
        Me.BattleMenuItem.Size = New System.Drawing.Size(59, 20)
        Me.BattleMenuItem.Text = "对战(&A)"
        '
        'EditTeamMenuItem
        '
        Me.EditTeamMenuItem.Name = "EditTeamMenuItem"
        Me.EditTeamMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.T), System.Windows.Forms.Keys)
        Me.EditTeamMenuItem.Size = New System.Drawing.Size(177, 22)
        Me.EditTeamMenuItem.Text = "编辑队伍(&T)"
        '
        '观看录象RToolStripMenuItem
        '
        Me.观看录象RToolStripMenuItem.Name = "观看录象RToolStripMenuItem"
        Me.观看录象RToolStripMenuItem.Size = New System.Drawing.Size(177, 22)
        Me.观看录象RToolStripMenuItem.Text = "观看录象(&R)"
        '
        '自定义数据ToolStripMenuItem
        '
        Me.自定义数据ToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuLoadCData, Me.EditCustomMenuItem})
        Me.自定义数据ToolStripMenuItem.Name = "自定义数据ToolStripMenuItem"
        Me.自定义数据ToolStripMenuItem.Size = New System.Drawing.Size(95, 20)
        Me.自定义数据ToolStripMenuItem.Text = "自定义数据(&C)"
        '
        'mnuLoadCData
        '
        Me.mnuLoadCData.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cboCustomData})
        Me.mnuLoadCData.Name = "mnuLoadCData"
        Me.mnuLoadCData.Size = New System.Drawing.Size(112, 22)
        Me.mnuLoadCData.Text = "设置(&L)"
        '
        'cboCustomData
        '
        Me.cboCustomData.Items.AddRange(New Object() {"不使用"})
        Me.cboCustomData.Name = "cboCustomData"
        Me.cboCustomData.Size = New System.Drawing.Size(121, 20)
        '
        'EditCustomMenuItem
        '
        Me.EditCustomMenuItem.Name = "EditCustomMenuItem"
        Me.EditCustomMenuItem.Size = New System.Drawing.Size(112, 22)
        Me.EditCustomMenuItem.Text = "编辑(&E)"
        '
        '资料集合ToolStripMenuItem
        '
        Me.资料集合ToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.精灵资料PToolStripMenuItem, Me.技能ToolStripMenuItem})
        Me.资料集合ToolStripMenuItem.Name = "资料集合ToolStripMenuItem"
        Me.资料集合ToolStripMenuItem.Size = New System.Drawing.Size(83, 20)
        Me.资料集合ToolStripMenuItem.Text = "资料集合(&D)"
        '
        '精灵资料PToolStripMenuItem
        '
        Me.精灵资料PToolStripMenuItem.Name = "精灵资料PToolStripMenuItem"
        Me.精灵资料PToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.P), System.Windows.Forms.Keys)
        Me.精灵资料PToolStripMenuItem.Size = New System.Drawing.Size(177, 22)
        Me.精灵资料PToolStripMenuItem.Text = "精灵资料(&P)"
        '
        '技能ToolStripMenuItem
        '
        Me.技能ToolStripMenuItem.Name = "技能ToolStripMenuItem"
        Me.技能ToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.M), System.Windows.Forms.Keys)
        Me.技能ToolStripMenuItem.Size = New System.Drawing.Size(177, 22)
        Me.技能ToolStripMenuItem.Text = "技能资料(&M)"
        '
        '工具TToolStripMenuItem
        '
        Me.工具TToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.选项OToolStripMenuItem})
        Me.工具TToolStripMenuItem.Name = "工具TToolStripMenuItem"
        Me.工具TToolStripMenuItem.Size = New System.Drawing.Size(59, 20)
        Me.工具TToolStripMenuItem.Text = "工具(&T)"
        '
        '选项OToolStripMenuItem
        '
        Me.选项OToolStripMenuItem.Image = CType(resources.GetObject("选项OToolStripMenuItem.Image"), System.Drawing.Image)
        Me.选项OToolStripMenuItem.Name = "选项OToolStripMenuItem"
        Me.选项OToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.I), System.Windows.Forms.Keys)
        Me.选项OToolStripMenuItem.Size = New System.Drawing.Size(153, 22)
        Me.选项OToolStripMenuItem.Text = "选项(&O)"
        '
        '帮助HToolStripMenuItem
        '
        Me.帮助HToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.关于AToolStripMenuItem})
        Me.帮助HToolStripMenuItem.Name = "帮助HToolStripMenuItem"
        Me.帮助HToolStripMenuItem.Size = New System.Drawing.Size(59, 20)
        Me.帮助HToolStripMenuItem.Text = "帮助(&H)"
        '
        '关于AToolStripMenuItem
        '
        Me.关于AToolStripMenuItem.Name = "关于AToolStripMenuItem"
        Me.关于AToolStripMenuItem.Size = New System.Drawing.Size(130, 22)
        Me.关于AToolStripMenuItem.Text = "关于(&A)..."
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(645, 477)
        Me.Controls.Add(Me.MainMenu)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.IsMdiContainer = True
        Me.MainMenuStrip = Me.MainMenu
        Me.Name = "MainForm"
        Me.Text = "PokemonBattleOnline"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.MainMenu.ResumeLayout(False)
        Me.MainMenu.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MainMenu As System.Windows.Forms.MenuStrip
    Friend WithEvents 帮助HToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 关于AToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 资料集合ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 精灵资料PToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 技能ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BattleMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EditTeamMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 观看录象RToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 文件FToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuNewTeam As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuOpenTeam As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents toolStripSeparator As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents 保存SToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 另存为AToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents toolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents 工具TToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 选项OToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuExit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuHistory As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 自定义数据ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuLoadCData As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EditCustomMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 无ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cboCustomData As System.Windows.Forms.ToolStripComboBox
End Class
