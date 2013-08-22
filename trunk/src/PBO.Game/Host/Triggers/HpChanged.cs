using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Data;

namespace PokemonBattleOnline.Game.Host.Triggers
{
    internal static class HpChanged
    {
        public static void Execute(PokemonProxy pm)
        {
            switch (pm.Item)
            {
                case Is.ORAN_BERRY: //135
                    RecoverBerry(pm, 10);
                    break;
                case Is.BERRY_JUICE: //194
                    RecoverBerry(pm, 20);
                    break;
                case Is.SITRUS_BERRY: //138
                    if (pm.Hp << 1 < pm.Pokemon.Hp.Origin) pm.HpRecoverByOneNth(4, false, "ItemRecover", 138, true);
                    break;
                case 139:
                case 140:
                case 141:
                case 142:
                case 143:
                    TastyBerry(pm);
                    break;
                case Is.LIECHI_BERRY: //181
                    Up1Berry(pm, StatType.Atk);
                    break;
                case Is.GANLON_BERRY:
                    Up1Berry(pm, StatType.Def);
                    break;
                case Is.SALAC_BERRY:
                    Up1Berry(pm, StatType.Speed);
                    break;
                case Is.PETAYA_BERRY:
                    Up1Berry(pm, StatType.SpAtk);
                    break;
                case Is.APICOT_BERRY: //185
                    Up1Berry(pm, StatType.SpDef);
                    break;
                case Is.STARF_BERRY: //187
                    StarfBerry(pm);
                    break;
                case Is.LANSAT_BERRY: //186
                    if (As.Gluttony(pm) && pm.OnboardPokemon.AddCondition("FocusEnergy"))
                    {
                        pm.Controller.ReportBuilder.Add(new GameEvents.RemoveItem("ItemEnFocusEnergy", pm, 186));
                        pm.ConsumeItem();
                    }
                    break;
            }
        }
        private static void Up1Berry(PokemonProxy pm, StatType stat)
        {
            if (As.Gluttony(pm)) Is.ChangeLv5D(pm, stat, 1);
        }
        private static void StarfBerry(PokemonProxy pm)
        {
            if (As.Gluttony(pm))
            {
                var ss = from StatType stat in StatTypeHelper.Type5D
                         where pm.CanChangeLv7D(pm, stat, 2, false) != 0
                         select stat;
                int n = ss.Count();
                if (n != 0) Is.ChangeLv5D(pm, ss.ElementAt(pm.Controller.GetRandomInt(0, n - 1)), 2);
            }
        }
        private static void RecoverBerry(PokemonProxy pm, int hp)
        {
            if (pm.Hp << 1 <= pm.Pokemon.Hp.Origin) pm.HpRecover(hp, false, "ItemRecover", pm.Pokemon.Item.Id, true);
        }
        private static void TastyBerry(PokemonProxy pm)
        {
            if (As.Gluttony(pm) && pm.CanHpRecover(false))
            {
                pm.HpRecoverByOneNth(8, false, "ItemRecover", pm.Pokemon.Item.Id, true);
                if (pm.Pokemon.Nature.DislikeTaste(Is.GetTaste(pm.Pokemon.Item))) pm.AddState(pm, AttachedState.Confuse, false);
            }
        }
    }
}
