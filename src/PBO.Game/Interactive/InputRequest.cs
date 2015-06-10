﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using PokemonBattleOnline.Game;

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
        public void Target(PokemonOutward target = null)
        {
            throw new NotImplementedException();
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

        [DataMember(EmitDefaultValue = false)]
        public int[] Xs;

        [DataMember(Name = "c")]
        public int Time;

        public InputRequest()
        {
        }
        protected InputRequest(InputRequest ir)
        {
            Pms = ir.Pms;
            Xs = ir.Xs;
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

        private void NextPm()
        {
            while (CurrentX < Pms.Length)
                if (Pms[CurrentX++] != null) break;
            if (CurrentX == Pms.Length) InputFinished(input);
        }
        public void Init(SimGame game)
        {
            this.game = game;
            CurrentX = -1;
            if (Pms != null)
                for (int i = 0; i < Pms.Length; ++i)
                    if (Pms[i] != null)
                    {
                        Pms[i].Init(game, game.OnboardPokemons[i]);
                        if (CurrentX == -1) CurrentX = i;
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
        public bool Move(SimMove move, bool mega)
        {
            if (Pms[CurrentX].Move(move))
            {
                if (!game.Settings.Mode.NeedTarget())
                {
                    input.UseMove(CurrentX, move, mega);
                    NextPm();
                }
                return true;
            }
            error = Pms[CurrentX].GetErrorMessage();
            return false;
        }
        public void Target(PokemonOutward target = null)
        {
            Pms[CurrentX].Target(target);
        }
        public bool Pokemon(SimPokemon pokemon, int x)
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
            input.SendOut(x, pokemon);
            //多打中有多只精灵倒下，要把哪只精灵收回来
            //单打和4P没有这种需要
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