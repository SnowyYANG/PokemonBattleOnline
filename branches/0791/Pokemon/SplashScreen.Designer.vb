<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SplashScreen
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
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
        Me.picIcon = New System.Windows.Forms.PictureBox
        Me.ApplicationTitle = New System.Windows.Forms.Label
        Me.Version = New System.Windows.Forms.Label
        Me.lblExit = New System.Windows.Forms.LinkLabel
        Me.lblLoading = New System.Windows.Forms.Label
        CType(Me.picIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'picIcon
        '
        Me.picIcon.Location = New System.Drawing.Point(94, 12)
        Me.picIcon.Name = "picIcon"
        Me.picIcon.Size = New System.Drawing.Size(48, 48)
        Me.picIcon.TabIndex = 0
        Me.picIcon.TabStop = False
        '
        'ApplicationTitle
        '
        Me.ApplicationTitle.AutoSize = True
        Me.ApplicationTitle.Font = New System.Drawing.Font("Times New Roman", 15.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ApplicationTitle.Location = New System.Drawing.Point(46, 63)
        Me.ApplicationTitle.Name = "ApplicationTitle"
        Me.ApplicationTitle.Size = New System.Drawing.Size(145, 23)
        Me.ApplicationTitle.TabIndex = 1
        Me.ApplicationTitle.Text = "ApplicationTitle"
        Me.ApplicationTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Version
        '
        Me.Version.AutoSize = True
        Me.Version.Location = New System.Drawing.Point(47, 90)
        Me.Version.Name = "Version"
        Me.Version.Size = New System.Drawing.Size(143, 12)
        Me.Version.TabIndex = 2
        Me.Version.Text = "Version {0}.{1}.{2}.{3}"
        Me.Version.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblExit
        '
        Me.lblExit.AutoSize = True
        Me.lblExit.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.lblExit.Location = New System.Drawing.Point(172, 149)
        Me.lblExit.Name = "lblExit"
        Me.lblExit.Size = New System.Drawing.Size(53, 12)
        Me.lblExit.TabIndex = 3
        Me.lblExit.TabStop = True
        Me.lblExit.Text = "关闭程序"
        '
        'lblLoading
        '
        Me.lblLoading.AutoSize = True
        Me.lblLoading.Font = New System.Drawing.Font("宋体", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.lblLoading.Location = New System.Drawing.Point(27, 116)
        Me.lblLoading.Name = "lblLoading"
        Me.lblLoading.Size = New System.Drawing.Size(182, 14)
        Me.lblLoading.TabIndex = 4
        Me.lblLoading.Text = "正在加载数据,请稍候......"
        '
        'SplashScreen
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Silver
        Me.ClientSize = New System.Drawing.Size(237, 170)
        Me.ControlBox = False
        Me.Controls.Add(Me.lblLoading)
        Me.Controls.Add(Me.lblExit)
        Me.Controls.Add(Me.Version)
        Me.Controls.Add(Me.ApplicationTitle)
        Me.Controls.Add(Me.picIcon)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SplashScreen"
        Me.Opacity = 0.5
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.TransparencyKey = System.Drawing.SystemColors.Control
        CType(Me.picIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents picIcon As System.Windows.Forms.PictureBox
    Friend WithEvents ApplicationTitle As System.Windows.Forms.Label
    Friend WithEvents Version As System.Windows.Forms.Label
    Friend WithEvents lblExit As System.Windows.Forms.LinkLabel
    Friend WithEvents lblLoading As System.Windows.Forms.Label

End Class
