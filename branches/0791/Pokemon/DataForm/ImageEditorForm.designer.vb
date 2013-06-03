<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ImageEditorForm
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
        Me.picFront = New System.Windows.Forms.PictureBox
        Me.picBack = New System.Windows.Forms.PictureBox
        Me.btnBack = New System.Windows.Forms.Button
        Me.btnFront = New System.Windows.Forms.Button
        Me.picIcon = New System.Windows.Forms.PictureBox
        Me.btnIcon = New System.Windows.Forms.Button
        Me.btnFront2 = New System.Windows.Forms.Button
        Me.btnBack2 = New System.Windows.Forms.Button
        Me.picBack2 = New System.Windows.Forms.PictureBox
        Me.picFront2 = New System.Windows.Forms.PictureBox
        Me.btnFrame2 = New System.Windows.Forms.Button
        Me.picFrame2 = New System.Windows.Forms.PictureBox
        Me.btnFrame = New System.Windows.Forms.Button
        Me.picFrame = New System.Windows.Forms.PictureBox
        Me.lblIcon = New System.Windows.Forms.Label
        Me.lblFront = New System.Windows.Forms.Label
        Me.lblFront2 = New System.Windows.Forms.Label
        Me.lblFrame = New System.Windows.Forms.Label
        Me.lblFrame2 = New System.Windows.Forms.Label
        Me.lblBack = New System.Windows.Forms.Label
        Me.lblBack2 = New System.Windows.Forms.Label
        CType(Me.picFront, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picBack, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picBack2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picFront2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picFrame2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picFrame, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'picFront
        '
        Me.picFront.BackColor = System.Drawing.SystemColors.Control
        Me.picFront.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picFront.Location = New System.Drawing.Point(10, 27)
        Me.picFront.Name = "picFront"
        Me.picFront.Size = New System.Drawing.Size(80, 80)
        Me.picFront.TabIndex = 0
        Me.picFront.TabStop = False
        '
        'picBack
        '
        Me.picBack.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picBack.Location = New System.Drawing.Point(182, 27)
        Me.picBack.Name = "picBack"
        Me.picBack.Size = New System.Drawing.Size(80, 80)
        Me.picBack.TabIndex = 1
        Me.picBack.TabStop = False
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(187, 113)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(70, 23)
        Me.btnBack.TabIndex = 2
        Me.btnBack.Text = "设置"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'btnFront
        '
        Me.btnFront.Location = New System.Drawing.Point(15, 113)
        Me.btnFront.Name = "btnFront"
        Me.btnFront.Size = New System.Drawing.Size(70, 23)
        Me.btnFront.TabIndex = 0
        Me.btnFront.Text = "设置"
        Me.btnFront.UseVisualStyleBackColor = True
        '
        'picIcon
        '
        Me.picIcon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picIcon.Location = New System.Drawing.Point(281, 27)
        Me.picIcon.Name = "picIcon"
        Me.picIcon.Size = New System.Drawing.Size(32, 32)
        Me.picIcon.TabIndex = 4
        Me.picIcon.TabStop = False
        '
        'btnIcon
        '
        Me.btnIcon.Location = New System.Drawing.Point(271, 62)
        Me.btnIcon.Name = "btnIcon"
        Me.btnIcon.Size = New System.Drawing.Size(53, 23)
        Me.btnIcon.TabIndex = 6
        Me.btnIcon.Text = "设置"
        Me.btnIcon.UseVisualStyleBackColor = True
        '
        'btnFront2
        '
        Me.btnFront2.Location = New System.Drawing.Point(15, 253)
        Me.btnFront2.Name = "btnFront2"
        Me.btnFront2.Size = New System.Drawing.Size(70, 23)
        Me.btnFront2.TabIndex = 3
        Me.btnFront2.Text = "设置"
        Me.btnFront2.UseVisualStyleBackColor = True
        '
        'btnBack2
        '
        Me.btnBack2.Location = New System.Drawing.Point(187, 253)
        Me.btnBack2.Name = "btnBack2"
        Me.btnBack2.Size = New System.Drawing.Size(70, 23)
        Me.btnBack2.TabIndex = 5
        Me.btnBack2.Text = "设置"
        Me.btnBack2.UseVisualStyleBackColor = True
        '
        'picBack2
        '
        Me.picBack2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picBack2.Location = New System.Drawing.Point(182, 167)
        Me.picBack2.Name = "picBack2"
        Me.picBack2.Size = New System.Drawing.Size(80, 80)
        Me.picBack2.TabIndex = 7
        Me.picBack2.TabStop = False
        '
        'picFront2
        '
        Me.picFront2.BackColor = System.Drawing.SystemColors.Control
        Me.picFront2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picFront2.Location = New System.Drawing.Point(10, 167)
        Me.picFront2.Name = "picFront2"
        Me.picFront2.Size = New System.Drawing.Size(80, 80)
        Me.picFront2.TabIndex = 6
        Me.picFront2.TabStop = False
        '
        'btnFrame2
        '
        Me.btnFrame2.Location = New System.Drawing.Point(101, 253)
        Me.btnFrame2.Name = "btnFrame2"
        Me.btnFrame2.Size = New System.Drawing.Size(70, 23)
        Me.btnFrame2.TabIndex = 4
        Me.btnFrame2.Text = "设置"
        Me.btnFrame2.UseVisualStyleBackColor = True
        '
        'picFrame2
        '
        Me.picFrame2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picFrame2.Location = New System.Drawing.Point(96, 167)
        Me.picFrame2.Name = "picFrame2"
        Me.picFrame2.Size = New System.Drawing.Size(80, 80)
        Me.picFrame2.TabIndex = 12
        Me.picFrame2.TabStop = False
        '
        'btnFrame
        '
        Me.btnFrame.Location = New System.Drawing.Point(101, 113)
        Me.btnFrame.Name = "btnFrame"
        Me.btnFrame.Size = New System.Drawing.Size(70, 23)
        Me.btnFrame.TabIndex = 1
        Me.btnFrame.Text = "设置"
        Me.btnFrame.UseVisualStyleBackColor = True
        '
        'picFrame
        '
        Me.picFrame.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picFrame.Location = New System.Drawing.Point(96, 27)
        Me.picFrame.Name = "picFrame"
        Me.picFrame.Size = New System.Drawing.Size(80, 80)
        Me.picFrame.TabIndex = 10
        Me.picFrame.TabStop = False
        '
        'lblIcon
        '
        Me.lblIcon.AutoSize = True
        Me.lblIcon.Location = New System.Drawing.Point(283, 9)
        Me.lblIcon.Name = "lblIcon"
        Me.lblIcon.Size = New System.Drawing.Size(29, 12)
        Me.lblIcon.TabIndex = 10
        Me.lblIcon.Text = "图标"
        '
        'lblFront
        '
        Me.lblFront.AutoSize = True
        Me.lblFront.Location = New System.Drawing.Point(30, 9)
        Me.lblFront.Name = "lblFront"
        Me.lblFront.Size = New System.Drawing.Size(41, 12)
        Me.lblFront.TabIndex = 7
        Me.lblFront.Text = "正面图"
        '
        'lblFront2
        '
        Me.lblFront2.AutoSize = True
        Me.lblFront2.Location = New System.Drawing.Point(27, 152)
        Me.lblFront2.Name = "lblFront2"
        Me.lblFront2.Size = New System.Drawing.Size(47, 12)
        Me.lblFront2.TabIndex = 11
        Me.lblFront2.Text = "正面图2"
        '
        'lblFrame
        '
        Me.lblFrame.AutoSize = True
        Me.lblFrame.Location = New System.Drawing.Point(116, 9)
        Me.lblFrame.Name = "lblFrame"
        Me.lblFrame.Size = New System.Drawing.Size(41, 12)
        Me.lblFrame.TabIndex = 8
        Me.lblFrame.Text = "出场图"
        '
        'lblFrame2
        '
        Me.lblFrame2.AutoSize = True
        Me.lblFrame2.Location = New System.Drawing.Point(113, 152)
        Me.lblFrame2.Name = "lblFrame2"
        Me.lblFrame2.Size = New System.Drawing.Size(47, 12)
        Me.lblFrame2.TabIndex = 12
        Me.lblFrame2.Text = "出场图2"
        '
        'lblBack
        '
        Me.lblBack.AutoSize = True
        Me.lblBack.Location = New System.Drawing.Point(202, 9)
        Me.lblBack.Name = "lblBack"
        Me.lblBack.Size = New System.Drawing.Size(41, 12)
        Me.lblBack.TabIndex = 9
        Me.lblBack.Text = "背面图"
        '
        'lblBack2
        '
        Me.lblBack2.AutoSize = True
        Me.lblBack2.Location = New System.Drawing.Point(199, 152)
        Me.lblBack2.Name = "lblBack2"
        Me.lblBack2.Size = New System.Drawing.Size(47, 12)
        Me.lblBack2.TabIndex = 13
        Me.lblBack2.Text = "背面图2"
        '
        'FrmPMImgEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(337, 288)
        Me.Controls.Add(Me.lblBack2)
        Me.Controls.Add(Me.lblBack)
        Me.Controls.Add(Me.lblFrame2)
        Me.Controls.Add(Me.lblFrame)
        Me.Controls.Add(Me.lblFront2)
        Me.Controls.Add(Me.lblFront)
        Me.Controls.Add(Me.lblIcon)
        Me.Controls.Add(Me.btnFrame2)
        Me.Controls.Add(Me.picFrame2)
        Me.Controls.Add(Me.btnFrame)
        Me.Controls.Add(Me.picFrame)
        Me.Controls.Add(Me.btnFront2)
        Me.Controls.Add(Me.btnBack2)
        Me.Controls.Add(Me.picBack2)
        Me.Controls.Add(Me.picFront2)
        Me.Controls.Add(Me.btnIcon)
        Me.Controls.Add(Me.picIcon)
        Me.Controls.Add(Me.btnFront)
        Me.Controls.Add(Me.btnBack)
        Me.Controls.Add(Me.picBack)
        Me.Controls.Add(Me.picFront)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "FrmPMImgEditor"
        Me.ShowInTaskbar = False
        Me.Text = "编辑精灵图象"
        CType(Me.picFront, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picBack, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picIcon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picBack2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picFront2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picFrame2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picFrame, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents picFront As System.Windows.Forms.PictureBox
    Friend WithEvents picBack As System.Windows.Forms.PictureBox
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents btnFront As System.Windows.Forms.Button
    Friend WithEvents picIcon As System.Windows.Forms.PictureBox
    Friend WithEvents btnIcon As System.Windows.Forms.Button
    Friend WithEvents btnFront2 As System.Windows.Forms.Button
    Friend WithEvents btnBack2 As System.Windows.Forms.Button
    Friend WithEvents picBack2 As System.Windows.Forms.PictureBox
    Friend WithEvents picFront2 As System.Windows.Forms.PictureBox
    Friend WithEvents btnFrame2 As System.Windows.Forms.Button
    Friend WithEvents picFrame2 As System.Windows.Forms.PictureBox
    Friend WithEvents btnFrame As System.Windows.Forms.Button
    Friend WithEvents picFrame As System.Windows.Forms.PictureBox
    Friend WithEvents lblIcon As System.Windows.Forms.Label
    Friend WithEvents lblFront As System.Windows.Forms.Label
    Friend WithEvents lblFront2 As System.Windows.Forms.Label
    Friend WithEvents lblFrame As System.Windows.Forms.Label
    Friend WithEvents lblFrame2 As System.Windows.Forms.Label
    Friend WithEvents lblBack As System.Windows.Forms.Label
    Friend WithEvents lblBack2 As System.Windows.Forms.Label
End Class
