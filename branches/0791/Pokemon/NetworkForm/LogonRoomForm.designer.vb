<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LogonRoomForm
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(LogonRoomForm))
        Me.NameText = New System.Windows.Forms.TextBox()
        Me.UsernameLabel = New System.Windows.Forms.Label()
        Me.QuitButton = New System.Windows.Forms.Button()
        Me.LogonButton = New System.Windows.Forms.Button()
        Me.ImageUpnButton = New System.Windows.Forms.Button()
        Me.ImageDownButton = New System.Windows.Forms.Button()
        Me.MyImage = New System.Windows.Forms.PictureBox()
        Me.UserImages = New System.Windows.Forms.ImageList(Me.components)
        Me.AutoLogonCheck = New System.Windows.Forms.CheckBox()
        Me.AdvanceLinkButton = New System.Windows.Forms.CheckBox()
        CType(Me.MyImage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'NameText
        '
        Me.NameText.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.PokemonBattleOnline.My.MySettings.Default, "UserName", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.NameText.ImeMode = System.Windows.Forms.ImeMode.Alpha
        Me.NameText.Location = New System.Drawing.Point(138, 48)
        Me.NameText.MaxLength = 16
        Me.NameText.Name = "NameText"
        Me.NameText.Size = New System.Drawing.Size(116, 21)
        Me.NameText.TabIndex = 0
        Me.NameText.Text = Global.PokemonBattleOnline.My.MySettings.Default.UserName
        '
        'UsernameLabel
        '
        Me.UsernameLabel.Location = New System.Drawing.Point(136, 28)
        Me.UsernameLabel.Name = "UsernameLabel"
        Me.UsernameLabel.Size = New System.Drawing.Size(118, 23)
        Me.UsernameLabel.TabIndex = 7
        Me.UsernameLabel.Text = "用户名"
        Me.UsernameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'QuitButton
        '
        Me.QuitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.QuitButton.Location = New System.Drawing.Point(179, 111)
        Me.QuitButton.Name = "QuitButton"
        Me.QuitButton.Size = New System.Drawing.Size(75, 27)
        Me.QuitButton.TabIndex = 2
        Me.QuitButton.Text = "取消(&C)"
        '
        'LogonButton
        '
        Me.LogonButton.Location = New System.Drawing.Point(16, 111)
        Me.LogonButton.Name = "LogonButton"
        Me.LogonButton.Size = New System.Drawing.Size(75, 27)
        Me.LogonButton.TabIndex = 1
        Me.LogonButton.Text = "登录(&L)"
        '
        'ImageUpnButton
        '
        Me.ImageUpnButton.FlatAppearance.BorderColor = System.Drawing.Color.White
        Me.ImageUpnButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSkyBlue
        Me.ImageUpnButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SkyBlue
        Me.ImageUpnButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ImageUpnButton.Font = New System.Drawing.Font("宋体", 7.0!, System.Drawing.FontStyle.Bold)
        Me.ImageUpnButton.Location = New System.Drawing.Point(85, 47)
        Me.ImageUpnButton.Name = "ImageUpnButton"
        Me.ImageUpnButton.Size = New System.Drawing.Size(20, 20)
        Me.ImageUpnButton.TabIndex = 4
        Me.ImageUpnButton.Text = ">"
        Me.ImageUpnButton.UseVisualStyleBackColor = True
        '
        'ImageDownButton
        '
        Me.ImageDownButton.FlatAppearance.BorderColor = System.Drawing.Color.White
        Me.ImageDownButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSkyBlue
        Me.ImageDownButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SkyBlue
        Me.ImageDownButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ImageDownButton.Font = New System.Drawing.Font("宋体", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.ImageDownButton.Location = New System.Drawing.Point(21, 47)
        Me.ImageDownButton.Name = "ImageDownButton"
        Me.ImageDownButton.Size = New System.Drawing.Size(20, 20)
        Me.ImageDownButton.TabIndex = 3
        Me.ImageDownButton.Text = "<"
        Me.ImageDownButton.UseVisualStyleBackColor = True
        '
        'MyImage
        '
        Me.MyImage.Location = New System.Drawing.Point(47, 36)
        Me.MyImage.Name = "MyImage"
        Me.MyImage.Size = New System.Drawing.Size(32, 32)
        Me.MyImage.TabIndex = 8
        Me.MyImage.TabStop = False
        '
        'UserImages
        '
        Me.UserImages.ImageStream = CType(resources.GetObject("UserImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.UserImages.TransparentColor = System.Drawing.Color.Transparent
        Me.UserImages.Images.SetKeyName(0, "Pokemon1.ico")
        Me.UserImages.Images.SetKeyName(1, "Pokemon2.ico")
        Me.UserImages.Images.SetKeyName(2, "Pokemon3.ico")
        Me.UserImages.Images.SetKeyName(3, "Pokemon4.ico")
        Me.UserImages.Images.SetKeyName(4, "Pokemon5.ico")
        Me.UserImages.Images.SetKeyName(5, "Pokemon6.ico")
        Me.UserImages.Images.SetKeyName(6, "Pokemon7.ico")
        Me.UserImages.Images.SetKeyName(7, "Pokemon8.ico")
        Me.UserImages.Images.SetKeyName(8, "Pokemon9.ico")
        Me.UserImages.Images.SetKeyName(9, "Pokemon10.ico")
        Me.UserImages.Images.SetKeyName(10, "Pokemon11.ico")
        Me.UserImages.Images.SetKeyName(11, "Pokemon12.ico")
        Me.UserImages.Images.SetKeyName(12, "Pokemon13.ico")
        Me.UserImages.Images.SetKeyName(13, "Pokemon14.ico")
        Me.UserImages.Images.SetKeyName(14, "Pokemon15.ico")
        '
        'AutoLogonCheck
        '
        Me.AutoLogonCheck.AutoSize = True
        Me.AutoLogonCheck.Checked = Global.PokemonBattleOnline.My.MySettings.Default.AutoLoginUser
        Me.AutoLogonCheck.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Global.PokemonBattleOnline.My.MySettings.Default, "AutoLoginUser", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.AutoLogonCheck.Location = New System.Drawing.Point(16, 88)
        Me.AutoLogonCheck.Name = "AutoLogonCheck"
        Me.AutoLogonCheck.Size = New System.Drawing.Size(96, 16)
        Me.AutoLogonCheck.TabIndex = 5
        Me.AutoLogonCheck.Text = "下次自动登录"
        Me.AutoLogonCheck.UseVisualStyleBackColor = True
        '
        'AdvanceLinkButton
        '
        Me.AdvanceLinkButton.AutoSize = True
        Me.AdvanceLinkButton.Location = New System.Drawing.Point(16, 144)
        Me.AdvanceLinkButton.Name = "AdvanceLinkButton"
        Me.AdvanceLinkButton.Size = New System.Drawing.Size(72, 16)
        Me.AdvanceLinkButton.TabIndex = 6
        Me.AdvanceLinkButton.Text = "高级连接"
        Me.AdvanceLinkButton.UseVisualStyleBackColor = True
        '
        'LogonRoomForm
        '
        Me.AcceptButton = Me.LogonButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.QuitButton
        Me.ClientSize = New System.Drawing.Size(269, 164)
        Me.Controls.Add(Me.AdvanceLinkButton)
        Me.Controls.Add(Me.AutoLogonCheck)
        Me.Controls.Add(Me.ImageUpnButton)
        Me.Controls.Add(Me.ImageDownButton)
        Me.Controls.Add(Me.MyImage)
        Me.Controls.Add(Me.QuitButton)
        Me.Controls.Add(Me.LogonButton)
        Me.Controls.Add(Me.NameText)
        Me.Controls.Add(Me.UsernameLabel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "LogonRoomForm"
        Me.ShowInTaskbar = False
        Me.Text = "登录房间列表"
        CType(Me.MyImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents NameText As System.Windows.Forms.TextBox
    Friend WithEvents UsernameLabel As System.Windows.Forms.Label
    Friend WithEvents QuitButton As System.Windows.Forms.Button
    Friend WithEvents LogonButton As System.Windows.Forms.Button
    Friend WithEvents ImageUpnButton As System.Windows.Forms.Button
    Friend WithEvents ImageDownButton As System.Windows.Forms.Button
    Friend WithEvents MyImage As System.Windows.Forms.PictureBox
    Friend WithEvents UserImages As System.Windows.Forms.ImageList
    Friend WithEvents AutoLogonCheck As System.Windows.Forms.CheckBox
    Friend WithEvents AdvanceLinkButton As System.Windows.Forms.CheckBox
End Class
