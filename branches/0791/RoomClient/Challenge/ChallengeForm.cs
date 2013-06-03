using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PokemonBattle.BattleRoom.Client;
using PokemonBattle.BattleNetwork;

namespace PokemonBattle.RoomClient
{
    public partial class ChallengeForm : Form
    {
        private User _targetInfo;
        private ChallengeInfo _challengeInfo;
        private bool _myChallenge;

        public User TargetInfo
        {
            get { return _targetInfo; }
        }
        public ChallengeInfo ChallengeInfo
        {
            get { return _challengeInfo; }
        }

        public ChallengeForm(User target)
        {
            InitializeComponent();

            _targetInfo = target;
            _challengeInfo = new ChallengeInfo();
            _challengeInfo.Rules = new BattleRuleSequence();
            _myChallenge = true;

            OK_Button.Text = "确定(&O)";
            Cancel_Button.Text = "取消(&A)";
            NameLabel.Text = string.Format("对象 : {0}", _targetInfo.Name);
            CustomLabel.Text = string.Format("自定数据 : {0}", _targetInfo.CustomDataInfo);
            ModeCombo.SelectedIndex = 0;
            LinkCombo.SelectedIndex = 0;

            Cancel_Button.Click += Cancel_Button_Click;
        }
        public ChallengeForm(User from, ChallengeInfo challenge)
        {
            InitializeComponent();

            _targetInfo = from;
            _challengeInfo = challenge;
            _myChallenge = false;

            LinkCombo.Enabled = false;
            switch (challenge.LinkMode)
            {
                case BattleLinkMode.Agent:
                    LinkCombo.SelectedIndex = 1;
                    break;
                case BattleLinkMode.Direct:
                    LinkCombo.SelectedIndex = 0;
                    break;
            }
            ModeCombo.Enabled = false;
            switch (challenge.BattleMode)
            {
                case BattleMode.Single:
                    ModeCombo.SelectedIndex = 0;
                    break;
                case BattleMode.Double:
                    ModeCombo.SelectedIndex = 1;
                    break;
            }
            PPUpCheck.Enabled = false;
            if (challenge.Rules.Elements.Contains(BattleRule.PPUp)) PPUpCheck.Checked = true;
            RandomCheck.Enabled = false;
            if (challenge.Rules.Elements.Contains(BattleRule.Random)) RandomCheck.Checked = true;

            OK_Button.Text = "接受(&A)";
            Cancel_Button.Text = "拒绝(&R)";
            NameLabel.Text = string.Format("来自 : {0}", _targetInfo.Name);
            CustomLabel.Text = string.Format("自定数据 : {0}", _targetInfo.CustomDataInfo);
        }

        private void OK_Button_Click(object sender, EventArgs e)
        {
            if (_myChallenge)
            {
                _challengeInfo.BattleMode = new BattleMode[] { BattleMode.Single, BattleMode.Double }[ModeCombo.SelectedIndex];
                _challengeInfo.LinkMode = new BattleLinkMode[] { BattleLinkMode.Direct,BattleLinkMode.Agent }[LinkCombo.SelectedIndex];
                if (PPUpCheck.Checked) _challengeInfo.Rules.Elements.Add(BattleRule.PPUp);
                if (RandomCheck.Checked) _challengeInfo.Rules.Elements.Add(BattleRule.Random);

                OK_Button.Enabled = false;
                LinkCombo.Enabled = false;
                ModeCombo.Enabled = false;
                PPUpCheck.Enabled = false;
                RandomCheck.Enabled = false;
                Cancel_Button.Click -= Cancel_Button_Click;
            }
        }

        void Cancel_Button_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
