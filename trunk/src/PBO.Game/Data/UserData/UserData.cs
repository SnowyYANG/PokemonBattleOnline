using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

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
    }

    private string fileName;

    private UserData()
    {
      _teams = new ObservableList<PokemonTeam>();
    }

    private ObservableList<PokemonTeam> _teams;
    public ObservableList<PokemonTeam> Teams
    { 
      get
      {
        if (_teams == null)
        {
          _teams = new ObservableList<PokemonTeam>();
          _teams.Add(null);
        }
        return _teams;
      }
    }
    [DataMember]
    private PokemonTeam[] teams
    {
      get { return _teams.ToArray(); }
      set
      {
        _teams = new ObservableList<PokemonTeam>(value);
        if (_teams.FirstOrDefault() != null) _teams.Insert(0, null);
      }
    }

    public void ImportTeam(string text)
    {
      var pms = new PokemonData[6];
      Helper.Import(text, pms);
      Teams.Add(new PokemonTeam(pms));
    }
    public void AddTeam()
    {
      Teams.Insert(1, new PokemonTeam());
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
