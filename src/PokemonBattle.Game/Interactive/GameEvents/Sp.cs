using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game.Host;

namespace LightStudio.PokemonBattle.Game.GameEvents
{
  [DataContract(Namespace = Namespaces.LIGHT)]
  internal class BeginTurn : GameEvent
  {
    [DataMember(EmitDefaultValue = false)]
    int TurnNumber;

    public BeginTurn(int turnNumber)
    {
      TurnNumber = turnNumber;
    }

    protected override void Update()
    {
      if (TurnNumber == 0) AppendGameLog("GameStart");
      else AppendGameLog("BeginTurn", TurnNumber);
    }
  }
  [DataContract(Namespace = Namespaces.LIGHT)]
  internal class EndTurn : GameEvent
  {
    protected override void Update()
    {
      Game.EndTurn();
    }
  }
  
  [DataContract(Namespace = Namespaces.LIGHT)]
  internal class SelectMoveFail : GameEvent
  {
    [DataMember]
    public readonly string Key;

    [DataMember(EmitDefaultValue = false)]
    public readonly int Move; //知道怎么区分Block和Only了吧...

    public SelectMoveFail(string key, int move)
    {
      Key = key;
      Move = move;
    }

    protected override void Update()
    {
      var log = GetGameLog(Key);
      ((LogText)log).HiddenAfterBattle = true;
      log.SetData(Move);
      Game.AppendGameLog(log);
    }
  }
  [DataContract(Namespace = Namespaces.LIGHT)]
  internal class WithdrawFail : GameEvent
  {
    [DataMember]
    public readonly string Key;

    [DataMember]
    public readonly int Pm;

    [DataMember(EmitDefaultValue = false)]
    public readonly int AbPm;

    [DataMember(EmitDefaultValue = false)]
    public readonly int Ab;

    public WithdrawFail(string key, PokemonProxy pm, PokemonProxy abilityPm = null)
    {
      Key = key == "CantWithdraw"? null : key;
      Pm = pm.Id;
      //if (abilityPm != null)
      //{
      //  AbPm = abilityPm.Id;
      //  Ab = abilityPm.OnboardPokemon.Ability;
      //}
    }

    protected override void Update()
    {
      //if (Ab != 0)
      //{
      //  var log = GetGameLog("Ability");
      //  ((LogText)log).HiddenAfterBattle = true;
      //  log.SetData(AbPm, Ab);
      //  Game.Board.ShowAbility(GetPokemon(AbPm), DataService.GetAbility(Ab));
      //  Game.AppendGameLog(log);
      //}
      {
        var log = GetGameLog(Key ?? "CantWithdraw");
        ((LogText)log).HiddenAfterBattle = true;
        log.SetData(Pm);
        Game.AppendGameLog(log);
      }
    }
  }
}