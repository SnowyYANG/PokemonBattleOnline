using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Game
{
    public static class Ps
    {
        public const int _493_ARCUES = 493;
    }
    public static class As
    {
        /// <summary>
        ///恶臭 造成技能伤害时一定几率令对手害怕。排在队伍首位时，不容易遇到野生精灵。
        ///http://www.pokemon.name/wiki/恶臭
        /// </summary>
        public const int STENCH = 1;
        /// <summary>
        ///降雨 在战斗中出场时天气变为下雨。
        ///http://www.pokemon.name/wiki/降雨
        /// </summary>
        public const int DRIZZLE = 2;
        /// <summary>
        ///加速 在战斗中每回合结束时速度上升一级。
        ///http://www.pokemon.name/wiki/加速
        /// </summary>
        public const int SPEED_BOOST = 3;
        /// <summary>
        ///战斗盔甲 不会被对方的攻击击中要害。
        ///http://www.pokemon.name/wiki/战斗盔甲
        /// </summary>
        public const int BATTLE_ARMOR = 4;
        /// <summary>
        ///坚硬 不会受到一击必杀技的效果，此外，HP全满时即使受到一击致命攻击，也能留下最后的体力。
        ///http://www.pokemon.name/wiki/坚硬
        /// </summary>
        public const int STURDY = 5;
        /// <summary>
        ///潮湿 出战中，自爆与大爆炸技能不能使用，并且引爆特性也不会发动。
        ///http://www.pokemon.name/wiki/潮湿
        /// </summary>
        public const int DAMP = 6;
        /// <summary>
        ///柔软 在战斗中不会被麻痹。
        ///http://www.pokemon.name/wiki/柔软
        /// </summary>
        public const int LIMBER = 7;
        /// <summary>
        ///沙隐 天气为沙暴时，回避率加倍，并且不会受到沙暴伤害。排在队伍首位时，沙暴天气下不容易遇到野生精灵。
        ///http://www.pokemon.name/wiki/沙隐
        /// </summary>
        public const int SAND_VEIL = 8;
        /// <summary>
        ///静电 受到接触攻击时，有几率令攻击方麻痹。排在队伍首位时，容易遇到电系精灵。
        ///http://www.pokemon.name/wiki/静电
        /// </summary>
        public const int STATIC = 9;
        /// <summary>
        ///蓄电 使电系技能伤害无效，同时回复体力。
        ///http://www.pokemon.name/wiki/蓄电
        /// </summary>
        public const int VOLT_ABSORB = 10;
        /// <summary>
        ///贮水 使水系技能伤害无效，同时回复体力。
        ///http://www.pokemon.name/wiki/贮水
        /// </summary>
        public const int WATER_ABSORB = 11;
        /// <summary>
        ///钝感 战斗中不会被魅惑。
        ///http://www.pokemon.name/wiki/钝感
        /// </summary>
        public const int OBLIVIOUS = 12;
        /// <summary>
        ///无天气 出战中，使天气的效果消失，但天气本身不会消失。
        ///http://www.pokemon.name/wiki/无天气
        /// </summary>
        public const int CLOUD_NINE = 13;
        /// <summary>
        ///复眼 提高技能的命中率。排在队伍首位时，容易遇到携带道具的野生精灵。
        ///http://www.pokemon.name/wiki/复眼
        /// </summary>
        public const int COMPOUNDEYES = 14;
        /// <summary>
        ///不眠 战斗中不会睡眠。
        ///http://www.pokemon.name/wiki/不眠
        /// </summary>
        public const int INSOMNIA = 15;
        /// <summary>
        ///变色 受到对方的技能时，自己的属性变为与所受技能属性相同。
        ///http://www.pokemon.name/wiki/变色
        /// </summary>
        public const int COLOR_CHANGE = 16;
        /// <summary>
        ///免疫 战斗中不会中毒。
        ///http://www.pokemon.name/wiki/免疫
        /// </summary>
        public const int IMMUNITY = 17;
        /// <summary>
        ///引火 使受到的火系技能无效化，同时自己使用的火系技能伤害提高。
        ///http://www.pokemon.name/wiki/引火
        /// </summary>
        public const int FLASH_FIRE = 18;
        /// <summary>
        ///鳞粉 不会受到对方技能的追加效果。
        ///http://www.pokemon.name/wiki/鳞粉
        /// </summary>
        public const int SHIELD_DUST = 19;
        /// <summary>
        ///自我中心 战斗中不会混乱。
        ///http://www.pokemon.name/wiki/自我中心
        /// </summary>
        public const int OWN_TEMPO = 20;
        /// <summary>
        ///吸盘 无效化能把对手吹飞的技能。排在队伍首位时，容易用钓竿钓起精灵。
        ///http://www.pokemon.name/wiki/吸盘
        /// </summary>
        public const int SUCTION_CUPS = 21;
        /// <summary>
        ///威吓 在战斗中出场时，降低对方精灵的攻击能力。排在队伍首位时，不容易遇到低等级的野生精灵。
        ///http://www.pokemon.name/wiki/威吓
        /// </summary>
        public const int INTIMIDATE = 22;
        /// <summary>
        ///踩影 令对方的精灵不能逃跑，并且不能交换，但是可以通过技能效果交换。
        ///http://www.pokemon.name/wiki/踩影
        /// </summary>
        public const int SHADOW_TAG = 23;
        /// <summary>
        ///鲨鱼皮 受到接触攻击时，对攻击方造成伤害。
        ///http://www.pokemon.name/wiki/鲨鱼皮
        /// </summary>
        public const int ROUGH_SKIN = 24;
        /// <summary>
        ///奇异守护 除了效果显著的技能外，不会受到技能伤害。
        ///http://www.pokemon.name/wiki/奇异守护
        /// </summary>
        public const int WONDER_GUARD = 25;
        /// <summary>
        ///浮游 不会受到地面系的技能伤害，唯一例外的技能是扔沙。
        ///http://www.pokemon.name/wiki/浮游
        /// </summary>
        public const int LEVITATE = 26;
        /// <summary>
        ///孢子 受到接触攻击时，有几率令攻击方中毒、麻痹或睡眠之一。
        ///http://www.pokemon.name/wiki/孢子
        /// </summary>
        public const int EFFECT_SPORE = 27;
        /// <summary>
        ///同步率 被施加中毒、麻痹或烧伤时，令对方处于相同状态。排在队伍首位时，容易遇到性格相同的精灵。
        ///http://www.pokemon.name/wiki/同步率
        /// </summary>
        public const int SYNCHRONIZE = 28;
        /// <summary>
        ///净体 能力不会被对方的技能或特性降低。
        ///http://www.pokemon.name/wiki/净体
        /// </summary>
        public const int CLEAR_BODY = 29;
        /// <summary>
        ///自然回复 从战斗中退场时，能治好异常状态。
        ///http://www.pokemon.name/wiki/自然回复
        /// </summary>
        public const int NATURAL_CURE = 30;
        /// <summary>
        ///避雷针 使受到的电系技能无效化，同时自己的特攻能力提高。并且这个特性有吸引电系技能的效果。
        ///http://www.pokemon.name/wiki/避雷针
        /// </summary>
        public const int LIGHTNINGROD = 31;
        /// <summary>
        ///天之恩惠 攻击技能的追加效果容易发动。
        ///http://www.pokemon.name/wiki/天之恩惠
        /// </summary>
        public const int SERENE_GRACE = 32;
        /// <summary>
        ///轻快 天气为雨天时，速度翻倍。
        ///http://www.pokemon.name/wiki/轻快
        /// </summary>
        public const int SWIFT_SWIM = 33;
        /// <summary>
        ///叶绿素 阳光强烈时，速度翻倍。
        ///http://www.pokemon.name/wiki/叶绿素
        /// </summary>
        public const int CHLOROPHYLL = 34;
        /// <summary>
        ///发光 排在队伍首位时，容易遇到野生精灵。
        ///http://www.pokemon.name/wiki/发光
        /// </summary>
        public const int ILLUMINATE = 35;
        /// <summary>
        ///复制 在战斗中出场时，特性变为与对手一样。
        ///http://www.pokemon.name/wiki/复制
        /// </summary>
        public const int TRACE = 36;
        /// <summary>
        ///大力士 攻击翻倍。
        ///http://www.pokemon.name/wiki/大力士
        /// </summary>
        public const int HUGE_POWER = 37;
        /// <summary>
        ///毒刺 受到接触攻击时，有几率令攻击方中毒。
        ///http://www.pokemon.name/wiki/毒刺
        /// </summary>
        public const int POISON_POINT = 38;
        /// <summary>
        ///精神力 不会害怕。
        ///http://www.pokemon.name/wiki/精神力
        /// </summary>
        public const int INNER_FOCUS = 39;
        /// <summary>
        ///熔岩铠甲 不会冰冻。存在队伍中时，孵蛋效率增加。
        ///http://www.pokemon.name/wiki/熔岩铠甲
        /// </summary>
        public const int MAGMA_ARMOR = 40;
        /// <summary>
        ///水之掩护 不会烧伤。
        ///http://www.pokemon.name/wiki/水之掩护
        /// </summary>
        public const int WATER_VEIL = 41;
        /// <summary>
        ///磁力 令钢系精灵不能逃跑，并且不能交换，但是可以通过技能效果交换。
        ///http://www.pokemon.name/wiki/磁力
        /// </summary>
        public const int MAGNET_PULL = 42;
        /// <summary>
        ///防音 令用声音攻击的技能无效，对来自己方的技能也会无效化。
        ///http://www.pokemon.name/wiki/防音
        /// </summary>
        public const int SOUNDPROOF = 43;
        /// <summary>
        ///接雨盘 天气为雨天时，每回合逐渐回复HP。
        ///http://www.pokemon.name/wiki/接雨盘
        /// </summary>
        public const int RAIN_DISH = 44;
        /// <summary>
        ///起沙 在战斗中出场时天气变为沙暴。
        ///http://www.pokemon.name/wiki/起沙
        /// </summary>
        public const int SAND_STREAM = 45;
        /// <summary>
        ///压力 令对方使用技能的PP大幅减少。排在队伍首位时，容易遇到等级高的野生精灵。
        ///http://www.pokemon.name/wiki/压力
        /// </summary>
        public const int PRESSURE = 46;
        /// <summary>
        ///厚脂肪 火与冰系技能威力减半。
        ///http://www.pokemon.name/wiki/厚脂肪
        /// </summary>
        public const int THICK_FAT = 47;
        /// <summary>
        ///早起 进入睡眠状态后比平时更容易醒。
        ///http://www.pokemon.name/wiki/早起
        /// </summary>
        public const int EARLY_BIRD = 48;
        /// <summary>
        ///火焰之躯 受到接触攻击时，有几率令攻击方烧伤。存在队伍中时，孵蛋效率增加。
        ///http://www.pokemon.name/wiki/火焰之躯
        /// </summary>
        public const int FLAME_BODY = 49;
        /// <summary>
        ///逃足 在与野生精灵战斗中必定能逃跑。可以无视禁止对方逃跑的技能或特性效果。
        ///http://www.pokemon.name/wiki/逃足
        /// </summary>
        public const int RUN_AWAY = 50;
        /// <summary>
        ///锐利目光 战斗中命中率不会被降低。排在队伍首位时，不会遇到等级低的野生精灵。
        ///http://www.pokemon.name/wiki/锐利目光
        /// </summary>
        public const int KEEN_EYE = 51;
        /// <summary>
        ///怪力钳 战斗中攻击不会被降低。
        ///http://www.pokemon.name/wiki/怪力钳
        /// </summary>
        public const int HYPER_CUTTER = 52;
        /// <summary>
        ///拾取 对方用过道具后，在当回合结束时，捡起来当成自己的携带道具。即使不参加战斗，也有几率捡起道具，精灵的等级越高，能捡到的东西越好。
        ///http://www.pokemon.name/wiki/拾取
        /// </summary>
        public const int PICKUP = 53;
        /// <summary>
        ///懒惰 2回合内只能行动1次。
        ///http://www.pokemon.name/wiki/懒惰
        /// </summary>
        public const int TRUANT = 54;
        /// <summary>
        ///紧张 攻击提高，相对的，命中率降低。
        ///http://www.pokemon.name/wiki/紧张
        /// </summary>
        public const int HUSTLE = 55;
        /// <summary>
        ///魅惑身躯 受到接触攻击时，有几率令攻击方着迷。排在队伍首位时，容易遇到性别不同的精灵。
        ///http://www.pokemon.name/wiki/魅惑身躯
        /// </summary>
        public const int CUTE_CHARM = 56;
        /// <summary>
        ///正极 与正极或负极特性的精灵一起参加战斗时，特攻提高。
        ///http://www.pokemon.name/wiki/正极
        /// </summary>
        public const int PLUS = 57;
        /// <summary>
        ///负极 与正极或负极特性的精灵一起参加战斗时，特攻提高。
        ///http://www.pokemon.name/wiki/负极
        /// </summary>
        public const int MINUS = 58;
        /// <summary>
        ///气象台 这个特性的漂浮泡泡能随着天气改变属性。
        ///http://www.pokemon.name/wiki/气象台
        /// </summary>
        public const int FORECAST = 59;
        /// <summary>
        ///黏着 携带的道具不会被夺走。排在队伍首位时，容易用钓竿钓到精灵。
        ///http://www.pokemon.name/wiki/黏着
        /// </summary>
        public const int STICKY_HOLD = 60;
        /// <summary>
        ///蜕皮 在战斗中有几率回复异常状态。
        ///http://www.pokemon.name/wiki/蜕皮
        /// </summary>
        public const int SHED_SKIN = 61;
        /// <summary>
        ///根性 处于异常状态时攻击提高，并且这个效果能使烧伤的攻击减半无效。
        ///http://www.pokemon.name/wiki/根性
        /// </summary>
        public const int GUTS = 62;
        /// <summary>
        ///神秘鳞片 处于异常状态时防御提高。
        ///http://www.pokemon.name/wiki/神秘鳞片
        /// </summary>
        public const int MARVEL_SCALE = 63;
        /// <summary>
        ///毒液 受到吸收体力的技能攻击时，令攻击方的体力减少所受伤害的一半。
        ///http://www.pokemon.name/wiki/毒液
        /// </summary>
        public const int LIQUID_OOZE = 64;
        /// <summary>
        ///深绿 体力减少陷入危机时，草系技能的威力提高。
        ///http://www.pokemon.name/wiki/深绿
        /// </summary>
        public const int OVERGROW = 65;
        /// <summary>
        ///猛火 体力减少陷入危机时，火系技能的威力提高。
        ///http://www.pokemon.name/wiki/猛火
        /// </summary>
        public const int BLAZE = 66;
        /// <summary>
        ///激流 体力减少陷入危机时，水系技能的威力提高。
        ///http://www.pokemon.name/wiki/激流
        /// </summary>
        public const int TORRENT = 67;
        /// <summary>
        ///虫之预感 体力减少陷入危机时，虫系技能的威力提高。
        ///http://www.pokemon.name/wiki/虫之预感
        /// </summary>
        public const int SWARM = 68;
        /// <summary>
        ///石脑 即使使用反作用力技能也不会受到反作用伤害。
        ///http://www.pokemon.name/wiki/石脑
        /// </summary>
        public const int ROCK_HEAD = 69;
        /// <summary>
        ///干旱 在战斗中出场时，阳光变得强烈。
        ///http://www.pokemon.name/wiki/干旱
        /// </summary>
        public const int DROUGHT = 70;
        /// <summary>
        ///蚁地狱 令飞行属性或浮游特性之外的精灵不能逃跑与交换，但可以通过技能效果交换。
        ///http://www.pokemon.name/wiki/蚁地狱
        /// </summary>
        public const int ARENA_TRAP = 71;
        /// <summary>
        ///干劲 战斗中不会睡眠。排在队伍首位时，容易遇到等级高的野生精灵。
        ///http://www.pokemon.name/wiki/干劲
        /// </summary>
        public const int VITAL_SPIRIT = 72;
        /// <summary>
        ///白烟 能力等级不会被对方的技能或特性降低。
        ///http://www.pokemon.name/wiki/白烟
        /// </summary>
        public const int WHITE_SMOKE = 73;
        /// <summary>
        ///瑜珈之力 攻击翻倍。
        ///http://www.pokemon.name/wiki/瑜珈之力
        /// </summary>
        public const int PURE_POWER = 74;
        /// <summary>
        ///贝壳盔甲 不会被对方的攻击击中要害。
        ///http://www.pokemon.name/wiki/贝壳盔甲
        /// </summary>
        public const int SHELL_ARMOR = 75;
        /// <summary>
        ///天气锁 出战中，使天气的效果消失，但天气本身不会消失。
        ///http://www.pokemon.name/wiki/天气锁
        /// </summary>
        public const int AIR_LOCK = 76;
        /// <summary>
        ///蹒跚 混乱状态下回避率提高。
        ///http://www.pokemon.name/wiki/蹒跚
        /// </summary>
        public const int TANGLED_FEET = 77;
        /// <summary>
        ///电引擎 使受到的电系技能伤害无效化，同时自己的速度能力提高。
        ///http://www.pokemon.name/wiki/电引擎
        /// </summary>
        public const int MOTOR_DRIVE = 78;
        /// <summary>
        ///斗争心 对性别相同的对手技能威力提高，但是对性别不同的对手技能威力降低。
        ///http://www.pokemon.name/wiki/斗争心
        /// </summary>
        public const int RIVALRY = 79;
        /// <summary>
        ///不屈之心 害怕时速度能力提高。
        ///http://www.pokemon.name/wiki/不屈之心
        /// </summary>
        public const int STEADFAST = 80;
        /// <summary>
        ///雪隐 天气为冰雹时，回避率提高。排在队伍首位时，冰雹天气下不容易遇到野生精灵。
        ///http://www.pokemon.name/wiki/雪隐
        /// </summary>
        public const int SNOW_CLOAK = 81;
        /// <summary>
        ///贪吃 体力降低到一半以下时，会吃掉携带的树果。
        ///http://www.pokemon.name/wiki/贪吃
        /// </summary>
        public const int GLUTTONY = 82;
        /// <summary>
        ///怒穴 被击中要害时，攻击能力提高到最大。
        ///http://www.pokemon.name/wiki/怒穴
        /// </summary>
        public const int ANGER_POINT = 83;
        /// <summary>
        ///轻身 携带的道具消失时，速度翻倍。如果一开始就没有携带道具，效果不会发动。
        ///http://www.pokemon.name/wiki/轻身
        /// </summary>
        public const int UNBURDEN = 84;
        /// <summary>
        ///耐热 受到的火系技能伤害减半，但对烧伤伤害无效。
        ///http://www.pokemon.name/wiki/耐热
        /// </summary>
        public const int HEATPROOF = 85;
        /// <summary>
        ///单纯 能力等级变化的效果翻倍。
        ///http://www.pokemon.name/wiki/单纯
        /// </summary>
        public const int SIMPLE = 86;
        /// <summary>
        ///干燥肌肤 受到水系技能或天气是雨天时回复体力，但受到火系技能或阳光强烈时会减少体力。
        ///http://www.pokemon.name/wiki/干燥肌肤
        /// </summary>
        public const int DRY_SKIN = 87;
        /// <summary>
        ///下载 在战斗中出场时，比较对方的防御与特防，如果防御较低，攻击提高；如果特防较低，特攻提高。
        ///http://www.pokemon.name/wiki/下载
        /// </summary>
        public const int DOWNLOAD = 88;
        /// <summary>
        ///铁拳 用拳头攻击的技能威力提高。
        ///http://www.pokemon.name/wiki/铁拳
        /// </summary>
        public const int IRON_FIST = 89;
        /// <summary>
        ///毒疗 中毒时每回合逐渐回复体力。
        ///http://www.pokemon.name/wiki/毒疗
        /// </summary>
        public const int POISON_HEAL = 90;
        /// <summary>
        ///适应力 使用与自己属性相同的技能时，增加更高的伤害。
        ///http://www.pokemon.name/wiki/适应力
        /// </summary>
        public const int ADAPTABILITY = 91;
        /// <summary>
        ///技能连锁 使用连续攻击技时，攻击次数必定最大。
        ///http://www.pokemon.name/wiki/技能连锁
        /// </summary>
        public const int SKILL_LINK = 92;
        /// <summary>
        ///湿润身躯 天气为雨天时，每回合回复异常状态。
        ///http://www.pokemon.name/wiki/湿润身躯
        /// </summary>
        public const int HYDRATION = 93;
        /// <summary>
        ///太阳力量 阳光强烈时，特攻提高，但相对的每回合体力减少。
        ///http://www.pokemon.name/wiki/太阳力量
        /// </summary>
        public const int SOLAR_POWER = 94;
        /// <summary>
        ///早足 处于异常状态时，速度提高，并且麻痹的速度降低效果无效。排在队伍首位时，不容易遇到野生精灵。
        ///http://www.pokemon.name/wiki/早足
        /// </summary>
        public const int QUICK_FEET = 95;
        /// <summary>
        ///普通皮肤 使用的技能全部变为普通属性。
        ///http://www.pokemon.name/wiki/普通皮肤
        /// </summary>
        public const int NORMALIZE = 96;
        /// <summary>
        ///狙击手 攻击击中要害时，造成更多的伤害。
        ///http://www.pokemon.name/wiki/狙击手
        /// </summary>
        public const int SNIPER = 97;
        /// <summary>
        ///魔法守护 不会受到对方攻击以外的伤害。
        ///http://www.pokemon.name/wiki/魔法守护
        /// </summary>
        public const int MAGIC_GUARD = 98;
        /// <summary>
        ///无防御 在战斗中，双方攻击必定能命中。排在队伍首位时，容易遇到野生精灵。
        ///http://www.pokemon.name/wiki/无防御
        /// </summary>
        public const int NO_GUARD = 99;
        /// <summary>
        ///后出 无视速度必定后攻。
        ///http://www.pokemon.name/wiki/后出
        /// </summary>
        public const int STALL = 100;
        /// <summary>
        ///技师 可以提高低威力技能的威力。
        ///http://www.pokemon.name/wiki/技师
        /// </summary>
        public const int TECHNICIAN = 101;
        /// <summary>
        ///绿叶守护 阳光强烈时不会处于异常状态。
        ///http://www.pokemon.name/wiki/绿叶守护
        /// </summary>
        public const int LEAF_GUARD = 102;
        /// <summary>
        ///不器用 不会发动携带道具的效果。
        ///http://www.pokemon.name/wiki/不器用
        /// </summary>
        public const int KLUTZ = 103;
        /// <summary>
        ///破格 无视对方特性作出攻击。
        ///http://www.pokemon.name/wiki/破格
        /// </summary>
        public const int MOLD_BREAKER = 104;
        /// <summary>
        ///强运 技能容易击中要害。
        ///http://www.pokemon.name/wiki/强运
        /// </summary>
        public const int SUPER_LUCK = 105;
        /// <summary>
        ///引爆 受到接触攻击濒死时，对对方造成伤害。
        ///http://www.pokemon.name/wiki/引爆
        /// </summary>
        public const int AFTERMATH = 106;
        /// <summary>
        ///危险预知 如果对方拥有对自己效果显著的技能时会战栗地说明。对于一击必杀技与自爆、大爆炸也会说明。
        ///http://www.pokemon.name/wiki/危险预知
        /// </summary>
        public const int ANTICIPATION = 107;
        /// <summary>
        ///预知梦 参加战斗时会说明对方习得的一个技能，优先说明威力高的技能。
        ///http://www.pokemon.name/wiki/预知梦
        /// </summary>
        public const int FOREWARN = 108;
        /// <summary>
        ///天然 无视能力等级变化进行战斗，但是不能无视速度等级变化。
        ///http://www.pokemon.name/wiki/天然
        /// </summary>
        public const int UNAWARE = 109;
        /// <summary>
        ///有色眼镜 使用效果一般的技能时，造成的伤害翻倍。
        ///http://www.pokemon.name/wiki/有色眼镜
        /// </summary>
        public const int TINTED_LENS = 110;
        /// <summary>
        ///过滤器 受到对方效果显著的技能时，伤害减少。
        ///http://www.pokemon.name/wiki/过滤器
        /// </summary>
        public const int FILTER = 111;
        /// <summary>
        ///缓慢启动 进入战斗的5回合之内攻击与速度降低至一半。
        ///http://www.pokemon.name/wiki/缓慢启动
        /// </summary>
        public const int SLOW_START = 112;
        /// <summary>
        ///胆气 普通属性与格斗属性的技能能击中幽灵属性的精灵。
        ///http://www.pokemon.name/wiki/胆气
        /// </summary>
        public const int SCRAPPY = 113;
        /// <summary>
        ///引水 无效化受到的水系技能，同时自己的特攻能力提高。
        ///http://www.pokemon.name/wiki/引水
        /// </summary>
        public const int STORM_DRAIN = 114;
        /// <summary>
        ///寒冰身躯 天气为冰雹时，每回合逐渐回复体力。
        ///http://www.pokemon.name/wiki/寒冰身躯
        /// </summary>
        public const int ICE_BODY = 115;
        /// <summary>
        ///坚岩 受到对方效果显著的技能时，伤害减少。
        ///http://www.pokemon.name/wiki/坚岩
        /// </summary>
        public const int SOLID_ROCK = 116;
        /// <summary>
        ///降雪 进入战斗时天气变为冰雹。
        ///http://www.pokemon.name/wiki/降雪
        /// </summary>
        public const int SNOW_WARNING = 117;
        /// <summary>
        ///集蜜 战斗结束时，偶尔能捡到甜蜜，精灵等级越高越容易捡到。
        ///http://www.pokemon.name/wiki/集蜜
        /// </summary>
        public const int HONEY_GATHER = 118;
        /// <summary>
        ///洞察 如果对方携带道具，会说明携带的道具。
        ///http://www.pokemon.name/wiki/洞察
        /// </summary>
        public const int FRISK = 119;
        /// <summary>
        ///舍身 使用有反作用力的技能攻击时，技能的威力提高，对未命中会受到伤害的技能也会发动。
        ///http://www.pokemon.name/wiki/舍身
        /// </summary>
        public const int RECKLESS = 120;
        /// <summary>
        ///多重属性 这个特性的阿尔宙斯携带石板时，会根据不同的石板变成对应的属性。
        ///http://www.pokemon.name/wiki/多重属性
        /// </summary>
        public const int MULTITYPE = 121;
        /// <summary>
        ///花之礼物 这个特性的樱花儿在阳光强烈时会改变形态，攻击与特防的能力提高，对队友也有效。
        ///http://www.pokemon.name/wiki/花之礼物
        /// </summary>
        public const int FLOWER_GIFT = 122;
        /// <summary>
        ///梦魇 每回合减少睡眠状态的对手的体力。
        ///http://www.pokemon.name/wiki/梦魇
        /// </summary>
        public const int BAD_DREAMS = 123;
        /// <summary>
        ///偷盗恶习 受到接触攻击时，夺取攻击方的道具。
        ///http://www.pokemon.name/wiki/偷盗恶习
        /// </summary>
        public const int PICKPOCKET = 124;
        /// <summary>
        ///全力攻击 技能威力提高，相对的不会发动追加效果。
        ///http://www.pokemon.name/wiki/全力攻击
        /// </summary>
        public const int SHEER_FORCE = 125;
        /// <summary>
        ///性情乖僻 能力等级变化逆转，提高的变成降低，降低的变成提高。
        ///http://www.pokemon.name/wiki/性情乖僻
        /// </summary>
        public const int CONTRARY = 126;
        /// <summary>
        ///紧张感 战斗中对方不能使用树果。
        ///http://www.pokemon.name/wiki/紧张感
        /// </summary>
        public const int UNNERVE = 127;
        /// <summary>
        ///不服输 能力被对方降低时，攻击能力大幅提高，自己降低能力时不会发动。
        ///http://www.pokemon.name/wiki/不服输
        /// </summary>
        public const int DEFIANT = 128;
        /// <summary>
        ///懦弱 体力降低到一半以下时，攻击与特攻降低。
        ///http://www.pokemon.name/wiki/懦弱
        /// </summary>
        public const int DEFEATIST = 129;
        /// <summary>
        ///诅咒身躯 受到对方的攻击时，对该技能施加束缚状态。
        ///http://www.pokemon.name/wiki/诅咒身躯
        /// </summary>
        public const int CURSED_BODY = 130;
        /// <summary>
        ///治愈之心 能治疗参加战斗的同伴的异常状态，不能回复自己。
        ///http://www.pokemon.name/wiki/治愈之心
        /// </summary>
        public const int HEALER = 131;
        /// <summary>
        ///队友守护 减少参加战斗的同伴受到的伤害，不能减少自己受到的伤害。
        ///http://www.pokemon.name/wiki/队友守护
        /// </summary>
        public const int FRIEND_GUARD = 132;
        /// <summary>
        ///破碎铠甲 受到物理攻击时，防御能力降低，攻击能力提高。
        ///http://www.pokemon.name/wiki/破碎铠甲
        /// </summary>
        public const int WEAK_ARMOR = 133;
        /// <summary>
        ///重金属 自己的体重翻倍。
        ///http://www.pokemon.name/wiki/重金属
        /// </summary>
        public const int HEAVY_METAL = 134;
        /// <summary>
        ///轻金属 自己的体重减半。
        ///http://www.pokemon.name/wiki/轻金属
        /// </summary>
        public const int LIGHT_METAL = 135;
        /// <summary>
        ///多重鳞片 体力全满时，受到的伤害减少。
        ///http://www.pokemon.name/wiki/多重鳞片
        /// </summary>
        public const int MULTISCALE = 136;
        /// <summary>
        ///毒暴走 中毒时攻击提高。
        ///http://www.pokemon.name/wiki/毒暴走
        /// </summary>
        public const int TOXIC_BOOST = 137;
        /// <summary>
        ///热暴走 烧伤时特攻提高。
        ///http://www.pokemon.name/wiki/热暴走
        /// </summary>
        public const int FLARE_BOOST = 138;
        /// <summary>
        ///收获 有几率回收自己使用过的树果，阳光强烈时必定回收。
        ///http://www.pokemon.name/wiki/收获
        /// </summary>
        public const int HARVEST = 139;
        /// <summary>
        ///超感知觉 读取同伴的行动，防止自相残杀。
        ///http://www.pokemon.name/wiki/超感知觉
        /// </summary>
        public const int TELEPATHY = 140;
        /// <summary>
        ///心意不定 回合结束时，一项能力大幅提高，一项能力降低。
        ///http://www.pokemon.name/wiki/心意不定
        /// </summary>
        public const int MOODY = 141;
        /// <summary>
        ///防尘 不会受到天气伤害。
        ///http://www.pokemon.name/wiki/防尘
        /// </summary>
        public const int OVERCOAT = 142;
        /// <summary>
        ///毒手 受到接触攻击时，有几率令攻击方中毒。
        ///http://www.pokemon.name/wiki/毒手
        /// </summary>
        public const int POISON_TOUCH = 143;
        /// <summary>
        ///再生力 从战斗中退场时，能回复体力。
        ///http://www.pokemon.name/wiki/再生力
        /// </summary>
        public const int REGENERATOR = 144;
        /// <summary>
        ///胸甲 防御能力等级不会被对方降低。
        ///http://www.pokemon.name/wiki/胸甲
        /// </summary>
        public const int BIG_PECKS = 145;
        /// <summary>
        ///挖沙 天气为沙暴时，速度翻倍，并且不会受到沙暴伤害。
        ///http://www.pokemon.name/wiki/挖沙
        /// </summary>
        public const int SAND_RUSH = 146;
        /// <summary>
        ///奇迹皮肤 对变化技能的回避率翻倍，自己对自己使用的变化技能不会发动。
        ///http://www.pokemon.name/wiki/奇迹皮肤
        /// </summary>
        public const int WONDER_SKIN = 147;
        /// <summary>
        ///分析 如果最后一个攻击，技能威力提高。
        ///http://www.pokemon.name/wiki/分析
        /// </summary>
        public const int ANALYTIC = 148;
        /// <summary>
        ///幻影 变成队伍中最后一只精灵出战，受到攻击就会变回原来的样子。
        ///http://www.pokemon.name/wiki/幻影
        /// </summary>
        public const int ILLUSION = 149;
        /// <summary>
        ///替代物 在战斗中出场时，变为与对方精灵相同的样子，技能与一部分能力也一样。
        ///http://www.pokemon.name/wiki/替代物
        /// </summary>
        public const int IMPOSTER = 150;
        /// <summary>
        ///穿透 无视对方放出的墙壁效果作出攻击。
        ///http://www.pokemon.name/wiki/穿透
        /// </summary>
        public const int INFILTRATOR = 151;
        /// <summary>
        ///木乃伊 受到接触攻击时，令对方的特性变为木乃伊。
        ///http://www.pokemon.name/wiki/木乃伊
        /// </summary>
        public const int MUMMY = 152;
        /// <summary>
        ///自信过剩 击倒对手时，攻击能力提高。
        ///http://www.pokemon.name/wiki/自信过剩
        /// </summary>
        public const int MOXIE = 153;
        /// <summary>
        ///正义之心 受到恶系技能攻击时，攻击能力提高。
        ///http://www.pokemon.name/wiki/正义之心
        /// </summary>
        public const int JUSTIFIED = 154;
        /// <summary>
        ///颤抖 受到幽灵、虫或恶系技能攻击时，速度能力提高。
        ///http://www.pokemon.name/wiki/颤抖
        /// </summary>
        public const int RATTLED = 155;
        /// <summary>
        ///魔法反射 将对方使用的变化技能原封不动反弹回去。
        ///http://www.pokemon.name/wiki/魔法反射
        /// </summary>
        public const int MAGIC_BOUNCE = 156;
        /// <summary>
        ///食草 受到草系技能时攻击能力提高，并且技能效果与伤害无效。
        ///http://www.pokemon.name/wiki/食草
        /// </summary>
        public const int SAP_SIPPER = 157;
        /// <summary>
        ///恶作剧之心 可以先制发出变化技能。
        ///http://www.pokemon.name/wiki/恶作剧之心
        /// </summary>
        public const int PRANKSTER = 158;
        /// <summary>
        ///沙之力量 天气为沙暴时，地面、岩与钢系技能威力提高。
        ///http://www.pokemon.name/wiki/沙之力量
        /// </summary>
        public const int SAND_FORCE = 159;
        /// <summary>
        ///铁棘 受到接触攻击时，对攻击方造成伤害。
        ///http://www.pokemon.name/wiki/铁棘
        /// </summary>
        public const int IRON_BARBS = 160;
        /// <summary>
        ///不倒翁模式 达摩狒狒的体力在一半以下时，会变为达摩模式。
        ///http://www.pokemon.name/wiki/不倒翁模式
        /// </summary>
        public const int ZEN_MODE = 161;
        /// <summary>
        ///胜利之星 自己与己方精灵的技能命中率提高。
        ///http://www.pokemon.name/wiki/胜利之星
        /// </summary>
        public const int VICTORY_STAR = 162;
        /// <summary>
        ///涡轮火花 无视对方特性作出攻击。
        ///http://www.pokemon.name/wiki/涡轮火花
        /// </summary>
        public const int TURBOBLAZE = 163;
        /// <summary>
        ///垓级电压 无视对方特性作出攻击。
        ///http://www.pokemon.name/wiki/垓级电压
        /// </summary>
        public const int TERAVOLT = 164;
        /// <summary>
        ///芳香掩护 己方所有精灵不会受到精神系技能影响。
        ///http://www.pokemon.name/wiki/芳香掩护
        /// </summary>
        public const int AROMA_VEIL = 165;
        /// <summary>
        ///鲜花掩护 己方场上草系精灵不会受到能力弱化。
        ///http://www.pokemon.name/wiki/鲜花掩护
        /// </summary>
        public const int FLOWER_VEIL = 166;
        /// <summary>
        ///颊囊 对战中食用树果会额外回复HP。
        ///http://www.pokemon.name/wiki/颊囊
        /// </summary>
        public const int CHEEK_POUCH = 167;
        /// <summary>
        ///变幻自在 使用技能前自身变为与技能相同的属性。
        ///http://www.pokemon.name/wiki/变幻自在
        /// </summary>
        public const int PROTEAN = 168;
        /// <summary>
        ///毛皮外衣 受到物理技能的伤害减半。
        ///http://www.pokemon.name/wiki/毛皮外衣
        /// </summary>
        public const int FUR_COAT = 169;
        /// <summary>
        ///魔术师 攻击目标时获得目标道具。
        ///http://www.pokemon.name/wiki/魔术师
        /// </summary>
        public const int MAGICIAN = 170;
        /// <summary>
        ///防弹 无视弹与爆弹系技能。
        ///http://www.pokemon.name/wiki/防弹
        /// </summary>
        public const int BULLETPROOF = 171;
        /// <summary>
        ///争强好胜 能力降低时特攻大幅提高。
        ///http://www.pokemon.name/wiki/争强好胜
        /// </summary>
        public const int COMPETITIVE = 172;
        /// <summary>
        ///强壮之颚 用牙使用的技能威力提高。
        ///http://www.pokemon.name/wiki/强壮之颚
        /// </summary>
        public const int STRONG_JAW = 173;
        /// <summary>
        ///冰冻皮肤 使用普通技能变为冰系，且技能威力提升30%。
        ///http://www.pokemon.name/wiki/冰冻皮肤
        /// </summary>
        public const int REFRIGERATE = 174;
        /// <summary>
        ///甜蜜掩护 防止己方精灵进入睡眠状态。
        ///http://www.pokemon.name/wiki/甜蜜掩护
        /// </summary>
        public const int SWEET_VEIL = 175;
        /// <summary>
        ///战姿切换 坚盾剑怪使用攻击技时变为剑形态，使用王者之盾时变为盾形态。
        ///http://www.pokemon.name/wiki/战姿切换
        /// </summary>
        public const int STANCE_CHANGE = 176;
        /// <summary>
        ///疾风之翼 飞行系技能必定先制。
        ///http://www.pokemon.name/wiki/疾风之翼
        /// </summary>
        public const int GALE_WINGS = 177;
        /// <summary>
        ///超强炮台 波动系技能的威力提高50%。
        ///http://www.pokemon.name/wiki/超强炮台
        /// </summary>
        public const int MEGA_LAUNCHER = 178;
        /// <summary>
        ///绿草毛皮 在绿草场地上时防御提高。
        ///http://www.pokemon.name/wiki/绿草毛皮
        /// </summary>
        public const int GRASS_PELT = 179;
        /// <summary>
        ///共生 能将道具转交给队友。
        ///http://www.pokemon.name/wiki/共生
        /// </summary>
        public const int SYMBIOSIS = 180;
        /// <summary>
        ///坚硬之爪 接触攻击的威力提高。
        ///http://www.pokemon.name/wiki/坚硬之爪
        /// </summary>
        public const int TOUGH_CLAWS = 181;
        /// <summary>
        ///妖精皮肤 使用普通技能变为妖精系，且技能威力提升30%。
        ///http://www.pokemon.name/wiki/妖精皮肤
        /// </summary>
        public const int PIXILATE = 182;
        /// <summary>
        ///黏滑 受到接触攻击时降低攻击方速度。
        ///http://www.pokemon.name/wiki/黏滑
        /// </summary>
        public const int GOOEY = 183;
        /// <summary>
        ///天空皮肤 使用普通技能变为飞行系，且技能威力提升30%。
        ///http://www.pokemon.name/wiki/天空皮肤
        /// </summary>
        public const int AERILATE = 184;
        /// <summary>
        ///亲子爱 作出一次额外攻击。
        ///http://www.pokemon.name/wiki/亲子爱
        /// </summary>
        public const int PARENTAL_BOND = 185;
        /// <summary>
        ///黑暗光环 场上所有精灵的恶系技能威力提高。
        ///http://www.pokemon.name/wiki/黑暗光环
        /// </summary>
        public const int DARK_AURA = 186;
        /// <summary>
        ///妖精光环 场上所有精灵的妖精系技能威力提高。
        ///http://www.pokemon.name/wiki/妖精光环
        /// </summary>
        public const int FAIRY_AURA = 187;
        /// <summary>
        ///光环破坏 使光环特性的效果反转。
        ///http://www.pokemon.name/wiki/光环破坏
        /// </summary>
        public const int AURA_BREAK = 188;
        /// <summary>
        ///始源的大海 变为不会受到火系攻击的天气。
        ///http://www.pokemon.name/wiki/始源的大海
        /// </summary>
        public const int PRIMORDIAL_SEA = 189;
        /// <summary>
        ///终结的大地 变为不会受到水系攻击的天气。
        ///http://www.pokemon.name/wiki/终结的大地
        /// </summary>
        public const int DESOLATE_LAND = 190;
        /// <summary>
        ///德尔塔气流 变为飞行系没有弱点的天气。
        ///http://www.pokemon.name/wiki/德尔塔气流
        /// </summary>
        public const int DELTA_STREAM = 191;
    }
    public static class Is
    {
        public const int RSVP_MAIL = 1;
        public const int RARE_BONE = 2;
        public const int IRON_BALL = 3;
        public const int RING_TARGET = 4;
        public const int SAFETY_GOGGLES = 5;
        public const int BERRY_JUICE = 6;
        public const int BRIGHT_POWDER = 101;
        public const int WIDE_LENS = 102;
        public const int ZOOM_LENS = 103;
        public const int MUSCLE_BAND = 104;
        public const int WISE_GLASSES = 105;
        public const int EXPERT_BELT = 106;
        public const int LIFE_ORB = 107;
        public const int SHELL_BELL = 108;
        public const int METRONOME = 109;
        public const int SCOPE_LENS = 110;
        public const int RAZOR_CLAW = 111;
        public const int RAZOR_FANG = 112;
        public const int KINGS_ROCK = 113;
        public const int FLOAT_STONE = 114;
        public const int WHITE_HERB = 201;
        public const int MENTAL_HERB = 202;
        public const int DESTINY_KNOT = 203;
        public const int QUICK_CLAW = 301;
        public const int LAGGING_TAIL = 302;
        public const int SHED_SHELL = 303;
        public const int LEFTOVERS = 401;
        public const int BLACK_SLUDGE = 402;
        public const int TOXIC_ORB = 403;
        public const int FLAME_ORB = 404;
        public const int FOCUS_BAND = 501;
        public const int FOCUS_SASH = 502;
        public const int STICKY_BARB = 503;
        public const int ROCKY_HELMET = 504;
        public const int AIR_BALLOON = 505;
        public const int RED_CARD = 506;
        public const int EJECT_BUTTON = 507;
        public const int ABSORB_BULB = 508;
        public const int CELL_BATTERY = 509;
        public const int LUMINOUS_MOSS = 510;
        public const int SNOWBALL = 511;
        public const int WEAKNESS_POLICY = 512;
        public const int MACHO_BRACE = 601;
        public const int POWER_BRACER = 602;
        public const int POWER_BELT = 603;
        public const int POWER_LENS = 604;
        public const int POWER_BAND = 605;
        public const int POWER_ANKLET = 606;
        public const int POWER_WEIGHT = 607;
        public const int EVIOLITE = 1001;
        public const int LIGHT_BALL = 1002;
        public const int GRISEOUS_ORB = 1003;
        public const int ADAMANT_ORB = 1004;
        public const int LUSTROUS_ORB = 1005;
        public const int LUCKY_PUNCH = 1006;
        public const int THICK_CLUB = 1007;
        public const int STICK = 1008;
        public const int SOUL_DEW = 1009;
        public const int METAL_POWDER = 1101;
        public const int QUICK_POWDER = 1102;
        public const int DEEPSEATOOTH = 1201;
        public const int DEEPSEASCALE = 1202;
        public const int DOUSE_DRIVE = 1301;
        public const int SHOCK_DRIVE = 1302;
        public const int BURN_DRIVE = 1303;
        public const int CHILL_DRIVE = 1304;
        public const int POWER_HERB = 2001;
        public const int BIG_ROOT = 2002;
        public const int LIGHT_CLAY = 2003;
        public const int ICY_ROCK = 2101;
        public const int SMOOTH_ROCK = 2102;
        public const int HEAT_ROCK = 2103;
        public const int DAMP_ROCK = 2104;
        public const int GRIP_CLAW = 2201;
        public const int BINDING_BAND = 2202;
        public const int CHOICE_BAND = 3001;
        public const int CHOICE_SCARF = 3002;
        public const int CHOICE_SPECS = 3003;
        public const int ASSAULT_VEST = 3004;
        public const int SILVERPOWDER = 4001;
        public const int METAL_COAT = 4002;
        public const int SOFT_SAND = 4003;
        public const int HARD_STONE = 4004;
        public const int MIRACLE_SEED = 4005;
        public const int BLACKGLASSES = 4006;
        public const int BLACK_BELT = 4007;
        public const int MAGNET = 4008;
        public const int MYSTIC_WATER = 4009;
        public const int SHARP_BEAK = 4010;
        public const int POISON_BARB = 4011;
        public const int NEVERMELTICE = 4012;
        public const int SPELL_TAG = 4013;
        public const int TWISTEDSPOON = 4014;
        public const int CHARCOAL = 4015;
        public const int DRAGON_FANG = 4016;
        public const int SILK_SCARF = 4017;
        public const int SEA_INCENSE = 5001;
        public const int LAX_INCENSE = 5002;
        public const int ODD_INCENSE = 5003;
        public const int ROCK_INCENSE = 5004;
        public const int FULL_INCENSE = 5005;
        public const int WAVE_INCENSE = 5006;
        public const int ROSE_INCENSE = 5007;
        public const int FLAME_PLATE = 6001;
        public const int SPLASH_PLATE = 6002;
        public const int ZAP_PLATE = 6003;
        public const int MEADOW_PLATE = 6004;
        public const int ICICLE_PLATE = 6005;
        public const int FIST_PLATE = 6006;
        public const int TOXIC_PLATE = 6007;
        public const int EARTH_PLATE = 6008;
        public const int SKY_PLATE = 6009;
        public const int MIND_PLATE = 6010;
        public const int INSECT_PLATE = 6011;
        public const int STONE_PLATE = 6012;
        public const int SPOOKY_PLATE = 6013;
        public const int DRACO_PLATE = 6014;
        public const int DREAD_PLATE = 6015;
        public const int IRON_PLATE = 6016;
        public const int PIXIE_PLATE = 6017;
        public const int FIRE_GEM = 7001;
        public const int WATER_GEM = 7002;
        public const int ELECTRIC_GEM = 7003;
        public const int GRASS_GEM = 7004;
        public const int ICE_GEM = 7005;
        public const int FIGHTING_GEM = 7006;
        public const int POISON_GEM = 7007;
        public const int GROUND_GEM = 7008;
        public const int FLYING_GEM = 7009;
        public const int PSYCHIC_GEM = 7010;
        public const int BUG_GEM = 7011;
        public const int ROCK_GEM = 7012;
        public const int GHOST_GEM = 7013;
        public const int DRAGON_GEM = 7014;
        public const int DARK_GEM = 7015;
        public const int STEEL_GEM = 7016;
        public const int FAIRY_GEM = 7017;
        public const int NORMAL_GEM = 7018;
        public const int CHERI_BERRY = 8001;
        public const int CHESTO_BERRY = 8002;
        public const int PECHA_BERRY = 8003;
        public const int RAWST_BERRY = 8004;
        public const int ASPEAR_BERRY = 8005;
        public const int LEPPA_BERRY = 8006;
        public const int ORAN_BERRY = 8007;
        public const int PERSIM_BERRY = 8008;
        public const int LUM_BERRY = 8009;
        public const int SITRUS_BERRY = 8010;
        public const int FIGY_BERRY = 8011;
        public const int WIKI_BERRY = 8012;
        public const int MAGO_BERRY = 8013;
        public const int AGUAV_BERRY = 8014;
        public const int IAPAPA_BERRY = 8015;
        public const int RAZZ_BERRY = 8016;
        public const int BLUK_BERRY = 8017;
        public const int NANAB_BERRY = 8018;
        public const int WEPEAR_BERRY = 8019;
        public const int PINAP_BERRY = 8020;
        public const int POMEG_BERRY = 8021;
        public const int KELPSY_BERRY = 8022;
        public const int QUALOT_BERRY = 8023;
        public const int HONDEW_BERRY = 8024;
        public const int GREPA_BERRY = 8025;
        public const int TAMATO_BERRY = 8026;
        public const int CORNN_BERRY = 8027;
        public const int MAGOST_BERRY = 8028;
        public const int RABUTA_BERRY = 8029;
        public const int NOMEL_BERRY = 8030;
        public const int SPELON_BERRY = 8031;
        public const int PAMTRE_BERRY = 8032;
        public const int WATMEL_BERRY = 8033;
        public const int DURIN_BERRY = 8034;
        public const int BELUE_BERRY = 8035;
        public const int OCCA_BERRY = 8101;
        public const int PASSHO_BERRY = 8102;
        public const int WACAN_BERRY = 8103;
        public const int RINDO_BERRY = 8104;
        public const int YACHE_BERRY = 8105;
        public const int CHOPLE_BERRY = 8106;
        public const int KEBIA_BERRY = 8107;
        public const int SHUCA_BERRY = 8108;
        public const int COBA_BERRY = 8109;
        public const int PAYAPA_BERRY = 8110;
        public const int TANGA_BERRY = 8111;
        public const int CHARTI_BERRY = 8112;
        public const int KASIB_BERRY = 8113;
        public const int HABAN_BERRY = 8114;
        public const int COLBUR_BERRY = 8115;
        public const int BABIRI_BERRY = 8116;
        public const int ROSELI_BERRY = 8117;
        public const int CHILAN_BERRY = 8118;
        public const int LIECHI_BERRY = 8201;
        public const int GANLON_BERRY = 8202;
        public const int SALAC_BERRY = 8203;
        public const int PETAYA_BERRY = 8204;
        public const int APICOT_BERRY = 8205;
        public const int LANSAT_BERRY = 8206;
        public const int STARF_BERRY = 8207;
        public const int ENIGMA_BERRY = 8208;
        public const int MICLE_BERRY = 8209;
        public const int CUSTAP_BERRY = 8210;
        public const int JABOCA_BERRY = 8211;
        public const int ROWAP_BERRY = 8212;
        public const int KEE_BERRY = 8213;
        public const int MARANGA_BERRY = 8214;
        public const int VENUSAURITE = 9001;
        public const int CHARIZARDITE_X = 9002;
        public const int CHARIZARDITE_Y = 9003;
        public const int BLASTOISINITE = 9004;
        public const int ALAKAZITE = 9005;
        public const int GENGARITE = 9006;
        public const int KANGASKHANITE = 9007;
        public const int PINSIRITE = 9008;
        public const int GYARADOSITE = 9009;
        public const int AERODACTYLITE = 9010;
        public const int MEWTWONITE_X = 9011;
        public const int MEWTWONITE_Y = 9012;
        public const int AMPHAROSITE = 9013;
        public const int SCIZORITE = 9014;
        public const int HERACRONITE = 9015;
        public const int HOUNDOOMINITE = 9016;
        public const int TYRANITARITE = 9017;
        public const int BLAZIKENITE = 9018;
        public const int GARDEVOIRITE = 9019;
        public const int MAWILITE = 9020;
        public const int AGGRONITE = 9021;
        public const int MEDICHAMITE = 9022;
        public const int MANECTITE = 9023;
        public const int BANETTITE = 9024;
        public const int ABSOLITE = 9025;
        public const int GARCHOMPITE = 9026;
        public const int LUCARIONITE = 9027;
        public const int ABOMASITE = 9028;
        public const int BEEDRILLITE = 9029;
        public const int PIDGEOTITE = 9030;
        public const int SLOWBRONITE = 9031;
        public const int STEELIXITE = 9032;
        public const int SCEPTILITE = 9033;
        public const int SWAMPERTITE = 9034;
        public const int SABLENITE = 9035;
        public const int SHARPEDONITE = 9036;
        public const int CAMERUPTITE = 9037;
        public const int ALTARIANITE = 9038;
        public const int GLALITITE = 9039;
        public const int SALAMENCITE = 9040;
        public const int METAGROSSITE = 9041;
        public const int LATIASITE = 9042;
        public const int LATIOSITE = 9043;
        public const int BLUE_ORB = 9044;
        public const int RED_ORB = 9045;
        public const int LOPUNNITE = 9046;
        public const int GALLADITE = 9047;
        public const int AUDINITE = 9048;
        public const int DIANCITE = 9049;
    }
    public static class Ms
    {
        /// <summary>
        /// 拍击 -> http://wiki.52poke.com/wiki/拍击
        /// </summary>
        public const int POUND = 1;
        /// <summary>
        /// 手刀 -> http://wiki.52poke.com/wiki/手刀
        /// </summary>
        public const int KARATE_CHOP = 2;
        /// <summary>
        /// 连环巴掌 -> http://wiki.52poke.com/wiki/连环巴掌
        /// </summary>
        public const int DOUBLE_SLAP = 3;
        /// <summary>
        /// 连续拳 -> http://wiki.52poke.com/wiki/连续拳
        /// </summary>
        public const int COMET_PUNCH = 4;
        /// <summary>
        /// 百万吨拳击 -> http://wiki.52poke.com/wiki/百万吨拳击
        /// </summary>
        public const int MEGA_PUNCH = 5;
        /// <summary>
        /// 聚宝功 -> http://wiki.52poke.com/wiki/聚宝功
        /// </summary>
        public const int PAY_DAY = 6;
        /// <summary>
        /// 火焰拳 -> http://wiki.52poke.com/wiki/火焰拳
        /// </summary>
        public const int FIRE_PUNCH = 7;
        /// <summary>
        /// 急冻拳 -> http://wiki.52poke.com/wiki/急冻拳
        /// </summary>
        public const int ICE_PUNCH = 8;
        /// <summary>
        /// 雷光掌 -> http://wiki.52poke.com/wiki/雷光掌
        /// </summary>
        public const int THUNDER_PUNCH = 9;
        /// <summary>
        /// 利爪 -> http://wiki.52poke.com/wiki/利爪
        /// </summary>
        public const int SCRATCH = 10;
        /// <summary>
        /// 剪断 -> http://wiki.52poke.com/wiki/剪断
        /// </summary>
        public const int VICE_GRIP = 11;
        /// <summary>
        /// 剪刀断头台 -> http://wiki.52poke.com/wiki/剪刀断头台
        /// </summary>
        public const int GUILLOTINE = 12;
        /// <summary>
        /// 旋风刀 -> http://wiki.52poke.com/wiki/旋风刀
        /// </summary>
        public const int RAZOR_WIND = 13;
        /// <summary>
        /// 剑舞 -> http://wiki.52poke.com/wiki/剑舞
        /// </summary>
        public const int SWORDS_DANCE = 14;
        /// <summary>
        /// 一字斩 -> http://wiki.52poke.com/wiki/一字斩
        /// </summary>
        public const int CUT = 15;
        /// <summary>
        /// 烈暴风 -> http://wiki.52poke.com/wiki/烈暴风
        /// </summary>
        public const int GUST = 16;
        /// <summary>
        /// 翅膀攻击 -> http://wiki.52poke.com/wiki/翅膀攻击
        /// </summary>
        public const int WING_ATTACK = 17;
        /// <summary>
        /// 旋风 -> http://wiki.52poke.com/wiki/旋风
        /// </summary>
        public const int WHIRLWIND = 18;
        /// <summary>
        /// 飞翔 -> http://wiki.52poke.com/wiki/飞翔
        /// </summary>
        public const int FLY = 19;
        /// <summary>
        /// 绑紧 -> http://wiki.52poke.com/wiki/绑紧
        /// </summary>
        public const int BIND = 20;
        /// <summary>
        /// 叩打 -> http://wiki.52poke.com/wiki/叩打
        /// </summary>
        public const int SLAM = 21;
        /// <summary>
        /// 藤鞭 -> http://wiki.52poke.com/wiki/藤鞭
        /// </summary>
        public const int VINE_WHIP = 22;
        /// <summary>
        /// 践踏 -> http://wiki.52poke.com/wiki/践踏
        /// </summary>
        public const int STOMP = 23;
        /// <summary>
        /// 连环腿 -> http://wiki.52poke.com/wiki/连环腿
        /// </summary>
        public const int DOUBLE_KICK = 24;
        /// <summary>
        /// 百万吨飞腿 -> http://wiki.52poke.com/wiki/百万吨飞腿
        /// </summary>
        public const int MEGA_KICK = 25;
        /// <summary>
        /// 飞踢 -> http://wiki.52poke.com/wiki/飞踢
        /// </summary>
        public const int JUMP_KICK = 26;
        /// <summary>
        /// 旋风腿 -> http://wiki.52poke.com/wiki/旋风腿
        /// </summary>
        public const int ROLLING_KICK = 27;
        /// <summary>
        /// 飞沙脚 -> http://wiki.52poke.com/wiki/飞沙脚
        /// </summary>
        public const int SAND_ATTACK = 28;
        /// <summary>
        /// 铁头功 -> http://wiki.52poke.com/wiki/铁头功
        /// </summary>
        public const int HEADBUTT = 29;
        /// <summary>
        /// 角撞 -> http://wiki.52poke.com/wiki/角撞
        /// </summary>
        public const int HORN_ATTACK = 30;
        /// <summary>
        /// 疯狂攻击 -> http://wiki.52poke.com/wiki/疯狂攻击
        /// </summary>
        public const int FURY_ATTACK = 31;
        /// <summary>
        /// 独角钻 -> http://wiki.52poke.com/wiki/独角钻
        /// </summary>
        public const int HORN_DRILL = 32;
        /// <summary>
        /// 冲击 -> http://wiki.52poke.com/wiki/冲击
        /// </summary>
        public const int TACKLE = 33;
        /// <summary>
        /// 泰山压顶 -> http://wiki.52poke.com/wiki/泰山压顶
        /// </summary>
        public const int BODY_SLAM = 34;
        /// <summary>
        /// 捆绑 -> http://wiki.52poke.com/wiki/捆绑
        /// </summary>
        public const int WRAP = 35;
        /// <summary>
        /// 猛撞 -> http://wiki.52poke.com/wiki/猛撞
        /// </summary>
        public const int TAKE_DOWN = 36;
        /// <summary>
        /// 横冲直撞 -> http://wiki.52poke.com/wiki/横冲直撞
        /// </summary>
        public const int THRASH = 37;
        /// <summary>
        /// 舍身攻击 -> http://wiki.52poke.com/wiki/舍身攻击
        /// </summary>
        public const int DOUBLEEDGE = 38;
        /// <summary>
        /// 摇尾巴 -> http://wiki.52poke.com/wiki/摇尾巴
        /// </summary>
        public const int TAIL_WHIP = 39;
        /// <summary>
        /// 毒针 -> http://wiki.52poke.com/wiki/毒针
        /// </summary>
        public const int POISON_STING = 40;
        /// <summary>
        /// 双针 -> http://wiki.52poke.com/wiki/双针
        /// </summary>
        public const int TWINEEDLE = 41;
        /// <summary>
        /// 飞弹针 -> http://wiki.52poke.com/wiki/飞弹针
        /// </summary>
        public const int PIN_MISSILE = 42;
        /// <summary>
        /// 瞪眼 -> http://wiki.52poke.com/wiki/瞪眼
        /// </summary>
        public const int LEER = 43;
        /// <summary>
        /// 咬咬 -> http://wiki.52poke.com/wiki/咬咬
        /// </summary>
        public const int BITE = 44;
        /// <summary>
        /// 嚎叫 -> http://wiki.52poke.com/wiki/嚎叫
        /// </summary>
        public const int GROWL = 45;
        /// <summary>
        /// 吼叫 -> http://wiki.52poke.com/wiki/吼叫
        /// </summary>
        public const int ROAR = 46;
        /// <summary>
        /// 唱歌 -> http://wiki.52poke.com/wiki/唱歌
        /// </summary>
        public const int SING = 47;
        /// <summary>
        /// 超音波 -> http://wiki.52poke.com/wiki/超音波
        /// </summary>
        public const int SUPERSONIC = 48;
        /// <summary>
        /// 音爆 -> http://wiki.52poke.com/wiki/音爆
        /// </summary>
        public const int SONIC_BOOM = 49;
        /// <summary>
        /// 石化功 -> http://wiki.52poke.com/wiki/石化功
        /// </summary>
        public const int DISABLE = 50;
        /// <summary>
        /// 溶解液 -> http://wiki.52poke.com/wiki/溶解液
        /// </summary>
        public const int ACID = 51;
        /// <summary>
        /// 火花 -> http://wiki.52poke.com/wiki/火花
        /// </summary>
        public const int EMBER = 52;
        /// <summary>
        /// 喷射火焰 -> http://wiki.52poke.com/wiki/喷射火焰
        /// </summary>
        public const int FLAMETHROWER = 53;
        /// <summary>
        /// 白雾 -> http://wiki.52poke.com/wiki/白雾
        /// </summary>
        public const int MIST = 54;
        /// <summary>
        /// 水枪 -> http://wiki.52poke.com/wiki/水枪
        /// </summary>
        public const int WATER_GUN = 55;
        /// <summary>
        /// 水炮 -> http://wiki.52poke.com/wiki/水炮
        /// </summary>
        public const int HYDRO_PUMP = 56;
        /// <summary>
        /// 冲浪 -> http://wiki.52poke.com/wiki/冲浪
        /// </summary>
        public const int SURF = 57;
        /// <summary>
        /// 急冻光线 -> http://wiki.52poke.com/wiki/急冻光线
        /// </summary>
        public const int ICE_BEAM = 58;
        /// <summary>
        /// 暴风雪 -> http://wiki.52poke.com/wiki/暴风雪
        /// </summary>
        public const int BLIZZARD = 59;
        /// <summary>
        /// 幻象光 -> http://wiki.52poke.com/wiki/幻象光
        /// </summary>
        public const int PSYBEAM = 60;
        /// <summary>
        /// 泡沫光线 -> http://wiki.52poke.com/wiki/泡沫光线
        /// </summary>
        public const int BUBBLE_BEAM = 61;
        /// <summary>
        /// 极光束 -> http://wiki.52poke.com/wiki/极光束
        /// </summary>
        public const int AURORA_BEAM = 62;
        /// <summary>
        /// 破坏死光 -> http://wiki.52poke.com/wiki/破坏死光
        /// </summary>
        public const int HYPER_BEAM = 63;
        /// <summary>
        /// 啄 -> http://wiki.52poke.com/wiki/啄
        /// </summary>
        public const int PECK = 64;
        /// <summary>
        /// 冲钻 -> http://wiki.52poke.com/wiki/冲钻
        /// </summary>
        public const int DRILL_PECK = 65;
        /// <summary>
        /// 地狱滚动 -> http://wiki.52poke.com/wiki/地狱滚动
        /// </summary>
        public const int SUBMISSION = 66;
        /// <summary>
        /// 下踢 -> http://wiki.52poke.com/wiki/下踢
        /// </summary>
        public const int LOW_KICK = 67;
        /// <summary>
        /// 返拳 -> http://wiki.52poke.com/wiki/返拳
        /// </summary>
        public const int COUNTER = 68;
        /// <summary>
        /// 地球上投 -> http://wiki.52poke.com/wiki/地球上投
        /// </summary>
        public const int SEISMIC_TOSS = 69;
        /// <summary>
        /// 劲力 -> http://wiki.52poke.com/wiki/劲力
        /// </summary>
        public const int STRENGTH = 70;
        /// <summary>
        /// 吸收 -> http://wiki.52poke.com/wiki/吸收
        /// </summary>
        public const int ABSORB = 71;
        /// <summary>
        /// 百万吨吸收 -> http://wiki.52poke.com/wiki/百万吨吸收
        /// </summary>
        public const int MEGA_DRAIN = 72;
        /// <summary>
        /// 寄生种子 -> http://wiki.52poke.com/wiki/寄生种子
        /// </summary>
        public const int LEECH_SEED = 73;
        /// <summary>
        /// 生长 -> http://wiki.52poke.com/wiki/生长
        /// </summary>
        public const int GROWTH = 74;
        /// <summary>
        /// 飞叶快刀 -> http://wiki.52poke.com/wiki/飞叶快刀
        /// </summary>
        public const int RAZOR_LEAF = 75;
        /// <summary>
        /// 阳光烈焰 -> http://wiki.52poke.com/wiki/阳光烈焰
        /// </summary>
        public const int SOLAR_BEAM = 76;
        /// <summary>
        /// 毒粉末 -> http://wiki.52poke.com/wiki/毒粉末
        /// </summary>
        public const int POISON_POWDER = 77;
        /// <summary>
        /// 麻痹粉 -> http://wiki.52poke.com/wiki/麻痹粉
        /// </summary>
        public const int STUN_SPORE = 78;
        /// <summary>
        /// 睡眠粉 -> http://wiki.52poke.com/wiki/睡眠粉
        /// </summary>
        public const int SLEEP_POWDER = 79;
        /// <summary>
        /// 花之舞 -> http://wiki.52poke.com/wiki/花之舞
        /// </summary>
        public const int PETAL_DANCE = 80;
        /// <summary>
        /// 吐丝 -> http://wiki.52poke.com/wiki/吐丝
        /// </summary>
        public const int STRING_SHOT = 81;
        /// <summary>
        /// 龙之怒 -> http://wiki.52poke.com/wiki/龙之怒
        /// </summary>
        public const int DRAGON_RAGE = 82;
        /// <summary>
        /// 火焰漩涡 -> http://wiki.52poke.com/wiki/火焰漩涡
        /// </summary>
        public const int FIRE_SPIN = 83;
        /// <summary>
        /// 电击 -> http://wiki.52poke.com/wiki/电击
        /// </summary>
        public const int THUNDER_SHOCK = 84;
        /// <summary>
        /// 十万伏特 -> http://wiki.52poke.com/wiki/十万伏特
        /// </summary>
        public const int THUNDERBOLT = 85;
        /// <summary>
        /// 电磁波 -> http://wiki.52poke.com/wiki/电磁波
        /// </summary>
        public const int THUNDER_WAVE = 86;
        /// <summary>
        /// 打雷 -> http://wiki.52poke.com/wiki/打雷
        /// </summary>
        public const int THUNDER = 87;
        /// <summary>
        /// 滚石 -> http://wiki.52poke.com/wiki/滚石
        /// </summary>
        public const int ROCK_THROW = 88;
        /// <summary>
        /// 地震 -> http://wiki.52poke.com/wiki/地震
        /// </summary>
        public const int EARTHQUAKE = 89;
        /// <summary>
        /// 地裂 -> http://wiki.52poke.com/wiki/地裂
        /// </summary>
        public const int FISSURE = 90;
        /// <summary>
        /// 挖地洞 -> http://wiki.52poke.com/wiki/挖地洞
        /// </summary>
        public const int DIG = 91;
        /// <summary>
        /// 猛毒素 -> http://wiki.52poke.com/wiki/猛毒素
        /// </summary>
        public const int TOXIC = 92;
        /// <summary>
        /// 念力 -> http://wiki.52poke.com/wiki/念力
        /// </summary>
        public const int CONFUSION = 93;
        /// <summary>
        /// 幻象术 -> http://wiki.52poke.com/wiki/幻象术
        /// </summary>
        public const int PSYCHIC = 94;
        /// <summary>
        /// 催眠术 -> http://wiki.52poke.com/wiki/催眠术
        /// </summary>
        public const int HYPNOSIS = 95;
        /// <summary>
        /// 瑜伽姿势 -> http://wiki.52poke.com/wiki/瑜伽姿势
        /// </summary>
        public const int MEDITATE = 96;
        /// <summary>
        /// 高速移动 -> http://wiki.52poke.com/wiki/高速移动
        /// </summary>
        public const int AGILITY = 97;
        /// <summary>
        /// 电光一闪 -> http://wiki.52poke.com/wiki/电光一闪
        /// </summary>
        public const int QUICK_ATTACK = 98;
        /// <summary>
        /// 愤怒 -> http://wiki.52poke.com/wiki/愤怒
        /// </summary>
        public const int RAGE = 99;
        /// <summary>
        /// 瞬间移动 -> http://wiki.52poke.com/wiki/瞬间移动
        /// </summary>
        public const int TELEPORT = 100;
        /// <summary>
        /// 黑夜诅咒 -> http://wiki.52poke.com/wiki/黑夜诅咒
        /// </summary>
        public const int NIGHT_SHADE = 101;
        /// <summary>
        /// 模仿 -> http://wiki.52poke.com/wiki/模仿
        /// </summary>
        public const int MIMIC = 102;
        /// <summary>
        /// 噪音 -> http://wiki.52poke.com/wiki/噪音
        /// </summary>
        public const int SCREECH = 103;
        /// <summary>
        /// 影子分身 -> http://wiki.52poke.com/wiki/影子分身
        /// </summary>
        public const int DOUBLE_TEAM = 104;
        /// <summary>
        /// 自我复原 -> http://wiki.52poke.com/wiki/自我复原
        /// </summary>
        public const int RECOVER = 105;
        /// <summary>
        /// 硬梆梆 -> http://wiki.52poke.com/wiki/硬梆梆
        /// </summary>
        public const int HARDEN = 106;
        /// <summary>
        /// 缩小 -> http://wiki.52poke.com/wiki/缩小
        /// </summary>
        public const int MINIMIZE = 107;
        /// <summary>
        /// 烟幕 -> http://wiki.52poke.com/wiki/烟幕
        /// </summary>
        public const int SMOKESCREEN = 108;
        /// <summary>
        /// 奇异光线 -> http://wiki.52poke.com/wiki/奇异光线
        /// </summary>
        public const int CONFUSE_RAY = 109;
        /// <summary>
        /// 缩壳 -> http://wiki.52poke.com/wiki/缩壳
        /// </summary>
        public const int WITHDRAW = 110;
        /// <summary>
        /// 防卫卷 -> http://wiki.52poke.com/wiki/防卫卷
        /// </summary>
        public const int DEFENSE_CURL = 111;
        /// <summary>
        /// 障碍 -> http://wiki.52poke.com/wiki/障碍
        /// </summary>
        public const int BARRIER = 112;
        /// <summary>
        /// 光墙 -> http://wiki.52poke.com/wiki/光墙
        /// </summary>
        public const int LIGHT_SCREEN = 113;
        /// <summary>
        /// 黑雾 -> http://wiki.52poke.com/wiki/黑雾
        /// </summary>
        public const int HAZE = 114;
        /// <summary>
        /// 减半反射 -> http://wiki.52poke.com/wiki/减半反射
        /// </summary>
        public const int REFLECT = 115;
        /// <summary>
        /// 集气 -> http://wiki.52poke.com/wiki/集气
        /// </summary>
        public const int FOCUS_ENERGY = 116;
        /// <summary>
        /// 忍忍 -> http://wiki.52poke.com/wiki/忍忍
        /// </summary>
        public const int BIDE = 117;
        /// <summary>
        /// 挥指功 -> http://wiki.52poke.com/wiki/挥指功
        /// </summary>
        public const int METRONOME = 118;
        /// <summary>
        /// 学舌术 -> http://wiki.52poke.com/wiki/学舌术
        /// </summary>
        public const int MIRROR_MOVE = 119;
        /// <summary>
        /// 自爆 -> http://wiki.52poke.com/wiki/自爆
        /// </summary>
        public const int SELFDESTRUCT = 120;
        /// <summary>
        /// 炸蛋 -> http://wiki.52poke.com/wiki/炸蛋
        /// </summary>
        public const int EGG_BOMB = 121;
        /// <summary>
        /// 舔舌头 -> http://wiki.52poke.com/wiki/舔舌头
        /// </summary>
        public const int LICK = 122;
        /// <summary>
        /// 迷雾 -> http://wiki.52poke.com/wiki/迷雾
        /// </summary>
        public const int SMOG = 123;
        /// <summary>
        /// 泥浆攻击 -> http://wiki.52poke.com/wiki/泥浆攻击
        /// </summary>
        public const int SLUDGE = 124;
        /// <summary>
        /// 骨棒 -> http://wiki.52poke.com/wiki/骨棒
        /// </summary>
        public const int BONE_CLUB = 125;
        /// <summary>
        /// 大字爆 -> http://wiki.52poke.com/wiki/大字爆
        /// </summary>
        public const int FIRE_BLAST = 126;
        /// <summary>
        /// 鱼跃龙门 -> http://wiki.52poke.com/wiki/鱼跃龙门
        /// </summary>
        public const int WATERFALL = 127;
        /// <summary>
        /// 夹壳 -> http://wiki.52poke.com/wiki/夹壳
        /// </summary>
        public const int CLAMP = 128;
        /// <summary>
        /// 高速星星 -> http://wiki.52poke.com/wiki/高速星星
        /// </summary>
        public const int SWIFT = 129;
        /// <summary>
        /// 火箭头槌 -> http://wiki.52poke.com/wiki/火箭头槌
        /// </summary>
        public const int SKULL_BASH = 130;
        /// <summary>
        /// 尖刺加农炮 -> http://wiki.52poke.com/wiki/尖刺加农炮
        /// </summary>
        public const int SPIKE_CANNON = 131;
        /// <summary>
        /// 纠缠 -> http://wiki.52poke.com/wiki/纠缠
        /// </summary>
        public const int CONSTRICT = 132;
        /// <summary>
        /// 瞬间失忆 -> http://wiki.52poke.com/wiki/瞬间失忆
        /// </summary>
        public const int AMNESIA = 133;
        /// <summary>
        /// 折弯汤匙 -> http://wiki.52poke.com/wiki/折弯汤匙
        /// </summary>
        public const int KINESIS = 134;
        /// <summary>
        /// 生蛋 -> http://wiki.52poke.com/wiki/生蛋
        /// </summary>
        public const int SOFTBOILED = 135;
        /// <summary>
        /// 飞膝撞 -> http://wiki.52poke.com/wiki/飞膝撞
        /// </summary>
        public const int HIGH_JUMP_KICK = 136;
        /// <summary>
        /// 大蛇瞪眼 -> http://wiki.52poke.com/wiki/大蛇瞪眼
        /// </summary>
        public const int GLARE = 137;
        /// <summary>
        /// 食梦 -> http://wiki.52poke.com/wiki/食梦
        /// </summary>
        public const int DREAM_EATER = 138;
        /// <summary>
        /// 毒瓦斯 -> http://wiki.52poke.com/wiki/毒瓦斯
        /// </summary>
        public const int POISON_GAS = 139;
        /// <summary>
        /// 丢球 -> http://wiki.52poke.com/wiki/丢球
        /// </summary>
        public const int BARRAGE = 140;
        /// <summary>
        /// 吸血 -> http://wiki.52poke.com/wiki/吸血
        /// </summary>
        public const int LEECH_LIFE = 141;
        /// <summary>
        /// 恶魔之吻 -> http://wiki.52poke.com/wiki/恶魔之吻
        /// </summary>
        public const int LOVELY_KISS = 142;
        /// <summary>
        /// 高空攻击 -> http://wiki.52poke.com/wiki/高空攻击
        /// </summary>
        public const int SKY_ATTACK = 143;
        /// <summary>
        /// 变身 -> http://wiki.52poke.com/wiki/变身
        /// </summary>
        public const int TRANSFORM = 144;
        /// <summary>
        /// 泡沫 -> http://wiki.52poke.com/wiki/泡沫
        /// </summary>
        public const int BUBBLE = 145;
        /// <summary>
        /// 迷昏拳 -> http://wiki.52poke.com/wiki/迷昏拳
        /// </summary>
        public const int DIZZY_PUNCH = 146;
        /// <summary>
        /// 蘑菇孢子 -> http://wiki.52poke.com/wiki/蘑菇孢子
        /// </summary>
        public const int SPORE = 147;
        /// <summary>
        /// 闪光 -> http://wiki.52poke.com/wiki/闪光
        /// </summary>
        public const int FLASH = 148;
        /// <summary>
        /// 幻象波 -> http://wiki.52poke.com/wiki/幻象波
        /// </summary>
        public const int PSYWAVE = 149;
        /// <summary>
        /// 水溅跃 -> http://wiki.52poke.com/wiki/水溅跃
        /// </summary>
        public const int SPLASH = 150;
        /// <summary>
        /// 溶化 -> http://wiki.52poke.com/wiki/溶化
        /// </summary>
        public const int ACID_ARMOR = 151;
        /// <summary>
        /// 螃蟹拳 -> http://wiki.52poke.com/wiki/螃蟹拳
        /// </summary>
        public const int CRABHAMMER = 152;
        /// <summary>
        /// 大爆炸 -> http://wiki.52poke.com/wiki/大爆炸
        /// </summary>
        public const int EXPLOSION = 153;
        /// <summary>
        /// 疯狂乱抓 -> http://wiki.52poke.com/wiki/疯狂乱抓
        /// </summary>
        public const int FURY_SWIPES = 154;
        /// <summary>
        /// 骨头回力镖 -> http://wiki.52poke.com/wiki/骨头回力镖
        /// </summary>
        public const int BONEMERANG = 155;
        /// <summary>
        /// 睡觉 -> http://wiki.52poke.com/wiki/睡觉
        /// </summary>
        public const int REST = 156;
        /// <summary>
        /// 山崩地裂 -> http://wiki.52poke.com/wiki/山崩地裂
        /// </summary>
        public const int ROCK_SLIDE = 157;
        /// <summary>
        /// 必杀门牙 -> http://wiki.52poke.com/wiki/必杀门牙
        /// </summary>
        public const int HYPER_FANG = 158;
        /// <summary>
        /// 棱角 -> http://wiki.52poke.com/wiki/棱角
        /// </summary>
        public const int SHARPEN = 159;
        /// <summary>
        /// 变性 -> http://wiki.52poke.com/wiki/变性
        /// </summary>
        public const int CONVERSION = 160;
        /// <summary>
        /// 三角攻击 -> http://wiki.52poke.com/wiki/三角攻击
        /// </summary>
        public const int TRI_ATTACK = 161;
        /// <summary>
        /// 愤怒之牙 -> http://wiki.52poke.com/wiki/愤怒之牙
        /// </summary>
        public const int SUPER_FANG = 162;
        /// <summary>
        /// 劈开 -> http://wiki.52poke.com/wiki/劈开
        /// </summary>
        public const int SLASH = 163;
        /// <summary>
        /// 替身 -> http://wiki.52poke.com/wiki/替身
        /// </summary>
        public const int SUBSTITUTE = 164;
        /// <summary>
        /// 挣扎 -> http://wiki.52poke.com/wiki/挣扎
        /// </summary>
        public const int STRUGGLE = 165;
        /// <summary>
        /// 写生 -> http://wiki.52poke.com/wiki/写生
        /// </summary>
        public const int SKETCH = 166;
        /// <summary>
        /// 三倍足攻 -> http://wiki.52poke.com/wiki/三倍足攻
        /// </summary>
        public const int TRIPLE_KICK = 167;
        /// <summary>
        /// 小偷 -> http://wiki.52poke.com/wiki/小偷
        /// </summary>
        public const int THIEF = 168;
        /// <summary>
        /// 蛛丝 -> http://wiki.52poke.com/wiki/蛛丝
        /// </summary>
        public const int SPIDER_WEB = 169;
        /// <summary>
        /// 心眼 -> http://wiki.52poke.com/wiki/心眼
        /// </summary>
        public const int MIND_READER = 170;
        /// <summary>
        /// 恶梦 -> http://wiki.52poke.com/wiki/恶梦
        /// </summary>
        public const int NIGHTMARE = 171;
        /// <summary>
        /// 火焰轮 -> http://wiki.52poke.com/wiki/火焰轮
        /// </summary>
        public const int FLAME_WHEEL = 172;
        /// <summary>
        /// 打鼾 -> http://wiki.52poke.com/wiki/打鼾
        /// </summary>
        public const int SNORE = 173;
        /// <summary>
        /// 咒语 -> http://wiki.52poke.com/wiki/咒语
        /// </summary>
        public const int CURSE = 174;
        /// <summary>
        /// 手足慌乱 -> http://wiki.52poke.com/wiki/手足慌乱
        /// </summary>
        public const int FLAIL = 175;
        /// <summary>
        /// 变性II -> http://wiki.52poke.com/wiki/变性II
        /// </summary>
        public const int CONVERSION_2 = 176;
        /// <summary>
        /// 空气爆炸 -> http://wiki.52poke.com/wiki/空气爆炸
        /// </summary>
        public const int AEROBLAST = 177;
        /// <summary>
        /// 棉孢子 -> http://wiki.52poke.com/wiki/棉孢子
        /// </summary>
        public const int COTTON_SPORE = 178;
        /// <summary>
        /// 起死回生 -> http://wiki.52poke.com/wiki/起死回生
        /// </summary>
        public const int REVERSAL = 179;
        /// <summary>
        /// 痛恨 -> http://wiki.52poke.com/wiki/痛恨
        /// </summary>
        public const int SPITE = 180;
        /// <summary>
        /// 细雪 -> http://wiki.52poke.com/wiki/细雪
        /// </summary>
        public const int POWDER_SNOW = 181;
        /// <summary>
        /// 守住 -> http://wiki.52poke.com/wiki/守住
        /// </summary>
        public const int PROTECT = 182;
        /// <summary>
        /// 音速拳 -> http://wiki.52poke.com/wiki/音速拳
        /// </summary>
        public const int MACH_PUNCH = 183;
        /// <summary>
        /// 鬼脸 -> http://wiki.52poke.com/wiki/鬼脸
        /// </summary>
        public const int SCARY_FACE = 184;
        /// <summary>
        /// 虚晃一招 -> http://wiki.52poke.com/wiki/虚晃一招
        /// </summary>
        public const int FEINT_ATTACK = 185;
        /// <summary>
        /// 天使之吻 -> http://wiki.52poke.com/wiki/天使之吻
        /// </summary>
        public const int SWEET_KISS = 186;
        /// <summary>
        /// 肚子大鼓 -> http://wiki.52poke.com/wiki/肚子大鼓
        /// </summary>
        public const int BELLY_DRUM = 187;
        /// <summary>
        /// 臭泥爆弹 -> http://wiki.52poke.com/wiki/臭泥爆弹
        /// </summary>
        public const int SLUDGE_BOMB = 188;
        /// <summary>
        /// 泥汤 -> http://wiki.52poke.com/wiki/泥汤
        /// </summary>
        public const int MUDSLAP = 189;
        /// <summary>
        /// 章鱼桶炮 -> http://wiki.52poke.com/wiki/章鱼桶炮
        /// </summary>
        public const int OCTAZOOKA = 190;
        /// <summary>
        /// 满地星 -> http://wiki.52poke.com/wiki/满地星
        /// </summary>
        public const int SPIKES = 191;
        /// <summary>
        /// 电磁炮 -> http://wiki.52poke.com/wiki/电磁炮
        /// </summary>
        public const int ZAP_CANNON = 192;
        /// <summary>
        /// 看破 -> http://wiki.52poke.com/wiki/看破
        /// </summary>
        public const int FORESIGHT = 193;
        /// <summary>
        /// 同命 -> http://wiki.52poke.com/wiki/同命
        /// </summary>
        public const int DESTINY_BOND = 194;
        /// <summary>
        /// 灭亡之歌 -> http://wiki.52poke.com/wiki/灭亡之歌
        /// </summary>
        public const int PERISH_SONG = 195;
        /// <summary>
        /// 冻风 -> http://wiki.52poke.com/wiki/冻风
        /// </summary>
        public const int ICY_WIND = 196;
        /// <summary>
        /// 先决 -> http://wiki.52poke.com/wiki/先决
        /// </summary>
        public const int DETECT = 197;
        /// <summary>
        /// 骨击一气 -> http://wiki.52poke.com/wiki/骨击一气
        /// </summary>
        public const int BONE_RUSH = 198;
        /// <summary>
        /// 锁定 -> http://wiki.52poke.com/wiki/锁定
        /// </summary>
        public const int LOCKON = 199;
        /// <summary>
        /// 龙鳞之怒 -> http://wiki.52poke.com/wiki/龙鳞之怒
        /// </summary>
        public const int OUTRAGE = 200;
        /// <summary>
        /// 沙雹 -> http://wiki.52poke.com/wiki/沙雹
        /// </summary>
        public const int SANDSTORM = 201;
        /// <summary>
        /// 超级吸收 -> http://wiki.52poke.com/wiki/超级吸收
        /// </summary>
        public const int GIGA_DRAIN = 202;
        /// <summary>
        /// 忍耐 -> http://wiki.52poke.com/wiki/忍耐
        /// </summary>
        public const int ENDURE = 203;
        /// <summary>
        /// 撒娇 -> http://wiki.52poke.com/wiki/撒娇
        /// </summary>
        public const int CHARM = 204;
        /// <summary>
        /// 滚动 -> http://wiki.52poke.com/wiki/滚动
        /// </summary>
        public const int ROLLOUT = 205;
        /// <summary>
        /// 刀背击打 -> http://wiki.52poke.com/wiki/刀背击打
        /// </summary>
        public const int FALSE_SWIPE = 206;
        /// <summary>
        /// 装腔作势 -> http://wiki.52poke.com/wiki/装腔作势
        /// </summary>
        public const int SWAGGER = 207;
        /// <summary>
        /// 饮奶 -> http://wiki.52poke.com/wiki/饮奶
        /// </summary>
        public const int MILK_DRINK = 208;
        /// <summary>
        /// 闪电 -> http://wiki.52poke.com/wiki/闪电
        /// </summary>
        public const int SPARK = 209;
        /// <summary>
        /// 连切 -> http://wiki.52poke.com/wiki/连切
        /// </summary>
        public const int FURY_CUTTER = 210;
        /// <summary>
        /// 钢翼 -> http://wiki.52poke.com/wiki/钢翼
        /// </summary>
        public const int STEEL_WING = 211;
        /// <summary>
        /// 黑色眼光 -> http://wiki.52poke.com/wiki/黑色眼光
        /// </summary>
        public const int MEAN_LOOK = 212;
        /// <summary>
        /// 迷人 -> http://wiki.52poke.com/wiki/迷人
        /// </summary>
        public const int ATTRACT = 213;
        /// <summary>
        /// 梦话 -> http://wiki.52poke.com/wiki/梦话
        /// </summary>
        public const int SLEEP_TALK = 214;
        /// <summary>
        /// 治愈铃声 -> http://wiki.52poke.com/wiki/治愈铃声
        /// </summary>
        public const int HEAL_BELL = 215;
        /// <summary>
        /// 报恩 -> http://wiki.52poke.com/wiki/报恩
        /// </summary>
        public const int RETURN = 216;
        /// <summary>
        /// 礼物 -> http://wiki.52poke.com/wiki/礼物
        /// </summary>
        public const int PRESENT = 217;
        /// <summary>
        /// 牵连 -> http://wiki.52poke.com/wiki/牵连
        /// </summary>
        public const int FRUSTRATION = 218;
        /// <summary>
        /// 神秘护身 -> http://wiki.52poke.com/wiki/神秘护身
        /// </summary>
        public const int SAFEGUARD = 219;
        /// <summary>
        /// 分享痛楚 -> http://wiki.52poke.com/wiki/分享痛楚
        /// </summary>
        public const int PAIN_SPLIT = 220;
        /// <summary>
        /// 圣之火 -> http://wiki.52poke.com/wiki/圣之火
        /// </summary>
        public const int SACRED_FIRE = 221;
        /// <summary>
        /// 震级 -> http://wiki.52poke.com/wiki/震级
        /// </summary>
        public const int MAGNITUDE = 222;
        /// <summary>
        /// 爆裂拳 -> http://wiki.52poke.com/wiki/爆裂拳
        /// </summary>
        public const int DYNAMIC_PUNCH = 223;
        /// <summary>
        /// 百万吨角击 -> http://wiki.52poke.com/wiki/百万吨角击
        /// </summary>
        public const int MEGAHORN = 224;
        /// <summary>
        /// 龙吸 -> http://wiki.52poke.com/wiki/龙吸
        /// </summary>
        public const int DRAGON_BREATH = 225;
        /// <summary>
        /// 接棒 -> http://wiki.52poke.com/wiki/接棒
        /// </summary>
        public const int BATON_PASS = 226;
        /// <summary>
        /// 再来一次 -> http://wiki.52poke.com/wiki/再来一次
        /// </summary>
        public const int ENCORE = 227;
        /// <summary>
        /// 追打 -> http://wiki.52poke.com/wiki/追打
        /// </summary>
        public const int PURSUIT = 228;
        /// <summary>
        /// 高速回转 -> http://wiki.52poke.com/wiki/高速回转
        /// </summary>
        public const int RAPID_SPIN = 229;
        /// <summary>
        /// 甜气 -> http://wiki.52poke.com/wiki/甜气
        /// </summary>
        public const int SWEET_SCENT = 230;
        /// <summary>
        /// 铁尾 -> http://wiki.52poke.com/wiki/铁尾
        /// </summary>
        public const int IRON_TAIL = 231;
        /// <summary>
        /// 合金爪 -> http://wiki.52poke.com/wiki/合金爪
        /// </summary>
        public const int METAL_CLAW = 232;
        /// <summary>
        /// 一招制敌 -> http://wiki.52poke.com/wiki/一招制敌
        /// </summary>
        public const int VITAL_THROW = 233;
        /// <summary>
        /// 晨光 -> http://wiki.52poke.com/wiki/晨光
        /// </summary>
        public const int MORNING_SUN = 234;
        /// <summary>
        /// 光学合成 -> http://wiki.52poke.com/wiki/光学合成
        /// </summary>
        public const int SYNTHESIS = 235;
        /// <summary>
        /// 月光 -> http://wiki.52poke.com/wiki/月光
        /// </summary>
        public const int MOONLIGHT = 236;
        /// <summary>
        /// 催醒 -> http://wiki.52poke.com/wiki/催醒
        /// </summary>
        public const int HIDDEN_POWER = 237;
        /// <summary>
        /// 十字切 -> http://wiki.52poke.com/wiki/十字切
        /// </summary>
        public const int CROSS_CHOP = 238;
        /// <summary>
        /// 龙卷风 -> http://wiki.52poke.com/wiki/龙卷风
        /// </summary>
        public const int TWISTER = 239;
        /// <summary>
        /// 乞雨 -> http://wiki.52poke.com/wiki/乞雨
        /// </summary>
        public const int RAIN_DANCE = 240;
        /// <summary>
        /// 大晴天 -> http://wiki.52poke.com/wiki/大晴天
        /// </summary>
        public const int SUNNY_DAY = 241;
        /// <summary>
        /// 咬碎 -> http://wiki.52poke.com/wiki/咬碎
        /// </summary>
        public const int CRUNCH = 242;
        /// <summary>
        /// 表面涂层 -> http://wiki.52poke.com/wiki/表面涂层
        /// </summary>
        public const int MIRROR_COAT = 243;
        /// <summary>
        /// 自我暗示 -> http://wiki.52poke.com/wiki/自我暗示
        /// </summary>
        public const int PSYCH_UP = 244;
        /// <summary>
        /// 神速 -> http://wiki.52poke.com/wiki/神速
        /// </summary>
        public const int EXTREME_SPEED = 245;
        /// <summary>
        /// 原始之力 -> http://wiki.52poke.com/wiki/原始之力
        /// </summary>
        public const int ANCIENT_POWER = 246;
        /// <summary>
        /// 影子球 -> http://wiki.52poke.com/wiki/影子球
        /// </summary>
        public const int SHADOW_BALL = 247;
        /// <summary>
        /// 先知 -> http://wiki.52poke.com/wiki/先知
        /// </summary>
        public const int FUTURE_SIGHT = 248;
        /// <summary>
        /// 岩石粉碎 -> http://wiki.52poke.com/wiki/岩石粉碎
        /// </summary>
        public const int ROCK_SMASH = 249;
        /// <summary>
        /// 潮旋 -> http://wiki.52poke.com/wiki/潮旋
        /// </summary>
        public const int WHIRLPOOL = 250;
        /// <summary>
        /// 痛打一气 -> http://wiki.52poke.com/wiki/痛打一气
        /// </summary>
        public const int BEAT_UP = 251;
        /// <summary>
        /// 假动作 -> http://wiki.52poke.com/wiki/假动作
        /// </summary>
        public const int FAKE_OUT = 252;
        /// <summary>
        /// 吵闹 -> http://wiki.52poke.com/wiki/吵闹
        /// </summary>
        public const int UPROAR = 253;
        /// <summary>
        /// 储存 -> http://wiki.52poke.com/wiki/储存
        /// </summary>
        public const int STOCKPILE = 254;
        /// <summary>
        /// 喷出 -> http://wiki.52poke.com/wiki/喷出
        /// </summary>
        public const int SPIT_UP = 255;
        /// <summary>
        /// 吞下 -> http://wiki.52poke.com/wiki/吞下
        /// </summary>
        public const int SWALLOW = 256;
        /// <summary>
        /// 热风 -> http://wiki.52poke.com/wiki/热风
        /// </summary>
        public const int HEAT_WAVE = 257;
        /// <summary>
        /// 冰雹 -> http://wiki.52poke.com/wiki/冰雹
        /// </summary>
        public const int HAIL = 258;
        /// <summary>
        /// 假指控 -> http://wiki.52poke.com/wiki/假指控
        /// </summary>
        public const int TORMENT = 259;
        /// <summary>
        /// 煽惑 -> http://wiki.52poke.com/wiki/煽惑
        /// </summary>
        public const int FLATTER = 260;
        /// <summary>
        /// 鬼火 -> http://wiki.52poke.com/wiki/鬼火
        /// </summary>
        public const int WILLOWISP = 261;
        /// <summary>
        /// 临别礼物 -> http://wiki.52poke.com/wiki/临别礼物
        /// </summary>
        public const int MEMENTO = 262;
        /// <summary>
        /// 假勇敢 -> http://wiki.52poke.com/wiki/假勇敢
        /// </summary>
        public const int FACADE = 263;
        /// <summary>
        /// 集中猛击 -> http://wiki.52poke.com/wiki/集中猛击
        /// </summary>
        public const int FOCUS_PUNCH = 264;
        /// <summary>
        /// 苏醒 -> http://wiki.52poke.com/wiki/苏醒
        /// </summary>
        public const int SMELLING_SALTS = 265;
        /// <summary>
        /// 跟我来 -> http://wiki.52poke.com/wiki/跟我来
        /// </summary>
        public const int FOLLOW_ME = 266;
        /// <summary>
        /// 自然能力 -> http://wiki.52poke.com/wiki/自然能力
        /// </summary>
        public const int NATURE_POWER = 267;
        /// <summary>
        /// 充电 -> http://wiki.52poke.com/wiki/充电
        /// </summary>
        public const int CHARGE = 268;
        /// <summary>
        /// 挑拨 -> http://wiki.52poke.com/wiki/挑拨
        /// </summary>
        public const int TAUNT = 269;
        /// <summary>
        /// 帮助 -> http://wiki.52poke.com/wiki/帮助
        /// </summary>
        public const int HELPING_HAND = 270;
        /// <summary>
        /// 骗术 -> http://wiki.52poke.com/wiki/骗术
        /// </summary>
        public const int TRICK = 271;
        /// <summary>
        /// 角色扮演 -> http://wiki.52poke.com/wiki/角色扮演
        /// </summary>
        public const int ROLE_PLAY = 272;
        /// <summary>
        /// 祈求 -> http://wiki.52poke.com/wiki/祈求
        /// </summary>
        public const int WISH = 273;
        /// <summary>
        /// 协助 -> http://wiki.52poke.com/wiki/协助
        /// </summary>
        public const int ASSIST = 274;
        /// <summary>
        /// 根深蒂固 -> http://wiki.52poke.com/wiki/根深蒂固
        /// </summary>
        public const int INGRAIN = 275;
        /// <summary>
        /// 蛮力 -> http://wiki.52poke.com/wiki/蛮力
        /// </summary>
        public const int SUPERPOWER = 276;
        /// <summary>
        /// 魔术外衣 -> http://wiki.52poke.com/wiki/魔术外衣
        /// </summary>
        public const int MAGIC_COAT = 277;
        /// <summary>
        /// 回收 -> http://wiki.52poke.com/wiki/回收
        /// </summary>
        public const int RECYCLE = 278;
        /// <summary>
        /// 报仇 -> http://wiki.52poke.com/wiki/报仇
        /// </summary>
        public const int REVENGE = 279;
        /// <summary>
        /// 劈瓦 -> http://wiki.52poke.com/wiki/劈瓦
        /// </summary>
        public const int BRICK_BREAK = 280;
        /// <summary>
        /// 哈欠 -> http://wiki.52poke.com/wiki/哈欠
        /// </summary>
        public const int YAWN = 281;
        /// <summary>
        /// 落拳 -> http://wiki.52poke.com/wiki/落拳
        /// </summary>
        public const int KNOCK_OFF = 282;
        /// <summary>
        /// 强攻 -> http://wiki.52poke.com/wiki/强攻
        /// </summary>
        public const int ENDEAVOR = 283;
        /// <summary>
        /// 喷火 -> http://wiki.52poke.com/wiki/喷火
        /// </summary>
        public const int ERUPTION = 284;
        /// <summary>
        /// 交换绝招 -> http://wiki.52poke.com/wiki/交换绝招
        /// </summary>
        public const int SKILL_SWAP = 285;
        /// <summary>
        /// 封印 -> http://wiki.52poke.com/wiki/封印
        /// </summary>
        public const int IMPRISON = 286;
        /// <summary>
        /// 清新 -> http://wiki.52poke.com/wiki/清新
        /// </summary>
        public const int REFRESH = 287;
        /// <summary>
        /// 怨恨 -> http://wiki.52poke.com/wiki/怨恨
        /// </summary>
        public const int GRUDGE = 288;
        /// <summary>
        /// 抢夺 -> http://wiki.52poke.com/wiki/抢夺
        /// </summary>
        public const int SNATCH = 289;
        /// <summary>
        /// 神秘力量 -> http://wiki.52poke.com/wiki/神秘力量
        /// </summary>
        public const int SECRET_POWER = 290;
        /// <summary>
        /// 潜水 -> http://wiki.52poke.com/wiki/潜水
        /// </summary>
        public const int DIVE = 291;
        /// <summary>
        /// 猛推 -> http://wiki.52poke.com/wiki/猛推
        /// </summary>
        public const int ARM_THRUST = 292;
        /// <summary>
        /// 保护色 -> http://wiki.52poke.com/wiki/保护色
        /// </summary>
        public const int CAMOUFLAGE = 293;
        /// <summary>
        /// 萤火 -> http://wiki.52poke.com/wiki/萤火
        /// </summary>
        public const int TAIL_GLOW = 294;
        /// <summary>
        /// 洁净光泽 -> http://wiki.52poke.com/wiki/洁净光泽
        /// </summary>
        public const int LUSTER_PURGE = 295;
        /// <summary>
        /// 薄雾球 -> http://wiki.52poke.com/wiki/薄雾球
        /// </summary>
        public const int MIST_BALL = 296;
        /// <summary>
        /// 羽毛舞 -> http://wiki.52poke.com/wiki/羽毛舞
        /// </summary>
        public const int FEATHER_DANCE = 297;
        /// <summary>
        /// 摇晃舞 -> http://wiki.52poke.com/wiki/摇晃舞
        /// </summary>
        public const int TEETER_DANCE = 298;
        /// <summary>
        /// 火焰踢 -> http://wiki.52poke.com/wiki/火焰踢
        /// </summary>
        public const int BLAZE_KICK = 299;
        /// <summary>
        /// 玩泥巴 -> http://wiki.52poke.com/wiki/玩泥巴
        /// </summary>
        public const int MUD_SPORT = 300;
        /// <summary>
        /// 冰球 -> http://wiki.52poke.com/wiki/冰球
        /// </summary>
        public const int ICE_BALL = 301;
        /// <summary>
        /// 针叶暗器 -> http://wiki.52poke.com/wiki/针叶暗器
        /// </summary>
        public const int NEEDLE_ARM = 302;
        /// <summary>
        /// 懒惰 -> http://wiki.52poke.com/wiki/懒惰
        /// </summary>
        public const int SLACK_OFF = 303;
        /// <summary>
        /// 高音 -> http://wiki.52poke.com/wiki/高音
        /// </summary>
        public const int HYPER_VOICE = 304;
        /// <summary>
        /// 毒液牙 -> http://wiki.52poke.com/wiki/毒液牙
        /// </summary>
        public const int POISON_FANG = 305;
        /// <summary>
        /// 突击爪 -> http://wiki.52poke.com/wiki/突击爪
        /// </summary>
        public const int CRUSH_CLAW = 306;
        /// <summary>
        /// 爆炸燃烧 -> http://wiki.52poke.com/wiki/爆炸燃烧
        /// </summary>
        public const int BLAST_BURN = 307;
        /// <summary>
        /// 水电炮 -> http://wiki.52poke.com/wiki/水电炮
        /// </summary>
        public const int HYDRO_CANNON = 308;
        /// <summary>
        /// 流星拳 -> http://wiki.52poke.com/wiki/流星拳
        /// </summary>
        public const int METEOR_MASH = 309;
        /// <summary>
        /// 惊吓 -> http://wiki.52poke.com/wiki/惊吓
        /// </summary>
        public const int ASTONISH = 310;
        /// <summary>
        /// 气象球 -> http://wiki.52poke.com/wiki/气象球
        /// </summary>
        public const int WEATHER_BALL = 311;
        /// <summary>
        /// 芳香治疗 -> http://wiki.52poke.com/wiki/芳香治疗
        /// </summary>
        public const int AROMATHERAPY = 312;
        /// <summary>
        /// 假哭 -> http://wiki.52poke.com/wiki/假哭
        /// </summary>
        public const int FAKE_TEARS = 313;
        /// <summary>
        /// 破空斩 -> http://wiki.52poke.com/wiki/破空斩
        /// </summary>
        public const int AIR_CUTTER = 314;
        /// <summary>
        /// 过热 -> http://wiki.52poke.com/wiki/过热
        /// </summary>
        public const int OVERHEAT = 315;
        /// <summary>
        /// 气味侦测 -> http://wiki.52poke.com/wiki/气味侦测
        /// </summary>
        public const int ODOR_SLEUTH = 316;
        /// <summary>
        /// 岩石封闭 -> http://wiki.52poke.com/wiki/岩石封闭
        /// </summary>
        public const int ROCK_TOMB = 317;
        /// <summary>
        /// 银色旋风 -> http://wiki.52poke.com/wiki/银色旋风
        /// </summary>
        public const int SILVER_WIND = 318;
        /// <summary>
        /// 金属音 -> http://wiki.52poke.com/wiki/金属音
        /// </summary>
        public const int METAL_SOUND = 319;
        /// <summary>
        /// 草笛魔音 -> http://wiki.52poke.com/wiki/草笛魔音
        /// </summary>
        public const int GRASS_WHISTLE = 320;
        /// <summary>
        /// 搔痒 -> http://wiki.52poke.com/wiki/搔痒
        /// </summary>
        public const int TICKLE = 321;
        /// <summary>
        /// 无限能量 -> http://wiki.52poke.com/wiki/无限能量
        /// </summary>
        public const int COSMIC_POWER = 322;
        /// <summary>
        /// 喷水 -> http://wiki.52poke.com/wiki/喷水
        /// </summary>
        public const int WATER_SPOUT = 323;
        /// <summary>
        /// 信号光束 -> http://wiki.52poke.com/wiki/信号光束
        /// </summary>
        public const int SIGNAL_BEAM = 324;
        /// <summary>
        /// 影子拳 -> http://wiki.52poke.com/wiki/影子拳
        /// </summary>
        public const int SHADOW_PUNCH = 325;
        /// <summary>
        /// 神通力 -> http://wiki.52poke.com/wiki/神通力
        /// </summary>
        public const int EXTRASENSORY = 326;
        /// <summary>
        /// 上天拳 -> http://wiki.52poke.com/wiki/上天拳
        /// </summary>
        public const int SKY_UPPERCUT = 327;
        /// <summary>
        /// 流沙地狱 -> http://wiki.52poke.com/wiki/流沙地狱
        /// </summary>
        public const int SAND_TOMB = 328;
        /// <summary>
        /// 绝对零度 -> http://wiki.52poke.com/wiki/绝对零度
        /// </summary>
        public const int SHEER_COLD = 329;
        /// <summary>
        /// 浊流 -> http://wiki.52poke.com/wiki/浊流
        /// </summary>
        public const int MUDDY_WATER = 330;
        /// <summary>
        /// 种子机关枪 -> http://wiki.52poke.com/wiki/种子机关枪
        /// </summary>
        public const int BULLET_SEED = 331;
        /// <summary>
        /// 回转攻 -> http://wiki.52poke.com/wiki/回转攻
        /// </summary>
        public const int AERIAL_ACE = 332;
        /// <summary>
        /// 冰针 -> http://wiki.52poke.com/wiki/冰针
        /// </summary>
        public const int ICICLE_SPEAR = 333;
        /// <summary>
        /// 铁壁 -> http://wiki.52poke.com/wiki/铁壁
        /// </summary>
        public const int IRON_DEFENSE = 334;
        /// <summary>
        /// 挡路 -> http://wiki.52poke.com/wiki/挡路
        /// </summary>
        public const int BLOCK = 335;
        /// <summary>
        /// 长嚎 -> http://wiki.52poke.com/wiki/长嚎
        /// </summary>
        public const int HOWL = 336;
        /// <summary>
        /// 龙爪 -> http://wiki.52poke.com/wiki/龙爪
        /// </summary>
        public const int DRAGON_CLAW = 337;
        /// <summary>
        /// 疯狂机器 -> http://wiki.52poke.com/wiki/疯狂机器
        /// </summary>
        public const int FRENZY_PLANT = 338;
        /// <summary>
        /// 健美 -> http://wiki.52poke.com/wiki/健美
        /// </summary>
        public const int BULK_UP = 339;
        /// <summary>
        /// 飞跳 -> http://wiki.52poke.com/wiki/飞跳
        /// </summary>
        public const int BOUNCE = 340;
        /// <summary>
        /// 泥巴射击 -> http://wiki.52poke.com/wiki/泥巴射击
        /// </summary>
        public const int MUD_SHOT = 341;
        /// <summary>
        /// 毒尾 -> http://wiki.52poke.com/wiki/毒尾
        /// </summary>
        public const int POISON_TAIL = 342;
        /// <summary>
        /// 渴望 -> http://wiki.52poke.com/wiki/渴望
        /// </summary>
        public const int COVET = 343;
        /// <summary>
        /// 伏特攻击 -> http://wiki.52poke.com/wiki/伏特攻击
        /// </summary>
        public const int VOLT_TACKLE = 344;
        /// <summary>
        /// 魔法叶 -> http://wiki.52poke.com/wiki/魔法叶
        /// </summary>
        public const int MAGICAL_LEAF = 345;
        /// <summary>
        /// 水之游 -> http://wiki.52poke.com/wiki/水之游
        /// </summary>
        public const int WATER_SPORT = 346;
        /// <summary>
        /// 冥想 -> http://wiki.52poke.com/wiki/冥想
        /// </summary>
        public const int CALM_MIND = 347;
        /// <summary>
        /// 刀叶 -> http://wiki.52poke.com/wiki/刀叶
        /// </summary>
        public const int LEAF_BLADE = 348;
        /// <summary>
        /// 龙之舞 -> http://wiki.52poke.com/wiki/龙之舞
        /// </summary>
        public const int DRAGON_DANCE = 349;
        /// <summary>
        /// 岩石爆破 -> http://wiki.52poke.com/wiki/岩石爆破
        /// </summary>
        public const int ROCK_BLAST = 350;
        /// <summary>
        /// 电击波 -> http://wiki.52poke.com/wiki/电击波
        /// </summary>
        public const int SHOCK_WAVE = 351;
        /// <summary>
        /// 水波动 -> http://wiki.52poke.com/wiki/水波动
        /// </summary>
        public const int WATER_PULSE = 352;
        /// <summary>
        /// 破灭愿望 -> http://wiki.52poke.com/wiki/破灭愿望
        /// </summary>
        public const int DOOM_DESIRE = 353;
        /// <summary>
        /// 精神提升 -> http://wiki.52poke.com/wiki/精神提升
        /// </summary>
        public const int PSYCHO_BOOST = 354;
        /// <summary>
        /// 歇息 -> http://wiki.52poke.com/wiki/歇息
        /// </summary>
        public const int ROOST = 355;
        /// <summary>
        /// 重力 -> http://wiki.52poke.com/wiki/重力
        /// </summary>
        public const int GRAVITY = 356;
        /// <summary>
        /// 奇迹眼 -> http://wiki.52poke.com/wiki/奇迹眼
        /// </summary>
        public const int MIRACLE_EYE = 357;
        /// <summary>
        /// 苏醒巴掌 -> http://wiki.52poke.com/wiki/苏醒巴掌
        /// </summary>
        public const int WAKEUP_SLAP = 358;
        /// <summary>
        /// 臂力拳 -> http://wiki.52poke.com/wiki/臂力拳
        /// </summary>
        public const int HAMMER_ARM = 359;
        /// <summary>
        /// 回转球 -> http://wiki.52poke.com/wiki/回转球
        /// </summary>
        public const int GYRO_BALL = 360;
        /// <summary>
        /// 治愈愿望 -> http://wiki.52poke.com/wiki/治愈愿望
        /// </summary>
        public const int HEALING_WISH = 361;
        /// <summary>
        /// 盐水 -> http://wiki.52poke.com/wiki/盐水
        /// </summary>
        public const int BRINE = 362;
        /// <summary>
        /// 自然之恩 -> http://wiki.52poke.com/wiki/自然之恩
        /// </summary>
        public const int NATURAL_GIFT = 363;
        /// <summary>
        /// 佯攻 -> http://wiki.52poke.com/wiki/佯攻
        /// </summary>
        public const int FEINT = 364;
        /// <summary>
        /// 啄食 -> http://wiki.52poke.com/wiki/啄食
        /// </summary>
        public const int PLUCK = 365;
        /// <summary>
        /// 顺风 -> http://wiki.52poke.com/wiki/顺风
        /// </summary>
        public const int TAILWIND = 366;
        /// <summary>
        /// 点穴 -> http://wiki.52poke.com/wiki/点穴
        /// </summary>
        public const int ACUPRESSURE = 367;
        /// <summary>
        /// 合金爆裂 -> http://wiki.52poke.com/wiki/合金爆裂
        /// </summary>
        public const int METAL_BURST = 368;
        /// <summary>
        /// 急速转变 -> http://wiki.52poke.com/wiki/急速转变
        /// </summary>
        public const int UTURN = 369;
        /// <summary>
        /// 近身打 -> http://wiki.52poke.com/wiki/近身打
        /// </summary>
        public const int CLOSE_COMBAT = 370;
        /// <summary>
        /// 报复 -> http://wiki.52poke.com/wiki/报复
        /// </summary>
        public const int PAYBACK = 371;
        /// <summary>
        /// 再三嘱咐 -> http://wiki.52poke.com/wiki/再三嘱咐
        /// </summary>
        public const int ASSURANCE = 372;
        /// <summary>
        /// 征收 -> http://wiki.52poke.com/wiki/征收
        /// </summary>
        public const int EMBARGO = 373;
        /// <summary>
        /// 投掷 -> http://wiki.52poke.com/wiki/投掷
        /// </summary>
        public const int FLING = 374;
        /// <summary>
        /// 幻象转移 -> http://wiki.52poke.com/wiki/幻象转移
        /// </summary>
        public const int PSYCHO_SHIFT = 375;
        /// <summary>
        /// 王牌 -> http://wiki.52poke.com/wiki/王牌
        /// </summary>
        public const int TRUMP_CARD = 376;
        /// <summary>
        /// 回复阻挡 -> http://wiki.52poke.com/wiki/回复阻挡
        /// </summary>
        public const int HEAL_BLOCK = 377;
        /// <summary>
        /// 拧干 -> http://wiki.52poke.com/wiki/拧干
        /// </summary>
        public const int WRING_OUT = 378;
        /// <summary>
        /// 能量骗术 -> http://wiki.52poke.com/wiki/能量骗术
        /// </summary>
        public const int POWER_TRICK = 379;
        /// <summary>
        /// 胃液 -> http://wiki.52poke.com/wiki/胃液
        /// </summary>
        public const int GASTRO_ACID = 380;
        /// <summary>
        /// 幸运咒语 -> http://wiki.52poke.com/wiki/幸运咒语
        /// </summary>
        public const int LUCKY_CHANT = 381;
        /// <summary>
        /// 先抢先赢 -> http://wiki.52poke.com/wiki/先抢先赢
        /// </summary>
        public const int ME_FIRST = 382;
        /// <summary>
        /// 仿造 -> http://wiki.52poke.com/wiki/仿造
        /// </summary>
        public const int COPYCAT = 383;
        /// <summary>
        /// 力量交换 -> http://wiki.52poke.com/wiki/力量交换
        /// </summary>
        public const int POWER_SWAP = 384;
        /// <summary>
        /// 防御交换 -> http://wiki.52poke.com/wiki/防御交换
        /// </summary>
        public const int GUARD_SWAP = 385;
        /// <summary>
        /// 惩罚 -> http://wiki.52poke.com/wiki/惩罚
        /// </summary>
        public const int PUNISHMENT = 386;
        /// <summary>
        /// 珍藏 -> http://wiki.52poke.com/wiki/珍藏
        /// </summary>
        public const int LAST_RESORT = 387;
        /// <summary>
        /// 烦恼种子 -> http://wiki.52poke.com/wiki/烦恼种子
        /// </summary>
        public const int WORRY_SEED = 388;
        /// <summary>
        /// 突袭 -> http://wiki.52poke.com/wiki/突袭
        /// </summary>
        public const int SUCKER_PUNCH = 389;
        /// <summary>
        /// 毒尖钉 -> http://wiki.52poke.com/wiki/毒尖钉
        /// </summary>
        public const int TOXIC_SPIKES = 390;
        /// <summary>
        /// 交换意志 -> http://wiki.52poke.com/wiki/交换意志
        /// </summary>
        public const int HEART_SWAP = 391;
        /// <summary>
        /// 水柱圈 -> http://wiki.52poke.com/wiki/水柱圈
        /// </summary>
        public const int AQUA_RING = 392;
        /// <summary>
        /// 电磁浮游 -> http://wiki.52poke.com/wiki/电磁浮游
        /// </summary>
        public const int MAGNET_RISE = 393;
        /// <summary>
        /// 爆炎电击 -> http://wiki.52poke.com/wiki/爆炎电击
        /// </summary>
        public const int FLARE_BLITZ = 394;
        /// <summary>
        /// 发劲 -> http://wiki.52poke.com/wiki/发劲
        /// </summary>
        public const int FORCE_PALM = 395;
        /// <summary>
        /// 波导弹 -> http://wiki.52poke.com/wiki/波导弹
        /// </summary>
        public const int AURA_SPHERE = 396;
        /// <summary>
        /// 磨光岩石 -> http://wiki.52poke.com/wiki/磨光岩石
        /// </summary>
        public const int ROCK_POLISH = 397;
        /// <summary>
        /// 毒刺 -> http://wiki.52poke.com/wiki/毒刺
        /// </summary>
        public const int POISON_JAB = 398;
        /// <summary>
        /// 恶波动 -> http://wiki.52poke.com/wiki/恶波动
        /// </summary>
        public const int DARK_PULSE = 399;
        /// <summary>
        /// 街头试刀 -> http://wiki.52poke.com/wiki/街头试刀
        /// </summary>
        public const int NIGHT_SLASH = 400;
        /// <summary>
        /// 水柱尾 -> http://wiki.52poke.com/wiki/水柱尾
        /// </summary>
        public const int AQUA_TAIL = 401;
        /// <summary>
        /// 种子爆弹 -> http://wiki.52poke.com/wiki/种子爆弹
        /// </summary>
        public const int SEED_BOMB = 402;
        /// <summary>
        /// 空气砍 -> http://wiki.52poke.com/wiki/空气砍
        /// </summary>
        public const int AIR_SLASH = 403;
        /// <summary>
        /// 剪刀十字拳 -> http://wiki.52poke.com/wiki/剪刀十字拳
        /// </summary>
        public const int XSCISSOR = 404;
        /// <summary>
        /// 虫鸣 -> http://wiki.52poke.com/wiki/虫鸣
        /// </summary>
        public const int BUG_BUZZ = 405;
        /// <summary>
        /// 龙波动 -> http://wiki.52poke.com/wiki/龙波动
        /// </summary>
        public const int DRAGON_PULSE = 406;
        /// <summary>
        /// 龙神俯冲 -> http://wiki.52poke.com/wiki/龙神俯冲
        /// </summary>
        public const int DRAGON_RUSH = 407;
        /// <summary>
        /// 力量宝石 -> http://wiki.52poke.com/wiki/力量宝石
        /// </summary>
        public const int POWER_GEM = 408;
        /// <summary>
        /// 吸收拳 -> http://wiki.52poke.com/wiki/吸收拳
        /// </summary>
        public const int DRAIN_PUNCH = 409;
        /// <summary>
        /// 真空波 -> http://wiki.52poke.com/wiki/真空波
        /// </summary>
        public const int VACUUM_WAVE = 410;
        /// <summary>
        /// 集气弹 -> http://wiki.52poke.com/wiki/集气弹
        /// </summary>
        public const int FOCUS_BLAST = 411;
        /// <summary>
        /// 能源球 -> http://wiki.52poke.com/wiki/能源球
        /// </summary>
        public const int ENERGY_BALL = 412;
        /// <summary>
        /// 神鸟特攻 -> http://wiki.52poke.com/wiki/神鸟特攻
        /// </summary>
        public const int BRAVE_BIRD = 413;
        /// <summary>
        /// 大地之力 -> http://wiki.52poke.com/wiki/大地之力
        /// </summary>
        public const int EARTH_POWER = 414;
        /// <summary>
        /// 掉包 -> http://wiki.52poke.com/wiki/掉包
        /// </summary>
        public const int SWITCHEROO = 415;
        /// <summary>
        /// 超极冲击 -> http://wiki.52poke.com/wiki/超极冲击
        /// </summary>
        public const int GIGA_IMPACT = 416;
        /// <summary>
        /// 诡计 -> http://wiki.52poke.com/wiki/诡计
        /// </summary>
        public const int NASTY_PLOT = 417;
        /// <summary>
        /// 飞弹拳 -> http://wiki.52poke.com/wiki/飞弹拳
        /// </summary>
        public const int BULLET_PUNCH = 418;
        /// <summary>
        /// 雪崩 -> http://wiki.52poke.com/wiki/雪崩
        /// </summary>
        public const int AVALANCHE = 419;
        /// <summary>
        /// 冰粒 -> http://wiki.52poke.com/wiki/冰粒
        /// </summary>
        public const int ICE_SHARD = 420;
        /// <summary>
        /// 影子钩爪 -> http://wiki.52poke.com/wiki/影子钩爪
        /// </summary>
        public const int SHADOW_CLAW = 421;
        /// <summary>
        /// 雷牙 -> http://wiki.52poke.com/wiki/雷牙
        /// </summary>
        public const int THUNDER_FANG = 422;
        /// <summary>
        /// 冰牙 -> http://wiki.52poke.com/wiki/冰牙
        /// </summary>
        public const int ICE_FANG = 423;
        /// <summary>
        /// 炎牙 -> http://wiki.52poke.com/wiki/炎牙
        /// </summary>
        public const int FIRE_FANG = 424;
        /// <summary>
        /// 影子偷袭 -> http://wiki.52poke.com/wiki/影子偷袭
        /// </summary>
        public const int SHADOW_SNEAK = 425;
        /// <summary>
        /// 泥巴爆弹 -> http://wiki.52poke.com/wiki/泥巴爆弹
        /// </summary>
        public const int MUD_BOMB = 426;
        /// <summary>
        /// 幻象斩 -> http://wiki.52poke.com/wiki/幻象斩
        /// </summary>
        public const int PSYCHO_CUT = 427;
        /// <summary>
        /// 意念头锤 -> http://wiki.52poke.com/wiki/意念头锤
        /// </summary>
        public const int ZEN_HEADBUTT = 428;
        /// <summary>
        /// 镜光射击 -> http://wiki.52poke.com/wiki/镜光射击
        /// </summary>
        public const int MIRROR_SHOT = 429;
        /// <summary>
        /// 光泽电炮 -> http://wiki.52poke.com/wiki/光泽电炮
        /// </summary>
        public const int FLASH_CANNON = 430;
        /// <summary>
        /// 攀登岩石 -> http://wiki.52poke.com/wiki/攀登岩石
        /// </summary>
        public const int ROCK_CLIMB = 431;
        /// <summary>
        /// 清除浓雾 -> http://wiki.52poke.com/wiki/清除浓雾
        /// </summary>
        public const int DEFOG = 432;
        /// <summary>
        /// 骗术空间 -> http://wiki.52poke.com/wiki/骗术空间
        /// </summary>
        public const int TRICK_ROOM = 433;
        /// <summary>
        /// 天龙流星 -> http://wiki.52poke.com/wiki/天龙流星
        /// </summary>
        public const int DRACO_METEOR = 434;
        /// <summary>
        /// 放电 -> http://wiki.52poke.com/wiki/放电
        /// </summary>
        public const int DISCHARGE = 435;
        /// <summary>
        /// 喷烟 -> http://wiki.52poke.com/wiki/喷烟
        /// </summary>
        public const int LAVA_PLUME = 436;
        /// <summary>
        /// 叶暴风 -> http://wiki.52poke.com/wiki/叶暴风
        /// </summary>
        public const int LEAF_STORM = 437;
        /// <summary>
        /// 能量鞭打 -> http://wiki.52poke.com/wiki/能量鞭打
        /// </summary>
        public const int POWER_WHIP = 438;
        /// <summary>
        /// 岩石炮 -> http://wiki.52poke.com/wiki/岩石炮
        /// </summary>
        public const int ROCK_WRECKER = 439;
        /// <summary>
        /// 十字毒药 -> http://wiki.52poke.com/wiki/十字毒药
        /// </summary>
        public const int CROSS_POISON = 440;
        /// <summary>
        /// 灰尘射击 -> http://wiki.52poke.com/wiki/灰尘射击
        /// </summary>
        public const int GUNK_SHOT = 441;
        /// <summary>
        /// 铁头 -> http://wiki.52poke.com/wiki/铁头
        /// </summary>
        public const int IRON_HEAD = 442;
        /// <summary>
        /// 磁铁爆弹 -> http://wiki.52poke.com/wiki/磁铁爆弹
        /// </summary>
        public const int MAGNET_BOMB = 443;
        /// <summary>
        /// 尖石攻击 -> http://wiki.52poke.com/wiki/尖石攻击
        /// </summary>
        public const int STONE_EDGE = 444;
        /// <summary>
        /// 诱惑 -> http://wiki.52poke.com/wiki/诱惑
        /// </summary>
        public const int CAPTIVATE = 445;
        /// <summary>
        /// 隐形岩 -> http://wiki.52poke.com/wiki/隐形岩
        /// </summary>
        public const int STEALTH_ROCK = 446;
        /// <summary>
        /// 打草结 -> http://wiki.52poke.com/wiki/打草结
        /// </summary>
        public const int GRASS_KNOT = 447;
        /// <summary>
        /// 喋喋不休 -> http://wiki.52poke.com/wiki/喋喋不休
        /// </summary>
        public const int CHATTER = 448;
        /// <summary>
        /// 制裁石砾 -> http://wiki.52poke.com/wiki/制裁石砾
        /// </summary>
        public const int JUDGMENT = 449;
        /// <summary>
        /// 虫咬 -> http://wiki.52poke.com/wiki/虫咬
        /// </summary>
        public const int BUG_BITE = 450;
        /// <summary>
        /// 充电光束 -> http://wiki.52poke.com/wiki/充电光束
        /// </summary>
        public const int CHARGE_BEAM = 451;
        /// <summary>
        /// 木槌 -> http://wiki.52poke.com/wiki/木槌
        /// </summary>
        public const int WOOD_HAMMER = 452;
        /// <summary>
        /// 喷射水柱 -> http://wiki.52poke.com/wiki/喷射水柱
        /// </summary>
        public const int AQUA_JET = 453;
        /// <summary>
        /// 攻击指令 -> http://wiki.52poke.com/wiki/攻击指令
        /// </summary>
        public const int ATTACK_ORDER = 454;
        /// <summary>
        /// 防御指令 -> http://wiki.52poke.com/wiki/防御指令
        /// </summary>
        public const int DEFEND_ORDER = 455;
        /// <summary>
        /// 回复指令 -> http://wiki.52poke.com/wiki/回复指令
        /// </summary>
        public const int HEAL_ORDER = 456;
        /// <summary>
        /// 双刃头锤 -> http://wiki.52poke.com/wiki/双刃头锤
        /// </summary>
        public const int HEAD_SMASH = 457;
        /// <summary>
        /// 双重攻击 -> http://wiki.52poke.com/wiki/双重攻击
        /// </summary>
        public const int DOUBLE_HIT = 458;
        /// <summary>
        /// 时间咆哮 -> http://wiki.52poke.com/wiki/时间咆哮
        /// </summary>
        public const int ROAR_OF_TIME = 459;
        /// <summary>
        /// 隔空切断 -> http://wiki.52poke.com/wiki/隔空切断
        /// </summary>
        public const int SPACIAL_REND = 460;
        /// <summary>
        /// 新月舞 -> http://wiki.52poke.com/wiki/新月舞
        /// </summary>
        public const int LUNAR_DANCE = 461;
        /// <summary>
        /// 捏碎 -> http://wiki.52poke.com/wiki/捏碎
        /// </summary>
        public const int CRUSH_GRIP = 462;
        /// <summary>
        /// 岩浆暴风 -> http://wiki.52poke.com/wiki/岩浆暴风
        /// </summary>
        public const int MAGMA_STORM = 463;
        /// <summary>
        /// 黑洞 -> http://wiki.52poke.com/wiki/黑洞
        /// </summary>
        public const int DARK_VOID = 464;
        /// <summary>
        /// 种子闪光 -> http://wiki.52poke.com/wiki/种子闪光
        /// </summary>
        public const int SEED_FLARE = 465;
        /// <summary>
        /// 奇异之风 -> http://wiki.52poke.com/wiki/奇异之风
        /// </summary>
        public const int OMINOUS_WIND = 466;
        /// <summary>
        /// 影子潜水 -> http://wiki.52poke.com/wiki/影子潜水
        /// </summary>
        public const int SHADOW_FORCE = 467;
        /// <summary>
        /// 磨爪子 -> http://wiki.52poke.com/wiki/磨爪子
        /// </summary>
        public const int HONE_CLAWS = 468;
        /// <summary>
        /// 全体防御 -> http://wiki.52poke.com/wiki/全体防御
        /// </summary>
        public const int WIDE_GUARD = 469;
        /// <summary>
        /// 防御平分 -> http://wiki.52poke.com/wiki/防御平分
        /// </summary>
        public const int GUARD_SPLIT = 470;
        /// <summary>
        /// 力量平分 -> http://wiki.52poke.com/wiki/力量平分
        /// </summary>
        public const int POWER_SPLIT = 471;
        /// <summary>
        /// 奇妙空间 -> http://wiki.52poke.com/wiki/奇妙空间
        /// </summary>
        public const int WONDER_ROOM = 472;
        /// <summary>
        /// 幻象攻击 -> http://wiki.52poke.com/wiki/幻象攻击
        /// </summary>
        public const int PSYSHOCK = 473;
        /// <summary>
        /// 毒液攻击 -> http://wiki.52poke.com/wiki/毒液攻击
        /// </summary>
        public const int VENOSHOCK = 474;
        /// <summary>
        /// 体重减轻 -> http://wiki.52poke.com/wiki/体重减轻
        /// </summary>
        public const int AUTOTOMIZE = 475;
        /// <summary>
        /// 愤怒之粉 -> http://wiki.52poke.com/wiki/愤怒之粉
        /// </summary>
        public const int RAGE_POWDER = 476;
        /// <summary>
        /// 心灵传动术 -> http://wiki.52poke.com/wiki/心灵传动术
        /// </summary>
        public const int TELEKINESIS = 477;
        /// <summary>
        /// 魔法空间 -> http://wiki.52poke.com/wiki/魔法空间
        /// </summary>
        public const int MAGIC_ROOM = 478;
        /// <summary>
        /// 击落 -> http://wiki.52poke.com/wiki/击落
        /// </summary>
        public const int SMACK_DOWN = 479;
        /// <summary>
        /// 山岚 -> http://wiki.52poke.com/wiki/山岚
        /// </summary>
        public const int STORM_THROW = 480;
        /// <summary>
        /// 爆炸火焰 -> http://wiki.52poke.com/wiki/爆炸火焰
        /// </summary>
        public const int FLAME_BURST = 481;
        /// <summary>
        /// 泥浆波 -> http://wiki.52poke.com/wiki/泥浆波
        /// </summary>
        public const int SLUDGE_WAVE = 482;
        /// <summary>
        /// 蝶之舞 -> http://wiki.52poke.com/wiki/蝶之舞
        /// </summary>
        public const int QUIVER_DANCE = 483;
        /// <summary>
        /// 沉重轰炸 -> http://wiki.52poke.com/wiki/沉重轰炸
        /// </summary>
        public const int HEAVY_SLAM = 484;
        /// <summary>
        /// 同步干扰 -> http://wiki.52poke.com/wiki/同步干扰
        /// </summary>
        public const int SYNCHRONOISE = 485;
        /// <summary>
        /// 电球 -> http://wiki.52poke.com/wiki/电球
        /// </summary>
        public const int ELECTRO_BALL = 486;
        /// <summary>
        /// 浸水 -> http://wiki.52poke.com/wiki/浸水
        /// </summary>
        public const int SOAK = 487;
        /// <summary>
        /// 火焰袭击 -> http://wiki.52poke.com/wiki/火焰袭击
        /// </summary>
        public const int FLAME_CHARGE = 488;
        /// <summary>
        /// 盘卷 -> http://wiki.52poke.com/wiki/盘卷
        /// </summary>
        public const int COIL = 489;
        /// <summary>
        /// 低空踢 -> http://wiki.52poke.com/wiki/低空踢
        /// </summary>
        public const int LOW_SWEEP = 490;
        /// <summary>
        /// 酸液炸弹 -> http://wiki.52poke.com/wiki/酸液炸弹
        /// </summary>
        public const int ACID_SPRAY = 491;
        /// <summary>
        /// 诈欺 -> http://wiki.52poke.com/wiki/诈欺
        /// </summary>
        public const int FOUL_PLAY = 492;
        /// <summary>
        /// 单纯光线 -> http://wiki.52poke.com/wiki/单纯光线
        /// </summary>
        public const int SIMPLE_BEAM = 493;
        /// <summary>
        /// 变为同伴 -> http://wiki.52poke.com/wiki/变为同伴
        /// </summary>
        public const int ENTRAINMENT = 494;
        /// <summary>
        /// 你先请 -> http://wiki.52poke.com/wiki/你先请
        /// </summary>
        public const int AFTER_YOU = 495;
        /// <summary>
        /// 轮唱 -> http://wiki.52poke.com/wiki/轮唱
        /// </summary>
        public const int ROUND = 496;
        /// <summary>
        /// 回音 -> http://wiki.52poke.com/wiki/回音
        /// </summary>
        public const int ECHOED_VOICE = 497;
        /// <summary>
        /// 循序渐进 -> http://wiki.52poke.com/wiki/循序渐进
        /// </summary>
        public const int CHIP_AWAY = 498;
        /// <summary>
        /// 清除迷雾 -> http://wiki.52poke.com/wiki/清除迷雾
        /// </summary>
        public const int CLEAR_SMOG = 499;
        /// <summary>
        /// 协助力量 -> http://wiki.52poke.com/wiki/协助力量
        /// </summary>
        public const int STORED_POWER = 500;
        /// <summary>
        /// 先制防御 -> http://wiki.52poke.com/wiki/先制防御
        /// </summary>
        public const int QUICK_GUARD = 501;
        /// <summary>
        /// 位置交换 -> http://wiki.52poke.com/wiki/位置交换
        /// </summary>
        public const int ALLY_SWITCH = 502;
        /// <summary>
        /// 热水 -> http://wiki.52poke.com/wiki/热水
        /// </summary>
        public const int SCALD = 503;
        /// <summary>
        /// 破壳 -> http://wiki.52poke.com/wiki/破壳
        /// </summary>
        public const int SHELL_SMASH = 504;
        /// <summary>
        /// 治愈波动 -> http://wiki.52poke.com/wiki/治愈波动
        /// </summary>
        public const int HEAL_PULSE = 505;
        /// <summary>
        /// 厄运临头 -> http://wiki.52poke.com/wiki/厄运临头
        /// </summary>
        public const int HEX = 506;
        /// <summary>
        /// 自由落体 -> http://wiki.52poke.com/wiki/自由落体
        /// </summary>
        public const int SKY_DROP = 507;
        /// <summary>
        /// 交换齿轮 -> http://wiki.52poke.com/wiki/交换齿轮
        /// </summary>
        public const int SHIFT_GEAR = 508;
        /// <summary>
        /// 巴投 -> http://wiki.52poke.com/wiki/巴投
        /// </summary>
        public const int CIRCLE_THROW = 509;
        /// <summary>
        /// 烧尽 -> http://wiki.52poke.com/wiki/烧尽
        /// </summary>
        public const int INCINERATE = 510;
        /// <summary>
        /// 延后 -> http://wiki.52poke.com/wiki/延后
        /// </summary>
        public const int QUASH = 511;
        /// <summary>
        /// 特技演员 -> http://wiki.52poke.com/wiki/特技演员
        /// </summary>
        public const int ACROBATICS = 512;
        /// <summary>
        /// 镜子系 -> http://wiki.52poke.com/wiki/镜子系
        /// </summary>
        public const int REFLECT_TYPE = 513;
        /// <summary>
        /// 讨敌 -> http://wiki.52poke.com/wiki/讨敌
        /// </summary>
        public const int RETALIATE = 514;
        /// <summary>
        /// 赌命 -> http://wiki.52poke.com/wiki/赌命
        /// </summary>
        public const int FINAL_GAMBIT = 515;
        /// <summary>
        /// 礼物传递 -> http://wiki.52poke.com/wiki/礼物传递
        /// </summary>
        public const int BESTOW = 516;
        /// <summary>
        /// 炼狱 -> http://wiki.52poke.com/wiki/炼狱
        /// </summary>
        public const int INFERNO = 517;
        /// <summary>
        /// 水的誓约 -> http://wiki.52poke.com/wiki/水的誓约
        /// </summary>
        public const int WATER_PLEDGE = 518;
        /// <summary>
        /// 炎的誓约 -> http://wiki.52poke.com/wiki/炎的誓约
        /// </summary>
        public const int FIRE_PLEDGE = 519;
        /// <summary>
        /// 草的誓约 -> http://wiki.52poke.com/wiki/草的誓约
        /// </summary>
        public const int GRASS_PLEDGE = 520;
        /// <summary>
        /// 交换伏特 -> http://wiki.52poke.com/wiki/交换伏特
        /// </summary>
        public const int VOLT_SWITCH = 521;
        /// <summary>
        /// 虫的抵抗 -> http://wiki.52poke.com/wiki/虫的抵抗
        /// </summary>
        public const int STRUGGLE_BUG = 522;
        /// <summary>
        /// 整地 -> http://wiki.52poke.com/wiki/整地
        /// </summary>
        public const int BULLDOZE = 523;
        /// <summary>
        /// 冰吸 -> http://wiki.52poke.com/wiki/冰吸
        /// </summary>
        public const int FROST_BREATH = 524;
        /// <summary>
        /// 龙尾 -> http://wiki.52poke.com/wiki/龙尾
        /// </summary>
        public const int DRAGON_TAIL = 525;
        /// <summary>
        /// 自我激励 -> http://wiki.52poke.com/wiki/自我激励
        /// </summary>
        public const int WORK_UP = 526;
        /// <summary>
        /// 电网 -> http://wiki.52poke.com/wiki/电网
        /// </summary>
        public const int ELECTROWEB = 527;
        /// <summary>
        /// 疯狂伏特 -> http://wiki.52poke.com/wiki/疯狂伏特
        /// </summary>
        public const int WILD_CHARGE = 528;
        /// <summary>
        /// 低空钻 -> http://wiki.52poke.com/wiki/低空钻
        /// </summary>
        public const int DRILL_RUN = 529;
        /// <summary>
        /// 双重劈 -> http://wiki.52poke.com/wiki/双重劈
        /// </summary>
        public const int DUAL_CHOP = 530;
        /// <summary>
        /// 爱心捣击 -> http://wiki.52poke.com/wiki/爱心捣击
        /// </summary>
        public const int HEART_STAMP = 531;
        /// <summary>
        /// 木角 -> http://wiki.52poke.com/wiki/木角
        /// </summary>
        public const int HORN_LEECH = 532;
        /// <summary>
        /// 圣剑 -> http://wiki.52poke.com/wiki/圣剑
        /// </summary>
        public const int SACRED_SWORD = 533;
        /// <summary>
        /// 贝壳刀 -> http://wiki.52poke.com/wiki/贝壳刀
        /// </summary>
        public const int RAZOR_SHELL = 534;
        /// <summary>
        /// 高温捣击 -> http://wiki.52poke.com/wiki/高温捣击
        /// </summary>
        public const int HEAT_CRASH = 535;
        /// <summary>
        /// 青草搅拌器 -> http://wiki.52poke.com/wiki/青草搅拌器
        /// </summary>
        public const int LEAF_TORNADO = 536;
        /// <summary>
        /// 疯狂滚动 -> http://wiki.52poke.com/wiki/疯狂滚动
        /// </summary>
        public const int STEAMROLLER = 537;
        /// <summary>
        /// 棉花防御 -> http://wiki.52poke.com/wiki/棉花防御
        /// </summary>
        public const int COTTON_GUARD = 538;
        /// <summary>
        /// 黑夜爆裂 -> http://wiki.52poke.com/wiki/黑夜爆裂
        /// </summary>
        public const int NIGHT_DAZE = 539;
        /// <summary>
        /// 精神破坏 -> http://wiki.52poke.com/wiki/精神破坏
        /// </summary>
        public const int PSYSTRIKE = 540;
        /// <summary>
        /// 扫巴掌 -> http://wiki.52poke.com/wiki/扫巴掌
        /// </summary>
        public const int TAIL_SLAP = 541;
        /// <summary>
        /// 暴风 -> http://wiki.52poke.com/wiki/暴风
        /// </summary>
        public const int HURRICANE = 542;
        /// <summary>
        /// 爆爆头突击 -> http://wiki.52poke.com/wiki/爆爆头突击
        /// </summary>
        public const int HEAD_CHARGE = 543;
        /// <summary>
        /// 齿轮飞盘 -> http://wiki.52poke.com/wiki/齿轮飞盘
        /// </summary>
        public const int GEAR_GRIND = 544;
        /// <summary>
        /// 火炎弹 -> http://wiki.52poke.com/wiki/火炎弹
        /// </summary>
        public const int SEARING_SHOT = 545;
        /// <summary>
        /// 破坏技术 -> http://wiki.52poke.com/wiki/破坏技术
        /// </summary>
        public const int TECHNO_BLAST = 546;
        /// <summary>
        /// 古老之歌 -> http://wiki.52poke.com/wiki/古老之歌
        /// </summary>
        public const int RELIC_SONG = 547;
        /// <summary>
        /// 神秘之剑 -> http://wiki.52poke.com/wiki/神秘之剑
        /// </summary>
        public const int SECRET_SWORD = 548;
        /// <summary>
        /// 冰冻世界 -> http://wiki.52poke.com/wiki/冰冻世界
        /// </summary>
        public const int GLACIATE = 549;
        /// <summary>
        /// 雷击 -> http://wiki.52poke.com/wiki/雷击
        /// </summary>
        public const int BOLT_STRIKE = 550;
        /// <summary>
        /// 青炎 -> http://wiki.52poke.com/wiki/青炎
        /// </summary>
        public const int BLUE_FLARE = 551;
        /// <summary>
        /// 炎之舞 -> http://wiki.52poke.com/wiki/炎之舞
        /// </summary>
        public const int FIERY_DANCE = 552;
        /// <summary>
        /// 冻结伏特 -> http://wiki.52poke.com/wiki/冻结伏特
        /// </summary>
        public const int FREEZE_SHOCK = 553;
        /// <summary>
        /// 寒冷闪光 -> http://wiki.52poke.com/wiki/寒冷闪光
        /// </summary>
        public const int ICE_BURN = 554;
        /// <summary>
        /// 大声咆哮 -> http://wiki.52poke.com/wiki/大声咆哮
        /// </summary>
        public const int SNARL = 555;
        /// <summary>
        /// 掉落冰柱 -> http://wiki.52poke.com/wiki/掉落冰柱
        /// </summary>
        public const int ICICLE_CRASH = 556;
        /// <summary>
        /// 胜利V热炎 -> http://wiki.52poke.com/wiki/胜利V热炎
        /// </summary>
        public const int VCREATE = 557;
        /// <summary>
        /// 十字交叉火焰 -> http://wiki.52poke.com/wiki/十字交叉火焰
        /// </summary>
        public const int FUSION_FLARE = 558;
        /// <summary>
        /// 十字交叉闪电 -> http://wiki.52poke.com/wiki/十字交叉闪电
        /// </summary>
        public const int FUSION_BOLT = 559;
        /// <summary>
        /// 飞冲落地 -> http://wiki.52poke.com/wiki/飞冲落地
        /// </summary>
        public const int FLYING_PRESS = 560;
        /// <summary>
        /// 垫返 -> http://wiki.52poke.com/wiki/垫返
        /// </summary>
        public const int MAT_BLOCK = 561;
        /// <summary>
        /// 打嗝 -> http://wiki.52poke.com/wiki/打嗝
        /// </summary>
        public const int BELCH = 562;
        /// <summary>
        /// 耕地 -> http://wiki.52poke.com/wiki/耕地
        /// </summary>
        public const int ROTOTILLER = 563;
        /// <summary>
        /// 黏黏网 -> http://wiki.52poke.com/wiki/黏黏网
        /// </summary>
        public const int STICKY_WEB = 564;
        /// <summary>
        /// 击倒针刺 -> http://wiki.52poke.com/wiki/击倒针刺
        /// </summary>
        public const int FELL_STINGER = 565;
        /// <summary>
        /// 幽灵潜水 -> http://wiki.52poke.com/wiki/幽灵潜水
        /// </summary>
        public const int PHANTOM_FORCE = 566;
        /// <summary>
        /// 万圣夜狂欢 -> http://wiki.52poke.com/wiki/万圣夜狂欢
        /// </summary>
        public const int TRICKORTREAT = 567;
        /// <summary>
        /// 战吼 -> http://wiki.52poke.com/wiki/战吼
        /// </summary>
        public const int NOBLE_ROAR = 568;
        /// <summary>
        /// 电浆阵雨 -> http://wiki.52poke.com/wiki/电浆阵雨
        /// </summary>
        public const int ION_DELUGE = 569;
        /// <summary>
        /// 抛物线充电 -> http://wiki.52poke.com/wiki/抛物线充电
        /// </summary>
        public const int PARABOLIC_CHARGE = 570;
        /// <summary>
        /// 森林诅咒 -> http://wiki.52poke.com/wiki/森林诅咒
        /// </summary>
        public const int FORESTS_CURSE = 571;
        /// <summary>
        /// 花瓣暴风雪 -> http://wiki.52poke.com/wiki/花瓣暴风雪
        /// </summary>
        public const int PETAL_BLIZZARD = 572;
        /// <summary>
        /// 冰冻干燥 -> http://wiki.52poke.com/wiki/冰冻干燥
        /// </summary>
        public const int FREEZEDRY = 573;
        /// <summary>
        /// 魅惑之音 -> http://wiki.52poke.com/wiki/魅惑之音
        /// </summary>
        public const int DISARMING_VOICE = 574;
        /// <summary>
        /// 放狠话 -> http://wiki.52poke.com/wiki/放狠话
        /// </summary>
        public const int PARTING_SHOT = 575;
        /// <summary>
        /// 翻倒 -> http://wiki.52poke.com/wiki/翻倒
        /// </summary>
        public const int TOPSYTURVY = 576;
        /// <summary>
        /// 吸取之吻 -> http://wiki.52poke.com/wiki/吸取之吻
        /// </summary>
        public const int DRAINING_KISS = 577;
        /// <summary>
        /// 狡猾防御 -> http://wiki.52poke.com/wiki/狡猾防御
        /// </summary>
        public const int CRAFTY_SHIELD = 578;
        /// <summary>
        /// 鲜花防御 -> http://wiki.52poke.com/wiki/鲜花防御
        /// </summary>
        public const int FLOWER_SHIELD = 579;
        /// <summary>
        /// 草原场地 -> http://wiki.52poke.com/wiki/草原场地
        /// </summary>
        public const int GRASSY_TERRAIN = 580;
        /// <summary>
        /// 薄雾场地 -> http://wiki.52poke.com/wiki/薄雾场地
        /// </summary>
        public const int MISTY_TERRAIN = 581;
        /// <summary>
        /// 送电 -> http://wiki.52poke.com/wiki/送电
        /// </summary>
        public const int ELECTRIFY = 582;
        /// <summary>
        /// 嬉戏 -> http://wiki.52poke.com/wiki/嬉戏
        /// </summary>
        public const int PLAY_ROUGH = 583;
        /// <summary>
        /// 妖精之风 -> http://wiki.52poke.com/wiki/妖精之风
        /// </summary>
        public const int FAIRY_WIND = 584;
        /// <summary>
        /// 月亮攻击 -> http://wiki.52poke.com/wiki/月亮攻击
        /// </summary>
        public const int MOONBLAST = 585;
        /// <summary>
        /// 爆音波 -> http://wiki.52poke.com/wiki/爆音波
        /// </summary>
        public const int BOOMBURST = 586;
        /// <summary>
        /// 妖精之锁 -> http://wiki.52poke.com/wiki/妖精之锁
        /// </summary>
        public const int FAIRY_LOCK = 587;
        /// <summary>
        /// 王者盾牌 -> http://wiki.52poke.com/wiki/王者盾牌
        /// </summary>
        public const int KINGS_SHIELD = 588;
        /// <summary>
        /// 做朋友 -> http://wiki.52poke.com/wiki/做朋友
        /// </summary>
        public const int PLAY_NICE = 589;
        /// <summary>
        /// 悄悄话 -> http://wiki.52poke.com/wiki/悄悄话
        /// </summary>
        public const int CONFIDE = 590;
        /// <summary>
        /// 钻石暴风 -> http://wiki.52poke.com/wiki/钻石暴风
        /// </summary>
        public const int DIAMOND_STORM = 591;
        /// <summary>
        /// 蒸汽喷发 -> http://wiki.52poke.com/wiki/蒸汽喷发
        /// </summary>
        public const int STEAM_ERUPTION = 592;
        /// <summary>
        /// 异次元虫洞 -> http://wiki.52poke.com/wiki/异次元虫洞
        /// </summary>
        public const int HYPERSPACE_HOLE = 593;
        /// <summary>
        /// 水手里剑 -> http://wiki.52poke.com/wiki/水手里剑
        /// </summary>
        public const int WATER_SHURIKEN = 594;
        /// <summary>
        /// 魔法火焰 -> http://wiki.52poke.com/wiki/魔法火焰
        /// </summary>
        public const int MYSTICAL_FIRE = 595;
        /// <summary>
        /// 刺针防御 -> http://wiki.52poke.com/wiki/刺针防御
        /// </summary>
        public const int SPIKY_SHIELD = 596;
        /// <summary>
        /// 芳香薄雾 -> http://wiki.52poke.com/wiki/芳香薄雾
        /// </summary>
        public const int AROMATIC_MIST = 597;
        /// <summary>
        /// 怪电波 -> http://wiki.52poke.com/wiki/怪电波
        /// </summary>
        public const int EERIE_IMPULSE = 598;
        /// <summary>
        /// 毒液陷阱 -> http://wiki.52poke.com/wiki/毒液陷阱
        /// </summary>
        public const int VENOM_DRENCH = 599;
        /// <summary>
        /// 粉尘 -> http://wiki.52poke.com/wiki/粉尘
        /// </summary>
        public const int POWDER = 600;
        /// <summary>
        /// 大地控制 -> http://wiki.52poke.com/wiki/大地控制
        /// </summary>
        public const int GEOMANCY = 601;
        /// <summary>
        /// 磁场操作 -> http://wiki.52poke.com/wiki/磁场操作
        /// </summary>
        public const int MAGNETIC_FLUX = 602;
        /// <summary>
        /// 快乐时光 -> http://wiki.52poke.com/wiki/快乐时光
        /// </summary>
        public const int HAPPY_HOUR = 603;
        /// <summary>
        /// 电气场地 -> http://wiki.52poke.com/wiki/电气场地
        /// </summary>
        public const int ELECTRIC_TERRAIN = 604;
        /// <summary>
        /// 魔法照耀 -> http://wiki.52poke.com/wiki/魔法照耀
        /// </summary>
        public const int DAZZLING_GLEAM = 605;
        /// <summary>
        /// 庆祝 -> http://wiki.52poke.com/wiki/庆祝
        /// </summary>
        public const int CELEBRATE = 606;
        /// <summary>
        /// 牵手 -> http://wiki.52poke.com/wiki/牵手
        /// </summary>
        public const int HOLD_HANDS = 607;
        /// <summary>
        /// 圆滑目光 -> http://wiki.52poke.com/wiki/圆滑目光
        /// </summary>
        public const int BABYDOLL_EYES = 608;
        /// <summary>
        /// 碰碰脸颊 -> http://wiki.52poke.com/wiki/碰碰脸颊
        /// </summary>
        public const int NUZZLE = 609;
        /// <summary>
        /// 手下留情 -> http://wiki.52poke.com/wiki/手下留情
        /// </summary>
        public const int HOLD_BACK = 610;
        /// <summary>
        /// 缠身 -> http://wiki.52poke.com/wiki/缠身
        /// </summary>
        public const int INFESTATION = 611;
        /// <summary>
        /// 增强拳 -> http://wiki.52poke.com/wiki/增强拳
        /// </summary>
        public const int POWERUP_PUNCH = 612;
        /// <summary>
        /// 死亡之翼 -> http://wiki.52poke.com/wiki/死亡之翼
        /// </summary>
        public const int OBLIVION_WING = 613;
        /// <summary>
        /// 万箭齐发 -> http://wiki.52poke.com/wiki/万箭齐发
        /// </summary>
        public const int THOUSAND_ARROWS = 614;
        /// <summary>
        /// 万波激震 -> http://wiki.52poke.com/wiki/万波激震
        /// </summary>
        public const int THOUSAND_WAVES = 615;
        /// <summary>
        /// 大地原力 -> http://wiki.52poke.com/wiki/大地原力
        /// </summary>
        public const int LANDS_WRATH = 616;
        /// <summary>
        /// 破灭之光 -> http://wiki.52poke.com/wiki/破灭之光
        /// </summary>
        public const int LIGHT_OF_RUIN = 617;
        /// <summary>
        /// 根源波动 -> http://wiki.52poke.com/wiki/根源波动
        /// </summary>
        public const int ORIGIN_PULSE = 618;
        /// <summary>
        /// 断崖之剑 -> http://wiki.52poke.com/wiki/断崖之剑
        /// </summary>
        public const int PRECIPICE_BLADES = 619;
        /// <summary>
        /// 画龙点睛 -> http://wiki.52poke.com/wiki/画龙点睛
        /// </summary>
        public const int DRAGON_ASCENT = 620;
        /// <summary>
        /// 异次元冲锋 -> http://wiki.52poke.com/wiki/异次元冲锋
        /// </summary>
        public const int HYPERSPACE_FURY = 621;
    }
}
