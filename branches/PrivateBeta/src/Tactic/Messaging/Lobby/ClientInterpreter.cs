using System;
using System.Text;
using System.Xml;
using System.IO;
using System.Collections.Generic;

namespace LightStudio.Tactic.Messaging.Lobby
{
  internal interface IClientInnerService
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
    public static bool Interpret(IMessage message, IClientInnerService service)
    {
      switch (message.Header)
      {
        case MessageHeaders.ON_LOGIN_FAILED:
          service.OnLoginFailed();
          break;
        case MessageHeaders.ON_LOGIN_SUCCEEDED:
          message.Resolve(reader =>
            service.OnLoginSucceeded(reader.ReadUserId(), reader.ReadArray((Func<User>)reader.ReadUser)));
          break;
        case MessageHeaders.ON_USER_LOGINED:
          message.Resolve(reader =>
            service.OnUserLogined(reader.ReadUser()));
          break;
        case MessageHeaders.ON_USER_EXITED:
          message.Resolve(reader =>
            service.OnUserExited(reader.ReadUserId()));
          break;
        case MessageHeaders.ON_MESSAGE_RECEIVED:
          message.Resolve(reader =>
            service.OnMessageReceived(reader.ReadUserId(), reader.ReadString()));
          break;
        case MessageHeaders.ON_BROADCAST_RECEIVED:
          message.Resolve(reader =>
            service.OnBroadcastReceived(reader.ReadUserId(), reader.ReadString()));
          break;
        case MessageHeaders.ON_USER_STATE_CHANGED:
          message.Resolve(reader =>
            service.OnUserStateChanged(reader.ReadUserId(), reader.ReadUserState()));
          break;
        case MessageHeaders.ON_USER_INFO_CHANGED:
          message.Resolve(reader =>
            service.OnUserInfoChanged(reader.ReadUserId(), reader.ReadUserState(), reader.ReadString()));
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
    public static IMessage Login(string name, int avatar)
    {
      return BuildMessage(MessageHeaders.LOGIN, writer =>
        {
          writer.Write(name);
          writer.Write(avatar);
        });
    }
    public static IMessage SendMessage(IEnumerable<int> receivers, string content)
    {
      return BuildMessage(MessageHeaders.SEND_MESSAGE, writer =>
        {
          writer.WriteArray(receivers, writer.WriteUserId);
          writer.Write(content);
        });
    }
    public static IMessage BroadcastMessage(string content)
    {
      return BuildMessage(MessageHeaders.BROADCAST, writer => writer.Write(content));
    }
    public static IMessage Logout()
    {
      return BuildMessage(MessageHeaders.LOGOUT);
    }
    public static IMessage ChangeState(UserState state)
    {
      return BuildMessage(MessageHeaders.CHANGE_STATE, writer => writer.WriteUserState(state));
    }
    public static IMessage ChangeInfo(UserState state, string sign)
    {
      return BuildMessage(MessageHeaders.CHANGE_INFO, writer =>
        {
          writer.WriteUserState(state);
          writer.Write(sign);
        });
    }
  }//public static class
}
