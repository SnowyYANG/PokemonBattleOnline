using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Data
{
  [DataContract(Namespace = PBOMarks.PBO)]
  public abstract class GameElement
  {
    [DataMember]
    public int Id { get; private set; }

    public GameElement(int id)
    {
      Id = id;
    }

    [DataMember(Name = "Name")]
    public string EnglishName { get; protected set; }

    [DataMember]
    private readonly string _japaneseName;
    public string JapaneseName
    { get { return _japaneseName; } }

    public abstract string Name { get; }
    public abstract string Description { get; }

    public override string ToString()
    {
      return Name;
    }
  }
}
