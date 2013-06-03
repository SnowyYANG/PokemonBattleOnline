<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RoomListForm
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
        Me.EnterButton = New System.Windows.Forms.Button
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.AdvanceButton = New System.Windows.Forms.Button
        Me.InfoText = New System.Windows.Forms.TextBox
        Me.RoomList = New System.Windows.Forms.ListView
        Me.NameColumn = New System.Windows.Forms.ColumnHeader
        Me.UserColumn = New System.Windows.Forms.ColumnHeader
        Me.MaxUserColumn = New System.Windows.Forms.ColumnHeader
        Me.SuspendLayout()
        '
        'EnterButton
        '
        Me.EnterButton.Location = New System.Drawing.Point(12, 300)
        Me.EnterButton.Name = "EnterButton"
        Me.EnterButton.Size = New System.Drawing.Size(80, 33)
        Me.EnterButton.TabIndex = 2
        Me.EnterButton.Text = "进入(&E)"
        Me.EnterButton.UseVisualStyleBackColor = True
        '
        'Cancel_Button
        '
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(258, 300)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(80, 33)
        Me.Cancel_Button.TabIndex = 4
        Me.Cancel_Button.Text = "取消(&C)"
        Me.Cancel_Button.UseVisualStyleBackColor = True
        '
        'AdvanceButton
        '
        Me.AdvanceButton.Location = New System.Drawing.Point(135, 300)
        Me.AdvanceButton.Name = "AdvanceButton"
        Me.AdvanceButton.Size = New System.Drawing.Size(80, 33)
        Me.AdvanceButton.TabIndex = 5
        Me.AdvanceButton.Text = "高级(&A)"
        Me.AdvanceButton.UseVisualStyleBackColor = True
        '
        'InfoText
        '
        Me.InfoText.Dock = System.Windows.Forms.DockStyle.Top
        Me.InfoText.Location = New System.Drawing.Point(0, 249)
        Me.InfoText.Multiline = True
        Me.InfoText.Name = "InfoText"
        Me.InfoText.ReadOnly = True
        Me.InfoText.Size = New System.Drawing.Size(350, 45)
        Me.InfoText.TabIndex = 7
        '
        'RoomList
        '
        Me.RoomList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.NameColumn, Me.UserColumn, Me.MaxUserColumn})
        Me.RoomList.Dock = System.Windows.Forms.DockStyle.Top
        Me.RoomList.FullRowSelect = True
        Me.RoomList.Location = New System.Drawing.Point(0, 0)
        Me.RoomList.MultiSelect = False
        Me.RoomList.Name = "RoomList"
        Me.RoomList.Size = New System.Drawing.Size(350, 249)
        Me.RoomList.TabIndex = 6
        Me.RoomList.UseCompatibleStateImageBehavior = False
        Me.RoomList.View = System.Windows.Forms.View.Details
        '
        'NameColumn
        '
        Me.NameColumn.Text = "房间名称"
        Me.NameColumn.Width = 192
        '
        'UserColumn
        '
        Me.UserColumn.Text = "在线人数"
        Me.UserColumn.Width = 75
        '
        'MaxUserColumn
        '
        Me.MaxUserColumn.Text = "最大人数"
        Me.MaxUserColumn.Width = 75
        '
        'RoomListForm
        '
        Me.AcceptButton = Me.EnterButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(350, 344)
        Me.Controls.Add(Me.InfoText)
        Me.Controls.Add(Me.RoomList)
        Me.Controls.Add(Me.AdvanceButton)
        Me.Controls.Add(Me.Cancel_Button)
        Me.Controls.Add(Me.EnterButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "RoomListForm"
        Me.ShowInTaskbar = False
        Me.Text = "房间列表"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents EnterButton As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents AdvanceButton As System.Windows.Forms.Button
    Friend WithEvents InfoText As System.Windows.Forms.TextBox
    Friend WithEvents RoomList As System.Windows.Forms.ListView
    Friend WithEvents NameColumn As System.Windows.Forms.ColumnHeader
    Friend WithEvents UserColumn As System.Windows.Forms.ColumnHeader
    Friend WithEvents MaxUserColumn As System.Windows.Forms.ColumnHeader
End Class
