using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Data;

namespace PokemonBattleOnline.Game.Host
{
  public class ItemE
  {
    protected readonly Item Item;

    public ItemE(int id)
    {
      Id = id;
      Item = GameDataService.GetItem(id);
    }

    public int Id
    { get; private set; }

    public virtual int CompareValue(PokemonProxy pm) { return 0; }
    public virtual Modifier SModifier(PokemonProxy pm) { return 0x1000; }
    public virtual Modifier AModifier(AtkContext atk) { return 0x1000; }
    public virtual Modifier DModifier(DefContext def) { return 0x1000; }
    public virtual Modifier DamageFinalModifier(DefContext def) { return 0x1000; }
    public virtual Modifier PowerModifier(AtkContext atk) { return 0x1000; }
    public virtual int CtLvRevise(PokemonProxy pm) { return 0; }

    protected virtual void RaiseImplement(PokemonProxy pm, string key)
    {
#if DEBUG
      if (key == null) System.Diagnostics.Debugger.Break();
#endif
      if (Item.Type == ItemType.Normal) pm.AddReportPm(key, Item.Id);
      else
      {
        pm.ConsumeItem();
        pm.Controller.ReportBuilder.Add(new GameEvents.RemoveItem(key, pm, Item.Id));
      }
    }
    public void Raise(PokemonProxy pm, string key) //不必包含虫食、啄食、投掷
    {
      if (pm.Pokemon.Item == Item) RaiseImplement(pm, key);
    }

    public virtual void Attach(PokemonProxy pm) { }
    public virtual void HpChanged(PokemonProxy pm) { }
    /// <summary>
    /// 红线、柿果、精神香草、5树果
    /// </summary>
    public virtual void StateAdded(PokemonProxy pm, PokemonProxy by, AttachedState state) { }
    public virtual void Attacked(DefContext def) { }
  }
}
