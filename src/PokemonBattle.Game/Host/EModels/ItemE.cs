using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  public class ItemE : IItemE
  {
    protected readonly Item Item;
    protected readonly string LogKey;

    public ItemE(int id, string logKey = null)
    {
      Item = DataService.GetItem(id);
    }
    
    int IItemE.Id
    { get { return Item.Id; } }

    public virtual bool CanLost(PokemonProxy pm) { return true; }
    public virtual int CompareValue(PokemonProxy pm) { return 0; }
    public virtual double Get5DRevise(PokemonProxy pm, StatType stat) { return 1; }
    public virtual int GetCtLvRevise(PokemonProxy pm) { return 0; }

    protected virtual void RaiseImpl(PokemonProxy pm)
    {
      if (LogKey != null)
        pm.Controller.ReportBuilder.Add(new Interactive.GameEvents.UseItem(LogKey, pm, Item));
    }
    public void Raise(PokemonProxy pm) //不必包含虫食、啄食、投掷
    {
      if (pm.Pokemon.Item == Item) //奇妙的单实例道具
      {
        RaiseImpl(pm);
        if (Item.Type != ItemType.Normal) pm.ConsumeItem();
      }
    }

    public virtual void Attach(PokemonProxy pm) { }
    public virtual void HpChanged(PokemonProxy pm) { }
    public virtual void StateAdded(PokemonProxy pm, AttachedState state) { }
    public virtual void CalculatingAccuracy(AtkContext atk) { }
    public virtual void CalculatingAccuracy(DefContext def) { }
    public virtual void CalculatingPower(AtkContext atk) { }
    public virtual void ReviseDamage2(AtkContext atk) { }
    public virtual void ReviseDamage3(DefContext def) { }
    public virtual void Attacked(DefContext def) { }
  }
}
