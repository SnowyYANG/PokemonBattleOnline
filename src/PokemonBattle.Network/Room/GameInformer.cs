using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using LightStudio.PokemonBattle.Interactive;

namespace LightStudio.PokemonBattle.Room
{
  internal interface IGameInformer
  {
    void InformTimeUp(IList<int> breakers);
    void InformTimeElapsed(int remainingSeconds);
    void InformGameResult(int[] gameResult, bool isStoped);
    void InformReportUpdate(ReportFragment fragment); //all

    void InformPmAdditional(PokemonAdditionalInfo pms);
    
    void InformRequestTie();
    void InformTieRejected();
    void InformRequireInput();
    void InformInputFail();
    void InformInputSucceed();
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  class TimeUpInfo : IUserInformation
  {
    [DataMember(EmitDefaultValue = false)]
    public IList<int> Breakers
    { get; private set; }

    public TimeUpInfo(IList<int> breakers)
    {
      this.Breakers = breakers;
    }
    void IUserInformation.Execute(IUser user)
    {
      user.InformTimeUp(Breakers);
    }
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  class TimeElapsedInfo : IUserInformation
  {
    [DataMember]
    readonly int RemainingTime;

    public TimeElapsedInfo(int remainingTime)
    {
      this.RemainingTime = remainingTime;
    }
    void IUserInformation.Execute(IUser user)
    {
      user.InformTimeElapsed(RemainingTime);
    }
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  class GameResultInfo : IUserInformation
  {
    public static GameResultInfo GameTie()
    {
      GameResultInfo info = new GameResultInfo(true);
      return info;
    }
    public static GameResultInfo GameStop()
    {
      GameResultInfo info = new GameResultInfo(true);
      info.IsStoped = true;
      return info;
    }
    public static GameResultInfo GameResult(int team0, int team1)
    {
      GameResultInfo info = new GameResultInfo();
      info.RemaningPokemons[0] = team0;
      info.RemaningPokemons[1] = team1;
      return info;
    }
    /// <summary>
    /// null if user tie
    /// </summary>
    [DataMember(EmitDefaultValue = false)]
    readonly int[] RemaningPokemons;

    [DataMember(EmitDefaultValue = false)]
    bool IsStoped;

    private GameResultInfo(bool useNull = false)
    {
      if (useNull) RemaningPokemons = null;
      else RemaningPokemons = new int[2];
    }
    void IUserInformation.Execute(IUser user)
    {
      user.InformGameResult(RemaningPokemons, IsStoped);
    }
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  class ReportUpdateInfo : IUserInformation
  {
    [DataMember]
    ReportFragment Fragment;

    public ReportUpdateInfo(ReportFragment turn)
    {
      Fragment = turn;
    }
    void IUserInformation.Execute(IUser user)
    {
      user.InformReportUpdate(Fragment);
    }
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  class PmAddionalInfo : IUserInformation
  {
    [DataMember]
    PokemonAdditionalInfo PmInfo;

    public PmAddionalInfo(PokemonAdditionalInfo pmInfo)
    {
      PmInfo = pmInfo;
    }
    void IUserInformation.Execute(IUser user)
    {
      user.InformPmAdditional(PmInfo);
    }
  }
  
  [DataContract(Namespace = Namespaces.DEFAULT)]
  class RequestTieInfo : IUserInformation
  {
    void IUserInformation.Execute(IUser user)
    {
      user.InformRequestTie();
    }
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  class RejectTieInfo : IUserInformation
  {
    void IUserInformation.Execute(IUser user)
    {
      user.InformTieRejected();
    }
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  class InputFailInfo : IUserInformation
  {
    void IUserInformation.Execute(IUser user)
    {
      user.InformInputFail();
    }
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  class RequireInputInfo : IUserInformation
  {
    void IUserInformation.Execute(IUser user)
    {
      user.InformRequireInput();
    }
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  class InputSucceedInfo : IUserInformation
  {
    void IUserInformation.Execute(IUser user)
    {
      user.InformInputSucceed();
    }
  }
}