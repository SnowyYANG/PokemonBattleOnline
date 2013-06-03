<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BattleForm
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
        Me.lblProgress = New System.Windows.Forms.Label()
        Me.pgbSendData = New System.Windows.Forms.ProgressBar()
        Me.picScreenDown = New System.Windows.Forms.PictureBox()
        Me.picScreenUp = New System.Windows.Forms.PictureBox()
        Me.picText = New System.Windows.Forms.PictureBox()
        Me.PokemonLabel = New System.Windows.Forms.Label()
        Me.PokemonIcon = New System.Windows.Forms.PictureBox()
        Me.PokemonGroup = New System.Windows.Forms.GroupBox()
        Me.MainMenu = New System.Windows.Forms.MenuStrip()
        Me.图像ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RepaintMenu = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.picScreenDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picScreenUp, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PokemonIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PokemonGroup.SuspendLayout()
        Me.MainMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblProgress
        '
        Me.lblProgress.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblProgress.AutoSize = True
        Me.lblProgress.Location = New System.Drawing.Point(125, 426)
        Me.lblProgress.Name = "lblProgress"
        Me.lblProgress.Size = New System.Drawing.Size(0, 12)
        Me.lblProgress.TabIndex = 0
        '
        'pgbSendData
        '
        Me.pgbSendData.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.pgbSendData.Location = New System.Drawing.Point(119, 443)
        Me.pgbSendData.Name = "pgbSendData"
        Me.pgbSendData.Size = New System.Drawing.Size(256, 23)
        Me.pgbSendData.TabIndex = 1
        '
        'picScreenDown
        '
        Me.picScreenDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.picScreenDown.Location = New System.Drawing.Point(120, 274)
        Me.picScreenDown.Name = "picScreenDown"
        Me.picScreenDown.Size = New System.Drawing.Size(256, 192)
        Me.picScreenDown.TabIndex = 9
        Me.picScreenDown.TabStop = False
        '
        'picScreenUp
        '
        Me.picScreenUp.Location = New System.Drawing.Point(120, 45)
        Me.picScreenUp.Name = "picScreenUp"
        Me.picScreenUp.Size = New System.Drawing.Size(256, 144)
        Me.picScreenUp.TabIndex = 8
        Me.picScreenUp.TabStop = False
        '
        'picText
        '
        Me.picText.Location = New System.Drawing.Point(120, 189)
        Me.picText.Name = "picText"
        Me.picText.Size = New System.Drawing.Size(256, 85)
        Me.picText.TabIndex = 10
        Me.picText.TabStop = False
        '
        'PokemonLabel
        '
        Me.PokemonLabel.Location = New System.Drawing.Point(5, 58)
        Me.PokemonLabel.Name = "PokemonLabel"
        Me.PokemonLabel.Size = New System.Drawing.Size(100, 292)
        Me.PokemonLabel.TabIndex = 11
        '
        'PokemonIcon
        '
        Me.PokemonIcon.Location = New System.Drawing.Point(40, 20)
        Me.PokemonIcon.Name = "PokemonIcon"
        Me.PokemonIcon.Size = New System.Drawing.Size(35, 35)
        Me.PokemonIcon.TabIndex = 12
        Me.PokemonIcon.TabStop = False
        '
        'PokemonGroup
        '
        Me.PokemonGroup.Controls.Add(Me.PokemonIcon)
        Me.PokemonGroup.Controls.Add(Me.PokemonLabel)
        Me.PokemonGroup.Location = New System.Drawing.Point(3, 45)
        Me.PokemonGroup.Name = "PokemonGroup"
        Me.PokemonGroup.Size = New System.Drawing.Size(111, 357)
        Me.PokemonGroup.TabIndex = 13
        Me.PokemonGroup.TabStop = False
        Me.PokemonGroup.Text = "精灵状态"
        '
        'MainMenu
        '
        Me.MainMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.图像ToolStripMenuItem})
        Me.MainMenu.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu.Name = "MainMenu"
        Me.MainMenu.Size = New System.Drawing.Size(388, 25)
        Me.MainMenu.TabIndex = 14
        Me.MainMenu.Text = "MenuStrip1"
        '
        '图像ToolStripMenuItem
        '
        Me.图像ToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RepaintMenu})
        Me.图像ToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.MatchOnly
        Me.图像ToolStripMenuItem.Name = "图像ToolStripMenuItem"
        Me.图像ToolStripMenuItem.Size = New System.Drawing.Size(44, 21)
        Me.图像ToolStripMenuItem.Text = "图像"
        '
        'RepaintMenu
        '
        Me.RepaintMenu.Name = "RepaintMenu"
        Me.RepaintMenu.Size = New System.Drawing.Size(100, 22)
        Me.RepaintMenu.Text = "重绘"
        Me.RepaintMenu.ToolTipText = "请在图像无法显示时使用"
        '
        'BattleForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(388, 489)
        Me.Controls.Add(Me.PokemonGroup)
        Me.Controls.Add(Me.picText)
        Me.Controls.Add(Me.lblProgress)
        Me.Controls.Add(Me.pgbSendData)
        Me.Controls.Add(Me.picScreenDown)
        Me.Controls.Add(Me.picScreenUp)
        Me.Controls.Add(Me.MainMenu)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "BattleForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "BattleForm"
        Me.TransparencyKey = System.Drawing.Color.White
        CType(Me.picScreenDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picScreenUp, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PokemonIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PokemonGroup.ResumeLayout(False)
        Me.MainMenu.ResumeLayout(False)
        Me.MainMenu.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblProgress As System.Windows.Forms.Label
    Friend WithEvents pgbSendData As System.Windows.Forms.ProgressBar
    Friend WithEvents picScreenDown As System.Windows.Forms.PictureBox
    Friend WithEvents picScreenUp As System.Windows.Forms.PictureBox
    Friend WithEvents picText As System.Windows.Forms.PictureBox
    Friend WithEvents PokemonLabel As System.Windows.Forms.Label
    Friend WithEvents PokemonIcon As System.Windows.Forms.PictureBox
    Friend WithEvents PokemonGroup As System.Windows.Forms.GroupBox
    Friend WithEvents MainMenu As System.Windows.Forms.MenuStrip
    Friend WithEvents 图像ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RepaintMenu As System.Windows.Forms.ToolStripMenuItem
End Class
