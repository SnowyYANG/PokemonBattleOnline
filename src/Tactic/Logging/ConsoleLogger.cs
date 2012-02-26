using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.Tactic.Logging
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message, Category category, Priority priority)
        {
            Console.WriteLine("{0}({1}) : {2}", category, priority, message);
        }
    }
}
