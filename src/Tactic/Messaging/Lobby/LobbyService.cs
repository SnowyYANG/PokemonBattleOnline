using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.Tactic.Messaging.Lobby;

namespace LightStudio.PokemonBattle.Messaging
{
  public static class LobbyService
  {
    private static LobbyClient client;

    public static User User
    { get { return client.User; } }

    public static void Register(LobbyClient client)
    {
      lock(client)
        LobbyService.client = client;
    }
    public static void Deregister()
    {
      lock(client)
        client = null;
    }
    public static User GetUser(int userId)
    {
      return client.GetUser(userId);
    }
  }
}
