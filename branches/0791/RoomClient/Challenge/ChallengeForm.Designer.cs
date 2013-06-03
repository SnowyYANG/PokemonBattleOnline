namespace PokemonBattle.RoomClient
{
    partial class ChallengeForm
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
            this.RuleGroup = new System.Windows.Forms.GroupBox();
            this.RandomCheck = new System.Windows.Forms.CheckBox();
            this.PPUpCheck = new System.Windows.Forms.CheckBox();
            this.CustomLabel = new System.Windows.Forms.Label();
            this.ModeLabel = new System.Windows.Forms.Label();
            this.ModeCombo = new System.Windows.Forms.ComboBox();
            this.LinkCombo = new System.Windows.Forms.ComboBox();
            this.LinkLabel = new System.Windows.Forms.Label();
            this.Cancel_Button = new System.Windows.Forms.Button();
            this.OK_Button = new System.Windows.Forms.Button();
            this.NameLabel = new System.Windows.Forms.Label();
            this.RuleGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // RuleGroup
            // 
            this.RuleGroup.Controls.Add(this.RandomCheck);
            this.RuleGroup.Controls.Add(this.PPUpCheck);
            this.RuleGroup.Location = new System.Drawing.Point(13, 149);
            this.RuleGroup.Name = "RuleGroup";
            this.RuleGroup.Size = new System.Drawing.Size(267, 63);
            this.RuleGroup.TabIndex = 25;
            this.RuleGroup.TabStop = false;
            this.RuleGroup.Text = "可选规则";
            // 
            // RandomCheck
            // 
            this.RandomCheck.AutoSize = true;
            this.RandomCheck.Location = new System.Drawing.Point(177, 20);
            this.RandomCheck.Name = "RandomCheck";
            this.RandomCheck.Size = new System.Drawing.Size(72, 16);
            this.RandomCheck.TabIndex = 1;
            this.RandomCheck.Text = "随机队伍";
            this.RandomCheck.UseVisualStyleBackColor = true;
            // 
            // PPUpCheck
            // 
            this.PPUpCheck.AutoSize = true;
            this.PPUpCheck.Checked = true;
            this.PPUpCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.PPUpCheck.Location = new System.Drawing.Point(14, 20);
            this.PPUpCheck.Name = "PPUpCheck";
            this.PPUpCheck.Size = new System.Drawing.Size(84, 16);
            this.PPUpCheck.TabIndex = 0;
            this.PPUpCheck.Text = "PP上限提升";
            this.PPUpCheck.UseVisualStyleBackColor = true;
            // 
            // CustomLabel
            // 
            this.CustomLabel.AutoSize = true;
            this.CustomLabel.Location = new System.Drawing.Point(56, 132);
            this.CustomLabel.Name = "CustomLabel";
            this.CustomLabel.Size = new System.Drawing.Size(0, 12);
            this.CustomLabel.TabIndex = 24;
            this.CustomLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ModeLabel
            // 
            this.ModeLabel.AutoSize = true;
            this.ModeLabel.Location = new System.Drawing.Point(53, 106);
            this.ModeLabel.Name = "ModeLabel";
            this.ModeLabel.Size = new System.Drawing.Size(65, 12);
            this.ModeLabel.TabIndex = 23;
            this.ModeLabel.Text = "对战模式 :";
            this.ModeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ModeCombo
            // 
            this.ModeCombo.FormattingEnabled = true;
            this.ModeCombo.Items.AddRange(new object[] {
            "1V1",
            "2V2"});
            this.ModeCombo.Location = new System.Drawing.Point(124, 103);
            this.ModeCombo.Name = "ModeCombo";
            this.ModeCombo.Size = new System.Drawing.Size(94, 20);
            this.ModeCombo.TabIndex = 22;
            // 
            // LinkCombo
            // 
            this.LinkCombo.FormattingEnabled = true;
            this.LinkCombo.Items.AddRange(new object[] {
            "直连",
            "服务器中转"});
            this.LinkCombo.Location = new System.Drawing.Point(124, 77);
            this.LinkCombo.Name = "LinkCombo";
            this.LinkCombo.Size = new System.Drawing.Size(94, 20);
            this.LinkCombo.TabIndex = 21;
            // 
            // LinkLabel
            // 
            this.LinkLabel.AutoSize = true;
            this.LinkLabel.Location = new System.Drawing.Point(53, 80);
            this.LinkLabel.Name = "LinkLabel";
            this.LinkLabel.Size = new System.Drawing.Size(65, 12);
            this.LinkLabel.TabIndex = 20;
            this.LinkLabel.Text = "连接方式 :";
            this.LinkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Cancel_Button
            // 
            this.Cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Cancel_Button.Location = new System.Drawing.Point(176, 218);
            this.Cancel_Button.Name = "Cancel_Button";
            this.Cancel_Button.Size = new System.Drawing.Size(83, 31);
            this.Cancel_Button.TabIndex = 18;
            this.Cancel_Button.UseVisualStyleBackColor = true;
            // 
            // OK_Button
            // 
            this.OK_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.OK_Button.Location = new System.Drawing.Point(35, 218);
            this.OK_Button.Name = "OK_Button";
            this.OK_Button.Size = new System.Drawing.Size(83, 31);
            this.OK_Button.TabIndex = 17;
            this.OK_Button.UseVisualStyleBackColor = true;
            this.OK_Button.Click += new System.EventHandler(this.OK_Button_Click);
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Font = new System.Drawing.Font("宋体", 19F);
            this.NameLabel.Location = new System.Drawing.Point(47, 15);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(0, 26);
            this.NameLabel.TabIndex = 19;
            // 
            // ChallengeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 268);
            this.Controls.Add(this.RuleGroup);
            this.Controls.Add(this.CustomLabel);
            this.Controls.Add(this.ModeLabel);
            this.Controls.Add(this.ModeCombo);
            this.Controls.Add(this.LinkCombo);
            this.Controls.Add(this.LinkLabel);
            this.Controls.Add(this.Cancel_Button);
            this.Controls.Add(this.OK_Button);
            this.Controls.Add(this.NameLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChallengeForm";
            this.ShowInTaskbar = false;
            this.Text = "挑战窗口";
            this.RuleGroup.ResumeLayout(false);
            this.RuleGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.GroupBox RuleGroup;
        internal System.Windows.Forms.CheckBox RandomCheck;
        internal System.Windows.Forms.CheckBox PPUpCheck;
        internal System.Windows.Forms.Label CustomLabel;
        internal System.Windows.Forms.Label ModeLabel;
        internal System.Windows.Forms.ComboBox ModeCombo;
        internal System.Windows.Forms.ComboBox LinkCombo;
        internal System.Windows.Forms.Label LinkLabel;
        internal System.Windows.Forms.Button Cancel_Button;
        internal System.Windows.Forms.Button OK_Button;
        internal System.Windows.Forms.Label NameLabel;
    }
}