<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class IndexForm
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
        Me.cmdTeam = New System.Windows.Forms.Button
        Me.StatusStrip = New System.Windows.Forms.StatusStrip
        Me.lblTeam = New System.Windows.Forms.ToolStripStatusLabel
        Me.cmdBattle = New System.Windows.Forms.Button
        Me.cmdExit = New System.Windows.Forms.Button
        Me.StatusStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdTeam
        '
        Me.cmdTeam.Location = New System.Drawing.Point(82, 12)
        Me.cmdTeam.Name = "cmdTeam"
        Me.cmdTeam.Size = New System.Drawing.Size(90, 45)
        Me.cmdTeam.TabIndex = 0
        Me.cmdTeam.Text = "编辑队伍(&E)"
        Me.cmdTeam.UseVisualStyleBackColor = True
        '
        'StatusStrip
        '
        Me.StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblTeam})
        Me.StatusStrip.Location = New System.Drawing.Point(0, 201)
        Me.StatusStrip.Name = "StatusStrip"
        Me.StatusStrip.Size = New System.Drawing.Size(256, 22)
        Me.StatusStrip.SizingGrip = False
        Me.StatusStrip.TabIndex = 3
        '
        'lblTeam
        '
        Me.lblTeam.Name = "lblTeam"
        Me.lblTeam.Size = New System.Drawing.Size(0, 17)
        '
        'cmdBattle
        '
        Me.cmdBattle.Location = New System.Drawing.Point(82, 81)
        Me.cmdBattle.Name = "cmdBattle"
        Me.cmdBattle.Size = New System.Drawing.Size(90, 45)
        Me.cmdBattle.TabIndex = 1
        Me.cmdBattle.Text = "对战(&B)"
        Me.cmdBattle.UseVisualStyleBackColor = True
        '
        'cmdExit
        '
        Me.cmdExit.Location = New System.Drawing.Point(82, 150)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.Size = New System.Drawing.Size(90, 29)
        Me.cmdExit.TabIndex = 2
        Me.cmdExit.Text = "退出(&X)"
        Me.cmdExit.UseVisualStyleBackColor = True
        '
        'FrmIndex
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(256, 223)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdBattle)
        Me.Controls.Add(Me.StatusStrip)
        Me.Controls.Add(Me.cmdTeam)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "FrmIndex"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "FrmIndex"
        Me.StatusStrip.ResumeLayout(False)
        Me.StatusStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmdTeam As System.Windows.Forms.Button
    Friend WithEvents StatusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents lblTeam As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents cmdBattle As System.Windows.Forms.Button
    Friend WithEvents cmdExit As System.Windows.Forms.Button
End Class
