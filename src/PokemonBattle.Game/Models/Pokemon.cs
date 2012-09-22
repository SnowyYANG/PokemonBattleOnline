using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  public class Pokemon : INotifyPropertyChanged
  {
    private static readonly PropertyChangedEventArgs STATE = new PropertyChangedEventArgs("State");
    
    public event PropertyChangedEventHandler PropertyChanged;
    
    public readonly int Id;
    public readonly Player Owner;
    public readonly int TeamId;

    #region data
    internal readonly int AbilityIndex;
    public readonly ReadOnly6D Iv;
    public readonly ReadOnly6D Ev;

    public string Name { get; private set; }
    public PokemonType PokemonType { get; private set; }
    public PokemonGender Gender { get; private set; }
    public int Lv { get; private set; }
    public Ability Ability { get; private set; }
    public Move[] Moves { get; private set; }
    public byte Happiness { get; private set; }
    public PokemonNature Nature { get; private set; }
    /// <summary>
    /// for binding only
    /// </summary>
    public ReadOnly6D Static { get; private set; }

    private PairValue hp;
    public Item Item { get; set; }
    public PokemonState State { get; set; }
    #endregion

    internal Pokemon(Player owner, PokemonCustomInfo custom, IGameSettings settings, Func<int> nextId)
    {
      Id = nextId();
      Owner = owner;
      TeamId = owner.TeamId;

      Name = custom.Name;
      PokemonType = DataService.GetPokemonType(custom.PokemonTypeId);
      Happiness = custom.Happiness;
      Gender = custom.Gender;
      Lv = custom.Lv;
      AbilityIndex = custom.AbilityIndex;
      Ability = PokemonType.GetAbility(AbilityIndex);
      Nature = custom.Nature;
      Moves = custom.MoveIds.Select((m) => new Move(m, settings)).ToArray();

      Iv = new ReadOnly6D(custom.HpIv, custom.AtkIv, custom.DefIv, custom.SpAtkIv, custom.SpDefIv, custom.SpeedIv);
      Ev = new ReadOnly6D(custom.HpEv, custom.AtkEv, custom.DefEv, custom.SpAtkEv, custom.SpDefEv, custom.SpeedEv);
      Static = new ReadOnly6D(GetState(StatType.Hp), GetState(StatType.Atk), GetState(StatType.Def), GetState(StatType.SpAtk), GetState(StatType.SpDef), GetState(StatType.Speed));
      
      if (custom.ItemId.HasValue) Item = DataService.GetItem(custom.ItemId.Value);
      int h = PokemonStatHelper.GetHp(PokemonType.BaseHp, (byte)Iv.Hp, (byte)Ev.Hp, (byte)Lv);
      hp = new PairValue(h, h, 48);
    }

    public int IndexInOwner
    { get { return Owner.GetPokemonIndex(Id); } }
    public IPairValue Hp
    { get { return hp; } }

    private int GetState(StatType type)
    {
      return PokemonStatHelper.GetStat(type, Nature, PokemonType.GetBaseStat(type), (byte)Iv.GetStat(type), (byte)Ev.GetStat(type), (byte)Lv);
    }
    public void SetHp(int value)
    {
      if (value < 0) value = 0;
      else if (value > Hp.Origin) value = Hp.Origin;
      hp.Value = value;
    }
    public void ClientChangePokemonStateWithNotify(PokemonState value)
    {
      if (State != value)
      {
        State = value;
        if (PropertyChanged != null) PropertyChanged(this, STATE);
      }
    }
  }
}
