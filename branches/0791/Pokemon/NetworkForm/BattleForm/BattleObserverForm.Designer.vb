<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BattleObserverForm
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
        Me.picText = New System.Windows.Forms.PictureBox
        CType(Me.picScreen, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picText, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'picScreen
        '
        Me.picScreen.Location = New System.Drawing.Point(13, 12)
        Me.picScreen.Name = "picScreen"
        Me.picScreen.Size = New System.Drawing.Size(256, 144)
        Me.picScreen.TabIndex = 7
        Me.picScreen.TabStop = False
        '
        'picText
        '
        Me.picText.Location = New System.Drawing.Point(13, 156)
        Me.picText.Name = "picText"
        Me.picText.Size = New System.Drawing.Size(256, 85)
        Me.picText.TabIndex = 8
        Me.picText.TabStop = False
        '
        'FrmBattleWatcher
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(282, 252)
        Me.Controls.Add(Me.picText)
        Me.Controls.Add(Me.picScreen)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "FrmBattleWatcher"
        Me.Text = "观战窗口"
        CType(Me.picScreen, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picText, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents picScreen As System.Windows.Forms.PictureBox
    Friend WithEvents picText As System.Windows.Forms.PictureBox
End Class
