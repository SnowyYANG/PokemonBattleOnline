using PokemonBattleOnline.Network.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace PokemonBattleOnline.Network
{
    public class PboServer : IDisposable
    {
        public static PboServer Current { get; private set; }

        internal readonly object Locker = new object();

        private static void OnKeepAlive(object state)
        {
#if !TEST
            var server = (PboServer)state;
            var lastPack = DateTime.Now.AddMilliseconds(-2d * PBOMarks.TIMEOUT);
#endif
        }

        private readonly WebSocketServer ws;
        private readonly Dictionary<string, PboUser> Users = new Dictionary<string, PboUser>(); //已登录用户
        private readonly Dictionary<string, RoomHost> Rooms = new Dictionary<string, RoomHost>();
        private readonly int Port;
        private readonly Timer KeepAliveTimer;

        public PboServer(int port)
        {
            Current = this;
            ws = new WebSocketServer(port);
            ws.AddWebSocketService("/", () => new PboUser(this));
            Port = port;
            KeepAliveTimer = new Timer(OnKeepAlive, this, PBOMarks.TIMEOUT << 1, PBOMarks.TIMEOUT << 1);
        }

        public void Start()
        {
            ws.Start();
        }

        internal void Send(IS2C s2c)
        {
            try
            {
                string json;
                using (var ms = new MemoryStream())
                {
                    var serializer = PboUser.S2CSerializer;
                    serializer.WriteObject(ms, s2c);
                    ms.Position = 0;
                    using (var sr = new StreamReader(ms))
                    {
                        json = sr.ReadToEnd();
                    }
                }
                foreach (var u in Users.Values) u.Send(json);
            }
            catch
            {
            }
        }
        internal PboUser GetUser(string id)
        {
            return Users.ValueOrDefault(id);
        }
        internal void AddUser(string id, PboUser user, string roomId)
        {
            lock (Locker)
            {
                RoomHost rh = null;
                if (!Rooms.TryGetValue(roomId, out rh)) rh = AddRoom(roomId);
                user.Room = rh;
                user.Send(new ClientInitS2C(id, rh.Room.GetUsers()));
                rh.AddUser(user, user.User.Seat);
                user.Room.Send(UserS2C.AddUser(user.ID, user.User.Name, user.Room.Room.Id, user.User.Seat));
                Users.Add(user.ID, user);
            }
            Console.WriteLine("({0}) {1} has entered the lobby.", DateTime.Now, user.User.Name);
        }
        internal void RemoveUser(PboUser user)
        {
            lock (Locker)
            {
                user.Room?.Send(UserS2C.RemoveUser(user.ID));
                if (user.Room != null) user.Room.RemoveUser(user);
                Users.Remove(user.ID);
            }
            Console.WriteLine("({0}) {1} has left the lobby.", DateTime.Now, user.User.Name);
        }
        internal RoomHost GetRoom(string id)
        {
            return Rooms.ValueOrDefault(id);
        }

        /// <summary>
        /// 追加空房间
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private RoomHost AddRoom(string id)
        {
            var rc = new RoomHost(this, id);
            Rooms.Add(id, rc);
            return rc;
        }

        internal void RemoveRoom(RoomHost rc)
        {
            var room = rc.Room;
            if (Rooms.Remove(room.Id))
            {
                rc.Dispose();
            }
        }

        public void Dispose()
        {
            KeepAliveTimer.Dispose();
            ws.Stop();
        }
    }
}
