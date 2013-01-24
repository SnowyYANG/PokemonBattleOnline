using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Network.Lobby.Commands
{
  [DataContract(Namespace = Namespaces.JSON)]
  public class UserChanged : ClientCommand
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
      if (Name == null) client.RemoveUser(Id);
      else client.AddUser(Id, Name, Avatar);
    }
  }
}
