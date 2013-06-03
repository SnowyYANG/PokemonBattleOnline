<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmExpertEditor
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
        Me.nudHPEV = New System.Windows.Forms.NumericUpDown
        Me.grpEV = New System.Windows.Forms.GroupBox
        Me.cmdClear = New System.Windows.Forms.Button
        Me.lblEVRest = New System.Windows.Forms.Label
        Me.lblSDefEV = New System.Windows.Forms.Label
        Me.nudSDefEV = New System.Windows.Forms.NumericUpDown
        Me.lblSAtkEV = New System.Windows.Forms.Label
        Me.nudSAtkEV = New System.Windows.Forms.NumericUpDown
        Me.lblSpeedEV = New System.Windows.Forms.Label
        Me.nudSpeedEV = New System.Windows.Forms.NumericUpDown
        Me.lblDefEV = New System.Windows.Forms.Label
        Me.nudDefEV = New System.Windows.Forms.NumericUpDown
        Me.lblAtkEV = New System.Windows.Forms.Label
        Me.nudAtkEV = New System.Windows.Forms.NumericUpDown
        Me.lblHPEV = New System.Windows.Forms.Label
        Me.lblEV = New System.Windows.Forms.Label
        Me.grpIV = New System.Windows.Forms.GroupBox
        Me.lblSDefIV = New System.Windows.Forms.Label
        Me.nudSDefIV = New System.Windows.Forms.NumericUpDown
        Me.lblSAtkIV = New System.Windows.Forms.Label
        Me.nudSAtkIV = New System.Windows.Forms.NumericUpDown
        Me.lblSpeedIV = New System.Windows.Forms.Label
        Me.nudSpeedIV = New System.Windows.Forms.NumericUpDown
        Me.lblDefIV = New System.Windows.Forms.Label
        Me.nudDefIV = New System.Windows.Forms.NumericUpDown
        Me.lblAtkIV = New System.Windows.Forms.Label
        Me.nudAtkIV = New System.Windows.Forms.NumericUpDown
        Me.lblHPIV = New System.Windows.Forms.Label
        Me.nudHPIV = New System.Windows.Forms.NumericUpDown
        Me.grpGender = New System.Windows.Forms.GroupBox
        Me.radNoGender = New System.Windows.Forms.RadioButton
        Me.radFemale = New System.Windows.Forms.RadioButton
        Me.radMale = New System.Windows.Forms.RadioButton
        Me.cboNature = New System.Windows.Forms.ComboBox
        Me.grpNature = New System.Windows.Forms.GroupBox
        Me.grpTrait = New System.Windows.Forms.GroupBox
        Me.radTrait2 = New System.Windows.Forms.RadioButton
        Me.radTrait1 = New System.Windows.Forms.RadioButton
        Me.lblSpeed = New System.Windows.Forms.Label
        Me.lblHP = New System.Windows.Forms.Label
        Me.lblSAtk = New System.Windows.Forms.Label
        Me.lblAtk = New System.Windows.Forms.Label
        Me.lblSDef = New System.Windows.Forms.Label
        Me.lblDef = New System.Windows.Forms.Label
        Me.lblSDefVal = New System.Windows.Forms.Label
        Me.lblSAtkVal = New System.Windows.Forms.Label
        Me.lblDefVal = New System.Windows.Forms.Label
        Me.lblAtkVal = New System.Windows.Forms.Label
        Me.lblSpeedVal = New System.Windows.Forms.Label
        Me.lblHPVal = New System.Windows.Forms.Label
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        CType(Me.nudHPEV, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpEV.SuspendLayout()
        CType(Me.nudSDefEV, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudSAtkEV, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudSpeedEV, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudDefEV, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudAtkEV, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpIV.SuspendLayout()
        CType(Me.nudSDefIV, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudSAtkIV, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudSpeedIV, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudDefIV, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudAtkIV, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudHPIV, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpGender.SuspendLayout()
        Me.grpNature.SuspendLayout()
        Me.grpTrait.SuspendLayout()
        Me.SuspendLayout()
        '
        'nudHPEV
        '
        Me.nudHPEV.Location = New System.Drawing.Point(41, 36)
        Me.nudHPEV.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me.nudHPEV.Name = "nudHPEV"
        Me.nudHPEV.Size = New System.Drawing.Size(55, 21)
        Me.nudHPEV.TabIndex = 1
        '
        'grpEV
        '
        Me.grpEV.Controls.Add(Me.cmdClear)
        Me.grpEV.Controls.Add(Me.lblEVRest)
        Me.grpEV.Controls.Add(Me.lblSDefEV)
        Me.grpEV.Controls.Add(Me.nudSDefEV)
        Me.grpEV.Controls.Add(Me.lblSAtkEV)
        Me.grpEV.Controls.Add(Me.nudSAtkEV)
        Me.grpEV.Controls.Add(Me.lblSpeedEV)
        Me.grpEV.Controls.Add(Me.nudSpeedEV)
        Me.grpEV.Controls.Add(Me.lblDefEV)
        Me.grpEV.Controls.Add(Me.nudDefEV)
        Me.grpEV.Controls.Add(Me.lblAtkEV)
        Me.grpEV.Controls.Add(Me.nudAtkEV)
        Me.grpEV.Controls.Add(Me.lblHPEV)
        Me.grpEV.Controls.Add(Me.lblEV)
        Me.grpEV.Controls.Add(Me.nudHPEV)
        Me.grpEV.Location = New System.Drawing.Point(166, 118)
        Me.grpEV.Name = "grpEV"
        Me.grpEV.Size = New System.Drawing.Size(361, 140)
        Me.grpEV.TabIndex = 1
        Me.grpEV.TabStop = False
        Me.grpEV.Text = "努力值分配"
        '
        'cmdClear
        '
        Me.cmdClear.Location = New System.Drawing.Point(280, 108)
        Me.cmdClear.Name = "cmdClear"
        Me.cmdClear.Size = New System.Drawing.Size(75, 23)
        Me.cmdClear.TabIndex = 7
        Me.cmdClear.Text = "清零(&L)"
        Me.cmdClear.UseVisualStyleBackColor = True
        '
        'lblEVRest
        '
        Me.lblEVRest.AutoSize = True
        Me.lblEVRest.Location = New System.Drawing.Point(78, 17)
        Me.lblEVRest.Name = "lblEVRest"
        Me.lblEVRest.Size = New System.Drawing.Size(0, 12)
        Me.lblEVRest.TabIndex = 14
        '
        'lblSDefEV
        '
        Me.lblSDefEV.AutoSize = True
        Me.lblSDefEV.Location = New System.Drawing.Point(265, 69)
        Me.lblSDefEV.Name = "lblSDefEV"
        Me.lblSDefEV.Size = New System.Drawing.Size(29, 12)
        Me.lblSDefEV.TabIndex = 13
        Me.lblSDefEV.Text = "特防"
        '
        'nudSDefEV
        '
        Me.nudSDefEV.Location = New System.Drawing.Point(300, 65)
        Me.nudSDefEV.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me.nudSDefEV.Name = "nudSDefEV"
        Me.nudSDefEV.Size = New System.Drawing.Size(55, 21)
        Me.nudSDefEV.TabIndex = 6
        '
        'lblSAtkEV
        '
        Me.lblSAtkEV.AutoSize = True
        Me.lblSAtkEV.Location = New System.Drawing.Point(135, 69)
        Me.lblSAtkEV.Name = "lblSAtkEV"
        Me.lblSAtkEV.Size = New System.Drawing.Size(29, 12)
        Me.lblSAtkEV.TabIndex = 11
        Me.lblSAtkEV.Text = "特攻"
        '
        'nudSAtkEV
        '
        Me.nudSAtkEV.Location = New System.Drawing.Point(170, 65)
        Me.nudSAtkEV.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me.nudSAtkEV.Name = "nudSAtkEV"
        Me.nudSAtkEV.Size = New System.Drawing.Size(55, 21)
        Me.nudSAtkEV.TabIndex = 4
        '
        'lblSpeedEV
        '
        Me.lblSpeedEV.AutoSize = True
        Me.lblSpeedEV.Location = New System.Drawing.Point(6, 69)
        Me.lblSpeedEV.Name = "lblSpeedEV"
        Me.lblSpeedEV.Size = New System.Drawing.Size(29, 12)
        Me.lblSpeedEV.TabIndex = 9
        Me.lblSpeedEV.Text = "速度"
        '
        'nudSpeedEV
        '
        Me.nudSpeedEV.Location = New System.Drawing.Point(41, 65)
        Me.nudSpeedEV.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me.nudSpeedEV.Name = "nudSpeedEV"
        Me.nudSpeedEV.Size = New System.Drawing.Size(55, 21)
        Me.nudSpeedEV.TabIndex = 2
        '
        'lblDefEV
        '
        Me.lblDefEV.AutoSize = True
        Me.lblDefEV.Location = New System.Drawing.Point(265, 40)
        Me.lblDefEV.Name = "lblDefEV"
        Me.lblDefEV.Size = New System.Drawing.Size(29, 12)
        Me.lblDefEV.TabIndex = 12
        Me.lblDefEV.Text = "物防"
        '
        'nudDefEV
        '
        Me.nudDefEV.Location = New System.Drawing.Point(300, 36)
        Me.nudDefEV.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me.nudDefEV.Name = "nudDefEV"
        Me.nudDefEV.Size = New System.Drawing.Size(55, 21)
        Me.nudDefEV.TabIndex = 5
        '
        'lblAtkEV
        '
        Me.lblAtkEV.AutoSize = True
        Me.lblAtkEV.Location = New System.Drawing.Point(135, 40)
        Me.lblAtkEV.Name = "lblAtkEV"
        Me.lblAtkEV.Size = New System.Drawing.Size(29, 12)
        Me.lblAtkEV.TabIndex = 10
        Me.lblAtkEV.Text = "物攻"
        '
        'nudAtkEV
        '
        Me.nudAtkEV.Location = New System.Drawing.Point(170, 36)
        Me.nudAtkEV.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me.nudAtkEV.Name = "nudAtkEV"
        Me.nudAtkEV.Size = New System.Drawing.Size(55, 21)
        Me.nudAtkEV.TabIndex = 3
        '
        'lblHPEV
        '
        Me.lblHPEV.AutoSize = True
        Me.lblHPEV.Location = New System.Drawing.Point(6, 40)
        Me.lblHPEV.Name = "lblHPEV"
        Me.lblHPEV.Size = New System.Drawing.Size(17, 12)
        Me.lblHPEV.TabIndex = 8
        Me.lblHPEV.Text = "HP"
        '
        'lblEV
        '
        Me.lblEV.AutoSize = True
        Me.lblEV.Location = New System.Drawing.Point(6, 17)
        Me.lblEV.Name = "lblEV"
        Me.lblEV.Size = New System.Drawing.Size(77, 12)
        Me.lblEV.TabIndex = 0
        Me.lblEV.Text = "剩余努力值："
        '
        'grpIV
        '
        Me.grpIV.Controls.Add(Me.lblSDefIV)
        Me.grpIV.Controls.Add(Me.nudSDefIV)
        Me.grpIV.Controls.Add(Me.lblSAtkIV)
        Me.grpIV.Controls.Add(Me.nudSAtkIV)
        Me.grpIV.Controls.Add(Me.lblSpeedIV)
        Me.grpIV.Controls.Add(Me.nudSpeedIV)
        Me.grpIV.Controls.Add(Me.lblDefIV)
        Me.grpIV.Controls.Add(Me.nudDefIV)
        Me.grpIV.Controls.Add(Me.lblAtkIV)
        Me.grpIV.Controls.Add(Me.nudAtkIV)
        Me.grpIV.Controls.Add(Me.lblHPIV)
        Me.grpIV.Controls.Add(Me.nudHPIV)
        Me.grpIV.Location = New System.Drawing.Point(166, 12)
        Me.grpIV.Name = "grpIV"
        Me.grpIV.Size = New System.Drawing.Size(361, 100)
        Me.grpIV.TabIndex = 0
        Me.grpIV.TabStop = False
        Me.grpIV.Text = "个体值"
        '
        'lblSDefIV
        '
        Me.lblSDefIV.AutoSize = True
        Me.lblSDefIV.Location = New System.Drawing.Point(265, 69)
        Me.lblSDefIV.Name = "lblSDefIV"
        Me.lblSDefIV.Size = New System.Drawing.Size(29, 12)
        Me.lblSDefIV.TabIndex = 11
        Me.lblSDefIV.Text = "特防"
        '
        'nudSDefIV
        '
        Me.nudSDefIV.Location = New System.Drawing.Point(300, 65)
        Me.nudSDefIV.Maximum = New Decimal(New Integer() {31, 0, 0, 0})
        Me.nudSDefIV.Name = "nudSDefIV"
        Me.nudSDefIV.Size = New System.Drawing.Size(55, 21)
        Me.nudSDefIV.TabIndex = 5
        '
        'lblSAtkIV
        '
        Me.lblSAtkIV.AutoSize = True
        Me.lblSAtkIV.Location = New System.Drawing.Point(135, 69)
        Me.lblSAtkIV.Name = "lblSAtkIV"
        Me.lblSAtkIV.Size = New System.Drawing.Size(29, 12)
        Me.lblSAtkIV.TabIndex = 9
        Me.lblSAtkIV.Text = "特攻"
        '
        'nudSAtkIV
        '
        Me.nudSAtkIV.Location = New System.Drawing.Point(170, 65)
        Me.nudSAtkIV.Maximum = New Decimal(New Integer() {31, 0, 0, 0})
        Me.nudSAtkIV.Name = "nudSAtkIV"
        Me.nudSAtkIV.Size = New System.Drawing.Size(55, 21)
        Me.nudSAtkIV.TabIndex = 3
        '
        'lblSpeedIV
        '
        Me.lblSpeedIV.AutoSize = True
        Me.lblSpeedIV.Location = New System.Drawing.Point(6, 69)
        Me.lblSpeedIV.Name = "lblSpeedIV"
        Me.lblSpeedIV.Size = New System.Drawing.Size(29, 12)
        Me.lblSpeedIV.TabIndex = 7
        Me.lblSpeedIV.Text = "速度"
        '
        'nudSpeedIV
        '
        Me.nudSpeedIV.Location = New System.Drawing.Point(41, 65)
        Me.nudSpeedIV.Maximum = New Decimal(New Integer() {31, 0, 0, 0})
        Me.nudSpeedIV.Name = "nudSpeedIV"
        Me.nudSpeedIV.Size = New System.Drawing.Size(55, 21)
        Me.nudSpeedIV.TabIndex = 1
        '
        'lblDefIV
        '
        Me.lblDefIV.AutoSize = True
        Me.lblDefIV.Location = New System.Drawing.Point(265, 40)
        Me.lblDefIV.Name = "lblDefIV"
        Me.lblDefIV.Size = New System.Drawing.Size(29, 12)
        Me.lblDefIV.TabIndex = 10
        Me.lblDefIV.Text = "物防"
        '
        'nudDefIV
        '
        Me.nudDefIV.Location = New System.Drawing.Point(300, 36)
        Me.nudDefIV.Maximum = New Decimal(New Integer() {31, 0, 0, 0})
        Me.nudDefIV.Name = "nudDefIV"
        Me.nudDefIV.Size = New System.Drawing.Size(55, 21)
        Me.nudDefIV.TabIndex = 4
        '
        'lblAtkIV
        '
        Me.lblAtkIV.AutoSize = True
        Me.lblAtkIV.Location = New System.Drawing.Point(135, 40)
        Me.lblAtkIV.Name = "lblAtkIV"
        Me.lblAtkIV.Size = New System.Drawing.Size(29, 12)
        Me.lblAtkIV.TabIndex = 8
        Me.lblAtkIV.Text = "物攻"
        '
        'nudAtkIV
        '
        Me.nudAtkIV.Location = New System.Drawing.Point(170, 36)
        Me.nudAtkIV.Maximum = New Decimal(New Integer() {31, 0, 0, 0})
        Me.nudAtkIV.Name = "nudAtkIV"
        Me.nudAtkIV.Size = New System.Drawing.Size(55, 21)
        Me.nudAtkIV.TabIndex = 2
        '
        'lblHPIV
        '
        Me.lblHPIV.AutoSize = True
        Me.lblHPIV.Location = New System.Drawing.Point(6, 40)
        Me.lblHPIV.Name = "lblHPIV"
        Me.lblHPIV.Size = New System.Drawing.Size(17, 12)
        Me.lblHPIV.TabIndex = 6
        Me.lblHPIV.Text = "HP"
        '
        'nudHPIV
        '
        Me.nudHPIV.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.nudHPIV.Location = New System.Drawing.Point(41, 36)
        Me.nudHPIV.Maximum = New Decimal(New Integer() {31, 0, 0, 0})
        Me.nudHPIV.Name = "nudHPIV"
        Me.nudHPIV.Size = New System.Drawing.Size(55, 21)
        Me.nudHPIV.TabIndex = 0
        '
        'grpGender
        '
        Me.grpGender.Controls.Add(Me.radNoGender)
        Me.grpGender.Controls.Add(Me.radFemale)
        Me.grpGender.Controls.Add(Me.radMale)
        Me.grpGender.Location = New System.Drawing.Point(464, 264)
        Me.grpGender.Name = "grpGender"
        Me.grpGender.Size = New System.Drawing.Size(63, 84)
        Me.grpGender.TabIndex = 4
        Me.grpGender.TabStop = False
        Me.grpGender.Text = "性别"
        '
        'radNoGender
        '
        Me.radNoGender.AutoSize = True
        Me.radNoGender.Location = New System.Drawing.Point(9, 62)
        Me.radNoGender.Name = "radNoGender"
        Me.radNoGender.Size = New System.Drawing.Size(35, 16)
        Me.radNoGender.TabIndex = 2
        Me.radNoGender.TabStop = True
        Me.radNoGender.Text = "无"
        Me.radNoGender.UseVisualStyleBackColor = True
        '
        'radFemale
        '
        Me.radFemale.AutoSize = True
        Me.radFemale.Location = New System.Drawing.Point(9, 42)
        Me.radFemale.Name = "radFemale"
        Me.radFemale.Size = New System.Drawing.Size(35, 16)
        Me.radFemale.TabIndex = 1
        Me.radFemale.TabStop = True
        Me.radFemale.Text = "母"
        Me.radFemale.UseVisualStyleBackColor = True
        '
        'radMale
        '
        Me.radMale.AutoSize = True
        Me.radMale.Location = New System.Drawing.Point(9, 20)
        Me.radMale.Name = "radMale"
        Me.radMale.Size = New System.Drawing.Size(35, 16)
        Me.radMale.TabIndex = 0
        Me.radMale.TabStop = True
        Me.radMale.Text = "公"
        Me.radMale.UseVisualStyleBackColor = True
        '
        'cboNature
        '
        Me.cboNature.FormattingEnabled = True
        Me.cboNature.Items.AddRange(New Object() {"实干     无影响", "孤僻     +物攻-物防", "勇敢     +物攻-速度", "固执     +物攻-特攻", "调皮     +物攻-特防", "大胆　　 +物防-物攻", "坦率　   无影响", "悠闲     +物防-速度", "淘气     +物防-特攻", "无虑     +物防-特防", "胆小     +速度-物攻", "急躁     +速度-物防", "认真     无影响", "开朗     +速度-特攻", "天真     +速度-特防", "保守     +特攻-物攻", "稳重     +特攻-物防", "冷静     +特攻-速度", "害羞     无影响", "马虎     +特攻-特防", "沉着     +特防-物攻", "温顺     +特防-物防", "狂妄     +特防-速度", "慎重     +特防-特攻", "浮躁     无影响"})
        Me.cboNature.Location = New System.Drawing.Point(6, 35)
        Me.cboNature.Name = "cboNature"
        Me.cboNature.Size = New System.Drawing.Size(152, 20)
        Me.cboNature.TabIndex = 0
        '
        'grpNature
        '
        Me.grpNature.Controls.Add(Me.cboNature)
        Me.grpNature.Location = New System.Drawing.Point(166, 264)
        Me.grpNature.Name = "grpNature"
        Me.grpNature.Size = New System.Drawing.Size(164, 84)
        Me.grpNature.TabIndex = 2
        Me.grpNature.TabStop = False
        Me.grpNature.Text = "性格"
        '
        'grpTrait
        '
        Me.grpTrait.Controls.Add(Me.radTrait2)
        Me.grpTrait.Controls.Add(Me.radTrait1)
        Me.grpTrait.Location = New System.Drawing.Point(336, 264)
        Me.grpTrait.Name = "grpTrait"
        Me.grpTrait.Size = New System.Drawing.Size(122, 84)
        Me.grpTrait.TabIndex = 3
        Me.grpTrait.TabStop = False
        Me.grpTrait.Text = "特性"
        '
        'radTrait2
        '
        Me.radTrait2.AutoSize = True
        Me.radTrait2.Location = New System.Drawing.Point(9, 51)
        Me.radTrait2.Name = "radTrait2"
        Me.radTrait2.Size = New System.Drawing.Size(35, 16)
        Me.radTrait2.TabIndex = 1
        Me.radTrait2.TabStop = True
        Me.radTrait2.Text = "No"
        Me.radTrait2.UseVisualStyleBackColor = True
        '
        'radTrait1
        '
        Me.radTrait1.AutoSize = True
        Me.radTrait1.Location = New System.Drawing.Point(9, 20)
        Me.radTrait1.Name = "radTrait1"
        Me.radTrait1.Size = New System.Drawing.Size(35, 16)
        Me.radTrait1.TabIndex = 0
        Me.radTrait1.TabStop = True
        Me.radTrait1.Text = "No"
        Me.radTrait1.UseVisualStyleBackColor = True
        '
        'lblSpeed
        '
        Me.lblSpeed.AutoSize = True
        Me.lblSpeed.Location = New System.Drawing.Point(12, 229)
        Me.lblSpeed.Name = "lblSpeed"
        Me.lblSpeed.Size = New System.Drawing.Size(41, 12)
        Me.lblSpeed.TabIndex = 12
        Me.lblSpeed.Text = "速度 :"
        '
        'lblHP
        '
        Me.lblHP.AutoSize = True
        Me.lblHP.Location = New System.Drawing.Point(12, 79)
        Me.lblHP.Name = "lblHP"
        Me.lblHP.Size = New System.Drawing.Size(41, 12)
        Me.lblHP.TabIndex = 7
        Me.lblHP.Text = "HP   :"
        '
        'lblSAtk
        '
        Me.lblSAtk.AutoSize = True
        Me.lblSAtk.Location = New System.Drawing.Point(12, 169)
        Me.lblSAtk.Name = "lblSAtk"
        Me.lblSAtk.Size = New System.Drawing.Size(41, 12)
        Me.lblSAtk.TabIndex = 10
        Me.lblSAtk.Text = "特攻 :"
        '
        'lblAtk
        '
        Me.lblAtk.AutoSize = True
        Me.lblAtk.Location = New System.Drawing.Point(12, 109)
        Me.lblAtk.Name = "lblAtk"
        Me.lblAtk.Size = New System.Drawing.Size(41, 12)
        Me.lblAtk.TabIndex = 8
        Me.lblAtk.Text = "物攻 :"
        '
        'lblSDef
        '
        Me.lblSDef.AutoSize = True
        Me.lblSDef.Location = New System.Drawing.Point(12, 199)
        Me.lblSDef.Name = "lblSDef"
        Me.lblSDef.Size = New System.Drawing.Size(41, 12)
        Me.lblSDef.TabIndex = 11
        Me.lblSDef.Text = "特防 :"
        '
        'lblDef
        '
        Me.lblDef.AutoSize = True
        Me.lblDef.Location = New System.Drawing.Point(12, 139)
        Me.lblDef.Name = "lblDef"
        Me.lblDef.Size = New System.Drawing.Size(41, 12)
        Me.lblDef.TabIndex = 9
        Me.lblDef.Text = "物防 :"
        '
        'lblSDefVal
        '
        Me.lblSDefVal.AutoSize = True
        Me.lblSDefVal.Location = New System.Drawing.Point(59, 201)
        Me.lblSDefVal.Name = "lblSDefVal"
        Me.lblSDefVal.Size = New System.Drawing.Size(0, 12)
        Me.lblSDefVal.TabIndex = 17
        '
        'lblSAtkVal
        '
        Me.lblSAtkVal.AutoSize = True
        Me.lblSAtkVal.Location = New System.Drawing.Point(59, 171)
        Me.lblSAtkVal.Name = "lblSAtkVal"
        Me.lblSAtkVal.Size = New System.Drawing.Size(0, 12)
        Me.lblSAtkVal.TabIndex = 16
        '
        'lblDefVal
        '
        Me.lblDefVal.AutoSize = True
        Me.lblDefVal.Location = New System.Drawing.Point(59, 141)
        Me.lblDefVal.Name = "lblDefVal"
        Me.lblDefVal.Size = New System.Drawing.Size(0, 12)
        Me.lblDefVal.TabIndex = 15
        '
        'lblAtkVal
        '
        Me.lblAtkVal.AutoSize = True
        Me.lblAtkVal.Location = New System.Drawing.Point(59, 111)
        Me.lblAtkVal.Name = "lblAtkVal"
        Me.lblAtkVal.Size = New System.Drawing.Size(0, 12)
        Me.lblAtkVal.TabIndex = 14
        '
        'lblSpeedVal
        '
        Me.lblSpeedVal.AutoSize = True
        Me.lblSpeedVal.Location = New System.Drawing.Point(59, 231)
        Me.lblSpeedVal.Name = "lblSpeedVal"
        Me.lblSpeedVal.Size = New System.Drawing.Size(0, 12)
        Me.lblSpeedVal.TabIndex = 18
        '
        'lblHPVal
        '
        Me.lblHPVal.AutoSize = True
        Me.lblHPVal.Location = New System.Drawing.Point(59, 81)
        Me.lblHPVal.Name = "lblHPVal"
        Me.lblHPVal.Size = New System.Drawing.Size(0, 12)
        Me.lblHPVal.TabIndex = 13
        '
        'cmdOK
        '
        Me.cmdOK.Location = New System.Drawing.Point(345, 361)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(75, 23)
        Me.cmdOK.TabIndex = 5
        Me.cmdOK.Text = "确定(&O)"
        Me.cmdOK.UseVisualStyleBackColor = True
        '
        'cmdCancel
        '
        Me.cmdCancel.Location = New System.Drawing.Point(452, 361)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(75, 23)
        Me.cmdCancel.TabIndex = 6
        Me.cmdCancel.Text = "取消(&C)"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'FrmExpertEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(539, 396)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.lblSDefVal)
        Me.Controls.Add(Me.lblSAtkVal)
        Me.Controls.Add(Me.lblDefVal)
        Me.Controls.Add(Me.lblAtkVal)
        Me.Controls.Add(Me.lblSpeedVal)
        Me.Controls.Add(Me.lblHPVal)
        Me.Controls.Add(Me.lblSDef)
        Me.Controls.Add(Me.lblSAtk)
        Me.Controls.Add(Me.lblDef)
        Me.Controls.Add(Me.lblAtk)
        Me.Controls.Add(Me.lblSpeed)
        Me.Controls.Add(Me.lblHP)
        Me.Controls.Add(Me.grpTrait)
        Me.Controls.Add(Me.grpNature)
        Me.Controls.Add(Me.grpGender)
        Me.Controls.Add(Me.grpIV)
        Me.Controls.Add(Me.grpEV)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "FrmExpertEditor"
        Me.ShowInTaskbar = False
        Me.Text = "详细设置"
        CType(Me.nudHPEV, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpEV.ResumeLayout(False)
        Me.grpEV.PerformLayout()
        CType(Me.nudSDefEV, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudSAtkEV, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudSpeedEV, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudDefEV, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudAtkEV, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpIV.ResumeLayout(False)
        Me.grpIV.PerformLayout()
        CType(Me.nudSDefIV, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudSAtkIV, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudSpeedIV, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudDefIV, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudAtkIV, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudHPIV, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpGender.ResumeLayout(False)
        Me.grpGender.PerformLayout()
        Me.grpNature.ResumeLayout(False)
        Me.grpTrait.ResumeLayout(False)
        Me.grpTrait.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents nudHPEV As System.Windows.Forms.NumericUpDown
    Friend WithEvents grpEV As System.Windows.Forms.GroupBox
    Friend WithEvents lblEV As System.Windows.Forms.Label
    Friend WithEvents lblHPEV As System.Windows.Forms.Label
    Friend WithEvents lblAtkEV As System.Windows.Forms.Label
    Friend WithEvents nudAtkEV As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblDefEV As System.Windows.Forms.Label
    Friend WithEvents nudDefEV As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblSpeedEV As System.Windows.Forms.Label
    Friend WithEvents nudSpeedEV As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblSDefEV As System.Windows.Forms.Label
    Friend WithEvents nudSDefEV As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblSAtkEV As System.Windows.Forms.Label
    Friend WithEvents nudSAtkEV As System.Windows.Forms.NumericUpDown
    Friend WithEvents grpIV As System.Windows.Forms.GroupBox
    Friend WithEvents lblSDefIV As System.Windows.Forms.Label
    Friend WithEvents nudSDefIV As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblSAtkIV As System.Windows.Forms.Label
    Friend WithEvents nudSAtkIV As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblSpeedIV As System.Windows.Forms.Label
    Friend WithEvents nudSpeedIV As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblDefIV As System.Windows.Forms.Label
    Friend WithEvents nudDefIV As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblAtkIV As System.Windows.Forms.Label
    Friend WithEvents nudAtkIV As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblHPIV As System.Windows.Forms.Label
    Friend WithEvents nudHPIV As System.Windows.Forms.NumericUpDown
    Friend WithEvents grpGender As System.Windows.Forms.GroupBox
    Friend WithEvents radFemale As System.Windows.Forms.RadioButton
    Friend WithEvents radMale As System.Windows.Forms.RadioButton
    Friend WithEvents cboNature As System.Windows.Forms.ComboBox
    Friend WithEvents grpNature As System.Windows.Forms.GroupBox
    Friend WithEvents grpTrait As System.Windows.Forms.GroupBox
    Friend WithEvents radTrait2 As System.Windows.Forms.RadioButton
    Friend WithEvents radTrait1 As System.Windows.Forms.RadioButton
    Friend WithEvents lblSpeed As System.Windows.Forms.Label
    Friend WithEvents lblHP As System.Windows.Forms.Label
    Friend WithEvents lblSAtk As System.Windows.Forms.Label
    Friend WithEvents lblAtk As System.Windows.Forms.Label
    Friend WithEvents lblSDef As System.Windows.Forms.Label
    Friend WithEvents lblDef As System.Windows.Forms.Label
    Friend WithEvents lblSDefVal As System.Windows.Forms.Label
    Friend WithEvents lblSAtkVal As System.Windows.Forms.Label
    Friend WithEvents lblDefVal As System.Windows.Forms.Label
    Friend WithEvents lblAtkVal As System.Windows.Forms.Label
    Friend WithEvents lblSpeedVal As System.Windows.Forms.Label
    Friend WithEvents lblHPVal As System.Windows.Forms.Label
    Friend WithEvents radNoGender As System.Windows.Forms.RadioButton
    Friend WithEvents lblEVRest As System.Windows.Forms.Label
    Friend WithEvents cmdClear As System.Windows.Forms.Button
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
End Class
