using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.IO.Compression;
using System.Diagnostics.Contracts;

namespace LightStudio.PokemonBattle.Data
{
    internal class DataSaver
    {
        private ISaveObject userData;
        private DataChangeObserver changeObserver;
        private Timer timer;

        private int _saveInterval;
        public int SaveInterval
        {
            get
            {
                return _saveInterval;
            }
            set
            {
                if (value > 0 && _saveInterval != value)
                {
                    _saveInterval = value;
                }
            }
        }

        public DataSaver(ISaveObject data, int saveInterval)
        {
            Contract.Requires(data != null);

            this.userData = data;
            this.changeObserver = new DataChangeObserver(data);
            this.SaveInterval = saveInterval;
            this.timer = new Timer(obj => Save());
            SetTimer();
        }

        public void SetTimer()
        {
            timer.Change(SaveInterval * 60000, Timeout.Infinite);
        }

        public void Save()
        {
            if (changeObserver.Changed)
            {
                changeObserver.SetUnchanged();
                userData.Save();
            }
            SetTimer();
        }


    }
}
