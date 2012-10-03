using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game.Host;

namespace LightStudio.PokemonBattle.Game.GameEvents
{
  [DataContract(Namespace = Namespaces.PBO)]
  internal class BeginTurn : GameEvent
  {
    protected override void Update()
    {
      ++Game.TurnNumber;
      if (Game.TurnNumber == 0) AppendGameLog("GameStart");
      else AppendGameLog("BeginTurn", Game.TurnNumber);
    }
  }

  [DataContract(Namespace = Namespaces.PBO)]
  internal class EndTurn : GameEvent
  {
    protected override void Update()
    {
      if (Game.TurnNumber != 0)
      {
        AppendGameLog("EndTurn", Game.TurnNumber);
        for (int t = 0; t < Game.Settings.Mode.TeamCount(); ++t)
          for (int x = 0; x < Game.Settings.Mode.XBound(); ++x)
          {
            var pm = Game.Board[t, x];
            if (pm != null)
            {
              if (pm.State == PokemonState.Normal) AppendGameLog("EndTurnNormalPm", pm.Id, pm.Hp.Value);
              else AppendGameLog("EndTurnAbnormalPm", pm.Id, pm.Hp.Value, pm.State);
            }
          }
      }
      Game.EndTurn();
    }
  }
  
  [DataContract(Namespace = Namespaces.PBO)]
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

  [DataContract(Namespace = Namespaces.PBO)]
  internal class HorizontalLine : GameEvent
  {
    protected override void Update()
    {
      AppendGameLog("----");
    }
  }
}