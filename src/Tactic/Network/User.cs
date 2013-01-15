using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace PokemonBattleOnline.Tactic.Network
{
  public class User<TUE>
  {
    internal User(int id, string name, TUE e)
    {
      _id = id;
      _name = name;
      _extension = e;
    }

    private readonly int _id;
    public int Id
    { get { return _id; } }
    private readonly string _name;
    public string Name
    { get { return _name; } }
    private readonly TUE _extension;
    public TUE E
    { get { return _extension; } }
  }
}
