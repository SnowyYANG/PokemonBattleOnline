using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  public abstract class ItemE : IItemE
  {
    protected readonly Item Item;

    protected ItemE(int id)
    {
      Item = DataService.GetItem(id);
    }
    
    int IItemE.Id
    { get { return Item.Id; } }

    protected void SafeRaise(PokemonProxy pm)
    {
      if (pm.Pokemon.Item == Item) //奇妙的单实例道具
      {
        Raise(pm);
        if (Item.IsOneTime) pm.Pokemon.Item = null;
      }
    }

    public virtual bool CanLost(PokemonProxy pm) { return true; }
    public virtual double Get5DRevise(PokemonProxy pm, StatType stat) { return 1; }

    public virtual void Raise(PokemonProxy pm) //啄食 虫食
    {
    }
    public virtual void Debut(PokemonProxy pm)
    {
    }
    public virtual void HpChanged(PokemonProxy pm) { }
    public virtual void ImplementMove(AtkContext atk) { }
    public virtual void ConditionAdded(PokemonProxy pm, string condition) { }
    public virtual void CalculatingAccuracy(AtkContext atk) { }
    public virtual void CalculatingAccuracy(DefContext def) { }
  }
}
