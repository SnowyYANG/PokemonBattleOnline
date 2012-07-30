using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using LightStudio.Tactic.Logging;
using LightStudio.Tactic.Messaging.Lobby;
using LightStudio.Tactic.Messaging.Primitive;

namespace LightStudio.Tactic.Messaging
{
  public class Server<T> : ServerBase, IServerService where T : new()
  {
    public event Action<int> UserChanged;
    public event Action<int, string> MessageBroadcast;
    private readonly Dictionary<int, User<T>> users;

    public Server(IMessageServer server) : base(server)
    {
      users = new Dictionary<int, User<T>>();
    }

    public IEnumerable<IUser<T>> Users
    { get { return users.Values; } }

    private void OnUserChanged(int userId)
    {
      if (UserChanged != null) UserChanged(userId);
    }
    private void OnMessageBroadcast(int userId, string content)
    {
      if (MessageBroadcast != null) MessageBroadcast(userId, content);
    }
    protected override void OnReceive(int userId, IMessage message)
    {
      if (!ServerInterpreter.Interpret(userId, message, this))
      {
        LoggerFacade.LogWarn(string.Format("LobbyServer.OnReceive : cannot handle message {0}", message.Header));
        return;
      }
    }
    protected override void OnClientExited(int userId)
    {
      //thread safe?
      User<T> user;
      users.TryGetValue(userId, out user);
      if (user != null)
      {
        users.Remove(userId);
        LoggerFacade.LogDebug(string.Format("LobbyServer: user {0} exited", user.Name));
        Broadcast(ServerInterpreter.OnUserExited(userId));
        OnUserChanged(userId);
      }
    }
    public IUser<T> GetUser(int id)
    {
      return users.ValueOrDefault(id);
    }

    #region ILobbyServerService
    void IServerService.Login(int clientId, string name)
    {
      if (!users.ContainsKey(clientId) && Users.All(u => u.Name != name))
      {
        LoggerFacade.LogDebug(string.Format("LobbyServer: user {0} is logining", name));
        Send(clientId, ServerInterpreter.OnLoginSucceeded(clientId, users.Values.ToArray()));
        users.Add(clientId, new User<T>(clientId, name));
        //OnUserChanged(clientId); //?
      }
      else
      {
        Send(clientId, ServerInterpreter.OnLoginFailed());
      }
    }
    void IServerService.CompleteLogin(int clientId, Avatar avatar)
    {
      if (users.ContainsKey(clientId))
      {
        var user = users[clientId];
        user.Avatar = avatar;
        user.State = UserState.Normal;/////////
        LoggerFacade.LogDebug(string.Format("LobbyServer: user {0} logined", user.Name));
        Broadcast(ServerInterpreter.OnUserLogined(user));
        OnUserChanged(clientId);
      }
      else
        LoggerFacade.LogDebug("LobbyServer: invalid operation - CompleteLogin");
    }
    void IServerService.SendMessage(int clientId, int[] receivers, string content)
    {
      Send(receivers, ServerInterpreter.OnMessageReceived(clientId, content));
    }
    void IServerService.BroadcastMessage(int clientId, string content)
    {
      Broadcast(ServerInterpreter.OnBroadcastReceived(clientId, content));
      OnMessageBroadcast(clientId, content);
    }
    void IServerService.Logout(int clientId)
    {
      OnClientExited(clientId);
      OnUserChanged(clientId);
    }
    void IServerService.ChangeState(int clientId, UserState state)
    {
      //thread safe?
      users[clientId].State = state;
      Broadcast(ServerInterpreter.OnUserStateChanged(clientId, state));
      OnUserChanged(clientId);
    }
    void IServerService.ChangeInfo(int clientId, UserState state, string sign)
    {
      var user = users[clientId];
      if (user != null)
      {
        user.State = state;
        user.Sign = sign;
        Broadcast(ServerInterpreter.OnUserInfoChanged(clientId, state, sign));
        OnUserChanged(clientId);
      }
    }
    #endregion
  }
}
