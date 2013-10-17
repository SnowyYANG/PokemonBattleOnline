using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.Network.Commands;

namespace PokemonBattleOnline.Network
{
  public class ClientController
  {
    public static event Action Disconnected;
    internal static void OnDisconnected()
    {
      if (Disconnected != null) Disconnected();
    }
    public static event Action<string, User> PublicChat;
    internal static void OnPublicChat(string chat, User user)
    {
      if (PublicChat != null) PublicChat(chat, user);
    }
    public static event Action<string, User> PrivateChat;
    internal static void OnPrivateChat(string chat, User user)
    {
      PrivateChat(chat, user);
    }
    public static event Action<string, User> RoomChat;
    internal static void OnRoomChat(string chat, User user)
    {
      RoomChat(chat, user);
    }

    private readonly Client Client;
    
    internal ClientController(Client client)
    {
      Client = client;
    }

    private void Send(IC2S c2s)
    {
      Client.SendC2S(c2s);
    }

    public void ChatPublic(string chat)
    {
      Send(ChatC2S.PublicChat(chat));
    }
    public void ChatPrivate(User to, string chat)
    {
      Send(ChatC2S.PrivateChat(to.Id, chat));
    }
    public void ChatRoom(string chat)
    {
      if (Client.State.User.Room != null) Send(ChatC2S.RoomChat(chat));
    }
    public void NewRoom()
    {
    }
    public void EnterRoom(Room room, Seat seat)
    {
    }
    public void ChangeSeat(Seat seat)
    {
    }
    public void GameReady(PokemonData[] team)
    {
    }
    public void GameInput(ActionInput input)
    {
    }
  }
}
