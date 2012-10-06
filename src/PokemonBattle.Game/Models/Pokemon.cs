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
    public readonly I6D Iv;
    public readonly I6D Ev;

    public string Name
    { get; private set; }
    public PokemonForm Form
    { get; private set; }
    public PokemonGender Gender
    { get; private set; }
    public int Lv
    { get; private set; }
    public Ability Ability
    { get { return Form.Data.GetAbility(AbilityIndex); } }
    public Move[] Moves
    { get; private set; }
    public int Happiness
    { get; private set; }
    public PokemonNature Nature
    { get; private set; }
    /// <summary>
    /// for binding only
    /// </summary>
    public ReadOnly6D FiveD
    { get; private set; }

    private PairValue hp;
    public Item Item
    { get; set; }
    public PokemonState State
    { get; set; }
    #endregion

    internal Pokemon(Player owner, PokemonCustomInfo custom, IGameSettings settings, Func<int> nextId)
    {
      Id = nextId();
      Owner = owner;
      TeamId = owner.TeamId;

      Name = custom.Name;
      Happiness = custom.Happiness;
      Gender = custom.Gender;
      Lv = custom.Lv;
      Nature = custom.Nature;
      AbilityIndex = custom.AbilityIndex;
      Moves = custom.MoveIds.Select((m) => new Move(m, settings)).ToArray();
      Item = DataService.GetItem(custom.ItemId);
      Iv = new ReadOnly6D(custom.Iv);
      Ev = new ReadOnly6D(custom.Ev);
      {
        int h = PokemonStatHelper.GetHp(Form.Data.Base.Hp, (byte)Iv.Hp, (byte)Ev.Hp, (byte)Lv);
        hp = new PairValue(h, h, 48);
      }

      ChangeForm(custom.Form);
    }

    public int IndexInOwner
    { get { return Owner.GetPokemonIndex(Id); } }
    public IPairValue Hp
    { get { return hp; } }

    private int Get5D(StatType type)
    {
      return PokemonStatHelper.Get5D(type, Nature, Form.Data.Base.GetStat(type), (byte)Iv.GetStat(type), (byte)Ev.GetStat(type), (byte)Lv);
    }
    private void ChangeForm(PokemonForm form)
    {
      Form = form;
      FiveD = new ReadOnly6D(0, Get5D(StatType.Atk), Get5D(StatType.Def), Get5D(StatType.SpAtk), Get5D(StatType.SpDef), Get5D(StatType.Speed));
    }
    public void SetHp(int value)
    {
      if (value < 0) value = 0;
      else if (value > Hp.Origin) value = Hp.Origin;
      hp.Value = value;
    }
    public void ChangeForm(int form)
    {
      ChangeForm(Form.Type.GetForm(form));
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
