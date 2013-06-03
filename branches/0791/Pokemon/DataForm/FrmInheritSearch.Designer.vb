<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmInheritSearch
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
        Me.grpResult = New System.Windows.Forms.GroupBox
        Me.lstResult = New System.Windows.Forms.ListView
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader6 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader7 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader8 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader9 = New System.Windows.Forms.ColumnHeader
        Me.grpSearchBy = New System.Windows.Forms.GroupBox
        Me.cboMove4 = New System.Windows.Forms.ComboBox
        Me.cboMove3 = New System.Windows.Forms.ComboBox
        Me.cboMove2 = New System.Windows.Forms.ComboBox
        Me.cboMove1 = New System.Windows.Forms.ComboBox
        Me.lblMove = New System.Windows.Forms.Label
        Me.cmdClose = New System.Windows.Forms.Button
        Me.cmdSearch = New System.Windows.Forms.Button
        Me.grpResult.SuspendLayout()
        Me.grpSearchBy.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpResult
        '
        Me.grpResult.Controls.Add(Me.lstResult)
        Me.grpResult.Location = New System.Drawing.Point(12, 151)
        Me.grpResult.Name = "grpResult"
        Me.grpResult.Size = New System.Drawing.Size(389, 163)
        Me.grpResult.TabIndex = 1
        Me.grpResult.TabStop = False
        Me.grpResult.Text = "搜索结果"
        '
        'lstResult
        '
        Me.lstResult.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader3, Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader4, Me.ColumnHeader5, Me.ColumnHeader6, Me.ColumnHeader7, Me.ColumnHeader8, Me.ColumnHeader9})
        Me.lstResult.FullRowSelect = True
        Me.lstResult.Location = New System.Drawing.Point(6, 20)
        Me.lstResult.MultiSelect = False
        Me.lstResult.Name = "lstResult"
        Me.lstResult.Size = New System.Drawing.Size(377, 137)
        Me.lstResult.TabIndex = 0
        Me.lstResult.UseCompatibleStateImageBehavior = False
        Me.lstResult.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "编号"
        Me.ColumnHeader3.Width = 53
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "名称"
        Me.ColumnHeader1.Width = 79
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "属性"
        Me.ColumnHeader2.Width = 70
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "HP"
        Me.ColumnHeader4.Width = 34
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "物攻"
        Me.ColumnHeader5.Width = 40
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "物防"
        Me.ColumnHeader6.Width = 40
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Text = "速度"
        Me.ColumnHeader7.Width = 40
        '
        'ColumnHeader8
        '
        Me.ColumnHeader8.Text = "特攻"
        Me.ColumnHeader8.Width = 40
        '
        'ColumnHeader9
        '
        Me.ColumnHeader9.Text = "特防"
        Me.ColumnHeader9.Width = 40
        '
        'grpSearchBy
        '
        Me.grpSearchBy.Controls.Add(Me.cboMove4)
        Me.grpSearchBy.Controls.Add(Me.cboMove3)
        Me.grpSearchBy.Controls.Add(Me.cboMove2)
        Me.grpSearchBy.Controls.Add(Me.cboMove1)
        Me.grpSearchBy.Controls.Add(Me.lblMove)
        Me.grpSearchBy.Controls.Add(Me.cmdClose)
        Me.grpSearchBy.Controls.Add(Me.cmdSearch)
        Me.grpSearchBy.Location = New System.Drawing.Point(18, 13)
        Me.grpSearchBy.Name = "grpSearchBy"
        Me.grpSearchBy.Size = New System.Drawing.Size(389, 131)
        Me.grpSearchBy.TabIndex = 0
        Me.grpSearchBy.TabStop = False
        Me.grpSearchBy.Text = "搜索项"
        '
        'cboMove4
        '
        Me.cboMove4.FormattingEnabled = True
        Me.cboMove4.Items.AddRange(New Object() {"无"})
        Me.cboMove4.Location = New System.Drawing.Point(196, 58)
        Me.cboMove4.Name = "cboMove4"
        Me.cboMove4.Size = New System.Drawing.Size(121, 20)
        Me.cboMove4.TabIndex = 3
        Me.cboMove4.Text = "无"
        '
        'cboMove3
        '
        Me.cboMove3.FormattingEnabled = True
        Me.cboMove3.Items.AddRange(New Object() {"无"})
        Me.cboMove3.Location = New System.Drawing.Point(39, 58)
        Me.cboMove3.Name = "cboMove3"
        Me.cboMove3.Size = New System.Drawing.Size(121, 20)
        Me.cboMove3.TabIndex = 1
        Me.cboMove3.Text = "无"
        '
        'cboMove2
        '
        Me.cboMove2.FormattingEnabled = True
        Me.cboMove2.Items.AddRange(New Object() {"无"})
        Me.cboMove2.Location = New System.Drawing.Point(196, 32)
        Me.cboMove2.Name = "cboMove2"
        Me.cboMove2.Size = New System.Drawing.Size(121, 20)
        Me.cboMove2.TabIndex = 2
        Me.cboMove2.Text = "无"
        '
        'cboMove1
        '
        Me.cboMove1.FormattingEnabled = True
        Me.cboMove1.Location = New System.Drawing.Point(39, 32)
        Me.cboMove1.Name = "cboMove1"
        Me.cboMove1.Size = New System.Drawing.Size(121, 20)
        Me.cboMove1.TabIndex = 0
        Me.cboMove1.Text = "无"
        '
        'lblMove
        '
        Me.lblMove.AutoSize = True
        Me.lblMove.Location = New System.Drawing.Point(37, 17)
        Me.lblMove.Name = "lblMove"
        Me.lblMove.Size = New System.Drawing.Size(53, 12)
        Me.lblMove.TabIndex = 6
        Me.lblMove.Text = "遗传技能"
        '
        'cmdClose
        '
        Me.cmdClose.Location = New System.Drawing.Point(308, 102)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(75, 23)
        Me.cmdClose.TabIndex = 5
        Me.cmdClose.Text = "关闭(&C)"
        Me.cmdClose.UseVisualStyleBackColor = True
        '
        'cmdSearch
        '
        Me.cmdSearch.Location = New System.Drawing.Point(227, 102)
        Me.cmdSearch.Name = "cmdSearch"
        Me.cmdSearch.Size = New System.Drawing.Size(75, 23)
        Me.cmdSearch.TabIndex = 4
        Me.cmdSearch.Text = "搜索(&S)"
        Me.cmdSearch.UseVisualStyleBackColor = True
        '
        'FrmInheritSearch
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(412, 326)
        Me.Controls.Add(Me.grpSearchBy)
        Me.Controls.Add(Me.grpResult)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "FrmInheritSearch"
        Me.Text = "遗传搜索"
        Me.grpResult.ResumeLayout(False)
        Me.grpSearchBy.ResumeLayout(False)
        Me.grpSearchBy.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpResult As System.Windows.Forms.GroupBox
    Friend WithEvents lstResult As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader8 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader9 As System.Windows.Forms.ColumnHeader
    Friend WithEvents grpSearchBy As System.Windows.Forms.GroupBox
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents cmdSearch As System.Windows.Forms.Button
    Friend WithEvents cboMove1 As System.Windows.Forms.ComboBox
    Friend WithEvents lblMove As System.Windows.Forms.Label
    Friend WithEvents cboMove4 As System.Windows.Forms.ComboBox
    Friend WithEvents cboMove3 As System.Windows.Forms.ComboBox
    Friend WithEvents cboMove2 As System.Windows.Forms.ComboBox
End Class
