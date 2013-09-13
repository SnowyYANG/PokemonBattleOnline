using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Game
{
  [DataContract(Namespace = PBOMarks.PBO)]
  public class UserData : SimpleData
  {
    public static UserData Current { get; private set; }
    public static void Load(string fileName)
    {
      try
      {
        Current = LoadFromDat<UserData>(fileName);
      }
      catch
      {
        Current = new UserData();
      }
      Current.fileName = fileName;
      PokemonBT.PokemonDeleted += Current.Recycler.Add;
    }

    private string fileName;

    private UserData()
    {
      Boxes = new CollectionGroup();
      Teams = new CollectionGroup();
      Recycler = new PokemonRecycler();
    }

    private CollectionGroup _boxes;
    [DataMember]
    public CollectionGroup Boxes
    { 
      get { return _boxes; }
      set
      {
        _boxes = value;
        _boxes.CollectionSize = 30;
      }
    }
    [DataMember]
    private string[] BoxesNames
    {
      get { return Boxes.Select((b) => b.Name).ToArray(); }
      set
      {
        int i = 0;
        foreach (var box in Boxes) box.Name = value[i++];
      }
    }
    private CollectionGroup _teams;
    [DataMember]
    public CollectionGroup Teams
    { 
      get { return _teams; }
      set
      {
        _teams = value;
        _teams.CollectionSize = 6;
      }
    }
    [DataMember]
    private string[] TeamsNames
    {
      get { return Teams.Select((t) => t.Name).ToArray(); }
      set
      {
        int i = 0;
        foreach (var team in Teams) team.Name = value[i++];
      }
    }
    private PokemonRecycler _recycler;
    [DataMember]
    public PokemonRecycler Recycler
    { 
      get { return _recycler; }
      set
      {
        _recycler = value;
        _recycler.Size = 100;
      }
    }

    public void Save(string fileName)
    {
      SaveDat(fileName);
    }
    public void Save()
    {
      Save(fileName);
    }
  }
}
