using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Game
{
    public class SelectMoveFail
    {
        public readonly string Key;
        public readonly int Move; //区分Block和Only

        public SelectMoveFail(string key, int move)
        {
            Key = key;
            Move = move;
        }
    }
    [DataContract(Name = "pir", Namespace = PBOMarks.JSON)]
    public sealed class PmInputRequest
    {
        [DataMember(EmitDefaultValue = false)]
        public int OnlyMove;

        [DataMember(EmitDefaultValue = false)]
        public string Only; //choice/encore

        [DataMember(EmitDefaultValue = false)]
        public string[] Block; //封印挑拨寻衅回复封印残废

        [DataMember(EmitDefaultValue = false)]
        public bool CantWithdraw;

        [DataMember(EmitDefaultValue = false)]
        public bool CanMega;

        /// <summary>
        /// 持有诅咒技能且可选目标
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public bool Curse;

        #region Client
        private SimGame Game;
        private SimOnboardPokemon Pm;
        private string error;
        public void Init(SimGame game, SimOnboardPokemon pm)
        {
            Game = game;
            Pm = pm;
        }
        public string GetErrorMessage()
        {
            string r = error;
            error = null;
            return r;
        }
        private void SetErrorMessage(string key, string arg1, string arg2)
        {
            var text = GameString.Current.BattleLog("subtitle_" + key);
            error = string.Format(text, Pm.Pokemon.Name, arg1, arg2);
        }
        public bool Fight()
        {
            return OnlyMove == Ms.STRUGGLE;
        }
        /// <summary>
        /// 不判断PP数及技能是否存在
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        public bool Move(SimMove move)
        {
            if (OnlyMove != 0 && OnlyMove != move.Type.Id) SetErrorMessage(Only, GameString.Current.Move(RomData.GetMove(OnlyMove).Id), GameString.Current.Item(Pm.Pokemon.Item));
            else
              if (Block != null)
                for (int i = 0; i < Pm.Moves.Length; ++i)
                    if (move == Pm.Moves[i])
                    {
                        if (Block[i] != null) SetErrorMessage(Block[i], GameString.Current.Move(move.Type.Id), null);
                        break;
                    }
            return error == null;
        }
        /// <summary>
        /// 判断pokemon是否在场和Hp
        /// </summary>
        /// <param name="pokemon"></param>
        /// <returns></returns>
        public bool Pokemon(SimPokemon pokemon)
        {
            if (pokemon.Hp.Value == 0)
            {
                error = string.Format(GameString.Current.BattleLog("PokemonFainted"), pokemon.Name);
                return false;
            }
            if (pokemon.IndexInOwner < Game.Settings.Mode.OnboardPokemonsPerPlayer())
            {
                error = string.Format(GameString.Current.BattleLog("PokemonFighting"), pokemon.Name);
                return false;
            }
            if (CantWithdraw)
            {
                error = string.Format(GameString.Current.BattleLog("PokemonCannotWithdraw"), Pm.Pokemon.Name);
                return false;
            }
            return true;
        }
        #endregion
    }

    [DataContract(Name = "ir", Namespace = PBOMarks.JSON)]
    public class InputRequest
    {
        [DataMember(EmitDefaultValue = false)]
        public PmInputRequest[] Pms;

        /// <summary>
        /// 逃生按钮 追击死亡 （回合末精灵登场Pms和X均为null）
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        private int i;
        public int? Index
        {
            get { return i == 0 ? null : (int?)(i - 1); }
            set { i = value == null ? 0 : i + 1; }
        }

        [DataMember(Name = "a")]
        public int Time;

        public InputRequest()
        {
        }
        protected InputRequest(InputRequest ir)
        {
            Pms = ir.Pms;
            Index = ir.Index;
            Time = ir.Time;
        }

        #region PlayerClient
        public event Action<ActionInput> InputFinished;
        private ActionInput input;
        private SimGame game;
        private string error;

        public bool IsSendOut
        { get { return Pms == null; } }
        public int CurrentX
        { get; private set; }
        public bool CanMega
        { get { return Pms != null && Pms[CurrentX].CanMega; } }
        public bool NeedTarget
        { get { return game.Settings.Mode.NeedTarget(); } }
        public MoveRange MoveRange
        { get; private set; }

        private void NextPm()
        {
            while (++CurrentX < Pms.Length)
                if (Pms[CurrentX] != null) break;
            if (CurrentX == Pms.Length) InputFinished(input);
        }
        public void Init(SimGame game)
        {
            this.game = game;
            CurrentX = -1;
            if (Pms != null)
                for (int x = 0; x < Pms.Length; ++x)
                    if (Pms[x] != null)
                    {
                        Pms[x].Init(game, game.OnboardPokemons[x]);
                        if (CurrentX == -1) CurrentX = x;
                    }
            input = new ActionInput(game.Settings.Mode.XBound());
        }
        public string GetErrorMessage()
        {
            return error;
        }
        public bool Fight()
        {
            if (Pms[CurrentX].Fight())
            {
                input.Struggle(CurrentX);
                NextPm();
                return true;
            }
            error = Pms[CurrentX].GetErrorMessage();
            return false;
        }

        private SimMove move;
        private bool mega;
        public bool Move(SimMove move, bool mega)
        {
            if (Pms[CurrentX].Move(move))
            {
                if (NeedTarget)
                {
                    MoveRange = move.Type.Id == Ms.CURSE && Pms[CurrentX].Curse ? MoveRange.SelectedTarget : move.Type.Range;
                    this.move = move;
                    this.mega = mega;
                }
                else
                {
                    input.UseMove(CurrentX, move, mega);
                    NextPm();
                }
                return true;
            }
            error = Pms[CurrentX].GetErrorMessage();
            return false;
        }
        public void Target(int team, int x)
        {
            if (move != null)
            {
                input.UseMove(CurrentX, move, mega, team, x);
                move = null;
                mega = false;
                NextPm();
            }
        }
        public void Target()
        {
            if (move != null)
            {
                input.UseMove(CurrentX, move, mega);
                move = null;
                mega = false;
                NextPm();
            }
        }
        public bool Pokemon(SimPokemon pokemon, int index)
        {
            if (pokemon.Hp.Value == 0)
            {
                error = string.Format(GameString.Current.BattleLog("PokemonFainted"), pokemon.Name);
                return false;
            }
            if (pokemon.IndexInOwner < game.Settings.Mode.OnboardPokemonsPerPlayer())
            {
                error = string.Format(GameString.Current.BattleLog("PokemonFighting"), pokemon.Name);
                return false;
            }
            input.SendOut(index, pokemon);
            //多打中有多只精灵倒下，要把哪只精灵收回来
            //单打和合作没有这种需要
            InputFinished(input);
            return true;
        }
        public bool Pokemon(SimPokemon pokemon)
        {
            if (Pms[CurrentX].Pokemon(pokemon))
            {
                input.Switch(CurrentX, pokemon);
                NextPm();
                return true;
            }
            error = Pms[CurrentX].GetErrorMessage();
            return false;
        }
        public void GiveUp()
        {
            input.GiveUp = true;
            InputFinished(input);
        }
        //public void Undo();
        //public void TurnLeft();
        //public void TurnRight();
        //public void MoveToCenter();
        #endregion
    }
}
