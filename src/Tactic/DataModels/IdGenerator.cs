using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.Tactic.DataModels
{
    public class IdGenerator
    {
        private int IdBase = 0;
        public int NextId()
        {
          lock (this)
            return ++IdBase;
        }
        public void Reset()
        {
            IdBase = 0;
        }
    }
}
