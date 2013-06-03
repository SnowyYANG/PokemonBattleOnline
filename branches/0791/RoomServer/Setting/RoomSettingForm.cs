using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace PokemonBattle.RoomServer
{
    public partial class RoomSettingForm : Form
    {
        private RoomSetting _setting;
        public RoomSettingForm(RoomSetting setting)
        {
            InitializeComponent();
            _setting = setting;
            if (_setting.BattleSetting.MoveInterval == 0) _setting.BattleSetting.MoveInterval = 120;
            if (string.IsNullOrEmpty(_setting.Name))
            {
                TimeNumberic.Value = _setting.BattleSetting.MoveInterval;
                Cancel_Button.Enabled = false;
                OK_Button.Enabled = false;
            }
            else
            {
                NameText.Text = _setting.Name;
                DescText.Text = _setting.Description;
                WelcomeText.Text = _setting.WelcomeMessage;
                LogonListCheck.Checked = _setting.LogonList;
                MaxUserNumberic.Value = _setting.MaxUser;
                SingleCheck.Checked = _setting.BattleSetting.SingleBan;
                DoubleCheck.Checked = _setting.BattleSetting.DoubleBan;
                FourPlayerCheck.Checked = _setting.BattleSetting.FourPlayerBan;
                RandomCheck.Checked = _setting.BattleSetting.RandomOnly;
                TimeNumberic.Value = _setting.BattleSetting.MoveInterval;
                VersionText.Text = _setting.BattleSetting.Version;
            }
            Icon = Properties.Resources.PokemonBall;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            _setting.Name = NameText.Text;
            _setting.Description = DescText.Text;
            _setting.WelcomeMessage = WelcomeText.Text;
            _setting.LogonList = LogonListCheck.Checked;
            _setting.MaxUser = (byte)MaxUserNumberic.Value;
            _setting.BattleSetting.SingleBan = SingleCheck.Checked;
            _setting.BattleSetting.DoubleBan = DoubleCheck.Checked;
            _setting.BattleSetting.FourPlayerBan = FourPlayerCheck.Checked;
            _setting.BattleSetting.RandomOnly = RandomCheck.Checked;
            _setting.BattleSetting.MoveInterval = (int)TimeNumberic.Value;
            _setting.BattleSetting.Version = VersionText.Text;
        }

        private void NameText_TextChanged(object sender, EventArgs e)
        {
            if (IsEmptyString(NameText.Text))
            {
                OK_Button.Enabled = false;
            }
            else
            {
                OK_Button.Enabled = true;
            }
        }
        private bool IsEmptyString(string str)
        {
            return Regex.IsMatch(str, "^\\s*$");
        }
    }
}
