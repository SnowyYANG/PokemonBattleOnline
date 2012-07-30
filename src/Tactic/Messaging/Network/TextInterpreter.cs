using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;

namespace LightStudio.Tactic.Messaging.Primitive
{
  internal class TextInterpreter : IInterpreter
  {

    public TextInterpreter()
    { }

    public WriteMessageResult Write(IMessage message, BinaryWriter writer)
    {
      try
      {
        WriteMessage(message, writer);
        return new WriteMessageResult(true);
      }
      catch (Exception ex)
      {
        return new WriteMessageResult(ex);
      }
    }

    public ReadMessageResult Read(BinaryReader reader)
    {
      try
      {
        return new ReadMessageResult(ReadMessage(reader));
      }
      catch (Exception ex)
      {
        return new ReadMessageResult(ex);
      }
    }

    public void WriteAsync(IMessage message, BinaryWriter writer,
        Action<WriteMessageResult> callback)
    {
      Task.Factory.StartNew(
          () =>
          {
            bool success = false;
            try
            {
              WriteMessage(message, writer);
              success = true;
            }
            catch (Exception ex)
            {
              callback(new WriteMessageResult(ex));
              return;
            }
            callback(new WriteMessageResult(success));
          });
    }

    public void ReadAsync(BinaryReader reader, Action<ReadMessageResult> callback)
    {
      Task.Factory.StartNew(
          () =>
          {
            IMessage message = null;
            try
            {
              message = ReadMessage(reader);
            }
            catch (Exception ex)
            {
              callback(new ReadMessageResult(ex));
              return;
            }
            callback(new ReadMessageResult(message));
          });
    }

    private void WriteMessage(IMessage message, BinaryWriter writer)
    {
      writer.Write(message.Header);
      writer.Write(message.Content);
      writer.Flush();
    }

    private IMessage ReadMessage(BinaryReader reader)
    {
      return new TextMessage(reader.ReadString(), reader.ReadString());
    }
  }
}
