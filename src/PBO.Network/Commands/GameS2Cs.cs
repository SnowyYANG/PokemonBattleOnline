using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.Network.Commands
{
    [DataContract(Name = "prepare", Namespace = PBOMarks.JSON)]
    public class SetPrepareS2C : IS2C
    {
        [DataMember(Name = "seat", EmitDefaultValue = false)]
        Seat Seat;
        [DataMember(Name = "value", EmitDefaultValue = false)]
        bool Prepare;

        public SetPrepareS2C(Seat seat, bool prepare)
        {
            Seat = seat;
            Prepare = prepare;
        }

        void IS2C.Execute(PboClient client)
        {
            var room = client.RoomController;
            switch (Seat)
            {
                case Seat.Player00:
                    room.Prepare00 = Prepare;
                    break;
                case Seat.Player01:
                    room.Prepare01 = Prepare;
                    break;
                case Seat.Player10:
                    room.Prepare10 = Prepare;
                    break;
                case Seat.Player11:
                    room.Prepare11 = Prepare;
                    break;
            }
        }
    }

    [DataContract(Name = "partnerinfo", Namespace = PBOMarks.JSON)]
    public class PartnerInfoS2C : IS2C
    {
        [DataMember]
        PokemonData[] a_;

        public PartnerInfoS2C(IPokemonData[] pms)
        {
            a_ = new PokemonData[pms.Length];
            for (int i = 0; i < pms.Length; ++i) a_[i] = (PokemonData)pms[i];
        }

        void IS2C.Execute(PboClient client)
        {
            client.RoomController.Partner = a_;
        }
    }

    [DataContract(Name = "requireinput", Namespace = PBOMarks.JSON)]
    public class RequireInputS2C : InputRequest, IS2C
    {
        public RequireInputS2C(InputRequest ir)
          : base(ir)
        {
        }

        void IS2C.Execute(PboClient client)
        {
            client.RoomController.InputRequest = this;
        }
    }

    [DataContract(Name = "waiting", Namespace = PBOMarks.JSON)]
    public class WaitingForInputS2C : IS2C
    {
        [DataMember(Name = "a")]
        string[] Players;

        public WaitingForInputS2C(string[] players)
        {
            Players = players;
        }

        void IS2C.Execute(PboClient client)
        {
            RoomController.OnTimeReminder(Players.Select((p) => client.RoomController.GetUser(p)).ToArray());
        }
    }

    [DataContract(Name = "gamestart", Namespace = PBOMarks.JSON)]
    public class GameStartS2C : ReportFragment, IS2C
    {
        public GameStartS2C(ReportFragment rf)
          : base(rf)
        {
        }

        void IS2C.Execute(PboClient client)
        {
            client.RoomController.GameStart(this);
        }
    }

    [KnownType("KnownEvents")]
    [DataContract(Name = "gameupdate", Namespace = PBOMarks.JSON)]
    public class GameUpdateS2C : IS2C
    {
        static Type[] knownGameEvents;
        static IEnumerable<Type> KnownEvents()
        {
            if (knownGameEvents == null) knownGameEvents = typeof(GameEvent).SubClasses();
            return knownGameEvents;
        }

        [DataMember]
        public GameEvent[] Es;

        public GameUpdateS2C(GameEvent[] events)
        {
            Es = events;
        }

        void IS2C.Execute(PboClient client)
        {
            client.RoomController.Update(Es);
        }
    }

    [DataContract(Name = "gameend", Namespace = PBOMarks.JSON)]
    public class GameEndS2C : IS2C
    {
        public static GameEndS2C GameStop(string player, GameStopReason reason)
        {
            return new GameEndS2C() { Player = player, Reason = reason };
        }
        public static GameEndS2C TimeUp(KeyValuePair<string, int>[] time)
        {
            return new GameEndS2C() { Time = time };
        }
        [DataMember(EmitDefaultValue = false)]
        string Player;
        [DataMember(EmitDefaultValue = false)]
        GameStopReason Reason;

        [DataMember(EmitDefaultValue = false)]
        KeyValuePair<string, int>[] Time;

        private GameEndS2C()
        {
        }
        void IS2C.Execute(PboClient client)
        {
            if (Player != null) client.RoomController.OnGameStop(Reason, client.RoomController.GetUser(Player));
            else if (Time != null) RoomController.OnTimeUp(Time.Select((p) => new KeyValuePair<User, int>(client.RoomController.GetUser(p.Key), p.Value)).ToArray());
            else client.RoomController.OnGameStop(Reason, null);
            client.RoomController.Reset();
        }
    }
}
