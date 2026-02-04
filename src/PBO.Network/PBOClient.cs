using PokemonBattleOnline.Game;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;

namespace PokemonBattleOnline.Network
{
    public class PboClient
    {
        private static readonly DataContractJsonSerializer C2SSerializer;
        private static readonly DataContractJsonSerializer S2CSerializer;

        public WatsonWebsocket.WatsonWsClient ws;

        public RoomController RoomController
        { get; internal set; }
        internal bool inited;

        static PboClient()
        {
            var c2s = typeof(IC2S);
            C2SSerializer = new DataContractJsonSerializer(c2s, c2s.SubClasses());
            var s2c = typeof(IS2C);
            S2CSerializer = new DataContractJsonSerializer(s2c, s2c.SubClasses());
        }

        public string MyId
        { get; internal set; }

        public string MyRoomId
        { get; private set; }

        public Room Room => RoomController.Room;

        private User _user;
        public User User
        {
            get
            {
                if (_user == null) _user = RoomController.GetUser(MyId);
                return _user;
            }
        }

        public PlayerController PlayerController => RoomController.PlayerController;

        public GameOutward Game => RoomController.Game;

        public PboClient(string url)
        {
            ws = new WatsonWebsocket.WatsonWsClient(new Uri(url));
            ws.MessageReceived += (s, e) =>
            {
                using (var ms = new MemoryStream(e.Data.Array, e.Data.Offset, e.Data.Count))
                {
                    var obj = S2CSerializer.ReadObject(ms) as IS2C;
                    obj.Execute(this);
                }
            };
            ws.Start();
        }

        public void Init(string name, string room, Seat seat)
        {
            MyRoomId = room;
            var init = new Commands.ClientInitC2S()
            {
                name = name,
                room = room,
                seat = seat,
                version = PBOMarks.VERSION.ToString(),
            };
            Send(init);
        }

        public async void Send(IC2S command)
        {
            using (var ms = new MemoryStream())
            {
                C2SSerializer.WriteObject(ms, command);
                ms.Position = 0;
                using (var sr = new StreamReader(ms, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, bufferSize: 1024, leaveOpen: true))
                {
                    await ws.SendAsync(sr.ReadToEnd());
                }
            }
        }
    }
}
