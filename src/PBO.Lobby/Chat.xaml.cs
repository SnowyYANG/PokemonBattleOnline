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
using LightStudio.PokemonBattle.Messaging;
using LightStudio.PokemonBattle.PBO.UIElements;
using SoundPlayer = System.Media.SoundPlayer;
using User = LightStudio.Tactic.Messaging.Lobby.User;

namespace LightStudio.PokemonBattle.PBO.Lobby
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
      sound = new SoundPlayer("Resources\\chat.wav");
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

    PokemonLobbyClient controller;
    ScrollViewer scroll;
    Dictionary<int, TabItem> chatTabs;
    string userName;

    public Chat()
    {
      InitializeComponent();
      chatTabs = new Dictionary<int, TabItem>();
      whom.SelectionChanged += (sender, e) =>
        {
          if (whom.SelectedIndex > 0)
            ((TabItem)whom.SelectedItem).Foreground = System.Windows.Media.Brushes.Black;
        };
      Current = this;
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
          foreach (KeyValuePair<int, TabItem> p in chatTabs)
            if (whom.SelectedItem == p.Value)
            {
              controller.Chat(speaking.Text, p.Key);
              ((TextBox)p.Value.Content).AppendText(userName + ": " + speaking.Text + "\n");
            }
        }
        else controller.BroadcastMessage(speaking.Text);
        speaking.Clear();
      }
    }
    private TabItem GetChatTab(User user)
    {
      TabItem ti;
      if (!chatTabs.ContainsKey(user.Id))
      {
        ti = new TabItem();
        ti.Header = new { Name = user.Name, CloseCommand = new SimpleCommand(() =>
          {
            whom.Items.Remove(ti);
            chatTabs.Remove(user.Id);
          }) };
        ti.Content = new TextBox();
        chatTabs.Add(user.Id, ti);
        whom.Items.Add(ti);
      }
      else ti = chatTabs[user.Id];
      return ti;
    }
    internal void Init(PokemonLobbyClient client)
    {
      if (controller != null)
      {
        //那个Intern的string岂不是悲剧了 放不出来了
        controller.ChatMessageReceived -= controller_ChatMessageReceived;
      }
      if (client != null)
      {
        speaking.Clear();
        chat.Inlines.Clear();
        userName = client.User.Name; //userName = String.Intern(client.User.Name); //还是算了
        controller = client;
        controller.BroadcastReceived += controller_BroadcastReceived;
        controller.ChatMessageReceived += controller_ChatMessageReceived;
      }
      else
      {
        IsEnabled = false;
      }
    }
    internal void NewChat(User user)
    {
      whom.SelectedItem = GetChatTab(user);
    }

    void controller_BroadcastReceived(User user, string content)
    {
      UIDispatcher.Invoke(() =>
        {
          Run r = new Run(user.Name + ": " + content + "\n");
          r.Foreground = UserVM.GetChatBrush(user.Name);
          if (Scroll.ScrollableHeight - Scroll.ExtentHeight < 5)
          {
            chat.Inlines.Add(r);
            Scroll.ScrollToEnd();
          }
          else
            chat.Inlines.Add(r);
        });
    }
    void controller_ChatMessageReceived(object sender, ChatMessageReceivedEventArgs e)
    {
      //私聊要开新Tab
      UIDispatcher.Invoke(() =>
        {
          TabItem ti = GetChatTab(e.UserInfo);
          ((TextBox)ti.Content).AppendText(e.UserInfo.Name + ": " + e.Content + "\n");
          if (whom.SelectedItem != ti)
          {
            ti.Foreground = UIElements.Brushes.OrangeM;
            PlaySound();
          }
        });
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      Speak();
    }
    private void speaking_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter) Speak();
    }
  }
}
