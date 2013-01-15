using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PokemonBattleOnline.Data;

namespace PokemonBattleOnline.Game.Host
{
  public class Pokemon
  {
    public readonly int Id;
    public readonly Player Owner;
    public readonly int TeamId;

    #region data
    internal readonly int AbilityIndex;
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

    internal Pokemon(int id, Player owner, IPokemonData custom)
    {
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
      Item = GameDataService.GetItem(custom.ItemId);
      Iv = new ReadOnly6D(custom.Iv);
      Ev = new ReadOnly6D(custom.Ev);
      Chatter = custom.Chatter;
      {
        int h = PokemonStatHelper.GetHp(custom.Form.Data.Base.Hp, (byte)Iv.Hp, (byte)Ev.Hp, (byte)Lv);
        _hp = new PairValue(h);
      }
    }

    public PokemonForm Form; //shaymi
    public Item Item;
    public PokemonState State;
    public bool Shiny
    { get; internal set; }

    public int IndexInOwner
    { get { return Owner.GetPokemonIndex(Id); } }
    private PairValue _hp;
    public IPairValue Hp
    { get { return _hp; } }

    private int Get5D(StatType type)
    {
      return PokemonStatHelper.Get5D(type, Nature, Form.Data.Base.GetStat(type), (byte)Iv.GetStat(type), (byte)Ev.GetStat(type), (byte)Lv);
    }
    public void SetHp(int value)
    {
      if (value < 0) value = 0;
      else if (value > Hp.Origin) value = Hp.Origin;
      _hp.Value = value;
    }
  }
}
