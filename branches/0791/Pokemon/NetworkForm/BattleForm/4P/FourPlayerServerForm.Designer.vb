<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FourPlayerServerForm
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
        Me.Player1Button = New System.Windows.Forms.Button
        Me.Player2Button = New System.Windows.Forms.Button
        Me.Player3Button = New System.Windows.Forms.Button
        Me.Player4Button = New System.Windows.Forms.Button
        Me.StartButton = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Player1Button
        '
        Me.Player1Button.Enabled = False
        Me.Player1Button.Location = New System.Drawing.Point(49, 18)
        Me.Player1Button.Name = "Player1Button"
        Me.Player1Button.Size = New System.Drawing.Size(75, 35)
        Me.Player1Button.TabIndex = 0
        Me.Player1Button.UseVisualStyleBackColor = True
        '
        'Player2Button
        '
        Me.Player2Button.Enabled = False
        Me.Player2Button.Location = New System.Drawing.Point(150, 18)
        Me.Player2Button.Name = "Player2Button"
        Me.Player2Button.Size = New System.Drawing.Size(75, 35)
        Me.Player2Button.TabIndex = 1
        Me.Player2Button.UseVisualStyleBackColor = True
        '
        'Player3Button
        '
        Me.Player3Button.Enabled = False
        Me.Player3Button.Location = New System.Drawing.Point(49, 76)
        Me.Player3Button.Name = "Player3Button"
        Me.Player3Button.Size = New System.Drawing.Size(75, 35)
        Me.Player3Button.TabIndex = 2
        Me.Player3Button.UseVisualStyleBackColor = True
        '
        'Player4Button
        '
        Me.Player4Button.Enabled = False
        Me.Player4Button.Location = New System.Drawing.Point(150, 76)
        Me.Player4Button.Name = "Player4Button"
        Me.Player4Button.Size = New System.Drawing.Size(75, 35)
        Me.Player4Button.TabIndex = 3
        Me.Player4Button.UseVisualStyleBackColor = True
        '
        'StartButton
        '
        Me.StartButton.Enabled = False
        Me.StartButton.Location = New System.Drawing.Point(187, 148)
        Me.StartButton.Name = "StartButton"
        Me.StartButton.Size = New System.Drawing.Size(75, 23)
        Me.StartButton.TabIndex = 4
        Me.StartButton.Text = "开始对战"
        Me.StartButton.UseVisualStyleBackColor = True
        '
        'FourPServerForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(274, 183)
        Me.Controls.Add(Me.StartButton)
        Me.Controls.Add(Me.Player4Button)
        Me.Controls.Add(Me.Player3Button)
        Me.Controls.Add(Me.Player2Button)
        Me.Controls.Add(Me.Player1Button)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "FourPServerForm"
        Me.Text = "4P房间"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Player1Button As System.Windows.Forms.Button
    Friend WithEvents Player2Button As System.Windows.Forms.Button
    Friend WithEvents Player3Button As System.Windows.Forms.Button
    Friend WithEvents Player4Button As System.Windows.Forms.Button
    Friend WithEvents StartButton As System.Windows.Forms.Button
End Class
