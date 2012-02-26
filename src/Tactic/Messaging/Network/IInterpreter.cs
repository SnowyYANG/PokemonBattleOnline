using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LightStudio.Tactic.Messaging
{
    internal interface IInterpreter
    {
        WriteMessageResult Write(IMessage message, BinaryWriter writer);
        ReadMessageResult Read(BinaryReader reader);
        void WriteAsync(IMessage message, BinaryWriter writer, Action<WriteMessageResult> callback);
        void ReadAsync(BinaryReader reader, Action<ReadMessageResult> callback);
    }
}
