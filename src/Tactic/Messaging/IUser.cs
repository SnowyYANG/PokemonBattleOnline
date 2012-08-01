using System;
using System.Text;
using System.Xml;
using System.IO;
using System.Collections.Generic;

namespace LightStudio.Tactic.Messaging
{
  public enum UserState : byte
  {
    /// <summary>
    /// login not complete, avatar isn't uploaded
    /// </summary>
    Invalid = 0,
    Normal,
    Aggressive,
    Battling,
    Watching,
    Afk
  }
}