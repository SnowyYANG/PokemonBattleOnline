using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.Tactic.Logging
{
    public interface ILogger
    {
        void Log(string message, Category category, Priority priority);
    }
}
