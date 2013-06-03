using System;
using System.Collections.Generic;
using System.Text;
using NetworkLib;

namespace PokemonBattle.RoomClient
{
    class BroadcastController
    {
        public event VoidFunctionDelegate OnCounterChanged;
        private void HandleCounterChangedEvent()
        {
            if (OnCounterChanged != null) OnCounterChanged();
        }

        private int _counter = 0;
        private System.Timers.Timer _timer;

        public BroadcastController()
        {
            _timer = new System.Timers.Timer(5000);
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(_timer_Elapsed);
            _timer.Start();
        }

        void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_counter > 0)
            {
                _counter -= 1;
                if (_counter < 10) HandleCounterChangedEvent();
            }
        }

        public int Counter
        {
            get { return _counter; }
        }

        public void Tick()
        {
            _counter += 1;
            HandleCounterChangedEvent();
            if (_counter == 10) _counter = 40;
        }

        public void Stop()
        {
            _timer.Stop();
        }
    }
}
