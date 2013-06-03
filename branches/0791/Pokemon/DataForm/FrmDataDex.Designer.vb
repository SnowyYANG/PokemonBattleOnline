<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmDataDex
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
        Me.grpImage = New System.Windows.Forms.GroupBox()
        Me.picBack2 = New System.Windows.Forms.PictureBox()
        Me.picBack1 = New System.Windows.Forms.PictureBox()
        Me.picFrame2 = New System.Windows.Forms.PictureBox()
        Me.picFrame1 = New System.Windows.Forms.PictureBox()
        Me.picFront2 = New System.Windows.Forms.PictureBox()
        Me.picFront1 = New System.Windows.Forms.PictureBox()
        Me.tabDexs = New System.Windows.Forms.TabControl()
        Me.tabPM = New System.Windows.Forms.TabPage()
        Me.cmdInherit = New System.Windows.Forms.Button()
        Me.treEvolution = New System.Windows.Forms.TreeView()
        Me.cmdChange = New System.Windows.Forms.Button()
        Me.cmdSearch = New System.Windows.Forms.Button()
        Me.cmdClose = New System.Windows.Forms.Button()
        Me.grpMoves = New System.Windows.Forms.GroupBox()
        Me.lstPMMoves = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.imgType = New System.Windows.Forms.ImageList(Me.components)
        Me.grpBase = New System.Windows.Forms.GroupBox()
        Me.picBase = New System.Windows.Forms.PictureBox()
        Me.grpInfo = New System.Windows.Forms.GroupBox()
        Me.lblCount = New System.Windows.Forms.Label()
        Me.lblEggGroup = New System.Windows.Forms.Label()
        Me.lblGender = New System.Windows.Forms.Label()
        Me.lblWeight = New System.Windows.Forms.Label()
        Me.lblTrait = New System.Windows.Forms.Label()
        Me.lblPMType = New System.Windows.Forms.Label()
        Me.lblPMName = New System.Windows.Forms.Label()
        Me.grpNo = New System.Windows.Forms.GroupBox()
        Me.nudNo = New System.Windows.Forms.NumericUpDown()
        Me.tabMoves = New System.Windows.Forms.TabPage()
        Me.grpItems = New System.Windows.Forms.GroupBox()
        Me.lstItems = New System.Windows.Forms.ListBox()
        Me.txtItemInfo = New System.Windows.Forms.TextBox()
        Me.txtItem = New System.Windows.Forms.TextBox()
        Me.grpTraits = New System.Windows.Forms.GroupBox()
        Me.lstTraits = New System.Windows.Forms.ListBox()
        Me.txtTraitInfo = New System.Windows.Forms.TextBox()
        Me.txtTrait = New System.Windows.Forms.TextBox()
        Me.grpMoveInfo = New System.Windows.Forms.GroupBox()
        Me.lblTarget = New System.Windows.Forms.Label()
        Me.txtInfo = New System.Windows.Forms.TextBox()
        Me.chkPuch = New System.Windows.Forms.CheckBox()
        Me.chkBP = New System.Windows.Forms.CheckBox()
        Me.chkKR = New System.Windows.Forms.CheckBox()
        Me.chkSound = New System.Windows.Forms.CheckBox()
        Me.chkContact = New System.Windows.Forms.CheckBox()
        Me.lblType = New System.Windows.Forms.Label()
        Me.lblPP = New System.Windows.Forms.Label()
        Me.lblMoveType = New System.Windows.Forms.Label()
        Me.lblAcc = New System.Windows.Forms.Label()
        Me.lblPower = New System.Windows.Forms.Label()
        Me.lblMoveName = New System.Windows.Forms.Label()
        Me.cmdSearchMove = New System.Windows.Forms.Button()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.lstMoves = New System.Windows.Forms.ListView()
        Me.CounterButton = New System.Windows.Forms.Button()
        Me.grpImage.SuspendLayout()
        CType(Me.picBack2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picBack1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picFrame2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picFrame1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picFront2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picFront1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabDexs.SuspendLayout()
        Me.tabPM.SuspendLayout()
        Me.grpMoves.SuspendLayout()
        Me.grpBase.SuspendLayout()
        CType(Me.picBase, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpInfo.SuspendLayout()
        Me.grpNo.SuspendLayout()
        CType(Me.nudNo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabMoves.SuspendLayout()
        Me.grpItems.SuspendLayout()
        Me.grpTraits.SuspendLayout()
        Me.grpMoveInfo.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpImage
        '
        Me.grpImage.Controls.Add(Me.picBack2)
        Me.grpImage.Controls.Add(Me.picBack1)
        Me.grpImage.Controls.Add(Me.picFrame2)
        Me.grpImage.Controls.Add(Me.picFrame1)
        Me.grpImage.Controls.Add(Me.picFront2)
        Me.grpImage.Controls.Add(Me.picFront1)
        Me.grpImage.Location = New System.Drawing.Point(8, 68)
        Me.grpImage.Name = "grpImage"
        Me.grpImage.Size = New System.Drawing.Size(303, 180)
        Me.grpImage.TabIndex = 7
        Me.grpImage.TabStop = False
        Me.grpImage.Text = "图象"
        '
        'picBack2
        '
        Me.picBack2.Location = New System.Drawing.Point(216, 95)
        Me.picBack2.Name = "picBack2"
        Me.picBack2.Size = New System.Drawing.Size(80, 80)
        Me.picBack2.TabIndex = 5
        Me.picBack2.TabStop = False
        '
        'picBack1
        '
        Me.picBack1.Location = New System.Drawing.Point(216, 15)
        Me.picBack1.Name = "picBack1"
        Me.picBack1.Size = New System.Drawing.Size(80, 80)
        Me.picBack1.TabIndex = 4
        Me.picBack1.TabStop = False
        '
        'picFrame2
        '
        Me.picFrame2.Location = New System.Drawing.Point(111, 95)
        Me.picFrame2.Name = "picFrame2"
        Me.picFrame2.Size = New System.Drawing.Size(80, 80)
        Me.picFrame2.TabIndex = 3
        Me.picFrame2.TabStop = False
        '
        'picFrame1
        '
        Me.picFrame1.Location = New System.Drawing.Point(111, 15)
        Me.picFrame1.Name = "picFrame1"
        Me.picFrame1.Size = New System.Drawing.Size(80, 80)
        Me.picFrame1.TabIndex = 2
        Me.picFrame1.TabStop = False
        '
        'picFront2
        '
        Me.picFront2.Location = New System.Drawing.Point(6, 95)
        Me.picFront2.Name = "picFront2"
        Me.picFront2.Size = New System.Drawing.Size(80, 80)
        Me.picFront2.TabIndex = 1
        Me.picFront2.TabStop = False
        '
        'picFront1
        '
        Me.picFront1.Location = New System.Drawing.Point(6, 15)
        Me.picFront1.Name = "picFront1"
        Me.picFront1.Size = New System.Drawing.Size(80, 80)
        Me.picFront1.TabIndex = 0
        Me.picFront1.TabStop = False
        '
        'tabDexs
        '
        Me.tabDexs.Controls.Add(Me.tabPM)
        Me.tabDexs.Controls.Add(Me.tabMoves)
        Me.tabDexs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabDexs.Location = New System.Drawing.Point(0, 0)
        Me.tabDexs.Name = "tabDexs"
        Me.tabDexs.SelectedIndex = 0
        Me.tabDexs.Size = New System.Drawing.Size(700, 484)
        Me.tabDexs.TabIndex = 0
        '
        'tabPM
        '
        Me.tabPM.Controls.Add(Me.CounterButton)
        Me.tabPM.Controls.Add(Me.cmdInherit)
        Me.tabPM.Controls.Add(Me.treEvolution)
        Me.tabPM.Controls.Add(Me.cmdChange)
        Me.tabPM.Controls.Add(Me.cmdSearch)
        Me.tabPM.Controls.Add(Me.cmdClose)
        Me.tabPM.Controls.Add(Me.grpMoves)
        Me.tabPM.Controls.Add(Me.grpBase)
        Me.tabPM.Controls.Add(Me.grpInfo)
        Me.tabPM.Controls.Add(Me.grpNo)
        Me.tabPM.Controls.Add(Me.grpImage)
        Me.tabPM.Location = New System.Drawing.Point(4, 22)
        Me.tabPM.Name = "tabPM"
        Me.tabPM.Padding = New System.Windows.Forms.Padding(3)
        Me.tabPM.Size = New System.Drawing.Size(692, 458)
        Me.tabPM.TabIndex = 0
        Me.tabPM.Text = "精灵"
        Me.tabPM.UseVisualStyleBackColor = True
        '
        'cmdInherit
        '
        Me.cmdInherit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdInherit.Location = New System.Drawing.Point(604, 391)
        Me.cmdInherit.Name = "cmdInherit"
        Me.cmdInherit.Size = New System.Drawing.Size(82, 27)
        Me.cmdInherit.TabIndex = 1
        Me.cmdInherit.Text = "遗传搜索(&I)"
        Me.cmdInherit.UseVisualStyleBackColor = True
        '
        'treEvolution
        '
        Me.treEvolution.Location = New System.Drawing.Point(479, 23)
        Me.treEvolution.Name = "treEvolution"
        Me.treEvolution.Size = New System.Drawing.Size(205, 197)
        Me.treEvolution.TabIndex = 5
        '
        'cmdChange
        '
        Me.cmdChange.Location = New System.Drawing.Point(206, 23)
        Me.cmdChange.Name = "cmdChange"
        Me.cmdChange.Size = New System.Drawing.Size(82, 27)
        Me.cmdChange.TabIndex = 4
        Me.cmdChange.Text = "改变形态(&H)"
        Me.cmdChange.UseVisualStyleBackColor = True
        Me.cmdChange.Visible = False
        '
        'cmdSearch
        '
        Me.cmdSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdSearch.Location = New System.Drawing.Point(604, 358)
        Me.cmdSearch.Name = "cmdSearch"
        Me.cmdSearch.Size = New System.Drawing.Size(82, 27)
        Me.cmdSearch.TabIndex = 0
        Me.cmdSearch.Text = "搜索(&S)"
        Me.cmdSearch.UseVisualStyleBackColor = True
        '
        'cmdClose
        '
        Me.cmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdClose.Location = New System.Drawing.Point(604, 424)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(82, 27)
        Me.cmdClose.TabIndex = 2
        Me.cmdClose.Text = "关闭(&C)"
        Me.cmdClose.UseVisualStyleBackColor = True
        '
        'grpMoves
        '
        Me.grpMoves.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpMoves.Controls.Add(Me.lstPMMoves)
        Me.grpMoves.Location = New System.Drawing.Point(348, 234)
        Me.grpMoves.Name = "grpMoves"
        Me.grpMoves.Size = New System.Drawing.Size(250, 210)
        Me.grpMoves.TabIndex = 8
        Me.grpMoves.TabStop = False
        Me.grpMoves.Text = "技能"
        '
        'lstPMMoves
        '
        Me.lstPMMoves.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3})
        Me.lstPMMoves.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstPMMoves.Location = New System.Drawing.Point(3, 17)
        Me.lstPMMoves.Name = "lstPMMoves"
        Me.lstPMMoves.Size = New System.Drawing.Size(244, 190)
        Me.lstPMMoves.SmallImageList = Me.imgType
        Me.lstPMMoves.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lstPMMoves.TabIndex = 0
        Me.lstPMMoves.UseCompatibleStateImageBehavior = False
        Me.lstPMMoves.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "名称"
        Me.ColumnHeader1.Width = 95
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "类型"
        Me.ColumnHeader2.Width = 45
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "习得途径"
        Me.ColumnHeader3.Width = 90
        '
        'imgType
        '
        Me.imgType.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
        Me.imgType.ImageSize = New System.Drawing.Size(35, 15)
        Me.imgType.TransparentColor = System.Drawing.Color.Transparent
        '
        'grpBase
        '
        Me.grpBase.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpBase.BackColor = System.Drawing.Color.Transparent
        Me.grpBase.Controls.Add(Me.picBase)
        Me.grpBase.Location = New System.Drawing.Point(11, 261)
        Me.grpBase.Name = "grpBase"
        Me.grpBase.Size = New System.Drawing.Size(331, 181)
        Me.grpBase.TabIndex = 8
        Me.grpBase.TabStop = False
        Me.grpBase.Text = "种族值"
        '
        'picBase
        '
        Me.picBase.BackColor = System.Drawing.Color.Transparent
        Me.picBase.Dock = System.Windows.Forms.DockStyle.Top
        Me.picBase.Location = New System.Drawing.Point(3, 17)
        Me.picBase.Name = "picBase"
        Me.picBase.Size = New System.Drawing.Size(325, 160)
        Me.picBase.TabIndex = 0
        Me.picBase.TabStop = False
        '
        'grpInfo
        '
        Me.grpInfo.Controls.Add(Me.lblCount)
        Me.grpInfo.Controls.Add(Me.lblEggGroup)
        Me.grpInfo.Controls.Add(Me.lblGender)
        Me.grpInfo.Controls.Add(Me.lblWeight)
        Me.grpInfo.Controls.Add(Me.lblTrait)
        Me.grpInfo.Controls.Add(Me.lblPMType)
        Me.grpInfo.Controls.Add(Me.lblPMName)
        Me.grpInfo.Location = New System.Drawing.Point(317, 23)
        Me.grpInfo.Name = "grpInfo"
        Me.grpInfo.Size = New System.Drawing.Size(156, 205)
        Me.grpInfo.TabIndex = 6
        Me.grpInfo.TabStop = False
        Me.grpInfo.Text = "详细信息"
        '
        'lblCount
        '
        Me.lblCount.AutoSize = True
        Me.lblCount.Location = New System.Drawing.Point(10, 179)
        Me.lblCount.Name = "lblCount"
        Me.lblCount.Size = New System.Drawing.Size(65, 12)
        Me.lblCount.TabIndex = 6
        Me.lblCount.Text = "对战统计 :"
        '
        'lblEggGroup
        '
        Me.lblEggGroup.AutoSize = True
        Me.lblEggGroup.Location = New System.Drawing.Point(10, 153)
        Me.lblEggGroup.Name = "lblEggGroup"
        Me.lblEggGroup.Size = New System.Drawing.Size(65, 12)
        Me.lblEggGroup.TabIndex = 5
        Me.lblEggGroup.Text = "生蛋组别 :"
        '
        'lblGender
        '
        Me.lblGender.AutoSize = True
        Me.lblGender.Location = New System.Drawing.Point(10, 127)
        Me.lblGender.Name = "lblGender"
        Me.lblGender.Size = New System.Drawing.Size(65, 12)
        Me.lblGender.TabIndex = 4
        Me.lblGender.Text = "性别限定 :"
        '
        'lblWeight
        '
        Me.lblWeight.AutoSize = True
        Me.lblWeight.Location = New System.Drawing.Point(10, 101)
        Me.lblWeight.Name = "lblWeight"
        Me.lblWeight.Size = New System.Drawing.Size(41, 12)
        Me.lblWeight.TabIndex = 3
        Me.lblWeight.Text = "体重 :"
        '
        'lblTrait
        '
        Me.lblTrait.AutoSize = True
        Me.lblTrait.Location = New System.Drawing.Point(12, 75)
        Me.lblTrait.Name = "lblTrait"
        Me.lblTrait.Size = New System.Drawing.Size(41, 12)
        Me.lblTrait.TabIndex = 2
        Me.lblTrait.Text = "特性 :"
        '
        'lblPMType
        '
        Me.lblPMType.AutoSize = True
        Me.lblPMType.Location = New System.Drawing.Point(10, 49)
        Me.lblPMType.Name = "lblPMType"
        Me.lblPMType.Size = New System.Drawing.Size(41, 12)
        Me.lblPMType.TabIndex = 1
        Me.lblPMType.Text = "属性 :"
        '
        'lblPMName
        '
        Me.lblPMName.AutoSize = True
        Me.lblPMName.Location = New System.Drawing.Point(10, 23)
        Me.lblPMName.Name = "lblPMName"
        Me.lblPMName.Size = New System.Drawing.Size(41, 12)
        Me.lblPMName.TabIndex = 0
        Me.lblPMName.Text = "名称 :"
        '
        'grpNo
        '
        Me.grpNo.Controls.Add(Me.nudNo)
        Me.grpNo.Location = New System.Drawing.Point(11, 6)
        Me.grpNo.Name = "grpNo"
        Me.grpNo.Size = New System.Drawing.Size(124, 50)
        Me.grpNo.TabIndex = 3
        Me.grpNo.TabStop = False
        Me.grpNo.Text = "编号"
        '
        'nudNo
        '
        Me.nudNo.Location = New System.Drawing.Point(10, 17)
        Me.nudNo.Maximum = New Decimal(New Integer() {493, 0, 0, 0})
        Me.nudNo.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudNo.Name = "nudNo"
        Me.nudNo.Size = New System.Drawing.Size(105, 21)
        Me.nudNo.TabIndex = 0
        Me.nudNo.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'tabMoves
        '
        Me.tabMoves.Controls.Add(Me.grpItems)
        Me.tabMoves.Controls.Add(Me.grpTraits)
        Me.tabMoves.Controls.Add(Me.grpMoveInfo)
        Me.tabMoves.Controls.Add(Me.cmdSearchMove)
        Me.tabMoves.Controls.Add(Me.txtSearch)
        Me.tabMoves.Controls.Add(Me.lstMoves)
        Me.tabMoves.Location = New System.Drawing.Point(4, 22)
        Me.tabMoves.Name = "tabMoves"
        Me.tabMoves.Padding = New System.Windows.Forms.Padding(3)
        Me.tabMoves.Size = New System.Drawing.Size(692, 458)
        Me.tabMoves.TabIndex = 1
        Me.tabMoves.Text = "技能"
        Me.tabMoves.UseVisualStyleBackColor = True
        '
        'grpItems
        '
        Me.grpItems.Controls.Add(Me.lstItems)
        Me.grpItems.Controls.Add(Me.txtItemInfo)
        Me.grpItems.Controls.Add(Me.txtItem)
        Me.grpItems.Location = New System.Drawing.Point(389, 227)
        Me.grpItems.Name = "grpItems"
        Me.grpItems.Size = New System.Drawing.Size(284, 205)
        Me.grpItems.TabIndex = 4
        Me.grpItems.TabStop = False
        Me.grpItems.Text = "道具"
        '
        'lstItems
        '
        Me.lstItems.FormattingEnabled = True
        Me.lstItems.ItemHeight = 12
        Me.lstItems.Location = New System.Drawing.Point(16, 45)
        Me.lstItems.Name = "lstItems"
        Me.lstItems.Size = New System.Drawing.Size(100, 148)
        Me.lstItems.Sorted = True
        Me.lstItems.TabIndex = 8
        '
        'txtItemInfo
        '
        Me.txtItemInfo.Location = New System.Drawing.Point(122, 18)
        Me.txtItemInfo.Multiline = True
        Me.txtItemInfo.Name = "txtItemInfo"
        Me.txtItemInfo.ReadOnly = True
        Me.txtItemInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtItemInfo.Size = New System.Drawing.Size(156, 181)
        Me.txtItemInfo.TabIndex = 9
        '
        'txtItem
        '
        Me.txtItem.ImeMode = System.Windows.Forms.ImeMode.Alpha
        Me.txtItem.Location = New System.Drawing.Point(16, 18)
        Me.txtItem.Name = "txtItem"
        Me.txtItem.Size = New System.Drawing.Size(100, 21)
        Me.txtItem.TabIndex = 7
        '
        'grpTraits
        '
        Me.grpTraits.Controls.Add(Me.lstTraits)
        Me.grpTraits.Controls.Add(Me.txtTraitInfo)
        Me.grpTraits.Controls.Add(Me.txtTrait)
        Me.grpTraits.Location = New System.Drawing.Point(389, 14)
        Me.grpTraits.Name = "grpTraits"
        Me.grpTraits.Size = New System.Drawing.Size(284, 207)
        Me.grpTraits.TabIndex = 3
        Me.grpTraits.TabStop = False
        Me.grpTraits.Text = "特性"
        '
        'lstTraits
        '
        Me.lstTraits.FormattingEnabled = True
        Me.lstTraits.ItemHeight = 12
        Me.lstTraits.Location = New System.Drawing.Point(16, 48)
        Me.lstTraits.Name = "lstTraits"
        Me.lstTraits.Size = New System.Drawing.Size(100, 148)
        Me.lstTraits.Sorted = True
        Me.lstTraits.TabIndex = 7
        '
        'txtTraitInfo
        '
        Me.txtTraitInfo.Location = New System.Drawing.Point(122, 20)
        Me.txtTraitInfo.Multiline = True
        Me.txtTraitInfo.Name = "txtTraitInfo"
        Me.txtTraitInfo.ReadOnly = True
        Me.txtTraitInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtTraitInfo.Size = New System.Drawing.Size(156, 181)
        Me.txtTraitInfo.TabIndex = 6
        '
        'txtTrait
        '
        Me.txtTrait.ImeMode = System.Windows.Forms.ImeMode.Alpha
        Me.txtTrait.Location = New System.Drawing.Point(16, 20)
        Me.txtTrait.Name = "txtTrait"
        Me.txtTrait.Size = New System.Drawing.Size(100, 21)
        Me.txtTrait.TabIndex = 1
        '
        'grpMoveInfo
        '
        Me.grpMoveInfo.Controls.Add(Me.lblTarget)
        Me.grpMoveInfo.Controls.Add(Me.txtInfo)
        Me.grpMoveInfo.Controls.Add(Me.chkPuch)
        Me.grpMoveInfo.Controls.Add(Me.chkBP)
        Me.grpMoveInfo.Controls.Add(Me.chkKR)
        Me.grpMoveInfo.Controls.Add(Me.chkSound)
        Me.grpMoveInfo.Controls.Add(Me.chkContact)
        Me.grpMoveInfo.Controls.Add(Me.lblType)
        Me.grpMoveInfo.Controls.Add(Me.lblPP)
        Me.grpMoveInfo.Controls.Add(Me.lblMoveType)
        Me.grpMoveInfo.Controls.Add(Me.lblAcc)
        Me.grpMoveInfo.Controls.Add(Me.lblPower)
        Me.grpMoveInfo.Controls.Add(Me.lblMoveName)
        Me.grpMoveInfo.Location = New System.Drawing.Point(159, 45)
        Me.grpMoveInfo.Name = "grpMoveInfo"
        Me.grpMoveInfo.Size = New System.Drawing.Size(172, 387)
        Me.grpMoveInfo.TabIndex = 1
        Me.grpMoveInfo.TabStop = False
        Me.grpMoveInfo.Text = "详细信息"
        '
        'lblTarget
        '
        Me.lblTarget.AutoSize = True
        Me.lblTarget.Location = New System.Drawing.Point(20, 196)
        Me.lblTarget.Name = "lblTarget"
        Me.lblTarget.Size = New System.Drawing.Size(41, 12)
        Me.lblTarget.TabIndex = 7
        Me.lblTarget.Text = "对象 :"
        '
        'txtInfo
        '
        Me.txtInfo.Location = New System.Drawing.Point(6, 309)
        Me.txtInfo.Multiline = True
        Me.txtInfo.Name = "txtInfo"
        Me.txtInfo.ReadOnly = True
        Me.txtInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtInfo.Size = New System.Drawing.Size(160, 72)
        Me.txtInfo.TabIndex = 0
        '
        'chkPuch
        '
        Me.chkPuch.AutoSize = True
        Me.chkPuch.Enabled = False
        Me.chkPuch.Location = New System.Drawing.Point(18, 265)
        Me.chkPuch.Name = "chkPuch"
        Me.chkPuch.Size = New System.Drawing.Size(72, 16)
        Me.chkPuch.TabIndex = 9
        Me.chkPuch.Text = "拳击攻击"
        Me.chkPuch.UseVisualStyleBackColor = True
        '
        'chkBP
        '
        Me.chkBP.AutoSize = True
        Me.chkBP.Enabled = False
        Me.chkBP.Location = New System.Drawing.Point(95, 287)
        Me.chkBP.Name = "chkBP"
        Me.chkBP.Size = New System.Drawing.Size(60, 16)
        Me.chkBP.TabIndex = 12
        Me.chkBP.Text = "可抢夺"
        Me.chkBP.UseVisualStyleBackColor = True
        '
        'chkKR
        '
        Me.chkKR.AutoSize = True
        Me.chkKR.Enabled = False
        Me.chkKR.Location = New System.Drawing.Point(18, 287)
        Me.chkKR.Name = "chkKR"
        Me.chkKR.Size = New System.Drawing.Size(72, 16)
        Me.chkKR.TabIndex = 10
        Me.chkKR.Text = "王者之证"
        Me.chkKR.UseVisualStyleBackColor = True
        '
        'chkSound
        '
        Me.chkSound.AutoSize = True
        Me.chkSound.Enabled = False
        Me.chkSound.Location = New System.Drawing.Point(95, 243)
        Me.chkSound.Name = "chkSound"
        Me.chkSound.Size = New System.Drawing.Size(72, 16)
        Me.chkSound.TabIndex = 11
        Me.chkSound.Text = "声音攻击"
        Me.chkSound.UseVisualStyleBackColor = True
        '
        'chkContact
        '
        Me.chkContact.AutoSize = True
        Me.chkContact.Enabled = False
        Me.chkContact.Location = New System.Drawing.Point(18, 243)
        Me.chkContact.Name = "chkContact"
        Me.chkContact.Size = New System.Drawing.Size(72, 16)
        Me.chkContact.TabIndex = 8
        Me.chkContact.Text = "接触攻击"
        Me.chkContact.UseVisualStyleBackColor = True
        '
        'lblType
        '
        Me.lblType.AutoSize = True
        Me.lblType.Location = New System.Drawing.Point(20, 56)
        Me.lblType.Name = "lblType"
        Me.lblType.Size = New System.Drawing.Size(41, 12)
        Me.lblType.TabIndex = 2
        Me.lblType.Text = "属性 :"
        '
        'lblPP
        '
        Me.lblPP.AutoSize = True
        Me.lblPP.Location = New System.Drawing.Point(20, 168)
        Me.lblPP.Name = "lblPP"
        Me.lblPP.Size = New System.Drawing.Size(29, 12)
        Me.lblPP.TabIndex = 6
        Me.lblPP.Text = "PP :"
        '
        'lblMoveType
        '
        Me.lblMoveType.AutoSize = True
        Me.lblMoveType.Location = New System.Drawing.Point(20, 140)
        Me.lblMoveType.Name = "lblMoveType"
        Me.lblMoveType.Size = New System.Drawing.Size(41, 12)
        Me.lblMoveType.TabIndex = 5
        Me.lblMoveType.Text = "类型 :"
        '
        'lblAcc
        '
        Me.lblAcc.AutoSize = True
        Me.lblAcc.Location = New System.Drawing.Point(20, 112)
        Me.lblAcc.Name = "lblAcc"
        Me.lblAcc.Size = New System.Drawing.Size(53, 12)
        Me.lblAcc.TabIndex = 4
        Me.lblAcc.Text = "命中率 :"
        '
        'lblPower
        '
        Me.lblPower.AutoSize = True
        Me.lblPower.Location = New System.Drawing.Point(20, 84)
        Me.lblPower.Name = "lblPower"
        Me.lblPower.Size = New System.Drawing.Size(41, 12)
        Me.lblPower.TabIndex = 3
        Me.lblPower.Text = "威力 :"
        '
        'lblMoveName
        '
        Me.lblMoveName.AutoSize = True
        Me.lblMoveName.Location = New System.Drawing.Point(20, 28)
        Me.lblMoveName.Name = "lblMoveName"
        Me.lblMoveName.Size = New System.Drawing.Size(41, 12)
        Me.lblMoveName.TabIndex = 1
        Me.lblMoveName.Text = "名称 :"
        '
        'cmdSearchMove
        '
        Me.cmdSearchMove.Location = New System.Drawing.Point(159, 16)
        Me.cmdSearchMove.Name = "cmdSearchMove"
        Me.cmdSearchMove.Size = New System.Drawing.Size(75, 23)
        Me.cmdSearchMove.TabIndex = 1
        Me.cmdSearchMove.Text = "查找"
        Me.cmdSearchMove.UseVisualStyleBackColor = True
        '
        'txtSearch
        '
        Me.txtSearch.ImeMode = System.Windows.Forms.ImeMode.Alpha
        Me.txtSearch.Location = New System.Drawing.Point(8, 16)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(145, 21)
        Me.txtSearch.TabIndex = 0
        '
        'lstMoves
        '
        Me.lstMoves.LargeImageList = Me.imgType
        Me.lstMoves.Location = New System.Drawing.Point(8, 43)
        Me.lstMoves.Name = "lstMoves"
        Me.lstMoves.Size = New System.Drawing.Size(145, 389)
        Me.lstMoves.SmallImageList = Me.imgType
        Me.lstMoves.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lstMoves.TabIndex = 2
        Me.lstMoves.UseCompatibleStateImageBehavior = False
        Me.lstMoves.View = System.Windows.Forms.View.SmallIcon
        '
        'CounterButton
        '
        Me.CounterButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CounterButton.Location = New System.Drawing.Point(604, 325)
        Me.CounterButton.Name = "CounterButton"
        Me.CounterButton.Size = New System.Drawing.Size(82, 27)
        Me.CounterButton.TabIndex = 9
        Me.CounterButton.Text = "导出统计(&E)"
        Me.CounterButton.UseVisualStyleBackColor = True
        '
        'FrmDataDex
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(700, 484)
        Me.Controls.Add(Me.tabDexs)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "FrmDataDex"
        Me.Text = "FrmPokeDex"
        Me.grpImage.ResumeLayout(False)
        CType(Me.picBack2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picBack1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picFrame2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picFrame1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picFront2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picFront1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabDexs.ResumeLayout(False)
        Me.tabPM.ResumeLayout(False)
        Me.grpMoves.ResumeLayout(False)
        Me.grpBase.ResumeLayout(False)
        CType(Me.picBase, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpInfo.ResumeLayout(False)
        Me.grpInfo.PerformLayout()
        Me.grpNo.ResumeLayout(False)
        CType(Me.nudNo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabMoves.ResumeLayout(False)
        Me.tabMoves.PerformLayout()
        Me.grpItems.ResumeLayout(False)
        Me.grpItems.PerformLayout()
        Me.grpTraits.ResumeLayout(False)
        Me.grpTraits.PerformLayout()
        Me.grpMoveInfo.ResumeLayout(False)
        Me.grpMoveInfo.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpImage As System.Windows.Forms.GroupBox
    Friend WithEvents tabDexs As System.Windows.Forms.TabControl
    Friend WithEvents tabPM As System.Windows.Forms.TabPage
    Friend WithEvents nudNo As System.Windows.Forms.NumericUpDown
    Friend WithEvents grpNo As System.Windows.Forms.GroupBox
    Friend WithEvents grpBase As System.Windows.Forms.GroupBox
    Friend WithEvents picBase As System.Windows.Forms.PictureBox
    Friend WithEvents grpMoves As System.Windows.Forms.GroupBox
    Friend WithEvents lstPMMoves As System.Windows.Forms.ListView
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents cmdSearch As System.Windows.Forms.Button
    Friend WithEvents imgType As System.Windows.Forms.ImageList
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents cmdChange As System.Windows.Forms.Button
    Friend WithEvents tabMoves As System.Windows.Forms.TabPage
    Friend WithEvents lstMoves As System.Windows.Forms.ListView
    Friend WithEvents txtSearch As System.Windows.Forms.TextBox
    Friend WithEvents cmdSearchMove As System.Windows.Forms.Button
    Friend WithEvents grpInfo As System.Windows.Forms.GroupBox
    Friend WithEvents lblGender As System.Windows.Forms.Label
    Friend WithEvents lblWeight As System.Windows.Forms.Label
    Friend WithEvents lblTrait As System.Windows.Forms.Label
    Friend WithEvents lblPMType As System.Windows.Forms.Label
    Friend WithEvents lblPMName As System.Windows.Forms.Label
    Friend WithEvents grpMoveInfo As System.Windows.Forms.GroupBox
    Friend WithEvents lblMoveType As System.Windows.Forms.Label
    Friend WithEvents lblAcc As System.Windows.Forms.Label
    Friend WithEvents lblPower As System.Windows.Forms.Label
    Friend WithEvents lblMoveName As System.Windows.Forms.Label
    Friend WithEvents lblPP As System.Windows.Forms.Label
    Friend WithEvents lblType As System.Windows.Forms.Label
    Friend WithEvents chkPuch As System.Windows.Forms.CheckBox
    Friend WithEvents chkBP As System.Windows.Forms.CheckBox
    Friend WithEvents chkKR As System.Windows.Forms.CheckBox
    Friend WithEvents chkSound As System.Windows.Forms.CheckBox
    Friend WithEvents chkContact As System.Windows.Forms.CheckBox
    Friend WithEvents txtInfo As System.Windows.Forms.TextBox
    Friend WithEvents lblEggGroup As System.Windows.Forms.Label
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents treEvolution As System.Windows.Forms.TreeView
    Friend WithEvents cmdInherit As System.Windows.Forms.Button
    Friend WithEvents lblTarget As System.Windows.Forms.Label
    Friend WithEvents picBack2 As System.Windows.Forms.PictureBox
    Friend WithEvents picBack1 As System.Windows.Forms.PictureBox
    Friend WithEvents picFrame2 As System.Windows.Forms.PictureBox
    Friend WithEvents picFrame1 As System.Windows.Forms.PictureBox
    Friend WithEvents picFront2 As System.Windows.Forms.PictureBox
    Friend WithEvents picFront1 As System.Windows.Forms.PictureBox
    Friend WithEvents grpItems As System.Windows.Forms.GroupBox
    Friend WithEvents txtItemInfo As System.Windows.Forms.TextBox
    Friend WithEvents txtItem As System.Windows.Forms.TextBox
    Friend WithEvents grpTraits As System.Windows.Forms.GroupBox
    Friend WithEvents txtTraitInfo As System.Windows.Forms.TextBox
    Friend WithEvents txtTrait As System.Windows.Forms.TextBox
    Friend WithEvents lstItems As System.Windows.Forms.ListBox
    Friend WithEvents lstTraits As System.Windows.Forms.ListBox
    Friend WithEvents lblCount As System.Windows.Forms.Label
    Friend WithEvents CounterButton As System.Windows.Forms.Button
End Class
