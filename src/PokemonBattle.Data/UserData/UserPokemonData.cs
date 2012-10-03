using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using LightStudio.Tactic.Serialization;

namespace LightStudio.PokemonBattle.Data
{
  internal class UserPokemonData : IUserPokemonData
  {
    private const string USER_DATA_FILE = "user_pm.dat";

    public static UserPokemonData Load()
    {
      try
      {
        using (FileStream fs = new FileStream(USER_DATA_FILE, FileMode.Open))
        using (DeflateStream stream = new DeflateStream(fs, CompressionMode.Decompress))
          return UserPokemonData.Load(stream);
      }
      catch (Exception e)
      {
        return UserPokemonData.Create();
      }
    }

    public IPokemonCollection Teams
    { get; private set; }

    public IPokemonCollection Boxes
    { get; private set; }

    public IPokemonRecycler Recycler
    { get; private set; }

    private UserPokemonData()
    { }

    public void Save()
    {
      using (FileStream fs = new FileStream(USER_DATA_FILE, FileMode.Create))
      using (DeflateStream stream = new DeflateStream(fs, CompressionMode.Compress))
        Save(stream);
    }

    private void Save(Stream stream)
    {
      Serializer.Serialize(new UserPokemonDataInfo(this), stream);
    }

    private static UserPokemonData Load(Stream stream)
    {
      var data = new UserPokemonData();
      UserPokemonDataInfo dataInfo = Serializer.Deserialize<UserPokemonDataInfo>(stream);
      data.Teams = dataInfo.Teams.ToCollection(UserPokemonData.TeamSize);
      data.Boxes = dataInfo.Boxes.ToCollection(UserPokemonData.BoxSize);
      data.Recycler = dataInfo.Recycler.ToRecycle();
      return data;
    }

    private static UserPokemonData Create()
    {
      var data = new UserPokemonData();
      data.Teams = new PokemonCollection(UserPokemonData.TeamSize);
      data.Boxes = new PokemonCollection(UserPokemonData.BoxSize);
      data.Recycler = new PokemonRecycler(UserPokemonData.DefaultRecyclerSize);
      return data;
    }

    public const int TeamSize = 6;
    public const int BoxSize = 50;
    public const int DefaultRecyclerSize = 30;
  }
}
