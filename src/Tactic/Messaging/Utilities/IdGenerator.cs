using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace LightStudio.Tactic.Messaging
{
    internal class ThreadSafeIdGenerator
    {
        private int currentId = 0;

        public ThreadSafeIdGenerator(int idBase)
        {
            this.currentId = idBase;
        }

        public int NextId()
        {
            var id = Interlocked.Increment(ref currentId);
            return id;
        }
    }
}
