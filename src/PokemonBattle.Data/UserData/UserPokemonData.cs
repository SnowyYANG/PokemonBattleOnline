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
  internal class UserPokemonData : IUserPokemonData, ISaveObject
  {
    private DataSaver dataSaver;

    public IPokemonCollection Teams
    { get; private set; }

    public IPokemonCollection Boxes
    { get; private set; }

    public IPokemonRecycler Recycler
    { get; private set; }

    public int SaveInterval
    {
      get
      {
        return dataSaver.SaveInterval;
      }
      set
      {
        dataSaver.SaveInterval = value;
      }
    }

    private UserPokemonData()
    { }

    public void Save()
    {
      dataSaver.Save();
    }

    void ISaveObject.Save()
    {
      using (DeflateStream stream = new DeflateStream(
          DataIOHelper.CreateFile(CONSTS.USER_DATA_FILE, FileMode.Create),
          CompressionMode.Compress))
      {
        Save(stream);
      }
    }

    private void Save(Stream stream)
    {
      Serializer.Serialize(new UserPokemonDataInfo(this), stream);
    }

    public static UserPokemonData Load()
    {
      try
      {
        if (File.Exists(DataIOHelper.GetFullPath(CONSTS.USER_DATA_FILE)))
        {
          using (DeflateStream stream = new DeflateStream(
              DataIOHelper.OpenFile(CONSTS.USER_DATA_FILE, FileMode.Open),
              CompressionMode.Decompress))
          {
            return UserPokemonData.Load(stream);
          }
        }
      }
      catch (Exception e)
      { }
      return UserPokemonData.Create();
    }

    private static UserPokemonData Load(Stream stream)
    {
      var data = new UserPokemonData();
      UserPokemonDataInfo dataInfo = Serializer.Deserialize<UserPokemonDataInfo>(stream);
      data.Teams = dataInfo.Teams.ToCollection(UserPokemonData.TeamSize);
      data.Boxes = dataInfo.Boxes.ToCollection(UserPokemonData.BoxSize);
      data.Recycler = dataInfo.Recycler.ToRecycle();
      data.dataSaver = new DataSaver(data, dataInfo.SaveInterval);
      return data;
    }

    private static UserPokemonData Create()
    {
      var data = new UserPokemonData();
      data.Teams = new PokemonCollection(UserPokemonData.TeamSize);
      data.Boxes = new PokemonCollection(UserPokemonData.BoxSize);
      data.Recycler = new PokemonRecycler(UserPokemonData.DefaultRecyclerSize);
      data.dataSaver = new DataSaver(data, UserPokemonData.DefaultSaveInterval);
      return data;
    }

    public const int TeamSize = 6;
    public const int BoxSize = 50;
    public const int DefaultRecyclerSize = 30;
    public const int DefaultSaveInterval = 5;
  }
}
