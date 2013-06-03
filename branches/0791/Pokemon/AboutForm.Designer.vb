<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AboutForm
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
        Me.lblText = New System.Windows.Forms.Label
        Me.lblText2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.cmdClose = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'lblText
        '
        Me.lblText.AutoSize = True
        Me.lblText.Location = New System.Drawing.Point(24, 50)
        Me.lblText.Name = "lblText"
        Me.lblText.Size = New System.Drawing.Size(245, 36)
        Me.lblText.TabIndex = 0
        Me.lblText.Text = "Pokemon 版权所有 (c) 1995-2007 Nintendo," & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Creatures inc , Game Freak inc." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'lblText2
        '
        Me.lblText2.AutoSize = True
        Me.lblText2.Location = New System.Drawing.Point(24, 98)
        Me.lblText2.Name = "lblText2"
        Me.lblText2.Size = New System.Drawing.Size(167, 60)
        Me.lblText2.TabIndex = 1
        Me.lblText2.Text = "本程序编写者 : fightangel" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "E-mail : fightangel@163.com" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "说明 : 本程序仅供测试"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(24, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(173, 12)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Pokemon Battle Online  版本 "
        '
        'cmdClose
        '
        Me.cmdClose.Location = New System.Drawing.Point(205, 196)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(75, 23)
        Me.cmdClose.TabIndex = 3
        Me.cmdClose.Text = "关闭(&C)"
        Me.cmdClose.UseVisualStyleBackColor = True
        '
        'FrmAbout
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(292, 231)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblText2)
        Me.Controls.Add(Me.lblText)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "FrmAbout"
        Me.Text = "关于"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblText As System.Windows.Forms.Label
    Friend WithEvents lblText2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmdClose As System.Windows.Forms.Button
End Class
