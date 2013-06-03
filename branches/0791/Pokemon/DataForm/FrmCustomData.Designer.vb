<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmCustomData
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmCustomData))
        Me.tabData = New System.Windows.Forms.TabControl
        Me.tabAdd = New System.Windows.Forms.TabPage
        Me.lstNewPM = New System.Windows.Forms.ListBox
        Me.grpPM = New System.Windows.Forms.GroupBox
        Me.btnRemoveMove = New System.Windows.Forms.Button
        Me.btnAddMove = New System.Windows.Forms.Button
        Me.lblMoves = New System.Windows.Forms.Label
        Me.lstMoves = New System.Windows.Forms.ListBox
        Me.txtNumber = New System.Windows.Forms.TextBox
        Me.nudWeight = New System.Windows.Forms.NumericUpDown
        Me.nudSpeed = New System.Windows.Forms.NumericUpDown
        Me.nudSDef = New System.Windows.Forms.NumericUpDown
        Me.nudSAtk = New System.Windows.Forms.NumericUpDown
        Me.nudDef = New System.Windows.Forms.NumericUpDown
        Me.nudAtk = New System.Windows.Forms.NumericUpDown
        Me.nudHP = New System.Windows.Forms.NumericUpDown
        Me.cboEg2 = New System.Windows.Forms.ComboBox
        Me.lblEg2 = New System.Windows.Forms.Label
        Me.cboEg1 = New System.Windows.Forms.ComboBox
        Me.lblEg1 = New System.Windows.Forms.Label
        Me.lblWeight = New System.Windows.Forms.Label
        Me.cboGenderRS = New System.Windows.Forms.ComboBox
        Me.lblGenderRS = New System.Windows.Forms.Label
        Me.lblNO = New System.Windows.Forms.Label
        Me.btnImage = New System.Windows.Forms.Button
        Me.cboTrait2 = New System.Windows.Forms.ComboBox
        Me.lblTrait2 = New System.Windows.Forms.Label
        Me.cboTrait1 = New System.Windows.Forms.ComboBox
        Me.lblTrait1 = New System.Windows.Forms.Label
        Me.lblSpeed = New System.Windows.Forms.Label
        Me.lblSDef = New System.Windows.Forms.Label
        Me.lblSAtk = New System.Windows.Forms.Label
        Me.lblDef = New System.Windows.Forms.Label
        Me.lblAtk = New System.Windows.Forms.Label
        Me.lblHP = New System.Windows.Forms.Label
        Me.cboType2 = New System.Windows.Forms.ComboBox
        Me.lblType2 = New System.Windows.Forms.Label
        Me.cboType1 = New System.Windows.Forms.ComboBox
        Me.lblType1 = New System.Windows.Forms.Label
        Me.lblName = New System.Windows.Forms.Label
        Me.txtName = New System.Windows.Forms.TextBox
        Me.btnDelNew = New System.Windows.Forms.Button
        Me.btnAddNew = New System.Windows.Forms.Button
        Me.tabUpdate = New System.Windows.Forms.TabPage
        Me.lstUpdate = New System.Windows.Forms.ListBox
        Me.grpUpdate = New System.Windows.Forms.GroupBox
        Me.lblInfo = New System.Windows.Forms.Label
        Me.nudNewWeight = New System.Windows.Forms.NumericUpDown
        Me.nudNewSpeed = New System.Windows.Forms.NumericUpDown
        Me.nudNewSDef = New System.Windows.Forms.NumericUpDown
        Me.nudNewSAtk = New System.Windows.Forms.NumericUpDown
        Me.nudNewDef = New System.Windows.Forms.NumericUpDown
        Me.nudNewAtk = New System.Windows.Forms.NumericUpDown
        Me.nudNewHP = New System.Windows.Forms.NumericUpDown
        Me.btnDelRM = New System.Windows.Forms.Button
        Me.btnAddRM = New System.Windows.Forms.Button
        Me.btnDelAM = New System.Windows.Forms.Button
        Me.btnAddAM = New System.Windows.Forms.Button
        Me.lblRemoveMove = New System.Windows.Forms.Label
        Me.lblAddMove = New System.Windows.Forms.Label
        Me.lstRemoveMove = New System.Windows.Forms.ListBox
        Me.lstAddMove = New System.Windows.Forms.ListBox
        Me.lblUpdateWeight = New System.Windows.Forms.Label
        Me.cboNewTrait2 = New System.Windows.Forms.ComboBox
        Me.lblUpdateTrait2 = New System.Windows.Forms.Label
        Me.cboNewTrait1 = New System.Windows.Forms.ComboBox
        Me.lblUpdateTrait1 = New System.Windows.Forms.Label
        Me.lblUpdateSpeed = New System.Windows.Forms.Label
        Me.lblUpdateSDef = New System.Windows.Forms.Label
        Me.lblUpdateSAtk = New System.Windows.Forms.Label
        Me.lblUpdateDef = New System.Windows.Forms.Label
        Me.lblUpdateAtk = New System.Windows.Forms.Label
        Me.lblUpdateHP = New System.Windows.Forms.Label
        Me.cboNewType2 = New System.Windows.Forms.ComboBox
        Me.lblUpdateType2 = New System.Windows.Forms.Label
        Me.cboNewType1 = New System.Windows.Forms.ComboBox
        Me.lblUpdateType1 = New System.Windows.Forms.Label
        Me.lblNameBase = New System.Windows.Forms.Label
        Me.txtNameBase = New System.Windows.Forms.TextBox
        Me.btnDelUpdate = New System.Windows.Forms.Button
        Me.btnAddUpdate = New System.Windows.Forms.Button
        Me.tabRemove = New System.Windows.Forms.TabPage
        Me.btnDelRemove = New System.Windows.Forms.Button
        Me.btnAddRemove = New System.Windows.Forms.Button
        Me.lstRemovePM = New System.Windows.Forms.ListBox
        Me.tabImage = New System.Windows.Forms.TabPage
        Me.btnDelImg = New System.Windows.Forms.Button
        Me.btnAddImg = New System.Windows.Forms.Button
        Me.lstImages = New System.Windows.Forms.ListView
        Me.ImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.mnuCustom = New System.Windows.Forms.MenuStrip
        Me.项目FToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.新建NToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.打开OToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.toolStripSeparator = New System.Windows.Forms.ToolStripSeparator
        Me.保存SToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.另存为AToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.生成数据BToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.toolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.退出XToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.更改项目名RToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.tabData.SuspendLayout()
        Me.tabAdd.SuspendLayout()
        Me.grpPM.SuspendLayout()
        CType(Me.nudWeight, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudSpeed, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudSDef, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudSAtk, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudDef, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudAtk, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudHP, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabUpdate.SuspendLayout()
        Me.grpUpdate.SuspendLayout()
        CType(Me.nudNewWeight, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudNewSpeed, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudNewSDef, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudNewSAtk, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudNewDef, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudNewAtk, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudNewHP, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabRemove.SuspendLayout()
        Me.tabImage.SuspendLayout()
        Me.mnuCustom.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabData
        '
        Me.tabData.Controls.Add(Me.tabAdd)
        Me.tabData.Controls.Add(Me.tabUpdate)
        Me.tabData.Controls.Add(Me.tabRemove)
        Me.tabData.Controls.Add(Me.tabImage)
        Me.tabData.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.tabData.Location = New System.Drawing.Point(0, 26)
        Me.tabData.Name = "tabData"
        Me.tabData.SelectedIndex = 0
        Me.tabData.Size = New System.Drawing.Size(636, 446)
        Me.tabData.TabIndex = 0
        '
        'tabAdd
        '
        Me.tabAdd.Controls.Add(Me.lstNewPM)
        Me.tabAdd.Controls.Add(Me.grpPM)
        Me.tabAdd.Controls.Add(Me.btnDelNew)
        Me.tabAdd.Controls.Add(Me.btnAddNew)
        Me.tabAdd.Location = New System.Drawing.Point(4, 21)
        Me.tabAdd.Name = "tabAdd"
        Me.tabAdd.Padding = New System.Windows.Forms.Padding(3)
        Me.tabAdd.Size = New System.Drawing.Size(628, 421)
        Me.tabAdd.TabIndex = 0
        Me.tabAdd.Text = "添加精灵"
        Me.tabAdd.UseVisualStyleBackColor = True
        '
        'lstNewPM
        '
        Me.lstNewPM.FormattingEnabled = True
        Me.lstNewPM.ItemHeight = 12
        Me.lstNewPM.Location = New System.Drawing.Point(9, 8)
        Me.lstNewPM.Name = "lstNewPM"
        Me.lstNewPM.Size = New System.Drawing.Size(120, 400)
        Me.lstNewPM.TabIndex = 62
        '
        'grpPM
        '
        Me.grpPM.Controls.Add(Me.btnRemoveMove)
        Me.grpPM.Controls.Add(Me.btnAddMove)
        Me.grpPM.Controls.Add(Me.lblMoves)
        Me.grpPM.Controls.Add(Me.lstMoves)
        Me.grpPM.Controls.Add(Me.txtNumber)
        Me.grpPM.Controls.Add(Me.nudWeight)
        Me.grpPM.Controls.Add(Me.nudSpeed)
        Me.grpPM.Controls.Add(Me.nudSDef)
        Me.grpPM.Controls.Add(Me.nudSAtk)
        Me.grpPM.Controls.Add(Me.nudDef)
        Me.grpPM.Controls.Add(Me.nudAtk)
        Me.grpPM.Controls.Add(Me.nudHP)
        Me.grpPM.Controls.Add(Me.cboEg2)
        Me.grpPM.Controls.Add(Me.lblEg2)
        Me.grpPM.Controls.Add(Me.cboEg1)
        Me.grpPM.Controls.Add(Me.lblEg1)
        Me.grpPM.Controls.Add(Me.lblWeight)
        Me.grpPM.Controls.Add(Me.cboGenderRS)
        Me.grpPM.Controls.Add(Me.lblGenderRS)
        Me.grpPM.Controls.Add(Me.lblNO)
        Me.grpPM.Controls.Add(Me.btnImage)
        Me.grpPM.Controls.Add(Me.cboTrait2)
        Me.grpPM.Controls.Add(Me.lblTrait2)
        Me.grpPM.Controls.Add(Me.cboTrait1)
        Me.grpPM.Controls.Add(Me.lblTrait1)
        Me.grpPM.Controls.Add(Me.lblSpeed)
        Me.grpPM.Controls.Add(Me.lblSDef)
        Me.grpPM.Controls.Add(Me.lblSAtk)
        Me.grpPM.Controls.Add(Me.lblDef)
        Me.grpPM.Controls.Add(Me.lblAtk)
        Me.grpPM.Controls.Add(Me.lblHP)
        Me.grpPM.Controls.Add(Me.cboType2)
        Me.grpPM.Controls.Add(Me.lblType2)
        Me.grpPM.Controls.Add(Me.cboType1)
        Me.grpPM.Controls.Add(Me.lblType1)
        Me.grpPM.Controls.Add(Me.lblName)
        Me.grpPM.Controls.Add(Me.txtName)
        Me.grpPM.Location = New System.Drawing.Point(193, 8)
        Me.grpPM.Name = "grpPM"
        Me.grpPM.Size = New System.Drawing.Size(427, 407)
        Me.grpPM.TabIndex = 9
        Me.grpPM.TabStop = False
        Me.grpPM.Text = "PM"
        '
        'btnRemoveMove
        '
        Me.btnRemoveMove.Enabled = False
        Me.btnRemoveMove.Location = New System.Drawing.Point(359, 377)
        Me.btnRemoveMove.Name = "btnRemoveMove"
        Me.btnRemoveMove.Size = New System.Drawing.Size(52, 23)
        Me.btnRemoveMove.TabIndex = 72
        Me.btnRemoveMove.Text = "删除"
        Me.btnRemoveMove.UseVisualStyleBackColor = True
        '
        'btnAddMove
        '
        Me.btnAddMove.Enabled = False
        Me.btnAddMove.Location = New System.Drawing.Point(290, 377)
        Me.btnAddMove.Name = "btnAddMove"
        Me.btnAddMove.Size = New System.Drawing.Size(52, 23)
        Me.btnAddMove.TabIndex = 71
        Me.btnAddMove.Text = "添加"
        Me.btnAddMove.UseVisualStyleBackColor = True
        '
        'lblMoves
        '
        Me.lblMoves.AutoSize = True
        Me.lblMoves.Font = New System.Drawing.Font("宋体", 9.0!)
        Me.lblMoves.Location = New System.Drawing.Point(288, 17)
        Me.lblMoves.Name = "lblMoves"
        Me.lblMoves.Size = New System.Drawing.Size(53, 12)
        Me.lblMoves.TabIndex = 70
        Me.lblMoves.Text = "拥有技能"
        Me.lblMoves.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lstMoves
        '
        Me.lstMoves.FormattingEnabled = True
        Me.lstMoves.ItemHeight = 12
        Me.lstMoves.Location = New System.Drawing.Point(290, 32)
        Me.lstMoves.Name = "lstMoves"
        Me.lstMoves.Size = New System.Drawing.Size(120, 340)
        Me.lstMoves.TabIndex = 69
        '
        'txtNumber
        '
        Me.txtNumber.Location = New System.Drawing.Point(51, 379)
        Me.txtNumber.MaxLength = 5
        Me.txtNumber.Name = "txtNumber"
        Me.txtNumber.ReadOnly = True
        Me.txtNumber.Size = New System.Drawing.Size(41, 21)
        Me.txtNumber.TabIndex = 68
        '
        'nudWeight
        '
        Me.nudWeight.DecimalPlaces = 2
        Me.nudWeight.Enabled = False
        Me.nudWeight.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.nudWeight.Location = New System.Drawing.Point(57, 189)
        Me.nudWeight.Maximum = New Decimal(New Integer() {999, 0, 0, 0})
        Me.nudWeight.Name = "nudWeight"
        Me.nudWeight.Size = New System.Drawing.Size(78, 21)
        Me.nudWeight.TabIndex = 67
        '
        'nudSpeed
        '
        Me.nudSpeed.Enabled = False
        Me.nudSpeed.Location = New System.Drawing.Point(216, 189)
        Me.nudSpeed.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me.nudSpeed.Name = "nudSpeed"
        Me.nudSpeed.Size = New System.Drawing.Size(62, 21)
        Me.nudSpeed.TabIndex = 66
        '
        'nudSDef
        '
        Me.nudSDef.Enabled = False
        Me.nudSDef.Location = New System.Drawing.Point(216, 156)
        Me.nudSDef.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me.nudSDef.Name = "nudSDef"
        Me.nudSDef.Size = New System.Drawing.Size(62, 21)
        Me.nudSDef.TabIndex = 65
        '
        'nudSAtk
        '
        Me.nudSAtk.Enabled = False
        Me.nudSAtk.Location = New System.Drawing.Point(216, 123)
        Me.nudSAtk.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me.nudSAtk.Name = "nudSAtk"
        Me.nudSAtk.Size = New System.Drawing.Size(62, 21)
        Me.nudSAtk.TabIndex = 64
        '
        'nudDef
        '
        Me.nudDef.Enabled = False
        Me.nudDef.Location = New System.Drawing.Point(216, 90)
        Me.nudDef.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me.nudDef.Name = "nudDef"
        Me.nudDef.Size = New System.Drawing.Size(62, 21)
        Me.nudDef.TabIndex = 63
        '
        'nudAtk
        '
        Me.nudAtk.Enabled = False
        Me.nudAtk.Location = New System.Drawing.Point(216, 57)
        Me.nudAtk.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me.nudAtk.Name = "nudAtk"
        Me.nudAtk.Size = New System.Drawing.Size(62, 21)
        Me.nudAtk.TabIndex = 62
        '
        'nudHP
        '
        Me.nudHP.Enabled = False
        Me.nudHP.Location = New System.Drawing.Point(216, 24)
        Me.nudHP.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me.nudHP.Name = "nudHP"
        Me.nudHP.Size = New System.Drawing.Size(62, 21)
        Me.nudHP.TabIndex = 61
        '
        'cboEg2
        '
        Me.cboEg2.Enabled = False
        Me.cboEg2.FormattingEnabled = True
        Me.cboEg2.Items.AddRange(New Object() {"无", "人型", "植物", "怪兽", "龙", "水中1", "水中2", "水中3", "虫", "飞行", "陆上", "妖精", "矿物", "不定形", "百变怪", "未发现"})
        Me.cboEg2.Location = New System.Drawing.Point(81, 246)
        Me.cboEg2.Name = "cboEg2"
        Me.cboEg2.Size = New System.Drawing.Size(78, 20)
        Me.cboEg2.TabIndex = 56
        '
        'lblEg2
        '
        Me.lblEg2.AutoSize = True
        Me.lblEg2.Font = New System.Drawing.Font("宋体", 9.0!)
        Me.lblEg2.Location = New System.Drawing.Point(16, 249)
        Me.lblEg2.Name = "lblEg2"
        Me.lblEg2.Size = New System.Drawing.Size(59, 12)
        Me.lblEg2.TabIndex = 55
        Me.lblEg2.Text = "生蛋组别2"
        Me.lblEg2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cboEg1
        '
        Me.cboEg1.Enabled = False
        Me.cboEg1.FormattingEnabled = True
        Me.cboEg1.Items.AddRange(New Object() {"人型", "植物", "怪兽", "龙", "水中1", "水中2", "水中3", "虫", "飞行", "陆上", "妖精", "矿物", "不定形", "百变怪", "未发现"})
        Me.cboEg1.Location = New System.Drawing.Point(81, 220)
        Me.cboEg1.Name = "cboEg1"
        Me.cboEg1.Size = New System.Drawing.Size(78, 20)
        Me.cboEg1.TabIndex = 54
        '
        'lblEg1
        '
        Me.lblEg1.AutoSize = True
        Me.lblEg1.Font = New System.Drawing.Font("宋体", 9.0!)
        Me.lblEg1.Location = New System.Drawing.Point(16, 223)
        Me.lblEg1.Name = "lblEg1"
        Me.lblEg1.Size = New System.Drawing.Size(59, 12)
        Me.lblEg1.TabIndex = 53
        Me.lblEg1.Text = "生蛋组别1"
        Me.lblEg1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblWeight
        '
        Me.lblWeight.AutoSize = True
        Me.lblWeight.Font = New System.Drawing.Font("宋体", 9.0!)
        Me.lblWeight.Location = New System.Drawing.Point(16, 192)
        Me.lblWeight.Name = "lblWeight"
        Me.lblWeight.Size = New System.Drawing.Size(29, 12)
        Me.lblWeight.TabIndex = 51
        Me.lblWeight.Text = "体重"
        Me.lblWeight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cboGenderRS
        '
        Me.cboGenderRS.Enabled = False
        Me.cboGenderRS.FormattingEnabled = True
        Me.cboGenderRS.Items.AddRange(New Object() {"无", "公", "母", "无性别"})
        Me.cboGenderRS.Location = New System.Drawing.Point(81, 272)
        Me.cboGenderRS.Name = "cboGenderRS"
        Me.cboGenderRS.Size = New System.Drawing.Size(78, 20)
        Me.cboGenderRS.TabIndex = 49
        '
        'lblGenderRS
        '
        Me.lblGenderRS.AutoSize = True
        Me.lblGenderRS.Font = New System.Drawing.Font("宋体", 9.0!)
        Me.lblGenderRS.Location = New System.Drawing.Point(16, 275)
        Me.lblGenderRS.Name = "lblGenderRS"
        Me.lblGenderRS.Size = New System.Drawing.Size(53, 12)
        Me.lblGenderRS.TabIndex = 48
        Me.lblGenderRS.Text = "性别限制"
        Me.lblGenderRS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblNO
        '
        Me.lblNO.AutoSize = True
        Me.lblNO.Font = New System.Drawing.Font("宋体", 9.0!)
        Me.lblNO.Location = New System.Drawing.Point(16, 382)
        Me.lblNO.Name = "lblNO"
        Me.lblNO.Size = New System.Drawing.Size(29, 12)
        Me.lblNO.TabIndex = 46
        Me.lblNO.Text = "编号"
        Me.lblNO.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnImage
        '
        Me.btnImage.Enabled = False
        Me.btnImage.Location = New System.Drawing.Point(98, 370)
        Me.btnImage.Name = "btnImage"
        Me.btnImage.Size = New System.Drawing.Size(75, 31)
        Me.btnImage.TabIndex = 44
        Me.btnImage.Text = "编辑图象"
        Me.btnImage.UseVisualStyleBackColor = True
        '
        'cboTrait2
        '
        Me.cboTrait2.Enabled = False
        Me.cboTrait2.FormattingEnabled = True
        Me.cboTrait2.Items.AddRange(New Object() {"无", "恶臭", "逃跑能手", "发光", "蜜蜂", "捡东西", "降雨", "日照", "气象锁", "沙尘暴", "无天气", "暴雪", "冰之躯体", "接雨盘", "润湿身体", "干燥皮肤", "太阳能", "轻快", "沙隐术", "气象台", "花朵礼物", "雪遁", "叶绿素", "叶子守护", "自然恢复", "柔软", "精神力", "蜕皮", "迟钝", "自我中心", "早起", "活跃", "不眠", "熔岩盔甲", "水幕", "免疫", "石头脑袋", "坚硬", "化石盔甲", "吸盘", "贝壳装甲", "湿气", "隔音", "魔法守护", "蓄水", "蓄电", "电力引擎", "避雷针", "浮游", "属性之盾", "引火", "吸水", "厚脂肪", "耐热", "坚硬岩石", "过滤器", "威吓", "怪力钳", "加速", "净体", "白烟", "锐利眼光", "单纯", "天然", "有色眼镜", "走钢丝", "幸运", "狙击手", "技巧连接", "舍身", "下载", "强有力 ", "铁拳", "适应力", "技师", "好斗", "不要防守", "复眼", "瑜伽威力 ", "气魄", "普通皮肤", "变色", "天之恩赐", "磷粉", "愤怒神经", "上进", "同步率", "千鸟足", "快步走", "不屈之心", "神奇鳞片", "毒疗", "鲨鱼皮", "静电", "毒针", "淤泥", "苞子", "火焰之身", "魅惑之身", "引爆", "激流", "发芽", "烈火", "虫族报告", "活地狱", "踩影子", "磁力", "早食", "粘着", "破格", "复制", "洞悉心灵", "预知危险", "精神梦境", "后手打击", "缓慢启动", "偷懒", "紧张", "不用武器", "压力", "正极 ", "负极 ", "噩梦", "多重属性"})
        Me.cboTrait2.Location = New System.Drawing.Point(57, 157)
        Me.cboTrait2.Name = "cboTrait2"
        Me.cboTrait2.Size = New System.Drawing.Size(78, 20)
        Me.cboTrait2.TabIndex = 42
        '
        'lblTrait2
        '
        Me.lblTrait2.AutoSize = True
        Me.lblTrait2.Font = New System.Drawing.Font("宋体", 9.0!)
        Me.lblTrait2.Location = New System.Drawing.Point(16, 159)
        Me.lblTrait2.Name = "lblTrait2"
        Me.lblTrait2.Size = New System.Drawing.Size(35, 12)
        Me.lblTrait2.TabIndex = 41
        Me.lblTrait2.Text = "特性2"
        Me.lblTrait2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cboTrait1
        '
        Me.cboTrait1.Enabled = False
        Me.cboTrait1.FormattingEnabled = True
        Me.cboTrait1.Items.AddRange(New Object() {"恶臭", "逃跑能手", "发光", "蜜蜂", "捡东西", "降雨", "日照", "气象锁", "沙尘暴", "无天气", "暴雪", "冰之躯体", "接雨盘", "润湿身体", "干燥皮肤", "太阳能", "轻快", "沙隐术", "气象台", "花朵礼物", "雪遁", "叶绿素", "叶子守护", "自然恢复", "柔软", "精神力", "蜕皮", "迟钝", "自我中心", "早起", "活跃", "不眠", "熔岩盔甲", "水幕", "免疫", "石头脑袋", "坚硬", "化石盔甲", "吸盘", "贝壳装甲", "湿气", "隔音", "魔法守护", "蓄水", "蓄电", "电力引擎", "避雷针", "浮游", "属性之盾", "引火", "吸水", "厚脂肪", "耐热", "坚硬岩石", "过滤器", "威吓", "怪力钳", "加速", "净体", "白烟", "锐利眼光", "单纯", "天然", "有色眼镜", "走钢丝", "幸运", "狙击手", "技巧连接", "舍身", "下载", "强有力 ", "铁拳", "适应力", "技师", "好斗", "不要防守", "复眼", "瑜伽威力 ", "气魄", "普通皮肤", "变色", "天之恩赐", "磷粉", "愤怒神经", "上进", "同步率", "千鸟足", "快步走", "不屈之心", "神奇鳞片", "毒疗", "鲨鱼皮", "静电", "毒针", "淤泥", "苞子", "火焰之身", "魅惑之身", "引爆", "激流", "发芽", "烈火", "虫族报告", "活地狱", "踩影子", "磁力", "早食", "粘着", "破格", "复制", "洞悉心灵", "预知危险", "精神梦境", "后手打击", "缓慢启动", "偷懒", "紧张", "不用武器", "压力", "正极 ", "负极 ", "噩梦", "多重属性"})
        Me.cboTrait1.Location = New System.Drawing.Point(57, 124)
        Me.cboTrait1.Name = "cboTrait1"
        Me.cboTrait1.Size = New System.Drawing.Size(78, 20)
        Me.cboTrait1.TabIndex = 40
        '
        'lblTrait1
        '
        Me.lblTrait1.AutoSize = True
        Me.lblTrait1.Font = New System.Drawing.Font("宋体", 9.0!)
        Me.lblTrait1.Location = New System.Drawing.Point(16, 126)
        Me.lblTrait1.Name = "lblTrait1"
        Me.lblTrait1.Size = New System.Drawing.Size(35, 12)
        Me.lblTrait1.TabIndex = 39
        Me.lblTrait1.Text = "特性1"
        Me.lblTrait1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblSpeed
        '
        Me.lblSpeed.AutoSize = True
        Me.lblSpeed.Font = New System.Drawing.Font("宋体", 9.0!)
        Me.lblSpeed.Location = New System.Drawing.Point(141, 192)
        Me.lblSpeed.Name = "lblSpeed"
        Me.lblSpeed.Size = New System.Drawing.Size(65, 12)
        Me.lblSpeed.TabIndex = 37
        Me.lblSpeed.Text = "速度种族值"
        Me.lblSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblSDef
        '
        Me.lblSDef.AutoSize = True
        Me.lblSDef.Font = New System.Drawing.Font("宋体", 9.0!)
        Me.lblSDef.Location = New System.Drawing.Point(141, 159)
        Me.lblSDef.Name = "lblSDef"
        Me.lblSDef.Size = New System.Drawing.Size(65, 12)
        Me.lblSDef.TabIndex = 35
        Me.lblSDef.Text = "特防种族值"
        Me.lblSDef.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblSAtk
        '
        Me.lblSAtk.AutoSize = True
        Me.lblSAtk.Font = New System.Drawing.Font("宋体", 9.0!)
        Me.lblSAtk.Location = New System.Drawing.Point(141, 126)
        Me.lblSAtk.Name = "lblSAtk"
        Me.lblSAtk.Size = New System.Drawing.Size(65, 12)
        Me.lblSAtk.TabIndex = 33
        Me.lblSAtk.Text = "特攻种族值"
        Me.lblSAtk.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblDef
        '
        Me.lblDef.AutoSize = True
        Me.lblDef.Font = New System.Drawing.Font("宋体", 9.0!)
        Me.lblDef.Location = New System.Drawing.Point(141, 93)
        Me.lblDef.Name = "lblDef"
        Me.lblDef.Size = New System.Drawing.Size(65, 12)
        Me.lblDef.TabIndex = 31
        Me.lblDef.Text = "物防种族值"
        Me.lblDef.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblAtk
        '
        Me.lblAtk.AutoSize = True
        Me.lblAtk.Font = New System.Drawing.Font("宋体", 9.0!)
        Me.lblAtk.Location = New System.Drawing.Point(141, 60)
        Me.lblAtk.Name = "lblAtk"
        Me.lblAtk.Size = New System.Drawing.Size(65, 12)
        Me.lblAtk.TabIndex = 29
        Me.lblAtk.Text = "物攻种族值"
        Me.lblAtk.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblHP
        '
        Me.lblHP.AutoSize = True
        Me.lblHP.Font = New System.Drawing.Font("宋体", 9.0!)
        Me.lblHP.Location = New System.Drawing.Point(141, 27)
        Me.lblHP.Name = "lblHP"
        Me.lblHP.Size = New System.Drawing.Size(53, 12)
        Me.lblHP.TabIndex = 27
        Me.lblHP.Text = "HP种族值"
        Me.lblHP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cboType2
        '
        Me.cboType2.Enabled = False
        Me.cboType2.FormattingEnabled = True
        Me.cboType2.Items.AddRange(New Object() {"无"})
        Me.cboType2.Location = New System.Drawing.Point(57, 91)
        Me.cboType2.Name = "cboType2"
        Me.cboType2.Size = New System.Drawing.Size(78, 20)
        Me.cboType2.TabIndex = 26
        '
        'lblType2
        '
        Me.lblType2.AutoSize = True
        Me.lblType2.Font = New System.Drawing.Font("宋体", 9.0!)
        Me.lblType2.Location = New System.Drawing.Point(16, 94)
        Me.lblType2.Name = "lblType2"
        Me.lblType2.Size = New System.Drawing.Size(35, 12)
        Me.lblType2.TabIndex = 25
        Me.lblType2.Text = "属性2"
        Me.lblType2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cboType1
        '
        Me.cboType1.Enabled = False
        Me.cboType1.FormattingEnabled = True
        Me.cboType1.Location = New System.Drawing.Point(57, 58)
        Me.cboType1.Name = "cboType1"
        Me.cboType1.Size = New System.Drawing.Size(78, 20)
        Me.cboType1.TabIndex = 24
        '
        'lblType1
        '
        Me.lblType1.AutoSize = True
        Me.lblType1.Font = New System.Drawing.Font("宋体", 9.0!)
        Me.lblType1.Location = New System.Drawing.Point(16, 60)
        Me.lblType1.Name = "lblType1"
        Me.lblType1.Size = New System.Drawing.Size(35, 12)
        Me.lblType1.TabIndex = 23
        Me.lblType1.Text = "属性1"
        Me.lblType1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.Font = New System.Drawing.Font("宋体", 9.0!)
        Me.lblName.Location = New System.Drawing.Point(16, 27)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(29, 12)
        Me.lblName.TabIndex = 6
        Me.lblName.Text = "名称"
        Me.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtName
        '
        Me.txtName.Enabled = False
        Me.txtName.Location = New System.Drawing.Point(57, 24)
        Me.txtName.MaxLength = 4
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(78, 21)
        Me.txtName.TabIndex = 7
        '
        'btnDelNew
        '
        Me.btnDelNew.Enabled = False
        Me.btnDelNew.Location = New System.Drawing.Point(135, 37)
        Me.btnDelNew.Name = "btnDelNew"
        Me.btnDelNew.Size = New System.Drawing.Size(52, 23)
        Me.btnDelNew.TabIndex = 12
        Me.btnDelNew.Text = "删除"
        Me.btnDelNew.UseVisualStyleBackColor = True
        '
        'btnAddNew
        '
        Me.btnAddNew.Enabled = False
        Me.btnAddNew.Location = New System.Drawing.Point(135, 8)
        Me.btnAddNew.Name = "btnAddNew"
        Me.btnAddNew.Size = New System.Drawing.Size(52, 23)
        Me.btnAddNew.TabIndex = 11
        Me.btnAddNew.Text = "添加"
        Me.btnAddNew.UseVisualStyleBackColor = True
        '
        'tabUpdate
        '
        Me.tabUpdate.Controls.Add(Me.lstUpdate)
        Me.tabUpdate.Controls.Add(Me.grpUpdate)
        Me.tabUpdate.Controls.Add(Me.btnDelUpdate)
        Me.tabUpdate.Controls.Add(Me.btnAddUpdate)
        Me.tabUpdate.Location = New System.Drawing.Point(4, 21)
        Me.tabUpdate.Name = "tabUpdate"
        Me.tabUpdate.Padding = New System.Windows.Forms.Padding(3)
        Me.tabUpdate.Size = New System.Drawing.Size(628, 421)
        Me.tabUpdate.TabIndex = 1
        Me.tabUpdate.Text = "修改精灵"
        Me.tabUpdate.UseVisualStyleBackColor = True
        '
        'lstUpdate
        '
        Me.lstUpdate.FormattingEnabled = True
        Me.lstUpdate.ItemHeight = 12
        Me.lstUpdate.Location = New System.Drawing.Point(9, 8)
        Me.lstUpdate.Name = "lstUpdate"
        Me.lstUpdate.Size = New System.Drawing.Size(120, 400)
        Me.lstUpdate.TabIndex = 61
        '
        'grpUpdate
        '
        Me.grpUpdate.Controls.Add(Me.lblInfo)
        Me.grpUpdate.Controls.Add(Me.nudNewWeight)
        Me.grpUpdate.Controls.Add(Me.nudNewSpeed)
        Me.grpUpdate.Controls.Add(Me.nudNewSDef)
        Me.grpUpdate.Controls.Add(Me.nudNewSAtk)
        Me.grpUpdate.Controls.Add(Me.nudNewDef)
        Me.grpUpdate.Controls.Add(Me.nudNewAtk)
        Me.grpUpdate.Controls.Add(Me.nudNewHP)
        Me.grpUpdate.Controls.Add(Me.btnDelRM)
        Me.grpUpdate.Controls.Add(Me.btnAddRM)
        Me.grpUpdate.Controls.Add(Me.btnDelAM)
        Me.grpUpdate.Controls.Add(Me.btnAddAM)
        Me.grpUpdate.Controls.Add(Me.lblRemoveMove)
        Me.grpUpdate.Controls.Add(Me.lblAddMove)
        Me.grpUpdate.Controls.Add(Me.lstRemoveMove)
        Me.grpUpdate.Controls.Add(Me.lstAddMove)
        Me.grpUpdate.Controls.Add(Me.lblUpdateWeight)
        Me.grpUpdate.Controls.Add(Me.cboNewTrait2)
        Me.grpUpdate.Controls.Add(Me.lblUpdateTrait2)
        Me.grpUpdate.Controls.Add(Me.cboNewTrait1)
        Me.grpUpdate.Controls.Add(Me.lblUpdateTrait1)
        Me.grpUpdate.Controls.Add(Me.lblUpdateSpeed)
        Me.grpUpdate.Controls.Add(Me.lblUpdateSDef)
        Me.grpUpdate.Controls.Add(Me.lblUpdateSAtk)
        Me.grpUpdate.Controls.Add(Me.lblUpdateDef)
        Me.grpUpdate.Controls.Add(Me.lblUpdateAtk)
        Me.grpUpdate.Controls.Add(Me.lblUpdateHP)
        Me.grpUpdate.Controls.Add(Me.cboNewType2)
        Me.grpUpdate.Controls.Add(Me.lblUpdateType2)
        Me.grpUpdate.Controls.Add(Me.cboNewType1)
        Me.grpUpdate.Controls.Add(Me.lblUpdateType1)
        Me.grpUpdate.Controls.Add(Me.lblNameBase)
        Me.grpUpdate.Controls.Add(Me.txtNameBase)
        Me.grpUpdate.Location = New System.Drawing.Point(193, 8)
        Me.grpUpdate.Name = "grpUpdate"
        Me.grpUpdate.Size = New System.Drawing.Size(293, 407)
        Me.grpUpdate.TabIndex = 17
        Me.grpUpdate.TabStop = False
        Me.grpUpdate.Text = "PM"
        '
        'lblInfo
        '
        Me.lblInfo.AutoSize = True
        Me.lblInfo.Location = New System.Drawing.Point(120, 213)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(167, 12)
        Me.lblInfo.TabIndex = 75
        Me.lblInfo.Text = "(0表示不对该项数值进行修改)"
        '
        'nudNewWeight
        '
        Me.nudNewWeight.DecimalPlaces = 2
        Me.nudNewWeight.Enabled = False
        Me.nudNewWeight.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.nudNewWeight.Location = New System.Drawing.Point(57, 189)
        Me.nudNewWeight.Maximum = New Decimal(New Integer() {999, 0, 0, 0})
        Me.nudNewWeight.Name = "nudNewWeight"
        Me.nudNewWeight.Size = New System.Drawing.Size(81, 21)
        Me.nudNewWeight.TabIndex = 74
        '
        'nudNewSpeed
        '
        Me.nudNewSpeed.Enabled = False
        Me.nudNewSpeed.Location = New System.Drawing.Point(225, 189)
        Me.nudNewSpeed.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me.nudNewSpeed.Name = "nudNewSpeed"
        Me.nudNewSpeed.Size = New System.Drawing.Size(62, 21)
        Me.nudNewSpeed.TabIndex = 73
        '
        'nudNewSDef
        '
        Me.nudNewSDef.Enabled = False
        Me.nudNewSDef.Location = New System.Drawing.Point(225, 156)
        Me.nudNewSDef.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me.nudNewSDef.Name = "nudNewSDef"
        Me.nudNewSDef.Size = New System.Drawing.Size(62, 21)
        Me.nudNewSDef.TabIndex = 72
        '
        'nudNewSAtk
        '
        Me.nudNewSAtk.Enabled = False
        Me.nudNewSAtk.Location = New System.Drawing.Point(225, 123)
        Me.nudNewSAtk.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me.nudNewSAtk.Name = "nudNewSAtk"
        Me.nudNewSAtk.Size = New System.Drawing.Size(62, 21)
        Me.nudNewSAtk.TabIndex = 71
        '
        'nudNewDef
        '
        Me.nudNewDef.Enabled = False
        Me.nudNewDef.Location = New System.Drawing.Point(225, 90)
        Me.nudNewDef.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me.nudNewDef.Name = "nudNewDef"
        Me.nudNewDef.Size = New System.Drawing.Size(62, 21)
        Me.nudNewDef.TabIndex = 70
        '
        'nudNewAtk
        '
        Me.nudNewAtk.Enabled = False
        Me.nudNewAtk.Location = New System.Drawing.Point(225, 57)
        Me.nudNewAtk.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me.nudNewAtk.Name = "nudNewAtk"
        Me.nudNewAtk.Size = New System.Drawing.Size(62, 21)
        Me.nudNewAtk.TabIndex = 69
        '
        'nudNewHP
        '
        Me.nudNewHP.Enabled = False
        Me.nudNewHP.Location = New System.Drawing.Point(225, 24)
        Me.nudNewHP.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me.nudNewHP.Name = "nudNewHP"
        Me.nudNewHP.Size = New System.Drawing.Size(62, 21)
        Me.nudNewHP.TabIndex = 68
        '
        'btnDelRM
        '
        Me.btnDelRM.Enabled = False
        Me.btnDelRM.Location = New System.Drawing.Point(236, 378)
        Me.btnDelRM.Name = "btnDelRM"
        Me.btnDelRM.Size = New System.Drawing.Size(52, 23)
        Me.btnDelRM.TabIndex = 60
        Me.btnDelRM.Text = "删除"
        Me.btnDelRM.UseVisualStyleBackColor = True
        '
        'btnAddRM
        '
        Me.btnAddRM.Enabled = False
        Me.btnAddRM.Location = New System.Drawing.Point(167, 378)
        Me.btnAddRM.Name = "btnAddRM"
        Me.btnAddRM.Size = New System.Drawing.Size(52, 23)
        Me.btnAddRM.TabIndex = 59
        Me.btnAddRM.Text = "添加"
        Me.btnAddRM.UseVisualStyleBackColor = True
        '
        'btnDelAM
        '
        Me.btnDelAM.Enabled = False
        Me.btnDelAM.Location = New System.Drawing.Point(86, 378)
        Me.btnDelAM.Name = "btnDelAM"
        Me.btnDelAM.Size = New System.Drawing.Size(52, 23)
        Me.btnDelAM.TabIndex = 58
        Me.btnDelAM.Text = "删除"
        Me.btnDelAM.UseVisualStyleBackColor = True
        '
        'btnAddAM
        '
        Me.btnAddAM.Enabled = False
        Me.btnAddAM.Location = New System.Drawing.Point(17, 378)
        Me.btnAddAM.Name = "btnAddAM"
        Me.btnAddAM.Size = New System.Drawing.Size(52, 23)
        Me.btnAddAM.TabIndex = 57
        Me.btnAddAM.Text = "添加"
        Me.btnAddAM.UseVisualStyleBackColor = True
        '
        'lblRemoveMove
        '
        Me.lblRemoveMove.AutoSize = True
        Me.lblRemoveMove.Font = New System.Drawing.Font("宋体", 9.0!)
        Me.lblRemoveMove.Location = New System.Drawing.Point(166, 236)
        Me.lblRemoveMove.Name = "lblRemoveMove"
        Me.lblRemoveMove.Size = New System.Drawing.Size(53, 12)
        Me.lblRemoveMove.TabIndex = 56
        Me.lblRemoveMove.Text = "屏蔽技能"
        Me.lblRemoveMove.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblAddMove
        '
        Me.lblAddMove.AutoSize = True
        Me.lblAddMove.Font = New System.Drawing.Font("宋体", 9.0!)
        Me.lblAddMove.Location = New System.Drawing.Point(16, 236)
        Me.lblAddMove.Name = "lblAddMove"
        Me.lblAddMove.Size = New System.Drawing.Size(53, 12)
        Me.lblAddMove.TabIndex = 55
        Me.lblAddMove.Text = "新增技能"
        Me.lblAddMove.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lstRemoveMove
        '
        Me.lstRemoveMove.FormattingEnabled = True
        Me.lstRemoveMove.ItemHeight = 12
        Me.lstRemoveMove.Location = New System.Drawing.Point(167, 251)
        Me.lstRemoveMove.Name = "lstRemoveMove"
        Me.lstRemoveMove.Size = New System.Drawing.Size(120, 124)
        Me.lstRemoveMove.TabIndex = 54
        '
        'lstAddMove
        '
        Me.lstAddMove.FormattingEnabled = True
        Me.lstAddMove.ItemHeight = 12
        Me.lstAddMove.Location = New System.Drawing.Point(18, 251)
        Me.lstAddMove.Name = "lstAddMove"
        Me.lstAddMove.Size = New System.Drawing.Size(120, 124)
        Me.lstAddMove.TabIndex = 53
        '
        'lblUpdateWeight
        '
        Me.lblUpdateWeight.AutoSize = True
        Me.lblUpdateWeight.Font = New System.Drawing.Font("宋体", 9.0!)
        Me.lblUpdateWeight.Location = New System.Drawing.Point(16, 192)
        Me.lblUpdateWeight.Name = "lblUpdateWeight"
        Me.lblUpdateWeight.Size = New System.Drawing.Size(29, 12)
        Me.lblUpdateWeight.TabIndex = 51
        Me.lblUpdateWeight.Text = "体重"
        Me.lblUpdateWeight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cboNewTrait2
        '
        Me.cboNewTrait2.Enabled = False
        Me.cboNewTrait2.FormattingEnabled = True
        Me.cboNewTrait2.Items.AddRange(New Object() {"无更改", "恶臭", "逃跑能手", "发光", "蜜蜂", "捡东西", "降雨", "日照", "气象锁", "沙尘暴", "无天气", "暴雪", "冰之躯体", "接雨盘", "润湿身体", "干燥皮肤", "太阳能", "轻快", "沙隐术", "气象台", "花朵礼物", "雪遁", "叶绿素", "叶子守护", "自然恢复", "柔软", "精神力", "蜕皮", "迟钝", "自我中心", "早起", "活跃", "不眠", "熔岩盔甲", "水幕", "免疫", "石头脑袋", "坚硬", "化石盔甲", "吸盘", "贝壳装甲", "湿气", "隔音", "魔法守护", "蓄水", "蓄电", "电力引擎", "避雷针", "浮游", "属性之盾", "引火", "吸水", "厚脂肪", "耐热", "坚硬岩石", "过滤器", "威吓", "怪力钳", "加速", "净体", "白烟", "锐利眼光", "单纯", "天然", "有色眼镜", "走钢丝", "幸运", "狙击手", "技巧连接", "舍身", "下载", "强有力 ", "铁拳", "适应力", "技师", "好斗", "不要防守", "复眼", "瑜伽威力 ", "气魄", "普通皮肤", "变色", "天之恩赐", "磷粉", "愤怒神经", "上进", "同步率", "千鸟足", "快步走", "不屈之心", "神奇鳞片", "毒疗", "鲨鱼皮", "静电", "毒针", "淤泥", "苞子", "火焰之身", "魅惑之身", "引爆", "激流", "发芽", "烈火", "虫族报告", "活地狱", "踩影子", "磁力", "早食", "粘着", "破格", "复制", "洞悉心灵", "预知危险", "精神梦境", "后手打击", "缓慢启动", "偷懒", "紧张", "不用武器", "压力", "正极 ", "负极 ", "噩梦", "多重属性"})
        Me.cboNewTrait2.Location = New System.Drawing.Point(57, 157)
        Me.cboNewTrait2.Name = "cboNewTrait2"
        Me.cboNewTrait2.Size = New System.Drawing.Size(81, 20)
        Me.cboNewTrait2.TabIndex = 42
        '
        'lblUpdateTrait2
        '
        Me.lblUpdateTrait2.AutoSize = True
        Me.lblUpdateTrait2.Font = New System.Drawing.Font("宋体", 9.0!)
        Me.lblUpdateTrait2.Location = New System.Drawing.Point(16, 159)
        Me.lblUpdateTrait2.Name = "lblUpdateTrait2"
        Me.lblUpdateTrait2.Size = New System.Drawing.Size(35, 12)
        Me.lblUpdateTrait2.TabIndex = 41
        Me.lblUpdateTrait2.Text = "特性2"
        Me.lblUpdateTrait2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cboNewTrait1
        '
        Me.cboNewTrait1.Enabled = False
        Me.cboNewTrait1.FormattingEnabled = True
        Me.cboNewTrait1.Items.AddRange(New Object() {"无更改", "恶臭", "逃跑能手", "发光", "蜜蜂", "捡东西", "降雨", "日照", "气象锁", "沙尘暴", "无天气", "暴雪", "冰之躯体", "接雨盘", "润湿身体", "干燥皮肤", "太阳能", "轻快", "沙隐术", "气象台", "花朵礼物", "雪遁", "叶绿素", "叶子守护", "自然恢复", "柔软", "精神力", "蜕皮", "迟钝", "自我中心", "早起", "活跃", "不眠", "熔岩盔甲", "水幕", "免疫", "石头脑袋", "坚硬", "化石盔甲", "吸盘", "贝壳装甲", "湿气", "隔音", "魔法守护", "蓄水", "蓄电", "电力引擎", "避雷针", "浮游", "属性之盾", "引火", "吸水", "厚脂肪", "耐热", "坚硬岩石", "过滤器", "威吓", "怪力钳", "加速", "净体", "白烟", "锐利眼光", "单纯", "天然", "有色眼镜", "走钢丝", "幸运", "狙击手", "技巧连接", "舍身", "下载", "强有力 ", "铁拳", "适应力", "技师", "好斗", "不要防守", "复眼", "瑜伽威力 ", "气魄", "普通皮肤", "变色", "天之恩赐", "磷粉", "愤怒神经", "上进", "同步率", "千鸟足", "快步走", "不屈之心", "神奇鳞片", "毒疗", "鲨鱼皮", "静电", "毒针", "淤泥", "苞子", "火焰之身", "魅惑之身", "引爆", "激流", "发芽", "烈火", "虫族报告", "活地狱", "踩影子", "磁力", "早食", "粘着", "破格", "复制", "洞悉心灵", "预知危险", "精神梦境", "后手打击", "缓慢启动", "偷懒", "紧张", "不用武器", "压力", "正极 ", "负极 ", "噩梦", "多重属性"})
        Me.cboNewTrait1.Location = New System.Drawing.Point(57, 124)
        Me.cboNewTrait1.Name = "cboNewTrait1"
        Me.cboNewTrait1.Size = New System.Drawing.Size(81, 20)
        Me.cboNewTrait1.TabIndex = 40
        '
        'lblUpdateTrait1
        '
        Me.lblUpdateTrait1.AutoSize = True
        Me.lblUpdateTrait1.Font = New System.Drawing.Font("宋体", 9.0!)
        Me.lblUpdateTrait1.Location = New System.Drawing.Point(16, 126)
        Me.lblUpdateTrait1.Name = "lblUpdateTrait1"
        Me.lblUpdateTrait1.Size = New System.Drawing.Size(35, 12)
        Me.lblUpdateTrait1.TabIndex = 39
        Me.lblUpdateTrait1.Text = "特性1"
        Me.lblUpdateTrait1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblUpdateSpeed
        '
        Me.lblUpdateSpeed.AutoSize = True
        Me.lblUpdateSpeed.Font = New System.Drawing.Font("宋体", 9.0!)
        Me.lblUpdateSpeed.Location = New System.Drawing.Point(150, 192)
        Me.lblUpdateSpeed.Name = "lblUpdateSpeed"
        Me.lblUpdateSpeed.Size = New System.Drawing.Size(65, 12)
        Me.lblUpdateSpeed.TabIndex = 37
        Me.lblUpdateSpeed.Text = "速度种族值"
        Me.lblUpdateSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblUpdateSDef
        '
        Me.lblUpdateSDef.AutoSize = True
        Me.lblUpdateSDef.Font = New System.Drawing.Font("宋体", 9.0!)
        Me.lblUpdateSDef.Location = New System.Drawing.Point(150, 159)
        Me.lblUpdateSDef.Name = "lblUpdateSDef"
        Me.lblUpdateSDef.Size = New System.Drawing.Size(65, 12)
        Me.lblUpdateSDef.TabIndex = 35
        Me.lblUpdateSDef.Text = "特防种族值"
        Me.lblUpdateSDef.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblUpdateSAtk
        '
        Me.lblUpdateSAtk.AutoSize = True
        Me.lblUpdateSAtk.Font = New System.Drawing.Font("宋体", 9.0!)
        Me.lblUpdateSAtk.Location = New System.Drawing.Point(150, 126)
        Me.lblUpdateSAtk.Name = "lblUpdateSAtk"
        Me.lblUpdateSAtk.Size = New System.Drawing.Size(65, 12)
        Me.lblUpdateSAtk.TabIndex = 33
        Me.lblUpdateSAtk.Text = "特攻种族值"
        Me.lblUpdateSAtk.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblUpdateDef
        '
        Me.lblUpdateDef.AutoSize = True
        Me.lblUpdateDef.Font = New System.Drawing.Font("宋体", 9.0!)
        Me.lblUpdateDef.Location = New System.Drawing.Point(150, 93)
        Me.lblUpdateDef.Name = "lblUpdateDef"
        Me.lblUpdateDef.Size = New System.Drawing.Size(65, 12)
        Me.lblUpdateDef.TabIndex = 31
        Me.lblUpdateDef.Text = "物防种族值"
        Me.lblUpdateDef.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblUpdateAtk
        '
        Me.lblUpdateAtk.AutoSize = True
        Me.lblUpdateAtk.Font = New System.Drawing.Font("宋体", 9.0!)
        Me.lblUpdateAtk.Location = New System.Drawing.Point(150, 60)
        Me.lblUpdateAtk.Name = "lblUpdateAtk"
        Me.lblUpdateAtk.Size = New System.Drawing.Size(65, 12)
        Me.lblUpdateAtk.TabIndex = 29
        Me.lblUpdateAtk.Text = "物攻种族值"
        Me.lblUpdateAtk.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblUpdateHP
        '
        Me.lblUpdateHP.AutoSize = True
        Me.lblUpdateHP.Font = New System.Drawing.Font("宋体", 9.0!)
        Me.lblUpdateHP.Location = New System.Drawing.Point(150, 27)
        Me.lblUpdateHP.Name = "lblUpdateHP"
        Me.lblUpdateHP.Size = New System.Drawing.Size(53, 12)
        Me.lblUpdateHP.TabIndex = 27
        Me.lblUpdateHP.Text = "HP种族值"
        Me.lblUpdateHP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cboNewType2
        '
        Me.cboNewType2.Enabled = False
        Me.cboNewType2.FormattingEnabled = True
        Me.cboNewType2.Items.AddRange(New Object() {"无更改"})
        Me.cboNewType2.Location = New System.Drawing.Point(57, 91)
        Me.cboNewType2.Name = "cboNewType2"
        Me.cboNewType2.Size = New System.Drawing.Size(81, 20)
        Me.cboNewType2.TabIndex = 26
        '
        'lblUpdateType2
        '
        Me.lblUpdateType2.AutoSize = True
        Me.lblUpdateType2.Font = New System.Drawing.Font("宋体", 9.0!)
        Me.lblUpdateType2.Location = New System.Drawing.Point(16, 94)
        Me.lblUpdateType2.Name = "lblUpdateType2"
        Me.lblUpdateType2.Size = New System.Drawing.Size(35, 12)
        Me.lblUpdateType2.TabIndex = 25
        Me.lblUpdateType2.Text = "属性2"
        Me.lblUpdateType2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cboNewType1
        '
        Me.cboNewType1.Enabled = False
        Me.cboNewType1.FormattingEnabled = True
        Me.cboNewType1.Items.AddRange(New Object() {"无更改"})
        Me.cboNewType1.Location = New System.Drawing.Point(57, 58)
        Me.cboNewType1.Name = "cboNewType1"
        Me.cboNewType1.Size = New System.Drawing.Size(81, 20)
        Me.cboNewType1.TabIndex = 24
        '
        'lblUpdateType1
        '
        Me.lblUpdateType1.AutoSize = True
        Me.lblUpdateType1.Font = New System.Drawing.Font("宋体", 9.0!)
        Me.lblUpdateType1.Location = New System.Drawing.Point(16, 60)
        Me.lblUpdateType1.Name = "lblUpdateType1"
        Me.lblUpdateType1.Size = New System.Drawing.Size(35, 12)
        Me.lblUpdateType1.TabIndex = 23
        Me.lblUpdateType1.Text = "属性1"
        Me.lblUpdateType1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblNameBase
        '
        Me.lblNameBase.AutoSize = True
        Me.lblNameBase.Font = New System.Drawing.Font("宋体", 9.0!)
        Me.lblNameBase.Location = New System.Drawing.Point(16, 27)
        Me.lblNameBase.Name = "lblNameBase"
        Me.lblNameBase.Size = New System.Drawing.Size(29, 12)
        Me.lblNameBase.TabIndex = 6
        Me.lblNameBase.Text = "名称"
        Me.lblNameBase.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtNameBase
        '
        Me.txtNameBase.Location = New System.Drawing.Point(57, 24)
        Me.txtNameBase.Name = "txtNameBase"
        Me.txtNameBase.ReadOnly = True
        Me.txtNameBase.Size = New System.Drawing.Size(81, 21)
        Me.txtNameBase.TabIndex = 7
        '
        'btnDelUpdate
        '
        Me.btnDelUpdate.Enabled = False
        Me.btnDelUpdate.Location = New System.Drawing.Point(135, 37)
        Me.btnDelUpdate.Name = "btnDelUpdate"
        Me.btnDelUpdate.Size = New System.Drawing.Size(52, 23)
        Me.btnDelUpdate.TabIndex = 15
        Me.btnDelUpdate.Text = "删除"
        Me.btnDelUpdate.UseVisualStyleBackColor = True
        '
        'btnAddUpdate
        '
        Me.btnAddUpdate.Enabled = False
        Me.btnAddUpdate.Location = New System.Drawing.Point(135, 8)
        Me.btnAddUpdate.Name = "btnAddUpdate"
        Me.btnAddUpdate.Size = New System.Drawing.Size(52, 23)
        Me.btnAddUpdate.TabIndex = 14
        Me.btnAddUpdate.Text = "添加"
        Me.btnAddUpdate.UseVisualStyleBackColor = True
        '
        'tabRemove
        '
        Me.tabRemove.Controls.Add(Me.btnDelRemove)
        Me.tabRemove.Controls.Add(Me.btnAddRemove)
        Me.tabRemove.Controls.Add(Me.lstRemovePM)
        Me.tabRemove.Location = New System.Drawing.Point(4, 21)
        Me.tabRemove.Name = "tabRemove"
        Me.tabRemove.Padding = New System.Windows.Forms.Padding(3)
        Me.tabRemove.Size = New System.Drawing.Size(628, 421)
        Me.tabRemove.TabIndex = 2
        Me.tabRemove.Text = "屏蔽精灵"
        Me.tabRemove.UseVisualStyleBackColor = True
        '
        'btnDelRemove
        '
        Me.btnDelRemove.Enabled = False
        Me.btnDelRemove.Location = New System.Drawing.Point(76, 388)
        Me.btnDelRemove.Name = "btnDelRemove"
        Me.btnDelRemove.Size = New System.Drawing.Size(52, 23)
        Me.btnDelRemove.TabIndex = 63
        Me.btnDelRemove.Text = "删除"
        Me.btnDelRemove.UseVisualStyleBackColor = True
        '
        'btnAddRemove
        '
        Me.btnAddRemove.Enabled = False
        Me.btnAddRemove.Location = New System.Drawing.Point(8, 388)
        Me.btnAddRemove.Name = "btnAddRemove"
        Me.btnAddRemove.Size = New System.Drawing.Size(52, 23)
        Me.btnAddRemove.TabIndex = 62
        Me.btnAddRemove.Text = "添加"
        Me.btnAddRemove.UseVisualStyleBackColor = True
        '
        'lstRemovePM
        '
        Me.lstRemovePM.FormattingEnabled = True
        Me.lstRemovePM.ItemHeight = 12
        Me.lstRemovePM.Location = New System.Drawing.Point(8, 6)
        Me.lstRemovePM.Name = "lstRemovePM"
        Me.lstRemovePM.Size = New System.Drawing.Size(120, 376)
        Me.lstRemovePM.TabIndex = 61
        '
        'tabImage
        '
        Me.tabImage.Controls.Add(Me.btnDelImg)
        Me.tabImage.Controls.Add(Me.btnAddImg)
        Me.tabImage.Controls.Add(Me.lstImages)
        Me.tabImage.Location = New System.Drawing.Point(4, 21)
        Me.tabImage.Name = "tabImage"
        Me.tabImage.Padding = New System.Windows.Forms.Padding(3)
        Me.tabImage.Size = New System.Drawing.Size(628, 421)
        Me.tabImage.TabIndex = 3
        Me.tabImage.Text = "图象资源"
        Me.tabImage.UseVisualStyleBackColor = True
        '
        'btnDelImg
        '
        Me.btnDelImg.Enabled = False
        Me.btnDelImg.Location = New System.Drawing.Point(546, 376)
        Me.btnDelImg.Name = "btnDelImg"
        Me.btnDelImg.Size = New System.Drawing.Size(75, 37)
        Me.btnDelImg.TabIndex = 2
        Me.btnDelImg.Text = "删除"
        Me.btnDelImg.UseVisualStyleBackColor = True
        '
        'btnAddImg
        '
        Me.btnAddImg.Enabled = False
        Me.btnAddImg.Location = New System.Drawing.Point(465, 376)
        Me.btnAddImg.Name = "btnAddImg"
        Me.btnAddImg.Size = New System.Drawing.Size(75, 37)
        Me.btnAddImg.TabIndex = 1
        Me.btnAddImg.Text = "添加"
        Me.btnAddImg.UseVisualStyleBackColor = True
        '
        'lstImages
        '
        Me.lstImages.LargeImageList = Me.ImageList
        Me.lstImages.Location = New System.Drawing.Point(6, 6)
        Me.lstImages.Name = "lstImages"
        Me.lstImages.Size = New System.Drawing.Size(614, 364)
        Me.lstImages.TabIndex = 0
        Me.lstImages.UseCompatibleStateImageBehavior = False
        '
        'ImageList
        '
        Me.ImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth16Bit
        Me.ImageList.ImageSize = New System.Drawing.Size(50, 50)
        Me.ImageList.TransparentColor = System.Drawing.Color.Transparent
        '
        'mnuCustom
        '
        Me.mnuCustom.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.项目FToolStripMenuItem})
        Me.mnuCustom.Location = New System.Drawing.Point(0, 0)
        Me.mnuCustom.Name = "mnuCustom"
        Me.mnuCustom.Size = New System.Drawing.Size(636, 24)
        Me.mnuCustom.TabIndex = 64
        Me.mnuCustom.Text = "MenuStrip1"
        '
        '项目FToolStripMenuItem
        '
        Me.项目FToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.新建NToolStripMenuItem, Me.打开OToolStripMenuItem, Me.toolStripSeparator, Me.保存SToolStripMenuItem, Me.另存为AToolStripMenuItem, Me.ToolStripSeparator1, Me.更改项目名RToolStripMenuItem, Me.生成数据BToolStripMenuItem, Me.toolStripSeparator2, Me.退出XToolStripMenuItem})
        Me.项目FToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.MatchOnly
        Me.项目FToolStripMenuItem.Name = "项目FToolStripMenuItem"
        Me.项目FToolStripMenuItem.Size = New System.Drawing.Size(59, 20)
        Me.项目FToolStripMenuItem.Text = "项目(&D)"
        '
        '新建NToolStripMenuItem
        '
        Me.新建NToolStripMenuItem.Image = CType(resources.GetObject("新建NToolStripMenuItem.Image"), System.Drawing.Image)
        Me.新建NToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.新建NToolStripMenuItem.Name = "新建NToolStripMenuItem"
        Me.新建NToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.新建NToolStripMenuItem.Text = "新建(&N)"
        '
        '打开OToolStripMenuItem
        '
        Me.打开OToolStripMenuItem.Image = CType(resources.GetObject("打开OToolStripMenuItem.Image"), System.Drawing.Image)
        Me.打开OToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.打开OToolStripMenuItem.Name = "打开OToolStripMenuItem"
        Me.打开OToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.打开OToolStripMenuItem.Text = "打开(&O)"
        '
        'toolStripSeparator
        '
        Me.toolStripSeparator.Name = "toolStripSeparator"
        Me.toolStripSeparator.Size = New System.Drawing.Size(149, 6)
        '
        '保存SToolStripMenuItem
        '
        Me.保存SToolStripMenuItem.Image = CType(resources.GetObject("保存SToolStripMenuItem.Image"), System.Drawing.Image)
        Me.保存SToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.保存SToolStripMenuItem.Name = "保存SToolStripMenuItem"
        Me.保存SToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.保存SToolStripMenuItem.Text = "保存(&S)"
        '
        '另存为AToolStripMenuItem
        '
        Me.另存为AToolStripMenuItem.Name = "另存为AToolStripMenuItem"
        Me.另存为AToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.另存为AToolStripMenuItem.Text = "另存为(&A)"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(149, 6)
        '
        '生成数据BToolStripMenuItem
        '
        Me.生成数据BToolStripMenuItem.Name = "生成数据BToolStripMenuItem"
        Me.生成数据BToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.生成数据BToolStripMenuItem.Text = "生成数据(&B)"
        '
        'toolStripSeparator2
        '
        Me.toolStripSeparator2.Name = "toolStripSeparator2"
        Me.toolStripSeparator2.Size = New System.Drawing.Size(149, 6)
        '
        '退出XToolStripMenuItem
        '
        Me.退出XToolStripMenuItem.Name = "退出XToolStripMenuItem"
        Me.退出XToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.退出XToolStripMenuItem.Text = "退出(&X)"
        '
        '更改项目名RToolStripMenuItem
        '
        Me.更改项目名RToolStripMenuItem.Name = "更改项目名RToolStripMenuItem"
        Me.更改项目名RToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.更改项目名RToolStripMenuItem.Text = "更改项目名(&R)"
        '
        'FrmCustomData
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(636, 472)
        Me.Controls.Add(Me.mnuCustom)
        Me.Controls.Add(Me.tabData)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "FrmCustomData"
        Me.Text = "自定义数据"
        Me.tabData.ResumeLayout(False)
        Me.tabAdd.ResumeLayout(False)
        Me.grpPM.ResumeLayout(False)
        Me.grpPM.PerformLayout()
        CType(Me.nudWeight, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudSpeed, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudSDef, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudSAtk, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudDef, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudAtk, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudHP, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabUpdate.ResumeLayout(False)
        Me.grpUpdate.ResumeLayout(False)
        Me.grpUpdate.PerformLayout()
        CType(Me.nudNewWeight, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudNewSpeed, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudNewSDef, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudNewSAtk, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudNewDef, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudNewAtk, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudNewHP, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabRemove.ResumeLayout(False)
        Me.tabImage.ResumeLayout(False)
        Me.mnuCustom.ResumeLayout(False)
        Me.mnuCustom.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tabData As System.Windows.Forms.TabControl
    Friend WithEvents tabAdd As System.Windows.Forms.TabPage
    Friend WithEvents tabUpdate As System.Windows.Forms.TabPage
    Friend WithEvents btnDelNew As System.Windows.Forms.Button
    Friend WithEvents btnAddNew As System.Windows.Forms.Button
    Friend WithEvents grpPM As System.Windows.Forms.GroupBox
    Friend WithEvents cboEg2 As System.Windows.Forms.ComboBox
    Friend WithEvents lblEg2 As System.Windows.Forms.Label
    Friend WithEvents cboEg1 As System.Windows.Forms.ComboBox
    Friend WithEvents lblEg1 As System.Windows.Forms.Label
    Friend WithEvents lblWeight As System.Windows.Forms.Label
    Friend WithEvents cboGenderRS As System.Windows.Forms.ComboBox
    Friend WithEvents lblGenderRS As System.Windows.Forms.Label
    Friend WithEvents lblNO As System.Windows.Forms.Label
    Friend WithEvents btnImage As System.Windows.Forms.Button
    Friend WithEvents cboTrait2 As System.Windows.Forms.ComboBox
    Friend WithEvents lblTrait2 As System.Windows.Forms.Label
    Friend WithEvents cboTrait1 As System.Windows.Forms.ComboBox
    Friend WithEvents lblTrait1 As System.Windows.Forms.Label
    Friend WithEvents lblSpeed As System.Windows.Forms.Label
    Friend WithEvents lblSDef As System.Windows.Forms.Label
    Friend WithEvents lblSAtk As System.Windows.Forms.Label
    Friend WithEvents lblDef As System.Windows.Forms.Label
    Friend WithEvents lblAtk As System.Windows.Forms.Label
    Friend WithEvents lblHP As System.Windows.Forms.Label
    Friend WithEvents cboType2 As System.Windows.Forms.ComboBox
    Friend WithEvents lblType2 As System.Windows.Forms.Label
    Friend WithEvents cboType1 As System.Windows.Forms.ComboBox
    Friend WithEvents lblType1 As System.Windows.Forms.Label
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents grpUpdate As System.Windows.Forms.GroupBox
    Friend WithEvents lblUpdateWeight As System.Windows.Forms.Label
    Friend WithEvents cboNewTrait2 As System.Windows.Forms.ComboBox
    Friend WithEvents lblUpdateTrait2 As System.Windows.Forms.Label
    Friend WithEvents cboNewTrait1 As System.Windows.Forms.ComboBox
    Friend WithEvents lblUpdateTrait1 As System.Windows.Forms.Label
    Friend WithEvents lblUpdateSpeed As System.Windows.Forms.Label
    Friend WithEvents lblUpdateSDef As System.Windows.Forms.Label
    Friend WithEvents lblUpdateSAtk As System.Windows.Forms.Label
    Friend WithEvents lblUpdateDef As System.Windows.Forms.Label
    Friend WithEvents lblUpdateAtk As System.Windows.Forms.Label
    Friend WithEvents lblUpdateHP As System.Windows.Forms.Label
    Friend WithEvents cboNewType2 As System.Windows.Forms.ComboBox
    Friend WithEvents lblUpdateType2 As System.Windows.Forms.Label
    Friend WithEvents cboNewType1 As System.Windows.Forms.ComboBox
    Friend WithEvents lblUpdateType1 As System.Windows.Forms.Label
    Friend WithEvents lblNameBase As System.Windows.Forms.Label
    Friend WithEvents txtNameBase As System.Windows.Forms.TextBox
    Friend WithEvents btnDelUpdate As System.Windows.Forms.Button
    Friend WithEvents btnAddUpdate As System.Windows.Forms.Button
    Friend WithEvents lblRemoveMove As System.Windows.Forms.Label
    Friend WithEvents lblAddMove As System.Windows.Forms.Label
    Friend WithEvents lstRemoveMove As System.Windows.Forms.ListBox
    Friend WithEvents lstAddMove As System.Windows.Forms.ListBox
    Friend WithEvents btnDelRM As System.Windows.Forms.Button
    Friend WithEvents btnAddRM As System.Windows.Forms.Button
    Friend WithEvents btnDelAM As System.Windows.Forms.Button
    Friend WithEvents btnAddAM As System.Windows.Forms.Button
    Friend WithEvents tabRemove As System.Windows.Forms.TabPage
    Friend WithEvents btnDelRemove As System.Windows.Forms.Button
    Friend WithEvents btnAddRemove As System.Windows.Forms.Button
    Friend WithEvents lstRemovePM As System.Windows.Forms.ListBox
    Friend WithEvents lstNewPM As System.Windows.Forms.ListBox
    Friend WithEvents lstUpdate As System.Windows.Forms.ListBox
    Friend WithEvents nudWeight As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudSpeed As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudSDef As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudSAtk As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudDef As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudAtk As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudHP As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudNewWeight As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudNewSpeed As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudNewSDef As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudNewSAtk As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudNewDef As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudNewAtk As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudNewHP As System.Windows.Forms.NumericUpDown
    Friend WithEvents txtNumber As System.Windows.Forms.TextBox
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents btnRemoveMove As System.Windows.Forms.Button
    Friend WithEvents btnAddMove As System.Windows.Forms.Button
    Friend WithEvents lblMoves As System.Windows.Forms.Label
    Friend WithEvents lstMoves As System.Windows.Forms.ListBox
    Friend WithEvents tabImage As System.Windows.Forms.TabPage
    Friend WithEvents lstImages As System.Windows.Forms.ListView
    Friend WithEvents ImageList As System.Windows.Forms.ImageList
    Friend WithEvents btnDelImg As System.Windows.Forms.Button
    Friend WithEvents btnAddImg As System.Windows.Forms.Button
    Friend WithEvents mnuCustom As System.Windows.Forms.MenuStrip
    Friend WithEvents 项目FToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 新建NToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 打开OToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents toolStripSeparator As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents 保存SToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 另存为AToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents toolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents 退出XToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents 生成数据BToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 更改项目名RToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
