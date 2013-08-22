using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Data;
using PokemonBattleOnline.Game.Host.Triggers;

namespace PokemonBattleOnline.Game.Host
{
    internal static class Is
    {
        #region ids
        public const int GRISEOUS_ORB = 1;
        public const int ADAMANT_ORB = 2;
        public const int LUSTROUS_ORB = 3;
        public const int BRIGHT_POWDER = 4;
        public const int WHITE_HERB = 5;
        public const int MACHO_BRACE = 6;
        public const int QUICK_CLAW = 7;
        public const int MENTAL_HERB = 8;
        public const int CHOICE_BAND = 9;
        public const int KINGS_ROCK = 10;
        public const int SILVERPOWDER = 11;
        public const int SOUL_DEW = 12;
        public const int DEEPSEATOOTH = 13;
        public const int DEEPSEASCALE = 14;
        public const int FOCUS_BAND = 15;
        public const int SCOPE_LENS = 16;
        public const int METAL_COAT = 17;
        public const int LEFTOVERS = 18;
        public const int LIGHT_BALL = 19;
        public const int SOFT_SAND = 20;
        public const int HARD_STONE = 21;
        public const int MIRACLE_SEED = 22;
        public const int BLACKGLASSES = 23;
        public const int BLACK_BELT = 24;
        public const int MAGNET = 25;
        public const int MYSTIC_WATER = 26;
        public const int SHARP_BEAK = 27;
        public const int POISON_BARB = 28;
        public const int NEVERMELTICE = 29;
        public const int SPELL_TAG = 30;
        public const int TWISTEDSPOON = 31;
        public const int CHARCOAL = 32;
        public const int DRAGON_FANG = 33;
        public const int SILK_SCARF = 34;
        public const int SHELL_BELL = 35;
        public const int SEA_INCENSE = 36;
        public const int LAX_INCENSE = 37;
        public const int LUCKY_PUNCH = 38;
        public const int METAL_POWDER = 39;
        public const int THICK_CLUB = 40;
        public const int STICK = 41;
        public const int WIDE_LENS = 42;
        public const int MUSCLE_BAND = 43;
        public const int WISE_GLASSES = 44;
        public const int EXPERT_BELT = 45;
        public const int LIGHT_CLAY = 46;
        public const int LIFE_ORB = 47;
        public const int POWER_HERB = 48;
        public const int TOXIC_ORB = 49;
        public const int FLAME_ORB = 50;
        public const int QUICK_POWDER = 51;
        public const int FOCUS_SASH = 52;
        public const int ZOOM_LENS = 53;
        public const int METRONOME = 54;
        public const int IRON_BALL = 55;
        public const int LAGGING_TAIL = 56;
        public const int DESTINY_KNOT = 57;
        public const int BLACK_SLUDGE = 58;
        public const int ICY_ROCK = 59;
        public const int SMOOTH_ROCK = 60;
        public const int HEAT_ROCK = 61;
        public const int DAMP_ROCK = 62;
        public const int GRIP_CLAW = 63;
        public const int CHOICE_SCARF = 64;
        public const int STICKY_BARB = 65;
        public const int POWER_BRACER = 66;
        public const int POWER_BELT = 67;
        public const int POWER_LENS = 68;
        public const int POWER_BAND = 69;
        public const int POWER_ANKLET = 70;
        public const int POWER_WEIGHT = 71;
        public const int SHED_SHELL = 72;
        public const int BIG_ROOT = 73;
        public const int CHOICE_SPECS = 74;
        public const int FLAME_PLATE = 75;
        public const int SPLASH_PLATE = 76;
        public const int ZAP_PLATE = 77;
        public const int MEADOW_PLATE = 78;
        public const int ICICLE_PLATE = 79;
        public const int FIST_PLATE = 80;
        public const int TOXIC_PLATE = 81;
        public const int EARTH_PLATE = 82;
        public const int SKY_PLATE = 83;
        public const int MIND_PLATE = 84;
        public const int INSECT_PLATE = 85;
        public const int STONE_PLATE = 86;
        public const int SPOOKY_PLATE = 87;
        public const int DRACO_PLATE = 88;
        public const int DREAD_PLATE = 89;
        public const int IRON_PLATE = 90;
        public const int ODD_INCENSE = 91;
        public const int ROCK_INCENSE = 92;
        public const int FULL_INCENSE = 93;
        public const int WAVE_INCENSE = 94;
        public const int ROSE_INCENSE = 95;
        public const int RAZOR_CLAW = 96;
        public const int RAZOR_FANG = 97;
        public const int DOUSE_DRIVE = 98;
        public const int SHOCK_DRIVE = 99;
        public const int BURN_DRIVE = 100;
        public const int CHILL_DRIVE = 101;
        public const int EVIOLITE = 102;
        public const int FLOAT_STONE = 103;
        public const int ROCKY_HELMET = 104;
        public const int AIR_BALLOON = 105;
        public const int RED_CARD = 106;
        public const int RING_TARGET = 107;
        public const int BINDING_BAND = 108;
        public const int ABSORB_BULB = 109;
        public const int CELL_BATTERY = 110;
        public const int EJECT_BUTTON = 111;
        public const int FIRE_GEM = 112;
        public const int WATER_GEM = 113;
        public const int ELECTRIC_GEM = 114;
        public const int GRASS_GEM = 115;
        public const int ICE_GEM = 116;
        public const int FIGHTING_GEM = 117;
        public const int POISON_GEM = 118;
        public const int GROUND_GEM = 119;
        public const int FLYING_GEM = 120;
        public const int PSYCHIC_GEM = 121;
        public const int BUG_GEM = 122;
        public const int ROCK_GEM = 123;
        public const int GHOST_GEM = 124;
        public const int DRAGON_GEM = 125;
        public const int DARK_GEM = 126;
        public const int STEEL_GEM = 127;
        public const int NORMAL_GEM = 128;
        public const int CHERI_BERRY = 129;
        public const int CHESTO_BERRY = 130;
        public const int PECHA_BERRY = 131;
        public const int RAWST_BERRY = 132;
        public const int ASPEAR_BERRY = 133;
        public const int LEPPA_BERRY = 134;
        public const int ORAN_BERRY = 135;
        public const int PERSIM_BERRY = 136;
        public const int LUM_BERRY = 137;
        public const int SITRUS_BERRY = 138;
        public const int FIGY_BERRY = 139;
        public const int WIKI_BERRY = 140;
        public const int MAGO_BERRY = 141;
        public const int AGUAV_BERRY = 142;
        public const int IAPAPA_BERRY = 143;
        public const int RAZZ_BERRY = 144;
        public const int BLUK_BERRY = 145;
        public const int NANAB_BERRY = 146;
        public const int WEPEAR_BERRY = 147;
        public const int PINAP_BERRY = 148;
        public const int POMEG_BERRY = 149;
        public const int KELPSY_BERRY = 150;
        public const int QUALOT_BERRY = 151;
        public const int HONDEW_BERRY = 152;
        public const int GREPA_BERRY = 153;
        public const int TAMATO_BERRY = 154;
        public const int CORNN_BERRY = 155;
        public const int MAGOST_BERRY = 156;
        public const int RABUTA_BERRY = 157;
        public const int NOMEL_BERRY = 158;
        public const int SPELON_BERRY = 159;
        public const int PAMTRE_BERRY = 160;
        public const int WATMEL_BERRY = 161;
        public const int DURIN_BERRY = 162;
        public const int BELUE_BERRY = 163;
        public const int OCCA_BERRY = 164;
        public const int PASSHO_BERRY = 165;
        public const int WACAN_BERRY = 166;
        public const int RINDO_BERRY = 167;
        public const int YACHE_BERRY = 168;
        public const int CHOPLE_BERRY = 169;
        public const int KEBIA_BERRY = 170;
        public const int SHUCA_BERRY = 171;
        public const int COBA_BERRY = 172;
        public const int PAYAPA_BERRY = 173;
        public const int TANGA_BERRY = 174;
        public const int CHARTI_BERRY = 175;
        public const int KASIB_BERRY = 176;
        public const int HABAN_BERRY = 177;
        public const int COLBUR_BERRY = 178;
        public const int BABIRI_BERRY = 179;
        public const int CHILAN_BERRY = 180;
        public const int LIECHI_BERRY = 181;
        public const int GANLON_BERRY = 182;
        public const int SALAC_BERRY = 183;
        public const int PETAYA_BERRY = 184;
        public const int APICOT_BERRY = 185;
        public const int LANSAT_BERRY = 186;
        public const int STARF_BERRY = 187;
        public const int ENIGMA_BERRY = 188;
        public const int MICLE_BERRY = 189;
        public const int CUSTAP_BERRY = 190;
        public const int JABOCA_BERRY = 191;
        public const int ROWAP_BERRY = 192;
        public const int RSVP_MAIL = 193;
        public const int BERRY_JUICE = 194;
        #endregion

