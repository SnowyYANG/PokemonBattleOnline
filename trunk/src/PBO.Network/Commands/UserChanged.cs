using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Network.Commands
{
  [DataContract(Namespace = PBOMarks.JSON)]
  public class UserChanged : S2C
  {
    public static UserChanged AddUser(int id, string name, ushort avatar)
    {
      return new UserChanged() { Id = id, Name = name, Avatar = avatar };
    }
    public static UserChanged RemoveUser(int id)
    {
      return new UserChanged() { Id = id };
    }

    private UserChanged()
    {
    }

    [DataMember(Name = "a")]
    int Id;
    [DataMember(Name = "b", EmitDefaultValue = false)]
    string Name;
    [DataMember(Name = "c", EmitDefaultValue = false)]
    ushort Avatar;

    public override void Execute(Client client)
    {
      if (Name == null) client.State.RemoveUser(Id);
      else
      {
        var u = new User(Id, Name, Avatar);
        client.State.AddUser(u);
      }
    }
  }
}
