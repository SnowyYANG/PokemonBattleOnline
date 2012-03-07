using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.Tactic.DataModels;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.Interactive
{
  [DataContract(Namespace = Namespaces.DEFAULT)]
  public abstract class GameEvent
  {
    #region static
    #region event types
    protected const string ERROR = "Error"; //-1
    protected const string GAMESTART = "GameStart";
    protected const string BEGINTURN = "BeginTurn";
    protected const string ENDTURN = "EndTurn";
    protected const string SENDOUT = "SendOut";
    protected const string WITHDRAW = "WithDraw";
    protected const string MOVE = "Move";
    protected const string EXERTSKILL = "ExertSkill";
    protected const string EXERTSKILLONTARGET = "ExertSkillOnTarget";
    protected const string SKILLMISS = "SkillMiss";
    protected const string ATTACKTARGET = "AttackTarget";
    //protected const string Quit = 10;
    protected const string HPCHANGE = "HpChanged";
    //protected const string MpChange = 12;
    //protected const string PPChange = 13;
    protected const string MOVEFAIL = "MoveFail";
    protected const string ADDSTATE = "AddState";
    protected const string REMOVESTATE = "RemoveState";
    protected const string UPDATESTATE = "UpdateState";
    //protected const string WithdrawFail = 18;
    //protected const string SwitchFail = 19;
    //protected const string SendoutFail = 20;
    //protected const string ClearState = 21;
    #endregion

    protected static IText GetGameLog(string type)
    {
      return DataService.GameLog[type];
    }
    #endregion

    public abstract IText GetGameLog(GameOutward game);
    public virtual void Update(GameOutward game)
    {
    }
    public virtual void Update(SimGame game)
    {
    }
  }
}
