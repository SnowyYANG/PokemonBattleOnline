namespace PokemonBattle.RoomServer
{
    partial class RoomServerForm
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
            this.更改名称NToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.更改简介MToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.更改欢迎信息WToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.退出EToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.RoomMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.HideMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SettingMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RoomCodeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DisplayText = new System.Windows.Forms.TextBox();
            this.UserList = new System.Windows.Forms.ListView();
            this.NameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AddressColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.StateColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.UserMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.KickMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.MessageText = new System.Windows.Forms.TextBox();
            this.SendButton = new System.Windows.Forms.Button();
            this.RoomIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.RoomIconMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.OpenWinMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SettingMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ExitMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenu.SuspendLayout();
            this.UserMenu.SuspendLayout();
            this.RoomIconMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // 更改名称NToolStripMenuItem
            // 
            this.更改名称NToolStripMenuItem.Name = "更改名称NToolStripMenuItem";
            this.更改名称NToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.更改名称NToolStripMenuItem.Text = "更改名称(&N)";
            // 
            // 更改简介MToolStripMenuItem
            // 
            this.更改简介MToolStripMenuItem.Name = "更改简介MToolStripMenuItem";
            this.更改简介MToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.更改简介MToolStripMenuItem.Text = "更改简介(&I)";
            // 
            // 更改欢迎信息WToolStripMenuItem
            // 
            this.更改欢迎信息WToolStripMenuItem.Name = "更改欢迎信息WToolStripMenuItem";
            this.更改欢迎信息WToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.更改欢迎信息WToolStripMenuItem.Text = "更改欢迎信息(&W)";
            // 
            // ToolStripSeparator1
            // 
            this.ToolStripSeparator1.Name = "ToolStripSeparator1";
            this.ToolStripSeparator1.Size = new System.Drawing.Size(157, 6);
            // 
            // 退出EToolStripMenuItem
            // 
            this.退出EToolStripMenuItem.Name = "退出EToolStripMenuItem";
            this.退出EToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.退出EToolStripMenuItem.Text = "退出(&E)";
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RoomMenu});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(692, 25);
            this.MainMenu.TabIndex = 5;
            // 
            // RoomMenu
            // 
            this.RoomMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.HideMenuItem,
            this.SettingMenuItem,
            this.RoomCodeMenuItem,
            this.toolStripSeparator2,
            this.ExitMenuItem});
            this.RoomMenu.MergeAction = System.Windows.Forms.MergeAction.Remove;
            this.RoomMenu.Name = "RoomMenu";
            this.RoomMenu.Size = new System.Drawing.Size(60, 21);
            this.RoomMenu.Text = "房间(&R)";
            // 
            // HideMenuItem
            // 
            this.HideMenuItem.Name = "HideMenuItem";
            this.HideMenuItem.Size = new System.Drawing.Size(153, 22);
            this.HideMenuItem.Text = "隐藏主窗口(&H)";
            this.HideMenuItem.Click += new System.EventHandler(this.HideMenuItem_Click);
            // 
            // SettingMenuItem
            // 
            this.SettingMenuItem.Name = "SettingMenuItem";
            this.SettingMenuItem.Size = new System.Drawing.Size(153, 22);
            this.SettingMenuItem.Text = "设置(&S)";
            this.SettingMenuItem.Click += new System.EventHandler(this.SettingMenuItem_Click);
            // 
            // RoomCodeMenuItem
            // 
            this.RoomCodeMenuItem.Name = "RoomCodeMenuItem";
            this.RoomCodeMenuItem.Size = new System.Drawing.Size(153, 22);
            this.RoomCodeMenuItem.Text = "房间代码(&C)";
            this.RoomCodeMenuItem.Click += new System.EventHandler(this.RoomCodeMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(150, 6);
            // 
            // ExitMenuItem
            // 
            this.ExitMenuItem.Name = "ExitMenuItem";
            this.ExitMenuItem.Size = new System.Drawing.Size(153, 22);
            this.ExitMenuItem.Text = "退出(&E)";
            this.ExitMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
            // 
            // DisplayText
            // 
            this.DisplayText.BackColor = System.Drawing.Color.White;
            this.DisplayText.Location = new System.Drawing.Point(12, 27);
            this.DisplayText.Multiline = true;
            this.DisplayText.Name = "DisplayText";
            this.DisplayText.ReadOnly = true;
            this.DisplayText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.DisplayText.Size = new System.Drawing.Size(450, 400);
            this.DisplayText.TabIndex = 6;
            // 
            // UserList
            // 
            this.UserList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.UserList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.NameColumn,
            this.AddressColumn,
            this.StateColumn});
            this.UserList.ContextMenuStrip = this.UserMenu;
            this.UserList.FullRowSelect = true;
            this.UserList.Location = new System.Drawing.Point(468, 27);
            this.UserList.MultiSelect = false;
            this.UserList.Name = "UserList";
            this.UserList.Size = new System.Drawing.Size(221, 400);
            this.UserList.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.UserList.TabIndex = 7;
            this.UserList.UseCompatibleStateImageBehavior = false;
            this.UserList.View = System.Windows.Forms.View.Details;
            // 
            // NameColumn
            // 
            this.NameColumn.Text = "用户名";
            this.NameColumn.Width = 69;
            // 
            // AddressColumn
            // 
            this.AddressColumn.Text = "IP";
            this.AddressColumn.Width = 84;
            // 
            // StateColumn
            // 
            this.StateColumn.Text = "状态";
            // 
            // UserMenu
            // 
            this.UserMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.KickMenu});
            this.UserMenu.Name = "UserMenu";
            this.UserMenu.Size = new System.Drawing.Size(119, 26);
            // 
            // KickMenu
            // 
            this.KickMenu.Name = "KickMenu";
            this.KickMenu.Size = new System.Drawing.Size(118, 22);
            this.KickMenu.Text = "请出房间";
            this.KickMenu.Click += new System.EventHandler(this.KickMenu_Click);
            // 
            // MessageText
            // 
            this.MessageText.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.MessageText.Location = new System.Drawing.Point(12, 433);
            this.MessageText.MaxLength = 50;
            this.MessageText.Name = "MessageText";
            this.MessageText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.MessageText.Size = new System.Drawing.Size(376, 21);
            this.MessageText.TabIndex = 8;
            // 
            // SendButton
            // 
            this.SendButton.Location = new System.Drawing.Point(394, 433);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(75, 23);
            this.SendButton.TabIndex = 9;
            this.SendButton.Text = "发送";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // RoomIcon
            // 
            this.RoomIcon.ContextMenuStrip = this.RoomIconMenu;
            this.RoomIcon.Text = "房间服务器";
            this.RoomIcon.Visible = true;
            this.RoomIcon.DoubleClick += new System.EventHandler(this.OpenWinMenuItem_Click);
            // 
            // RoomIconMenu
            // 
            this.RoomIconMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenWinMenuItem,
            this.SettingMenuItem2,
            this.toolStripSeparator3,
            this.ExitMenuItem2});
            this.RoomIconMenu.Name = "RoomIconMenu";
            this.RoomIconMenu.Size = new System.Drawing.Size(209, 76);
            // 
            // OpenWinMenuItem
            // 
            this.OpenWinMenuItem.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.OpenWinMenuItem.Name = "OpenWinMenuItem";
            this.OpenWinMenuItem.Size = new System.Drawing.Size(208, 22);
            this.OpenWinMenuItem.Text = "打开房间服务器窗口(&O)";
            this.OpenWinMenuItem.Click += new System.EventHandler(this.OpenWinMenuItem_Click);
            // 
            // SettingMenuItem2
            // 
            this.SettingMenuItem2.Name = "SettingMenuItem2";
            this.SettingMenuItem2.Size = new System.Drawing.Size(208, 22);
            this.SettingMenuItem2.Text = "房间设置(&S)";
            this.SettingMenuItem2.Click += new System.EventHandler(this.SettingMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(205, 6);
            // 
            // ExitMenuItem2
            // 
            this.ExitMenuItem2.Name = "ExitMenuItem2";
            this.ExitMenuItem2.Size = new System.Drawing.Size(208, 22);
            this.ExitMenuItem2.Text = "退出(&X)";
            this.ExitMenuItem2.Click += new System.EventHandler(this.ExitMenuItem_Click);
            // 
            // RoomServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 468);
            this.Controls.Add(this.SendButton);
            this.Controls.Add(this.MessageText);
            this.Controls.Add(this.UserList);
            this.Controls.Add(this.DisplayText);
            this.Controls.Add(this.MainMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "RoomServerForm";
            this.Text = "房间服务器";
            this.Load += new System.EventHandler(this.RoomServerForm_Load);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.UserMenu.ResumeLayout(false);
            this.RoomIconMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.ToolStripMenuItem 更改名称NToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem 更改简介MToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem 更改欢迎信息WToolStripMenuItem;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator1;
        internal System.Windows.Forms.ToolStripMenuItem 退出EToolStripMenuItem;
        internal System.Windows.Forms.MenuStrip MainMenu;
        internal System.Windows.Forms.ToolStripMenuItem RoomMenu;
        internal System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        internal System.Windows.Forms.ToolStripMenuItem ExitMenuItem;
        internal System.Windows.Forms.TextBox DisplayText;
        internal System.Windows.Forms.ListView UserList;
        internal System.Windows.Forms.ColumnHeader NameColumn;
        internal System.Windows.Forms.ColumnHeader AddressColumn;
        internal System.Windows.Forms.ColumnHeader StateColumn;
        internal System.Windows.Forms.TextBox MessageText;
        internal System.Windows.Forms.Button SendButton;
        private System.Windows.Forms.ToolStripMenuItem SettingMenuItem;
        private System.Windows.Forms.ContextMenuStrip UserMenu;
        private System.Windows.Forms.ToolStripMenuItem KickMenu;
        private System.Windows.Forms.NotifyIcon RoomIcon;
        private System.Windows.Forms.ContextMenuStrip RoomIconMenu;
        private System.Windows.Forms.ToolStripMenuItem ExitMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem OpenWinMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SettingMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem HideMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RoomCodeMenuItem;
    }
}