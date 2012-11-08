using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host
{
  public sealed class Tile : ConditionalObject
  {
    public const int NOPM_INDEX = -1;

    public readonly Field Field;
    public readonly int X;

    internal Tile(Field team, int x, IGameSettings gameSettings)
    {
      Field = team;
      X = x;
      _speed = (Team << 3) + x;
      WillSendoutPokemonIndex = gameSettings.Mode.GetPokemonIndex(x);
    }

    public int Team
    { get { return Field.Team; } }
    public PokemonProxy Pokemon
    { get; internal set; }

    public int WillSendoutPokemonIndex
    { get; internal set; }
    private int _speed;
    public int Speed
    { 
      get
      {
        if (Pokemon != null) _speed = Pokemon.Speed;
        return _speed;
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
