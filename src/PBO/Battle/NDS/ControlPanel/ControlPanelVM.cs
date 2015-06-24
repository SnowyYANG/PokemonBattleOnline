using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.Network;

namespace PokemonBattleOnline.PBO.Battle
{
    class ControlPanelVM : ObservableObject
    {
        public const int INACTIVE = -1;
        public const int MAIN = 0;
        public const int FIGHT = 1;
        public const int POKEMONS = 2;
        public const int RUN = 3;
        public const int TARGET = 4;

        public event Action<string> InputFailed;

        readonly PlayerController Controller;
        readonly GameOutward Game;
        readonly DispatcherTimer Timer;
        InputRequest Request;

        public ControlPanelVM(RoomController c)
        {
            Controller = c.PlayerController;
            Game = c.Game;
            Timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
            _time = 180;
            _selectedPanel = INACTIVE;
            _pokemons = new SimPokemon[6];
            Controller.RequireInput += RequireInput;
            c.Game.GameEnd += () => Timer.Stop();
            Timer.Tick += (sender, e) => Time--;
        }

        private int _time;
        public int Time
        {
            get { return _time; }
            private set
            {
                if (_time != value)
                {
                    _time = value;
                    OnPropertyChanged("Time");
                }
            }
        }

        private int _selectedPanel;
        public int SelectedPanel
        {
            get { return _selectedPanel; }
            set
            {
                if (_selectedPanel != value)
                {
                    _selectedPanel = value;
                    OnPropertyChanged("SelectedPanel");
                }
            }
        }

        public Weather Weather
        { get { return Game.Board.Weather; } }

        private bool _mega;
        public bool Mega
        {
            get { return Request != null && Request.CanMega && _mega; }
            set
            {
                if (_mega != value)
                {
                    _mega = value;
                    OnPropertyChanged("Mega");
                }
            }
        }

        public Visibility MegaVisibility
        { get { return Request != null && Request.CanMega ? Visibility.Visible : Visibility.Collapsed; } }

        public SimOnboardPokemon ControllingPokemon
        { get { return Request == null ? null : Controller.Game.OnboardPokemons.ValueOrDefault(Request.CurrentX); } }

        public Visibility UndoVisibility
        { get { return Visibility.Collapsed; } }

        public bool IsReturnEnabled
        { get { return ControllingPokemon != null; } }

        private SimPokemon[] _pokemons;
        public SimPokemon[] Pokemons
        { get { return _pokemons; } }

        public TargetPanel TargetPanel
        { get { return null; } }

        public PokemonOutward[] PokemonsOnBoard
        { get { return null; } }

        private void RequireInput(InputRequest request)
        {
            Request = request;
            request.Init(Controller.Game);
            request.InputFinished += (i) =>
            {
                SelectedPanel = INACTIVE;
                Controller.Input(i);
                Timer.Stop();
            };
            _selectedPanel = request.IsSendOut ? POKEMONS : MAIN;
            _mega = false;
            _time = 180 - request.Time;
            {
                var step = Game.Settings.Mode.PlayersPerTeam();
                var i = 0;
                foreach (var pm in Controller.Game.Team[0].Pokemons) _pokemons[i += step] = pm;
                if (step == 2)
                {
                    i = 1;
                    foreach (var pm in Controller.Game.Team[1].Pokemons) _pokemons[i += step] = pm;
                }
            }
            Timer.Start();
            OnPropertyChanged();
        }

        public void Pokemon_Click(SimPokemon pokemon)
        {
            if (Request.IsSendOut)
            {
                if (!Request.Pokemon(pokemon, 0)) InputFailed(Request.GetErrorMessage());
            }
            else
            {
                if (!Request.Pokemon(pokemon)) InputFailed(Request.GetErrorMessage());
            }
        }
        public void Fight_Click()
        {
            if (!Request.Fight()) SelectedPanel = FIGHT;
        }
        public void Move_Click(SimMove move)
        {
            if (move.PP.Value == 0) return;
            if (!Request.Move(move, Mega)) InputFailed(Request.GetErrorMessage());
        }
        public void GiveUp_Click()
        {
            Request.GiveUp();
        }
    }
}
