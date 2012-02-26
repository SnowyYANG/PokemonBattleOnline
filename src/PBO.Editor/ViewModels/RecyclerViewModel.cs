using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using LightStudio.PokemonBattle.Data;
using LightStudio.Tactic.Globalization;
using LightStudio.PokemonBattle.PBO.UIElements;

namespace LightStudio.PokemonBattle.PBO.Editor
{
  internal class RecyclerViewModel : FolderViewModelBase
  {
    static readonly object OPENICON;
    static readonly object ICON;
    static RecyclerViewModel()
    {
      OPENICON = PBO.Helper.GetImage(@"Balls/NetOpen.png");
      ICON = PBO.Helper.GetImage(@"Balls/Net.png");
    }

    private IPokemonRecycler model;
    private LocalizedString nameString;
    private MenuCommand increaseSize;
    private MenuCommand decreaseSize;

    public RecyclerViewModel(IPokemonRecycler model)
      : base(model)
    {
      this.model = model;
      nameString = DataService.String.GetLocalizedString(Model.Name);
      nameString.PropertyChanged += (sender, e) => OnPropertyChanged("Name");
      increaseSize = new MenuCommand("增加回收站容量", IncreaseSize);
      decreaseSize = new MenuCommand("减少回收站容量", DecreaseSize);
      RefreshChangeSizeEnable();
      PokemonCommands.Add(increaseSize);//
      PokemonCommands.Add(decreaseSize);
    }
    public override object BorderBrush
    {
      get
      {
        return System.Windows.Media.Brushes.Gray;
      }
    }
    public override object Background
    {
      get
      {
        return @"#25110E";
      }
    }
    public override object Icon
    {
      get
      {
        if (IsOpen) return OPENICON;
        return ICON;
      }
    }
    public override string Name
    {
      get
      {
        return nameString.Value;
      }
      set
      { }
    }

    private void IncreaseSize()
    {
      if (Size <= 60)
      {
        model.ChangeSize(Size << 1);
        OnPropertyChanged("Size");
        RefreshChangeSizeEnable();
      }
    }
    private void DecreaseSize()
    {
      if (Size >= 30)
      {
        model.ChangeSize(Size >> 1);
        OnPropertyChanged("Size");
        RefreshChangeSizeEnable();
      }
    }
    private void RefreshChangeSizeEnable()
    {
      increaseSize.IsEnabled = Size <= 60;
      decreaseSize.IsEnabled = Size >= 30;
    }
    public void RecyclePokemon(PokemonViewModel pm)
    {
      if (!Pokemons.Contains(pm))
      {
        InsertPokemon(Pokemons.Count, pm);
      }
      else//remove from recycler
      {
        pm.EndEditing();
      }
    }
  }
}
