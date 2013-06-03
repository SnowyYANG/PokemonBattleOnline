namespace PokemonBattle.RoomClient
{
    partial class RoomClientForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RoomClientForm));
            this.UserImage = new System.Windows.Forms.ImageList(this.components);
            this.UserList = new System.Windows.Forms.ListView();
            this.UserNameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.StateColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.UserMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ChatToMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ChallengeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ObserveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RoomTab = new System.Windows.Forms.TabControl();
            this.ChatPage = new System.Windows.Forms.TabPage();
            this.BcCounterLabel = new System.Windows.Forms.Label();
            this.BroadcastLabel = new System.Windows.Forms.Label();
            this.CommandCheck = new System.Windows.Forms.CheckBox();
            this.SendButton = new System.Windows.Forms.Button();
            this.MessageText = new System.Windows.Forms.TextBox();
            this.DisplayText = new System.Windows.Forms.TextBox();
            this.FourPlayerPage = new System.Windows.Forms.TabPage();
            this.CreateButton = new System.Windows.Forms.Button();
            this.EnterButton = new System.Windows.Forms.Button();
            this.FourPlayerList = new System.Windows.Forms.ListView();
            this.RoomNameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PlayerCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.RoomMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UserInfoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AwayMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ChatsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UserMenu.SuspendLayout();
            this.RoomTab.SuspendLayout();
            this.ChatPage.SuspendLayout();
            this.FourPlayerPage.SuspendLayout();
            this.MainMenu.SuspendLayout();
            this.SuspendLayout();
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
            // UserList
            // 
            this.UserList.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.UserList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.UserNameColumn,
            this.StateColumn});
            this.UserList.ContextMenuStrip = this.UserMenu;
            this.UserList.FullRowSelect = true;
            this.UserList.Location = new System.Drawing.Point(445, 48);
            this.UserList.MultiSelect = false;
            this.UserList.Name = "UserList";
            this.UserList.Size = new System.Drawing.Size(196, 410);
            this.UserList.SmallImageList = this.UserImage;
            this.UserList.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.UserList.TabIndex = 4;
            this.UserList.UseCompatibleStateImageBehavior = false;
            this.UserList.View = System.Windows.Forms.View.Details;
            // 
            // UserNameColumn
            // 
            this.UserNameColumn.Text = "名称";
            this.UserNameColumn.Width = 113;
            // 
            // StateColumn
            // 
            this.StateColumn.Text = "状态";
            // 
            // UserMenu
            // 
            this.UserMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ChatToMenuItem,
            this.ToolStripSeparator1,
            this.ChallengeMenuItem,
            this.ObserveMenuItem});
            this.UserMenu.Name = "mnuRoomList";
            this.UserMenu.ShowImageMargin = false;
            this.UserMenu.Size = new System.Drawing.Size(100, 76);
            // 
            // ChatToMenuItem
            // 
            this.ChatToMenuItem.Name = "ChatToMenuItem";
            this.ChatToMenuItem.Size = new System.Drawing.Size(99, 22);
            this.ChatToMenuItem.Text = "私人聊天";
            this.ChatToMenuItem.Click += new System.EventHandler(this.ChatToMenuItem_Click);
            // 
            // ToolStripSeparator1
            // 
            this.ToolStripSeparator1.Name = "ToolStripSeparator1";
            this.ToolStripSeparator1.Size = new System.Drawing.Size(96, 6);
            // 
            // ChallengeMenuItem
            // 
            this.ChallengeMenuItem.Name = "ChallengeMenuItem";
            this.ChallengeMenuItem.Size = new System.Drawing.Size(99, 22);
            this.ChallengeMenuItem.Text = "挑战";
            this.ChallengeMenuItem.Click += new System.EventHandler(this.ChallengeMenuItem_Click);
            // 
            // ObserveMenuItem
            // 
            this.ObserveMenuItem.Name = "ObserveMenuItem";
            this.ObserveMenuItem.Size = new System.Drawing.Size(99, 22);
            this.ObserveMenuItem.Text = "观战";
            this.ObserveMenuItem.Click += new System.EventHandler(this.ObserveMenuItem_Click);
            // 
            // RoomTab
            // 
            this.RoomTab.Controls.Add(this.ChatPage);
            this.RoomTab.Controls.Add(this.FourPlayerPage);
            this.RoomTab.Location = new System.Drawing.Point(0, 27);
            this.RoomTab.Name = "RoomTab";
            this.RoomTab.SelectedIndex = 0;
            this.RoomTab.Size = new System.Drawing.Size(438, 443);
            this.RoomTab.TabIndex = 5;
            // 
            // ChatPage
            // 
            this.ChatPage.Controls.Add(this.BcCounterLabel);
            this.ChatPage.Controls.Add(this.BroadcastLabel);
            this.ChatPage.Controls.Add(this.CommandCheck);
            this.ChatPage.Controls.Add(this.SendButton);
            this.ChatPage.Controls.Add(this.MessageText);
            this.ChatPage.Controls.Add(this.DisplayText);
            this.ChatPage.Location = new System.Drawing.Point(4, 22);
            this.ChatPage.Name = "ChatPage";
            this.ChatPage.Padding = new System.Windows.Forms.Padding(3);
            this.ChatPage.Size = new System.Drawing.Size(430, 417);
            this.ChatPage.TabIndex = 1;
            this.ChatPage.Text = "房间";
            this.ChatPage.UseVisualStyleBackColor = true;
            // 
            // BcCounterLabel
            // 
            this.BcCounterLabel.AutoSize = true;
            this.BcCounterLabel.ForeColor = System.Drawing.Color.Red;
            this.BcCounterLabel.Location = new System.Drawing.Point(80, 396);
            this.BcCounterLabel.Name = "BcCounterLabel";
            this.BcCounterLabel.Size = new System.Drawing.Size(17, 12);
            this.BcCounterLabel.TabIndex = 5;
            this.BcCounterLabel.Text = "0%";
            // 
            // BroadcastLabel
            // 
            this.BroadcastLabel.AutoSize = true;
            this.BroadcastLabel.Location = new System.Drawing.Point(9, 396);
            this.BroadcastLabel.Name = "BroadcastLabel";
            this.BroadcastLabel.Size = new System.Drawing.Size(65, 12);
            this.BroadcastLabel.TabIndex = 4;
            this.BroadcastLabel.Text = "发言限制 :";
            // 
            // CommandCheck
            // 
            this.CommandCheck.AutoSize = true;
            this.CommandCheck.Location = new System.Drawing.Point(280, 391);
            this.CommandCheck.Name = "CommandCheck";
            this.CommandCheck.Size = new System.Drawing.Size(72, 16);
            this.CommandCheck.TabIndex = 3;
            this.CommandCheck.Text = "房间命令";
            this.CommandCheck.UseVisualStyleBackColor = true;
            // 
            // SendButton
            // 
            this.SendButton.Enabled = false;
            this.SendButton.Location = new System.Drawing.Point(358, 387);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(63, 23);
            this.SendButton.TabIndex = 2;
            this.SendButton.Text = "发送(&S)";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // MessageText
            // 
            this.MessageText.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.MessageText.Location = new System.Drawing.Point(3, 360);
            this.MessageText.MaxLength = 50;
            this.MessageText.Name = "MessageText";
            this.MessageText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.MessageText.Size = new System.Drawing.Size(418, 21);
            this.MessageText.TabIndex = 1;
            // 
            // DisplayText
            // 
            this.DisplayText.BackColor = System.Drawing.Color.White;
            this.DisplayText.Dock = System.Windows.Forms.DockStyle.Top;
            this.DisplayText.Location = new System.Drawing.Point(3, 3);
            this.DisplayText.Multiline = true;
            this.DisplayText.Name = "DisplayText";
            this.DisplayText.ReadOnly = true;
            this.DisplayText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.DisplayText.Size = new System.Drawing.Size(424, 351);
            this.DisplayText.TabIndex = 0;
            // 
            // FourPlayerPage
            // 
            this.FourPlayerPage.Controls.Add(this.CreateButton);
            this.FourPlayerPage.Controls.Add(this.EnterButton);
            this.FourPlayerPage.Controls.Add(this.FourPlayerList);
            this.FourPlayerPage.Location = new System.Drawing.Point(4, 22);
            this.FourPlayerPage.Name = "FourPlayerPage";
            this.FourPlayerPage.Padding = new System.Windows.Forms.Padding(3);
            this.FourPlayerPage.Size = new System.Drawing.Size(430, 417);
            this.FourPlayerPage.TabIndex = 2;
            this.FourPlayerPage.Text = "4P对战";
            this.FourPlayerPage.UseVisualStyleBackColor = true;
            // 
            // CreateButton
            // 
            this.CreateButton.Enabled = false;
            this.CreateButton.Location = new System.Drawing.Point(268, 387);
            this.CreateButton.Name = "CreateButton";
            this.CreateButton.Size = new System.Drawing.Size(75, 23);
            this.CreateButton.TabIndex = 5;
            this.CreateButton.Text = "创建(&C)";
            this.CreateButton.UseVisualStyleBackColor = true;
            this.CreateButton.Click += new System.EventHandler(this.CreateButton_Click);
            // 
            // EnterButton
            // 
            this.EnterButton.Enabled = false;
            this.EnterButton.Location = new System.Drawing.Point(349, 387);
            this.EnterButton.Name = "EnterButton";
            this.EnterButton.Size = new System.Drawing.Size(75, 23);
            this.EnterButton.TabIndex = 4;
            this.EnterButton.Text = "进入(&E)";
            this.EnterButton.UseVisualStyleBackColor = true;
            this.EnterButton.Click += new System.EventHandler(this.EnterButton_Click);
            // 
            // FourPlayerList
            // 
            this.FourPlayerList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.RoomNameColumn,
            this.PlayerCount});
            this.FourPlayerList.FullRowSelect = true;
            this.FourPlayerList.Location = new System.Drawing.Point(0, 0);
            this.FourPlayerList.MultiSelect = false;
            this.FourPlayerList.Name = "FourPlayerList";
            this.FourPlayerList.Size = new System.Drawing.Size(430, 371);
            this.FourPlayerList.TabIndex = 3;
            this.FourPlayerList.UseCompatibleStateImageBehavior = false;
            this.FourPlayerList.View = System.Windows.Forms.View.Details;
            this.FourPlayerList.SelectedIndexChanged += new System.EventHandler(this.FourPlayerList_SelectedIndexChanged);
            // 
            // RoomNameColumn
            // 
            this.RoomNameColumn.Text = "4P房间";
            this.RoomNameColumn.Width = 252;
            // 
            // PlayerCount
            // 
            this.PlayerCount.Text = "人数";
            this.PlayerCount.Width = 96;
            // 
            // MainMenu
            // 
            this.MainMenu.AllowMerge = false;
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RoomMenuItem,
            this.ChatsMenuItem});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(644, 25);
            this.MainMenu.TabIndex = 6;
            // 
            // RoomMenuItem
            // 
            this.RoomMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.UserInfoMenuItem,
            this.AwayMenuItem,
            this.ExitMenuItem});
            this.RoomMenuItem.Enabled = false;
            this.RoomMenuItem.Name = "RoomMenuItem";
            this.RoomMenuItem.Size = new System.Drawing.Size(60, 21);
            this.RoomMenuItem.Text = "房间(&R)";
            // 
            // UserInfoMenuItem
            // 
            this.UserInfoMenuItem.Name = "UserInfoMenuItem";
            this.UserInfoMenuItem.Size = new System.Drawing.Size(139, 22);
            this.UserInfoMenuItem.Text = "用户信息(&S)";
            this.UserInfoMenuItem.Click += new System.EventHandler(this.UserInfoMenuItem_Click);
            // 
            // AwayMenuItem
            // 
            this.AwayMenuItem.CheckOnClick = true;
            this.AwayMenuItem.Name = "AwayMenuItem";
            this.AwayMenuItem.Size = new System.Drawing.Size(139, 22);
            this.AwayMenuItem.Text = "离开(&A)";
            this.AwayMenuItem.CheckedChanged += new System.EventHandler(this.AwayMenuItem_CheckedChanged);
            // 
            // ExitMenuItem
            // 
            this.ExitMenuItem.Name = "ExitMenuItem";
            this.ExitMenuItem.Size = new System.Drawing.Size(139, 22);
            this.ExitMenuItem.Text = "退出(&X)";
            this.ExitMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
            // 
            // ChatsMenuItem
            // 
            this.ChatsMenuItem.Enabled = false;
            this.ChatsMenuItem.Name = "ChatsMenuItem";
            this.ChatsMenuItem.Size = new System.Drawing.Size(84, 21);
            this.ChatsMenuItem.Text = "私人聊天(&C)";
            // 
            // RoomClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 470);
            this.Controls.Add(this.MainMenu);
            this.Controls.Add(this.RoomTab);
            this.Controls.Add(this.UserList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "RoomClientForm";
            this.Text = "房间窗口";
            this.Load += new System.EventHandler(this.RoomUserForm_Load);
            this.UserMenu.ResumeLayout(false);
            this.RoomTab.ResumeLayout(false);
            this.ChatPage.ResumeLayout(false);
            this.ChatPage.PerformLayout();
            this.FourPlayerPage.ResumeLayout(false);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.ImageList UserImage;
        internal System.Windows.Forms.ListView UserList;
        internal System.Windows.Forms.ColumnHeader UserNameColumn;
        internal System.Windows.Forms.ColumnHeader StateColumn;
        internal System.Windows.Forms.TabControl RoomTab;
        internal System.Windows.Forms.TabPage ChatPage;
        internal System.Windows.Forms.Button SendButton;
        internal System.Windows.Forms.TextBox MessageText;
        internal System.Windows.Forms.TabPage FourPlayerPage;
        internal System.Windows.Forms.Button EnterButton;
        internal System.Windows.Forms.ListView FourPlayerList;
        internal System.Windows.Forms.ColumnHeader RoomNameColumn;
        internal System.Windows.Forms.ColumnHeader PlayerCount;
        internal System.Windows.Forms.ContextMenuStrip UserMenu;
        internal System.Windows.Forms.ToolStripMenuItem ChatToMenuItem;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator1;
        internal System.Windows.Forms.ToolStripMenuItem ChallengeMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ObserveMenuItem;
        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem RoomMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ChatsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AwayMenuItem;
        private System.Windows.Forms.ToolStripMenuItem UserInfoMenuItem;
        private System.Windows.Forms.Button CreateButton;
        private System.Windows.Forms.CheckBox CommandCheck;
        private System.Windows.Forms.Label BcCounterLabel;
        private System.Windows.Forms.Label BroadcastLabel;
        internal System.Windows.Forms.TextBox DisplayText;
    }
}