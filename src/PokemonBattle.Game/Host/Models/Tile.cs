using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace LightStudio.PokemonBattle.Game.Host
{
  [DataContract(Namespace = Namespaces.PBO)]
  public sealed class Tile : ConditionalObject
  {
    public const int NOPM_INDEX = -1;

    [DataMember(EmitDefaultValue = false)]
    public readonly int Team;
    [DataMember(EmitDefaultValue = false)]
    public readonly int X;
    private int speed;

    internal Tile(int team, int x, IGameSettings gameSettings)
    {
      Team = team;
      X = x;
      speed = (team << 3) + x;
      WillSendoutPokemonIndex = gameSettings.Mode.GetPokemonIndex(x);
    }

    public PokemonProxy Pokemon
    { get; internal set; }

    public int WillSendoutPokemonIndex
    { get; internal set; }
    public int Speed
    { 
      get
      {
        if (Pokemon != null)
          speed = Pokemon.Speed;
        return speed;
      }
    }

    internal void Debut()
    {
      var pm = Pokemon;
      bool s = !(pm.State == PokemonState.Normal && pm.Hp == pm.Pokemon.Hp.Origin);
      if (s && HasCondition("HealingWish"))
      {
        pm.Pokemon.SetHp(pm.Pokemon.Hp.Origin);
        pm.Pokemon.State = PokemonState.Normal;
        pm.Controller.ReportBuilder.Add(new GameEvents.HLLD(pm, false));
      }
      else if ((s || pm.Pokemon.Moves.Any((m) => m.PP.Origin != m.PP.Value)) && HasCondition("LunarDance"))
      {
        pm.Pokemon.SetHp(pm.Pokemon.Hp.Origin);
        pm.Pokemon.State = PokemonState.Normal;
        foreach (var m in pm.Moves) m.PP = m.Move.PP.Origin;
        pm.Controller.ReportBuilder.Add(new GameEvents.HLLD(pm, true));
      }
    }
  }
}
