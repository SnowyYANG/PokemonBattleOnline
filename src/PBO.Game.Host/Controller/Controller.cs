﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Game.Host
{
    internal class Controller : IDisposable
    {
        public event Action<ReportFragment, InputRequest[,]> GameUpdated;
        public event Action GameEnd;

        public readonly ReportBuilder ReportBuilder;
        public readonly IGameSettings GameSettings;
        public readonly Team[] Teams;
        public readonly Board Board;
        public readonly GoTimer Timer;
        private readonly SwitchController SwitchController;
        private readonly InputController InputController;
        private readonly TurnController TurnController;

        private Random random;
#if TEST
    private Random randomSeeds;
#endif

        internal Controller(IGameSettings settings, IPokemonData[,][] pms)
        {
            GameSettings = settings;
            Board = new Board(GameSettings);

            Teams = new Team[2];
            for (int t = 0; t < Teams.Length; ++t)
            {
                var players = new Player[settings.Mode.PlayersPerTeam()];
                for (int p = 0; p < settings.Mode.PlayersPerTeam(); ++p) players[p] = new Player(this, t, p, pms[t, p]);
                Teams[t] = new Team(t, players, settings);
            }

            ReportBuilder = new ReportBuilder(this);
            SwitchController = new SwitchController(this);
            InputController = new InputController(this);
            TurnController = new TurnController(this);
#if TEST
            randomSeeds = new Random(1);
            random = new Random(randomSeeds.Next());
#else
            random = new Random();
#endif
            Timer = new GoTimer(GameSettings.Mode.PlayersPerTeam() == 1 ? new Player[] { Teams[0].GetPlayer(0), Teams[1].GetPlayer(0) } : new Player[] { Teams[0].GetPlayer(0), Teams[0].GetPlayer(1), Teams[1].GetPlayer(0), Teams[1].GetPlayer(1) });
            Timer.Start();
        }

        /// <summary>
        /// sorted by action speed
        /// </summary>
        public List<PokemonProxy> ActingPokemons
        { get { return TurnController.ActingPokemons; } }
        /// <summary>
        /// sorted by speed
        /// </summary>
        public IEnumerable<Tile> Tiles
        { get { return TurnController.Tiles; } }
        public IEnumerable<PokemonProxy> OnboardPokemons
        { get { return TurnController.Pokemons; } }
        public int TurnNumber
        { get { return ReportBuilder.TurnNumber; } }

        #region Access
        internal Player GetPlayer(Tile tile)
        {
            return Teams[tile.Team].GetPlayer(GameSettings.Mode.GetPlayerIndex(tile.X));
        }
        public int GetRandomInt(int min, int max)
        {
            return min == max ? min : random.Next(min, max + 1);
        }
        public bool RandomHappen(int percentage)
        {
            return random.Next(100) < percentage;
        }
        public bool OneNth(int n)
        {
            return random.Next(n) == 0;
        }
        public Weather Weather
        { get { return ATs.IgnoreWeather(this) ? Weather.Normal : Board.Weather; } }
        /// <summary>
        /// sorted by speed
        /// </summary>
        public IEnumerable<PokemonProxy> GetOnboardPokemons(int teamId)
        {
            return OnboardPokemons.Where((p) => p.Pokemon.TeamId == teamId);
        }
        #endregion

        #region Turn Loop
        private bool _isGameEnd;
        private bool[] lose = new bool[2];
        public bool CanContinue
        {
            get
            {
                if (_isGameEnd || InputController.NeedInput) return false;
                foreach (var t in Teams)
                {
                    bool allfaint = true;
                    foreach (var p in t.Players) allfaint &= p.PmsAlive == 0;
                    if (allfaint) lose[t.Id] = true;
                }
                if (lose[0] || lose[1])
                {
                    SetGameEnd();
                    return false;
                }
                return true;
            }
        }
        internal void StartGameLoop()
        {
            ReportBuilder.NewFragment();
            TurnController.StartGameLoop();
        }
        internal void TryContinueGameLoop()
        {
            if (!InputController.NeedInput)
            {
                foreach(var t in Teams)
                    foreach(var p in t.Players)
                        if (p.GiveUp)
                        {
                            lose[t.Id] = true;
                            break;
                        }
                if (lose[0] || lose[1]) SetGameEnd();
                else
                {
                    ReportBuilder.TimeTick();
                    if (SingleSendOut != null)
                    {
                        SendOut(SingleSendOut);
                        ReportBuilder.AddHorizontalLine();
                        SingleSendOut = null;
                    }
                    TurnController.StartGameLoop();
                }
            }
        }
        private void SetGameEnd()
        {
            _isGameEnd = true;
            GameUpdated(ReportBuilder.GameEnd(lose[0], lose[1]), null);
            Dispose();
            GameEnd();
        }
        #endregion

        #region Input
        internal bool CheckInputSucceed(int teamId, int teamIndex)
        {
            return InputController.CheckInputSucceed(Teams[teamId].GetPlayer(teamIndex));
        }
        private Tile SingleSendOut;
        public void PauseForSendOutInput(Tile tile) //逃生按钮、追击死亡
        {
            if (InputController.PauseForSendOutInput(tile))
            {
                SingleSendOut = tile;
                PauseForInput();
            }
        }
        internal void PauseForTurnInput()
        {
            if (InputController.PauseForTurnInput()) PauseForInput();
        }
        internal void PauseForEndTurnInput()
        {
            if (InputController.PauseForEndTurnInput()) PauseForInput();
        }
        private void PauseForInput()
        {
            if (InputController.NeedInput)
            {
#if TEST
        random = new Random(randomSeeds.Next());
#else
                random = new Random();
#endif
                ReportBuilder.NewFragment();
                var r = InputController.InputRequirements;
                GameUpdated(ReportBuilder.GetFragment(), r);
                var players = new List<Player>();
                if (r[0, 0] != null) players.Add(Teams[0].GetPlayer(0));
                if (r[0, 1] != null) players.Add(Teams[0].GetPlayer(1));
                if (r[1, 0] != null) players.Add(Teams[1].GetPlayer(0));
                if (r[1, 1] != null) players.Add(Teams[1].GetPlayer(1));
                Timer.Resume(players);
            }
        }
        internal bool InputSendOut(Tile tile, int sendoutIndex)
        {
            return InputController.SendOut(tile, sendoutIndex);
        }
        internal bool InputSelectMove(MoveProxy move, Tile target, bool mega)
        {
            return InputController.SelectMove(move, target, mega);
        }
        internal bool InputStruggle(PokemonProxy pm)
        {
            return InputController.Struggle(pm);
        }
        public void InputGiveUp(int teamId, int teamIndex)
        {
            InputController.GiveUp(teamId, teamIndex);
        }
        #endregion

        #region switch or send out
        public bool CanSendOut(Tile tile)
        {
            return SwitchController.CanSendOut(tile);
        }
        public bool CanSendOut(PokemonProxy pokemon)
        {
            return SwitchController.CanSendOut(pokemon);
        }
        public bool CanWithdraw(PokemonProxy pm)
        {
            return SwitchController.CanWithdraw(pm);
        }
        /// <summary>
        /// null log to show nothing
        /// </summary>
        /// <param name="pm"></param>
        /// <param name="log"></param>
        /// <param name="arg1"></param>
        /// <param name="canPursuit"></param>
        /// <returns></returns>
        public bool Withdraw(PokemonProxy pm, string log, int arg1 = 0, bool canPursuit = true)
        {
            if (SwitchController.Withdraw(pm, log, arg1, canPursuit))
            {
                Board.RefreshPokemons();
                return true;
            }
            return false;
        }
        internal void GameStartSendOut(IEnumerable<Tile> tiles)
        {
            SwitchController.GameStartSendOut(tiles);
        }
        public bool SendOut(Tile position, bool debut = true, string log = LogKeys.SendOut1)
        {
            if (SwitchController.SendOut(position, debut, log))
            {
                Board.RefreshPokemons();
                return true;
            }
            return false;
        }
        #endregion

        public void Dispose()
        {
            Timer.Dispose();
        }
    }
}