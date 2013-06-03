<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmSearchPM
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
        Me.grpSearchBy = New System.Windows.Forms.GroupBox
        Me.cmdClose = New System.Windows.Forms.Button
        Me.cboType2 = New System.Windows.Forms.ComboBox
        Me.chkType2 = New System.Windows.Forms.CheckBox
        Me.cmdSearch = New System.Windows.Forms.Button
        Me.lstMove = New System.Windows.Forms.ListView
        Me.chkMove = New System.Windows.Forms.CheckBox
        Me.cboTrait = New System.Windows.Forms.ComboBox
        Me.chkTrait = New System.Windows.Forms.CheckBox
        Me.cboType = New System.Windows.Forms.ComboBox
        Me.chkType = New System.Windows.Forms.CheckBox
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
        Me.grpSearchBy.SuspendLayout()
        Me.grpResult.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpSearchBy
        '
        Me.grpSearchBy.Controls.Add(Me.cmdClose)
        Me.grpSearchBy.Controls.Add(Me.cboType2)
        Me.grpSearchBy.Controls.Add(Me.chkType2)
        Me.grpSearchBy.Controls.Add(Me.cmdSearch)
        Me.grpSearchBy.Controls.Add(Me.lstMove)
        Me.grpSearchBy.Controls.Add(Me.chkMove)
        Me.grpSearchBy.Controls.Add(Me.cboTrait)
        Me.grpSearchBy.Controls.Add(Me.chkTrait)
        Me.grpSearchBy.Controls.Add(Me.cboType)
        Me.grpSearchBy.Controls.Add(Me.chkType)
        Me.grpSearchBy.Location = New System.Drawing.Point(11, 12)
        Me.grpSearchBy.Name = "grpSearchBy"
        Me.grpSearchBy.Size = New System.Drawing.Size(389, 192)
        Me.grpSearchBy.TabIndex = 0
        Me.grpSearchBy.TabStop = False
        Me.grpSearchBy.Text = "搜索项"
        '
        'cmdClose
        '
        Me.cmdClose.Location = New System.Drawing.Point(308, 158)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(75, 23)
        Me.cmdClose.TabIndex = 9
        Me.cmdClose.Text = "关闭(&C)"
        Me.cmdClose.UseVisualStyleBackColor = True
        '
        'cboType2
        '
        Me.cboType2.FormattingEnabled = True
        Me.cboType2.Location = New System.Drawing.Point(23, 92)
        Me.cboType2.Name = "cboType2"
        Me.cboType2.Size = New System.Drawing.Size(87, 20)
        Me.cboType2.TabIndex = 3
        '
        'chkType2
        '
        Me.chkType2.AutoSize = True
        Me.chkType2.Location = New System.Drawing.Point(23, 70)
        Me.chkType2.Name = "chkType2"
        Me.chkType2.Size = New System.Drawing.Size(72, 16)
        Me.chkType2.TabIndex = 2
        Me.chkType2.Text = "第二属性"
        Me.chkType2.UseVisualStyleBackColor = True
        '
        'cmdSearch
        '
        Me.cmdSearch.Enabled = False
        Me.cmdSearch.Location = New System.Drawing.Point(308, 118)
        Me.cmdSearch.Name = "cmdSearch"
        Me.cmdSearch.Size = New System.Drawing.Size(75, 23)
        Me.cmdSearch.TabIndex = 8
        Me.cmdSearch.Text = "搜索(&S)"
        Me.cmdSearch.UseVisualStyleBackColor = True
        '
        'lstMove
        '
        Me.lstMove.CheckBoxes = True
        Me.lstMove.Location = New System.Drawing.Point(145, 42)
        Me.lstMove.Name = "lstMove"
        Me.lstMove.Size = New System.Drawing.Size(142, 144)
        Me.lstMove.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lstMove.TabIndex = 7
        Me.lstMove.UseCompatibleStateImageBehavior = False
        Me.lstMove.View = System.Windows.Forms.View.SmallIcon
        '
        'chkMove
        '
        Me.chkMove.AutoSize = True
        Me.chkMove.Location = New System.Drawing.Point(145, 20)
        Me.chkMove.Name = "chkMove"
        Me.chkMove.Size = New System.Drawing.Size(48, 16)
        Me.chkMove.TabIndex = 6
        Me.chkMove.Text = "技能"
        Me.chkMove.UseVisualStyleBackColor = True
        '
        'cboTrait
        '
        Me.cboTrait.FormattingEnabled = True
        Me.cboTrait.Items.AddRange(New Object() {"恶臭", "逃跑能手", "发光", "蜜蜂", "捡东西", "降雨", "日照", "气象锁", "沙尘暴", "无天气", "暴雪", "冰之躯体", "接雨盘", "润湿身体", "干燥皮肤", "太阳能", "轻快", "沙隐术", "气象台", "花朵礼物", "雪遁", "叶绿素", "叶子守护", "自然恢复", "柔软", "精神力", "蜕皮", "迟钝", "自我中心", "早起", "活跃", "不眠", "熔岩盔甲", "水幕", "免疫", "石头脑袋", "坚硬", "化石盔甲", "吸盘", "贝壳装甲", "湿气", "隔音", "魔法守护", "蓄水", "蓄电", "电力引擎", "避雷针", "浮游", "属性之盾", "引火", "吸水", "厚脂肪", "耐热", "坚硬岩石", "过滤器", "威吓", "怪力钳", "加速", "净体", "白烟", "锐利眼光", "单纯", "天然", "有色眼镜", "走钢丝", "幸运", "狙击手", "技巧连接", "舍身", "下载", "强有力 ", "铁拳", "适应力", "技师", "好斗", "不要防守", "复眼", "瑜伽威力 ", "气魄", "普通皮肤", "变色", "天之恩赐", "鳞粉", "愤怒神经", "上进", "同步率", "千鸟足", "快步走", "不屈之心", "神奇鳞片", "毒疗", "鲨鱼皮", "静电", "毒针", "淤泥", "孢子", "火焰之身", "魅惑之身", "引爆", "激流", "发芽", "烈火", "虫族报告", "活地狱", "踩影子", "磁力", "早食", "粘着", "破格", "复制", "洞悉心灵", "预知危险", "精神梦境", "后手打击", "缓慢启动", "偷懒", "紧张", "不用武器", "压力", "正极 ", "负极 ", "噩梦", "多重属性"})
        Me.cboTrait.Location = New System.Drawing.Point(23, 140)
        Me.cboTrait.Name = "cboTrait"
        Me.cboTrait.Size = New System.Drawing.Size(87, 20)
        Me.cboTrait.TabIndex = 5
        '
        'chkTrait
        '
        Me.chkTrait.AutoSize = True
        Me.chkTrait.Location = New System.Drawing.Point(23, 118)
        Me.chkTrait.Name = "chkTrait"
        Me.chkTrait.Size = New System.Drawing.Size(48, 16)
        Me.chkTrait.TabIndex = 4
        Me.chkTrait.Text = "特性"
        Me.chkTrait.UseVisualStyleBackColor = True
        '
        'cboType
        '
        Me.cboType.FormattingEnabled = True
        Me.cboType.Location = New System.Drawing.Point(23, 42)
        Me.cboType.Name = "cboType"
        Me.cboType.Size = New System.Drawing.Size(87, 20)
        Me.cboType.TabIndex = 1
        '
        'chkType
        '
        Me.chkType.AutoSize = True
        Me.chkType.Location = New System.Drawing.Point(23, 20)
        Me.chkType.Name = "chkType"
        Me.chkType.Size = New System.Drawing.Size(48, 16)
        Me.chkType.TabIndex = 0
        Me.chkType.Text = "属性"
        Me.chkType.UseVisualStyleBackColor = True
        '
        'grpResult
        '
        Me.grpResult.Controls.Add(Me.lstResult)
        Me.grpResult.Location = New System.Drawing.Point(11, 210)
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
        'FrmSearchPM
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(412, 386)
        Me.Controls.Add(Me.grpResult)
        Me.Controls.Add(Me.grpSearchBy)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "FrmSearchPM"
        Me.Text = "搜索精灵"
        Me.grpSearchBy.ResumeLayout(False)
        Me.grpSearchBy.PerformLayout()
        Me.grpResult.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpSearchBy As System.Windows.Forms.GroupBox
    Friend WithEvents grpResult As System.Windows.Forms.GroupBox
    Friend WithEvents cboTrait As System.Windows.Forms.ComboBox
    Friend WithEvents chkTrait As System.Windows.Forms.CheckBox
    Friend WithEvents cboType As System.Windows.Forms.ComboBox
    Friend WithEvents chkType As System.Windows.Forms.CheckBox
    Friend WithEvents chkMove As System.Windows.Forms.CheckBox
    Friend WithEvents lstMove As System.Windows.Forms.ListView
    Friend WithEvents cboType2 As System.Windows.Forms.ComboBox
    Friend WithEvents chkType2 As System.Windows.Forms.CheckBox
    Friend WithEvents cmdSearch As System.Windows.Forms.Button
    Friend WithEvents lstResult As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader8 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader9 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents cmdClose As System.Windows.Forms.Button
End Class
