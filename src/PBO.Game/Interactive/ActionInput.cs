using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Game
{
    [DataContract(Name = "xai", Namespace = PBOMarks.JSON)]
    public sealed class XActionInput
    {
        public static XActionInput UseMove(SimMove move, bool mega, int targetTeam, int targetX)
        {
            return new XActionInput(move.Type.Id, mega, targetTeam + 1, targetX + 1, 0);
        }
        public static XActionInput UseMove(SimMove move, bool mega)
        {
            return new XActionInput(move.Type.Id, mega, 0, 0, 0);
        }
        public static XActionInput SendOut(SimPokemon sendout)
        {
            return new XActionInput(0, false, 0, 0, sendout.IndexInOwner);
        }
        public static XActionInput Struggle()
        {
            return new XActionInput(0, false, 0, 0, 0);
        }

        [DataMember(Name = "a", EmitDefaultValue = false)]
        public readonly int Move;

        [DataMember(Name = "e", EmitDefaultValue = false)]
        public readonly bool Mega;

        [DataMember(Name = "c", EmitDefaultValue = false)]
        public readonly byte TargetTeam;

        [DataMember(Name = "d", EmitDefaultValue = false)]
        public readonly byte TargetX;

        [DataMember(Name = "b", EmitDefaultValue = false)]
        public readonly byte SendOutIndex;

        private XActionInput(int move, bool mega, int targetTeam, int targetX, int sendout)
        {
            Move = move;
            Mega = mega;
            TargetTeam = (byte)targetTeam;
            TargetX = (byte)targetX;
            SendOutIndex = (byte)sendout;
        }
    }
    [DataContract(Name = "ai", Namespace = PBOMarks.JSON)]
    public class ActionInput
    {
        public ActionInput(int maxX)
        {
        }
        protected ActionInput(ActionInput action)
        {
            I0 = action.I0;
            I1 = action.I1;
            I2 = action.I2;
            GiveUp = action.GiveUp;
        }
        protected ActionInput()
        {
        }

        [DataMember(EmitDefaultValue = false)]
        public bool GiveUp
        { get; internal set; }

        [DataMember(EmitDefaultValue = false)]
        private XActionInput I0;

        [DataMember(EmitDefaultValue = false)]
        private XActionInput I1;

        [DataMember(EmitDefaultValue = false)]
        private XActionInput I2;

        private void Set(int x, XActionInput input)
        {
            if (x == 0) I0 = input;
            else if (x == 1) I1 = input;
            else I2 = input;
        }
        public XActionInput Get(int x)
        {
            return x == 0 ? I0 : x == 1 ? I1 : I2;
        }

        public void UseMove(int x, SimMove move, bool mega, int targetTeam, int targetX)
        {
            Set(x, XActionInput.UseMove(move, mega, targetTeam, targetX));
        }
        public void UseMove(int x, SimMove move, bool mega)
        {
            Set(x, XActionInput.UseMove(move, mega));
        }
        public void Switch(int x, SimPokemon sendout)
        {
            Set(x, XActionInput.SendOut(sendout));
        }
        public void SendOut(int x, SimPokemon sendout)
        {
            Set(x, XActionInput.SendOut(sendout));
        }
        public void Struggle(int x)
        {
            Set(x, XActionInput.Struggle());
        }
    }
}
