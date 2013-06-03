namespace PokemonBattle.RoomServer
{
    partial class RoomSettingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.NameLabel = new System.Windows.Forms.Label();
            this.NameText = new System.Windows.Forms.TextBox();
            this.OK_Button = new System.Windows.Forms.Button();
            this.Cancel_Button = new System.Windows.Forms.Button();
            this.DescText = new System.Windows.Forms.TextBox();
            this.DescLabel = new System.Windows.Forms.Label();
            this.WelcomeText = new System.Windows.Forms.TextBox();
            this.WelcomeLabel = new System.Windows.Forms.Label();
            this.MaxUserLabel = new System.Windows.Forms.Label();
            this.MaxUserNumberic = new System.Windows.Forms.NumericUpDown();
            this.LogonListCheck = new System.Windows.Forms.CheckBox();
            this.BattleGroup = new System.Windows.Forms.GroupBox();
            this.TimeNumberic = new System.Windows.Forms.NumericUpDown();
            this.TimeoutLabel = new System.Windows.Forms.Label();
            this.RandomCheck = new System.Windows.Forms.CheckBox();
            this.FourPlayerCheck = new System.Windows.Forms.CheckBox();
            this.DoubleCheck = new System.Windows.Forms.CheckBox();
            this.SingleCheck = new System.Windows.Forms.CheckBox();
            this.VersionText = new System.Windows.Forms.TextBox();
            this.VersionLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.MaxUserNumberic)).BeginInit();
            this.BattleGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TimeNumberic)).BeginInit();
            this.SuspendLayout();
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Location = new System.Drawing.Point(20, 12);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(41, 12);
            this.NameLabel.TabIndex = 0;
            this.NameLabel.Text = "名称 :";
            // 
            // NameText
            // 
            this.NameText.Location = new System.Drawing.Point(22, 27);
            this.NameText.MaxLength = 16;
            this.NameText.Name = "NameText";
            this.NameText.Size = new System.Drawing.Size(100, 21);
            this.NameText.TabIndex = 2;
            this.NameText.TextChanged += new System.EventHandler(this.NameText_TextChanged);
            // 
            // OK_Button
            // 
            this.OK_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OK_Button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK_Button.Location = new System.Drawing.Point(143, 355);
            this.OK_Button.Name = "OK_Button";
            this.OK_Button.Size = new System.Drawing.Size(75, 23);
            this.OK_Button.TabIndex = 0;
            this.OK_Button.Text = "确定(&O)";
            this.OK_Button.UseVisualStyleBackColor = true;
            this.OK_Button.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // Cancel_Button
            // 
            this.Cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel_Button.Location = new System.Drawing.Point(224, 355);
            this.Cancel_Button.Name = "Cancel_Button";
            this.Cancel_Button.Size = new System.Drawing.Size(75, 23);
            this.Cancel_Button.TabIndex = 1;
            this.Cancel_Button.Text = "取消(&A)";
            this.Cancel_Button.UseVisualStyleBackColor = true;
            // 
            // DescText
            // 
            this.DescText.Location = new System.Drawing.Point(22, 66);
            this.DescText.MaxLength = 200;
            this.DescText.Multiline = true;
            this.DescText.Name = "DescText";
            this.DescText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.DescText.Size = new System.Drawing.Size(277, 38);
            this.DescText.TabIndex = 3;
            // 
            // DescLabel
            // 
            this.DescLabel.AutoSize = true;
            this.DescLabel.Location = new System.Drawing.Point(20, 51);
            this.DescLabel.Name = "DescLabel";
            this.DescLabel.Size = new System.Drawing.Size(41, 12);
            this.DescLabel.TabIndex = 4;
            this.DescLabel.Text = "简介 :";
            // 
            // WelcomeText
            // 
            this.WelcomeText.Location = new System.Drawing.Point(22, 122);
            this.WelcomeText.MaxLength = 300;
            this.WelcomeText.Multiline = true;
            this.WelcomeText.Name = "WelcomeText";
            this.WelcomeText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.WelcomeText.Size = new System.Drawing.Size(277, 38);
            this.WelcomeText.TabIndex = 4;
            // 
            // WelcomeLabel
            // 
            this.WelcomeLabel.AutoSize = true;
            this.WelcomeLabel.Location = new System.Drawing.Point(20, 107);
            this.WelcomeLabel.Name = "WelcomeLabel";
            this.WelcomeLabel.Size = new System.Drawing.Size(65, 12);
            this.WelcomeLabel.TabIndex = 6;
            this.WelcomeLabel.Text = "欢迎信息 :";
            // 
            // MaxUserLabel
            // 
            this.MaxUserLabel.AutoSize = true;
            this.MaxUserLabel.Location = new System.Drawing.Point(19, 170);
            this.MaxUserLabel.Name = "MaxUserLabel";
            this.MaxUserLabel.Size = new System.Drawing.Size(65, 12);
            this.MaxUserLabel.TabIndex = 8;
            this.MaxUserLabel.Text = "用户上限 :";
            // 
            // MaxUserNumberic
            // 
            this.MaxUserNumberic.Location = new System.Drawing.Point(90, 166);
            this.MaxUserNumberic.Maximum = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.MaxUserNumberic.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.MaxUserNumberic.Name = "MaxUserNumberic";
            this.MaxUserNumberic.Size = new System.Drawing.Size(50, 21);
            this.MaxUserNumberic.TabIndex = 5;
            this.MaxUserNumberic.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // LogonListCheck
            // 
            this.LogonListCheck.AutoSize = true;
            this.LogonListCheck.Location = new System.Drawing.Point(194, 169);
            this.LogonListCheck.Name = "LogonListCheck";
            this.LogonListCheck.Size = new System.Drawing.Size(96, 16);
            this.LogonListCheck.TabIndex = 6;
            this.LogonListCheck.Text = "登录房间列表";
            this.LogonListCheck.UseVisualStyleBackColor = true;
            // 
            // BattleGroup
            // 
            this.BattleGroup.Controls.Add(this.VersionText);
            this.BattleGroup.Controls.Add(this.TimeNumberic);
            this.BattleGroup.Controls.Add(this.VersionLabel);
            this.BattleGroup.Controls.Add(this.TimeoutLabel);
            this.BattleGroup.Controls.Add(this.RandomCheck);
            this.BattleGroup.Controls.Add(this.FourPlayerCheck);
            this.BattleGroup.Controls.Add(this.DoubleCheck);
            this.BattleGroup.Controls.Add(this.SingleCheck);
            this.BattleGroup.Location = new System.Drawing.Point(22, 193);
            this.BattleGroup.Name = "BattleGroup";
            this.BattleGroup.Size = new System.Drawing.Size(277, 156);
            this.BattleGroup.TabIndex = 9;
            this.BattleGroup.TabStop = false;
            this.BattleGroup.Text = "对战设置";
            // 
            // TimeNumberic
            // 
            this.TimeNumberic.Location = new System.Drawing.Point(111, 78);
            this.TimeNumberic.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.TimeNumberic.Minimum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.TimeNumberic.Name = "TimeNumberic";
            this.TimeNumberic.Size = new System.Drawing.Size(60, 21);
            this.TimeNumberic.TabIndex = 10;
            this.TimeNumberic.Value = new decimal(new int[] {
            120,
            0,
            0,
            0});
            // 
            // TimeoutLabel
            // 
            this.TimeoutLabel.AutoSize = true;
            this.TimeoutLabel.Location = new System.Drawing.Point(16, 82);
            this.TimeoutLabel.Name = "TimeoutLabel";
            this.TimeoutLabel.Size = new System.Drawing.Size(89, 12);
            this.TimeoutLabel.TabIndex = 4;
            this.TimeoutLabel.Text = "超时设定(秒) :";
            // 
            // RandomCheck
            // 
            this.RandomCheck.AutoSize = true;
            this.RandomCheck.Location = new System.Drawing.Point(18, 51);
            this.RandomCheck.Name = "RandomCheck";
            this.RandomCheck.Size = new System.Drawing.Size(72, 16);
            this.RandomCheck.TabIndex = 3;
            this.RandomCheck.Text = "随机限定";
            this.RandomCheck.UseVisualStyleBackColor = true;
            // 
            // FourPlayerCheck
            // 
            this.FourPlayerCheck.AutoSize = true;
            this.FourPlayerCheck.Location = new System.Drawing.Point(189, 20);
            this.FourPlayerCheck.Name = "FourPlayerCheck";
            this.FourPlayerCheck.Size = new System.Drawing.Size(60, 16);
            this.FourPlayerCheck.TabIndex = 2;
            this.FourPlayerCheck.Text = "4P禁止";
            this.FourPlayerCheck.UseVisualStyleBackColor = true;
            // 
            // DoubleCheck
            // 
            this.DoubleCheck.AutoSize = true;
            this.DoubleCheck.Location = new System.Drawing.Point(102, 20);
            this.DoubleCheck.Name = "DoubleCheck";
            this.DoubleCheck.Size = new System.Drawing.Size(66, 16);
            this.DoubleCheck.TabIndex = 1;
            this.DoubleCheck.Text = "2V2禁止";
            this.DoubleCheck.UseVisualStyleBackColor = true;
            // 
            // SingleCheck
            // 
            this.SingleCheck.AutoSize = true;
            this.SingleCheck.Location = new System.Drawing.Point(18, 20);
            this.SingleCheck.Name = "SingleCheck";
            this.SingleCheck.Size = new System.Drawing.Size(66, 16);
            this.SingleCheck.TabIndex = 0;
            this.SingleCheck.Text = "1V1禁止";
            this.SingleCheck.UseVisualStyleBackColor = true;
            // 
            // VersionText
            // 
            this.VersionText.Location = new System.Drawing.Point(18, 127);
            this.VersionText.MaxLength = 16;
            this.VersionText.Name = "VersionText";
            this.VersionText.Size = new System.Drawing.Size(143, 21);
            this.VersionText.TabIndex = 11;
            // 
            // VersionLabel
            // 
            this.VersionLabel.AutoSize = true;
            this.VersionLabel.Location = new System.Drawing.Point(16, 106);
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(89, 12);
            this.VersionLabel.TabIndex = 10;
            this.VersionLabel.Text = "用户版本限制 :";
            // 
            // RoomSettingForm
            // 
            this.AcceptButton = this.OK_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel_Button;
            this.ClientSize = new System.Drawing.Size(311, 390);
            this.Controls.Add(this.BattleGroup);
            this.Controls.Add(this.LogonListCheck);
            this.Controls.Add(this.MaxUserNumberic);
            this.Controls.Add(this.MaxUserLabel);
            this.Controls.Add(this.WelcomeText);
            this.Controls.Add(this.WelcomeLabel);
            this.Controls.Add(this.DescText);
            this.Controls.Add(this.DescLabel);
            this.Controls.Add(this.Cancel_Button);
            this.Controls.Add(this.OK_Button);
            this.Controls.Add(this.NameText);
            this.Controls.Add(this.NameLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "RoomSettingForm";
            this.ShowInTaskbar = false;
            this.Text = "房间设置";
            ((System.ComponentModel.ISupportInitialize)(this.MaxUserNumberic)).EndInit();
            this.BattleGroup.ResumeLayout(false);
            this.BattleGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TimeNumberic)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.TextBox NameText;
        private System.Windows.Forms.Button OK_Button;
        private System.Windows.Forms.Button Cancel_Button;
        private System.Windows.Forms.TextBox DescText;
        private System.Windows.Forms.Label DescLabel;
        private System.Windows.Forms.TextBox WelcomeText;
        private System.Windows.Forms.Label WelcomeLabel;
        private System.Windows.Forms.Label MaxUserLabel;
        private System.Windows.Forms.NumericUpDown MaxUserNumberic;
        private System.Windows.Forms.CheckBox LogonListCheck;
        private System.Windows.Forms.GroupBox BattleGroup;
        private System.Windows.Forms.CheckBox SingleCheck;
        private System.Windows.Forms.CheckBox RandomCheck;
        private System.Windows.Forms.CheckBox FourPlayerCheck;
        private System.Windows.Forms.CheckBox DoubleCheck;
        private System.Windows.Forms.NumericUpDown TimeNumberic;
        private System.Windows.Forms.Label TimeoutLabel;
        private System.Windows.Forms.TextBox VersionText;
        private System.Windows.Forms.Label VersionLabel;
    }
}