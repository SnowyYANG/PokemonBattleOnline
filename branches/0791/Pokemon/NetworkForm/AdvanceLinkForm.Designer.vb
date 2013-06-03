<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AdvanceLinkForm
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.AddressCombo = New System.Windows.Forms.ComboBox
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.OKButton = New System.Windows.Forms.Button
        Me.AddressLabel = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'AddressCombo
        '
        Me.AddressCombo.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.AddressCombo.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.AddressCombo.FormattingEnabled = True
        Me.AddressCombo.Location = New System.Drawing.Point(24, 28)
        Me.AddressCombo.Name = "AddressCombo"
        Me.AddressCombo.Size = New System.Drawing.Size(186, 20)
        Me.AddressCombo.TabIndex = 0
        '
        'Cancel_Button
        '
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(140, 65)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(70, 23)
        Me.Cancel_Button.TabIndex = 4
        Me.Cancel_Button.Text = "取消(&C)"
        '
        'OKButton
        '
        Me.OKButton.Location = New System.Drawing.Point(24, 65)
        Me.OKButton.Name = "OKButton"
        Me.OKButton.Size = New System.Drawing.Size(70, 23)
        Me.OKButton.TabIndex = 3
        Me.OKButton.Text = "确定(&O)"
        '
        'AddressLabel
        '
        Me.AddressLabel.AutoSize = True
        Me.AddressLabel.Location = New System.Drawing.Point(28, 11)
        Me.AddressLabel.Name = "AddressLabel"
        Me.AddressLabel.Size = New System.Drawing.Size(53, 12)
        Me.AddressLabel.TabIndex = 5
        Me.AddressLabel.Text = "房间地址"
        Me.AddressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'AdvanceLinkForm
        '
        Me.AcceptButton = Me.OKButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(235, 100)
        Me.Controls.Add(Me.AddressLabel)
        Me.Controls.Add(Me.Cancel_Button)
        Me.Controls.Add(Me.OKButton)
        Me.Controls.Add(Me.AddressCombo)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "AdvanceLinkForm"
        Me.ShowInTaskbar = False
        Me.Text = "高级连接"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents AddressCombo As System.Windows.Forms.ComboBox
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents OKButton As System.Windows.Forms.Button
    Friend WithEvents AddressLabel As System.Windows.Forms.Label
End Class
