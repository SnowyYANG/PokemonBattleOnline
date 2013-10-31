using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.Game.Host
{
  internal class Pokemon
  {
    public readonly Controller Controller;
    public readonly int Id;
    public readonly Player Owner;
    public readonly int TeamId;

    #region data
    public readonly int MaxHp;
    public readonly int AbilityIndex;
    public readonly I6D Iv;
    public readonly I6D Ev;
    public readonly string Chatter;

    public readonly string Name;
    public readonly PokemonGender Gender;
    public readonly int Lv;
    public readonly Move[] Moves;
    public readonly int Happiness;
    public readonly PokemonNature Nature;
    #endregion

    internal Pokemon(Controller controller, int id, Player owner, IPokemonData custom)
    {
      Controller = controller;
      Id = id;
      Owner = owner;
      TeamId = owner.TeamId;

      Form = custom.Form;
      Name = custom.Name;
      Happiness = custom.Happiness;
      Gender = custom.Gender;
      Lv = custom.Lv;
      Nature = custom.Nature;
      AbilityIndex = custom.AbilityIndex;
      Moves = custom.Moves.Select((m) => new Move(m.Move, m.PP)).ToArray();
      Item = custom.Item;
      Iv = new ReadOnly6D(custom.Iv);
      Ev = new ReadOnly6D(custom.Ev);
      Chatter = custom.Chatter;
      {
        int h = PokemonStatHelper.GetHp(custom.Form.Data.Base.Hp, (byte)Iv.Hp, (byte)Ev.Hp, (byte)Lv);
        MaxHp = h;
        Hp = h;
      }
    }

    public PokemonForm Form; //shaymi
    private int _item;
    public int Item
    {
      get { return _item; }
      set
      {
        if (_item != value)
        {
          _item = value;
          Controller.ReportBuilder.SetItem(this);
        }
      }
    }
    private PokemonState _state;
    public PokemonState State
    {
      get { return _state; }
      set
      {
        if (_state != value)
        {
          _state = value;
          if (value != PokemonState.Faint) Controller.ReportBuilder.SetState(this);
        }
      }
    }
    public bool Shiny
    { get; internal set; }

    public int IndexInOwner
    { get { return Owner.GetPokemonIndex(Id); } }
    /// <summary>
    /// for PokemonProxy only
    /// </summary>
    public int _hp;
    public int Hp
    { 
      get { return _hp; }
      set
      {
        if (value < 0) value = 0;
        else if (value > MaxHp) value = MaxHp;
        if (_hp != value)
        {
          _hp = value;
          Controller.ReportBuilder.SetHp(this);
        }
      }
    }

    private int Get5D(StatType type)
    {
      return PokemonStatHelper.Get5D(type, Nature, Form.Data.Base.GetStat(type), (byte)Iv.GetStat(type), (byte)Ev.GetStat(type), (byte)Lv);
    }

    /// <summary>
    /// battle report delay
    /// </summary>
    public void SetHp(int value)
    {
      if (value < 0) value = 0;
      else if (value > MaxHp) value = MaxHp;
      if (_hp != value) _hp = value;
    }
  }
}
