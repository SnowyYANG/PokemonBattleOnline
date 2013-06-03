namespace PokemonBattle.RoomClient
{
    partial class UserInfoForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserInfoForm));
            this.ImageDownButton = new System.Windows.Forms.Button();
            this.ImageUpButton = new System.Windows.Forms.Button();
            this.MyImage = new System.Windows.Forms.PictureBox();
            this.NameLabel = new System.Windows.Forms.Label();
            this.UserImage = new System.Windows.Forms.ImageList(this.components);
            this.NameText = new System.Windows.Forms.TextBox();
            this.Cancel_Button = new System.Windows.Forms.Button();
            this.OK_Button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.MyImage)).BeginInit();
            this.SuspendLayout();
            // 
            // ImageDownButton
            // 
            this.ImageDownButton.Enabled = false;
            this.ImageDownButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.ImageDownButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSkyBlue;
            this.ImageDownButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SkyBlue;
            this.ImageDownButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ImageDownButton.Font = new System.Drawing.Font("宋体", 7F, System.Drawing.FontStyle.Bold);
            this.ImageDownButton.Location = new System.Drawing.Point(77, 24);
            this.ImageDownButton.Name = "ImageDownButton";
            this.ImageDownButton.Size = new System.Drawing.Size(20, 20);
            this.ImageDownButton.TabIndex = 2;
            this.ImageDownButton.Text = "<";
            this.ImageDownButton.UseVisualStyleBackColor = true;
            this.ImageDownButton.Click += new System.EventHandler(this.ImageDownButton_Click);
            // 
            // ImageUpButton
            // 
            this.ImageUpButton.Enabled = false;
            this.ImageUpButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.ImageUpButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSkyBlue;
            this.ImageUpButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SkyBlue;
            this.ImageUpButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ImageUpButton.Font = new System.Drawing.Font("宋体", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ImageUpButton.Location = new System.Drawing.Point(141, 24);
            this.ImageUpButton.Name = "ImageUpButton";
            this.ImageUpButton.Size = new System.Drawing.Size(20, 20);
            this.ImageUpButton.TabIndex = 3;
            this.ImageUpButton.Text = ">";
            this.ImageUpButton.UseVisualStyleBackColor = true;
            this.ImageUpButton.Click += new System.EventHandler(this.ImageUpButton_Click);
            // 
            // MyImage
            // 
            this.MyImage.Location = new System.Drawing.Point(103, 12);
            this.MyImage.Name = "MyImage";
            this.MyImage.Size = new System.Drawing.Size(32, 32);
            this.MyImage.TabIndex = 10;
            this.MyImage.TabStop = false;
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Location = new System.Drawing.Point(22, 68);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(41, 12);
            this.NameLabel.TabIndex = 9;
            this.NameLabel.Text = "名称 :";
            // 
            // UserImage
            // 
            this.UserImage.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("UserImage.ImageStream")));
            this.UserImage.TransparentColor = System.Drawing.Color.Transparent;
            this.UserImage.Images.SetKeyName(0, "Pokemon1.ico");
            this.UserImage.Images.SetKeyName(1, "Pokemon2.ico");
            this.UserImage.Images.SetKeyName(2, "Pokemon3.ico");
            this.UserImage.Images.SetKeyName(3, "Pokemon4.ico");
            this.UserImage.Images.SetKeyName(4, "Pokemon5.ico");
            this.UserImage.Images.SetKeyName(5, "Pokemon6.ico");
            this.UserImage.Images.SetKeyName(6, "Pokemon7.ico");
            this.UserImage.Images.SetKeyName(7, "Pokemon8.ico");
            this.UserImage.Images.SetKeyName(8, "Pokemon9.ico");
            this.UserImage.Images.SetKeyName(9, "Pokemon10.ico");
            this.UserImage.Images.SetKeyName(10, "Pokemon11.ico");
            this.UserImage.Images.SetKeyName(11, "Pokemon12.ico");
            this.UserImage.Images.SetKeyName(12, "Pokemon13.ico");
            this.UserImage.Images.SetKeyName(13, "Pokemon14.ico");
            this.UserImage.Images.SetKeyName(14, "Pokemon15.ico");
            // 
            // NameText
            // 
            this.NameText.Enabled = false;
            this.NameText.Location = new System.Drawing.Point(69, 65);
            this.NameText.MaxLength = 16;
            this.NameText.Name = "NameText";
            this.NameText.Size = new System.Drawing.Size(100, 21);
            this.NameText.TabIndex = 4;
            this.NameText.TextChanged += new System.EventHandler(this.NameText_TextChanged);
            // 
            // Cancel_Button
            // 
            this.Cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel_Button.Location = new System.Drawing.Point(175, 123);
            this.Cancel_Button.Name = "Cancel_Button";
            this.Cancel_Button.Size = new System.Drawing.Size(75, 23);
            this.Cancel_Button.TabIndex = 1;
            this.Cancel_Button.Text = "取消(&A)";
            this.Cancel_Button.UseVisualStyleBackColor = true;
            // 
            // OK_Button
            // 
            this.OK_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OK_Button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK_Button.Location = new System.Drawing.Point(94, 123);
            this.OK_Button.Name = "OK_Button";
            this.OK_Button.Size = new System.Drawing.Size(75, 23);
            this.OK_Button.TabIndex = 0;
            this.OK_Button.Text = "确定(&O)";
            this.OK_Button.UseVisualStyleBackColor = true;
            this.OK_Button.Click += new System.EventHandler(this.OK_Button_Click);
            // 
            // UserInfoForm
            // 
            this.AcceptButton = this.OK_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel_Button;
            this.ClientSize = new System.Drawing.Size(262, 158);
            this.Controls.Add(this.Cancel_Button);
            this.Controls.Add(this.OK_Button);
            this.Controls.Add(this.NameText);
            this.Controls.Add(this.ImageDownButton);
            this.Controls.Add(this.ImageUpButton);
            this.Controls.Add(this.MyImage);
            this.Controls.Add(this.NameLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "UserInfoForm";
            this.ShowInTaskbar = false;
            this.Text = "用户信息";
            this.Load += new System.EventHandler(this.UserInfoForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.MyImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button ImageDownButton;
        internal System.Windows.Forms.Button ImageUpButton;
        internal System.Windows.Forms.PictureBox MyImage;
        internal System.Windows.Forms.Label NameLabel;
        internal System.Windows.Forms.ImageList UserImage;
        private System.Windows.Forms.TextBox NameText;
        private System.Windows.Forms.Button Cancel_Button;
        private System.Windows.Forms.Button OK_Button;
    }
}