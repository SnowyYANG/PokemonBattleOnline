using PokemonBattleOnline.Network.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using WatsonWebsocket;

namespace PokemonBattleOnline.Network
{
    public class PboServer : IDisposable
    {
        private static readonly DataContractJsonSerializer C2SESerializer;
        public static readonly DataContractJsonSerializer S2CSerializer;
        public static PboServer Current { get; private set; }

        static PboServer()
        {
            var c2se = typeof(IC2SE);
            C2SESerializer = new DataContractJsonSerializer(c2se, c2se.SubClasses());
            var s2c = typeof(IS2C);
            S2CSerializer = new DataContractJsonSerializer(s2c, s2c.SubClasses());
        }


        internal readonly object Locker = new object();

        private static void OnKeepAlive(object state)
        {
#if !TEST
            var server = (PboServer)state;
            var lastPack = DateTime.Now.AddMilliseconds(-2d * PBOMarks.TIMEOUT);
#endif
        }

        private readonly WatsonWsServer ws;
        private readonly Dictionary<string, PboUser> Users = new Dictionary<string, PboUser>(); //已登录用户
        private readonly Dictionary<string, RoomHost> Rooms = new Dictionary<string, RoomHost>();
        private readonly int Port;
        private readonly Timer KeepAliveTimer;

        public PboServer(int port)
        {
            Current = this;
            ws = new WatsonWsServer("localhost", port, false);
            ws.ClientConnected += ClientConnected;
            ws.ClientDisconnected += ClientDisconnected;
            ws.MessageReceived += MessageReceived; 
            Port = port;
            KeepAliveTimer = new Timer(OnKeepAlive, this, PBOMarks.TIMEOUT << 1, PBOMarks.TIMEOUT << 1);
        }

        public void Start()
        {
            ws.Start();
        }

        internal void Send(IS2C s2c)
        {
            lock (Locker)
            {
                foreach (var user in Users.Values)
                {
                    Send(user, s2c);
                }
            }
        }

        internal async void Send(PboUser user, IS2C s2c)
        {
            Send(user.Guid, s2c);
        }

        internal async void Send(Guid clientGuid, IS2C s2c)
        {
            try
            {
                string json;
                using (var ms = new MemoryStream())
                {
                    S2CSerializer.WriteObject(ms, s2c);
                    ms.Position = 0;
                    using (var sr = new StreamReader(ms))
                    {
                        json = sr.ReadToEnd();
                    }
                }
                await ws.SendAsync(clientGuid, json);
            }
            catch
            {
            }
        }

        internal async void Send(string name, IS2C s2c)
        {
            Send(GetUser(name), s2c);
        }

        internal void ClientConnected(object sender, ConnectionEventArgs args)
        {
            Console.WriteLine("ClientConnected: " + args.Client.Guid);
            Send(args.Client.Guid, new WelcomeS2C(PBOMarks.VERSION.ToString()));
        }
        internal void ClientDisconnected(object sender, DisconnectionEventArgs args)
        {
            Console.WriteLine("ClientDisconnected: " + args.Client.Guid);
            lock (Locker)
            {
                var user = GetUser(args.Client.Guid);
                if (user != null)
                {
                    RemoveUser(user);
                }
            }
        }
        internal void MessageReceived(object sender, MessageReceivedEventArgs args)
        {
            Console.WriteLine("MessageReceived: " + args.Client.Guid);
            if (args.Data[0] != '{' && args.Data[0] != '[')
            {
                Console.WriteLine("Message is not JSON.");
                return;
            }
            // Handle WebSocket messages here
            IC2SE c2s = null;
            using (var ms = new MemoryStream(args.Data.Array, args.Data.Offset, args.Data.Count))
            {
                Console.WriteLine("Message: " + Encoding.UTF8.GetString(args.Data.Array, args.Data.Offset, args.Data.Count));
                try
                {
                    c2s = (IC2SE)C2SESerializer.ReadObject(ms);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to deserialize command: " + ex.ToString());
                }
            }
            if (c2s == null)
            {
                Console.WriteLine("Unknown command received: " + Encoding.UTF8.GetString(args.Data.Array, args.Data.Offset, args.Data.Count));
                return;
            }
            Console.WriteLine("Received command: " + c2s.GetType().Name);
            lock (Locker)
            {
                var user = GetUser(args.Client.Guid);
                if (user == null && c2s is IC2SE)
                {
                    user = new PboUser(this) { Guid = args.Client.Guid };
                }
                if (user != null)
                {
                    c2s.Execute(user);
                }
            }
        }
        internal PboUser GetUser(string name)
        {
            return Users.ValueOrDefault(name);
        }
        internal PboUser GetUser(Guid clientGuid)
        {
            return Users.Values.FirstOrDefault(u => u.Guid == clientGuid);
        }   
        internal void AddUser(PboUser user, string name, string roomId, Seat seat)
        {
            user.User = new User(name, roomId, seat);
            lock (Locker)
            {
                RoomHost rh = null;
                if (!Rooms.TryGetValue(roomId, out rh))
                {
                    rh = AddRoom(roomId);
                    Console.WriteLine("new room " + roomId);
                }
                user.Room = rh;
                Send(user, new ClientInitS2C(name, rh.Room.GetUsers()));
                rh.AddUser(user, user.User.Seat);
                user.Room.Send(UserS2C.AddUser(user.User.Name, user.Room.Room.Id, user.User.Seat));
                Users.Add(user.User.Name, user);
            }
            Console.WriteLine("({0}) {1} has entered the lobby.", DateTime.Now, user.User.Name);
        }
        internal void RemoveUser(PboUser user)
        {
            lock (Locker)
            {
                user.Room?.Send(UserS2C.RemoveUser(user.User.Name));
                user.Room?.RemoveUser(user);
                Users.Remove(user.User.Name);
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
