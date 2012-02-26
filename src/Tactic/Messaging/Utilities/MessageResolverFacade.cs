using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace LightStudio.Tactic.Messaging
{
  public static class MessageResolverFacade
  {
    static MessageResolverFacade()
    {
      Resolver = new DataContractResolver();
    }

    internal static IMessageResolver Resolver
    { get; set; }

    public static IMessagable GetMessageObject(this IMessage message)
    {
      Contract.Requires(message != null);

      var resolver = Resolver;
      if (resolver != null)
      {
        return resolver.GetMessageObject(message);
      }
      return null;
    }

    /// <returns>null, if any exception occurs</returns>
    public static IMessagable GetMessageObjectOrNull(this IMessage message)
    {
      try
      {
        return message.GetMessageObject();
      }
      catch (Exception)
      {
        return null;
      }
    }

    public static IMessage ToMessage(this IMessagable obj)
    {
      Contract.Requires(obj != null);

      var resolver = Resolver;
      if (resolver != null)
      {
        return resolver.ToMessage(obj);
      }
      return null;
    }

    /// <returns>null, if any exception occurs</returns>
    public static IMessage ToMessageOrNull(this IMessagable obj)
    {
      try
      {
        return obj.ToMessage();
      }
      catch (Exception e)
      {
        return null;
      }
    }
  }
}
