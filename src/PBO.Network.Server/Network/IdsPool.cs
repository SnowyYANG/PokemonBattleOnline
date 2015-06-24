using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;

namespace PokemonBattleOnline
{
    public class IdsPool
    {
        private readonly ConcurrentBag<int> Pool;
        private int max;

        public IdsPool()
        {
            //ConcurrentBag在一次测试中值为{2, 3, 4, 4, ...}
            Pool = new ConcurrentBag<int>();
            //Interlocked.Increment有一次返回了0，可能是我错觉，总之尝试避免
            max = 0;
        }

        public int GetId()
        {
            int id;
            if (!Pool.TryTake(out id) || id == 0) id = Interlocked.Increment(ref max);
            return id;
        }
        public void Push(int id)
        {
            Pool.Add(id);
        }
    }
}
