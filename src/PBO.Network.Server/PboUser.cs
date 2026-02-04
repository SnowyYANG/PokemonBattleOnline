using PokemonBattleOnline;
using PokemonBattleOnline.Network;
using PokemonBattleOnline.Network.Commands;
using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;

internal class PboUser
{
    private object Locker
    { get { return Server.Locker; } }
    public readonly PboServer Server;

    public PboUser(PboServer server)
    {
        Server = server;
    }

    public Guid Guid
    { get; internal set; }

    public User User
    { get; internal set; }

    public RoomHost Room
    { get; internal set; }
}
