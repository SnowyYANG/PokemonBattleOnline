namespace PokemonBattle.RoomClient
{
    partial class ChatForm
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
            this.ExitButton = new System.Windows.Forms.Button();
            this.SendButton = new System.Windows.Forms.Button();
            this.DisplayText = new System.Windows.Forms.TextBox();
            this.MessageText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ExitButton
            // 
            this.ExitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ExitButton.Location = new System.Drawing.Point(245, 288);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(63, 23);
            this.ExitButton.TabIndex = 5;
            this.ExitButton.Text = "退出(&E)";
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // SendButton
            // 
            this.SendButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SendButton.Location = new System.Drawing.Point(176, 288);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(63, 23);
            this.SendButton.TabIndex = 4;
            this.SendButton.Text = "发送(&S)";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // DisplayText
            // 
            this.DisplayText.BackColor = System.Drawing.Color.White;
            this.DisplayText.Location = new System.Drawing.Point(12, 12);
            this.DisplayText.Multiline = true;
            this.DisplayText.Name = "DisplayText";
            this.DisplayText.ReadOnly = true;
            this.DisplayText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.DisplayText.Size = new System.Drawing.Size(296, 238);
            this.DisplayText.TabIndex = 6;
            // 
            // MessageText
            // 
            this.MessageText.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.MessageText.Location = new System.Drawing.Point(12, 256);
            this.MessageText.MaxLength = 50;
            this.MessageText.Name = "MessageText";
            this.MessageText.Size = new System.Drawing.Size(296, 21);
            this.MessageText.TabIndex = 7;
            // 
            // ChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 323);
            this.Controls.Add(this.MessageText);
            this.Controls.Add(this.DisplayText);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.SendButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ChatForm";
            this.ShowInTaskbar = false;
            this.Text = "私人聊天窗口";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button ExitButton;
        internal System.Windows.Forms.Button SendButton;
        private System.Windows.Forms.TextBox DisplayText;
        private System.Windows.Forms.TextBox MessageText;
    }
}