        public static void RaiseItem(this PokemonProxy pm, string key)
        {
            var item = pm.Pokemon.Item;
            if (item != null)
            {
                if (item.Type == ItemType.Normal) pm.AddReportPm(key, item.Id);
                else
                {
                    pm.ConsumeItem();
                    pm.Controller.ReportBuilder.Add(new GameEvents.RemoveItem(key, pm, item.Id));
                }
            }
        }

        public static void ChangeLv5D(PokemonProxy pm, StatType stat, int change)
        {
            change = pm.CanChangeLv7D(pm, stat, change, false);
            if (change == 0) return;
            var i = pm.Pokemon.Item.Id;
            string log;
            switch (change)
            {
                case 1:
                    log = "Item7DUp1";
                    break;
                case 2:
                    log = "Item7DUp2";
                    break;
                case -1:
                    log = "7DDown1";
                    break;
                case -2:
                    log = "7DDown2";
                    break;
                default:
                    log = change > 0 ? "Item7DUp3" : "7DDown3";
                    break;
            }
            pm.OnboardPokemon.ChangeLv7D(stat, change);
            pm.ConsumeItem();
            pm.Controller.ReportBuilder.Add(new GameEvents.RemoveItem(log, pm, stat, change > 0 ? i : 0));
        }

        public static bool ChoiceItem(int item)
        {
            return item == CHOICE_BAND || item == CHOICE_SCARF || item == CHOICE_SPECS;
        }

