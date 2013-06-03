using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PokemonBattle.BattleRoom.Client;
using System.Text.RegularExpressions;

namespace PokemonBattle.RoomClient
{
    public partial class UserInfoForm : Form
    {
        private User _userInfo;
        private int _imageIndex;

        public UserInfoForm(User userInfo)
        {
            InitializeComponent();
            _userInfo = userInfo;
        }

        private void UserInfoForm_Load(object sender, EventArgs e)
        {
            _imageIndex = _userInfo.ImageKey;
            NameText.Text = _userInfo.Name;
            UpdateImage();
        }

        private void ImageUpButton_Click(object sender, EventArgs e)
        {
            _imageIndex++;
            UpdateImage();
        }

        private void ImageDownButton_Click(object sender, EventArgs e)
        {
            _imageIndex--;
            UpdateImage();
        }

        private void UpdateImage()
        {
            MyImage.Image = UserImage.Images[_imageIndex];
            if (_imageIndex == 0)
            {
                ImageDownButton.Enabled = false;
                ImageUpButton.Enabled = true;
            }
            else if (_imageIndex == UserImage.Images.Count - 1)
            {
                ImageDownButton.Enabled = true;
                ImageUpButton.Enabled = false;
            }
            else
            {
                ImageDownButton.Enabled = true;
                ImageUpButton.Enabled = true;
            }
        }

        private void OK_Button_Click(object sender, EventArgs e)
        {
            _userInfo.Name = NameText.Text;
            _userInfo.ImageKey = (byte)_imageIndex;
        }

        private void NameText_TextChanged(object sender, EventArgs e)
        {
            if (RoomClientForm.IsEmptyString(NameText.Text))
            {
                OK_Button.Enabled = false;
            }
            else
            {
                OK_Button.Enabled = true;
            }
        }
    }
}
