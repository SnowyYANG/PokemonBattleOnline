<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TeamEditorForm
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
        Dim staMoveInfo As System.Windows.Forms.StatusStrip
        Me.lblMoveInfo = New System.Windows.Forms.ToolStripStatusLabel()
        Me.imgPM = New System.Windows.Forms.ImageList(Me.components)
        Me.PokemonCombo = New System.Windows.Forms.ComboBox()
        Me.lblPM = New System.Windows.Forms.Label()
        Me.lblNickName = New System.Windows.Forms.Label()
        Me.NickNameText = New System.Windows.Forms.TextBox()
        Me.lblItem = New System.Windows.Forms.Label()
        Me.ItemCombo = New System.Windows.Forms.ComboBox()
        Me.MoveList = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.imgTypes = New System.Windows.Forms.ImageList(Me.components)
        Me.MoveText1 = New System.Windows.Forms.TextBox()
        Me.MoveText2 = New System.Windows.Forms.TextBox()
        Me.MoveText3 = New System.Windows.Forms.TextBox()
        Me.MoveText4 = New System.Windows.Forms.TextBox()
        Me.SwitchButton = New System.Windows.Forms.Button()
        Me.ExpertButton = New System.Windows.Forms.Button()
        Me.lblMove = New System.Windows.Forms.Label()
        Me.lblSDefVal = New System.Windows.Forms.Label()
        Me.lblSAtkVal = New System.Windows.Forms.Label()
        Me.lblDefVal = New System.Windows.Forms.Label()
        Me.lblAtkVal = New System.Windows.Forms.Label()
        Me.lblSpeedVal = New System.Windows.Forms.Label()
        Me.lblHPVal = New System.Windows.Forms.Label()
        Me.lblSDef = New System.Windows.Forms.Label()
        Me.lblSAtk = New System.Windows.Forms.Label()
        Me.lblDef = New System.Windows.Forms.Label()
        Me.lblAtk = New System.Windows.Forms.Label()
        Me.lblSpeed = New System.Windows.Forms.Label()
        Me.lblHP = New System.Windows.Forms.Label()
        Me.lblLV = New System.Windows.Forms.Label()
        Me.LVNumberic = New System.Windows.Forms.NumericUpDown()
        Me.PokemonList = New System.Windows.Forms.ListView()
        Me.cmdOpen = New System.Windows.Forms.Button()
        Me.cmdSave = New System.Windows.Forms.Button()
        Me.cmdSaveTo = New System.Windows.Forms.Button()
        Me.lblType1Name = New System.Windows.Forms.Label()
        Me.lblType1 = New System.Windows.Forms.Label()
        Me.lblType2Name = New System.Windows.Forms.Label()
        Me.lblType2 = New System.Windows.Forms.Label()
        Me.picPM = New System.Windows.Forms.PictureBox()
        Me.cmdNew = New System.Windows.Forms.Button()
        Me.cmdUp = New System.Windows.Forms.Button()
        Me.cmdDown = New System.Windows.Forms.Button()
        Me.grpSearch = New System.Windows.Forms.GroupBox()
        Me.lblSearchName = New System.Windows.Forms.Label()
        Me.SearchButton = New System.Windows.Forms.Button()
        Me.SearchText = New System.Windows.Forms.TextBox()
        Me.scTeam = New System.Windows.Forms.SplitContainer()
        Me.cmdBox = New System.Windows.Forms.Button()
        Me.lblBoxItem = New System.Windows.Forms.Label()
        Me.lblBoxMove1 = New System.Windows.Forms.Label()
        Me.lblBoxMove2 = New System.Windows.Forms.Label()
        Me.lblBoxMove3 = New System.Windows.Forms.Label()
        Me.lblBoxMove4 = New System.Windows.Forms.Label()
        Me.lblBoxName = New System.Windows.Forms.Label()
        Me.lblBoxHP = New System.Windows.Forms.Label()
        Me.lblBoxSp = New System.Windows.Forms.Label()
        Me.lblBoxAtk = New System.Windows.Forms.Label()
        Me.lblBoxDef = New System.Windows.Forms.Label()
        Me.lblBoxSAtk = New System.Windows.Forms.Label()
        Me.lblBoxSDef = New System.Windows.Forms.Label()
        Me.picBox = New System.Windows.Forms.PictureBox()
        Me.cmdRemove = New System.Windows.Forms.Button()
        Me.cmdOut = New System.Windows.Forms.Button()
        Me.cmdIn = New System.Windows.Forms.Button()
        Me.lstPMBox = New System.Windows.Forms.ListBox()
        Me.cboPMBox = New System.Windows.Forms.ComboBox()
        Me.PokemonTip = New System.Windows.Forms.ToolTip(Me.components)
        staMoveInfo = New System.Windows.Forms.StatusStrip()
        staMoveInfo.SuspendLayout()
        CType(Me.LVNumberic, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picPM, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpSearch.SuspendLayout()
        Me.scTeam.Panel1.SuspendLayout()
        Me.scTeam.Panel2.SuspendLayout()
        Me.scTeam.SuspendLayout()
        CType(Me.picBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'staMoveInfo
        '
        staMoveInfo.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblMoveInfo})
        staMoveInfo.Location = New System.Drawing.Point(0, 480)
        staMoveInfo.Name = "staMoveInfo"
        staMoveInfo.Size = New System.Drawing.Size(817, 22)
        staMoveInfo.TabIndex = 1
        staMoveInfo.Text = "staTeam"
        '
        'lblMoveInfo
        '
        Me.lblMoveInfo.Name = "lblMoveInfo"
        Me.lblMoveInfo.Size = New System.Drawing.Size(0, 17)
        '
        'imgPM
        '
        Me.imgPM.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
        Me.imgPM.ImageSize = New System.Drawing.Size(32, 32)
        Me.imgPM.TransparentColor = System.Drawing.Color.Transparent
        '
        'PokemonCombo
        '
        Me.PokemonCombo.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.PokemonCombo.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.PokemonCombo.Enabled = False
        Me.PokemonCombo.FormattingEnabled = True
        Me.PokemonCombo.Location = New System.Drawing.Point(152, 20)
        Me.PokemonCombo.Name = "PokemonCombo"
        Me.PokemonCombo.Size = New System.Drawing.Size(111, 20)
        Me.PokemonCombo.TabIndex = 3
        Me.PokemonCombo.Text = "(请选择)"
        Me.PokemonTip.SetToolTip(Me.PokemonCombo, "输入编号后按回车键可快速跳转")
        '
        'lblPM
        '
        Me.lblPM.AutoSize = True
        Me.lblPM.Location = New System.Drawing.Point(113, 23)
        Me.lblPM.Name = "lblPM"
        Me.lblPM.Size = New System.Drawing.Size(29, 12)
        Me.lblPM.TabIndex = 16
        Me.lblPM.Text = "种类"
        '
        'lblNickName
        '
        Me.lblNickName.AutoSize = True
        Me.lblNickName.Location = New System.Drawing.Point(105, 56)
        Me.lblNickName.Name = "lblNickName"
        Me.lblNickName.Size = New System.Drawing.Size(41, 12)
        Me.lblNickName.TabIndex = 17
        Me.lblNickName.Text = "自定名"
        '
        'NickNameText
        '
        Me.NickNameText.ImeMode = System.Windows.Forms.ImeMode.Alpha
        Me.NickNameText.Location = New System.Drawing.Point(152, 53)
        Me.NickNameText.MaxLength = 7
        Me.NickNameText.Name = "NickNameText"
        Me.NickNameText.Size = New System.Drawing.Size(111, 21)
        Me.NickNameText.TabIndex = 4
        '
        'lblItem
        '
        Me.lblItem.AutoSize = True
        Me.lblItem.Location = New System.Drawing.Point(113, 125)
        Me.lblItem.Name = "lblItem"
        Me.lblItem.Size = New System.Drawing.Size(29, 12)
        Me.lblItem.TabIndex = 19
        Me.lblItem.Text = "道具"
        '
        'ItemCombo
        '
        Me.ItemCombo.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.ItemCombo.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.ItemCombo.FormattingEnabled = True
        Me.ItemCombo.Items.AddRange(New Object() {"无", "果实1/解麻果", "果实2/醒睡果", "果实3/解毒果", "果实4/烧伤果", "果实5/解冻果", "果实6/PP果", "果实7", "果实8/混乱果", "果实9/奇迹果", "果实10", "果实11", "果实12", "果实13", "果实14", "果实15", "果实16", "果实17", "果实18", "果实19", "果实20", "果实21", "果实22", "果实23", "果实24", "果实25", "果实26", "果实27", "果实28", "果实29", "果实30", "果实31", "果实32", "果实33", "果实34", "果实35", "果实36", "果实37", "果实38", "果实39", "果实40", "果实41", "果实42", "果实43", "果实44", "果实45", "果实46", "果实47", "果实48", "果实49", "果实50", "果实51", "果实52", "果实53/物攻果", "果实54/物防果", "果实55/速度果", "果实56/特攻果", "果实57/特防果", "果实58/会心果", "果实59/随机果", "果实60", "果实61", "果实62", "果实63", "果实64", "格斗属性石板", "飞行属性石板", "毒属性石板", "地面属性石板", "岩属性石板", "虫属性石板", "鬼属性石板", "钢属性石板", "火属性石板", "水属性石板", "草属性石板", "电属性石板", "超能属性石板", "冰属性石板", "龙属性石板", "恶属性石板", "热石", "湿石", "沙石", "冰石", "黑色眼镜", "黑带", "银粉", "弯曲汤匙", "坚硬岩石", "丝巾", "毒针", "锐利鸟喙", "不融冰", "柔软沙子", "咒符", "磁铁", "神秘水滴", "奇迹种子", "金属外套", "木炭", "龙牙", "金刚石", "白玉", "速度珠", "电珠", "金属粉末", "幸运拳套", "心之水滴", "深海之牙", "深海之鳞", "粗骨棒", "长葱", "专爱围巾", "专爱头巾", "专爱眼镜", "力量头巾", "智慧眼镜", "达人腰带", "聚焦镜", "尖锐爪", "先制之爪", "后攻尾", "满腹香炉", "放大镜", "瞄准镜", "光粉", "生命玉", "光土", "镰刀爪", "力量草药", "节拍器", "黑铁球", "剩饭", "大树根", "白色香草", "空贝铃", "气息腰带", "气息头巾", "火珠", "剧毒珠", "黑色淤泥", "红色线团", "王者证明", "尖锐牙", "绮丽外壳", "附针", "竞争背心", "精神香草", "白金玉"})
        Me.ItemCombo.Location = New System.Drawing.Point(152, 121)
        Me.ItemCombo.Name = "ItemCombo"
        Me.ItemCombo.Size = New System.Drawing.Size(111, 20)
        Me.ItemCombo.TabIndex = 6
        Me.ItemCombo.Text = "无"
        '
        'MoveList
        '
        Me.MoveList.CheckBoxes = True
        Me.MoveList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5, Me.ColumnHeader6})
        Me.MoveList.FullRowSelect = True
        Me.MoveList.Location = New System.Drawing.Point(101, 191)
        Me.MoveList.MultiSelect = False
        Me.MoveList.Name = "MoveList"
        Me.MoveList.Size = New System.Drawing.Size(400, 216)
        Me.MoveList.SmallImageList = Me.imgTypes
        Me.MoveList.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.MoveList.TabIndex = 9
        Me.MoveList.UseCompatibleStateImageBehavior = False
        Me.MoveList.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "技巧"
        Me.ColumnHeader1.Width = 110
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "威力"
        Me.ColumnHeader2.Width = 45
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "PP"
        Me.ColumnHeader3.Width = 40
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "命中"
        Me.ColumnHeader4.Width = 45
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "类型"
        Me.ColumnHeader5.Width = 45
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "习得途径"
        Me.ColumnHeader6.Width = 85
        '
        'imgTypes
        '
        Me.imgTypes.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
        Me.imgTypes.ImageSize = New System.Drawing.Size(35, 15)
        Me.imgTypes.TransparentColor = System.Drawing.Color.Transparent
        '
        'MoveText1
        '
        Me.MoveText1.Location = New System.Drawing.Point(102, 413)
        Me.MoveText1.Name = "MoveText1"
        Me.MoveText1.ReadOnly = True
        Me.MoveText1.Size = New System.Drawing.Size(75, 21)
        Me.MoveText1.TabIndex = 21
        '
        'MoveText2
        '
        Me.MoveText2.Location = New System.Drawing.Point(210, 413)
        Me.MoveText2.Name = "MoveText2"
        Me.MoveText2.ReadOnly = True
        Me.MoveText2.Size = New System.Drawing.Size(75, 21)
        Me.MoveText2.TabIndex = 22
        '
        'MoveText3
        '
        Me.MoveText3.Location = New System.Drawing.Point(318, 413)
        Me.MoveText3.Name = "MoveText3"
        Me.MoveText3.ReadOnly = True
        Me.MoveText3.Size = New System.Drawing.Size(75, 21)
        Me.MoveText3.TabIndex = 23
        '
        'MoveText4
        '
        Me.MoveText4.Location = New System.Drawing.Point(426, 413)
        Me.MoveText4.Name = "MoveText4"
        Me.MoveText4.ReadOnly = True
        Me.MoveText4.Size = New System.Drawing.Size(75, 21)
        Me.MoveText4.TabIndex = 24
        '
        'SwitchButton
        '
        Me.SwitchButton.Enabled = False
        Me.SwitchButton.Location = New System.Drawing.Point(269, 22)
        Me.SwitchButton.Name = "SwitchButton"
        Me.SwitchButton.Size = New System.Drawing.Size(76, 23)
        Me.SwitchButton.TabIndex = 7
        Me.SwitchButton.Text = "选择(&W)"
        Me.SwitchButton.UseVisualStyleBackColor = True
        '
        'ExpertButton
        '
        Me.ExpertButton.Location = New System.Drawing.Point(269, 52)
        Me.ExpertButton.Name = "ExpertButton"
        Me.ExpertButton.Size = New System.Drawing.Size(99, 28)
        Me.ExpertButton.TabIndex = 8
        Me.ExpertButton.Text = "详细设置(&M)"
        Me.ExpertButton.UseVisualStyleBackColor = True
        '
        'lblMove
        '
        Me.lblMove.AutoSize = True
        Me.lblMove.Font = New System.Drawing.Font("宋体", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblMove.Location = New System.Drawing.Point(101, 176)
        Me.lblMove.Name = "lblMove"
        Me.lblMove.Size = New System.Drawing.Size(38, 12)
        Me.lblMove.TabIndex = 20
        Me.lblMove.Text = "技能:"
        '
        'lblSDefVal
        '
        Me.lblSDefVal.AutoSize = True
        Me.lblSDefVal.Location = New System.Drawing.Point(445, 118)
        Me.lblSDefVal.Name = "lblSDefVal"
        Me.lblSDefVal.Size = New System.Drawing.Size(0, 12)
        Me.lblSDefVal.TabIndex = 35
        '
        'lblSAtkVal
        '
        Me.lblSAtkVal.AutoSize = True
        Me.lblSAtkVal.Location = New System.Drawing.Point(445, 94)
        Me.lblSAtkVal.Name = "lblSAtkVal"
        Me.lblSAtkVal.Size = New System.Drawing.Size(0, 12)
        Me.lblSAtkVal.TabIndex = 34
        '
        'lblDefVal
        '
        Me.lblDefVal.AutoSize = True
        Me.lblDefVal.Location = New System.Drawing.Point(445, 70)
        Me.lblDefVal.Name = "lblDefVal"
        Me.lblDefVal.Size = New System.Drawing.Size(0, 12)
        Me.lblDefVal.TabIndex = 33
        '
        'lblAtkVal
        '
        Me.lblAtkVal.AutoSize = True
        Me.lblAtkVal.Location = New System.Drawing.Point(445, 46)
        Me.lblAtkVal.Name = "lblAtkVal"
        Me.lblAtkVal.Size = New System.Drawing.Size(0, 12)
        Me.lblAtkVal.TabIndex = 32
        '
        'lblSpeedVal
        '
        Me.lblSpeedVal.AutoSize = True
        Me.lblSpeedVal.Location = New System.Drawing.Point(445, 143)
        Me.lblSpeedVal.Name = "lblSpeedVal"
        Me.lblSpeedVal.Size = New System.Drawing.Size(0, 12)
        Me.lblSpeedVal.TabIndex = 36
        '
        'lblHPVal
        '
        Me.lblHPVal.AutoSize = True
        Me.lblHPVal.Location = New System.Drawing.Point(445, 22)
        Me.lblHPVal.Name = "lblHPVal"
        Me.lblHPVal.Size = New System.Drawing.Size(0, 12)
        Me.lblHPVal.TabIndex = 31
        '
        'lblSDef
        '
        Me.lblSDef.AutoSize = True
        Me.lblSDef.Location = New System.Drawing.Point(398, 118)
        Me.lblSDef.Name = "lblSDef"
        Me.lblSDef.Size = New System.Drawing.Size(41, 12)
        Me.lblSDef.TabIndex = 29
        Me.lblSDef.Text = "特防 :"
        '
        'lblSAtk
        '
        Me.lblSAtk.AutoSize = True
        Me.lblSAtk.Location = New System.Drawing.Point(398, 94)
        Me.lblSAtk.Name = "lblSAtk"
        Me.lblSAtk.Size = New System.Drawing.Size(41, 12)
        Me.lblSAtk.TabIndex = 28
        Me.lblSAtk.Text = "特攻 :"
        '
        'lblDef
        '
        Me.lblDef.AutoSize = True
        Me.lblDef.Location = New System.Drawing.Point(398, 70)
        Me.lblDef.Name = "lblDef"
        Me.lblDef.Size = New System.Drawing.Size(41, 12)
        Me.lblDef.TabIndex = 27
        Me.lblDef.Text = "物防 :"
        '
        'lblAtk
        '
        Me.lblAtk.AutoSize = True
        Me.lblAtk.Location = New System.Drawing.Point(398, 46)
        Me.lblAtk.Name = "lblAtk"
        Me.lblAtk.Size = New System.Drawing.Size(41, 12)
        Me.lblAtk.TabIndex = 26
        Me.lblAtk.Text = "物攻 :"
        '
        'lblSpeed
        '
        Me.lblSpeed.AutoSize = True
        Me.lblSpeed.Location = New System.Drawing.Point(398, 143)
        Me.lblSpeed.Name = "lblSpeed"
        Me.lblSpeed.Size = New System.Drawing.Size(41, 12)
        Me.lblSpeed.TabIndex = 30
        Me.lblSpeed.Text = "速度 :"
        '
        'lblHP
        '
        Me.lblHP.AutoSize = True
        Me.lblHP.Location = New System.Drawing.Point(398, 22)
        Me.lblHP.Name = "lblHP"
        Me.lblHP.Size = New System.Drawing.Size(41, 12)
        Me.lblHP.TabIndex = 25
        Me.lblHP.Text = "HP   :"
        '
        'lblLV
        '
        Me.lblLV.AutoSize = True
        Me.lblLV.Location = New System.Drawing.Point(122, 91)
        Me.lblLV.Name = "lblLV"
        Me.lblLV.Size = New System.Drawing.Size(17, 12)
        Me.lblLV.TabIndex = 18
        Me.lblLV.Text = "LV"
        '
        'LVNumberic
        '
        Me.LVNumberic.Location = New System.Drawing.Point(152, 87)
        Me.LVNumberic.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.LVNumberic.Name = "LVNumberic"
        Me.LVNumberic.Size = New System.Drawing.Size(60, 21)
        Me.LVNumberic.TabIndex = 5
        Me.LVNumberic.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'PokemonList
        '
        Me.PokemonList.LargeImageList = Me.imgPM
        Me.PokemonList.Location = New System.Drawing.Point(1, 1)
        Me.PokemonList.MultiSelect = False
        Me.PokemonList.Name = "PokemonList"
        Me.PokemonList.Size = New System.Drawing.Size(80, 445)
        Me.PokemonList.SmallImageList = Me.imgPM
        Me.PokemonList.TabIndex = 0
        Me.PokemonList.UseCompatibleStateImageBehavior = False
        '
        'cmdOpen
        '
        Me.cmdOpen.Location = New System.Drawing.Point(216, 440)
        Me.cmdOpen.Name = "cmdOpen"
        Me.cmdOpen.Size = New System.Drawing.Size(75, 35)
        Me.cmdOpen.TabIndex = 11
        Me.cmdOpen.Text = "打开(O)"
        Me.cmdOpen.UseVisualStyleBackColor = True
        '
        'cmdSave
        '
        Me.cmdSave.Location = New System.Drawing.Point(323, 440)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.Size = New System.Drawing.Size(75, 35)
        Me.cmdSave.TabIndex = 12
        Me.cmdSave.Text = "保存(S)"
        Me.cmdSave.UseVisualStyleBackColor = True
        '
        'cmdSaveTo
        '
        Me.cmdSaveTo.Location = New System.Drawing.Point(430, 440)
        Me.cmdSaveTo.Name = "cmdSaveTo"
        Me.cmdSaveTo.Size = New System.Drawing.Size(75, 35)
        Me.cmdSaveTo.TabIndex = 13
        Me.cmdSaveTo.Text = "另存为(A)."
        Me.cmdSaveTo.UseVisualStyleBackColor = True
        '
        'lblType1Name
        '
        Me.lblType1Name.AutoSize = True
        Me.lblType1Name.Location = New System.Drawing.Point(601, 125)
        Me.lblType1Name.Name = "lblType1Name"
        Me.lblType1Name.Size = New System.Drawing.Size(0, 12)
        Me.lblType1Name.TabIndex = 39
        '
        'lblType1
        '
        Me.lblType1.AutoSize = True
        Me.lblType1.Location = New System.Drawing.Point(528, 125)
        Me.lblType1.Name = "lblType1"
        Me.lblType1.Size = New System.Drawing.Size(59, 12)
        Me.lblType1.TabIndex = 37
        Me.lblType1.Text = "第一属性:"
        '
        'lblType2Name
        '
        Me.lblType2Name.AutoSize = True
        Me.lblType2Name.Location = New System.Drawing.Point(601, 143)
        Me.lblType2Name.Name = "lblType2Name"
        Me.lblType2Name.Size = New System.Drawing.Size(0, 12)
        Me.lblType2Name.TabIndex = 40
        '
        'lblType2
        '
        Me.lblType2.AutoSize = True
        Me.lblType2.Location = New System.Drawing.Point(528, 143)
        Me.lblType2.Name = "lblType2"
        Me.lblType2.Size = New System.Drawing.Size(59, 12)
        Me.lblType2.TabIndex = 38
        Me.lblType2.Text = "第二属性:"
        '
        'picPM
        '
        Me.picPM.Location = New System.Drawing.Point(530, 22)
        Me.picPM.Name = "picPM"
        Me.picPM.Size = New System.Drawing.Size(80, 80)
        Me.picPM.TabIndex = 45
        Me.picPM.TabStop = False
        '
        'cmdNew
        '
        Me.cmdNew.Location = New System.Drawing.Point(109, 440)
        Me.cmdNew.Name = "cmdNew"
        Me.cmdNew.Size = New System.Drawing.Size(75, 35)
        Me.cmdNew.TabIndex = 10
        Me.cmdNew.Text = "新建(&N)"
        Me.cmdNew.UseVisualStyleBackColor = True
        '
        'cmdUp
        '
        Me.cmdUp.Enabled = False
        Me.cmdUp.Location = New System.Drawing.Point(2, 454)
        Me.cmdUp.Name = "cmdUp"
        Me.cmdUp.Size = New System.Drawing.Size(37, 23)
        Me.cmdUp.TabIndex = 1
        Me.cmdUp.Text = "↑"
        Me.cmdUp.UseVisualStyleBackColor = True
        '
        'cmdDown
        '
        Me.cmdDown.Enabled = False
        Me.cmdDown.Location = New System.Drawing.Point(45, 454)
        Me.cmdDown.Name = "cmdDown"
        Me.cmdDown.Size = New System.Drawing.Size(37, 23)
        Me.cmdDown.TabIndex = 2
        Me.cmdDown.Text = "↓"
        Me.cmdDown.UseVisualStyleBackColor = True
        '
        'grpSearch
        '
        Me.grpSearch.Controls.Add(Me.lblSearchName)
        Me.grpSearch.Controls.Add(Me.SearchButton)
        Me.grpSearch.Controls.Add(Me.SearchText)
        Me.grpSearch.Location = New System.Drawing.Point(507, 191)
        Me.grpSearch.Name = "grpSearch"
        Me.grpSearch.Size = New System.Drawing.Size(121, 114)
        Me.grpSearch.TabIndex = 15
        Me.grpSearch.TabStop = False
        Me.grpSearch.Text = "搜索技能"
        '
        'lblSearchName
        '
        Me.lblSearchName.AutoSize = True
        Me.lblSearchName.Location = New System.Drawing.Point(7, 21)
        Me.lblSearchName.Name = "lblSearchName"
        Me.lblSearchName.Size = New System.Drawing.Size(29, 12)
        Me.lblSearchName.TabIndex = 2
        Me.lblSearchName.Text = "名称"
        '
        'SearchButton
        '
        Me.SearchButton.Location = New System.Drawing.Point(23, 70)
        Me.SearchButton.Name = "SearchButton"
        Me.SearchButton.Size = New System.Drawing.Size(75, 23)
        Me.SearchButton.TabIndex = 1
        Me.SearchButton.Text = "搜索(&E)"
        Me.SearchButton.UseVisualStyleBackColor = True
        '
        'SearchText
        '
        Me.SearchText.ImeMode = System.Windows.Forms.ImeMode.Alpha
        Me.SearchText.Location = New System.Drawing.Point(10, 43)
        Me.SearchText.Name = "SearchText"
        Me.SearchText.Size = New System.Drawing.Size(100, 21)
        Me.SearchText.TabIndex = 0
        '
        'scTeam
        '
        Me.scTeam.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scTeam.IsSplitterFixed = True
        Me.scTeam.Location = New System.Drawing.Point(0, 0)
        Me.scTeam.Name = "scTeam"
        '
        'scTeam.Panel1
        '
        Me.scTeam.Panel1.Controls.Add(Me.cmdBox)
        Me.scTeam.Panel1.Controls.Add(Me.PokemonList)
        Me.scTeam.Panel1.Controls.Add(Me.PokemonCombo)
        Me.scTeam.Panel1.Controls.Add(Me.grpSearch)
        Me.scTeam.Panel1.Controls.Add(Me.lblPM)
        Me.scTeam.Panel1.Controls.Add(Me.cmdDown)
        Me.scTeam.Panel1.Controls.Add(Me.lblNickName)
        Me.scTeam.Panel1.Controls.Add(Me.cmdUp)
        Me.scTeam.Panel1.Controls.Add(Me.NickNameText)
        Me.scTeam.Panel1.Controls.Add(Me.cmdNew)
        Me.scTeam.Panel1.Controls.Add(Me.lblItem)
        Me.scTeam.Panel1.Controls.Add(Me.picPM)
        Me.scTeam.Panel1.Controls.Add(Me.ItemCombo)
        Me.scTeam.Panel1.Controls.Add(Me.lblType2Name)
        Me.scTeam.Panel1.Controls.Add(Me.MoveList)
        Me.scTeam.Panel1.Controls.Add(Me.lblType2)
        Me.scTeam.Panel1.Controls.Add(Me.MoveText1)
        Me.scTeam.Panel1.Controls.Add(Me.lblType1Name)
        Me.scTeam.Panel1.Controls.Add(Me.MoveText2)
        Me.scTeam.Panel1.Controls.Add(Me.lblType1)
        Me.scTeam.Panel1.Controls.Add(Me.MoveText3)
        Me.scTeam.Panel1.Controls.Add(Me.cmdSaveTo)
        Me.scTeam.Panel1.Controls.Add(Me.MoveText4)
        Me.scTeam.Panel1.Controls.Add(Me.cmdSave)
        Me.scTeam.Panel1.Controls.Add(Me.SwitchButton)
        Me.scTeam.Panel1.Controls.Add(Me.cmdOpen)
        Me.scTeam.Panel1.Controls.Add(Me.ExpertButton)
        Me.scTeam.Panel1.Controls.Add(Me.lblMove)
        Me.scTeam.Panel1.Controls.Add(Me.LVNumberic)
        Me.scTeam.Panel1.Controls.Add(Me.lblHP)
        Me.scTeam.Panel1.Controls.Add(Me.lblLV)
        Me.scTeam.Panel1.Controls.Add(Me.lblSpeed)
        Me.scTeam.Panel1.Controls.Add(Me.lblSDefVal)
        Me.scTeam.Panel1.Controls.Add(Me.lblAtk)
        Me.scTeam.Panel1.Controls.Add(Me.lblSAtkVal)
        Me.scTeam.Panel1.Controls.Add(Me.lblDef)
        Me.scTeam.Panel1.Controls.Add(Me.lblDefVal)
        Me.scTeam.Panel1.Controls.Add(Me.lblSAtk)
        Me.scTeam.Panel1.Controls.Add(Me.lblAtkVal)
        Me.scTeam.Panel1.Controls.Add(Me.lblSDef)
        Me.scTeam.Panel1.Controls.Add(Me.lblSpeedVal)
        Me.scTeam.Panel1.Controls.Add(Me.lblHPVal)
        '
        'scTeam.Panel2
        '
        Me.scTeam.Panel2.Controls.Add(Me.lblBoxItem)
        Me.scTeam.Panel2.Controls.Add(Me.lblBoxMove1)
        Me.scTeam.Panel2.Controls.Add(Me.lblBoxMove2)
        Me.scTeam.Panel2.Controls.Add(Me.lblBoxMove3)
        Me.scTeam.Panel2.Controls.Add(Me.lblBoxMove4)
        Me.scTeam.Panel2.Controls.Add(Me.lblBoxName)
        Me.scTeam.Panel2.Controls.Add(Me.lblBoxHP)
        Me.scTeam.Panel2.Controls.Add(Me.lblBoxSp)
        Me.scTeam.Panel2.Controls.Add(Me.lblBoxAtk)
        Me.scTeam.Panel2.Controls.Add(Me.lblBoxDef)
        Me.scTeam.Panel2.Controls.Add(Me.lblBoxSAtk)
        Me.scTeam.Panel2.Controls.Add(Me.lblBoxSDef)
        Me.scTeam.Panel2.Controls.Add(Me.picBox)
        Me.scTeam.Panel2.Controls.Add(Me.cmdRemove)
        Me.scTeam.Panel2.Controls.Add(Me.cmdOut)
        Me.scTeam.Panel2.Controls.Add(Me.cmdIn)
        Me.scTeam.Panel2.Controls.Add(Me.lstPMBox)
        Me.scTeam.Panel2.Controls.Add(Me.cboPMBox)
        Me.scTeam.Size = New System.Drawing.Size(817, 502)
        Me.scTeam.SplitterDistance = 637
        Me.scTeam.TabIndex = 0
        '
        'cmdBox
        '
        Me.cmdBox.Location = New System.Drawing.Point(537, 440)
        Me.cmdBox.Name = "cmdBox"
        Me.cmdBox.Size = New System.Drawing.Size(75, 35)
        Me.cmdBox.TabIndex = 14
        Me.cmdBox.Text = "PM箱子(&B)"
        Me.cmdBox.UseVisualStyleBackColor = True
        '
        'lblBoxItem
        '
        Me.lblBoxItem.AutoSize = True
        Me.lblBoxItem.Location = New System.Drawing.Point(46, 374)
        Me.lblBoxItem.Name = "lblBoxItem"
        Me.lblBoxItem.Size = New System.Drawing.Size(0, 12)
        Me.lblBoxItem.TabIndex = 6
        '
        'lblBoxMove1
        '
        Me.lblBoxMove1.AutoSize = True
        Me.lblBoxMove1.Location = New System.Drawing.Point(5, 444)
        Me.lblBoxMove1.Name = "lblBoxMove1"
        Me.lblBoxMove1.Size = New System.Drawing.Size(0, 12)
        Me.lblBoxMove1.TabIndex = 10
        '
        'lblBoxMove2
        '
        Me.lblBoxMove2.AutoSize = True
        Me.lblBoxMove2.Location = New System.Drawing.Point(88, 444)
        Me.lblBoxMove2.Name = "lblBoxMove2"
        Me.lblBoxMove2.Size = New System.Drawing.Size(0, 12)
        Me.lblBoxMove2.TabIndex = 15
        '
        'lblBoxMove3
        '
        Me.lblBoxMove3.AutoSize = True
        Me.lblBoxMove3.Location = New System.Drawing.Point(5, 456)
        Me.lblBoxMove3.Name = "lblBoxMove3"
        Me.lblBoxMove3.Size = New System.Drawing.Size(0, 12)
        Me.lblBoxMove3.TabIndex = 11
        '
        'lblBoxMove4
        '
        Me.lblBoxMove4.AutoSize = True
        Me.lblBoxMove4.Location = New System.Drawing.Point(88, 456)
        Me.lblBoxMove4.Name = "lblBoxMove4"
        Me.lblBoxMove4.Size = New System.Drawing.Size(0, 12)
        Me.lblBoxMove4.TabIndex = 16
        '
        'lblBoxName
        '
        Me.lblBoxName.AutoSize = True
        Me.lblBoxName.Location = New System.Drawing.Point(46, 355)
        Me.lblBoxName.Name = "lblBoxName"
        Me.lblBoxName.Size = New System.Drawing.Size(0, 12)
        Me.lblBoxName.TabIndex = 5
        '
        'lblBoxHP
        '
        Me.lblBoxHP.AutoSize = True
        Me.lblBoxHP.Location = New System.Drawing.Point(5, 389)
        Me.lblBoxHP.Name = "lblBoxHP"
        Me.lblBoxHP.Size = New System.Drawing.Size(0, 12)
        Me.lblBoxHP.TabIndex = 7
        '
        'lblBoxSp
        '
        Me.lblBoxSp.AutoSize = True
        Me.lblBoxSp.Location = New System.Drawing.Point(88, 389)
        Me.lblBoxSp.Name = "lblBoxSp"
        Me.lblBoxSp.Size = New System.Drawing.Size(0, 12)
        Me.lblBoxSp.TabIndex = 12
        '
        'lblBoxAtk
        '
        Me.lblBoxAtk.AutoSize = True
        Me.lblBoxAtk.Location = New System.Drawing.Point(5, 401)
        Me.lblBoxAtk.Name = "lblBoxAtk"
        Me.lblBoxAtk.Size = New System.Drawing.Size(0, 12)
        Me.lblBoxAtk.TabIndex = 8
        '
        'lblBoxDef
        '
        Me.lblBoxDef.AutoSize = True
        Me.lblBoxDef.Location = New System.Drawing.Point(88, 401)
        Me.lblBoxDef.Name = "lblBoxDef"
        Me.lblBoxDef.Size = New System.Drawing.Size(0, 12)
        Me.lblBoxDef.TabIndex = 13
        '
        'lblBoxSAtk
        '
        Me.lblBoxSAtk.AutoSize = True
        Me.lblBoxSAtk.Location = New System.Drawing.Point(5, 413)
        Me.lblBoxSAtk.Name = "lblBoxSAtk"
        Me.lblBoxSAtk.Size = New System.Drawing.Size(0, 12)
        Me.lblBoxSAtk.TabIndex = 9
        '
        'lblBoxSDef
        '
        Me.lblBoxSDef.AutoSize = True
        Me.lblBoxSDef.Location = New System.Drawing.Point(88, 413)
        Me.lblBoxSDef.Name = "lblBoxSDef"
        Me.lblBoxSDef.Size = New System.Drawing.Size(0, 12)
        Me.lblBoxSDef.TabIndex = 14
        '
        'picBox
        '
        Me.picBox.Location = New System.Drawing.Point(7, 354)
        Me.picBox.Name = "picBox"
        Me.picBox.Size = New System.Drawing.Size(32, 32)
        Me.picBox.TabIndex = 11
        Me.picBox.TabStop = False
        '
        'cmdRemove
        '
        Me.cmdRemove.Enabled = False
        Me.cmdRemove.Font = New System.Drawing.Font("宋体", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.cmdRemove.Location = New System.Drawing.Point(8, 138)
        Me.cmdRemove.Name = "cmdRemove"
        Me.cmdRemove.Size = New System.Drawing.Size(29, 23)
        Me.cmdRemove.TabIndex = 2
        Me.cmdRemove.Text = "×"
        Me.cmdRemove.UseVisualStyleBackColor = True
        '
        'cmdOut
        '
        Me.cmdOut.Enabled = False
        Me.cmdOut.Location = New System.Drawing.Point(8, 91)
        Me.cmdOut.Name = "cmdOut"
        Me.cmdOut.Size = New System.Drawing.Size(29, 23)
        Me.cmdOut.TabIndex = 1
        Me.cmdOut.Text = "←"
        Me.cmdOut.UseVisualStyleBackColor = True
        '
        'cmdIn
        '
        Me.cmdIn.Enabled = False
        Me.cmdIn.Location = New System.Drawing.Point(8, 44)
        Me.cmdIn.Name = "cmdIn"
        Me.cmdIn.Size = New System.Drawing.Size(29, 23)
        Me.cmdIn.TabIndex = 0
        Me.cmdIn.Text = "→"
        Me.cmdIn.UseVisualStyleBackColor = True
        '
        'lstPMBox
        '
        Me.lstPMBox.HorizontalScrollbar = True
        Me.lstPMBox.ItemHeight = 12
        Me.lstPMBox.Location = New System.Drawing.Point(43, 44)
        Me.lstPMBox.Name = "lstPMBox"
        Me.lstPMBox.Size = New System.Drawing.Size(120, 292)
        Me.lstPMBox.TabIndex = 4
        '
        'cboPMBox
        '
        Me.cboPMBox.FormattingEnabled = True
        Me.cboPMBox.Items.AddRange(New Object() {"PM箱子1", "PM箱子2", "PM箱子3", "PM箱子4", "PM箱子5", "PM箱子6", "PM箱子7", "PM箱子8", "PM箱子9", "PM箱子10"})
        Me.cboPMBox.Location = New System.Drawing.Point(42, 14)
        Me.cboPMBox.Name = "cboPMBox"
        Me.cboPMBox.Size = New System.Drawing.Size(121, 20)
        Me.cboPMBox.TabIndex = 3
        '
        'TeamEditorForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(817, 502)
        Me.Controls.Add(staMoveInfo)
        Me.Controls.Add(Me.scTeam)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "TeamEditorForm"
        Me.Text = "FrmTeamEditor"
        staMoveInfo.ResumeLayout(False)
        staMoveInfo.PerformLayout()
        CType(Me.LVNumberic, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picPM, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpSearch.ResumeLayout(False)
        Me.grpSearch.PerformLayout()
        Me.scTeam.Panel1.ResumeLayout(False)
        Me.scTeam.Panel1.PerformLayout()
        Me.scTeam.Panel2.ResumeLayout(False)
        Me.scTeam.Panel2.PerformLayout()
        Me.scTeam.ResumeLayout(False)
        CType(Me.picBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents imgPM As System.Windows.Forms.ImageList
    Friend WithEvents PokemonCombo As System.Windows.Forms.ComboBox
    Friend WithEvents lblPM As System.Windows.Forms.Label
    Friend WithEvents lblNickName As System.Windows.Forms.Label
    Friend WithEvents NickNameText As System.Windows.Forms.TextBox
    Friend WithEvents lblItem As System.Windows.Forms.Label
    Friend WithEvents ItemCombo As System.Windows.Forms.ComboBox
    Friend WithEvents MoveList As System.Windows.Forms.ListView
    Friend WithEvents MoveText1 As System.Windows.Forms.TextBox
    Friend WithEvents imgTypes As System.Windows.Forms.ImageList
    Friend WithEvents MoveText2 As System.Windows.Forms.TextBox
    Friend WithEvents MoveText3 As System.Windows.Forms.TextBox
    Friend WithEvents MoveText4 As System.Windows.Forms.TextBox
    Friend WithEvents SwitchButton As System.Windows.Forms.Button
    Friend WithEvents ExpertButton As System.Windows.Forms.Button
    Friend WithEvents lblMove As System.Windows.Forms.Label
    Friend WithEvents lblSDefVal As System.Windows.Forms.Label
    Friend WithEvents lblSAtkVal As System.Windows.Forms.Label
    Friend WithEvents lblDefVal As System.Windows.Forms.Label
    Friend WithEvents lblAtkVal As System.Windows.Forms.Label
    Friend WithEvents lblSpeedVal As System.Windows.Forms.Label
    Friend WithEvents lblHPVal As System.Windows.Forms.Label
    Friend WithEvents lblSDef As System.Windows.Forms.Label
    Friend WithEvents lblSAtk As System.Windows.Forms.Label
    Friend WithEvents lblDef As System.Windows.Forms.Label
    Friend WithEvents lblAtk As System.Windows.Forms.Label
    Friend WithEvents lblSpeed As System.Windows.Forms.Label
    Friend WithEvents lblHP As System.Windows.Forms.Label
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents lblLV As System.Windows.Forms.Label
    Friend WithEvents LVNumberic As System.Windows.Forms.NumericUpDown
    Friend WithEvents PokemonList As System.Windows.Forms.ListView
    Friend WithEvents cmdOpen As System.Windows.Forms.Button
    Friend WithEvents cmdSave As System.Windows.Forms.Button
    Friend WithEvents cmdSaveTo As System.Windows.Forms.Button
    Friend WithEvents lblType1Name As System.Windows.Forms.Label
    Friend WithEvents lblType1 As System.Windows.Forms.Label
    Friend WithEvents lblType2Name As System.Windows.Forms.Label
    Friend WithEvents lblType2 As System.Windows.Forms.Label
    Friend WithEvents picPM As System.Windows.Forms.PictureBox
    Friend WithEvents cmdNew As System.Windows.Forms.Button
    Friend WithEvents cmdUp As System.Windows.Forms.Button
    Friend WithEvents cmdDown As System.Windows.Forms.Button
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents grpSearch As System.Windows.Forms.GroupBox
    Friend WithEvents lblSearchName As System.Windows.Forms.Label
    Friend WithEvents SearchButton As System.Windows.Forms.Button
    Friend WithEvents SearchText As System.Windows.Forms.TextBox
    Friend WithEvents lblMoveInfo As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents scTeam As System.Windows.Forms.SplitContainer
    Friend WithEvents cmdBox As System.Windows.Forms.Button
    Friend WithEvents cmdOut As System.Windows.Forms.Button
    Friend WithEvents cmdIn As System.Windows.Forms.Button
    Friend WithEvents lstPMBox As System.Windows.Forms.ListBox
    Friend WithEvents cboPMBox As System.Windows.Forms.ComboBox
    Friend WithEvents cmdRemove As System.Windows.Forms.Button
    Friend WithEvents picBox As System.Windows.Forms.PictureBox
    Friend WithEvents lblBoxName As System.Windows.Forms.Label
    Friend WithEvents lblBoxHP As System.Windows.Forms.Label
    Friend WithEvents lblBoxSp As System.Windows.Forms.Label
    Friend WithEvents lblBoxAtk As System.Windows.Forms.Label
    Friend WithEvents lblBoxDef As System.Windows.Forms.Label
    Friend WithEvents lblBoxSAtk As System.Windows.Forms.Label
    Friend WithEvents lblBoxSDef As System.Windows.Forms.Label
    Friend WithEvents lblBoxMove1 As System.Windows.Forms.Label
    Friend WithEvents lblBoxMove2 As System.Windows.Forms.Label
    Friend WithEvents lblBoxMove3 As System.Windows.Forms.Label
    Friend WithEvents lblBoxMove4 As System.Windows.Forms.Label
    Friend WithEvents lblBoxItem As System.Windows.Forms.Label
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents PokemonTip As System.Windows.Forms.ToolTip
End Class
