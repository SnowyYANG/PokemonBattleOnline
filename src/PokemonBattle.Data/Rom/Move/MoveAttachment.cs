using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Data
{
  public enum AttachedState : sbyte
  {
    None,
    PAR = 0x01,
    SLP = 0x02,
    FRZ = 0x03,
    BRN = 0x04,
    /// <summary>
    /// 毒和剧毒都是，按持续时间分别
    /// </summary>
    PSN = 0x05,
    Confuse = 0x06,
    /// <summary>
    /// //着迷
    /// </summary>
    Attract = 0x07, 
    Trap = 0x08,
    Nightmare = 0x09,
    /// <summary>
    /// 寻衅
    /// </summary>
    Torment = 0x0C,
    Disable = 0x0D,
    Yawn = 0x0E,
    HealBlock = 0x0F,
    /// <summary>
    /// 嗅觉，奇迹之眼
    /// </summary>
    CanAttack = 0x11,
    LeechSeed = 0x12,
    /// <summary>
    /// 扣押，5回合内不得使用道具
    /// </summary>
    Embargo = 0x13,
    PerishSong = 0x14,
    /// <summary>
    /// 扎根
    /// </summary>
    Ingrain = 0x15,
    Special = -1 //三角攻击 击坠 念动力
  }
  
  public enum AttachedCategory : byte
  {
    None,
    Forever,
    LimitedTime,
    HeterosexOnly,
    Trap //强韧之爪
  }

  [DataContract(Namespace = Namespaces.PBO)]
  public class MoveAttachment
  {
    [DataMember]
    public AttachedState State { get; private set; }
    [DataMember(EmitDefaultValue = false)]
    public byte Probability { get; private set; }
    [DataMember]
    public AttachedCategory Category { get; private set; }
    [DataMember(EmitDefaultValue = false)]
    public byte MinTurn { get; private set; }
    [DataMember(EmitDefaultValue = false)]
    public byte MaxTurn { get; private set; }
  }
}
