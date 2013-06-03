<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Frm4PServer
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
        Me.btn1P = New System.Windows.Forms.Button
        Me.btn2P = New System.Windows.Forms.Button
        Me.btn3P = New System.Windows.Forms.Button
        Me.btn4P = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'btn1P
        '
        Me.btn1P.Location = New System.Drawing.Point(49, 18)
        Me.btn1P.Name = "btn1P"
        Me.btn1P.Size = New System.Drawing.Size(75, 35)
        Me.btn1P.TabIndex = 0
        Me.btn1P.Text = "Player1"
        Me.btn1P.UseVisualStyleBackColor = True
        '
        'btn2P
        '
        Me.btn2P.Location = New System.Drawing.Point(150, 18)
        Me.btn2P.Name = "btn2P"
        Me.btn2P.Size = New System.Drawing.Size(75, 35)
        Me.btn2P.TabIndex = 1
        Me.btn2P.Text = "Player2"
        Me.btn2P.UseVisualStyleBackColor = True
        '
        'btn3P
        '
        Me.btn3P.Location = New System.Drawing.Point(49, 76)
        Me.btn3P.Name = "btn3P"
        Me.btn3P.Size = New System.Drawing.Size(75, 35)
        Me.btn3P.TabIndex = 2
        Me.btn3P.Text = "Player3"
        Me.btn3P.UseVisualStyleBackColor = True
        '
        'btn4P
        '
        Me.btn4P.Location = New System.Drawing.Point(150, 76)
        Me.btn4P.Name = "btn4P"
        Me.btn4P.Size = New System.Drawing.Size(75, 35)
        Me.btn4P.TabIndex = 3
        Me.btn4P.Text = "Player4"
        Me.btn4P.UseVisualStyleBackColor = True
        '
        'Frm4PServer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(274, 128)
        Me.Controls.Add(Me.btn4P)
        Me.Controls.Add(Me.btn3P)
        Me.Controls.Add(Me.btn2P)
        Me.Controls.Add(Me.btn1P)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "Frm4PServer"
        Me.Text = "4P房间"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btn1P As System.Windows.Forms.Button
    Friend WithEvents btn2P As System.Windows.Forms.Button
    Friend WithEvents btn3P As System.Windows.Forms.Button
    Friend WithEvents btn4P As System.Windows.Forms.Button
End Class
