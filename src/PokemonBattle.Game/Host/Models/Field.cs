using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game.Host.Sp;

namespace LightStudio.PokemonBattle.Game.Host
{
  public class Field : ConditionalObject
  {
    private readonly List<EntryHazards> hazards;

    public readonly int Team;
    private readonly Tile[] tiles;

    internal Field(int team, IGameSettings settings)
    {
      Team = team;
      hazards = new List<EntryHazards>(3);
      tiles = new Tile[settings.Mode.XBound()];
      for (int x = 0; x < tiles.Length; ++x) tiles[x] = new Tile(this, x, settings); 
    }

    public Tile this[int x]
    { get { return tiles.ValueOrDefault(x); } }

    public IEnumerable<Tile> Tiles
    { get { return tiles; } }
    public IEnumerable<PokemonProxy> Pokemons
    {
      get
      {
        return
          from t in Tiles
          where t.Pokemon != null
          select t.Pokemon; 
      }
    }

    public IEnumerable<PokemonProxy> GetPokemons(int minX, int maxX)
    {
      return
        from t in Tiles
        where t.X >= minX && t.X < maxX && t.Pokemon != null
        select t.Pokemon;
    }
    public void EnEntryHazards(MoveType move)
    {
      foreach (var eh in hazards)
        if (eh.Move == move)
        {
          eh.En();
          return;
        }
      hazards.Add(EntryHazards.New(move));
    }
    public void DeEntryHazards(ReportBuilder report)
    {
      foreach (var eh in hazards) eh.De(report, Team);
      hazards.Clear();
    }
    /// <summary>
    /// 治愈之愿，钉子
    /// </summary>
    /// <param name="pm"></param>
    /// <returns>still alive</returns>
    public bool Debut(PokemonProxy pm)
    {
      foreach (var eh in hazards)
      {
        eh.Debut(pm);
        if (pm.CheckFaint()) return false;
      }
      return true;
    }

    private abstract class EntryHazards
    {
      public static EntryHazards New(MoveType move)
      {
        switch (move.Id)
        {
          case Moves.SPIKES:
            return new Spikes();
          case Moves.TOXIC_SPIKES:
            return new ToxicSpikes();
          default:
            return new StealthRock();
        }
      }
      
      public readonly MoveType Move;
      protected EntryHazards(int move)
      {
        Move = GameDataService.GetMove(move);
      }
      public abstract void En();
      public abstract void De(ReportBuilder report, int team);
      public abstract void Debut(PokemonProxy pm); //欢迎登场，口耐的精灵们（笑

      #region nested class
      private class Spikes : EntryHazards
      {
        int n;
        public Spikes()
          : base(Moves.SPIKES)
        {
          n = 8;
        }
        public override void En()
        {
          if (n == 8) n = 6;
          else n = 4;
        }
        public override void De(ReportBuilder report, int team)
        {
          report.Add("DeSpikes", team);
        }
        public override void Debut(PokemonProxy pm)
        {
          if (pm.CanEffectHurt && EffectsService.IsGroundAffectable.Execute(pm, false, false))
            pm.EffectHurtByOneNth(n, "Spikes");
        }
      }
      private class ToxicSpikes : EntryHazards
      {
        bool badly;
        public ToxicSpikes()
          : base(Moves.TOXIC_SPIKES)
        {
        }
        public override void En()
        {
          badly = true;
        }
        public override void De(ReportBuilder report, int team)
        {
          report.Add("DeToxicSpikes", team);
        }
        public override void Debut(PokemonProxy pm)
        {
          if (pm.CanAddState(pm, AttachedState.PSN, false) && EffectsService.IsGroundAffectable.Execute(pm, false, false))
            pm.AddState(pm, AttachedState.PSN, false, 15);
        }
      }
      private class StealthRock : EntryHazards
      {
        public StealthRock()
          : base(Moves.STEALTH_ROCK)
        {
        }
        public override void En()
        {
        }
        public override void De(ReportBuilder report, int team)
        {
          report.Add("DeStealthRock", team);
        }
        public override void Debut(PokemonProxy pm)
        {
          int revise = BattleType.Rock.EffectRevise(pm.OnboardPokemon.Type1) + BattleType.Rock.EffectRevise(pm.OnboardPokemon.Type2);//羽栖有效无效都无所谓
          int hp = (revise > 0 ? pm.Pokemon.Hp.Origin << revise : pm.Pokemon.Hp.Origin >> -revise) >> 3;
          if (pm.CanEffectHurt) pm.EffectHurt(hp, "StealthRock");
        }
      }
      #endregion
    }
  }
}
