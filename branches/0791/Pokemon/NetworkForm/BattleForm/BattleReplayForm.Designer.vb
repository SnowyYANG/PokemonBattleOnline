<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BattleReplayForm
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
        Me.picScreen = New System.Windows.Forms.PictureBox
        Me.cmdOpen = New System.Windows.Forms.Button
        Me.cmdPause = New System.Windows.Forms.Button
        Me.cmdBegin = New System.Windows.Forms.Button
        Me.cmdStop = New System.Windows.Forms.Button
        Me.picText = New System.Windows.Forms.PictureBox
        CType(Me.picScreen, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picText, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'picScreen
        '
        Me.picScreen.Location = New System.Drawing.Point(14, 12)
        Me.picScreen.Name = "picScreen"
        Me.picScreen.Size = New System.Drawing.Size(256, 144)
        Me.picScreen.TabIndex = 8
        Me.picScreen.TabStop = False
        '
        'cmdOpen
        '
        Me.cmdOpen.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOpen.Location = New System.Drawing.Point(12, 247)
        Me.cmdOpen.Name = "cmdOpen"
        Me.cmdOpen.Size = New System.Drawing.Size(67, 23)
        Me.cmdOpen.TabIndex = 0
        Me.cmdOpen.Text = "打开(&O)"
        Me.cmdOpen.UseVisualStyleBackColor = True
        '
        'cmdPause
        '
        Me.cmdPause.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdPause.Enabled = False
        Me.cmdPause.Location = New System.Drawing.Point(107, 276)
        Me.cmdPause.Name = "cmdPause"
        Me.cmdPause.Size = New System.Drawing.Size(67, 23)
        Me.cmdPause.TabIndex = 2
        Me.cmdPause.Text = "暂停(&P)"
        Me.cmdPause.UseVisualStyleBackColor = True
        '
        'cmdBegin
        '
        Me.cmdBegin.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdBegin.Enabled = False
        Me.cmdBegin.Location = New System.Drawing.Point(12, 276)
        Me.cmdBegin.Name = "cmdBegin"
        Me.cmdBegin.Size = New System.Drawing.Size(67, 23)
        Me.cmdBegin.TabIndex = 1
        Me.cmdBegin.Text = "播放(&B)"
        Me.cmdBegin.UseVisualStyleBackColor = True
        '
        'cmdStop
        '
        Me.cmdStop.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdStop.Enabled = False
        Me.cmdStop.Location = New System.Drawing.Point(202, 276)
        Me.cmdStop.Name = "cmdStop"
        Me.cmdStop.Size = New System.Drawing.Size(67, 23)
        Me.cmdStop.TabIndex = 3
        Me.cmdStop.Text = "停止(&S)"
        Me.cmdStop.UseVisualStyleBackColor = True
        '
        'picText
        '
        Me.picText.Location = New System.Drawing.Point(14, 156)
        Me.picText.Name = "picText"
        Me.picText.Size = New System.Drawing.Size(256, 85)
        Me.picText.TabIndex = 9
        Me.picText.TabStop = False
        '
        'FrmBattleReplay
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(285, 311)
        Me.Controls.Add(Me.picText)
        Me.Controls.Add(Me.cmdOpen)
        Me.Controls.Add(Me.cmdStop)
        Me.Controls.Add(Me.cmdBegin)
        Me.Controls.Add(Me.cmdPause)
        Me.Controls.Add(Me.picScreen)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "FrmBattleReplay"
        Me.Text = "观看录象"
        CType(Me.picScreen, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picText, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents picScreen As System.Windows.Forms.PictureBox
    Friend WithEvents cmdOpen As System.Windows.Forms.Button
    Friend WithEvents cmdPause As System.Windows.Forms.Button
    Friend WithEvents cmdBegin As System.Windows.Forms.Button
    Friend WithEvents cmdStop As System.Windows.Forms.Button
    Friend WithEvents picText As System.Windows.Forms.PictureBox
End Class
