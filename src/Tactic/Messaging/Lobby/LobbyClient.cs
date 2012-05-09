using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using System.IO;
using LightStudio.Tactic.Logging;

namespace LightStudio.Tactic.Messaging.Lobby
{
  /// <summary>
  /// Subclasses should NEVER implement ILobbyClientService again!
  /// </summary>
  public class LobbyClient : ClientBase, IClientService
  {
    private static void LogUserNotFound(int id)
    {
      LoggerFacade.LogWarn(string.Format("LobbyClient : failed to find user with id {0}", id));
    }

    public event Action LoginCompleted = delegate { };
    public event Action LoginFailed = delegate { };
    public event Action<int> UserChanged;
    public event Action<User, string> BroadcastReceived = delegate { };
    private ConcurrentDictionary<int, User> users;
    private int userId;
    private string userName;
    private Avatar avatar;

    public LobbyClient(IMessageClient client) : base(client)
    {
      userId = -1;
      users = new ConcurrentDictionary<int, User>();
    }

    public bool IsLogined { get; private set; }
    public User User { get; private set; }
    public IEnumerable<User> Users
    { get { return users.Values; } }

    protected override void OnReceive(IMessage message)
    {
      ClientInterpreter.Interpret(message, this);
    }
    protected override void OnConnected()
    {
      base.OnConnected();
      StartReceive();
      Send(ClientInterpreter.Login(userName));
    }
    protected void SendMessage(byte header, params int[] receivers)
    {
      SendMessage(header, null, receivers);
    }
    protected void SendMessage(byte header, Action<BinaryWriter> write, params int[] receivers)
    {
      using (var stream = new MemoryStream())
      {
        var writer = new BinaryWriter(stream);
        writer.Write(header);
        if (write != null) write(writer);
        Send(ClientInterpreter.SendMessage(receivers, Convert.ToBase64String(stream.ToArray())));
      }
    }
    protected void ReadMessage(string content, Action<byte, BinaryReader> read)
    {
      using (var stream = new MemoryStream(Convert.FromBase64String(content)))
      {
        var reader = new BinaryReader(stream);
        read(reader.ReadByte(), reader);
      }
    }
    public void Login(string name, byte avatarId, string url = null)
    {
      userName = name;
      avatar = new Avatar(avatarId, url);
      Connect();
    }
    public void Logout()
    {
      if (IsLogined) Send(ClientInterpreter.Logout()); //blocked?
      Disconnect(); //thread safe? I think in this issue the service should close the connect
    }
    public void BroadcastMessage(string content)//我了个去Broadcast这种词也能用在Client上...SendAll就SendAll
    {
      if (IsLogined) Send(ClientInterpreter.BroadcastMessage(content));
    }
    public User GetUser(int userId)
    {
      return users.ValueOrDefault(userId);
    }
    public void ChangeState(UserState state, string sign = null)
    {
      if (sign != null && sign != User.Sign) Send(ClientInterpreter.ChangeInfo(state, sign));
      else if (state != User.State) Send(ClientInterpreter.ChangeState(state));
    }

    #region ILobbyClientService
    protected virtual void OnMessageReceived(int senderid, string content)
    {
      User sender;
      if (!users.TryGetValue(senderid, out sender))
      {
        LogUserNotFound(senderid);
        return;
      }
    }
    protected virtual void OnLoginFailed()
    {
      LoggerFacade.LogDebug("LobbyClient : login failed");
      LoginFailed();
    }
    protected virtual void OnLoginSucceeded(int id, User[] userList)
    {
      Send(ClientInterpreter.CompleteLogin(avatar));
      //users = new ConcurrentDictionary<int, User>(1, userList.Length);
      userId = id;
      users.Clear();
      foreach (var user in userList) users[user.Id] = user;
      LoggerFacade.LogDebug("LobbyClient : logining");
      //LoginSucceeded();
    }
    protected virtual void OnUserLogined(User user)
    {
      #warning event order, think twice...
      if (user.Id == userId)
      {
        IsLogined = true;
        User = user;
        LoginCompleted();
      }      
      users[user.Id] = user;
      LoggerFacade.LogDebug(string.Format("LobbyClient : user {0} logined", user.Name));
      UserChanged(user.Id);
    }
    protected virtual void OnUserExited(int id)
    {
      User user;
      if (!users.TryRemove(id, out user))
      {
        LogUserNotFound(id);
        return;
      }
      LoggerFacade.LogDebug(string.Format("LobbyClient : user {0} exited", user.Name));
      UserChanged(id);
    }
    protected virtual void OnBroadcastReceived(int senderid, string content)
    {
      User sender;
      if (users.TryGetValue(senderid, out sender))
      {
        BroadcastReceived(sender, content);
      }
    }
    protected virtual void OnUserStateChanged(int senderid, UserState state)
    {
      var user = users[senderid];
      if (user != null)
      {
        user.State = state;
        UserChanged(senderid);
      }
    }
    protected virtual void OnUserInfoChanged(int senderid, UserState state, string sign)
    {
      var user = users[senderid];
      if (user != null)
      {
        user.State = state;
        user.Sign = sign;
        UserChanged(senderid);
      }
    }
    void IClientService.OnLoginFailed()
    {
      this.OnLoginFailed();
    }
    void IClientService.OnLoginSucceeded(int id, User[] userList)
    {
      this.OnLoginSucceeded(id, userList);
    }
    void IClientService.OnUserLogined(User user)
    {
      this.OnUserLogined(user);
    }
    void IClientService.OnUserExited(int id)
    {
      this.OnUserExited(id);
    }
    void IClientService.OnMessageReceived(int senderid, string content)
    {
      this.OnMessageReceived(senderid, content);
    }
    void IClientService.OnBroadcastReceived(int senderid, string content)
    {
      this.OnBroadcastReceived(senderid, content);
    }
    void IClientService.OnUserStateChanged(int senderid, UserState state)
    {
      OnUserStateChanged(senderid, state);
    }
    void IClientService.OnUserInfoChanged(int senderid, UserState state, string sign)
    {
      OnUserInfoChanged(senderid, state, sign);
    }
    #endregion

    protected override void DisposeManagedResources()
    {
      base.DisposeManagedResources();
    }
  }
}
