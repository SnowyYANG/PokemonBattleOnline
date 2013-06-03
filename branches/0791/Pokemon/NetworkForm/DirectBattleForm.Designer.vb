<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DirectBattleForm
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
        Me.lblName = New System.Windows.Forms.Label()
        Me.NameText = New System.Windows.Forms.TextBox()
        Me.ServerRadio = New System.Windows.Forms.RadioButton()
        Me.ClientRadio = New System.Windows.Forms.RadioButton()
        Me.ObseverRadio = New System.Windows.Forms.RadioButton()
        Me.AddressText = New System.Windows.Forms.TextBox()
        Me.lblIP = New System.Windows.Forms.Label()
        Me.OKButton = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.ModeCombo = New System.Windows.Forms.ComboBox()
        Me.lblBattleMode = New System.Windows.Forms.Label()
        Me.RuleGroup = New System.Windows.Forms.GroupBox()
        Me.RandomCheck = New System.Windows.Forms.CheckBox()
        Me.PPUpCheck = New System.Windows.Forms.CheckBox()
        Me.RuleGroup.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.Location = New System.Drawing.Point(67, 58)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(53, 12)
        Me.lblName.TabIndex = 7
        Me.lblName.Text = "用户名："
        Me.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'NameText
        '
        Me.NameText.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.PokemonBattleOnline.My.MySettings.Default, "UserName", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.NameText.ImeMode = System.Windows.Forms.ImeMode.Alpha
        Me.NameText.Location = New System.Drawing.Point(125, 55)
        Me.NameText.Name = "NameText"
        Me.NameText.Size = New System.Drawing.Size(90, 21)
        Me.NameText.TabIndex = 0
        Me.NameText.Text = Global.PokemonBattleOnline.My.MySettings.Default.UserName
        '
        'ServerRadio
        '
        Me.ServerRadio.AutoSize = True
        Me.ServerRadio.Checked = True
        Me.ServerRadio.Location = New System.Drawing.Point(43, 23)
        Me.ServerRadio.Name = "ServerRadio"
        Me.ServerRadio.Size = New System.Drawing.Size(47, 16)
        Me.ServerRadio.TabIndex = 4
        Me.ServerRadio.TabStop = True
        Me.ServerRadio.Text = "主机"
        Me.ServerRadio.UseVisualStyleBackColor = True
        '
        'ClientRadio
        '
        Me.ClientRadio.AutoSize = True
        Me.ClientRadio.Location = New System.Drawing.Point(117, 23)
        Me.ClientRadio.Name = "ClientRadio"
        Me.ClientRadio.Size = New System.Drawing.Size(59, 16)
        Me.ClientRadio.TabIndex = 5
        Me.ClientRadio.Text = "客户端"
        Me.ClientRadio.UseVisualStyleBackColor = True
        '
        'ObseverRadio
        '
        Me.ObseverRadio.AutoSize = True
        Me.ObseverRadio.Location = New System.Drawing.Point(203, 23)
        Me.ObseverRadio.Name = "ObseverRadio"
        Me.ObseverRadio.Size = New System.Drawing.Size(47, 16)
        Me.ObseverRadio.TabIndex = 6
        Me.ObseverRadio.Text = "观战"
        Me.ObseverRadio.UseVisualStyleBackColor = True
        '
        'AddressText
        '
        Me.AddressText.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.PokemonBattleOnline.My.MySettings.Default, "IPSetting", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.AddressText.Location = New System.Drawing.Point(125, 88)
        Me.AddressText.Name = "AddressText"
        Me.AddressText.ReadOnly = True
        Me.AddressText.Size = New System.Drawing.Size(90, 21)
        Me.AddressText.TabIndex = 1
        Me.AddressText.Text = Global.PokemonBattleOnline.My.MySettings.Default.IPSetting
        '
        'lblIP
        '
        Me.lblIP.AutoSize = True
        Me.lblIP.Location = New System.Drawing.Point(55, 91)
        Me.lblIP.Name = "lblIP"
        Me.lblIP.Size = New System.Drawing.Size(65, 12)
        Me.lblIP.TabIndex = 8
        Me.lblIP.Text = "主机地址："
        Me.lblIP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'OKButton
        '
        Me.OKButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OKButton.Location = New System.Drawing.Point(123, 220)
        Me.OKButton.Name = "OKButton"
        Me.OKButton.Size = New System.Drawing.Size(75, 23)
        Me.OKButton.TabIndex = 2
        Me.OKButton.Text = "确定"
        Me.OKButton.UseVisualStyleBackColor = True
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(204, 220)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(75, 23)
        Me.Cancel_Button.TabIndex = 3
        Me.Cancel_Button.Text = "取消"
        Me.Cancel_Button.UseVisualStyleBackColor = True
        '
        'ModeCombo
        '
        Me.ModeCombo.FormattingEnabled = True
        Me.ModeCombo.Items.AddRange(New Object() {"1V1", "2V2", "4P"})
        Me.ModeCombo.Location = New System.Drawing.Point(125, 125)
        Me.ModeCombo.Name = "ModeCombo"
        Me.ModeCombo.Size = New System.Drawing.Size(90, 20)
        Me.ModeCombo.TabIndex = 9
        '
        'lblBattleMode
        '
        Me.lblBattleMode.AutoSize = True
        Me.lblBattleMode.Location = New System.Drawing.Point(54, 128)
        Me.lblBattleMode.Name = "lblBattleMode"
        Me.lblBattleMode.Size = New System.Drawing.Size(65, 12)
        Me.lblBattleMode.TabIndex = 10
        Me.lblBattleMode.Text = "对战模式："
        Me.lblBattleMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'RuleGroup
        '
        Me.RuleGroup.Controls.Add(Me.RandomCheck)
        Me.RuleGroup.Controls.Add(Me.PPUpCheck)
        Me.RuleGroup.Location = New System.Drawing.Point(12, 151)
        Me.RuleGroup.Name = "RuleGroup"
        Me.RuleGroup.Size = New System.Drawing.Size(267, 63)
        Me.RuleGroup.TabIndex = 11
        Me.RuleGroup.TabStop = False
        Me.RuleGroup.Text = "可选规则"
        '
        'RandomCheck
        '
        Me.RandomCheck.AutoSize = True
        Me.RandomCheck.Location = New System.Drawing.Point(177, 20)
        Me.RandomCheck.Name = "RandomCheck"
        Me.RandomCheck.Size = New System.Drawing.Size(72, 16)
        Me.RandomCheck.TabIndex = 1
        Me.RandomCheck.Text = "随机队伍"
        Me.RandomCheck.UseVisualStyleBackColor = True
        '
        'PPUpCheck
        '
        Me.PPUpCheck.AutoSize = True
        Me.PPUpCheck.Checked = True
        Me.PPUpCheck.CheckState = System.Windows.Forms.CheckState.Checked
        Me.PPUpCheck.Location = New System.Drawing.Point(14, 20)
        Me.PPUpCheck.Name = "PPUpCheck"
        Me.PPUpCheck.Size = New System.Drawing.Size(84, 16)
        Me.PPUpCheck.TabIndex = 0
        Me.PPUpCheck.Text = "PP上限提升"
        Me.PPUpCheck.UseVisualStyleBackColor = True
        '
        'DirectBattleForm
        '
        Me.AcceptButton = Me.OKButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(291, 255)
        Me.Controls.Add(Me.RuleGroup)
        Me.Controls.Add(Me.lblBattleMode)
        Me.Controls.Add(Me.ModeCombo)
        Me.Controls.Add(Me.Cancel_Button)
        Me.Controls.Add(Me.OKButton)
        Me.Controls.Add(Me.AddressText)
        Me.Controls.Add(Me.lblIP)
        Me.Controls.Add(Me.ObseverRadio)
        Me.Controls.Add(Me.ClientRadio)
        Me.Controls.Add(Me.ServerRadio)
        Me.Controls.Add(Me.NameText)
        Me.Controls.Add(Me.lblName)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "DirectBattleForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "私人对战"
        Me.RuleGroup.ResumeLayout(False)
        Me.RuleGroup.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents NameText As System.Windows.Forms.TextBox
    Friend WithEvents ServerRadio As System.Windows.Forms.RadioButton
    Friend WithEvents ClientRadio As System.Windows.Forms.RadioButton
    Friend WithEvents ObseverRadio As System.Windows.Forms.RadioButton
    Friend WithEvents AddressText As System.Windows.Forms.TextBox
    Friend WithEvents lblIP As System.Windows.Forms.Label
    Friend WithEvents OKButton As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents ModeCombo As System.Windows.Forms.ComboBox
    Friend WithEvents lblBattleMode As System.Windows.Forms.Label
    Friend WithEvents RuleGroup As System.Windows.Forms.GroupBox
    Friend WithEvents RandomCheck As System.Windows.Forms.CheckBox
    Friend WithEvents PPUpCheck As System.Windows.Forms.CheckBox
End Class