        /// <summary>
        /// pm.Item should not be null
        /// </summary>
        /// <param name="pm"></param>
        /// <returns></returns>
        public static bool NeverLostItem(Pokemon pm)
        {
            var i = pm.Item.Id;
            return
              i == RSVP_MAIL ||
              pm.Form.Type.Number == 487 && i == GRISEOUS_ORB || //giratina
              PlatedArceus(pm) ||
              pm.Form.Type.Number == 649 && DOUSE_DRIVE <= i && i <= CHILL_DRIVE; //genesect
        }
        public static bool CanLostItem(PokemonProxy pm)
        {
            Item i = pm.Pokemon.Item;
            return !
              (
              i == null ||
              NeverLostItem(pm.Pokemon) ||
              pm.Ability == As.STICKY_HOLD
              );
        }
        public static bool CanUseItem(PokemonProxy pm)
        { return !(pm.OnboardPokemon.HasCondition("Embargo") || pm.Controller.Board.HasCondition("MagicRoom") || pm.Ability == As.KLUTZ); }
        public static bool PlatedArceus(Pokemon pm)
        {
            return pm.Item != null && pm.Form.Type.Number == 493 && FLAME_PLATE <= pm.Item.Id && pm.Item.Id <= IRON_PLATE;
        }
        public static bool Berry(int id)
        {
            return BerryNumber(id) != 0;
        }
        public static int BerryNumber(int id)
        {
            return 129 <= id && id <= 192 ? id - 128 : 0;
        }
        public static int BerryNumberToItemId(int number)
        {
            return 0 < number && number <= 64 ? 128 + number : 0;
        }
        public static bool Gem(int id)
        {
            return 111 < id && id < 129;
        }
        public static StatType GetTaste(int berry)
        {
            switch (berry)
            {
                case 11:
                    return StatType.Atk;
                case 12:
                    return StatType.SpAtk;
                case 13:
                    return StatType.Speed;
                case 14:
                    return StatType.SpDef;
                case 15:
                    return StatType.Def;
            }
            return StatType.Invalid;
        }
        public static StatType GetTaste(Item item)
        {
            return GetTaste(BerryNumber(item.Id));
        }
        public static void RaiseItemByMove(PokemonProxy pm, int id, PokemonProxy by)
        {
            var op = pm.OnboardPokemon;
            switch (id)
            {
                case Is.WHITE_HERB: //5
                    {
                        bool raise = false;
                        var lvs = (Simple6D)op.Lv5D;
                        if (lvs.Atk < 0) { lvs.Atk = 0; raise = true; }
                        if (lvs.Def < 0) { lvs.Def = 0; raise = true; }
                        if (lvs.SpAtk < 0) { lvs.SpAtk = 0; raise = true; }
                        if (lvs.SpDef < 0) { lvs.SpDef = 0; raise = true; }
                        if (lvs.Speed < 0) { lvs.Speed = 0; raise = true; }
                        if (op.AccuracyLv < 0) { op.AccuracyLv = 0; raise = true; }
                        if (op.EvasionLv < 0) { op.EvasionLv = 0; raise = true; }
                        if (raise) pm.AddReportPm("WhiteHerb");
                    }
                    break;
                case 8:
                    if (op.RemoveCondition("Attract")) pm.AddReportPm("ItemDeAttract", 8);
                    break;
                case 10:
                    op.AddTurnCondition("Flinch");
                    break;
                case 19:
                    pm.AddState(by, AttachedState.PAR, false);
                    break;
                case 28:
                    pm.AddState(by, AttachedState.PSN, false);
                    break;
                case 49:
                    pm.AddState(by, AttachedState.PSN, false, 15);//战报待验证
                    break;
                case 50:
                    pm.AddState(by, AttachedState.BRN, false);//战报
                    break;
                case 97:
                    op.AddTurnCondition("Flinch");
                    break;
                case 129:
                    if (pm.State == PokemonState.PAR) pm.DeAbnormalState();//战报
                    break;
                case 130:
                    if (pm.State == PokemonState.PAR) pm.DeAbnormalState();
                    break;
                case 131:
                    if (pm.State == PokemonState.PAR) pm.DeAbnormalState();
                    break;
                case 132:
                    if (pm.State == PokemonState.BRN) pm.DeAbnormalState();
                    break;
                case 133:
                    if (pm.State == PokemonState.FRZ) pm.DeAbnormalState();
                    break;
                case 134:
                    foreach (var m in pm.Moves)
                        if (m.PP == 0)
                        {
                            m.PP += 10;
                            pm.Controller.ReportBuilder.Add(new GameEvents.PPChange("ItemPPRecover", m) { Arg2 = 134 });
                            return;
                        }
                    foreach (var m in pm.Moves)
                        if (m.PP != m.Move.PP.Origin)
                        {
                            m.PP += 10;
                            pm.Controller.ReportBuilder.Add(new GameEvents.PPChange("ItemPPRecover", m) { Arg2 = 134 });
                            return;
                        }
                    break;
                case 135:
                    pm.HpRecover(10, false, "ItemHpRecover", 135);
                    break;
                case 136:
                    if (op.RemoveCondition("Confuse")) pm.AddReportPm("DeConfuse");
                    break;
                case 137:
                    if (pm.State != PokemonState.Normal) pm.DeAbnormalState();
                    break;
                case 138:
                    pm.HpRecoverByOneNth(3, false, "ItemHpRecover", 138);
                    break;
                case 139:
                case 140:
                case 141:
                case 142:
                case 143:
                    pm.HpRecoverByOneNth(8, false, "ItemRecover", id);
                    if (pm.Pokemon.Nature.DislikeTaste(GetTaste(BerryNumber(id)))) pm.AddState(pm, AttachedState.Confuse, false);
                    break;
                case 181:
                    pm.ChangeLv7D(by, StatType.Atk, 1, false);
                    break;
                case 182:
                    pm.ChangeLv7D(by, StatType.Def, 1, false);
                    break;
                case 183:
                    pm.ChangeLv7D(by, StatType.Speed, 1, false);
                    break;
                case 184:
                    pm.ChangeLv7D(by, StatType.SpAtk, 1, false);
                    break;
                case 185:
                    pm.ChangeLv7D(by, StatType.SpDef, 1, false);
                    break;
                case 186:
                    //会心果，号称进入蓄气状态
                    break;
                case STARF_BERRY:
                    {
                        var ss = from StatType stat in StatTypeHelper.Type5D
                                 where pm.CanChangeLv7D(@by, stat, 2, false) != 0
                                 select stat;
                        int n = ss.Count();
                        if (n != 0) pm.ChangeLv7D(by, ss.ElementAt(pm.Controller.GetRandomInt(0, n - 1)), 2, false);
                    }
                    break;
                case 189:
                    //神秘果，某种意义上提高命中
                    //测试，投掷后啄食或投掷后咬1/4血
                    break;
            }
        }

