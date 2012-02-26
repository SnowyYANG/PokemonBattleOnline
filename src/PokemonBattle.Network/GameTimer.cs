using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using LightStudio.Tactic.Messaging;

namespace LightStudio.PokemonBattle.Messaging
{
    internal class GameTimer : DisposableObject
    {
        private Timer secondTimer;
        private TickEventArgs lastTickEvent;

        public int TimeLimit
        { get; private set; }

        public int ElapsedTime
        { get; private set; }

        public GameTimer(int timeLimit)
        {
            this.TimeLimit = timeLimit;
            this.secondTimer = new Timer(OnTick);
            this.lastTickEvent = new TickEventArgs(0);
        }

        public void Start()
        {
            lastTickEvent.Canceled = true;
            secondTimer.Change(1000, 1000);
        }

        public void Reset()
        {
            ElapsedTime = 0;
        }

        public void Stop()
        {
            lastTickEvent.Canceled = true;
            secondTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        public event EventHandler<TickEventArgs> Tick = delegate { };
        private void OnTick()
        {
            lastTickEvent.Canceled = true;
            lastTickEvent = new TickEventArgs(TimeLimit - ElapsedTime);
            Tick(this, lastTickEvent);
        }

        private void OnTick(object state)
        {
            ElapsedTime++;
            if (ElapsedTime % (TimeLimit / 4) == 0 || ElapsedTime == TimeLimit)
            {
                OnTick();
            }
        }

        protected override void DisposeManagedResources()
        {
            lastTickEvent.Canceled = true;
            secondTimer.Dispose();
            base.DisposeManagedResources();
        }
    }
}
