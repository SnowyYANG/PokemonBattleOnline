using System;
using System.Collections.Generic;
using System.Text;
using NetworkLib;
using PokemonBattle.BattleRoom.Client;
using System.Windows.Forms;

namespace PokemonBattle.RoomClient
{
    public class ChallengeManager
    {
        #region Varibles
        private ChallengeForm _myChallenge;
        private Dictionary<int, ChallengeForm> _challenges = new Dictionary<int, ChallengeForm>();

        private RoomClient _roomClient;
        private RoomBattleSetting _battleSetting;
        #endregion

        public ChallengeManager(RoomClient client)
        {
            _roomClient = client;
        }

        public ChallengeForm Challenge(User target)
        {
            _myChallenge = new ChallengeForm(target);

            _myChallenge.OK_Button.Click += new EventHandler(ConfirmChallenge);
            _myChallenge.FormClosing += new System.Windows.Forms.FormClosingEventHandler(_myChallenge_FormClosing);
            if (_battleSetting.SingleBan)
            {
                _myChallenge.ModeCombo.SelectedIndex = 1;
                _myChallenge.ModeCombo.Enabled = false;
            }
            if (_battleSetting.DoubleBan)
            {
                _myChallenge.ModeCombo.SelectedIndex = 0;
                _myChallenge.ModeCombo.Enabled = false;
            }
            if (_battleSetting.RandomOnly)
            {
                _myChallenge.RandomCheck.Checked = true;
                _myChallenge.RandomCheck.Enabled = false;
            }

            List<ChallengeForm> challenges = new List<ChallengeForm>(_challenges.Values);
            foreach (ChallengeForm challenge in challenges)
            {
                challenge.Cancel_Button.PerformClick();
            }

            return _myChallenge;
        }

        void _myChallenge_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if (_myChallenge != null)
            {
                CancelChallenge(sender, EventArgs.Empty);
            }
        }
        void ConfirmChallenge(object sender, EventArgs e)
        {
            _myChallenge.Cancel_Button.Click += new EventHandler(CancelChallenge);
            _roomClient.Challenge(_myChallenge.TargetInfo.Identity, _myChallenge.ChallengeInfo);
        }
        void CancelChallenge(object sender, EventArgs e)
        {
            ChallengeForm form = _myChallenge;

            _roomClient.CancelChallenge(_myChallenge.TargetInfo.Identity);
            _myChallenge = null;

            form.Close();
        }

        public ChallengeForm ReceiveChallenge(User from, ChallengeInfo challenge)
        {
            ChallengeForm challengeForm;
            challengeForm = new ChallengeForm(from, challenge);
            challengeForm.OK_Button.Click += new EventHandler(AcceptChallenge);
            challengeForm.Cancel_Button.Click += new EventHandler(RefuseChallenge);
            challengeForm.FormClosing += new System.Windows.Forms.FormClosingEventHandler(challengeForm_FormClosing);

            _challenges[from.Identity] = challengeForm;
            return challengeForm;
        }

        void challengeForm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            int identity = (sender as ChallengeForm).TargetInfo.Identity;
            if (_challenges.ContainsKey(identity))
            {
                RefuseChallenge(sender, EventArgs.Empty);
            }
        }
        void RefuseChallenge(object sender, EventArgs e)
        {
            ChallengeForm challenge = (sender as Button).FindForm() as ChallengeForm;

            int identity = challenge.TargetInfo.Identity;
            _roomClient.RefuseChallenge(identity);
            _challenges.Remove(identity);

            challenge.Close();
        }
        void AcceptChallenge(object sender, EventArgs e)
        {
            ChallengeForm challenge = (sender as Button).FindForm() as ChallengeForm;

            int identity = challenge.TargetInfo.Identity;
            _roomClient.AcceptChallenge(identity);
            _challenges.Remove(identity);

            challenge.Close();
        }

        public void ChallengeCanceled(int identity)
        {
            if (_challenges.ContainsKey(identity))
            {
                ChallengeForm challenge = _challenges[identity];
                _challenges.Remove(identity);
                challenge.Invoke(new VoidFunctionDelegate(delegate { challenge.Close(); }));
            }
        }
        public void ChallengeAccepted()
        {
            _roomClient.StartBattle(_myChallenge.TargetInfo.Identity, _myChallenge.ChallengeInfo);
            if (_myChallenge != null)
            {
                ChallengeForm form = _myChallenge;
                _myChallenge = null;
                form.Invoke(new VoidFunctionDelegate(delegate { form.Close(); }));
            }
        }
        public void ChallengeRefused()
        {
            if (_myChallenge != null)
            {
                ChallengeForm form = _myChallenge;
                _myChallenge = null;
                form.Invoke(new VoidFunctionDelegate(
                    delegate
                    {
                        form.Close();
                        MessageBox.Show("对方拒绝了你的挑战", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    ));
            }
        }
        public bool UserLogout(int identity)
        {
            if (_myChallenge != null && _myChallenge.TargetInfo.Identity  == identity)
            {
                ChallengeRefused();
                return true;
            }
            else if (_challenges.ContainsKey(identity))
            {
                ChallengeCanceled(identity);
            }
            return false;
        }

        public void SetSetting(RoomBattleSetting setting)
        {
            _battleSetting = setting;
        }
    }
}
