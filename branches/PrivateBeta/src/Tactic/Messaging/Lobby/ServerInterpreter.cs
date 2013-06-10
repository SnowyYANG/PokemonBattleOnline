using System;
using System.Text;
using System.Xml;
using System.IO;
using System.Collections.Generic;

namespace LightStudio.Tactic.Messaging.Lobby
{
  public interface IServerService
  {
    void Login(int clientId, string name, int avatar);
    void SendMessage(int clientId, int[] receivers, string content);
    void BroadcastMessage(int clientId, string content);
    void Logout(int clientId);
    void ChangeState(int clientId, UserState state);
    void ChangeInfo(int clientId, UserState state, string sign);
  }
  internal static class ServerInterpreter
  {
    public static bool Interpret(int clientId, IMessage message, IServerService service)
    {
      switch (message.Header)
      {
        case MessageHeaders.LOGIN:
          message.Resolve(reader => service.Login(clientId, reader.ReadString(), reader.ReadAvatar()));
          break;
        case MessageHeaders.SEND_MESSAGE:
          message.Resolve(reader =>
            service.SendMessage(clientId, reader.ReadArray((Func<int>)reader.ReadUserId), reader.ReadString()));
          break;
        case MessageHeaders.BROADCAST:
          message.Resolve(reader =>
            service.BroadcastMessage(clientId, reader.ReadString()));
          break;
        case MessageHeaders.LOGOUT:
          service.Logout(clientId);
          break;
        case MessageHeaders.CHANGE_STATE:
          message.Resolve(reader => service.ChangeState(clientId, reader.ReadUserState()));
          break;
        case MessageHeaders.CHANGE_INFO:
          message.Resolve(reader =>
            service.ChangeInfo(clientId, reader.ReadUserState(), reader.ReadString()));
          break;
        default:
          return false;
      }
      return true;
    }
    private static IMessage BuildMessage(string header, Action<BinaryWriter> buildContent = null)
    {
      return new TextMessage(header, buildContent);
    }
    public static IMessage OnLoginFailed()
    {
      return BuildMessage(MessageHeaders.ON_LOGIN_FAILED);
    }
    public static IMessage OnLoginSucceeded(int id, User[] userList)
    {
      return BuildMessage(MessageHeaders.ON_LOGIN_SUCCEEDED, writer =>
        {
          writer.WriteUserId(id);
          writer.WriteArray(userList, writer.WriteUser);
        });
    }
    public static IMessage OnUserLogined(User user)
    {
      return BuildMessage(MessageHeaders.ON_USER_LOGINED, writer => writer.WriteUser(user));
    }
    public static IMessage OnUserExited(int id)
    {
      return BuildMessage(MessageHeaders.ON_USER_EXITED, writer => writer.Write(id));
    }
    public static IMessage OnMessageReceived(int senderid, string content)
    {
      return BuildMessage(MessageHeaders.ON_MESSAGE_RECEIVED, writer =>
        {
          writer.WriteUserId(senderid);
          writer.Write(content);
        });
    }
    public static IMessage OnBroadcastReceived(int senderid, string content)
    {
      return BuildMessage(MessageHeaders.ON_BROADCAST_RECEIVED, writer =>
        {
          writer.WriteUserId(senderid);
          writer.Write(content);
        });
    }
    public static IMessage OnUserStateChanged(int senderid, UserState state)
    {
      return BuildMessage(MessageHeaders.ON_USER_STATE_CHANGED, writer =>
        {
          writer.WriteUserId(senderid);
          writer.WriteUserState(state);
        });
    }
    public static IMessage OnUserInfoChanged(int senderid, UserState state, string sign)
    {
      return BuildMessage(MessageHeaders.ON_USER_INFO_CHANGED, writer =>
        {
          writer.WriteUserId(senderid);
          writer.WriteUserState(state);
          writer.Write(sign);
        });
    }
  }//public static class LobbyServerInterpreter
}