using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game.Host.Triggers;

namespace PokemonBattleOnline.Game.Host
{
    internal class TurnController : ControllerComponent
    {
        private readonly Comparer Comparer;
        private byte current;

        public TurnController(Controller controller)
          : base(controller)
        {
            Comparer = new Comparer(controller.Board);
            _tiles = Board.Tiles.ToArray(); //this is a copy
            ActingPokemons = new List<PokemonProxy>();
        }

        public List<PokemonProxy> ActingPokemons
        { get; private set; }

        private readonly Tile[] _tiles;
        public IEnumerable<Tile> Tiles
        { get { return _tiles; } }

        public IEnumerable<PokemonProxy> Pokemons
        { get { return Tiles.Where((t) => t.Pokemon != null).Select((t) => t.Pokemon); } }

        private void SortActingPokemons()
        {
            for (int i = 0; i < ActingPokemons.Count - 1; i++)
            {
                int j;
                j = Controller.GetRandomInt(i, ActingPokemons.Count - 1);
                PokemonProxy temp = ActingPokemons[i];
                ActingPokemons[i] = ActingPokemons[j];
                ActingPokemons[j] = temp;
            }
            foreach (var p in Board.Pokemons) p.CalculatePriority();
            ActingPokemons.Sort(Comparer);
        }
        private void SortTiles()
        {
            for (int i = 0; i < _tiles.Length - 1; i++)
            {
                int j;
                j = Controller.GetRandomInt(i, _tiles.Length - 1);
                Tile temp = _tiles[i];
                _tiles[i] = _tiles[j];
                _tiles[j] = temp;
            }
            Array.Sort(_tiles, Comparer);
        }

        public void StartGameLoop()
        {
            if (!Controller.CanContinue) return;
            LOOP:
            switch (current)
            {
                case 0:
                    BeginTurn();
                    break;
                case 1:
                    Prepare();
                    break;
                case 2:
                    Switch();
                    CheckFocusPunch();//暂定，实际上不在这
                    break;
                case 3:
                    Mega();
                    break;
                case 4:
                    Move();
                    break;
                case 5:
                    EndTurnEffects();
                    break;
                case 6:
                    EndTurnCheckForInput();
                    break;
                case 7:
                    EndTurnSendOut();
                    break;
                case 8:
                    NextTurn();
                    break;
                default:
                    current = 0;
                    goto case 0;
            }
            if (Controller.CanContinue)
            {
                current++;
                goto LOOP;
            }
        }

        private void BeginTurn()
        {
            bool needInput = false;
            foreach (PokemonProxy p in Pokemons)
                needInput |= p.CheckNeedInput();
            if (needInput) Controller.PauseForTurnInput();
        }
        private void Prepare()
        {
            Controller.Timer.NewTurn();
            ReportBuilder.NewTurn();
            SortTiles();
            SortActingPokemons();
            foreach (PokemonProxy p in ActingPokemons) p.AttachBehaviors();
        }
        private void Switch()
        {
            LOOP:
            PokemonProxy p = ActingPokemons.FirstOrDefault((pm) => pm.Action == PokemonAction.WillSwitch);
            if (p != null)
            {
                p.Switch();
                ReportBuilder.AddHorizontalLine();
                goto LOOP;
            }
        }
        private void CheckFocusPunch()
        {
            foreach (PokemonProxy p in ActingPokemons)
                if (p.Action == PokemonAction.MoveAttached && p.SelectedMove.MoveE.Id == Ms.FOCUS_PUNCH) p.ShowLogPm("EnFocusPunch");
        }
        private void Mega()
        {
            var m = false;
            foreach (var p in ActingPokemons)
                if (p.Action == PokemonAction.MoveAttached && p.SelectMega)
                {
                    p.ShowLogPm("MegaPre", p.Pokemon.Item);
                    p.Pokemon.Mega = true;
                    p.ChangeForm(ITs.MegaForm(p.Pokemon.Item), true, "Mega");
                    m = true;
                }
            if (m) ReportBuilder.AddHorizontalLine();
        }
        private void Move()
        {
            LOOP:
            PokemonProxy p = ActingPokemons.FirstOrDefault((pm) => pm.CanMove);
            if (p != null)
            {
                p.Move();
                if (Controller.CanContinue)
                {
                    ReportBuilder.AddHorizontalLine();
                    goto LOOP;
                }
            }
        }
        private void EndTurnEffects()
        {
            if (Controller.TurnNumber != 0)
            {
                SortTiles();
                EndTurn.Execute(Controller);
                ReportBuilder.AddHorizontalLine();
            }
        }
        private void EndTurnCheckForInput()
        {
            if (Controller.TurnNumber != 0)
            {
                current++;
                foreach (Tile t in Tiles)
                    if (t.Pokemon == null && Controller.CanSendOut(t))
                    {
                        Controller.PauseForEndTurnInput();
                        return;
                    }
            }
        }
        private void EndTurnSendOut()
        {
            if (Controller.TurnNumber == 0)
            {
                Controller.GameStartSendOut(Board[0].Tiles);
                Controller.GameStartSendOut(Board[1].Tiles);
                Board.RefreshPokemons();
            }
            else
                foreach (Tile t in Tiles)
                    if (t.WillSendOutPokemonIndex != Tile.NOPM_INDEX) Controller.SendOut(t, false);
            SortTiles();
            ATs.AttachUnnerve(Controller);
            var debut = new List<PokemonProxy>();
            foreach (Tile t in Tiles)
                if (t.Pokemon != null && t.Pokemon.Action == PokemonAction.Debuting)
                {
                    t.Pokemon.Debut();
                    if (t.Pokemon != null) debut.Add(t.Pokemon);
                }
            foreach (var p in debut) ATs.AttachWeatherObserver(p);
            ATs.WeatherChanged(Controller);
            if (ReportBuilder.TurnNumber != 0) current -= 2;
        }
        private void NextTurn()
        {
            ATs.SlowStart(Controller);
            Board.ClearTurnCondition();
            foreach (var f in Board.Fields) f.ClearTurnCondition();
            foreach (var t in Tiles)
            {
                if (t.Pokemon != null)
                {
                    t.Pokemon.OnboardPokemon.ClearTurnCondition();
                    if (t.Pokemon.AtkContext != null) t.Pokemon.AtkContext.ClearTurnCondition();
                }
                t.ClearTurnCondition();
            }
            ReportBuilder.Add(new GameEvents.EndTurn());
        }
    }
}
