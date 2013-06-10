using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net;
using LightStudio.Tactic.Logging;
using LightStudio.Tactic.Messaging.Primitive;
using LightStudio.Tactic.Messaging.Lobby;

namespace LightStudio.Tactic.Messaging
{
  public class Client : ClientBase, IClientInnerService
  {
    public static Client NewTcpClient(IPAddress serverAddress, int serverPort)
    {
      return new Client(Factory.TcpMessageClient(serverAddress, serverPort));
    }
    private static void LogUserNotFound(int id)
    {
      LoggerFacade.LogWarn(string.Format("LobbyClient : failed to find user with id {0}", id));
    }

    public event Action LoginCompleted = delegate { };
    public event Action LoginFailed = delegate { };
    public event Action<int> UserChanged;
    public event Action<User, string> BroadcastReceived = delegate { };
    
    private readonly Collection<ClientService> services;
    private readonly Dictionary<byte, ClientService> servicesIndex;
    private ConcurrentDictionary<int, User> users;
    private int userId;
    private User user;
    private string userName;
    private int avatar;

    public Client(IMessageClient client) : base(client) //though i dont wanna sealed class
    {
      userId = -1;
      users = new ConcurrentDictionary<int, User>();
      services = new Collection<ClientService>();
      servicesIndex = new Dictionary<byte, ClientService>();
    }

    public bool IsLogined { get; private set; }
    public User User
    { get { return user; } }
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
      Send(ClientInterpreter.Login(userName, avatar));
    }
    internal void SendMessage(byte header, Action<BinaryWriter> write, params int[] receivers)
    {
      using (var stream = new MemoryStream())
      {
        string content;
        {
          var writer = new BinaryWriter(stream);
          writer.Write(header);
          if (write != null) write(writer);
          content = Convert.ToBase64String(stream.ToArray());
        }

        List<int> remote = new List<int>();
        bool hasRemote = false;
        foreach (int i in receivers)
#if DeadLock
          if (i == User.Id) OnMessageReceived(User, content);
          else
#endif
          {
            remote.Add(i);
            hasRemote = true;
          }
        if (hasRemote) Send(ClientInterpreter.SendMessage(remote, content));
      }
    }
    public void BroadcastMessage(string content)//我了个去Broadcast这种词也能用在Client上...SendAll就SendAll
    {
      if (IsLogined) Send(ClientInterpreter.BroadcastMessage(content));
    }
    protected void ReadMessage(string content, Action<byte, BinaryReader> read)
    {
      using (var stream = new MemoryStream(Convert.FromBase64String(content)))
      {
        var reader = new BinaryReader(stream);
        read(reader.ReadByte(), reader);
      }
    }

    public void Login(string name, int av)
    {
      userName = name;
      avatar = av;
      Connect();
    }
    public void Logout()
    {
      if (IsLogined)
      {
        IsLogined = false;
        Send(ClientInterpreter.Logout()); //blocked?
      }
      //thread safe? I think in this issue the server should close the connect
      //also, the connect might be closed before logout sendout
      //so it should be the server close the connect, client can close the connect after a time-out waiting
    }
    public User GetUser(int userId)
    {
      return users.ValueOrDefault(userId);
    }
    public void ChangeUserState(UserState state, string sign = null)
    {
      if (sign != null && sign != User.Sign) Send(ClientInterpreter.ChangeInfo(state, sign));
      else if (state != User.State) Send(ClientInterpreter.ChangeState(state));
    }

    internal void RegisterService(ClientService service, params byte[] receiveMessageHeaders)
    {
      services.Add(service);
      foreach (byte h in receiveMessageHeaders) servicesIndex.Add(h, service);
    }
    #region ILobbyInnerService
    protected virtual void OnMessageReceived(User sender, string content)
    {
      if (!string.IsNullOrEmpty(content))
        ReadMessage(content, (header, reader) =>
        {
          var s = servicesIndex.ValueOrDefault(header);
          if (s != null) s.ReadMessage(sender, header, reader);
        });
    }
    protected virtual void OnLoginFailed()
    {
      LoggerFacade.LogDebug("LobbyClient : login failed");
      LoginFailed();
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
    void IClientInnerService.OnLoginFailed()
    {
      OnLoginFailed();
    }
    void IClientInnerService.OnLoginSucceeded(int id, User[] userList)
    {
      //users = new ConcurrentDictionary<int, User>(1, userList.Length);
      userId = id;
      users.Clear();
      foreach (var user in userList) users[user.Id] = user;
      LoggerFacade.LogDebug("LobbyClient : logining");
      //LoginSucceeded();
    }
    void IClientInnerService.OnUserLogined(User user)
    {
      if (user.Id == userId)
      {
        IsLogined = true;
        this.user = user;
        LoginCompleted();
      }
      users[user.Id] = user;
      LoggerFacade.LogDebug(string.Format("LobbyClient : user {0} logined", user.Name));
      UserChanged(user.Id);
    }
    void IClientInnerService.OnUserExited(int id)
    {
      OnUserExited(id);
    }
    void IClientInnerService.OnMessageReceived(int senderid, string content)
    {
      User sender;
      if (!users.TryGetValue(senderid, out sender)) LogUserNotFound(senderid);
      else OnMessageReceived(sender, content);
    }
    void IClientInnerService.OnBroadcastReceived(int senderid, string content)
    {
      OnBroadcastReceived(senderid, content);
    }
    void IClientInnerService.OnUserStateChanged(int senderid, UserState state)
    {
      OnUserStateChanged(senderid, state);
    }
    void IClientInnerService.OnUserInfoChanged(int senderid, UserState state, string sign)
    {
      OnUserInfoChanged(senderid, state, sign);
    }
    #endregion

    protected override void DisposeManagedResources()
    {
      base.DisposeManagedResources();

      LoginCompleted = delegate { };
      LoginFailed = delegate { };
      BroadcastReceived = delegate { };
      UserChanged = delegate { };

      foreach (ClientService s in services) s.Dispose();
      
      Disconnect(); 
    }
  }
}
