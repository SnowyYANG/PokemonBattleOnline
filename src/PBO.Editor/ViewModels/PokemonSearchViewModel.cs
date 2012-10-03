using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.PBO.UIElements;

namespace LightStudio.PokemonBattle.PBO.Editor
{
  internal class PokemonSearchViewModel : INotifyPropertyChanged
  {
    #region SearchOptions

    public string Name
    { get; set; }

    public BattleType? BattleType
    { get; set; }

    public PokemonForme PokemonType
    { get; set; }

    public Ability Ability
    { get; set; }

    public Item Item
    { get; set; }

    #endregion

    public MenuCommand SearchCommand
    { get; private set; }
    private Visibility visibility;
    public Visibility Visibility
    {
      get
      {
        return visibility;
      }
      set
      {
        if (visibility != value)
        {
          visibility = value;
          OnPropertyChanged("Visibility");
        }
      }
    }

    public PokemonSearchViewModel()
    {
      SearchCommand = new MenuCommand("Search", Search);
      Visibility = System.Windows.Visibility.Collapsed;
    }

    private void Search()
    {
      if (BattleType == null && PokemonType == null && Ability == null && Item == null && string.IsNullOrWhiteSpace(Name)) return;
      Editor.ShowSearchResult(Search(Editor.GetAllPokemons()));
      Visibility = Visibility.Collapsed;
    }

    private PokemonViewModel[] Search(IEnumerable<PokemonViewModel> collection)
    {
      string lowerName = string.IsNullOrWhiteSpace(Name) ? null : Name.ToLower();
      return collection.Where(
          pokemonViewModel =>
          {
            PokemonCustomInfo pm = pokemonViewModel.Model;
            PokemonForme pmType = DataService.GetPokemonType(pm.PokemonTypeId);
            return (lowerName == null || pm.Name.ToLower().Contains(lowerName))
                && (BattleType == null || pmType.Type1 == BattleType || pmType.Type2 == BattleType)
                && (PokemonType == null || PokemonType == pmType)
                && (Ability == null || DataService.GetPokemonType(pm.PokemonTypeId).GetAbility(pm.AbilityIndex) == Ability)
                && (Item == null || pm.ItemId == Item.Id);
          }).ToArray();
    }


    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(string propertyName)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion

  }
}
