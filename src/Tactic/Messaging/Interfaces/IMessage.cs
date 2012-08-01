using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.Tactic.Messaging
{
  public interface IMessage
  {
    string Header { get; }
    string Content { get; }
    void Resolve(Action<System.IO.BinaryReader> resolver);
  }
}
