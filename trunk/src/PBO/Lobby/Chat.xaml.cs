using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PokemonBattleOnline.Network;
using PokemonBattleOnline.PBO.Elements;
using SoundPlayer = System.Media.SoundPlayer;

namespace PokemonBattleOnline.PBO.Lobby
{
  /// <summary>
  /// Interaction logic for Chat.xaml
  /// </summary>
  public partial class Chat : UserControl
  {
    public static Chat Current { get; private set; }
    static readonly SoundPlayer sound;
    static Chat()
    {
      sound = new SoundPlayer("..\\res\\chat.wav");
      try
      {
        sound.LoadAsync();
      }
      catch { }
    }
    static void PlaySound()
    {
      if (sound.IsLoadCompleted) sound.Play();
    }

    ScrollViewer scroll;

    public Chat()
    {
      InitializeComponent();
      whom.SelectionChanged += (sender, e) =>
        {
          if (whom.SelectedIndex > 0)
            ((TabItem)whom.SelectedItem).Foreground = System.Windows.Media.Brushes.Black;
        };
      Current = this;
      ClientController.PublicChat += OnPublicChat;
      ClientController.PrivateChat += OnPrivateChat;
    }

    ScrollViewer Scroll
    { 
      get
      {
        if (scroll == null) scroll = chatViewer.Template.FindName("PART_ContentHost", chatViewer) as ScrollViewer;
        return scroll;
      }
    }

    /// <summary>
    /// it's ok to reinit.
    /// </summary>
    private void Speak()
    {
      if (!string.IsNullOrEmpty(speaking.Text))
      {
        if (whom.SelectedIndex > 0)
        {
          var ti = (TabItem)whom.SelectedItem;
          PBOClient.Current.Controller.ChatPrivate((User)ti.Header, speaking.Text);
          ((TextBox)ti.Content).AppendText(PBOClient.Current.Controller.User.Name + ": " + speaking.Text + "\n");
        }
        else PBOClient.Current.Controller.ChatPublic(speaking.Text);
        speaking.Clear();
      }
    }
    private TabItem GetChatTab(User user)
    {
      foreach (TabItem ti in whom.Items)
        if (ti.Header == user) return ti;
      var t = new TabItem() { Header = user, Content = new TextBox() };
      whom.Items.Add(t);
      return t;
    }
    internal void Init()
    {
      speaking.Clear();
      chat.Inlines.Clear();
      whom.Items.MoveCurrentToFirst();
      for (int i = whom.Items.Count - 1; i != 0; i--) whom.Items.RemoveAt(i);
    }
    internal void NewChat(User user)
    {
      whom.SelectedItem = GetChatTab(user);
    }

    void OnPublicChat(string content, User user)
    {
      Run r = new Run(user.Name + ": " + content + "\n");
      r.Foreground = Cartes.GetChatBrush(user.Name);
      if (Scroll.ScrollableHeight - Scroll.ExtentHeight < 5)
      {
        chat.Inlines.Add(r);
        Scroll.ScrollToEnd();
      }
      else chat.Inlines.Add(r);
    }
    void OnPrivateChat(string content, User user)
    {
      //私聊要开新Tab
      TabItem ti = GetChatTab(user);
      ((TextBox)ti.Content).AppendText(user.Name + ": " + content + "\n");
      if (whom.SelectedItem != ti)
      {
        ti.Foreground = Elements.SBrushes.OrangeM;
        PlaySound();
      }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      Speak();
    }
    private void speaking_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter) Speak();
    }
    private void close_Click(object sender, RoutedEventArgs e)
    {
      var ti = Helper.GetParent<TabItem>((DependencyObject)sender);
      if (ti != null) whom.Items.Remove(ti);
    }
  }
}
