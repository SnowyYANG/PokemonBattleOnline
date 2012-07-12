using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Diagnostics.Contracts;
using LightStudio.PokemonBattle.Data;
using LightStudio.Tactic.DataModels.IO;

namespace LightStudio.PokemonBattle.Data
{
  public class ImageService
  {
    private readonly DataCollection dataCollection;
    private readonly ImageSource[] icons;

    internal ImageService(DataCollection data)
    {
      dataCollection = data;
      icons = new ImageSource[DataService.Pokemons.Count() + 1];
    }

    public ImageSource SubstituteFront
    { get; private set; }
    public ImageSource SubstituteBack
    { get; private set; }

    private ImageSource GetImage(string path, object id)
    {
      string absolutePath = dataCollection.GetAbsolutePath(string.Format("image\\{0}\\{1}.png", path, id));
      try
      {
        return new BitmapImage(new Uri(absolutePath, UriKind.Absolute));
      }
      catch
      {
        return null;
      }
    }
    public ImageSource GetPokemonIcon(int id)
    {
      if (icons[id] == null)
        try
        {
          icons[id] = new BitmapImage(new Uri(dataCollection.GetAbsolutePath(string.Format("image\\icon\\{0:000}.png", id)), UriKind.Absolute));
        }
        catch { }
      return icons[id];
    }
    public ImageSource GetPokemonMaleFront(int id)
    {
      return GetImage("front", id);
    }
    public ImageSource GetPokemonFemaleFront(int id)
    {
      ImageSource image = GetImage("front\\female", id);
      if (image == null) image = GetPokemonMaleFront(id);
      return image;
    }
    public ImageSource GetPokemonMaleBack(int id)
    {
      return GetImage("back", id);
    }
    public ImageSource GetPokemonFemaleBack(int id)
    {
      ImageSource image = GetImage("back\\female", id);
      if (image == null) image = GetPokemonMaleBack(id);
      return image;
    }
    public ImageSource GetSpFront(string id)
    {
      return GetImage("front\\sp", id);
    }
    public ImageSource GetSpBack(string id)
    {
      return GetImage("back\\sp", id);
    }
  }
}
