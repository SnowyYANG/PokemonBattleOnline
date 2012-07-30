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
    public const string ON_LOGIN_FAILED = "F";
    public const string ON_LOGIN_SUCCEEDED = "W";
    public const string ON_USER_LOGINED = "N";
    public const string ON_USER_EXITED = "X";
    public const string ON_MESSAGE_RECEIVED = "M";
    public const string ON_BROADCAST_RECEIVED = "R";
    public const string ON_USER_STATE_CHANGED = "T";
    public const string ON_USER_INFO_CHANGED = "C";
  }
}