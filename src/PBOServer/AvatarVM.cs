using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AvatarModel = LightStudio.Tactic.Messaging.Lobby.Avatar;

namespace LightStudio.PokemonBattle.PBO
{
  /// <summary>
  /// for player only
  /// </summary>
  public class AvatarVM : INotifyPropertyChanged
  {
    public static readonly BitmapImage[] InnerAvatars;
    static BitmapImage GetImage(string filename)
    {
      return new BitmapImage(new Uri(@"pack://application:,,,/Server;component/images/" + filename, UriKind.Absolute));
    }
    static AvatarVM()
    {
      InnerAvatars = new BitmapImage[5];
      InnerAvatars[0] = GetImage("Avatars/0.png");
      InnerAvatars[1] = GetImage("Avatars/1.png");
      InnerAvatars[2] = GetImage("Avatars/2.png");
      InnerAvatars[3] = GetImage("Avatars/3.png");
      InnerAvatars[4] = GetImage("Avatars/4.png");
    }
    
    private static BitmapImage GetAvatar(Uri uri)
    {
      return new BitmapImage(uri);
    }
    private static BitmapImage GetAvatar(byte id, string url)
    {
      BitmapImage i;
      i = GetAvatar(url);
      if (i == null) i = GetAvatar(id);
      return i;
    }
    public static BitmapImage GetAvatar(AvatarModel avatar)
    {
      return GetAvatar(avatar.InnerAvatarId, avatar.Url);
    }
    public static BitmapImage GetAvatar(byte id)
    {
      return InnerAvatars[id % InnerAvatars.Length];
    }
    public static BitmapImage GetAvatar(string url)
    {
      BitmapImage i = null;
      try
      {
        if (!string.IsNullOrWhiteSpace(url))
          i = GetAvatar(new Uri(url));
      }
      catch { }
      return i;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    byte id;
    string url;
    BitmapImage urlAvatar;
    
    public AvatarVM(AvatarModel avatar, bool innerOnly = false)
      : this(avatar.InnerAvatarId, avatar.Url, innerOnly)
    {
    }
    public AvatarVM(byte innerId, string url, bool innerOnly = false)
    {
      this.id = innerId;
      if (innerOnly) this.url = url ?? string.Empty;
      else
      {
        this.url = AvatarModel.FormatUrl(url);
        this.urlAvatar = GetAvatar(this.url);
      }
      //if (urlAvatar != null)
      //  urlAvatar.DownloadCompleted += new EventHandler(avatar_DownloadCompleted);
    }

    public byte InnerAvatarId
    { 
      get { return id; }
      set
      {
        if (id != value)
        {
          id = value;
          OnPropertyChanged("InnerAvatarId");
          OnPropertyChanged("InnerAvatar");
          if (string.IsNullOrWhiteSpace(url)) OnPropertyChanged("Avatar");
        }
      }
    }
    public string Url
    { get { return url; } }
    public ImageSource InnerAvatar
    { get { return GetAvatar(id); } }
    public ImageSource Avatar
    { get { return urlAvatar ?? InnerAvatar; } }

    void OnPropertyChanged(string propertyName)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
    //void avatar_DownloadCompleted(object sender, EventArgs e)
    //{
    //  ((BitmapImage)sender).DownloadCompleted -= avatar_DownloadCompleted;
    //  OnPropertyChanged("Avatar");
    //}
    public void SetUrl(string url)
    {
      this.url = AvatarModel.FormatUrl(url);
      OnPropertyChanged("Url");
      //urlAvatar = null; //换成“尝试下载中...”图片 试试看xaml里Binding IsDownloading
      //if (!string.IsNullOrWhiteSpace(url))
      //{
      //  urlAvatar = GetAvatar(this.url);
      //  if (urlAvatar != null)
      //    urlAvatar.DownloadCompleted += avatar_DownloadCompleted; //thread unsafe maybe memory-leak, but no WeakEvent
      //}
      urlAvatar = GetAvatar(this.url);
      OnPropertyChanged("Avatar");
    }
  }
}
