﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game.Host
{
  public class ItemE : IItemE
  {
    protected readonly Item Item;

    public ItemE(int id)
    {
      Item = DataService.GetItem(id);
    }
    
    int IItemE.Id
    { get { return Item.Id; } }

    public virtual bool CanLost(PokemonProxy pm) { return true; }
    public virtual int CompareValue(PokemonProxy pm) { return 0; }
    public virtual Modifier ADSModifier(PokemonProxy pm, StatType stat) { return 0x1000; }
    public virtual Modifier DamageFinalModifier(DefContext def) { return 0x1000; }
    public virtual Modifier PowerModifier(AtkContext atk) { return 0x1000; }
    public virtual int CtLvRevise(PokemonProxy pm) { return 0; }

    protected virtual void RaiseImplement(PokemonProxy pm, string key)
    {
#if DEBUG
      if (key == null) System.Diagnostics.Debugger.Break();
#endif
      pm.Controller.ReportBuilder.Add(new GameEvents.UseItem(key, pm, Item.Id));
    }
    public void Raise(PokemonProxy pm, string key) //不必包含虫食、啄食、投掷
    {
      if (pm.Pokemon.Item == Item) //奇妙的单实例道具
      {
        RaiseImplement(pm, key);
        if (Item.Type != ItemType.Normal) pm.ConsumeItem();
      }
    }

    public virtual void Attach(PokemonProxy pm) { }
    public virtual void HpChanged(PokemonProxy pm) { }
    public virtual void StateAdded(PokemonProxy pm, PokemonProxy by, AttachedState state) { }
    public virtual void Attacked(DefContext def) { }
  }
}
