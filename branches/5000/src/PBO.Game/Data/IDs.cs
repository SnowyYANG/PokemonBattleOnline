using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Game
{
    public static class As
    {
        /// <summary>
        /// 恶臭 -> 降低野生神奇宝贝出现几率。
        /// </summary>
        public const int STENCH = 1;
        /// <summary>
        /// 毛毛雨 -> 神奇宝贝出场时下雨。
        /// </summary>
        public const int DRIZZLE = 2;
        /// <summary>
        /// 加速 -> 神奇宝贝速度在对战中逐渐提升。
        /// </summary>
        public const int SPEED_BOOST = 3;
        /// <summary>
        /// 甲虫盔甲 -> 免受击中要害的攻击。
        /// </summary>
        public const int BATTLE_ARMOR = 4;
        /// <summary>
        /// 结实 -> 不会受到一击必杀类技能攻击。
        /// </summary>
        public const int STURDY = 5;
        /// <summary>
        /// 湿气 -> 阻止发动自爆类型技能。
        /// </summary>
        public const int DAMP = 6;
        /// <summary>
        /// 柔软 -> 不会陷入麻痹状态。
        /// </summary>
        public const int LIMBER = 7;
        /// <summary>
        /// 沙里隐身 -> 在沙雹天气中回避率提升。
        /// </summary>
        public const int SAND_VEIL = 8;
        /// <summary>
        /// 静电 -> 受到近身攻击时对手可能陷入麻痹状态。
        /// </summary>
        public const int STATIC = 9;
        /// <summary>
        /// 蓄电 -> 受到电系技能攻击时体力恢复。
        /// </summary>
        public const int VOLT_ABSORB = 10;
        /// <summary>
        /// 储水 -> 受到水系技能攻击时体力恢复。
        /// </summary>
        public const int WATER_ABSORB = 11;
        /// <summary>
        /// 迟钝 -> 不会陷入颓废状态。
        /// </summary>
        public const int OBLIVIOUS = 12;
        /// <summary>
        /// 乐天 -> 不受天气的影响。
        /// </summary>
        public const int CLOUD_NINE = 13;
        /// <summary>
        /// 复眼 -> 命中率提升。
        /// </summary>
        public const int COMPOUNDEYES = 14;
        /// <summary>
        /// 失眠 -> 不会陷入睡眠状态。
        /// </summary>
        public const int INSOMNIA = 15;
        /// <summary>
        /// 变色 -> 根据对手技能改变神奇宝贝属性。
        /// </summary>
        public const int COLOR_CHANGE = 16;
        /// <summary>
        /// 免疫 -> 不会陷入中毒状态。
        /// </summary>
        public const int IMMUNITY = 17;
        /// <summary>
        /// 延烧 -> 受到炎系技能攻击时会提升自身炎系技能力量。
        /// </summary>
        public const int FLASH_FIRE = 18;
        /// <summary>
        /// 鳞粉 -> 不会受到对方技能的追加效果影响。
        /// </summary>
        public const int SHIELD_DUST = 19;
        /// <summary>
        /// 我行我素 -> 不会陷入混乱状态。
        /// </summary>
        public const int OWN_TEMPO = 20;
        /// <summary>
        /// 吸盘 -> 不会受到强制交换神奇宝贝技能的影响。
        /// </summary>
        public const int SUCTION_CUPS = 21;
        /// <summary>
        /// 威吓 -> 降低对手攻击。
        /// </summary>
        public const int INTIMIDATE = 22;
        /// <summary>
        /// 影子游戏 -> 阻止对手逃走。
        /// </summary>
        public const int SHADOW_TAG = 23;
        /// <summary>
        /// 蛇皮 -> 受到近身攻击时会伤害对手。
        /// </summary>
        public const int ROUGH_SKIN = 24;
        /// <summary>
        /// 奇异守护 -> 只会受到效果拔群技能的攻击。
        /// </summary>
        public const int WONDER_GUARD = 25;
        /// <summary>
        /// 漂浮 -> 不会受到地上系技能攻击。
        /// </summary>
        public const int LEVITATE = 26;
        /// <summary>
        /// 孢子 -> 受到近身攻击可能麻痹、中毒或睡眠。
        /// </summary>
        public const int EFFECT_SPORE = 27;
        /// <summary>
        /// 同步 -> 灼伤、中毒、麻痹时也会影响对手。
        /// </summary>
        public const int SYNCHRONIZE = 28;
        /// <summary>
        /// 透明体 -> 自身能力值不会降低。
        /// </summary>
        public const int CLEAR_BODY = 29;
        /// <summary>
        /// 自然恢复 -> 在收回后自动解除异常状态。
        /// </summary>
        public const int NATURAL_CURE = 30;
        /// <summary>
        /// 避雷针 -> 吸收所有电系技能攻击。
        /// </summary>
        public const int LIGHTNINGROD = 31;
        /// <summary>
        /// 天恩 -> 使用技能的追加效果出现率提升。
        /// </summary>
        public const int SERENE_GRACE = 32;
        /// <summary>
        /// 轻快 -> 下雨时速度提升。
        /// </summary>
        public const int SWIFT_SWIM = 33;
        /// <summary>
        /// 叶绿素 -> 阳光下速度提升。
        /// </summary>
        public const int CHLOROPHYLL = 34;
        /// <summary>
        /// 发光 -> 更容易遇到野生神奇宝贝。
        /// </summary>
        public const int ILLUMINATE = 35;
        /// <summary>
        /// 追踪 -> 复制对手的特性。
        /// </summary>
        public const int TRACE = 36;
        /// <summary>
        /// 大力士 -> 攻击提升。
        /// </summary>
        public const int HUGE_POWER = 37;
        /// <summary>
        /// 毒刺 -> 受到近身攻击时对手可能中毒。
        /// </summary>
        public const int POISON_POINT = 38;
        /// <summary>
        /// 精神力 -> 不会陷入退缩状态。
        /// </summary>
        public const int INNER_FOCUS = 39;
        /// <summary>
        /// 岩浆防护 -> 不会陷入冰冻状态。
        /// </summary>
        public const int MAGMA_ARMOR = 40;
        /// <summary>
        /// 水幕 -> 不会陷入灼伤状态。
        /// </summary>
        public const int WATER_VEIL = 41;
        /// <summary>
        /// 磁力 -> 钢系神奇宝贝无法逃脱。
        /// </summary>
        public const int MAGNET_PULL = 42;
        /// <summary>
        /// 隔音 -> 不受声音类技能的影响。
        /// </summary>
        public const int SOUNDPROOF = 43;
        /// <summary>
        /// 雨盘 -> 在下雨天气中恢复体力。
        /// </summary>
        public const int RAIN_DISH = 44;
        /// <summary>
        /// 沙流 -> 神奇宝贝出场时产生沙雹天气。
        /// </summary>
        public const int SAND_STREAM = 45;
        /// <summary>
        /// 气压 -> 增加对手PP的消耗。
        /// </summary>
        public const int PRESSURE = 46;
        /// <summary>
        /// 肥脂 -> 减弱冰系或炎系技能的伤害。
        /// </summary>
        public const int THICK_FAT = 47;
        /// <summary>
        /// 早起 -> 睡眠时可以较快苏醒。
        /// </summary>
        public const int EARLY_BIRD = 48;
        /// <summary>
        /// 火焰体 -> 受到近身攻击时对手可能灼伤。
        /// </summary>
        public const int FLAME_BODY = 49;
        /// <summary>
        /// 逃走 -> 在与野生神奇宝贝对战时确定可以逃走。
        /// </summary>
        public const int RUN_AWAY = 50;
        /// <summary>
        /// 锐利目光 -> 命中率不会降低。
        /// </summary>
        public const int KEEN_EYE = 51;
        /// <summary>
        /// 怪力剪刀 -> 攻击不会下降。
        /// </summary>
        public const int HYPER_CUTTER = 52;
        /// <summary>
        /// 捡拾 -> 对战结束后可能捡拾到道具。
        /// </summary>
        public const int PICKUP = 53;
        /// <summary>
        /// 懒惰 -> 不能连续发动技能。
        /// </summary>
        public const int TRUANT = 54;
        /// <summary>
        /// 活力 -> 攻击提升，命中率下降。
        /// </summary>
        public const int HUSTLE = 55;
        /// <summary>
        /// 迷人身躯 -> 受到近身攻击时异性对手可能陷入颓废状态。
        /// </summary>
        public const int CUTE_CHARM = 56;
        /// <summary>
        /// 阳性 -> 己方同伴有阴性特性时特攻提升。
        /// </summary>
        public const int PLUS = 57;
        /// <summary>
        /// 阴性 -> 己方同伴有阳性特性时特攻提升。
        /// </summary>
        public const int MINUS = 58;
        /// <summary>
        /// 天气预报 -> 受到天气影响改变型态。
        /// </summary>
        public const int FORECAST = 59;
        /// <summary>
        /// 黏着 -> 不会被对手夺去携带道具。
        /// </summary>
        public const int STICKY_HOLD = 60;
        /// <summary>
        /// 脱皮 -> 对战时有一定几率治愈自身异常状态。
        /// </summary>
        public const int SHED_SKIN = 61;
        /// <summary>
        /// 毅力 -> 状态异常时攻击提升。
        /// </summary>
        public const int GUTS = 62;
        /// <summary>
        /// 神奇鳞片 -> 状态异常时防御提升。
        /// </summary>
        public const int MARVEL_SCALE = 63;
        /// <summary>
        /// 黏液 -> 受到吸收类技能攻击时会伤害对手。
        /// </summary>
        public const int LIQUID_OOZE = 64;
        /// <summary>
        /// 茂盛 -> 在体力不支时草系技能力量增强。
        /// </summary>
        public const int OVERGROW = 65;
        /// <summary>
        /// 猛火 -> 在体力不支时炎系技能力量增强。
        /// </summary>
        public const int BLAZE = 66;
        /// <summary>
        /// 激流 -> 在体力不支时水系技能力量增强。
        /// </summary>
        public const int TORRENT = 67;
        /// <summary>
        /// 预感 -> 在体力不支时虫系技能力量增强。
        /// </summary>
        public const int SWARM = 68;
        /// <summary>
        /// 石头 -> 不会受到反弹伤害。
        /// </summary>
        public const int ROCK_HEAD = 69;
        /// <summary>
        /// 旱灾 -> 神奇宝贝出场时转为阳光。
        /// </summary>
        public const int DROUGHT = 70;
        /// <summary>
        /// 狮蚁 -> 使对手无法逃跑。
        /// </summary>
        public const int ARENA_TRAP = 71;
        /// <summary>
        /// 元气 -> 不会陷入睡眠状态。
        /// </summary>
        public const int VITAL_SPIRIT = 72;
        /// <summary>
        /// 白色烟雾 -> 自身能力值不会降低。
        /// </summary>
        public const int WHITE_SMOKE = 73;
        /// <summary>
        /// 瑜珈神力 -> 提升物理攻击技能力量。
        /// </summary>
        public const int PURE_POWER = 74;
        /// <summary>
        /// 贝壳盔甲 -> 受到攻击时不会击中要害。
        /// </summary>
        public const int SHELL_ARMOR = 75;
        /// <summary>
        /// 气闸 -> 不受天气的影响。
        /// </summary>
        public const int AIR_LOCK = 76;
        /// <summary>
        /// 蹒跚 -> 混乱状态时逃避率提升。
        /// </summary>
        public const int TANGLED_FEET = 77;
        /// <summary>
        /// 电气引擎 -> 受到电系技能攻击时速度提升。
        /// </summary>
        public const int MOTOR_DRIVE = 78;
        /// <summary>
        /// 斗争心 -> 对手具有相同性别时攻击提升，对手具有相异性别时攻击下降。
        /// </summary>
        public const int RIVALRY = 79;
        /// <summary>
        /// 不屈之心 -> 在陷入退缩状态时速度提升。
        /// </summary>
        public const int STEADFAST = 80;
        /// <summary>
        /// 雪里隐身 -> 在冰雹天气中回避率提升。
        /// </summary>
        public const int SNOW_CLOAK = 81;
        /// <summary>
        /// 贪吃鬼 -> 携带的树果会较早被吃掉。
        /// </summary>
        public const int GLUTTONY = 82;
        /// <summary>
        /// 愤怒穴道 -> 击中要害时攻击提升。
        /// </summary>
        public const int ANGER_POINT = 83;
        /// <summary>
        /// 杂技 -> 携带道具被使用时速度提升。
        /// </summary>
        public const int UNBURDEN = 84;
        /// <summary>
        /// 耐热 -> 减弱受到的炎系技能威力。
        /// </summary>
        public const int HEATPROOF = 85;
        /// <summary>
        /// 单纯 -> 使用能力变化的技能时效果增强。
        /// </summary>
        public const int SIMPLE = 86;
        /// <summary>
        /// 干燥皮肤 -> 炎热时体力消耗较快，接触到水时恢复体力。
        /// </summary>
        public const int DRY_SKIN = 87;
        /// <summary>
        /// 下载 -> 根据对手较强防御类型提升相应攻击类型的力量。
        /// </summary>
        public const int DOWNLOAD = 88;
        /// <summary>
        /// 铁拳 -> 拳击类技能力量提升。
        /// </summary>
        public const int IRON_FIST = 89;
        /// <summary>
        /// 毒疗 -> 中毒状态下恢复体力。
        /// </summary>
        public const int POISON_HEAL = 90;
        /// <summary>
        /// 适应力 -> 增强同属性技能的力量。
        /// </summary>
        public const int ADAPTABILITY = 91;
        /// <summary>
        /// 连续攻击 -> 使用重复发动的技能时次数增多。
        /// </summary>
        public const int SKILL_LINK = 92;
        /// <summary>
        /// 湿润身体 -> 下雨时状态异常消除。
        /// </summary>
        public const int HYDRATION = 93;
        /// <summary>
        /// 太阳能量 -> 阳光天气时特攻提升体力下降。
        /// </summary>
        public const int SOLAR_POWER = 94;
        /// <summary>
        /// 飞毛腿 -> 状态异常时速度提升。
        /// </summary>
        public const int QUICK_FEET = 95;
        /// <summary>
        /// 一般皮肤 -> 自身使用技能均变为一般系。
        /// </summary>
        public const int NORMALIZE = 96;
        /// <summary>
        /// 狙击手 -> 命中对手要害时威力提升。
        /// </summary>
        public const int SNIPER = 97;
        /// <summary>
        /// 魔法防御 -> 不会受到任何间接伤害。
        /// </summary>
        public const int MAGIC_GUARD = 98;
        /// <summary>
        /// 无防御 -> 自身与对手攻击都会绝对命中。
        /// </summary>
        public const int NO_GUARD = 99;
        /// <summary>
        /// 慢出 -> 始终慢于对手攻击。
        /// </summary>
        public const int STALL = 100;
        /// <summary>
        /// 技术员 -> 提升力量较小的技能的威力。
        /// </summary>
        public const int TECHNICIAN = 101;
        /// <summary>
        /// 叶片防御 -> 晴天时不会陷入异常状态。
        /// </summary>
        public const int LEAF_GUARD = 102;
        /// <summary>
        /// 笨拙 -> 不能使用携带道具。
        /// </summary>
        public const int KLUTZ = 103;
        /// <summary>
        /// 革新 -> 技能发动时不受对手特性影响。
        /// </summary>
        public const int MOLD_BREAKER = 104;
        /// <summary>
        /// 强运 -> 提升击中要害的几率。
        /// </summary>
        public const int SUPER_LUCK = 105;
        /// <summary>
        /// 引爆 -> 在受到最后一击后给对手造成伤害。
        /// </summary>
        public const int AFTERMATH = 106;
        /// <summary>
        /// 预知危险 -> 预知对手的危险技能。
        /// </summary>
        public const int ANTICIPATION = 107;
        /// <summary>
        /// 预知梦 -> 预知对手最强威力技能。
        /// </summary>
        public const int FOREWARN = 108;
        /// <summary>
        /// 后知后觉 -> 对手不能将其能力改变。
        /// </summary>
        public const int UNAWARE = 109;
        /// <summary>
        /// 有色眼镜 -> 增强效果一般技能的威力。
        /// </summary>
        public const int TINTED_LENS = 110;
        /// <summary>
        /// 过滤 -> 降低效果拔群技能的力量。
        /// </summary>
        public const int FILTER = 111;
        /// <summary>
        /// 错失先机 -> 对战初期攻击和速度减半。
        /// </summary>
        public const int SLOW_START = 112;
        /// <summary>
        /// 胆量 -> 可以使用任何技能攻击幽灵系神奇宝贝。
        /// </summary>
        public const int SCRAPPY = 113;
        /// <summary>
        /// 引水 -> 吸收所有水系技能攻击。
        /// </summary>
        public const int STORM_DRAIN = 114;
        /// <summary>
        /// 冰冻身体 -> 在冰雹天气中体力回复。
        /// </summary>
        public const int ICE_BODY = 115;
        /// <summary>
        /// 坚石 -> 降低效果拔群技能的力量。
        /// </summary>
        public const int SOLID_ROCK = 116;
        /// <summary>
        /// 降雪 -> 神奇宝贝出场时产生冰雹天气。
        /// </summary>
        public const int SNOW_WARNING = 117;
        /// <summary>
        /// 采蜜 -> 对战结束后可能采到甜蜜。
        /// </summary>
        public const int HONEY_GATHER = 118;
        /// <summary>
        /// 看穿 -> 看穿对手是否携带道具。
        /// </summary>
        public const int FRISK = 119;
        /// <summary>
        /// 舍身 -> 自身具有反弹伤害的技能力量提升。
        /// </summary>
        public const int RECKLESS = 120;
        /// <summary>
        /// 多元系 -> 携带相应令牌时改变属性。
        /// </summary>
        public const int MULTITYPE = 121;
        /// <summary>
        /// 花之礼 -> 阳光天气下己方神奇宝贝力量提升。
        /// </summary>
        public const int FLOWER_GIFT = 122;
        /// <summary>
        /// 恶梦 -> 消耗睡眠状态对手的体力。
        /// </summary>
        public const int BAD_DREAMS = 123;
        /// <summary>
        /// 偷盗恶习 -> 近身接触时偷走对手携带的道具。
        /// </summary>
        public const int PICKPOCKET = 124;
        /// <summary>
        /// 全力攻击 -> 技能拥有更高威力，但附加效果无法发动。
        /// </summary>
        public const int SHEER_FORCE = 125;
        /// <summary>
        /// 性情乖僻 -> 能力的变化逆转过来。
        /// </summary>
        public const int CONTRARY = 126;
        /// <summary>
        /// 紧张感 -> 对手神奇宝贝无法食用树果。
        /// </summary>
        public const int UNNERVE = 127;
        /// <summary>
        /// 不服输 -> 能力被降低时攻击提升。
        /// </summary>
        public const int DEFIANT = 128;
        /// <summary>
        /// 软弱 -> HP不足一半时能力下降。
        /// </summary>
        public const int DEFEATIST = 129;
        /// <summary>
        /// 诅咒身体 -> 受到攻击时可能禁用对手技能。
        /// </summary>
        public const int CURSED_BODY = 130;
        /// <summary>
        /// 治愈之心 -> 治愈队友的异常状态。
        /// </summary>
        public const int HEALER = 131;
        /// <summary>
        /// 队友守护 -> 减轻队友受到的伤害。
        /// </summary>
        public const int FRIEND_GUARD = 132;
        /// <summary>
        /// 碎甲 -> 受到物理攻击降低防御，提升速度。
        /// </summary>
        public const int WEAK_ARMOR = 133;
        /// <summary>
        /// 重合金 -> 体重变为两倍。
        /// </summary>
        public const int HEAVY_METAL = 134;
        /// <summary>
        /// 轻合金 -> 体重变为一半。
        /// </summary>
        public const int LIGHT_METAL = 135;
        /// <summary>
        /// 多重鳞片 -> HP满时受到伤害较少。
        /// </summary>
        public const int MULTISCALE = 136;
        /// <summary>
        /// 毒暴走 -> 中毒时物理威力提升。
        /// </summary>
        public const int TOXIC_BOOST = 137;
        /// <summary>
        /// 热暴走 -> 灼伤时特殊威力提升。
        /// </summary>
        public const int FLARE_BOOST = 138;
        /// <summary>
        /// 收获 -> 使用的树果有一定几率收回。
        /// </summary>
        public const int HARVEST = 139;
        /// <summary>
        /// 超感知 -> 避免受到队友的攻击。
        /// </summary>
        public const int TELEPATHY = 140;
        /// <summary>
        /// 心智不定 -> 一种能力降低，一种能力提升。
        /// </summary>
        public const int MOODY = 141;
        /// <summary>
        /// 防尘 -> 不会受到天气的伤害。
        /// </summary>
        public const int OVERCOAT = 142;
        /// <summary>
        /// 毒手 -> 近身攻击时一定几率使对手中毒。
        /// </summary>
        public const int POISON_TOUCH = 143;
        /// <summary>
        /// 再生力 -> 交换上场时回复HP。
        /// </summary>
        public const int REGENERATOR = 144;
        /// <summary>
        /// 鸽胸 -> 自身防御不会下降。
        /// </summary>
        public const int BIG_PECKS = 145;
        /// <summary>
        /// 挖沙 -> 沙雹天气下速度提升。
        /// </summary>
        public const int SAND_RUSH = 146;
        /// <summary>
        /// 奇迹皮肤 -> 一定几率不会受到变化技能的影响。
        /// </summary>
        public const int WONDER_SKIN = 147;
        /// <summary>
        /// 分析 -> 自身最后攻击的话，威力会增强。
        /// </summary>
        public const int ANALYTIC = 148;
        /// <summary>
        /// 幻觉 -> 变为队友神奇宝贝的样子。
        /// </summary>
        public const int ILLUSION = 149;
        /// <summary>
        /// 替代品 -> 整场对战中都会变身。
        /// </summary>
        public const int IMPOSTER = 150;
        /// <summary>
        /// 穿透 -> 不受减半反射和光墙影响。
        /// </summary>
        public const int INFILTRATOR = 151;
        /// <summary>
        /// 木乃伊 -> 受到近身攻击时对方特性变为木乃伊。
        /// </summary>
        public const int MUMMY = 152;
        /// <summary>
        /// 自信过剩 -> 打败一只神奇宝贝后攻击提升。
        /// </summary>
        public const int MOXIE = 153;
        /// <summary>
        /// 正义之心 -> 受到恶系技能攻击时攻击提升。
        /// </summary>
        public const int JUSTIFIED = 154;
        /// <summary>
        /// 颤抖 -> 一些情况下速度提升。
        /// </summary>
        public const int RATTLED = 155;
        /// <summary>
        /// 魔法反射 -> 受到变化技能攻击时会反弹。
        /// </summary>
        public const int MAGIC_BOUNCE = 156;
        /// <summary>
        /// 草食 -> 受到草系技能攻击时攻击提升。
        /// </summary>
        public const int SAP_SIPPER = 157;
        /// <summary>
        /// 恶作剧之心 -> 使用变化技能时总是先出。
        /// </summary>
        public const int PRANKSTER = 158;
        /// <summary>
        /// 砂之力 -> 沙雹天气下技能威力增加。
        /// </summary>
        public const int SAND_FORCE = 159;
        /// <summary>
        /// 铁刺荆棘 -> 受到近身攻击时反弹伤害。
        /// </summary>
        public const int IRON_BARBS = 160;
        /// <summary>
        /// 不倒翁模式 -> 在危急时转变为不倒翁模式。
        /// </summary>
        public const int ZEN_MODE = 161;
        /// <summary>
        /// 胜利之星 -> 己方全体神奇宝贝命中率提升。
        /// </summary>
        public const int VICTORY_STAR = 162;
        /// <summary>
        /// 涡轮高温 -> 无视阻碍自身技能的特性。
        /// </summary>
        public const int TURBOBLAZE = 163;
        /// <summary>
        /// 兆级电压 -> 无视阻碍自身技能的特性。
        /// </summary>
        public const int TERAVOLT = 164;
    }

    public static class Is
    {
        /// <summary>
        /// 白金玉 -> 让骑拉帝纳携带的话就能使龙属性和幽灵属性技能威力上升。闪耀着光芒的宝珠。
        /// </summary>
        public const int GRISEOUS_ORB = 1;
        /// <summary>
        /// 金刚玉 -> 帝牙卢卡携带的话，就能使龙属性和钢属性技能威力上升。闪耀着光芒的宝珠。
        /// </summary>
        public const int ADAMANT_ORB = 2;
        /// <summary>
        /// 白玉 -> 帕路奇犽携带的话，就能使龙属性和水属性技能威力上升。美丽的宝珠。
        /// </summary>
        public const int LUSTROUS_ORB = 3;
        /// <summary>
        /// 光粉 -> 熠熠生辉的粉末。携带后能迷惑对手，使其命中率下降。
        /// </summary>
        public const int BRIGHT_POWDER = 4;
        /// <summary>
        /// 白色药草 -> 宝可梦携带后，能力下降时能恢复１次状态。
        /// </summary>
        public const int WHITE_HERB = 5;
        /// <summary>
        /// 竞争背心 -> 又硬又重的石膏。携带会令速度下降，但成长会比较快。
        /// </summary>
        public const int MACHO_BRACE = 6;
        /// <summary>
        /// 先制之爪 -> 轻便锐利的爪子。携带后，有时能比对方更早出手。
        /// </summary>
        public const int QUICK_CLAW = 7;
        /// <summary>
        /// 精神药草 -> 宝可梦携带后，能在无法自由使用技能时恢复１次。
        /// </summary>
        public const int MENTAL_HERB = 8;
        /// <summary>
        /// 专爱头巾 -> 比较讲究的头巾。携带后虽然能提升攻击力，但只能发动同样的技能。
        /// </summary>
        public const int CHOICE_BAND = 9;
        /// <summary>
        /// 王者之证 -> 携带后，攻击对手时有一定几率使对方胆怯。
        /// </summary>
        public const int KINGS_ROCK = 10;
        /// <summary>
        /// 银色粉末 -> 发出银色光芒的粉末。携带后，虫属性技能威力会提升。
        /// </summary>
        public const int SILVERPOWDER = 11;
        /// <summary>
        /// 心之水滴 -> 让拉帝欧斯或拉帝亚斯携带的话，特攻和特防就能上升的神奇宝珠。
        /// </summary>
        public const int SOUL_DEW = 12;
        /// <summary>
        /// 深海之牙 -> 让珍珠贝携带的话，就能使特攻上升的牙齿，闪耀着银色的光芒。
        /// </summary>
        public const int DEEPSEATOOTH = 13;
        /// <summary>
        /// 深海之鳞 -> 让珍珠贝携带的话就能使特防上升的鳞片，闪耀着淡粉色的光芒。
        /// </summary>
        public const int DEEPSEASCALE = 14;
        /// <summary>
        /// 气息头巾 -> 携带后，有一定几率在遭受致命攻击时留下１点ＨＰ。
        /// </summary>
        public const int FOCUS_BAND = 15;
        /// <summary>
        /// 聚焦镜 -> 能看见弱点的透镜，宝可梦携带后可增加击中要害的几率。
        /// </summary>
        public const int SCOPE_LENS = 16;
        /// <summary>
        /// 金属外套 -> 特殊金属膜，携带后钢属性的技能威力会得到提升。
        /// </summary>
        public const int METAL_COAT = 17;
        /// <summary>
        /// 剩饭 -> 宝可梦携带后，在战斗时能持续恢复ＨＰ。
        /// </summary>
        public const int LEFTOVERS = 18;
        /// <summary>
        /// 电珠 -> 皮卡丘携带的话，能提升攻击和特攻威力的神奇宝玉。
        /// </summary>
        public const int LIGHT_BALL = 19;
        /// <summary>
        /// 软沙 -> 手感细腻的沙子，携带后地上属性技能威力会得到提升。
        /// </summary>
        public const int SOFT_SAND = 20;
        /// <summary>
        /// 坚硬岩石 -> 绝对不会碎裂的石子。携带后，岩石属性技能威力会得到提升。
        /// </summary>
        public const int HARD_STONE = 21;
        /// <summary>
        /// 奇迹之种 -> 有生命的种子。携带后草属性技能威力会得到提升。
        /// </summary>
        public const int MIRACLE_SEED = 22;
        /// <summary>
        /// 黑色眼镜 -> 造型奇特的眼镜。携带后恶属性技能威力会得到提升。
        /// </summary>
        public const int BLACKGLASSES = 23;
        /// <summary>
        /// 黑带 -> 能集中精力的带子。携带后格斗属性技能威力会得到提升。
        /// </summary>
        public const int BLACK_BELT = 24;
        /// <summary>
        /// 磁石 -> 强力磁铁。携带后电属性技能威力会得到提升。
        /// </summary>
        public const int MAGNET = 25;
        /// <summary>
        /// 神秘水滴 -> 水滴形宝石。携带后水属性技能威力会得到提升。
        /// </summary>
        public const int MYSTIC_WATER = 26;
        /// <summary>
        /// 尖锐鸟嘴 -> 长长的尖喙。携带后飞行属性技能威力会得到提升。
        /// </summary>
        public const int SHARP_BEAK = 27;
        /// <summary>
        /// 毒针 -> 有毒的细针。携带后毒属性技能威力会得到提升。
        /// </summary>
        public const int POISON_BARB = 28;
        /// <summary>
        /// 不融冰 -> 可以降温的冰。携带后冰属性技能威力会得到提升。
        /// </summary>
        public const int NEVERMELTICE = 29;
        /// <summary>
        /// 诅咒符 -> 令人毛骨悚然的咒符。携带后幽灵属性技能威力会得到提升。
        /// </summary>
        public const int SPELL_TAG = 30;
        /// <summary>
        /// 弯勺子 -> 注入了念力的羹匙。携带后超能力属性技能威力会得到提升。
        /// </summary>
        public const int TWISTEDSPOON = 31;
        /// <summary>
        /// 木炭 -> 促使燃烧的燃料。携带后炎属性技能威力会得到提升。
        /// </summary>
        public const int CHARCOAL = 32;
        /// <summary>
        /// 龙之牙 -> 坚硬锐利的牙齿。携带后龙属性技能威力会得到提升。
        /// </summary>
        public const int DRAGON_FANG = 33;
        /// <summary>
        /// 丝绸围巾 -> 手感舒适的围巾。携带后一般属性技能威力会得到提升。
        /// </summary>
        public const int SILK_SCARF = 34;
        /// <summary>
        /// 空贝铃 -> 宝可梦携带后，攻击对手时，能少量恢复ＨＰ。
        /// </summary>
        public const int SHELL_BELL = 35;
        /// <summary>
        /// 潮水香炉 -> 发出神秘香气的香炉。携带后水属性技能威力会得到提升。
        /// </summary>
        public const int SEA_INCENSE = 36;
        /// <summary>
        /// 无虑香炉 -> 携带后香料神秘的香气会迷惑对手，令对方命中率下降。
        /// </summary>
        public const int LAX_INCENSE = 37;
        /// <summary>
        /// 幸运拳套 -> 召唤幸运的手套。吉利蛋携带的话击中对方要害的几率会提升。
        /// </summary>
        public const int LUCKY_PUNCH = 38;
        /// <summary>
        /// 金属粉末 -> 百变怪携带的话能提升防御力的神秘粉末。非常细且质地坚硬。
        /// </summary>
        public const int METAL_POWDER = 39;
        /// <summary>
        /// 粗骨棒 -> 质地坚硬的骨头。可拉可拉或者嘎拉嘎拉携带后能提升攻击力。
        /// </summary>
        public const int THICK_CLUB = 40;
        /// <summary>
        /// 长葱 -> 很长很硬的草茎。大葱鸭携带的话击中对方要害的几率会提升。
        /// </summary>
        public const int STICK = 41;
        /// <summary>
        /// 广角镜 -> 能仔细观察的放大镜。携带后技能的命中率会稍微提升。
        /// </summary>
        public const int WIDE_LENS = 42;
        /// <summary>
        /// 力量头巾 -> 力如泉涌的头巾。携带后能够稍微提升物理攻击的威力。
        /// </summary>
        public const int MUSCLE_BAND = 43;
        /// <summary>
        /// 智慧眼镜 -> 超厚镜片的眼镜。携带后能够稍微提升特殊攻击的威力。
        /// </summary>
        public const int WISE_GLASSES = 44;
        /// <summary>
        /// 达人腰带 -> 用旧的黑带。携带后能够稍微提升效果拔群时技能的威力。
        /// </summary>
        public const int EXPERT_BELT = 45;
        /// <summary>
        /// 光之黏土 -> 携带它的宝可梦使用光墙或减半反射时，效果比平时更长。
        /// </summary>
        public const int LIGHT_CLAY = 46;
        /// <summary>
        /// 生命玉 -> 携带后每次攻击都会减少ＨＰ，但技能威力会得到提升。
        /// </summary>
        public const int LIFE_ORB = 47;
        /// <summary>
        /// 力量药草 -> 携带它的宝可梦有一次机会可以在第一回合使用需要经过蓄力的技能。
        /// </summary>
        public const int POWER_HERB = 48;
        /// <summary>
        /// 剧毒珠 -> 被碰到就会放出毒素的神秘宝玉。携带后会在战斗中进入剧毒状态。
        /// </summary>
        public const int TOXIC_ORB = 49;
        /// <summary>
        /// 火焰珠 -> 被碰到就会放出热量的神秘宝玉。携带后会在战斗中进入烧伤状态。
        /// </summary>
        public const int FLAME_ORB = 50;
        /// <summary>
        /// 速度粉末 -> 百变怪携带的话能提升速度的神秘粉末。非常细且质地坚硬。
        /// </summary>
        public const int QUICK_POWDER = 51;
        /// <summary>
        /// 气息腰带 -> 携带后ＨＰ全满时即使受到致命攻击，也有一次机会令ＨＰ留下１点。
        /// </summary>
        public const int FOCUS_SASH = 52;
        /// <summary>
        /// 瞄准镜 -> 携带它的宝可梦在遇见比自己行动迅速的对手时，能提高技能的命中率。
        /// </summary>
        public const int ZOOM_LENS = 53;
        /// <summary>
        /// 节拍器 -> 携带后，连续使用同样的技能会提升威力，一旦停止，威力也会复原。
        /// </summary>
        public const int METRONOME = 54;
        /// <summary>
        /// 黑铁球 -> 携带后，速度会下降。飞行系或漂浮特性宝可梦也会被地上属性技能打中。
        /// </summary>
        public const int IRON_BALL = 55;
        /// <summary>
        /// 后攻尾 -> 非常沉重的尾巴。携带后行动会变得迟缓。
        /// </summary>
        public const int LAGGING_TAIL = 56;
        /// <summary>
        /// 红色丝线 -> 又细又长的红色丝线。携带后进入迷人状态时对手也会进入迷人状态。
        /// </summary>
        public const int DESTINY_KNOT = 57;
        /// <summary>
        /// 黑色淤泥 -> 携带后毒系宝可梦会慢慢恢复ＨＰ。其他类型ＨＰ会减少。
        /// </summary>
        public const int BLACK_SLUDGE = 58;
        /// <summary>
        /// 寒冰岩石 -> 携带它的宝可梦使用冰雹时，冰雹的时间延长。
        /// </summary>
        public const int ICY_ROCK = 59;
        /// <summary>
        /// 光滑岩石 -> 携带它的宝可梦使用沙雹时沙雹的时间延长。
        /// </summary>
        public const int SMOOTH_ROCK = 60;
        /// <summary>
        /// 炽热岩石 -> 携带它的宝可梦使用大晴天时晴天的时间延长。
        /// </summary>
        public const int HEAT_ROCK = 61;
        /// <summary>
        /// 潮湿岩石 -> 携带它的宝可梦在使用乞雨时，下雨的时间延长。
        /// </summary>
        public const int DAMP_ROCK = 62;
        /// <summary>
        /// 粘附钩爪 -> 携带后束缚类的卷紧等能够连续攻击的技能攻击回合增加。
        /// </summary>
        public const int GRIP_CLAW = 63;
        /// <summary>
        /// 专爱围巾 -> 比较讲究的围巾。携带后虽然能提高速度，但只能发动同样的技能。
        /// </summary>
        public const int CHOICE_SCARF = 64;
        /// <summary>
        /// 附针 -> 携带后每回合都会受到伤害，但有时也会影响碰过自己的对手。
        /// </summary>
        public const int STICKY_BARB = 65;
        /// <summary>
        /// 力量护腕 -> 携带后速度虽然会下降，但宝可梦的攻击增长得比较迅速。
        /// </summary>
        public const int POWER_BRACER = 66;
        /// <summary>
        /// 力量腰带 -> 携带后速度虽然会下降，但宝可梦的防御增长得比较迅速。
        /// </summary>
        public const int POWER_BELT = 67;
        /// <summary>
        /// 力量眼镜 -> 携带后速度虽然会下降，但宝可梦的特攻增长得比较迅速。
        /// </summary>
        public const int POWER_LENS = 68;
        /// <summary>
        /// 力量束带 -> 携带后速度虽然会下降，但宝可梦的特防增长得比较迅速。
        /// </summary>
        public const int POWER_BAND = 69;
        /// <summary>
        /// 力量护踝 -> 携带后速度虽然会下降，但宝可梦的速度增长得比较迅速。
        /// </summary>
        public const int POWER_ANKLET = 70;
        /// <summary>
        /// 力量束腰 -> 携带后速度虽然会下降，但宝可梦的ＨＰ增长得比较迅速。
        /// </summary>
        public const int POWER_WEIGHT = 71;
        /// <summary>
        /// 绚丽外壳 -> 坚硬的蜕皮。携带它的宝可梦绝对能和后备宝可梦交换位置。
        /// </summary>
        public const int SHED_SHELL = 72;
        /// <summary>
        /// 大根 -> 携带后使用吸收ＨＰ的技能时能比平时吸收更多的ＨＰ。
        /// </summary>
        public const int BIG_ROOT = 73;
        /// <summary>
        /// 专爱眼镜 -> 比较讲究的眼镜。携带后虽然能提升特攻，但只能发动同样的技能。
        /// </summary>
        public const int CHOICE_SPECS = 74;
        /// <summary>
        /// 火球石板 -> 炎属性石板。携带后炎属性技能威力增强。
        /// </summary>
        public const int FLAME_PLATE = 75;
        /// <summary>
        /// 水滴石板 -> 水属性石板。携带后水属性技能威力增强。
        /// </summary>
        public const int SPLASH_PLATE = 76;
        /// <summary>
        /// 雷电石板 -> 电属性石板。携带后电属性技能威力增强。
        /// </summary>
        public const int ZAP_PLATE = 77;
        /// <summary>
        /// 绿色石板 -> 草属性石板。携带后草属性技能威力增强。
        /// </summary>
        public const int MEADOW_PLATE = 78;
        /// <summary>
        /// 冰柱石板 -> 冰属性石板。携带后冰属性技能威力增强。
        /// </summary>
        public const int ICICLE_PLATE = 79;
        /// <summary>
        /// 拳击石板 -> 格斗属性石板。携带后格斗属性技能威力增强。
        /// </summary>
        public const int FIST_PLATE = 80;
        /// <summary>
        /// 剧毒石板 -> 毒属性石板。携带后毒属性技能威力增强。
        /// </summary>
        public const int TOXIC_PLATE = 81;
        /// <summary>
        /// 大地石板 -> 地上属性石板。携带后地上属性技能威力增强。
        /// </summary>
        public const int EARTH_PLATE = 82;
        /// <summary>
        /// 青空石板 -> 飞行属性石板。携带后飞行属性技能威力增强。
        /// </summary>
        public const int SKY_PLATE = 83;
        /// <summary>
        /// 神秘石板 -> 超能力属性石板。携带后超能力属性技能威力增强。
        /// </summary>
        public const int MIND_PLATE = 84;
        /// <summary>
        /// 昆虫石板 -> 虫属性石板。携带后虫属性技能威力增强。
        /// </summary>
        public const int INSECT_PLATE = 85;
        /// <summary>
        /// 岩石石板 -> 岩石属性石板。携带后岩石属性技能威力增强。
        /// </summary>
        public const int STONE_PLATE = 86;
        /// <summary>
        /// 幽灵石板 -> 幽灵属性石板。携带后幽灵属性技能威力增强。
        /// </summary>
        public const int SPOOKY_PLATE = 87;
        /// <summary>
        /// 龙之石板 -> 龙属性石板。携带后龙属性技能威力增强。
        /// </summary>
        public const int DRACO_PLATE = 88;
        /// <summary>
        /// 恐怖石板 -> 恶属性石板。携带后恶属性技能威力增强。
        /// </summary>
        public const int DREAD_PLATE = 89;
        /// <summary>
        /// 钢铁石板 -> 钢属性石板。携带后钢属性技能威力增强。
        /// </summary>
        public const int IRON_PLATE = 90;
        /// <summary>
        /// 怪异香炉 -> 发出神秘香气的香炉。携带后超能力属性技能威力会得到提升。
        /// </summary>
        public const int ODD_INCENSE = 91;
        /// <summary>
        /// 岩石香炉 -> 发出神秘香气的香炉。携带后岩石属性技能威力会得到提升。
        /// </summary>
        public const int ROCK_INCENSE = 92;
        /// <summary>
        /// 满腹香炉 -> 发出神秘香气的香炉。携带它的宝可梦行动会变得迟缓。
        /// </summary>
        public const int FULL_INCENSE = 93;
        /// <summary>
        /// 细波香炉 -> 发出神秘香气的香炉。携带后水属性技能威力会得到提升。
        /// </summary>
        public const int WAVE_INCENSE = 94;
        /// <summary>
        /// 花朵香炉 -> 发出神秘香气的香炉。携带后草属性技能威力会得到提升。
        /// </summary>
        public const int ROSE_INCENSE = 95;
        /// <summary>
        /// 尖锐之爪 -> 尖锐的爪子。携带后击中要害的几率增加。
        /// </summary>
        public const int RAZOR_CLAW = 96;
        /// <summary>
        /// 尖锐之牙 -> 尖锐的牙齿。携带后攻击对手时有一定几率使对方胆怯。
        /// </summary>
        public const int RAZOR_FANG = 97;
        /// <summary>
        /// 海洋卡带 -> 让盖诺塞克特携带的话，科技爆破就会变成水属性。
        /// </summary>
        public const int DOUSE_DRIVE = 98;
        /// <summary>
        /// 雷电卡带 -> 让盖诺塞克特携带的话，科技爆破就会变成电属性。
        /// </summary>
        public const int SHOCK_DRIVE = 99;
        /// <summary>
        /// 火焰卡带 -> 让盖诺塞克特携带的话，科学爆破就会变成炎属性。
        /// </summary>
        public const int BURN_DRIVE = 100;
        /// <summary>
        /// 冰冻卡带 -> 让盖诺塞克特携带的话，科学爆破就会变成冰属性。
        /// </summary>
        public const int CHILL_DRIVE = 101;
        /// <summary>
        /// 进化辉石 -> 进化的神秘物品。携带后能提升进化前宝可梦的防御和特防。
        /// </summary>
        public const int EVIOLITE = 102;
        /// <summary>
        /// 浮石 -> 非常轻的石头。携带后宝可梦的体重会变轻。
        /// </summary>
        public const int FLOAT_STONE = 103;
        /// <summary>
        /// 坚硬头盔 -> 让宝可梦携带后，被打击技能攻击的话，对方也会受到伤害。
        /// </summary>
        public const int ROCKY_HELMET = 104;
        /// <summary>
        /// 气球 -> 让宝可梦携带的话，宝可梦能浮空。被攻击的话会破裂。
        /// </summary>
        public const int AIR_BALLOON = 105;
        /// <summary>
        /// 红牌 -> 拥有神秘力量的卡片。携带的话被攻击后能令对手退场。
        /// </summary>
        public const int RED_CARD = 106;
        /// <summary>
        /// 狙击标靶 -> 因为宝可梦属性无效的攻击也能打中对方。
        /// </summary>
        public const int RING_TARGET = 107;
        /// <summary>
        /// 束缚头巾 -> 辅助缠绕型攻击的皮带。携带后能提升缠绕型攻击的威力。
        /// </summary>
        public const int BINDING_BAND = 108;
        /// <summary>
        /// 灯泡 -> 一次性球根。携带后被水属性技能攻击的话可提升特攻。
        /// </summary>
        public const int ABSORB_BULB = 109;
        /// <summary>
        /// 充电电池 -> 一次性电池。携带后被电属性技能攻击的话可提升攻击。
        /// </summary>
        public const int CELL_BATTERY = 110;
        /// <summary>
        /// 逃生按钮 -> 携带后如果受到攻击，可从战斗中脱离和后备的宝可梦进行交换。
        /// </summary>
        public const int EJECT_BUTTON = 111;
        /// <summary>
        /// 火焰珠宝 -> 炎属性宝石。携带后有一次机会使炎属性技能威力增强。
        /// </summary>
        public const int FIRE_GEM = 112;
        /// <summary>
        /// 水之珠宝 -> 水属性宝石。携带后有一次机会使水属性技能威力增强。
        /// </summary>
        public const int WATER_GEM = 113;
        /// <summary>
        /// 闪电珠宝 -> 电属性宝石。携带后有一次机会使电属性技能威力增强。
        /// </summary>
        public const int ELECTRIC_GEM = 114;
        /// <summary>
        /// 草之珠宝 -> 草属性宝石。携带后有一次机会使草属性技能威力增强。
        /// </summary>
        public const int GRASS_GEM = 115;
        /// <summary>
        /// 寒冰珠宝 -> 冰属性宝石。携带后有一次机会使冰属性技能威力增强。
        /// </summary>
        public const int ICE_GEM = 116;
        /// <summary>
        /// 格斗珠宝 -> 格斗属性宝石。携带后有一次机会使格斗属性技能威力增强。
        /// </summary>
        public const int FIGHTING_GEM = 117;
        /// <summary>
        /// 剧毒珠宝 -> 毒属性宝石。携带后有一次机会使毒属性技能威力增强。
        /// </summary>
        public const int POISON_GEM = 118;
        /// <summary>
        /// 大地珠宝 -> 地上属性宝石。携带后有一次机会使地上属性技能威力增强。
        /// </summary>
        public const int GROUND_GEM = 119;
        /// <summary>
        /// 飞行珠宝 -> 飞行属性宝石。携带后有一次机会使飞行属性技能威力增强。
        /// </summary>
        public const int FLYING_GEM = 120;
        /// <summary>
        /// 超能珠宝 -> 超能力属性宝石。携带后有一次机会使超能力属性技能威力增强。
        /// </summary>
        public const int PSYCHIC_GEM = 121;
        /// <summary>
        /// 虫之珠宝 -> 虫属性宝石。携带后有一次机会使虫属性技能威力增强。
        /// </summary>
        public const int BUG_GEM = 122;
        /// <summary>
        /// 岩石珠宝 -> 岩石属性宝石。携带后有一次机会使岩石属性技能威力增强。
        /// </summary>
        public const int ROCK_GEM = 123;
        /// <summary>
        /// 幽灵珠宝 -> 幽灵属性宝石。携带后有一次机会使幽灵属性技能威力增强。
        /// </summary>
        public const int GHOST_GEM = 124;
        /// <summary>
        /// 飞龙珠宝 -> 龙属性宝石。携带后有一次机会使龙属性技能威力增强。
        /// </summary>
        public const int DRAGON_GEM = 125;
        /// <summary>
        /// 暗黑珠宝 -> 恶属性宝石。携带后有一次机会使恶属性技能威力增强。
        /// </summary>
        public const int DARK_GEM = 126;
        /// <summary>
        /// 钢铁珠宝 -> 钢属性宝石。携带后有一次机会使钢属性技能威力增强。
        /// </summary>
        public const int STEEL_GEM = 127;
        /// <summary>
        /// 普通珠宝 -> 一般属性宝石。携带后有一次机会使一般属性技能威力增强。
        /// </summary>
        public const int NORMAL_GEM = 128;
        /// <summary>
        /// 解麻果 -> 让宝可梦携带的话，能恢复麻痹。
        /// </summary>
        public const int CHERI_BERRY = 129;
        /// <summary>
        /// 醒睡果 -> 让宝可梦携带的话，能唤醒睡眠状态。
        /// </summary>
        public const int CHESTO_BERRY = 130;
        /// <summary>
        /// 解毒果 -> 让宝可梦携带的话，可以解毒。
        /// </summary>
        public const int PECHA_BERRY = 131;
        /// <summary>
        /// 烧伤果 -> 让宝可梦携带的话，能治疗烧伤。
        /// </summary>
        public const int RAWST_BERRY = 132;
        /// <summary>
        /// 解冻果 -> 让宝可梦携带的话，可以解冻。
        /// </summary>
        public const int ASPEAR_BERRY = 133;
        /// <summary>
        /// 海棠果 -> 让宝可梦携带的话，能恢复１０ＰＰ。
        /// </summary>
        public const int LEPPA_BERRY = 134;
        /// <summary>
        /// 橙果 -> 让宝可梦携带的话，能恢复１０ＨＰ。
        /// </summary>
        public const int ORAN_BERRY = 135;
        /// <summary>
        /// 混乱果 -> 让宝可梦携带的话，能治疗混乱。
        /// </summary>
        public const int PERSIM_BERRY = 136;
        /// <summary>
        /// 奇迹果 -> 让宝可梦携带的话，能解除所有状态异常。
        /// </summary>
        public const int LUM_BERRY = 137;
        /// <summary>
        /// 文旦果 -> 让宝可梦携带的话，能少量恢复ＨＰ。
        /// </summary>
        public const int SITRUS_BERRY = 138;
        /// <summary>
        /// 11号果实 -> 让宝可梦携带的话，危急时刻能恢复ＨＰ。如果讨厌这个味道则会混乱。
        /// </summary>
        public const int FIGY_BERRY = 139;
        /// <summary>
        /// 12号果实 -> 让宝可梦携带的话，危急时刻能恢复ＨＰ。如果讨厌这个味道则会混乱。
        /// </summary>
        public const int WIKI_BERRY = 140;
        /// <summary>
        /// 13号果实 -> 让宝可梦携带的话，危急时刻能恢复ＨＰ。如果讨厌这个味道则会混乱。
        /// </summary>
        public const int MAGO_BERRY = 141;
        /// <summary>
        /// 14号果实 -> 让宝可梦携带的话，危急时刻能恢复ＨＰ。如果讨厌这个味道则会混乱。
        /// </summary>
        public const int AGUAV_BERRY = 142;
        /// <summary>
        /// 15号果实 -> 让宝可梦携带的话，危急时刻能恢复ＨＰ。如果讨厌这个味道则会混乱。
        /// </summary>
        public const int IAPAPA_BERRY = 143;
        /// <summary>
        /// 16号果实 -> 在合众地区非常罕见的果实。发烧友会高价收购。
        /// </summary>
        public const int RAZZ_BERRY = 144;
        /// <summary>
        /// 17号果实 -> 在合众地区非常罕见的果实。发烧友会高价收购。
        /// </summary>
        public const int BLUK_BERRY = 145;
        /// <summary>
        /// 18号果实 -> 在合众地区非常罕见的果实。发烧友会高价收购。
        /// </summary>
        public const int NANAB_BERRY = 146;
        /// <summary>
        /// 19号果实 -> 在合众地区非常罕见的果实。发烧友会高价收购。
        /// </summary>
        public const int WEPEAR_BERRY = 147;
        /// <summary>
        /// 20号果实 -> 在合众地区非常罕见的果实。发烧友会高价收购。
        /// </summary>
        public const int PINAP_BERRY = 148;
        /// <summary>
        /// 21号果实 -> 将它交给宝可梦的话亲密度非常容易提升，但是ＨＰ的努力值会下降。
        /// </summary>
        public const int POMEG_BERRY = 149;
        /// <summary>
        /// 22号果实 -> 将它交给宝可梦的话亲密度非常容易提升，但是攻击的努力值会下降。
        /// </summary>
        public const int KELPSY_BERRY = 150;
        /// <summary>
        /// 23号果实 -> 将它交给宝可梦的话亲密度非常容易提升，但是防御的努力值会下降。
        /// </summary>
        public const int QUALOT_BERRY = 151;
        /// <summary>
        /// 24号果实 -> 将它交给宝可梦的话亲密度非常容易提升，但是特攻的努力值会下降。
        /// </summary>
        public const int HONDEW_BERRY = 152;
        /// <summary>
        /// 25号果实 -> 将它交给宝可梦的话亲密度非常容易提升，但是特防的努力值会下降。
        /// </summary>
        public const int GREPA_BERRY = 153;
        /// <summary>
        /// 26号果实 -> 将它交给宝可梦的话亲密度非常容易提升，但是速度的努力值会下降。
        /// </summary>
        public const int TAMATO_BERRY = 154;
        /// <summary>
        /// 27号果实 -> 在合众地区非常罕见的果实。发烧友会高价收购。
        /// </summary>
        public const int CORNN_BERRY = 155;
        /// <summary>
        /// 28号果实 -> 在合众地区非常罕见的果实。发烧友会高价收购。
        /// </summary>
        public const int MAGOST_BERRY = 156;
        /// <summary>
        /// 29号果实 -> 在合众地区非常罕见的果实。发烧友会高价收购。
        /// </summary>
        public const int RABUTA_BERRY = 157;
        /// <summary>
        /// 30号果实 -> 在合众地区非常罕见的果实。发烧友会高价收购。
        /// </summary>
        public const int NOMEL_BERRY = 158;
        /// <summary>
        /// 31号果实 -> 在合众地区非常罕见的果实。发烧友会高价收购。
        /// </summary>
        public const int SPELON_BERRY = 159;
        /// <summary>
        /// 32号果实 -> 在合众地区非常罕见的果实。发烧友会高价收购。
        /// </summary>
        public const int PAMTRE_BERRY = 160;
        /// <summary>
        /// 33号果实 -> 在合众地区非常罕见的果实。发烧友会高价收购。
        /// </summary>
        public const int WATMEL_BERRY = 161;
        /// <summary>
        /// 34号果实 -> 在合众地区非常罕见的果实。发烧友会高价收购。
        /// </summary>
        public const int DURIN_BERRY = 162;
        /// <summary>
        /// 35号果实 -> 在合众地区非常罕见的果实。发烧友会高价收购。
        /// </summary>
        public const int BELUE_BERRY = 163;
        /// <summary>
        /// 抗火果 -> 让宝可梦携带的话，被效果拔群的炎属性技能攻击时能令其威力下降。
        /// </summary>
        public const int OCCA_BERRY = 164;
        /// <summary>
        /// 抗水果 -> 让宝可梦携带的话，被效果拔群的水属性技能攻击时能令其威力下降。
        /// </summary>
        public const int PASSHO_BERRY = 165;
        /// <summary>
        /// 抗电果 -> 让宝可梦携带的话，被效果拔群的电属性技能攻击时能令其威力下降。
        /// </summary>
        public const int WACAN_BERRY = 166;
        /// <summary>
        /// 抗草果 -> 让宝可梦携带的话，被效果拔群的草属性技能攻击时能令其威力下降。
        /// </summary>
        public const int RINDO_BERRY = 167;
        /// <summary>
        /// 抗冰果 -> 让宝可梦携带的话，被效果拔群的冰属性技能攻击时能令其威力下降。
        /// </summary>
        public const int YACHE_BERRY = 168;
        /// <summary>
        /// 抗格果 -> 让宝可梦携带的话，被效果拔群的格斗属性技能攻击时能令其威力下降。
        /// </summary>
        public const int CHOPLE_BERRY = 169;
        /// <summary>
        /// 抗毒果 -> 让宝可梦携带的话，被效果拔群的毒属性技能攻击时能令其威力下降。
        /// </summary>
        public const int KEBIA_BERRY = 170;
        /// <summary>
        /// 抗地果 -> 让宝可梦携带的话，被效果拔群的地上属性技能攻击时能令其威力下降。
        /// </summary>
        public const int SHUCA_BERRY = 171;
        /// <summary>
        /// 抗飞果 -> 让宝可梦携带的话，被效果拔群的飞行属性技能攻击时能令其威力下降。
        /// </summary>
        public const int COBA_BERRY = 172;
        /// <summary>
        /// 抗超果 -> 让宝可梦携带的话，被效果拔群的超能力属性技能攻击时令其威力下降。
        /// </summary>
        public const int PAYAPA_BERRY = 173;
        /// <summary>
        /// 抗虫果 -> 让宝可梦携带的话，被效果拔群的虫属性技能攻击时能令其威力下降。
        /// </summary>
        public const int TANGA_BERRY = 174;
        /// <summary>
        /// 抗岩果 -> 让宝可梦携带的话，被效果拔群的岩石属性技能攻击时能令其威力下降。
        /// </summary>
        public const int CHARTI_BERRY = 175;
        /// <summary>
        /// 抗鬼果 -> 让宝可梦携带的话，被效果拔群的幽灵属性技能攻击时能令其威力下降。
        /// </summary>
        public const int KASIB_BERRY = 176;
        /// <summary>
        /// 抗龙果 -> 让宝可梦携带的话，被效果拔群的龙属性技能攻击时能令其威力下降。
        /// </summary>
        public const int HABAN_BERRY = 177;
        /// <summary>
        /// 抗恶果 -> 让宝可梦携带的话，被效果拔群的恶属性技能攻击时能令其威力下降。
        /// </summary>
        public const int COLBUR_BERRY = 178;
        /// <summary>
        /// 抗钢果 -> 让宝可梦携带的话，被效果拔群的钢属性技能攻击时能令其威力下降。
        /// </summary>
        public const int BABIRI_BERRY = 179;
        /// <summary>
        /// 抗普果 -> 让宝可梦携带的话，被一般属性技能攻击时，能令其威力下降。
        /// </summary>
        public const int CHILAN_BERRY = 180;
        /// <summary>
        /// 物攻果 -> 让宝可梦携带的话，危急时刻能提升自己的攻击。
        /// </summary>
        public const int LIECHI_BERRY = 181;
        /// <summary>
        /// 物防果 -> 让宝可梦携带的话，危急时刻能提升自己的防御。
        /// </summary>
        public const int GANLON_BERRY = 182;
        /// <summary>
        /// 速度果 -> 让宝可梦携带的话，危急时刻能提升自己的速度。
        /// </summary>
        public const int SALAC_BERRY = 183;
        /// <summary>
        /// 特攻果 -> 让宝可梦携带的话，危急时刻能提升自己的特攻。
        /// </summary>
        public const int PETAYA_BERRY = 184;
        /// <summary>
        /// 特防果 -> 让宝可梦携带的话，危急时刻能提升自己的特防。
        /// </summary>
        public const int APICOT_BERRY = 185;
        /// <summary>
        /// 会心果 -> 让宝可梦携带的话，危急时刻能提升攻击的命中要害几率。
        /// </summary>
        public const int LANSAT_BERRY = 186;
        /// <summary>
        /// 随机果 -> 让宝可梦携带的话，危急时刻某种能力会大幅提升。
        /// </summary>
        public const int STARF_BERRY = 187;
        /// <summary>
        /// 谜之果 -> 让宝可梦携带的话，被效果拔群的技能攻击时可恢复ＨＰ。
        /// </summary>
        public const int ENIGMA_BERRY = 188;
        /// <summary>
        /// 命中果 -> 让宝可梦携带的话，危急时刻可提升１次技能的命中率。
        /// </summary>
        public const int MICLE_BERRY = 189;
        /// <summary>
        /// 先制果 -> 让宝可梦携带的话，危急时刻可使行动提前１次。
        /// </summary>
        public const int CUSTAP_BERRY = 190;
        /// <summary>
        /// 嘉宝果 -> 让宝可梦携带的话，被物理攻击击中时对方也会受到伤害。
        /// </summary>
        public const int JABOCA_BERRY = 191;
        /// <summary>
        /// 莲雾果 -> 让宝可梦携带的话，被特殊技能击中时对方也会受到伤害。
        /// </summary>
        public const int ROWAP_BERRY = 192;
        /// <summary>
        /// 天空邮件 -> 适合写邀请函的信纸。让宝可梦携带使用。
        /// </summary>
        public const int RSVP_MAIL = 193;
        /// <summary>
        /// 果汁 -> １００％纯果汁。能使１只宝可梦恢复２０ＨＰ。
        /// </summary>
        public const int BERRY_JUICE = 194;
    }

    public static class Ms
    {
        /// <summary>
        /// No.001 拍击 -> 用长尾巴或长手拍打对方。
        /// </summary>
        public const int POUND = 1;
        /// <summary>
        /// No.002 手刀 -> 用强劲的手刀击打对方。容易发动会心一击。
        /// </summary>
        public const int KARATE_CHOP = 2;
        /// <summary>
        /// No.003 连环巴掌 -> 用反复拍打的方式连续攻击２－５次。
        /// </summary>
        public const int DOUBLESLAP = 3;
        /// <summary>
        /// No.004 连续拳 -> 用怒涛般的拳击连续攻击２－５次。
        /// </summary>
        public const int COMET_PUNCH = 4;
        /// <summary>
        /// No.005 百万吨拳击 -> 积蓄力量用拳头攻击。
        /// </summary>
        public const int MEGA_PUNCH = 5;
        /// <summary>
        /// No.006 聚宝功 -> 向对方扔钱，进行攻击。战斗后获得金钱。
        /// </summary>
        public const int PAY_DAY = 6;
        /// <summary>
        /// No.007 火焰拳 -> 用附带着火焰的拳头攻击对方。有时会使对方烧伤。
        /// </summary>
        public const int FIRE_PUNCH = 7;
        /// <summary>
        /// No.008 急冻拳 -> 用附带着冷气的拳头攻击对方。有时会使对方结冰。
        /// </summary>
        public const int ICE_PUNCH = 8;
        /// <summary>
        /// No.009 雷光掌 -> 用附带着电气的拳头攻击对方。有时会使对方麻痹。
        /// </summary>
        public const int THUNDERPUNCH = 9;
        /// <summary>
        /// No.010 利爪 -> 用坚硬的利爪攻击对方。
        /// </summary>
        public const int SCRATCH = 10;
        /// <summary>
        /// No.011 剪断 -> 将对方从两边夹起来给与伤害。
        /// </summary>
        public const int VICEGRIP = 11;
        /// <summary>
        /// No.012 剪刀断头台 -> 用巨大的钳子切断对方进行攻击。只要打中就会使对方濒死。
        /// </summary>
        public const int GUILLOTINE = 12;
        /// <summary>
        /// No.013 旋风刀 -> 制造风之刃连续攻击对方２回。容易发动会心一击。
        /// </summary>
        public const int RAZOR_WIND = 13;
        /// <summary>
        /// No.014 剑舞 -> 跳起战斗之舞提升气势。大幅提升自身的攻击。
        /// </summary>
        public const int SWORDS_DANCE = 14;
        /// <summary>
        /// No.015 一字斩 -> 用刀刃或爪子切割对方进行攻击。还能切断比较细的树。
        /// </summary>
        public const int CUT = 15;
        /// <summary>
        /// No.016 烈暴风 -> 用翅膀制造烈风攻击对方。
        /// </summary>
        public const int GUST = 16;
        /// <summary>
        /// No.017 翅膀攻击 -> 用展开的大翅膀攻击对方。
        /// </summary>
        public const int WING_ATTACK = 17;
        /// <summary>
        /// No.018 旋风 -> 将对方吹走，强制让对方交换[宝可梦]。野生[宝可梦]的话会直接结束战斗。
        /// </summary>
        public const int WHIRLWIND = 18;
        /// <summary>
        /// No.019 飞翔 -> 首回合飞到空中，下一回合攻击对方。能够飞到已经去过的城镇。
        /// </summary>
        public const int FLY = 19;
        /// <summary>
        /// No.020 绑紧 -> 使用长长的身体和藤蔓勒住对方４－５回合造成伤害。
        /// </summary>
        public const int BIND = 20;
        /// <summary>
        /// No.021 叩打 -> 使用长尾巴或者藤蔓攻击对方。
        /// </summary>
        public const int SLAM = 21;
        /// <summary>
        /// No.022 藤鞭 -> 使用如同鞭子般细长的藤蔓抽打对方进行攻击。
        /// </summary>
        public const int VINE_WHIP = 22;
        /// <summary>
        /// No.023 践踏 -> 用巨大的脚踩踏对方进行攻击。有时会使对方胆怯。
        /// </summary>
        public const int STOMP = 23;
        /// <summary>
        /// No.024 连环腿 -> 用双脚踢对方进行攻击。能够造成两次伤害。
        /// </summary>
        public const int DOUBLE_KICK = 24;
        /// <summary>
        /// No.025 百万吨飞腿 -> 使出充满浑身力量，用脚踢击对方进行攻击。
        /// </summary>
        public const int MEGA_KICK = 25;
        /// <summary>
        /// No.026 飞踢 -> 跳到高处向下踢击对方。如果没有命中，会使自己受伤。
        /// </summary>
        public const int JUMP_KICK = 26;
        /// <summary>
        /// No.027 旋风腿 -> 高速旋转身体，边对对方进行踢击。
        /// </summary>
        public const int ROLLING_KICK = 27;
        /// <summary>
        /// No.028 飞沙脚 -> 向对方脸上撒砂子降低对方命中率。
        /// </summary>
        public const int SANDATTACK = 28;
        /// <summary>
        /// No.029 铁头功 -> 冲向对方，用头进行攻击。有时会使对方胆怯。
        /// </summary>
        public const int HEADBUTT = 29;
        /// <summary>
        /// No.030 角撞 -> 用锋利的爪子攻击对方。
        /// </summary>
        public const int HORN_ATTACK = 30;
        /// <summary>
        /// No.031 疯狂攻击 -> 用角或者喙，连续攻击对方２ー５次。
        /// </summary>
        public const int FURY_ATTACK = 31;
        /// <summary>
        /// No.032 独角钻 -> 用高速旋转的角刺穿对方。如果打中会使对方濒死。
        /// </summary>
        public const int HORN_DRILL = 32;
        /// <summary>
        /// No.033 冲击 -> 使出全身的力气，撞击对方。
        /// </summary>
        public const int TACKLE = 33;
        /// <summary>
        /// No.034 泰山压顶 -> 用自己的身体，压住对方进行攻击。有时会使对方麻痹。
        /// </summary>
        public const int BODY_SLAM = 34;
        /// <summary>
        /// No.035 捆绑 -> 使用长长的身体和藤蔓缠绕对方４－５回合造成伤害。
        /// </summary>
        public const int WRAP = 35;
        /// <summary>
        /// No.036 猛撞 -> 使出全身的力气，向对方冲撞过去。自己也会受到一点伤害。
        /// </summary>
        public const int TAKE_DOWN = 36;
        /// <summary>
        /// No.037 横冲直撞 -> 在２ー３回合内，向对方进行狂暴的攻击。之后会陷入混乱。
        /// </summary>
        public const int THRASH = 37;
        /// <summary>
        /// No.038 舍身攻击 -> 舍身撞击对方。同时自己也会受到相当的伤害。
        /// </summary>
        public const int DOUBLEEDGE = 38;
        /// <summary>
        /// No.039 摇尾巴 -> 通过摇晃尾巴来迷惑对方。使对方防御降低。
        /// </summary>
        public const int TAIL_WHIP = 39;
        /// <summary>
        /// No.040 毒针 -> 用锋利的毒针攻击对方。有时会使对方中毒。
        /// </summary>
        public const int POISON_STING = 40;
        /// <summary>
        /// No.041 双针 -> 用锋利的双针连续攻击对方２次。有时会使对方中毒。
        /// </summary>
        public const int TWINEEDLE = 41;
        /// <summary>
        /// No.042 飞弹针 -> 朝向对方发射锐针攻击。连续攻击对方2-5次。
        /// </summary>
        public const int PIN_MISSILE = 42;
        /// <summary>
        /// No.043 瞪眼 -> 用锐利的眼神震慑对方使其防御降低。
        /// </summary>
        public const int LEER = 43;
        /// <summary>
        /// No.044 咬咬 -> 用锋利的牙齿攻击对方。有时会使对方胆怯。
        /// </summary>
        public const int BITE = 44;
        /// <summary>
        /// No.045 嚎叫 -> 发出可爱的叫声，让对方大意。降低对方的攻击。
        /// </summary>
        public const int GROWL = 45;
        /// <summary>
        /// No.046 吼叫 -> 让对方害怕逃跑，强制对方交换场上[宝可梦]。在野战时使用会直接结束战斗。
        /// </summary>
        public const int ROAR = 46;
        /// <summary>
        /// No.047 唱歌 -> 唱出轻柔的歌声，使对方[宝可梦]进入睡眠状态。
        /// </summary>
        public const int SING = 47;
        /// <summary>
        /// No.048 超音波 -> 从身体内发出特殊的声音，让对方混乱。
        /// </summary>
        public const int SUPERSONIC = 48;
        /// <summary>
        /// No.049 音爆 -> 向对方发射冲击波进行攻击。必定造成２０点伤害。
        /// </summary>
        public const int SONICBOOM = 49;
        /// <summary>
        /// No.050 石化功 -> 阻碍对方的行动，让它之前使用的技能在４回合内无法使用。
        /// </summary>
        public const int DISABLE = 50;
        /// <summary>
        /// No.051 溶解液 -> 向对方喷射强酸进行攻击。有时会降低对方的特防。
        /// </summary>
        public const int ACID = 51;
        /// <summary>
        /// No.052 火花 -> 用小型火焰攻击对方。有时会使对方烧伤。
        /// </summary>
        public const int EMBER = 52;
        /// <summary>
        /// No.053 喷射火焰 -> 用炙热火焰攻击对方。有时会使对方烧伤。
        /// </summary>
        public const int FLAMETHROWER = 53;
        /// <summary>
        /// No.054 白雾 -> 用白雾覆盖身体。在５回合之内不会让对方降低自己的能力。
        /// </summary>
        public const int MIST = 54;
        /// <summary>
        /// No.055 水枪 -> 用力喷水，攻击对方。
        /// </summary>
        public const int WATER_GUN = 55;
        /// <summary>
        /// No.056 水炮 -> 向对方喷射高压水柱进行攻击。
        /// </summary>
        public const int HYDRO_PUMP = 56;
        /// <summary>
        /// No.057 冲浪 -> 用大浪攻击自己周围所有单位。使用后可以在水上行动。
        /// </summary>
        public const int SURF = 57;
        /// <summary>
        /// No.058 急冻光线 -> 用极冷的光束攻击对方。有时会使对方结冰。
        /// </summary>
        public const int ICE_BEAM = 58;
        /// <summary>
        /// No.059 暴风雪 -> 用暴风雪攻击对方，有时会使对方结冰。
        /// </summary>
        public const int BLIZZARD = 59;
        /// <summary>
        /// No.060 幻象光 -> 发射不可思议的光线攻击对方。有时会使对方混乱。
        /// </summary>
        public const int PSYBEAM = 60;
        /// <summary>
        /// No.061 泡沫光线 -> 喷射大量的泡沫攻击对方。有时会降低对方的速度。
        /// </summary>
        public const int BUBBLEBEAM = 61;
        /// <summary>
        /// No.062 极光束 -> 发射彩色光线攻击对方。有时会降低对方的攻击。
        /// </summary>
        public const int AURORA_BEAM = 62;
        /// <summary>
        /// No.063 破坏死光 -> 放出强光攻击对方。自己下一回合会无法行动。
        /// </summary>
        public const int HYPER_BEAM = 63;
        /// <summary>
        /// No.064 啄 -> 用尖锐的喙或角刺击对方。
        /// </summary>
        public const int PECK = 64;
        /// <summary>
        /// No.065 冲钻 -> 用旋转的尖喙刺击对方。
        /// </summary>
        public const int DRILL_PECK = 65;
        /// <summary>
        /// No.066 地狱滚动 -> 让对方和自己一起摔到地面上造成伤害。自己也会受到一点伤害。
        /// </summary>
        public const int SUBMISSION = 66;
        /// <summary>
        /// No.067 下踢 -> 使劲踹对方的脚，让对方摔倒。对方越重造成的伤害就越大。
        /// </summary>
        public const int LOW_KICK = 67;
        /// <summary>
        /// No.068 返拳 -> 将本回合受到的物理攻击的伤害加倍奉还。
        /// </summary>
        public const int COUNTER = 68;
        /// <summary>
        /// No.069 地球上投 -> 利用引力把对方扔出去。造成和自己等级相同的伤害。
        /// </summary>
        public const int SEISMIC_TOSS = 69;
        /// <summary>
        /// No.070 劲力 -> 用尽全力攻击对方。还可以推动沉重的石头。
        /// </summary>
        public const int STRENGTH = 70;
        /// <summary>
        /// No.071 吸收 -> 吸取对方的养分。攻击造成的一半伤害会转化成自己的ＨＰ。
        /// </summary>
        public const int ABSORB = 71;
        /// <summary>
        /// No.072 百万吨吸收 -> 吸取对方的养分。攻击造成的一半伤害会转化成自己的ＨＰ。
        /// </summary>
        public const int MEGA_DRAIN = 72;
        /// <summary>
        /// No.073 寄生种子 -> 把种子植入对方体内吸取ＨＰ。每回合回复自身ＨＰ。
        /// </summary>
        public const int LEECH_SEED = 73;
        /// <summary>
        /// No.074 生长 -> 加速身体成长，提高自身的攻击和特攻。
        /// </summary>
        public const int GROWTH = 74;
        /// <summary>
        /// No.075 飞叶快刀 -> 使用叶片攻击对方。容易打中对方的要害。
        /// </summary>
        public const int RAZOR_LEAF = 75;
        /// <summary>
        /// No.076 阳光烈焰 -> 第１回合储蓄能量，第２回合发射光线攻击对方。
        /// </summary>
        public const int SOLARBEAM = 76;
        /// <summary>
        /// No.077 毒粉末 -> 将毒粉撒向对方使其中毒。
        /// </summary>
        public const int POISONPOWDER = 77;
        /// <summary>
        /// No.078 麻痹粉 -> 将麻痹粉撒向对方使其麻痹。
        /// </summary>
        public const int STUN_SPORE = 78;
        /// <summary>
        /// No.079 睡眠粉 -> 将催眠粉撒向对方使其入睡。
        /// </summary>
        public const int SLEEP_POWDER = 79;
        /// <summary>
        /// No.080 花之舞 -> 散落花瓣进行２－３回合攻击。之后会混乱。
        /// </summary>
        public const int PETAL_DANCE = 80;
        /// <summary>
        /// No.081 吐丝 -> 从口中吐出丝，降低对方的速度。
        /// </summary>
        public const int STRING_SHOT = 81;
        /// <summary>
        /// No.082 龙之怒 -> 用愤怒的冲击波攻击对方。伤害值固定为４０。
        /// </summary>
        public const int DRAGON_RAGE = 82;
        /// <summary>
        /// No.083 火焰漩涡 -> 将对方关在火焰旋涡中，造成４－５回合的伤害。
        /// </summary>
        public const int FIRE_SPIN = 83;
        /// <summary>
        /// No.084 电击 -> 用电流攻击对方。有时会使对方麻痹。
        /// </summary>
        public const int THUNDERSHOCK = 84;
        /// <summary>
        /// No.085 十万伏特 -> 用强大的电流攻击对方。有时会使对方麻痹。
        /// </summary>
        public const int THUNDERBOLT = 85;
        /// <summary>
        /// No.086 电磁波 -> 用微弱的电击使对方麻痹。
        /// </summary>
        public const int THUNDER_WAVE = 86;
        /// <summary>
        /// No.087 打雷 -> 使用强大的雷暴攻击对方。有时会使对方麻痹。
        /// </summary>
        public const int THUNDER = 87;
        /// <summary>
        /// No.088 滚石 -> 向对方扔出石块进行攻击。
        /// </summary>
        public const int ROCK_THROW = 88;
        /// <summary>
        /// No.089 地震 -> 引发地震，攻击周围的所有人。
        /// </summary>
        public const int EARTHQUAKE = 89;
        /// <summary>
        /// No.090 地裂 -> 引发地裂攻击对方。如果打中就会使对方濒死。
        /// </summary>
        public const int FISSURE = 90;
        /// <summary>
        /// No.091 挖地洞 -> 第１回合钻入地下，第２回合攻击对方。可以用于脱离洞穴。
        /// </summary>
        public const int DIG = 91;
        /// <summary>
        /// No.092 猛毒素 -> 让对方中剧毒。随着回合的推进，中毒伤害也会增加。
        /// </summary>
        public const int TOXIC = 92;
        /// <summary>
        /// No.093 念力 -> 用微弱的念力攻击对方。有时会使对方混乱。
        /// </summary>
        public const int CONFUSION = 93;
        /// <summary>
        /// No.094 幻象术 -> 用强大的念力攻击对方。有时会降低对方的特防。
        /// </summary>
        public const int PSYCHIC = 94;
        /// <summary>
        /// No.095 催眠术 -> 催眠对方，使其进入睡眠状态。
        /// </summary>
        public const int HYPNOSIS = 95;
        /// <summary>
        /// No.096 瑜珈姿势 -> 使出沉睡在自身体内的力量。提升自己的攻击。
        /// </summary>
        public const int MEDITATE = 96;
        /// <summary>
        /// No.097 高速移动 -> 放松身体，提高自身运动速度。大幅提升自身的速度。
        /// </summary>
        public const int AGILITY = 97;
        /// <summary>
        /// No.098 电光一闪 -> 高速接近对方进行攻击。必定能够先制攻击。
        /// </summary>
        public const int QUICK_ATTACK = 98;
        /// <summary>
        /// No.099 愤怒 -> 技能发出时受到攻击的话，会因为愤怒，而提高自身的攻击力。
        /// </summary>
        public const int RAGE = 99;
        /// <summary>
        /// No.100 瞬间移动 -> 在野外使用会直接结束战斗。也可以回到最后去过的有[宝可梦]中心的城镇。
        /// </summary>
        public const int TELEPORT = 100;
        /// <summary>
        /// No.101 黑夜诅咒 -> 制造幻影，给予对方和自己等级一样的伤害。
        /// </summary>
        public const int NIGHT_SHADE = 101;
        /// <summary>
        /// No.102 模仿 -> 能在战斗中记下对方使用过的技能给自己使用。
        /// </summary>
        public const int MIMIC = 102;
        /// <summary>
        /// No.103 噪音 -> 发出难以忍受的刺耳的声音，大幅降低对方的防御。
        /// </summary>
        public const int SCREECH = 103;
        /// <summary>
        /// No.104 影子分身 -> 用高速移动制造分身迷惑对方。提升回避率。
        /// </summary>
        public const int DOUBLE_TEAM = 104;
        /// <summary>
        /// No.105 自我复原 -> 让全身的细胞获得再生，回复一半ＨＰ。
        /// </summary>
        public const int RECOVER = 105;
        /// <summary>
        /// No.106 硬梆梆 -> 使出全身力气，让自己变硬，提升自身的防御。
        /// </summary>
        public const int HARDEN = 106;
        /// <summary>
        /// No.107 缩小 -> 缩小自己的身体，大幅提升自身的回避率。
        /// </summary>
        public const int MINIMIZE = 107;
        /// <summary>
        /// No.108 烟幕 -> 用烟或者墨汁迷惑对方，降低对方的命中率。
        /// </summary>
        public const int SMOKESCREEN = 108;
        /// <summary>
        /// No.109 奇异光线 -> 将奇怪的光线射向对方，使对方进入混乱状态。
        /// </summary>
        public const int CONFUSE_RAY = 109;
        /// <summary>
        /// No.110 缩壳 -> 缩进壳里保护身体，提升自身的防御。
        /// </summary>
        public const int WITHDRAW = 110;
        /// <summary>
        /// No.111 防卫卷 -> 使身体变圆，提升自身的防御。
        /// </summary>
        public const int DEFENSE_CURL = 111;
        /// <summary>
        /// No.112 障碍 -> 制造坚固的障壁，大幅提高自身的防御。
        /// </summary>
        public const int BARRIER = 112;
        /// <summary>
        /// No.113 光墙 -> 制造出神奇的墙，在５回合之内降低特殊攻击的伤害。
        /// </summary>
        public const int LIGHT_SCREEN = 113;
        /// <summary>
        /// No.114 黑雾 -> 发出黑色的雾气，复原全场[宝可梦]的能力变化。
        /// </summary>
        public const int HAZE = 114;
        /// <summary>
        /// No.115 减半反射 -> 制造出神奇的墙，在５回合之内降低物理攻击的伤害。
        /// </summary>
        public const int REFLECT = 115;
        /// <summary>
        /// No.116 集气 -> 凝神聚气，让自己的攻击更容易命中对方要害。
        /// </summary>
        public const int FOCUS_ENERGY = 116;
        /// <summary>
        /// No.117 忍忍 -> 两回合内受到的伤害两倍返还给对方。
        /// </summary>
        public const int BIDE = 117;
        /// <summary>
        /// No.118 挥指功 -> 摇动手指刺激自己的大脑，能够从所有的技能中随机使出一个来攻击对方。
        /// </summary>
        public const int METRONOME = 118;
        /// <summary>
        /// No.119 学舌术 -> 模仿对方最后使用的技能作为自己的技能使用。
        /// </summary>
        public const int MIRROR_MOVE = 119;
        /// <summary>
        /// No.120 自爆 -> 爆炸攻击自身周围的所有对象。使用后自身进入濒死状态。
        /// </summary>
        public const int SELFDESTRUCT = 120;
        /// <summary>
        /// No.121 炸蛋 -> 向对方投掷巨大的蛋进行攻击。
        /// </summary>
        public const int EGG_BOMB = 121;
        /// <summary>
        /// No.122 舔舌头 -> 用长舌头舔对方进行攻击。有时会使对方麻痹。
        /// </summary>
        public const int LICK = 122;
        /// <summary>
        /// No.123 迷雾 -> 放出毒气攻击对方。有时会使对方中毒。
        /// </summary>
        public const int SMOG = 123;
        /// <summary>
        /// No.124 泥浆攻击 -> 投掷淤泥攻击对方。有时会使对方中毒。
        /// </summary>
        public const int SLUDGE = 124;
        /// <summary>
        /// No.125 骨棒 -> 用手中的骨头殴打对方。有时会使对方害怕。
        /// </summary>
        public const int BONE_CLUB = 125;
        /// <summary>
        /// No.126 大字爆 -> 用爆成大字的火焰灼烧对方。有时会使对方烧伤。
        /// </summary>
        public const int FIRE_BLAST = 126;
        /// <summary>
        /// No.127 鱼跃龙门 -> 以猛烈的势头向对方冲过去，有时会使对方害怕。用它能够登上瀑布。
        /// </summary>
        public const int WATERFALL = 127;
        /// <summary>
        /// No.128 夹壳 -> 用非常坚固的壳将对方夹住４－５回合造成伤害。
        /// </summary>
        public const int CLAMP = 128;
        /// <summary>
        /// No.129 高速星星 -> 发射星型的光攻击对方。攻击必定会命中。
        /// </summary>
        public const int SWIFT = 129;
        /// <summary>
        /// No.130 火箭头槌 -> 第一回合把头缩进去，提升防御力。第２回合攻击对方。
        /// </summary>
        public const int SKULL_BASH = 130;
        /// <summary>
        /// No.131 尖刺加农炮 -> 用锐利的针刺连续攻击对方２－５次。
        /// </summary>
        public const int SPIKE_CANNON = 131;
        /// <summary>
        /// No.132 纠缠 -> 用触手或藤蔓攻击对方。有时会降低对方的速度。
        /// </summary>
        public const int CONSTRICT = 132;
        /// <summary>
        /// No.133 瞬间失忆 -> 将头脑清空，瞬间忘记刚才发生的事。大幅提升自身的特防。
        /// </summary>
        public const int AMNESIA = 133;
        /// <summary>
        /// No.134 折弯汤匙 -> 弯曲匙子吸引对方的注意力，使其命中率下降。
        /// </summary>
        public const int KINESIS = 134;
        /// <summary>
        /// No.135 生蛋 -> 回复自身最大ＨＰ的一半。也可以将ＨＰ分给队友。
        /// </summary>
        public const int SOFTBOILED = 135;
        /// <summary>
        /// No.136 飞膝撞 -> 高高跳起向下踢击对方，踢偏的话自身会受到伤害。
        /// </summary>
        public const int HI_JUMP_KICK = 136;
        /// <summary>
        /// No.137 大蛇瞪眼 -> 用腹部的花纹吓对方，让对方处于麻痹状态。
        /// </summary>
        public const int GLARE = 137;
        /// <summary>
        /// No.138 食梦 -> 吃掉正在睡觉的对方的梦。造成伤害的一半能够作为ＨＰ被自己吸收。
        /// </summary>
        public const int DREAM_EATER = 138;
        /// <summary>
        /// No.139 毒瓦斯 -> 向对方吹出毒气使其中毒。
        /// </summary>
        public const int POISON_GAS = 139;
        /// <summary>
        /// No.140 丢球 -> 投出圆形物体，连续攻击对方２ー５次。
        /// </summary>
        public const int BARRAGE = 140;
        /// <summary>
        /// No.141 吸血 -> 吸取对方的血。造成伤害的一半，用来回复自身ＨＰ。
        /// </summary>
        public const int LEECH_LIFE = 141;
        /// <summary>
        /// No.142 恶魔之吻 -> 用恐怖的脸亲吻对方，让其受惊昏睡。
        /// </summary>
        public const int LOVELY_KISS = 142;
        /// <summary>
        /// No.143 高空攻击 -> 在第２回合攻击对方，偶尔会使对方害怕容易命中要害。
        /// </summary>
        public const int SKY_ATTACK = 143;
        /// <summary>
        /// No.144 变身 -> 变身成对方[宝可梦]的样子。能够使用和对方完全相同的技能。
        /// </summary>
        public const int TRANSFORM = 144;
        /// <summary>
        /// No.145 泡泡 -> 发出无数的泡沫攻击对方。有时会使对方的速度。
        /// </summary>
        public const int BUBBLE = 145;
        /// <summary>
        /// No.146 迷昏拳 -> 有节奏地出拳攻击。有时会让对方混乱。
        /// </summary>
        public const int DIZZY_PUNCH = 146;
        /// <summary>
        /// No.147 蘑菇孢子 -> 释放出催眠孢子，让对方进入睡眠状态。
        /// </summary>
        public const int SPORE = 147;
        /// <summary>
        /// No.148 闪光 -> 发出强烈闪光，降低对方命中率。可照亮洞穴。
        /// </summary>
        public const int FLASH = 148;
        /// <summary>
        /// No.149 幻象波 -> 向对方发射神秘的念波，每次使用伤害都会变动。
        /// </summary>
        public const int PSYWAVE = 149;
        /// <summary>
        /// No.150 水溅跃 -> 毫无攻击力地乱跳。似乎什么都不会发生……
        /// </summary>
        public const int SPLASH = 150;
        /// <summary>
        /// No.151 溶化 -> 将细胞变成液态，大幅提升自身的防御。
        /// </summary>
        public const int ACID_ARMOR = 151;
        /// <summary>
        /// No.152 螃蟹拳 -> 用巨大的钳子敲击对方。容易打中要害。
        /// </summary>
        public const int CRABHAMMER = 152;
        /// <summary>
        /// No.153 大爆炸 -> 引发大爆炸杀伤全体。之后会濒死。
        /// </summary>
        public const int EXPLOSION = 153;
        /// <summary>
        /// No.154 疯狂乱抓 -> 用爪子或者镰刀连续攻击对方２ー５次。
        /// </summary>
        public const int FURY_SWIPES = 154;
        /// <summary>
        /// No.155 骨头回力镖 -> 用手中的骨头攻击对方。来回一共会造成两次伤害。
        /// </summary>
        public const int BONEMERANG = 155;
        /// <summary>
        /// No.156 睡觉 -> 睡上两个回合。能够回复全部ＨＰ，且清除异常状态。
        /// </summary>
        public const int REST = 156;
        /// <summary>
        /// No.157 山崩地裂 -> 用力将巨石砸向对方。有时会使对方害怕。
        /// </summary>
        public const int ROCK_SLIDE = 157;
        /// <summary>
        /// No.158 必杀门牙 -> 用锐利的牙齿攻击对方，有时会使对方害怕。
        /// </summary>
        public const int HYPER_FANG = 158;
        /// <summary>
        /// No.159 棱角 -> 增加身体的棱角，使自身的攻击获得提升。
        /// </summary>
        public const int SHARPEN = 159;
        /// <summary>
        /// No.160 变性 -> 自身的属性会变成与自己所学技能里其中一招的相同属性。
        /// </summary>
        public const int CONVERSION = 160;
        /// <summary>
        /// No.161 三角攻击 -> 发射混合光线攻击。有时会使对方进入麻痹、烧伤、结冰三种状态中的任意一种状态。
        /// </summary>
        public const int TRI_ATTACK = 161;
        /// <summary>
        /// No.162 愤怒之牙 -> 利用锐利的牙齿猛力啃咬。对方的当前ＨＰ减半。
        /// </summary>
        public const int SUPER_FANG = 162;
        /// <summary>
        /// No.163 劈开 -> 用利爪或镰刀等切裂对方。容易命中要害。
        /// </summary>
        public const int SLASH = 163;
        /// <summary>
        /// No.164 替身 -> 消耗一点ＨＰ来制造出替身。替身能够抵挡攻击。
        /// </summary>
        public const int SUBSTITUTE = 164;
        /// <summary>
        /// No.165 挣扎 -> 在ＰＰ耗尽后，使出所有剩余力量攻击的技能。自己也会受到一点伤害。
        /// </summary>
        public const int STRUGGLE = 165;
        /// <summary>
        /// No.166 写生 -> 复制对方使用的技能。使用１次后素描就会消失。
        /// </summary>
        public const int SKETCH = 166;
        /// <summary>
        /// No.167 三倍足攻 -> 连续３次飞踢，在每次命中的时候威力都会提升。
        /// </summary>
        public const int TRIPLE_KICK = 167;
        /// <summary>
        /// No.168 小偷 -> 攻击并偷窃道具。自己持有道具时，无法使用此技能。
        /// </summary>
        public const int THIEF = 168;
        /// <summary>
        /// No.169 蛛丝 -> 吐出线缠住对方使其不能逃脱。
        /// </summary>
        public const int SPIDER_WEB = 169;
        /// <summary>
        /// No.170 心眼 -> 用心感受对方的行动方式。下一次攻击必中。
        /// </summary>
        public const int MIND_READER = 170;
        /// <summary>
        /// No.171 恶梦 -> 让在睡眠中的对方做恶梦，每回合会减少１/４ＨＰ。
        /// </summary>
        public const int NIGHTMARE = 171;
        /// <summary>
        /// No.172 火焰轮 -> 用火焰缠绕身体向对方进行突击。有时会使对方烧伤。
        /// </summary>
        public const int FLAME_WHEEL = 172;
        /// <summary>
        /// No.173 打鼾 -> 在自己睡觉的时候发出鼾声攻击。有时会使对方害怕
        /// </summary>
        public const int SNORE = 173;
        /// <summary>
        /// No.174 咒语 -> 使用的效果取决于使用者是不是幽灵系。
        /// </summary>
        public const int CURSE = 174;
        /// <summary>
        /// No.175 手足慌乱 -> 拼命挣扎攻击对方。自身ＨＰ越少，技能的威力越大。
        /// </summary>
        public const int FLAIL = 175;
        /// <summary>
        /// No.176 变性２ -> 可以使自身属性变为对对方最后一次使用的技能有抗性的属性。
        /// </summary>
        public const int CONVERSION_2 = 176;
        /// <summary>
        /// No.177 空气爆炸 -> 制造空气漩涡攻击对方。容易命中要害。
        /// </summary>
        public const int AEROBLAST = 177;
        /// <summary>
        /// No.178 棉孢子 -> 用散发出去的孢子骚扰对方。大幅降低对方的速度。
        /// </summary>
        public const int COTTON_SPORE = 178;
        /// <summary>
        /// No.179 起死回生 -> 用上最后的力量攻击对方。自己的ＨＰ越少，技能的威力越大。
        /// </summary>
        public const int REVERSAL = 179;
        /// <summary>
        /// No.180 痛恨 -> 向对方最后使用的技能注入怨念。使该技能消耗４ＰＰ。
        /// </summary>
        public const int SPITE = 180;
        /// <summary>
        /// No.181 细雪 -> 用寒冷的冰雪攻击对方。有时会使对方结冰。
        /// </summary>
        public const int POWDER_SNOW = 181;
        /// <summary>
        /// No.182 守住 -> 完全抵挡对方的攻击。连续使用成功率会逐渐降低。
        /// </summary>
        public const int PROTECT = 182;
        /// <summary>
        /// No.183 音速拳 -> 用眼睛无法跟上的速度出拳攻击对方。必定能够先制攻击。
        /// </summary>
        public const int MACH_PUNCH = 183;
        /// <summary>
        /// No.184 鬼脸 -> 用可怕的脸瞪着对方，使其害怕。大幅降低对方的速度。
        /// </summary>
        public const int SCARY_FACE = 184;
        /// <summary>
        /// No.185 虚晃一招 -> 趁对方不注意接近然后发动攻击。攻击必定会命中。
        /// </summary>
        public const int FAINT_ATTACK = 185;
        /// <summary>
        /// No.186 天使之吻 -> 像天使般亲吻对方，让对方混乱。
        /// </summary>
        public const int SWEET_KISS = 186;
        /// <summary>
        /// No.187 肚子大鼓 -> 自身ＨＰ减少到满血的一半，攻击力提升到最大。
        /// </summary>
        public const int BELLY_DRUM = 187;
        /// <summary>
        /// No.188 臭泥爆弹 -> 向对方扔出肮脏的淤泥。有时会使对方中毒。
        /// </summary>
        public const int SLUDGE_BOMB = 188;
        /// <summary>
        /// No.189 泥汤 -> 向对方的脸投掷泥块攻击，降低对方的命中率。
        /// </summary>
        public const int MUDSLAP = 189;
        /// <summary>
        /// No.190 章鱼桶炮 -> 向对方吐出大量墨汁攻击。有时会降低对方的命中率。
        /// </summary>
        public const int OCTAZOOKA = 190;
        /// <summary>
        /// No.191 满地星 -> 向对方的周围撒菱。交替出场的[宝可梦]会受到伤害。
        /// </summary>
        public const int SPIKES = 191;
        /// <summary>
        /// No.192 电磁炮 -> 释放出大炮一样的电流攻击对方。有时会使对方麻痹。
        /// </summary>
        public const int ZAP_CANNON = 192;
        /// <summary>
        /// No.193 看破 -> 使对幽灵系没有效果的技能能够打中幽灵系。还能够命中回避率高的对方。
        /// </summary>
        public const int FORESIGHT = 193;
        /// <summary>
        /// No.194 同命 -> 使用后被对方击倒的话，对方也会一同死亡。
        /// </summary>
        public const int DESTINY_BOND = 194;
        /// <summary>
        /// No.195 灭亡之歌 -> 唱起死亡之歌，听到的[宝可梦]３回合后会濒死。离场的话效果解除。
        /// </summary>
        public const int PERISH_SONG = 195;
        /// <summary>
        /// No.196 冻风 -> 放出冷气攻击对方，降低对方的敏速度
        /// </summary>
        public const int ICY_WIND = 196;
        /// <summary>
        /// No.197 先决 -> 完全抵挡对方的攻击。连续使用成功率会逐渐降低。
        /// </summary>
        public const int DETECT = 197;
        /// <summary>
        /// No.198 骨击一气 -> 用坚硬的骨头连续攻击对方２－５次。
        /// </summary>
        public const int BONE_RUSH = 198;
        /// <summary>
        /// No.199 锁定 -> 锁定目标，使下次的攻击一定能够命中对方。
        /// </summary>
        public const int LOCKON = 199;
        /// <summary>
        /// No.200 龙鳞之怒 -> 在２ー３回合间进行狂暴的攻击。之后自己会陷入混乱。
        /// </summary>
        public const int OUTRAGE = 200;
        /// <summary>
        /// No.201 沙雹 -> ５回合刮起沙尘暴，除岩石系、地上系和钢系之外的[宝可梦]会受到伤害。
        /// </summary>
        public const int SANDSTORM = 201;
        /// <summary>
        /// No.202 超级吸收 -> 吸取养分的攻击。伤害的一半转化成自己的ＨＰ。
        /// </summary>
        public const int GIGA_DRAIN = 202;
        /// <summary>
        /// No.203 忍耐 -> 受到致命攻击后必定会留下１ＨＰ。连续使用成功率会降低。
        /// </summary>
        public const int ENDURE = 203;
        /// <summary>
        /// No.204 撒娇 -> 向对方撒娇，大幅降低对方的攻击。
        /// </summary>
        public const int CHARM = 204;
        /// <summary>
        /// No.205 滚动 -> 连续５回合内滚动攻击。每次命中的威力都会提升。
        /// </summary>
        public const int ROLLOUT = 205;
        /// <summary>
        /// No.206 刀背击打 -> 手下留情的攻击。至少会让对方剩下１ＨＰ。
        /// </summary>
        public const int FALSE_SWIPE = 206;
        /// <summary>
        /// No.207 装腔作势 -> 激怒对方并使其混乱。对方的攻击会大幅提升。
        /// </summary>
        public const int SWAGGER = 207;
        /// <summary>
        /// No.208 饮奶 -> 回复自身最大ＨＰ的一半。也可以将ＨＰ分给队友。
        /// </summary>
        public const int MILK_DRINK = 208;
        /// <summary>
        /// No.209 闪电 -> 用电气缠绕身体向对方进行突击。有时会使对方麻痹。
        /// </summary>
        public const int SPARK = 209;
        /// <summary>
        /// No.210 连切 -> 用利刃攻击对方。连续击中对方的话，威力会上升。
        /// </summary>
        public const int FURY_CUTTER = 210;
        /// <summary>
        /// No.211 钢翼 -> 用坚硬的翅膀攻击对方。有时会提升自身的防御。
        /// </summary>
        public const int STEEL_WING = 211;
        /// <summary>
        /// No.212 黑色眼光 -> 用漆黑深邃的眼神注视对方，使其无法逃脱。
        /// </summary>
        public const int MEAN_LOOK = 212;
        /// <summary>
        /// No.213 迷人 -> 诱惑异性，让对方不知所措。对方将很难使出技能来。
        /// </summary>
        public const int ATTRACT = 213;
        /// <summary>
        /// No.214 梦话 -> 随机使出自己的技能，只能在睡觉的时候用。
        /// </summary>
        public const int SLEEP_TALK = 214;
        /// <summary>
        /// No.215 治愈铃声 -> 治愈心灵的铃音。可恢复我方全队的异常状态。
        /// </summary>
        public const int HEAL_BELL = 215;
        /// <summary>
        /// No.216 报恩 -> 为了训练师用全力攻击对方。亲密度越高威力越大。
        /// </summary>
        public const int RETURN = 216;
        /// <summary>
        /// No.217 礼物 -> 丢出神秘的礼物。可能是炸弹，也可能是回复药。
        /// </summary>
        public const int PRESENT = 217;
        /// <summary>
        /// No.218 牵连 -> 发泄不满的攻击，亲密度越低威力越大。
        /// </summary>
        public const int FRUSTRATION = 218;
        /// <summary>
        /// No.219 神秘护身 -> ５回合内被神奇的力量包围。不会陷入异常状态。
        /// </summary>
        public const int SAFEGUARD = 219;
        /// <summary>
        /// No.220 分享痛楚 -> 把自己的ＨＰ和对方的ＨＰ加起来平分。
        /// </summary>
        public const int PAIN_SPLIT = 220;
        /// <summary>
        /// No.221 圣之火 -> 用神秘的火焰烧尽对方。有时会使对方烧伤。
        /// </summary>
        public const int SACRED_FIRE = 221;
        /// <summary>
        /// No.222 震级 -> 震动坚固的地面攻击全场所有对象。技能威力会随机改变。
        /// </summary>
        public const int MAGNITUDE = 222;
        /// <summary>
        /// No.223 爆裂拳 -> 用尽全力，击出爆裂之拳。对方必定混乱。
        /// </summary>
        public const int DYNAMICPUNCH = 223;
        /// <summary>
        /// No.224 百万吨角击 -> 用致命的大角狠狠地突刺对方。
        /// </summary>
        public const int MEGAHORN = 224;
        /// <summary>
        /// No.225 龙吸 -> 吐出强烈的气息攻击对方。有时会使对方麻痹。
        /// </summary>
        public const int DRAGONBREATH = 225;
        /// <summary>
        /// No.226 接棒 -> 和待机的[宝可梦]进行交换。能力的变化也会继承给那个[宝可梦]。
        /// </summary>
        public const int BATON_PASS = 226;
        /// <summary>
        /// No.227 再来一次 -> 让对方在３回合内只能使用最后使用过的技能。
        /// </summary>
        public const int ENCORE = 227;
        /// <summary>
        /// No.228 追打 -> 在对方交换[宝可梦]的时候使用的话，技能的威力会变为原来的两倍。
        /// </summary>
        public const int PURSUIT = 228;
        /// <summary>
        /// No.229 高速回转 -> 高速旋转攻击对方。可以摆脱捆紧、缠绕、寄生种子和撒菱等障碍。
        /// </summary>
        public const int RAPID_SPIN = 229;
        /// <summary>
        /// No.230 甜气 -> 用香气降低对方的回避率。在草丛使用的话会出现[宝可梦]。
        /// </summary>
        public const int SWEET_SCENT = 230;
        /// <summary>
        /// No.231 铁尾 -> 用坚硬的尾巴攻击对方。有时会降低对方的防御。
        /// </summary>
        public const int IRON_TAIL = 231;
        /// <summary>
        /// No.232 合金爪 -> 用钢铁之爪攻击对方。有时会提升自身的攻击。
        /// </summary>
        public const int METAL_CLAW = 232;
        /// <summary>
        /// No.233 一招制敌 -> 会在对方之后攻击，但是自己的攻击必定会命中。
        /// </summary>
        public const int VITAL_THROW = 233;
        /// <summary>
        /// No.234 晨光 -> 回复自己的ＨＰ。根据天气的变化回复量也会改变。
        /// </summary>
        public const int MORNING_SUN = 234;
        /// <summary>
        /// No.235 光学合成 -> 回复自己的ＨＰ。根据天气的变化恢复量也会改变。
        /// </summary>
        public const int SYNTHESIS = 235;
        /// <summary>
        /// No.236 月光 -> 回复自己的ＨＰ。根据天气的变化，ＨＰ回复量也会改变。
        /// </summary>
        public const int MOONLIGHT = 236;
        /// <summary>
        /// No.237 催醒 -> 根据[宝可梦]的潜力，技能的属性和威力会有变化。
        /// </summary>
        public const int HIDDEN_POWER = 237;
        /// <summary>
        /// No.238 十字切 -> 交叉绞杀对方进行攻击。容易命中要害。
        /// </summary>
        public const int CROSS_CHOP = 238;
        /// <summary>
        /// No.239 龙卷风 -> 制造旋风将对方卷入进行攻击。有时会使对方胆怯。
        /// </summary>
        public const int TWISTER = 239;
        /// <summary>
        /// No.240 乞雨 -> ５回合内会降雨。水系的技能威力会提升。
        /// </summary>
        public const int RAIN_DANCE = 240;
        /// <summary>
        /// No.241 大晴天 -> ５回合内放晴。炎系的技能威力会提升。
        /// </summary>
        public const int SUNNY_DAY = 241;
        /// <summary>
        /// No.242 咬碎 -> 用利牙撕咬对方进行攻击。有时会使对方的防御下降。
        /// </summary>
        public const int CRUNCH = 242;
        /// <summary>
        /// No.243 表面涂层 -> 将本回合受到的特殊攻击的伤害两倍返还给对方。
        /// </summary>
        public const int MIRROR_COAT = 243;
        /// <summary>
        /// No.244 自我暗示 -> 施以自我暗示，让能力等级变化和对方一样。
        /// </summary>
        public const int PSYCH_UP = 244;
        /// <summary>
        /// No.245 神速 -> 用眼睛无法跟上的速度向对方进行突击。必定能够先制攻击。
        /// </summary>
        public const int EXTREMESPEED = 245;
        /// <summary>
        /// No.246 原始之力 -> 用原始的力量攻击对方。有时会提升自身所有的能力。
        /// </summary>
        public const int ANCIENTPOWER = 246;
        /// <summary>
        /// No.247 影子球 -> 扔出一团黑影攻击对方。有时会降低对方的特防。
        /// </summary>
        public const int SHADOW_BALL = 247;
        /// <summary>
        /// No.248 先知 -> 使用技能后经过２回合，释放念力攻击。
        /// </summary>
        public const int FUTURE_SIGHT = 248;
        /// <summary>
        /// No.249 岩石粉碎 -> 用能击碎石头的拳头攻击对方。有时会使对方的防御下降。
        /// </summary>
        public const int ROCK_SMASH = 249;
        /// <summary>
        /// No.250 潮旋 -> 将对方关在水流的漩涡中，造成４－５回合的伤害。
        /// </summary>
        public const int WHIRLPOOL = 250;
        /// <summary>
        /// No.251 痛打一气 -> 全员出动。我方的[宝可梦]越多技能的攻击次数越多。
        /// </summary>
        public const int BEAT_UP = 251;
        /// <summary>
        /// No.252 假动作 -> 利用先制攻击让对方胆怯。在战斗开始时立刻使用才能成功。
        /// </summary>
        public const int FAKE_OUT = 252;
        /// <summary>
        /// No.253 吵闹 -> ３回合内用噪声攻击对方。期间全场不能睡眠。
        /// </summary>
        public const int UPROAR = 253;
        /// <summary>
        /// No.254 储存 -> 储存力量使自己的防御和特防上升。最多可以使用３次。
        /// </summary>
        public const int STOCKPILE = 254;
        /// <summary>
        /// No.255 喷出 -> 积蓄力量攻击对方。储存得越多威力越大。
        /// </summary>
        public const int SPIT_UP = 255;
        /// <summary>
        /// No.256 吞下 -> 用积蓄的力量回复自己的ＨＰ。积蓄得越多回复量越大。
        /// </summary>
        public const int SWALLOW = 256;
        /// <summary>
        /// No.257 热风 -> 向对方吹出灼热的风进行攻击。有时会使对方烧伤。
        /// </summary>
        public const int HEAT_WAVE = 257;
        /// <summary>
        /// No.258 冰雹 -> 在５回合内降下冰雹，冰系以外的[宝可梦]都会受到伤害。
        /// </summary>
        public const int HAIL = 258;
        /// <summary>
        /// No.259 假指控 -> 挑拨对方，令其不能连续使用同样的技能。
        /// </summary>
        public const int TORMENT = 259;
        /// <summary>
        /// No.260 煽惑 -> 让对方混乱，同时对方的特攻也会提升。
        /// </summary>
        public const int FLATTER = 260;
        /// <summary>
        /// No.261 鬼火 -> 放出怪异的火使对方被烧伤。
        /// </summary>
        public const int WILLOWISP = 261;
        /// <summary>
        /// No.262 临别礼物 -> 使用后自己陷入濒死，但对方的攻击和特攻也会大幅下降。
        /// </summary>
        public const int MEMENTO = 262;
        /// <summary>
        /// No.263 假勇敢 -> 自己处于中毒、烧伤、和麻痹状态的时候，技能的威力会变为２倍。
        /// </summary>
        public const int FACADE = 263;
        /// <summary>
        /// No.264 集中猛击 -> 先聚气再攻击，在出招前受到攻击的话会失败。
        /// </summary>
        public const int FOCUS_PUNCH = 264;
        /// <summary>
        /// No.265 苏醒 -> 对麻痹的对方使用，威力会变为２倍。但是对方会解除麻痹。
        /// </summary>
        public const int SMELLINGSALT = 265;
        /// <summary>
        /// No.266 跟我来 -> 吸引全场的注意力，将对方的攻击全部转移到自己身上。
        /// </summary>
        public const int FOLLOW_ME = 266;
        /// <summary>
        /// No.267 自然能力 -> 用自然之力攻击。根据场所的不同会变化成不同的技能。
        /// </summary>
        public const int NATURE_POWER = 267;
        /// <summary>
        /// No.268 充电 -> 下一回合使用的电系技能威力上升。自己的特防也会提升。
        /// </summary>
        public const int CHARGE = 268;
        /// <summary>
        /// No.269 挑拨 -> 挑拨对方，使其在３回合之内只能用攻击技能。
        /// </summary>
        public const int TAUNT = 269;
        /// <summary>
        /// No.270 帮助 -> 支援同伴，使其攻击技能的威力获得提升。
        /// </summary>
        public const int HELPING_HAND = 270;
        /// <summary>
        /// No.271 骗术 -> 用捉住对方的空隙，交换对方和自己的道具。
        /// </summary>
        public const int TRICK = 271;
        /// <summary>
        /// No.272 角色扮演 -> 变成和对方相同的特性。
        /// </summary>
        public const int ROLE_PLAY = 272;
        /// <summary>
        /// No.273 祈求 -> 在下一回合，回复自身最大ＨＰ的一半。
        /// </summary>
        public const int WISH = 273;
        /// <summary>
        /// No.274 协助 -> 紧急借助队友的力量，从的同伴那里随机使出一个技能来。
        /// </summary>
        public const int ASSIST = 274;
        /// <summary>
        /// No.275 根深蒂固 -> 在大地上扎根每回合恢复ＨＰ。在扎根期间不能交换[宝可梦]。
        /// </summary>
        public const int INGRAIN = 275;
        /// <summary>
        /// No.276 蛮力 -> 用野蛮的力量攻击对方。使用后自己的攻击和防御会下降。
        /// </summary>
        public const int SUPERPOWER = 276;
        /// <summary>
        /// No.277 魔术外衣 -> 反弹对方使用的异常状态等干扰技能。
        /// </summary>
        public const int MAGIC_COAT = 277;
        /// <summary>
        /// No.278 回收 -> 回收在战斗中已经消耗掉的道具，使之能再次使用。
        /// </summary>
        public const int RECYCLE = 278;
        /// <summary>
        /// No.279 报仇 -> 出招前被对方打中的话，技能威力会变为２倍。
        /// </summary>
        public const int REVENGE = 279;
        /// <summary>
        /// No.280 劈瓦 -> 用手刀进行致命一击，可以击碎光之壁和反射盾。
        /// </summary>
        public const int BRICK_BREAK = 280;
        /// <summary>
        /// No.281 哈欠 -> 打哈欠引起睡意。下回合对手会进入睡眠状态。
        /// </summary>
        public const int YAWN = 281;
        /// <summary>
        /// No.282 落拳 -> 封住对方的物品，在战斗结束前不能使用。
        /// </summary>
        public const int KNOCK_OFF = 282;
        /// <summary>
        /// No.283 强攻 -> 强制将对方的ＨＰ同步为自己的ＨＰ。
        /// </summary>
        public const int ENDEAVOR = 283;
        /// <summary>
        /// No.284 喷火 -> 喷出愤怒的熔岩。自己的剩余ＨＰ越多威力越大。
        /// </summary>
        public const int ERUPTION = 284;
        /// <summary>
        /// No.285 交换绝招 -> 用超能力交换双方的特性。
        /// </summary>
        public const int SKILL_SWAP = 285;
        /// <summary>
        /// No.286 封印 -> 如果对方拥有与自己相同的技能，那么对方将无法使用这些技能。
        /// </summary>
        public const int IMPRISON = 286;
        /// <summary>
        /// No.287 清新 -> 休整身体，自身的中毒、麻痹、和烧伤等异常会恢复。
        /// </summary>
        public const int REFRESH = 287;
        /// <summary>
        /// No.288 怨恨 -> 注入强烈的怨念。被对方击倒时对方所用技能的ＰＰ将变为０。
        /// </summary>
        public const int GRUDGE = 288;
        /// <summary>
        /// No.289 抢夺 -> 将对方打算使用的回复技能或能力变化技能夺为己用。
        /// </summary>
        public const int SNATCH = 289;
        /// <summary>
        /// No.290 神秘力量 -> 用神秘力量攻击。使用场所不同，追加效果也会不同。
        /// </summary>
        public const int SECRET_POWER = 290;
        /// <summary>
        /// No.291 潜水 -> 第１回合时潜入水中，第２回合浮上攻击对方。还能够潜入到海的深处。
        /// </summary>
        public const int DIVE = 291;
        /// <summary>
        /// No.292 猛推 -> 用双手推对方，连续攻击对方２－５次。
        /// </summary>
        public const int ARM_THRUST = 292;
        /// <summary>
        /// No.293 保护色 -> 根据自己所在场所的不同，变换自身属性。
        /// </summary>
        public const int CAMOUFLAGE = 293;
        /// <summary>
        /// No.294 萤火 -> 凝视闪烁的荧光，努力集中注意力，大幅提升自身的特攻。
        /// </summary>
        public const int TAIL_GLOW = 294;
        /// <summary>
        /// No.295 洁净光泽 -> 释放眩目的光芒攻击对方。有时会降低对方的特防。
        /// </summary>
        public const int LUSTER_PURGE = 295;
        /// <summary>
        /// No.296 雾气球 -> 用雾状的羽毛包围对方攻击。有时会降低对方的特攻。
        /// </summary>
        public const int MIST_BALL = 296;
        /// <summary>
        /// No.297 羽毛舞 -> 振动羽毛，用羽毛缠住对方。大幅降低对方的攻击。
        /// </summary>
        public const int FEATHERDANCE = 297;
        /// <summary>
        /// No.298 摇晃舞 -> 跳起轻盈的舞蹈，让全场[宝可梦]陷入混乱。
        /// </summary>
        public const int TEETER_DANCE = 298;
        /// <summary>
        /// No.299 火焰踢 -> 会让击中的对象烧伤，容易命中要害。
        /// </summary>
        public const int BLAZE_KICK = 299;
        /// <summary>
        /// No.300 玩泥巴 -> 让地上布满淤泥。只要自己在场就能够使电属性技能的威力降低。
        /// </summary>
        public const int MUD_SPORT = 300;
        /// <summary>
        /// No.301 冰球 -> 连续滚动５回合攻击对方。在每次命中的时候威力都会提升。
        /// </summary>
        public const int ICE_BALL = 301;
        /// <summary>
        /// No.302 针叶暗器 -> 用带刺的手臂猛烈的攻击对方。有时会使对方胆怯。
        /// </summary>
        public const int NEEDLE_ARM = 302;
        /// <summary>
        /// No.303 懒惰 -> 偷懒休息。能够回复自身最大ＨＰ的一半。
        /// </summary>
        public const int SLACK_OFF = 303;
        /// <summary>
        /// No.304 高音 -> 用又吵又响的巨大震动攻击对方。
        /// </summary>
        public const int HYPER_VOICE = 304;
        /// <summary>
        /// No.305 毒液牙 -> 用毒牙啃咬攻击对方。有时会使对方中剧毒。
        /// </summary>
        public const int POISON_FANG = 305;
        /// <summary>
        /// No.306 突击爪 -> 用尖锐的爪子攻击对方。有时会降低对方的防御。
        /// </summary>
        public const int CRUSH_CLAW = 306;
        /// <summary>
        /// No.307 爆炸燃烧 -> 放出强烈的爆炎攻击对方。在下一回合无法行动。
        /// </summary>
        public const int BLAST_BURN = 307;
        /// <summary>
        /// No.308 水电炮 -> 喷射水柱大炮攻击对方。在下一回合无法行动。
        /// </summary>
        public const int HYDRO_CANNON = 308;
        /// <summary>
        /// No.309 流星拳 -> 用彗星般的拳头攻击对方。有时会提升自身的攻击。
        /// </summary>
        public const int METEOR_MASH = 309;
        /// <summary>
        /// No.310 惊吓 -> 用尖叫声惊吓对方，有时会使对方胆怯。
        /// </summary>
        public const int ASTONISH = 310;
        /// <summary>
        /// No.311 气象球 -> 根据天气变化技能的属性和威力也会改变。
        /// </summary>
        public const int WEATHER_BALL = 311;
        /// <summary>
        /// No.312 芳香治疗 -> 放出沁人心脾的香气消除全队的异常状态。
        /// </summary>
        public const int AROMATHERAPY = 312;
        /// <summary>
        /// No.313 假哭 -> 装作伤心流泪的样子骗取对方的同情心从而使对方的特防大幅下降。
        /// </summary>
        public const int FAKE_TEARS = 313;
        /// <summary>
        /// No.314 破空斩 -> 用强风攻击对方。容易命中要害。
        /// </summary>
        public const int AIR_CUTTER = 314;
        /// <summary>
        /// No.315 过热 -> 使出全部力量进攻攻击。使用之后会因为反作用而使自身的特攻大幅下降。
        /// </summary>
        public const int OVERHEAT = 315;
        /// <summary>
        /// No.316 气味侦测 -> 使对幽灵系没有效果的技能能够打中幽灵系，还能够命中回避率高的对方。
        /// </summary>
        public const int ODOR_SLEUTH = 316;
        /// <summary>
        /// No.317 岩石封闭 -> 投出岩石攻击。可封住对方的行动，从而使对方的速度降低。
        /// </summary>
        public const int ROCK_TOMB = 317;
        /// <summary>
        /// No.318 银色旋风 -> 在风中掺入鳞粉攻击对方。有时会提升自身的全部能力。
        /// </summary>
        public const int SILVER_WIND = 318;
        /// <summary>
        /// No.319 金属音 -> 发出摩擦金属般的难以忍受的巨大噪音。使对方的特防大幅下降。
        /// </summary>
        public const int METAL_SOUND = 319;
        /// <summary>
        /// No.320 草笛魔音 -> 吹奏舒服的笛声，让对方睡觉。
        /// </summary>
        public const int GRASSWHISTLE = 320;
        /// <summary>
        /// No.321 搔痒 -> 给对方搔痒使其发笑，从而使它的攻击和防御下降。
        /// </summary>
        public const int TICKLE = 321;
        /// <summary>
        /// No.322 无限能量 -> 接收到从宇宙释放出的神秘力量，提升自身的防御和特防。
        /// </summary>
        public const int COSMIC_POWER = 322;
        /// <summary>
        /// No.323 喷水 -> 卷起巨大的海啸攻击对方。ＨＰ越多威力越大。
        /// </summary>
        public const int WATER_SPOUT = 323;
        /// <summary>
        /// No.324 信号光束 -> 放出神秘的光线攻击对方。有时会使对方混乱。
        /// </summary>
        public const int SIGNAL_BEAM = 324;
        /// <summary>
        /// No.325 影子拳 -> 用暗影之拳攻击对方。攻击必定会命中。
        /// </summary>
        public const int SHADOW_PUNCH = 325;
        /// <summary>
        /// No.326 神通力 -> 用看不见的神奇力量攻击对方。有时会使对方胆怯。
        /// </summary>
        public const int EXTRASENSORY = 326;
        /// <summary>
        /// No.327 上天拳 -> 用一记强力的上钩拳将对方击飞。
        /// </summary>
        public const int SKY_UPPERCUT = 327;
        /// <summary>
        /// No.328 流沙地狱 -> 卷起烦死人的沙尘，造成４－５回合的持续伤害，并使对方无法逃跑。
        /// </summary>
        public const int SAND_TOMB = 328;
        /// <summary>
        /// No.329 绝对零度 -> 用绝对零度冰封对方。一旦命中，对方必定濒死。
        /// </summary>
        public const int SHEER_COLD = 329;
        /// <summary>
        /// No.330 浊流 -> 用污水攻击对方。有时会降低对方的命中。
        /// </summary>
        public const int MUDDY_WATER = 330;
        /// <summary>
        /// No.331 种子机关枪 -> 尽情发射种子攻击。连续攻击对方２－５次。
        /// </summary>
        public const int BULLET_SEED = 331;
        /// <summary>
        /// No.332 回转攻 -> 以迅捷的速度玩弄对方后斩切，攻击必定会命中。
        /// </summary>
        public const int AERIAL_ACE = 332;
        /// <summary>
        /// No.333 冰针 -> 发射尖锐的冰针连续攻击对方２－５次。
        /// </summary>
        public const int ICICLE_SPEAR = 333;
        /// <summary>
        /// No.334 铁壁 -> 使皮肤变得像钢铁一般坚硬。大幅提升自身的防御。
        /// </summary>
        public const int IRON_DEFENSE = 334;
        /// <summary>
        /// No.335 挡路 -> 张开双手封住退路。使对方不能逃走。
        /// </summary>
        public const int BLOCK = 335;
        /// <summary>
        /// No.336 嗥叫 -> 大声吼叫提升气势，提升自己的攻击。
        /// </summary>
        public const int HOWL = 336;
        /// <summary>
        /// No.337 龙爪 -> 用锐利的巨爪切裂对方攻击对方。
        /// </summary>
        public const int DRAGON_CLAW = 337;
        /// <summary>
        /// No.338 疯狂植物 -> 用巨大的树木攻击对方。下一回合无法行动。
        /// </summary>
        public const int FRENZY_PLANT = 338;
        /// <summary>
        /// No.339 健美 -> 绷紧肌肉强化身体，提升自身的攻击和防御。
        /// </summary>
        public const int BULK_UP = 339;
        /// <summary>
        /// No.340 飞跳 -> 第１回合飞到高处，第２回合攻击对方。有时会使对方麻痹。
        /// </summary>
        public const int BOUNCE = 340;
        /// <summary>
        /// No.341 泥巴射击 -> 将凝固的水泥，投掷出去攻击对方。同时会降低对方的速度。
        /// </summary>
        public const int MUD_SHOT = 341;
        /// <summary>
        /// No.342 毒尾 -> 用尾巴攻击，有时会使对方中毒。容易命中要害。
        /// </summary>
        public const int POISON_TAIL = 342;
        /// <summary>
        /// No.343 渴望 -> 装做可爱的样子靠近对方，趁机夺取对方携带的道具。
        /// </summary>
        public const int COVET = 343;
        /// <summary>
        /// No.344 伏特攻击 -> 全身缠绕电气向对方突击，自身也会受到不小的伤害。有时会使对方麻痹。
        /// </summary>
        public const int VOLT_TACKLE = 344;
        /// <summary>
        /// No.345 魔法叶 -> 用施加魔法的树叶攻击对方。攻击必定会命中。
        /// </summary>
        public const int MAGICAL_LEAF = 345;
        /// <summary>
        /// No.346 水之游 -> 用水覆盖身体。只要自己在场就能够减弱炎系技能的伤害。
        /// </summary>
        public const int WATER_SPORT = 346;
        /// <summary>
        /// No.347 冥想 -> 静心凝神，提高自身的特攻与特防。
        /// </summary>
        public const int CALM_MIND = 347;
        /// <summary>
        /// No.348 刀叶 -> 用刃叶切割对方容易命中要害。
        /// </summary>
        public const int LEAF_BLADE = 348;
        /// <summary>
        /// No.349 龙之舞 -> 跳起神秘的、激烈的舞蹈。提升自身的攻击和速度。
        /// </summary>
        public const int DRAGON_DANCE = 349;
        /// <summary>
        /// No.350 岩石爆破 -> 投掷岩石，连续攻击对方２－５次。
        /// </summary>
        public const int ROCK_BLAST = 350;
        /// <summary>
        /// No.351 电击波 -> 用电流快速攻击对方。攻击必定会命中。
        /// </summary>
        public const int SHOCK_WAVE = 351;
        /// <summary>
        /// No.352 水波动 -> 以水的震动给予对方攻击，有时会使对方混乱。
        /// </summary>
        public const int WATER_PULSE = 352;
        /// <summary>
        /// No.353 破灭愿望 -> 使用技能２回合后，会有无数道光线射向对方。
        /// </summary>
        public const int DOOM_DESIRE = 353;
        /// <summary>
        /// No.354 精神提升 -> 使出全部力量进攻。使用后会因为反作用，而使自身的特攻大幅下降。
        /// </summary>
        public const int PSYCHO_BOOST = 354;
        /// <summary>
        /// No.355 歇息 -> 降下地面进行休息。回复最大ＨＰ的一半。
        /// </summary>
        public const int ROOST = 355;
        /// <summary>
        /// No.356 重力 -> ５回合增加对方所受重力。无法继续浮游且无法使用飞空技能。
        /// </summary>
        public const int GRAVITY = 356;
        /// <summary>
        /// No.357 奇迹眼 -> 使对恶系无效的技能能够打中恶系，还能够命中回避率高的对方。
        /// </summary>
        public const int MIRACLE_EYE = 357;
        /// <summary>
        /// No.358 苏醒巴掌 -> 攻击睡觉的对方时威力加倍，但会让对方醒来。
        /// </summary>
        public const int WAKEUP_SLAP = 358;
        /// <summary>
        /// No.359 臂力拳 -> 用强力的拳头攻击对方。同时自身的速度会降低。
        /// </summary>
        public const int HAMMER_ARM = 359;
        /// <summary>
        /// No.360 回转球 -> 高速旋转身体攻击对方。速度比对方越低威力越高。
        /// </summary>
        public const int GYRO_BALL = 360;
        /// <summary>
        /// No.361 治愈愿望 -> 让交换上场的[宝可梦]，回复ＨＰ且清除异常状态。同时自身进入濒死状态。
        /// </summary>
        public const int HEALING_WISH = 361;
        /// <summary>
        /// No.362 盐水 -> 当对方的ＨＰ低于一半时，我方技能威力翻倍。
        /// </summary>
        public const int BRINE = 362;
        /// <summary>
        /// No.363 自然恩惠 -> 通过从树果上获得的力量攻击对方。技能属性和威力会随树果变化。
        /// </summary>
        public const int NATURAL_GIFT = 363;
        /// <summary>
        /// No.364 佯攻 -> 能够攻击保护或见切的对手，同时解除对方保护。
        /// </summary>
        public const int FEINT = 364;
        /// <summary>
        /// No.365 啄食 -> 用喙攻击对方。如果对方携带果实，啄取该果实食用并获得其效果。
        /// </summary>
        public const int PLUCK = 365;
        /// <summary>
        /// No.366 顺风 -> 制造猛烈的旋风。在４回合内，提升我方全员的速度。
        /// </summary>
        public const int TAILWIND = 366;
        /// <summary>
        /// No.367 点穴 -> 通过点穴将经脉激活。随机大幅提升某一项能力。
        /// </summary>
        public const int ACUPRESSURE = 367;
        /// <summary>
        /// No.368 合金爆裂 -> 在出招之前，将最后一次受到的攻击伤害大力返还给对方。
        /// </summary>
        public const int METAL_BURST = 368;
        /// <summary>
        /// No.369 急速转变 -> 在攻击之后，以迅雷不及掩耳之势返回，和不在场的[宝可梦]进行交换。
        /// </summary>
        public const int UTURN = 369;
        /// <summary>
        /// No.370 近身打 -> 放弃防御，和对方近身肉搏。防御和特防会降低。
        /// </summary>
        public const int CLOSE_COMBAT = 370;
        /// <summary>
        /// No.371 报复 -> 蓄力攻击。在对方攻击后出招的话，技能的威力会翻倍。
        /// </summary>
        public const int PAYBACK = 371;
        /// <summary>
        /// No.372 再度欺压 -> 攻击的时候如果对方已经受到伤害的话，威力会变为原来的两倍。
        /// </summary>
        public const int ASSURANCE = 372;
        /// <summary>
        /// No.373 征收 -> 对方不能使用携带的道具，训练师也不能对那只[宝可梦]使用背包里的道具。
        /// </summary>
        public const int EMBARGO = 373;
        /// <summary>
        /// No.374 投掷 -> 快速扔出持有的道具攻击对方。根据持有的道具不同，效果和威力会发生变化。
        /// </summary>
        public const int FLING = 374;
        /// <summary>
        /// No.375 幻象转移 -> 用超能力，将自己的异常状态转移给对方。
        /// </summary>
        public const int PSYCHO_SHIFT = 375;
        /// <summary>
        /// No.376 王牌 -> 王牌技能。剩余的ＰＰ越少，威力越大。
        /// </summary>
        public const int TRUMP_CARD = 376;
        /// <summary>
        /// No.377 回复封闭 -> ５回合内无法使用回复ＨＰ的技能、特性和道具。
        /// </summary>
        public const int HEAL_BLOCK = 377;
        /// <summary>
        /// No.378 拧干 -> 缠紧对方进行攻击。对方ＨＰ越多，攻击就威力越大。
        /// </summary>
        public const int WRING_OUT = 378;
        /// <summary>
        /// No.379 能量骗术 -> 用超能力交换自己的攻击和防御。
        /// </summary>
        public const int POWER_TRICK = 379;
        /// <summary>
        /// No.380 胃酸 -> 向对方喷射胃液。沾上胃液的话特性效果消失。
        /// </summary>
        public const int GASTRO_ACID = 380;
        /// <summary>
        /// No.381 幸运咒语 -> 向天许愿。保护己方怪兽，不会被命中要害。
        /// </summary>
        public const int LUCKY_CHANT = 381;
        /// <summary>
        /// No.382 先抢先赢 -> 抢先使用对方要使用的技能，同时提高技能威力。如果对方不是先出招则会失败。
        /// </summary>
        public const int ME_FIRST = 382;
        /// <summary>
        /// No.383 仿造 -> 模仿对方的技能。对方不出招的话就会失败。
        /// </summary>
        public const int COPYCAT = 383;
        /// <summary>
        /// No.384 交换能量 -> 用超能力交换自身与对手的攻击与特攻等级。
        /// </summary>
        public const int POWER_SWAP = 384;
        /// <summary>
        /// No.385 交换守卫 -> 用超能力交换自身与对手的防御与特防等级。
        /// </summary>
        public const int GUARD_SWAP = 385;
        /// <summary>
        /// No.386 惩罚 -> 技能威力随着对方能力的提升而提升。
        /// </summary>
        public const int PUNISHMENT = 386;
        /// <summary>
        /// No.387 珍藏 -> 只有当学会的技能全部用过一次之后，才能使用的最后手段。
        /// </summary>
        public const int LAST_RESORT = 387;
        /// <summary>
        /// No.388 烦恼种子 -> 让对方心神不宁，使对方的特性变为不眠。
        /// </summary>
        public const int WORRY_SEED = 388;
        /// <summary>
        /// No.389 突袭 -> 对方使用攻击技能时可以优先攻击。对方不攻击则失败。
        /// </summary>
        public const int SUCKER_PUNCH = 389;
        /// <summary>
        /// No.390 毒尖钉 -> 在对方的周围撒上毒钉。换上场的[宝可梦]会中毒。
        /// </summary>
        public const int TOXIC_SPIKES = 390;
        /// <summary>
        /// No.391 交换意志 -> 用超能力交换自身与对方的能力等级。
        /// </summary>
        public const int HEART_SWAP = 391;
        /// <summary>
        /// No.392 水柱圈 -> 在身体周围制造水幕。每回合回复少量ＨＰ。
        /// </summary>
        public const int AQUA_RING = 392;
        /// <summary>
        /// No.393 电磁浮游 -> 利用电气产生的磁力悬浮在空中５回合。
        /// </summary>
        public const int MAGNET_RISE = 393;
        /// <summary>
        /// No.394 爆炎闪击 -> 将火焰全身缠绕向对方进行突击，自身也会受到不小的伤害。有时会使对方烧伤。
        /// </summary>
        public const int FLARE_BLITZ = 394;
        /// <summary>
        /// No.395 发劲 -> 用强力冲击波攻击对方。有时会使对方麻痹。
        /// </summary>
        public const int FORCE_PALM = 395;
        /// <summary>
        /// No.396 波导弹 -> 从体内产生波导的力量，向对方进行攻击。攻击必定会命中。
        /// </summary>
        public const int AURA_SPHERE = 396;
        /// <summary>
        /// No.397 磨光岩石 -> 打磨身体，减少空气阻力，可以迅速提高速度。
        /// </summary>
        public const int ROCK_POLISH = 397;
        /// <summary>
        /// No.398 毒刺 -> 用带毒的触手攻击对方。有时会使对方中毒。
        /// </summary>
        public const int POISON_JAB = 398;
        /// <summary>
        /// No.399 恶波动 -> 从体内放射出充满恶意的光环。有时会使对方害怕。
        /// </summary>
        public const int DARK_PULSE = 399;
        /// <summary>
        /// No.400 街头试刀 -> 瞬间瞄准对方盲区攻击。容易命中要害。
        /// </summary>
        public const int NIGHT_SLASH = 400;
        /// <summary>
        /// No.401 水柱尾 -> 如狂暴的海浪般挥舞尾巴，强力攻击对方。
        /// </summary>
        public const int AQUA_TAIL = 401;
        /// <summary>
        /// No.402 种子爆弹 -> 用外壳坚硬的巨大种子，从上方攻击对方。
        /// </summary>
        public const int SEED_BOMB = 402;
        /// <summary>
        /// No.403 空气砍 -> 制造空气之刃切裂对方。有时会使对方胆怯。
        /// </summary>
        public const int AIR_SLASH = 403;
        /// <summary>
        /// No.404 剪刀十字拳 -> 用镰刀或爪子交叉切裂对方。
        /// </summary>
        public const int XSCISSOR = 404;
        /// <summary>
        /// No.405 虫鸣 -> 用独特方式振动翅膀，发出声音攻击对方。有时会降低对方的特防。
        /// </summary>
        public const int BUG_BUZZ = 405;
        /// <summary>
        /// No.406 龙波动 -> 从口中喷射出巨大的冲击波攻击对方。
        /// </summary>
        public const int DRAGON_PULSE = 406;
        /// <summary>
        /// No.407 龙神俯冲 -> 释放出恐怖的杀气，同时撞击对方。有时会使对方胆怯。
        /// </summary>
        public const int DRAGON_RUSH = 407;
        /// <summary>
        /// No.408 力量宝石 -> 释放出如宝石般的光芒攻击对方。
        /// </summary>
        public const int POWER_GEM = 408;
        /// <summary>
        /// No.409 吸收拳 -> 利用拳头吸收对方的力量。造成伤害的一半，用来回复自身ＨＰ。
        /// </summary>
        public const int DRAIN_PUNCH = 409;
        /// <summary>
        /// No.410 真空波 -> 挥动拳头，发出真空波攻击。必定能够先制攻击。
        /// </summary>
        public const int VACUUM_WAVE = 410;
        /// <summary>
        /// No.411 集气弹 -> 集中精神，释放出全身的力量。有时会降低对方的特防。
        /// </summary>
        public const int FOCUS_BLAST = 411;
        /// <summary>
        /// No.412 能源球 -> 释放出从自然收集的生命的力量。有时会降低对方的特防。
        /// </summary>
        public const int ENERGY_BALL = 412;
        /// <summary>
        /// No.413 神鸟特攻 -> 收拢翅膀，在低空飞行中突击对方。自身也会受到大量伤害。
        /// </summary>
        public const int BRAVE_BIRD = 413;
        /// <summary>
        /// No.414 大地之力 -> 在对方周围释放大地的力量。有时会降低对方的特防。
        /// </summary>
        public const int EARTH_POWER = 414;
        /// <summary>
        /// No.415 掉包 -> 用高速的动作将自己和对方的道具进行交换。
        /// </summary>
        public const int SWITCHEROO = 415;
        /// <summary>
        /// No.416 超极冲击 -> 释放出全部力量进行突击。自己在下一回合无法行动。
        /// </summary>
        public const int GIGA_IMPACT = 416;
        /// <summary>
        /// No.417 诡计 -> 激活大脑，想出更多阴谋。大幅提升自身的特攻。
        /// </summary>
        public const int NASTY_PLOT = 417;
        /// <summary>
        /// No.418 飞弹拳 -> 像子弹般快速击出钢铁之拳。必定能够先制攻击。
        /// </summary>
        public const int BULLET_PUNCH = 418;
        /// <summary>
        /// No.419 雪崩 -> 出招前被攻击的话，技能的威力将会变为２倍。
        /// </summary>
        public const int AVALANCHE = 419;
        /// <summary>
        /// No.420 冰粒 -> 快速地释放出冰块攻击对方。必定能够先制攻击。
        /// </summary>
        public const int ICE_SHARD = 420;
        /// <summary>
        /// No.421 影子钩爪 -> 用锐利的影之爪攻击对方。容易命中要害。
        /// </summary>
        public const int SHADOW_CLAW = 421;
        /// <summary>
        /// No.422 雷牙 -> 用带有电流的牙撕咬对方。有时会使对方麻痹或害怕。
        /// </summary>
        public const int THUNDER_FANG = 422;
        /// <summary>
        /// No.423 冰牙 -> 用充满冷气的牙撕咬对方。有时会使对方结冰或害怕。
        /// </summary>
        public const int ICE_FANG = 423;
        /// <summary>
        /// No.424 炎牙 -> 用充满火焰的牙撕咬对方。有时会使对方烧伤或害怕。
        /// </summary>
        public const int FIRE_FANG = 424;
        /// <summary>
        /// No.425 影子偷袭 -> 伸长影子，从对方的背后攻击对方。必定能够先制攻击。
        /// </summary>
        public const int SHADOW_SNEAK = 425;
        /// <summary>
        /// No.426 泥巴爆弹 -> 发射坚硬的泥弹攻击对方。有时会降低对方的命中率。
        /// </summary>
        public const int MUD_BOMB = 426;
        /// <summary>
        /// No.427 幻象斩 -> 用实体化的精神利刃切割对方，容易命中要害。
        /// </summary>
        public const int PSYCHO_CUT = 427;
        /// <summary>
        /// No.428 意念头锤 -> 将念力集中在前额攻击。有时会使对方害怕。
        /// </summary>
        public const int ZEN_HEADBUTT = 428;
        /// <summary>
        /// No.429 镜光射击 -> 从被磨光的身体里发出闪光攻击对方。有时会降低对方的命中率。
        /// </summary>
        public const int MIRROR_SHOT = 429;
        /// <summary>
        /// No.430 光泽电炮 -> 收集身体的光芒聚集在一点释放出去。有时会降低对方的特防。
        /// </summary>
        public const int FLASH_CANNON = 430;
        /// <summary>
        /// No.431 攀登岩石 -> 用猛烈的势头向对方冲撞过去攻击。有时会使对方混乱。
        /// </summary>
        public const int ROCK_CLIMB = 431;
        /// <summary>
        /// No.432 清除浓雾 -> 用强风驱除光之壁或反射盾等效果。回避率也会降低。
        /// </summary>
        public const int DEFOG = 432;
        /// <summary>
        /// No.433 骗术空间 -> 制造出神秘空间。在５回合内，速度慢的[宝可梦]会先行动。
        /// </summary>
        public const int TRICK_ROOM = 433;
        /// <summary>
        /// No.434 天龙流星 -> 从天空中召唤陨石砸向对方。使用后自身的特攻会大幅降低。
        /// </summary>
        public const int DRACO_METEOR = 434;
        /// <summary>
        /// No.435 放电 -> 用猛烈的电流进行全体攻击。有时会使对方麻痹。
        /// </summary>
        public const int DISCHARGE = 435;
        /// <summary>
        /// No.436 喷烟 -> 用熊熊燃烧的烈火对全体进行攻击。有时会使对方烧伤。
        /// </summary>
        public const int LAVA_PLUME = 436;
        /// <summary>
        /// No.437 叶暴风 -> 用尖锐的树叶卷起风暴攻击对方。使用后自身的特攻会大幅下降。
        /// </summary>
        public const int LEAF_STORM = 437;
        /// <summary>
        /// No.438 能量鞭打 -> 激烈地挥动触手或者藤蔓攻击对方。
        /// </summary>
        public const int POWER_WHIP = 438;
        /// <summary>
        /// No.439 岩石炮 -> 发射巨大的岩石向对方进行攻击。攻击方在下一回合无法行动。
        /// </summary>
        public const int ROCK_WRECKER = 439;
        /// <summary>
        /// No.440 十字毒药 -> 用毒刃将对方斩裂。有时会使对方中毒，且容易命中要害。
        /// </summary>
        public const int CROSS_POISON = 440;
        /// <summary>
        /// No.441 灰尘射击 -> 用超级脏的垃圾攻击对方。有时会使对方中毒。
        /// </summary>
        public const int GUNK_SHOT = 441;
        /// <summary>
        /// No.442 铁头 -> 用钢铁般坚硬的头部撞击对方。有时会使对方胆怯。
        /// </summary>
        public const int IRON_HEAD = 442;
        /// <summary>
        /// No.443 磁铁爆弹 -> 发射能够追踪对方的钢铁炸弹。攻击必定会命中。
        /// </summary>
        public const int MAGNET_BOMB = 443;
        /// <summary>
        /// No.444 尖石攻击 -> 用尖锐的岩石攻击对方。容易命中要害。
        /// </summary>
        public const int STONE_EDGE = 444;
        /// <summary>
        /// No.445 诱惑 -> 能够诱惑异性，降低对方的特攻。
        /// </summary>
        public const int CAPTIVATE = 445;
        /// <summary>
        /// No.446 隐形岩 -> 让无数岩石悬浮在对方的周围，对交换出场的[宝可梦]造成伤害。
        /// </summary>
        public const int STEALTH_ROCK = 446;
        /// <summary>
        /// No.447 打草结 -> 用草绊倒对方，造成伤害。对方越重威力越大。
        /// </summary>
        public const int GRASS_KNOT = 447;
        /// <summary>
        /// No.448 喋喋不休 -> 利用记住的语言制造声波攻击对方。有时会使对方混乱。
        /// </summary>
        public const int CHATTER = 448;
        /// <summary>
        /// No.449 制裁石砾 -> 发射无数的光弹。根据持有石板的不同属性也会变化。
        /// </summary>
        public const int JUDGMENT = 449;
        /// <summary>
        /// No.450 虫咬 -> 啃咬攻击。可以夺取对方的树果并立刻使用。
        /// </summary>
        public const int BUG_BITE = 450;
        /// <summary>
        /// No.451 充电光束 -> 发射电流射线攻击对方。同时还能够蓄积电力，提升自身的特攻。
        /// </summary>
        public const int CHARGE_BEAM = 451;
        /// <summary>
        /// No.452 木槌 -> 用坚硬的身体撞击对方，自身也会受到不小的伤害。
        /// </summary>
        public const int WOOD_HAMMER = 452;
        /// <summary>
        /// No.453 喷射水柱 -> 用眼睛无法跟上的速度接近对方攻击。必定能够先制攻击。
        /// </summary>
        public const int AQUA_JET = 453;
        /// <summary>
        /// No.454 攻击指令 -> 召唤手下，向对方发起攻击。容易命中要害。
        /// </summary>
        public const int ATTACK_ORDER = 454;
        /// <summary>
        /// No.455 防御指令 -> 召唤手下，覆盖自己的身体。能够提升防御与特防。
        /// </summary>
        public const int DEFEND_ORDER = 455;
        /// <summary>
        /// No.456 回复指令 -> 召唤手下，治疗身上的伤口。能够回复一半ＨＰ。
        /// </summary>
        public const int HEAL_ORDER = 456;
        /// <summary>
        /// No.457 双刃头锤 -> 将全身的力量聚集在头部，向对方撞过去。自己也会受到非常大的伤害。
        /// </summary>
        public const int HEAD_SMASH = 457;
        /// <summary>
        /// No.458 双重攻击 -> 用尾巴等部位拍打对方，连续两次攻击对方。
        /// </summary>
        public const int DOUBLE_HIT = 458;
        /// <summary>
        /// No.459 时间咆哮 -> 释放能够扭曲时间的强大力量攻击对方。下一回合无法行动。
        /// </summary>
        public const int ROAR_OF_TIME = 459;
        /// <summary>
        /// No.460 隔空切断 -> 释放出能切裂空间的巨大能量，将对方连同其周围的空间撕裂。容易命中要害。
        /// </summary>
        public const int SPACIAL_REND = 460;
        /// <summary>
        /// No.461 新月舞 -> 自身进入濒死状态，清除交换上场的怪兽所有状态。
        /// </summary>
        public const int LUNAR_DANCE = 461;
        /// <summary>
        /// No.462 捏碎 -> 用惊人的力量捏扁对方。对方ＨＰ越多，威力越大
        /// </summary>
        public const int CRUSH_GRIP = 462;
        /// <summary>
        /// No.463 岩浆暴风 -> 将对方关在熊熊燃烧的火焰中，造成４－５回合的伤害。
        /// </summary>
        public const int MAGMA_STORM = 463;
        /// <summary>
        /// No.464 黑洞 -> 将对方卷入黑暗的世界，使对方昏睡。
        /// </summary>
        public const int DARK_VOID = 464;
        /// <summary>
        /// No.465 种子闪光 -> 从体内放出冲击波攻击对方。有时会大幅降低对方的特防。
        /// </summary>
        public const int SEED_FLARE = 465;
        /// <summary>
        /// No.466 奇异之风 -> 吹出可怕的强风，攻击对方。有时会提升自身的全部能力。
        /// </summary>
        public const int OMINOUS_WIND = 466;
        /// <summary>
        /// No.467 影子潜水 -> 第１回合消失，第２回合攻击对方。即使对方使用保护也能击中。
        /// </summary>
        public const int SHADOW_FORCE = 467;
        /// <summary>
        /// No.468 利爪打磨 -> 把爪子磨得更加锋利。提升自身的攻击和命中率。
        /// </summary>
        public const int HONE_CLAWS = 468;
        /// <summary>
        /// No.469 全体防御 -> 能够在１回合内，防住攻击我方全体的攻击。连续使用成功率会逐渐降低。
        /// </summary>
        public const int WIDE_GUARD = 469;
        /// <summary>
        /// No.470 防御平分 -> 使用超能力将对方和自己的防御与特防加起来再进行平分。
        /// </summary>
        public const int GUARD_SPLIT = 470;
        /// <summary>
        /// No.471 力量平分 -> 使用超能力将对方和自己的攻击与特攻加起来再进行平分。
        /// </summary>
        public const int POWER_SPLIT = 471;
        /// <summary>
        /// No.472 奇异空间 -> 制造出不可思议的空间。在５回合之内，交换所有[宝可梦]的防御和特防。
        /// </summary>
        public const int WONDER_ROOM = 472;
        /// <summary>
        /// No.473 幻象攻击 -> 将神秘的念力波动实体化攻击对方。给与对方物理伤害。
        /// </summary>
        public const int PSYSHOCK = 473;
        /// <summary>
        /// No.474 毒液攻击 -> 用特殊的毒液向对方攻击。对已经中毒的对方威力会加倍。
        /// </summary>
        public const int VENOSHOCK = 474;
        /// <summary>
        /// No.475 体重减轻 -> 削除掉身体上没用的部分，大幅提升自身的速度，同时体重也会变轻。
        /// </summary>
        public const int AUTOTOMIZE = 475;
        /// <summary>
        /// No.476 暴躁鳞粉 -> 将令人烦躁的粉末撒在自己身上用以吸引对方的注意力。使对方的攻击全部指向自己。
        /// </summary>
        public const int RAGE_POWDER = 476;
        /// <summary>
        /// No.477 心灵传动术 -> 使用超能力让对方浮起来。在３回合内，攻击更容易命中对方。
        /// </summary>
        public const int TELEKINESIS = 477;
        /// <summary>
        /// No.478 魔法空间 -> 制造出不可思议的空间。在５回合之内，所有[宝可梦]的道具都会失效。
        /// </summary>
        public const int MAGIC_ROOM = 478;
        /// <summary>
        /// No.479 击落 -> 投掷石头或者炮弹攻击飞行的对象。对方会被打到地面上。
        /// </summary>
        public const int SMACK_DOWN = 479;
        /// <summary>
        /// No.480 山岚 -> 使出强烈的一击攻击对方。必定会命中要害。
        /// </summary>
        public const int STORM_THROW = 480;
        /// <summary>
        /// No.481 烈焰爆破 -> 向对方发射击中后会爆裂的火焰。爆裂的火焰会飞溅到对方身旁的怪兽上。
        /// </summary>
        public const int FLAME_BURST = 481;
        /// <summary>
        /// No.482 淤泥波 -> 使用淤泥波浪攻击全场。有时会使对方中毒。
        /// </summary>
        public const int SLUDGE_WAVE = 482;
        /// <summary>
        /// No.483 蝶之舞 -> 跳起美丽而又神秘的舞蹈，提升自身的特攻、特防和速度。
        /// </summary>
        public const int QUIVER_DANCE = 483;
        /// <summary>
        /// No.484 重量炸弹 -> 用沉重的身体撞向对方自己越重，威力就越大。
        /// </summary>
        public const int HEAVY_SLAM = 484;
        /// <summary>
        /// No.485 同调噪声 -> 放出不可思议的电波给与周围所有和自己属性相同的[宝可梦]以伤害。
        /// </summary>
        public const int SYNCHRONOISE = 485;
        /// <summary>
        /// No.486 电球 -> 用电团砸向对方，自身速度比对方越高威力越大。
        /// </summary>
        public const int ELECTRO_BALL = 486;
        /// <summary>
        /// No.487 浸湿 -> 将大量的水喷射到对方身上，使对方变成水属性。
        /// </summary>
        public const int SOAK = 487;
        /// <summary>
        /// No.488 火焰袭击 -> 让身体缠绕火焰攻击对方。能够提升自身的速度。
        /// </summary>
        public const int FLAME_CHARGE = 488;
        /// <summary>
        /// No.489 蛇蜷缩 -> 蜷缩一团集中精神。提升自身的攻击、防御和命中率。
        /// </summary>
        public const int COIL = 489;
        /// <summary>
        /// No.490 低空踢 -> 利用迅猛的动作攻击对方的脚部。能够降低对方的速度。
        /// </summary>
        public const int LOW_SWEEP = 490;
        /// <summary>
        /// No.491 酸液炸弹 -> 吐出能溶化对方的液体攻击对方。大幅降低对方的特防。
        /// </summary>
        public const int ACID_SPRAY = 491;
        /// <summary>
        /// No.492 诈骗 -> 利用对方的力量进行攻击。如果对方的攻击越高，伤害值就会越高。
        /// </summary>
        public const int FOUL_PLAY = 492;
        /// <summary>
        /// No.493 单纯光线 -> 向对方发射神秘的念力波动。接收到波动的对象的特性会变为单纯。
        /// </summary>
        public const int SIMPLE_BEAM = 493;
        /// <summary>
        /// No.494 变为同伴 -> 用特殊的节奏跳舞。让对方情不自禁的模仿自己，从而使特性变得相同。
        /// </summary>
        public const int ENTRAINMENT = 494;
        /// <summary>
        /// No.495 您优先 -> 支援目标。使其在紧接着自己之后行动。
        /// </summary>
        public const int AFTER_YOU = 495;
        /// <summary>
        /// No.496 轮唱 -> 利用歌声攻击对方。经过大家轮唱后，再使用技能的话威力就会提升。
        /// </summary>
        public const int ROUND = 496;
        /// <summary>
        /// No.497 回声 -> 使用响亮的声音攻击对方。每回合只要有人使用，威力就会提升。
        /// </summary>
        public const int ECHOED_VOICE = 497;
        /// <summary>
        /// No.498 循序渐进 -> 看准时机，给予对方致命一击。无视对方能力变化造成伤害。
        /// </summary>
        public const int CHIP_AWAY = 498;
        /// <summary>
        /// No.499 清除迷雾 -> 扔出特殊的泥块攻击对方。能够将变化的能力恢复原状。
        /// </summary>
        public const int CLEAR_SMOG = 499;
        /// <summary>
        /// No.500 协助力量 -> 用蓄积起来的能量攻击对方。自己的能力提升的越多，威力就越大。
        /// </summary>
        public const int STORED_POWER = 500;
        /// <summary>
        /// No.501 先制防御 -> 能够守护自己和同伴，防止先制攻击。连续使用成功率会逐渐降低。
        /// </summary>
        public const int QUICK_GUARD = 501;
        /// <summary>
        /// No.502 位置交换 -> 使用不可思议的力量交换自己与同伴的位置。
        /// </summary>
        public const int ALLY_SWITCH = 502;
        /// <summary>
        /// No.503 热水 -> 发射沸腾炙热的水攻击对方。有时会使对方烧伤。
        /// </summary>
        public const int SCALD = 503;
        /// <summary>
        /// No.504 破壳 -> 打破坚壳，在降低自身的防御和特防的同时大幅提升攻击、特攻和速度。
        /// </summary>
        public const int SHELL_SMASH = 504;
        /// <summary>
        /// No.505 治愈波动 -> 放出治愈的波动，回复对象最大ＨＰ的一半。
        /// </summary>
        public const int HEAL_PULSE = 505;
        /// <summary>
        /// No.506 厄运临头 -> 接连不断的攻击对方。能够对进入异常状态的对象造成很大伤害。
        /// </summary>
        public const int HEX = 506;
        /// <summary>
        /// No.507 自由落体 -> 第１回合把对方带到空中去，第２回合把它扔下去造成伤害。被带到空中的对象无法行动。
        /// </summary>
        public const int SKY_DROP = 507;
        /// <summary>
        /// No.508 齿轮变换 -> 转动齿轮，除了提升自身的攻击外还能大幅提升自身的速度。
        /// </summary>
        public const int SHIFT_GEAR = 508;
        /// <summary>
        /// No.509 巴投 -> 把对方扔飞，强制让对方交换[宝可梦]。对野生[宝可梦]使用会直接结束战斗。
        /// </summary>
        public const int CIRCLE_THROW = 509;
        /// <summary>
        /// No.510 烧尽 -> 用火焰攻击对方。如果对方持有树果的话，能够连树果一起烧成灰。
        /// </summary>
        public const int INCINERATE = 510;
        /// <summary>
        /// No.511 延迟 -> 压制对方，让其在最后才会行动。
        /// </summary>
        public const int QUASH = 511;
        /// <summary>
        /// No.512 特技演员 -> 轻巧的攻击对方。在自己没有携带道具的时候能够造成很大的伤害。
        /// </summary>
        public const int ACROBATICS = 512;
        /// <summary>
        /// No.513 镜面属性 -> 反射对方的属性，让自己也变为同一属性。
        /// </summary>
        public const int REFLECT_TYPE = 513;
        /// <summary>
        /// No.514 报仇雪恨 -> 为倒下的同伴报仇。如果上一回合有同伴倒下，威力就会提升。
        /// </summary>
        public const int RETALIATE = 514;
        /// <summary>
        /// No.515 命悬一线 -> 赌上性命攻击对方。自身损失所有ＨＰ，给与对方等量伤害。
        /// </summary>
        public const int FINAL_GAMBIT = 515;
        /// <summary>
        /// No.516 礼物传递 -> 在对方未携带道具的时候，能够将自己携带的道具交给对方。
        /// </summary>
        public const int BESTOW = 516;
        /// <summary>
        /// No.517 炼狱 -> 用强烈的火焰包住对方进行攻击。会使对方烧伤。
        /// </summary>
        public const int INFERNO = 517;
        /// <summary>
        /// No.518 水之誓约 -> 用水柱攻击对方。和火之誓言组合后威力会提升。天空中会出现彩虹。
        /// </summary>
        public const int WATER_PLEDGE = 518;
        /// <summary>
        /// No.519 炎之誓约 -> 用火柱攻击对方。和草之誓约组合后威力会提升。周围会变为火海。
        /// </summary>
        public const int FIRE_PLEDGE = 519;
        /// <summary>
        /// No.520 草之誓约 -> 用草柱攻击对方。和水之誓约组合后威力会提升。周围会变为湿地。
        /// </summary>
        public const int GRASS_PLEDGE = 520;
        /// <summary>
        /// No.521 交换伏特 -> 在攻击之后以迅雷不及掩耳之势返回，替换在场[宝可梦]。
        /// </summary>
        public const int VOLT_SWITCH = 521;
        /// <summary>
        /// No.522 虫的抵抗 -> 进行顽强的抵抗，降低对方的特攻。
        /// </summary>
        public const int STRUGGLE_BUG = 522;
        /// <summary>
        /// No.523 压路 -> 用力踩踏地面，攻击全场。能够降低对方的速度。
        /// </summary>
        public const int BULLDOZE = 523;
        /// <summary>
        /// No.524 冰吸 -> 吹出冰冷的气息攻击对方。必定会命中要害。
        /// </summary>
        public const int FROST_BREATH = 524;
        /// <summary>
        /// No.525 龙之尾 -> 把对方弹飞，强制让对方交换[宝可梦]。对野生[宝可梦]使用会直接结束战斗。
        /// </summary>
        public const int DRAGON_TAIL = 525;
        /// <summary>
        /// No.526 自我激励 -> 使出浑身力量使自己奋起，提升攻击和特攻。
        /// </summary>
        public const int WORK_UP = 526;
        /// <summary>
        /// No.527 电磁网 -> 用电网捕捉对方进行攻击。能够降低对方的速度。
        /// </summary>
        public const int ELECTROWEB = 527;
        /// <summary>
        /// No.528 疯狂伏特 -> 身上缠绕电气撞向对方进行攻击。自己也会受到一点伤害。
        /// </summary>
        public const int WILD_CHARGE = 528;
        /// <summary>
        /// No.529 低空钻 -> 像电钻一样旋转身体旋转身体攻击对方。容易命中要害。
        /// </summary>
        public const int DRILL_RUN = 529;
        /// <summary>
        /// No.530 连环切击 -> 用身体坚硬的部分敲打对方，连续造成两次伤害。
        /// </summary>
        public const int DUAL_CHOP = 530;
        /// <summary>
        /// No.531 心灵压迫 -> 以可爱的样子迷惑对手，再给予对手致命一击。有时会使对方害怕。
        /// </summary>
        public const int HEART_STAMP = 531;
        /// <summary>
        /// No.532 木角 -> 用角刺穿对方吸取养分。所造成伤害的一半，用于回复自身ＨＰ。
        /// </summary>
        public const int HORN_LEECH = 532;
        /// <summary>
        /// No.533 神圣剑击 -> 使用长角攻击对方。能够无视对方的能力变化造成伤害。
        /// </summary>
        public const int SACRED_SWORD = 533;
        /// <summary>
        /// No.534 贝壳刀 -> 用尖锐的贝壳攻击对方。有时会降低对方的防御。
        /// </summary>
        public const int RAZOR_SHELL = 534;
        /// <summary>
        /// No.535 高温捣击 -> 用燃烧的身体撞击对方自己越重，威力就会越大。
        /// </summary>
        public const int HEAT_CRASH = 535;
        /// <summary>
        /// No.536 青草搅拌器 -> 使用尖锐的树叶包住对方进行攻击。有时会降低对方的命中率。
        /// </summary>
        public const int LEAF_TORNADO = 536;
        /// <summary>
        /// No.537 疯狂滚动 -> 旋转圆圆的身体压扁对方。有时会使对方害怕。
        /// </summary>
        public const int STEAMROLLER = 537;
        /// <summary>
        /// No.538 棉花防御 -> 用软绵绵的绒毛保护自己的身体。大幅提升自身的防御。
        /// </summary>
        public const int COTTON_GUARD = 538;
        /// <summary>
        /// No.539 黑夜爆裂 -> 释放黑暗的冲击波攻击对方。有时会使对方命中率下降。
        /// </summary>
        public const int NIGHT_DAZE = 539;
        /// <summary>
        /// No.540 精神破坏 -> 将不可思议的念力波动实体化攻击对方。给与对方物理伤害。
        /// </summary>
        public const int PSYSTRIKE = 540;
        /// <summary>
        /// No.541 扫荡拍击 -> 用坚硬的尾巴连续攻击对方２－５次。
        /// </summary>
        public const int TAIL_SLAP = 541;
        /// <summary>
        /// No.542 狂风 -> 用强烈的风席卷对方。有时会使对方混乱。
        /// </summary>
        public const int HURRICANE = 542;
        /// <summary>
        /// No.543 爆爆头突击 -> 用爆炸头突击对方自己也会受到一点伤害。
        /// </summary>
        public const int HEAD_CHARGE = 543;
        /// <summary>
        /// No.544 齿轮飞盘 -> 投掷钢铁齿轮攻击对方。能够造成两次伤害。
        /// </summary>
        public const int GEAR_GRIND = 544;
        /// <summary>
        /// No.545 火焰弹 -> 用通红的火焰，攻击自身周围所有单位。有时会使对方烧伤。
        /// </summary>
        public const int SEARING_SHOT = 545;
        /// <summary>
        /// No.546 科学爆破 -> 放出光弹攻击对方。根据自己所持有的卡带，属性也会改变。
        /// </summary>
        public const int TECHNO_BLAST = 546;
        /// <summary>
        /// No.547 远古歌声 -> 唱出远古时代的歌曲，震慑对方的内心。有时会使对方入睡。
        /// </summary>
        public const int RELIC_SONG = 547;
        /// <summary>
        /// No.548 神秘之剑 -> 用长长的角进行斩切攻击。角上拥有的不可思议的力量将造成物理伤害。
        /// </summary>
        public const int SECRET_SWORD = 548;
        /// <summary>
        /// No.549 冰封世界 -> 吹出冰冷的冷气向对方进行攻击。降低对方的速度。
        /// </summary>
        public const int GLACIATE = 549;
        /// <summary>
        /// No.550 破灭雷击 -> 让强大的电流充满身体后，向对方发动突击。有时会使对方麻痹。
        /// </summary>
        public const int BOLT_STRIKE = 550;
        /// <summary>
        /// No.551 幽冥蓝火 -> 用美丽的蓝色火焰包住对方进行攻击。有时会使对方烧伤。
        /// </summary>
        public const int BLUE_FLARE = 551;
        /// <summary>
        /// No.552 焰翼之舞 -> 让身上充满火焰，展翅向对方发起攻击。有时会提升自身的特攻。
        /// </summary>
        public const int FIERY_DANCE = 552;
        /// <summary>
        /// No.553 冻结伏特 -> 用充满电流电气的冰块在第２回合向对方发起攻击。有时会使对方麻痹。
        /// </summary>
        public const int FREEZE_SHOCK = 553;
        /// <summary>
        /// No.554 冷冻白耀 -> 用能够冻结一切的强烈的冷气在第２回合包裹对方。有时会使对方烧伤。
        /// </summary>
        public const int ICE_BURN = 554;
        /// <summary>
        /// No.555 大喊大叫 -> 没完没了地向对手怒吼，降低对手特攻。
        /// </summary>
        public const int SNARL = 555;
        /// <summary>
        /// No.556 掉落冰柱 -> 用巨大的冰柱向对方进行攻击。有时会使对方害怕。
        /// </summary>
        public const int ICICLE_CRASH = 556;
        /// <summary>
        /// No.557 创造胜利 -> 从额头部发射灼热的火焰，进行舍身的撞击。自身防御、特防和速度会降低。
        /// </summary>
        public const int VCREATE = 557;
        /// <summary>
        /// No.558 交叉烈焰 -> 用巨大的火焰攻击对方。受到巨大的雷电的影响，技能的威力会上升。
        /// </summary>
        public const int FUSION_FLARE = 558;
        /// <summary>
        /// No.559 十字闪电 -> 用巨大的雷电攻击对方。受到巨大的火焰的影响，技能的威力会上升。
        /// </summary>
        public const int FUSION_BOLT = 559;
    }
}
