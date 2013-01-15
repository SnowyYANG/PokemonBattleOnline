using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Tactic.Network.Commands
{
  [DataContract(Namespace = Namespaces.JSON)]
  public class UserChanged<TE, TUE> : ClientCommand<TE, TUE>
  {
    public static UserChanged<TE, TUE> AddUser(int id, string name, TUE e)
    {
      return new UserChanged<TE, TUE>() { Id = id, Name = name, E = e };
    }
    public static UserChanged<TE, TUE> RemoveUser(int id)
    {
      return new UserChanged<TE, TUE>() { Id = id };
    }

    private UserChanged()
    {
    }

    [DataMember(Name = "a")]
    int Id;
    [DataMember(Name = "b", EmitDefaultValue = false)]
    string Name;
    [DataMember(Name = "c", EmitDefaultValue = false)]
    TUE E;

    public override void Execute(Client<TE, TUE> client)
    {
      if (Name == null) client.RemoveUser(Id);
      else client.AddUser(Id, Name, E);
    }
  }
}
