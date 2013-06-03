<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SelectorForm
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
        Me.PersonalButton = New System.Windows.Forms.Button
        Me.LinkRoomButton = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'PersonalButton
        '
        Me.PersonalButton.Location = New System.Drawing.Point(65, 92)
        Me.PersonalButton.Name = "PersonalButton"
        Me.PersonalButton.Size = New System.Drawing.Size(100, 42)
        Me.PersonalButton.TabIndex = 2
        Me.PersonalButton.Text = "私人对战(&P)"
        Me.PersonalButton.UseVisualStyleBackColor = True
        '
        'LinkRoomButton
        '
        Me.LinkRoomButton.Location = New System.Drawing.Point(65, 22)
        Me.LinkRoomButton.Name = "LinkRoomButton"
        Me.LinkRoomButton.Size = New System.Drawing.Size(100, 42)
        Me.LinkRoomButton.TabIndex = 0
        Me.LinkRoomButton.Text = "连接房间(&N)"
        Me.LinkRoomButton.UseVisualStyleBackColor = True
        '
        'SelectorForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(231, 169)
        Me.Controls.Add(Me.LinkRoomButton)
        Me.Controls.Add(Me.PersonalButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "SelectorForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "请选择对战方式"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PersonalButton As System.Windows.Forms.Button
    Friend WithEvents LinkRoomButton As System.Windows.Forms.Button
End Class
