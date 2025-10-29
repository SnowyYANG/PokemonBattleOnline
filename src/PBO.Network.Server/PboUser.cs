using PokemonBattleOnline;
using PokemonBattleOnline.Network;
using PokemonBattleOnline.Network.Commands;
using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;
using WebSocketSharp;
using WebSocketSharp.Server;

internal class PboUser : WebSocketBehavior
{
    private static readonly DataContractJsonSerializer C2SSerializer;
    public static readonly DataContractJsonSerializer S2CSerializer;

    static PboUser()
    {
        var c2s = typeof(IC2SE);
        C2SSerializer = new DataContractJsonSerializer(c2s, c2s.SubClasses());
        var s2c = typeof(IS2C);
        S2CSerializer = new DataContractJsonSerializer(s2c, s2c.SubClasses());
    }

    private object Locker
    { get { return Server.Locker; } }
    public readonly PboServer Server;

    public PboUser(PboServer server)
    {
        Server = server;
    }

    public User User
    { get; internal set; }

    public RoomHost Room
    { get; internal set; }

    protected override void OnOpen()
    {
        base.OnOpen();
        Send(new WelcomeS2C(PBOMarks.VERSION.ToString()));
    }

    public void Init(string name, string room, Seat seat)
    {
        lock (Locker)
        {
            User = new User(ID, name, room, seat);
            Server.AddUser(ID, this);
        }
    }

    protected override void OnMessage(WebSocketSharp.MessageEventArgs e)
    {
        if (e.Data[0] != '{' && e.Data[0] != '[')
        {
            // Handle non-JSON messages here
            return;
        }
        // Handle WebSocket messages here
        IC2SE c2s = null;
        using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(e.Data)))
        {
            c2s = (IC2SE)C2SSerializer.ReadObject(ms) as IC2SE;
        }
        lock (Locker)
        {
            c2s?.Execute(this);
        }
    }

    protected override void OnClose(CloseEventArgs e)
    {
        base.OnClose(e);
        lock (Locker)
        {
            //notify all users
            Server.RemoveUser(this);
        }
    }

    protected override void OnError(WebSocketSharp.ErrorEventArgs e)
    {
        base.OnError(e);
        lock (Locker)
        {
            //norify all users
            Server.RemoveUser(this);
        }
    }

    public new void Send(string json)
    {
        base.Send(json);
    }

    public void Send(IS2C command)
    {
        using (var ms = new MemoryStream())
        {
            C2SSerializer.WriteObject(ms, command);
            ms.Position = 0;
            using (var sr = new StreamReader(ms, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, bufferSize: 1024, leaveOpen: true))
            {
                Send(sr.ReadToEnd());
            }
        }
    }
}
