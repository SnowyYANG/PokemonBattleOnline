using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace LightStudio.PokemonBattle.Game
{
  [DataContract(Namespace = Namespaces.DEFAULT)]
  public class TeamOutward : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    [DataMember(EmitDefaultValue = false)]
    public int Normal { get; private set; }
    [DataMember(EmitDefaultValue = false)]
    public int Abnormal { get; private set; }
    [DataMember(EmitDefaultValue = false)]
    public int Dying { get; private set; }

    internal TeamOutward(int normal, int abnormal, int dying)
    {
      Normal = normal;
      Abnormal = abnormal;
      Dying = dying;
    }

    internal void Update(TeamOutward team)
    {
      if (Normal != team.Normal || Abnormal != team.Abnormal || Dying != team.Dying)
      {
        Normal = team.Normal;
        Abnormal = team.Abnormal;
        Dying = team.Dying;
        //很少有只变一个数字的，干脆null
        if (PropertyChanged != null)
          PropertyChanged(this, new PropertyChangedEventArgs(null));
      }
    }
  }
}
