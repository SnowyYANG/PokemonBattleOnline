using System;
using System.Text;
using System.Xml;
using System.IO;
using System.Collections.Generic;

namespace LightStudio.Tactic.Messaging.Lobby
{
  public interface IServerService
  {
    void Login(int clientId, string name);
    void CompleteLogin(int clientId, Avatar avatar);
    void SendMessage(int clientId, int[] receivers, string content);
    void BroadcastMessage(int clientId, string content);
    void Logout(int clientId);
    void ChangeState(int clientId, UserState state);
    void ChangeInfo(int clientId, UserState state, string sign);
  }
  public static class ServerInterpreter
  {
    public static bool Interpret(int clientId, IMessage message, IServerService service)
    {
      switch (message.Header)
      {
        case MessageHeaders.LOGIN:
          MessageHelper.ResolveMessage(message, reader => service.Login(clientId, reader.ReadString()));
          break;
        case MessageHeaders.COMPLETELOGIN:
          MessageHelper.ResolveMessage(message, reader => service.CompleteLogin(clientId, reader.ReadAvatar()));
          break;
        case MessageHeaders.SEND_MESSAGE:
          MessageHelper.ResolveMessage(message, reader =>
            service.SendMessage(clientId, reader.ReadArray((Func<int>)reader.ReadUserId), reader.ReadString()));
          break;
        case MessageHeaders.BROADCAST:
          MessageHelper.ResolveMessage(message, reader =>
            service.BroadcastMessage(clientId, reader.ReadString()));
          break;
        case MessageHeaders.LOGOUT:
          service.Logout(clientId);
          break;
        case MessageHeaders.CHANGE_STATE:
          MessageHelper.ResolveMessage(message, reader => service.ChangeState(clientId, reader.ReadUserState()));
          break;
        case MessageHeaders.CHANGE_INFO:
          MessageHelper.ResolveMessage(message, reader =>
            service.ChangeInfo(clientId, reader.ReadUserState(), reader.ReadString()));
          break;
        default:
          return false;
      }
      return true;
    }
    public static IMessage OnLoginFailed()
    {
      return MessageHelper.BuildMessage(MessageHeaders.ON_LOGIN_FAILED);
    }
    public static IMessage OnLoginSucceeded(int id, User[] userList)
    {
      return MessageHelper.BuildMessage(MessageHeaders.ON_LOGIN_SUCCEEDED, writer =>
        {
          writer.WriteUserId(id);
          writer.WriteArray(userList, writer.WriteUser);
        });
    }
    public static IMessage OnUserLogined(User user)
    {
      return MessageHelper.BuildMessage(MessageHeaders.ON_USER_LOGINED, writer => writer.WriteUser(user));
    }
    public static IMessage OnUserExited(int id)
    {
      return MessageHelper.BuildMessage(MessageHeaders.ON_USER_EXITED, writer => writer.Write(id));
    }
    public static IMessage OnMessageReceived(int senderid, string content)
    {
      return MessageHelper.BuildMessage(MessageHeaders.ON_MESSAGE_RECEIVED, writer =>
        {
          writer.WriteUserId(senderid);
          writer.Write(content);
        });
    }
    public static IMessage OnBroadcastReceived(int senderid, string content)
    {
      return MessageHelper.BuildMessage(MessageHeaders.ON_BROADCAST_RECEIVED, writer =>
        {
          writer.WriteUserId(senderid);
          writer.Write(content);
        });
    }
    public static IMessage OnUserStateChanged(int senderid, UserState state)
    {
      return MessageHelper.BuildMessage(MessageHeaders.ON_USER_STATE_CHANGED, writer =>
        {
          writer.WriteUserId(senderid);
          writer.WriteUserState(state);
        });
    }
    public static IMessage OnUserInfoChanged(int senderid, UserState state, string sign)
    {
      return MessageHelper.BuildMessage(MessageHeaders.ON_USER_INFO_CHANGED, writer =>
        {
          writer.WriteUserId(senderid);
          writer.WriteUserState(state);
          writer.Write(sign);
        });
    }
  }//public static class LobbyServerInterpreter
}