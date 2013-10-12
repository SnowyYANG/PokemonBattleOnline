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
    private readonly Client Client;
    
    internal ClientController(Client client)
    {
      Client = client;
    }

    private void Send(IC2S c2s)
    {
      Client.SendC2S(c2s);
    }

    public void PublicChat(string chat)
    {
      Send(ChatC2S.PublicChat(chat));
    }
    public void PrivateChat(User to, string chat)
    {
      Send(ChatC2S.PrivateChat(to.Id, chat));
    }
    public void RoomChat(string chat)
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
