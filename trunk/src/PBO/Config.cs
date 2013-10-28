﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.PBO
{
  [DataContract(Namespace = PBOMarks.PBO)]
  public class Config : SimpleData
  {
    public static Config Current
    { get; private set; }

    public static void Load(string path)
    {
      try
      {
        Current = LoadFromXml<Config>(path);
      }
      catch
      {
        Current = new Config();
      }
      Current.Path = path;
    }

    private string Path;

    private int _pokemonNumber;
    [DataMember(EmitDefaultValue = false)]
    public int PokemonNumber
    {
      get
      {
        if (_pokemonNumber < 1) _pokemonNumber = 1;
        else
        {
          var max = RomData.Pokemons.Count();
          if (_pokemonNumber > max) _pokemonNumber = max;
        }
        return _pokemonNumber;
      }
      set { _pokemonNumber = value; }
    }
    
    [DataMember(EmitDefaultValue = false)]
    public int PokemonForm;
    
    [DataMember(EmitDefaultValue = false)]
    public string Server;

    [DataMember(EmitDefaultValue = false)]
    public string Name;

    private int _avatar;
    [DataMember(EmitDefaultValue = false)]
    public int Avatar
    {
      get
      {
        if (_avatar == 0)
        {
          _avatar = new Random().Next(651, 868);
          if (_avatar == 790 || _avatar == 856 || _avatar == 857 || _avatar == 858) _avatar = 821;
        }
        return _avatar;
      }
      set { _avatar = value; }
    }

    private Config()
    {
    }

    public void Save()
    {
      SaveXml(Path);
    }
  }
}
