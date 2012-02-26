using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using LightStudio.Tactic.Logging;

namespace LightStudio.Tactic.Messaging
{
  internal class DeflateResolver : IMessageResolver
  {
    public IMessageResolver BaseResolver
    { get; private set; }

    public int CompressionThreshold
    { get; private set; }

    public DeflateResolver(IMessageResolver resolver)
      : this(resolver, DefaultCompressionThreshold)
    { }

    public DeflateResolver(IMessageResolver resolver, int compressionThreshold)
    {
      this.BaseResolver = resolver;
      this.CompressionThreshold = compressionThreshold;
    }

    public IMessagable GetMessageObject(IMessage message)
    {
      var textMessage = new TextMessage(message.Header, message.Content);
      if (textMessage.Content.StartsWith("c"))//这...
      {
        textMessage.Content = DecompressMessage(textMessage.Content.Substring(1));
      }
      else if (textMessage.Content.StartsWith("u"))
      {
        textMessage.Content = textMessage.Content.Substring(1);
      }
      else
      {
        LoggerFacade.Log("DeflateResolver.GetMessageObject: missing prefix",
            Category.Exception);
      }
      return BaseResolver.GetMessageObject(textMessage);
    }

    public IMessage ToMessage(IMessagable obj)
    {
      IMessage message = BaseResolver.ToMessage(obj);
      var textMessage = new TextMessage(message.Header, message.Content);
      if (textMessage.Content.Length > DefaultCompressionThreshold)
      {
        textMessage.Content = "c" + CompressMessage(textMessage.Content);
      }
      else
      {
        textMessage.Content = "u" + textMessage.Content;
      }
      return textMessage;
    }

    private static string CompressMessage(string content)
    {
      using (var stream = new MemoryStream())
      {
        using (var writer =
            new BinaryWriter(new DeflateStream(stream, CompressionMode.Compress)))
        {
          writer.Write(content);
        }
        byte[] buffer = stream.ToArray();
        return Convert.ToBase64String(buffer);
      }
    }

    private static string DecompressMessage(string content)
    {
      using (var stream = new DeflateStream(
          new MemoryStream(Convert.FromBase64String(content)), CompressionMode.Decompress))
      {
        using (var reader = new BinaryReader(stream))
        {
          return reader.ReadString();
        }
      }
    }

    public const int DefaultCompressionThreshold = 1024;
  }
}
