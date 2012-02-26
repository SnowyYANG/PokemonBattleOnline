using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.PBO.Editor
{
  internal static class Editor
  {
    //public static event Action<int> FoldersSwitch = delegate { };
    private static int openFolderCount = 0;
    public static readonly EditorViewModel CurrentEditor = new EditorViewModel(Helper.DataMainInstance.PokemonData);

    public static void Open(this IFolderViewModel folder)
    {
      //if (CurrentEditor != null)
      CurrentEditor.OpenFolder(folder);
      ++openFolderCount;
      //FoldersSwitch(++openFolderCount);
    }

    public static void Close(this IFolderViewModel folder)
    {
      //if (CurrentEditor != null)
      CurrentEditor.CloseFolder(folder);
      --openFolderCount;
      //FoldersSwitch(--openFolderCount);
    }

    public static void Recycle(this PokemonViewModel pm)
    {
      //if (CurrentEditor != null)
      CurrentEditor.RecyclePokemon(pm);
    }

    #region Editing

    public static void Edit(this PokemonViewModel pm)
    {
      //if (CurrentEditor != null)
      CurrentEditor.EditPokemon(pm);
    }

    private static bool IsEditing(this PokemonViewModel pm)
    {
      //if (CurrentEditor == null)
      //   return false;
      return CurrentEditor.EditingPokemon.PokemonViewModel == pm;
    }

    public static void EndEditing(this PokemonViewModel pm)
    {
      if (pm.IsEditing())
        CurrentEditor.EndEditing();
    }

    #endregion

    #region Search

    public static IEnumerable<PokemonViewModel> GetAllPokemons()
    {
      //if (CurrentEditor == null)
      //    return Enumerable.Empty<PokemonViewModel>();
      var folders = CurrentEditor.Teams.Folders.Concat(CurrentEditor.Boxes.Folders);
      return from folder in folders
             from pm in folder.Pokemons
             select pm;
    }

    public static void ShowSearchResult(IList<PokemonViewModel> result)
    {
      //if (CurrentEditor != null)
      CurrentEditor.ShowSearchResult(new SearchResultViewModel(result));
    }

    public static void Close(this SearchResultViewModel result)
    {
      //if (CurrentEditor != null)
      CurrentEditor.CloseSearchResult(result);
    }
    #endregion
  }
}
