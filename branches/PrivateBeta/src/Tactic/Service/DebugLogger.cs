using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace LightStudio.Tactic.Logging
{
    public class DebugLogger : ILogger
    {
        public void Log(string message, Category category, Priority priority)
        {
            Debug.WriteLine(message, category.ToString());
        }
    }
}
