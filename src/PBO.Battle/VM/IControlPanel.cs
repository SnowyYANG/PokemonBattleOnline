using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using LightStudio.PokemonBattle.Game;
using LightStudio.PokemonBattle.Interactive;

namespace LightStudio.PokemonBattle.PBO.Battle.VM
{
  internal static class ControlPanelIndex
  {
    public const int INACTIVE = -1;
    public const int MAIN = 0;
    public const int FIGHT = 1;
    public const int POKEMONS = 2;
    public const int STOP = 3;
    public const int TARGET = 4;
  }
  interface IControlPanel : INotifyPropertyChanged
  {
    event Action<string> InputFailed;

    int SelectedPanel { get; set; }
    IEnumerable<Pokemon> Pokemons { get; } //it's a readonly IList infact, fine with WPF.Binding
    TargetPanel TargetPanel { get; }
    int Time { get; }

    Weather Weather { get; }
    Visibility ThumbnailsVisibility { get; }
    Visibility UndoVisibility { get; }
    PokemonOutward[] PokemonsOnBoard { get; } //3个图标
    SimPokemon ControllingPokemon { get; }
    bool IsFightEnabled { get; }
    TeamOutward TeamPokemonsCount { get; }
    TeamOutward RivalTeamPokemonsCount { get; }

    void Pokemon_Click(Pokemon pokemon);
    void Move_Click(SimMove move);
    void Struggle_Click();
    void Giveup_Click();
    void Undo_Click();
  }
}
