using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.Tactic.Messaging
{
  public interface IMessagable
  { //不写任何方法就做个标志呗，写了方法每个都要实现太恶心，扩展方法反而可以当继承用？
  }

  internal interface IMessageResolver
  {
    IMessagable GetMessageObject(IMessage message);
    IMessage ToMessage(IMessagable obj);
  }
}
