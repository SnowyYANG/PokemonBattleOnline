using System;
using System.Text;
using System.Xml;
using System.IO;
using System.Collections.Generic;

namespace LightStudio.Tactic.Messaging.Lobby
{
  public static class MessageHeaders
  {
    public const string LOGIN = "L";
    public const string COMPLETELOGIN = "A";
    public const string BROADCAST = "B";
    public const string LOGOUT = "O";
    public const string SEND_MESSAGE = "S";
    public const string CHANGE_STATE = "E";
    public const string CHANGE_INFO = "I";
    public const string ON_LOGIN_FAILED = "f";
    public const string ON_LOGIN_SUCCEEDED = "s";
    public const string ON_USER_LOGINED = "l";
    public const string ON_USER_EXITED = "x";
    public const string ON_MESSAGE_RECEIVED = "r";
    public const string ON_BROADCAST_RECEIVED = "b";
    public const string ON_USER_STATE_CHANGED = "e";
    public const string ON_USER_INFO_CHANGED = "i";
  }
}