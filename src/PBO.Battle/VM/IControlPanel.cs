using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.PBO.Battle.VM
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
    IEnumerable<SimPokemon> Pokemons { get; } //it's a readonly IList infact, fine with WPF.Binding
    TargetPanel TargetPanel { get; }
    int Time { get; }

    Weather Weather { get; }
    Visibility ThumbnailsVisibility { get; }
    Visibility UndoVisibility { get; }
    PokemonOutward[] PokemonsOnBoard { get; } //3个图标
    SimOnboardPokemon ControllingPokemon { get; }
    bool IsFightEnabled { get; }
    TeamOutward TeamPokemonsCount { get; }
    TeamOutward RivalTeamPokemonsCount { get; }

    void Pokemon_Click(SimPokemon pokemon);
    void Fight_Click();
    void Move_Click(SimMove move);
    void Giveup_Click();
    void Undo_Click();
  }
}
