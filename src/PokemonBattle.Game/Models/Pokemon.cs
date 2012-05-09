using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  /// <summary>
  /// do NOT binding directly
  /// </summary>
  public class Pokemon
  {
    public readonly int Id;
    public readonly Player Owner;
    public readonly int TeamId;

    #region data
    public string Name { get; private set; }
    public PokemonType PokemonType { get; private set; }
    public PokemonGender Gender { get; private set; }
    public int Lv { get; private set; }
    public Ability Ability { get; private set; }
    public Move[] Moves { get; private set; }
    public PokemonNature Nature { get; private set; }

    public readonly ReadOnly6D Base;
    public readonly ReadOnly6D Iv;
    public readonly ReadOnly6D Ev;
    public ReadOnly6D Static { get; private set; }

    private PairValue hp;
    public Item Item { get; set; }
    public PokemonState State { get; set; }
    #endregion

    public readonly int StruggleId;
    public readonly int SwitchId;

    internal Pokemon(Player owner, PokemonCustomInfo custom, GameSettings settings)
    {
      Id = settings.NextId();
      Owner = owner;
      TeamId = owner.TeamId;

      Name = custom.Name;
      PokemonType = DataService.GetPokemonType(custom.PokemonTypeId);
      Gender = custom.Gender;
      Lv = custom.Lv;
      Ability = DataService.GetAbility(custom.AbilityId);
      Nature = custom.Nature;

      {
        Moves = new Move[4];
        int i = 0;
        foreach (int moveId in custom.MoveIds)
          if (i < 4) Moves[i++] = new Move(moveId, settings);
        StruggleId = settings.NextId();
        SwitchId = settings.NextId();
      }

      Base = new ReadOnly6D(PokemonType.BaseHp, PokemonType.BaseAtk, PokemonType.BaseDef, PokemonType.BaseSpAtk, PokemonType.BaseSpDef, PokemonType.BaseSpeed);
      Iv = new ReadOnly6D(custom.HpIv, custom.AtkIv, custom.DefIv, custom.SpAtkIv, custom.SpDefIv, custom.SpeedIv);
      Ev = new ReadOnly6D(custom.HpEv, custom.AtkEv, custom.DefEv, custom.SpAtkEv, custom.SpDefEv, custom.SpeedEv);
      Static = new ReadOnly6D(GetState(StatType.Hp), GetState(StatType.Atk), GetState(StatType.Def), GetState(StatType.SpAtk), GetState(StatType.SpDef), GetState(StatType.Speed));
      
      if (custom.ItemId.HasValue) Item = DataService.GetItem(custom.ItemId.Value);
      hp = new PairValue(Static.Hp, Static.Hp, 48);
    }

    public int IndexInOwner
    { get { return Owner.GetPokemonIndex(Id); } }
    public IPairValue Hp
    { get { return hp; } }

    private int GetState(StatType type)
    {
      if (type == StatType.Hp) return PokemonStatHelper.GetHp(Base.Hp, (byte)Iv.Hp, (byte)Ev.Hp, (byte)Lv);
      else return PokemonStatHelper.GetStat(type, Nature, Base.GetStat(type), (byte)Iv.GetStat(type), (byte)Ev.GetStat(type), (byte)Lv);
    }
    internal void SetHp(int value)
    {
      if (value < 0) value = 0;
      else if (value > Hp.Origin) value = Hp.Origin;
      hp.Value = value;
    }
  }
}
