using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace LightStudio.PokemonBattle.Data
{
  public enum AttachedState : sbyte
  {
    None,
    Paralysis = 0x01,
    Sleep = 0x02,
    Freeze = 0x03,
    Burn = 0x04,
    Poison = 0x05, //毒和剧毒都是，按持续时间分别
    Confusion = 0x06,
    Infatuation = 0x07, //着迷
    Trapped = 0x08,
    Nightmare = 0x09,
    Torment = 0x0C,
    Disable = 0x0D,
    Yawn = 0x0E,
    HealBlock = 0x0F,
    CanAttack = 0x11, //嗅觉，奇迹之眼，并不是让技能类型本身修正>=1...
    LeechSeed = 0x12,
    Embargo = 0x13, //扣押，5回合内不得使用道具
    PerishSong = 0x14,
    Ingrain = 0x15, //扎根
    Special = -1 //三角攻击 击坠 念动力
  }
  
  public enum AttachedCategory : byte
  {
    None,
    Forever,
    LimitedTime,
    HeterosexOnly,
    Trapped //强韧之爪
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  public class MoveAttachment
  {
    [DataMember]
    public AttachedState State { get; private set; }
    [DataMember]
    public byte Probability { get; private set; } //0和100都是必中，但有区别
    [DataMember]
    public AttachedCategory Category { get; private set; }
    [DataMember]
    public byte MinTurn { get; private set; }
    [DataMember]
    public byte MaxTurn { get; private set; }
  }
}
