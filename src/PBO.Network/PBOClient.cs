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
        
        internal WebSocketSharp.WebSocket ws;
        internal RoomController Room;
        internal bool inited;

        static PboClient()
        {
            var c2s = typeof(IC2S);
            C2SSerializer = new DataContractJsonSerializer(c2s, c2s.SubClasses());
            var s2c = typeof(IS2C);
            S2CSerializer = new DataContractJsonSerializer(s2c, s2c.SubClasses());
        }

        public PboClient(string url)
        {
            ws = new WebSocketSharp.WebSocket(url);
            ws.OnMessage += (s, e) =>
            {
                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(e.Data)))
                {
                    var obj = S2CSerializer.ReadObject(ms) as IS2C;
                    obj.Execute(this);
                }
            };
            ws.Connect();
        }

        public void Init(string name, string room, Seat seat)
        {
            var init = new Commands.ClientInitC2S()
            {
                name = name,
                room = room,
                seat = seat,
                version = PBOMarks.VERSION.ToString(),
            };
            Send(init);
        }

        public void Send(IC2S command)
        {
            using (var ms = new MemoryStream())
            {
                C2SSerializer.WriteObject(ms, command);
                ms.Position = 0;
                using (var sr = new StreamReader(ms, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, bufferSize: 1024, leaveOpen: true))
                {
                    ws.Send(sr.ReadToEnd());
                }
            }
        }
    }
}
