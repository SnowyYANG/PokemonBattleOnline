using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Game.Host
{
  internal sealed class Tile : ConditionalObject
  {
    public const int NOPM_INDEX = -1;

    public readonly Field Field;
    public readonly int X;

    internal Tile(Field team, int x, IGameSettings gameSettings)
    {
      Field = team;
      X = x;
      _speed = (Team << 3) + x;
      WillSendOutPokemonIndex = gameSettings.Mode.GetPokemonIndex(x);
    }

    public int Team
    { get { return Field.Team; } }
    public PokemonProxy Pokemon
    { get; internal set; }

    public int WillSendOutPokemonIndex
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
      var h = pm.Hp != pm.Pokemon.MaxHp;
      var s = pm.State != PokemonState.Normal;
      if ((h || s) && HasCondition("HealingWish"))
      {
        if (h) pm.Pokemon.Hp = pm.Pokemon.MaxHp;
        if (s) pm.Pokemon.State = PokemonState.Normal;
        pm.AddReportPm("HealingWish");
      }
      else if ((h || s || pm.Pokemon.Moves.Any((m) => m.PP.Origin != m.PP.Value)) && HasCondition("LunarDance"))
      {
        if (h) pm.Pokemon.Hp = pm.Pokemon.MaxHp;
        if (s) pm.Pokemon.State = PokemonState.Normal;
        foreach (var m in pm.Moves) m.PP = m.Move.PP.Origin;
        pm.AddReportPm("LunarDance");
      }
    }
  }
}
