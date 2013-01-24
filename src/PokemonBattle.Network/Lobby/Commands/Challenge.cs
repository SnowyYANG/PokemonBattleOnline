using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using PokemonBattleOnline.Data;
using GameInitSettings = PokemonBattleOnline.Network.Room.GameInitSettings;

namespace PokemonBattleOnline.Network.Lobby.Commands
{
  internal class Challenge : P2PCommand
  {
    public override void Execute(Client client, User from)
    {
      throw new NotImplementedException();
    }
  }
  internal class CancelChallenge : P2PCommand
  {
    public override void Execute(Client client, User from)
    {
      throw new NotImplementedException();
    }
  }
  internal class RefuseChallenge : P2PCommand
  {
    public override void Execute(Client client, User from)
    {
      throw new NotImplementedException();
    }
  }
  internal class AcceptChallenge : P2PCommand
  {
    public override void Execute(Client client, User from)
    {
      throw new NotImplementedException();
    }
  }
  //public class ChallengeManager : ClientService
  //{
  //  private readonly object locker = new object();
  //  private readonly Hosts Hosts;
  //  private readonly BattleClient Battle;
  //  private IPokemonData[] challengingPms;
  //  private GameInitSettings currentSettings; //被挑战的临时游戏设置与此变量无关
    
  //  internal ChallengeManager(Hosts hosts, BattleClient battle)
  //    : base(hosts.Client, MessageHeaders.CHALLENGE, MessageHeaders.ACCEPT_CHALLENGE, MessageHeaders.REFUSE_CHALLENGE, MessageHeaders.CANCEL_CHALLENGE)
  //  {
  //    Hosts = hosts;
  //    Battle = battle;
  //  }

  //  protected override void ReadMessage(User sender, byte header, System.IO.BinaryReader reader)
  //  {
  //    switch (header)
  //    {
  //      case MessageHeaders.CHALLENGE:
  //        OnChallenged(sender, reader.ReadSettings());
  //        break;
  //      case MessageHeaders.CANCEL_CHALLENGE:
  //        OnChallengeCanceled(sender);
  //        break;
  //      case MessageHeaders.ACCEPT_CHALLENGE:
  //        OnChallengeAccepted(sender);
  //        break;
  //      case MessageHeaders.REFUSE_CHALLENGE:
  //        OnChallengeRefused(sender);
  //        break;
  //    };
  //  }

  //  #region Challenge
  //  public event Action<User, GameInitSettings> Challenged = delegate { };
  //  private void OnChallenged(User user, GameInitSettings settings)
  //  {
  //    settings.Lock();
  //    Challenged(user, settings);
  //  }
  //  public bool Challenge(int target, IPokemonData[] pokemons, GameInitSettings settings)
  //  {
  //    User u = Client.GetUser(target);
  //    if (u != null && u.State != UserState.Battling && pokemons != null && pokemons.Length > 0) //it's impossible for a client to get UserState.Invalid
  //      lock (locker)
  //      {
  //        if (challengingPms == null)
  //        {
  //          SendMessage(MessageHeaders.CHALLENGE, writer => writer.WriteSettings(settings), target);
  //          challengingPms = pokemons;
  //          currentSettings = settings;
  //          currentSettings.Lock();
  //          return true;
  //        }
  //      }
  //    return false;
  //  }
  //  #endregion

  //  #region CancelChallenge
  //  /// <summary>
  //  /// remember to determine user!=null
  //  /// </summary>
  //  public event Action<User> ChallengeCanceled = delegate { };
  //  private void OnChallengeCanceled(User user)
  //  {
  //    ChallengeCanceled(user);
  //  }
  //  public void CancelChallenge(int target)
  //  {
  //    lock (locker)
  //    {
  //      if (challengingPms != null)
  //      {
  //        SendMessage(MessageHeaders.CANCEL_CHALLENGE, target);
  //        challengingPms = null;
  //      }
  //    }
  //  }
  //  #endregion

  //  #region RefuseChallenge
  //  public event Action<User> ChallengeRefused = delegate { };
  //  private void OnChallengeRefused(User user)
  //  {
  //    lock (locker)
  //    {
  //      challengingPms = null;
  //    }
  //    ChallengeRefused(user);
  //  }
  //  public void RefuseChallenge(int challenger)
  //  {
  //    SendMessage(MessageHeaders.REFUSE_CHALLENGE, challenger);
  //  }
  //  #endregion

  //  #region AcceptChallenge & StartGame
  //  public event Action<User> ChallengeAccepted = delegate { };
  //  private void OnChallengeAccepted(User user)
  //  {
  //    lock (locker)
  //    {
  //      ChallengeAccepted(user);
  //      Hosts.AddHost(currentSettings, true);
  //      Battle.JoinGame(Client.User.Id, 0, challengingPms);
  //      challengingPms = null;
  //    }
  //  }
  //  public bool AcceptChallenge(int challenger, IPokemonData[] pokemons)
  //  {
  //    if (pokemons != null && pokemons.Length > 0)
  //      lock (locker)
  //        if (Battle.CanEnterRoom)
  //        {
  //          SendMessage(MessageHeaders.ACCEPT_CHALLENGE, challenger);
  //          //假如对方房间还没设好怎么办...测试了似乎没问题？
  //          Battle.JoinGame(challenger, 1, pokemons);
  //          challengingPms = null;
  //          return true;
  //        }
  //    return false;
  //  }
  //  #endregion
  //}
}