        public static void WhiteHerb(PokemonProxy pm)
        {
            if (pm.Item == WHITE_HERB)
            {
                Simple6D lvs = (Simple6D)pm.OnboardPokemon.Lv5D;
                bool raise = false;
                if (lvs.Atk < 0) { lvs.Atk = 0; raise = true; }
                if (lvs.Def < 0) { lvs.Def = 0; raise = true; }
                if (lvs.SpAtk < 0) { lvs.SpAtk = 0; raise = true; }
                if (lvs.SpDef < 0) { lvs.SpDef = 0; raise = true; }
                if (lvs.Speed < 0) { lvs.Speed = 0; raise = true; }
                if (pm.OnboardPokemon.AccuracyLv < 0) { pm.OnboardPokemon.AccuracyLv = 0; raise = true; }
                if (pm.OnboardPokemon.EvasionLv < 0) { pm.OnboardPokemon.EvasionLv = 0; raise = true; }
                if (raise) RaiseItem(pm, "WhiteHerb");
            }
        }
        public static bool AirBalloon(PokemonProxy pm) //气球的提示信息不是Attach而是Debut，是唯一会Debut的道具
        {
            if (pm.Item == AIR_BALLOON) //batonpass embargo
            {
                pm.AddReportPm("EnBalloon");
                return true;
            }
            return false;
        }
        public static void AirBalloon(DefContext def)
        {
            def.Defender.RemoveItem();
            def.Defender.AddReportPm("DeBalloon");
        }
        public static void AttackPostEffect(AtkContext atk)
        {
            var aer = atk.Attacker;
            var c = aer.Controller;
            if (!atk.IgnoreSwitchItem)
            {
                bool e = true, r = MoveE.CanForceSwitch(aer, true);
                foreach (var d in atk.Targets.Where((d) => !d.HitSubstitute && d.Defender.Tile != null).OrderBy((d) => d.Defender.Speed).ToArray())
                {
                    var der = d.Defender;
                    var i = der.Item;
                    if (e && i == EJECT_BUTTON)
                    {
                        atk.SetCondition("EjectButton", der.Tile);
                        der.ConsumeItem();
                        c.ReportBuilder.Add(new GameEvents.RemoveItem(null, der));
                        c.Withdraw(der, "EjectButton");
                        if (r == false) break;
                        e = false;
                    }
                    else if (r && i == RED_CARD)
                    {
                        der.ConsumeItem();
                        c.ReportBuilder.Add(new GameEvents.RemoveItem("RedCard", der, aer.Id));
                        MoveE.ForceSwitchImplement(aer, null);
                        if (e == false) return;
                        r = false;
                    }
                }
            }
            if (aer.Item == SHELL_BELL)
            {
                if (atk.TotalDamage != 0)
                    aer.HpRecoverByOneNth(atk.TotalDamage >> 3, false, "ItemRecover", SHELL_BELL);
            }
            else if (aer.Item == LIFE_ORB)
            {
                aer.EffectHurtByOneNth(10, "LifeOrb");
                aer.CheckFaint();
            }
        }
        public static bool CanAttackFlinch(DefContext def)
        {
            int item = def.AtkContext.Attacker.Item;
            return (item == KINGS_ROCK || item == RAZOR_FANG) && def.Defender.Controller.RandomHappen(10);
        }
        public static bool PowerHerb(PokemonProxy pm)
        {
            if (pm.Item == POWER_HERB)
            {
                RaiseItem(pm, "PowerHerb");
                pm.OnboardPokemon.CoordY = CoordY.Plate;
                return true;
            }
            return false;
        }
        public static double FloatStone(PokemonProxy pm)
        {
            if (pm.Item == FLOAT_STONE) return 0.5d;
            return 1d;
        }
        public static bool MicleBerry(AtkContext atk)
        {
            PokemonProxy pm = atk.Attacker;
            if (pm.Item == MICLE_BERRY && As.Gluttony(pm))
            {
                RaiseItem(pm, "MicleBerry");
                return true;
            }
            return false;
        }
        public static void CheckGem(AtkContext atk)
        {
            var i = atk.Attacker.Item;
            if (Gem(i) && BattleTypeHelper.GetItemType(i, 112, false) == atk.Type)
            {
                atk.SetTurnCondition("Gem");
                atk.Controller.ReportBuilder.Add(new GameEvents.RemoveItem("Gem", atk.Attacker, i, atk.Move.Id));
                atk.Attacker.ConsumeItem();
            }
        }
        public static void DestinyKnot(PokemonProxy pm, PokemonProxy by)
        {
            if (pm.Item == Is.DESTINY_KNOT) by.AddState(pm, AttachedState.Attract, false, 0, "ItemEnAttract", Is.DESTINY_KNOT);
        }
    }
}
