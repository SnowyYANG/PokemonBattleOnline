using System;
using System.Text;
using System.Xml;
using System.IO;
using System.Collections.Generic;

namespace LightStudio.Tactic.Messaging.Lobby
{
  internal interface IClientService
  {
    void OnLoginFailed();
    void OnLoginSucceeded(int id, User[] userList);
    void OnUserLogined(User user);
    void OnUserExited(int id);
    void OnMessageReceived(int senderid, string content);
    void OnBroadcastReceived(int senderid, string content);
    void OnUserStateChanged(int senderid, UserState state);
    void OnUserInfoChanged(int senderid, UserState state, string sign);
  }
  internal sealed class ClientInterpreter
  {
    public static bool Interpret(IMessage message, IClientService service)
    {
      switch (message.Header)
      {
        case MessageHeaders.ON_LOGIN_FAILED:
          service.OnLoginFailed();
          break;
        case MessageHeaders.ON_LOGIN_SUCCEEDED:
          MessageHelper.ResolveMessage(message, reader =>
            service.OnLoginSucceeded(reader.ReadUserId(), reader.ReadArray((Func<User>)reader.ReadUser)));
          break;
        case MessageHeaders.ON_USER_LOGINED:
          MessageHelper.ResolveMessage(message, reader =>
            service.OnUserLogined(reader.ReadUser()));
          break;
        case MessageHeaders.ON_USER_EXITED:
          MessageHelper.ResolveMessage(message, reader =>
            service.OnUserExited(reader.ReadUserId()));
          break;
        case MessageHeaders.ON_MESSAGE_RECEIVED:
          MessageHelper.ResolveMessage(message, reader =>
            service.OnMessageReceived(reader.ReadUserId(), reader.ReadString()));
          break;
        case MessageHeaders.ON_BROADCAST_RECEIVED:
          MessageHelper.ResolveMessage(message, reader =>
            service.OnBroadcastReceived(reader.ReadUserId(), reader.ReadString()));
          break;
        case MessageHeaders.ON_USER_STATE_CHANGED:
          MessageHelper.ResolveMessage(message, reader =>
            service.OnUserStateChanged(reader.ReadUserId(), reader.ReadUserState()));
          break;
        case MessageHeaders.ON_USER_INFO_CHANGED:
          MessageHelper.ResolveMessage(message, reader =>
            service.OnUserInfoChanged(reader.ReadUserId(), reader.ReadUserState(), reader.ReadString()));
          break;
        default:
          return false;
      }
      return true;
    }

    public static IMessage Login(string name)
    {
      return MessageHelper.BuildMessage(MessageHeaders.LOGIN, writer => writer.Write(name));
    }
    public static IMessage CompleteLogin(Avatar avatar)
    {
      return MessageHelper.BuildMessage(MessageHeaders.COMPLETELOGIN, writer => writer.WriteAvatar(avatar));
    }
    public static IMessage SendMessage(int[] receivers, string content)
    {
      return MessageHelper.BuildMessage(MessageHeaders.SEND_MESSAGE, writer =>
        {
          writer.WriteArray(receivers, writer.WriteUserId);
          writer.Write(content);
        });
    }
    public static IMessage BroadcastMessage(string content)
    {
      return MessageHelper.BuildMessage(MessageHeaders.BROADCAST, writer => writer.Write(content));
    }
    public static IMessage Logout()
    {
      return MessageHelper.BuildMessage(MessageHeaders.LOGOUT);
    }
    public static IMessage ChangeState(UserState state)
    {
      return MessageHelper.BuildMessage(MessageHeaders.CHANGE_STATE, writer => writer.WriteUserState(state));
    }
    public static IMessage ChangeInfo(UserState state, string sign)
    {
      return MessageHelper.BuildMessage(MessageHeaders.CHANGE_INFO, writer =>
        {
          writer.WriteUserState(state);
          writer.Write(sign);
        });
    }
  }//public static class
}
