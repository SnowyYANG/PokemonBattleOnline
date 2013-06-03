namespace PokemonBattle.RoomServer
{
    partial class RoomCoderForm
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
            this.CodeText = new System.Windows.Forms.TextBox();
            this.Cancel_Button = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.LanguageCombo = new System.Windows.Forms.ComboBox();
            this.ReferenceText = new System.Windows.Forms.TextBox();
            this.ReferenceLabel = new System.Windows.Forms.Label();
            this.UseCheck = new System.Windows.Forms.CheckBox();
            this.ClassLabel = new System.Windows.Forms.Label();
            this.ClassText = new System.Windows.Forms.TextBox();
            this.Help_Button = new System.Windows.Forms.Button();
            this.tabCompile = new System.Windows.Forms.TabControl();
            this.CodePage = new System.Windows.Forms.TabPage();
            this.FilePage = new System.Windows.Forms.TabPage();
            this.RemoveFileButton = new System.Windows.Forms.Button();
            this.AddFileButton = new System.Windows.Forms.Button();
            this.FileList = new System.Windows.Forms.ListBox();
            this.SourceCombo = new System.Windows.Forms.ComboBox();
            this.ComplieButton = new System.Windows.Forms.Button();
            this.tabCompile.SuspendLayout();
            this.CodePage.SuspendLayout();
            this.FilePage.SuspendLayout();
            this.SuspendLayout();
            // 
            // CodeText
            // 
            this.CodeText.AcceptsTab = true;
            this.CodeText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CodeText.Location = new System.Drawing.Point(3, 3);
            this.CodeText.Multiline = true;
            this.CodeText.Name = "CodeText";
            this.CodeText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.CodeText.Size = new System.Drawing.Size(529, 238);
            this.CodeText.TabIndex = 0;
            // 
            // Cancel_Button
            // 
            this.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel_Button.Location = new System.Drawing.Point(482, 411);
            this.Cancel_Button.Name = "Cancel_Button";
            this.Cancel_Button.Size = new System.Drawing.Size(75, 23);
            this.Cancel_Button.TabIndex = 1;
            this.Cancel_Button.Text = "取消(&C)";
            this.Cancel_Button.UseVisualStyleBackColor = true;
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(401, 411);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 2;
            this.OKButton.Text = "确定(&O)";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // LanguageCombo
            // 
            this.LanguageCombo.FormattingEnabled = true;
            this.LanguageCombo.Items.AddRange(new object[] {
            "CSharp",
            "VisualBasic"});
            this.LanguageCombo.Location = new System.Drawing.Point(12, 12);
            this.LanguageCombo.Name = "LanguageCombo";
            this.LanguageCombo.Size = new System.Drawing.Size(104, 20);
            this.LanguageCombo.TabIndex = 3;
            // 
            // ReferenceText
            // 
            this.ReferenceText.Location = new System.Drawing.Point(12, 60);
            this.ReferenceText.Name = "ReferenceText";
            this.ReferenceText.Size = new System.Drawing.Size(538, 21);
            this.ReferenceText.TabIndex = 4;
            // 
            // ReferenceLabel
            // 
            this.ReferenceLabel.AutoSize = true;
            this.ReferenceLabel.Location = new System.Drawing.Point(12, 45);
            this.ReferenceLabel.Name = "ReferenceLabel";
            this.ReferenceLabel.Size = new System.Drawing.Size(257, 12);
            this.ReferenceLabel.TabIndex = 5;
            this.ReferenceLabel.Text = "引用程序集(默认添加System.dll,用 / 隔开) :";
            // 
            // UseCheck
            // 
            this.UseCheck.AutoSize = true;
            this.UseCheck.Location = new System.Drawing.Point(466, 12);
            this.UseCheck.Name = "UseCheck";
            this.UseCheck.Size = new System.Drawing.Size(84, 16);
            this.UseCheck.TabIndex = 7;
            this.UseCheck.Text = "启用此功能";
            this.UseCheck.UseVisualStyleBackColor = true;
            // 
            // ClassLabel
            // 
            this.ClassLabel.AutoSize = true;
            this.ClassLabel.Location = new System.Drawing.Point(14, 94);
            this.ClassLabel.Name = "ClassLabel";
            this.ClassLabel.Size = new System.Drawing.Size(95, 12);
            this.ClassLabel.TabIndex = 8;
            this.ClassLabel.Text = "入口类 (全名) :";
            // 
            // ClassText
            // 
            this.ClassText.Location = new System.Drawing.Point(14, 109);
            this.ClassText.Name = "ClassText";
            this.ClassText.Size = new System.Drawing.Size(125, 21);
            this.ClassText.TabIndex = 9;
            // 
            // Help_Button
            // 
            this.Help_Button.Location = new System.Drawing.Point(12, 411);
            this.Help_Button.Name = "Help_Button";
            this.Help_Button.Size = new System.Drawing.Size(75, 23);
            this.Help_Button.TabIndex = 10;
            this.Help_Button.Text = "帮助(&H)";
            this.Help_Button.UseVisualStyleBackColor = true;
            this.Help_Button.Click += new System.EventHandler(this.Help_Button_Click);
            // 
            // tabCompile
            // 
            this.tabCompile.Controls.Add(this.CodePage);
            this.tabCompile.Controls.Add(this.FilePage);
            this.tabCompile.Location = new System.Drawing.Point(14, 136);
            this.tabCompile.Name = "tabCompile";
            this.tabCompile.SelectedIndex = 0;
            this.tabCompile.Size = new System.Drawing.Size(543, 269);
            this.tabCompile.TabIndex = 11;
            // 
            // CodePage
            // 
            this.CodePage.Controls.Add(this.CodeText);
            this.CodePage.Location = new System.Drawing.Point(4, 21);
            this.CodePage.Name = "CodePage";
            this.CodePage.Padding = new System.Windows.Forms.Padding(3);
            this.CodePage.Size = new System.Drawing.Size(535, 244);
            this.CodePage.TabIndex = 0;
            this.CodePage.Text = "代码";
            this.CodePage.UseVisualStyleBackColor = true;
            // 
            // FilePage
            // 
            this.FilePage.Controls.Add(this.RemoveFileButton);
            this.FilePage.Controls.Add(this.AddFileButton);
            this.FilePage.Controls.Add(this.FileList);
            this.FilePage.Location = new System.Drawing.Point(4, 21);
            this.FilePage.Name = "FilePage";
            this.FilePage.Padding = new System.Windows.Forms.Padding(3);
            this.FilePage.Size = new System.Drawing.Size(535, 244);
            this.FilePage.TabIndex = 1;
            this.FilePage.Text = "文件";
            this.FilePage.UseVisualStyleBackColor = true;
            // 
            // RemoveFileButton
            // 
            this.RemoveFileButton.Location = new System.Drawing.Point(454, 214);
            this.RemoveFileButton.Name = "RemoveFileButton";
            this.RemoveFileButton.Size = new System.Drawing.Size(75, 23);
            this.RemoveFileButton.TabIndex = 2;
            this.RemoveFileButton.Text = "移除(&R)";
            this.RemoveFileButton.UseVisualStyleBackColor = true;
            this.RemoveFileButton.Click += new System.EventHandler(this.RemoveFileButton_Click);
            // 
            // AddFileButton
            // 
            this.AddFileButton.Location = new System.Drawing.Point(373, 214);
            this.AddFileButton.Name = "AddFileButton";
            this.AddFileButton.Size = new System.Drawing.Size(75, 23);
            this.AddFileButton.TabIndex = 1;
            this.AddFileButton.Text = "添加(&A)";
            this.AddFileButton.UseVisualStyleBackColor = true;
            this.AddFileButton.Click += new System.EventHandler(this.AddFileButton_Click);
            // 
            // FileList
            // 
            this.FileList.FormattingEnabled = true;
            this.FileList.HorizontalScrollbar = true;
            this.FileList.ItemHeight = 12;
            this.FileList.Location = new System.Drawing.Point(6, 6);
            this.FileList.Name = "FileList";
            this.FileList.Size = new System.Drawing.Size(523, 196);
            this.FileList.TabIndex = 0;
            // 
            // SourceCombo
            // 
            this.SourceCombo.FormattingEnabled = true;
            this.SourceCombo.Items.AddRange(new object[] {
            "使用代码",
            "使用文件"});
            this.SourceCombo.Location = new System.Drawing.Point(132, 12);
            this.SourceCombo.Name = "SourceCombo";
            this.SourceCombo.Size = new System.Drawing.Size(99, 20);
            this.SourceCombo.TabIndex = 12;
            // 
            // ComplieButton
            // 
            this.ComplieButton.Location = new System.Drawing.Point(320, 411);
            this.ComplieButton.Name = "ComplieButton";
            this.ComplieButton.Size = new System.Drawing.Size(75, 23);
            this.ComplieButton.TabIndex = 13;
            this.ComplieButton.Text = "编译(&C)";
            this.ComplieButton.UseVisualStyleBackColor = true;
            this.ComplieButton.Click += new System.EventHandler(this.ComplieButton_Click);
            // 
            // RoomCoderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel_Button;
            this.ClientSize = new System.Drawing.Size(569, 446);
            this.Controls.Add(this.ComplieButton);
            this.Controls.Add(this.SourceCombo);
            this.Controls.Add(this.tabCompile);
            this.Controls.Add(this.Help_Button);
            this.Controls.Add(this.ClassText);
            this.Controls.Add(this.ClassLabel);
            this.Controls.Add(this.UseCheck);
            this.Controls.Add(this.ReferenceLabel);
            this.Controls.Add(this.ReferenceText);
            this.Controls.Add(this.LanguageCombo);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.Cancel_Button);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "RoomCoderForm";
            this.Text = "房间代码";
            this.Load += new System.EventHandler(this.RoomCoder_Load);
            this.tabCompile.ResumeLayout(false);
            this.CodePage.ResumeLayout(false);
            this.CodePage.PerformLayout();
            this.FilePage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox CodeText;
        private System.Windows.Forms.Button Cancel_Button;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.ComboBox LanguageCombo;
        private System.Windows.Forms.TextBox ReferenceText;
        private System.Windows.Forms.Label ReferenceLabel;
        private System.Windows.Forms.CheckBox UseCheck;
        private System.Windows.Forms.Label ClassLabel;
        private System.Windows.Forms.TextBox ClassText;
        private System.Windows.Forms.Button Help_Button;
        private System.Windows.Forms.TabControl tabCompile;
        private System.Windows.Forms.TabPage CodePage;
        private System.Windows.Forms.TabPage FilePage;
        private System.Windows.Forms.Button RemoveFileButton;
        private System.Windows.Forms.Button AddFileButton;
        private System.Windows.Forms.ListBox FileList;
        private System.Windows.Forms.ComboBox SourceCombo;
        private System.Windows.Forms.Button ComplieButton;
    }
}