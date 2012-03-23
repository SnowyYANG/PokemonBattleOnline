using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game
{
  public interface IItemE
  {
    int Id { get; }
    
    void Raise();
    void HpChanged();
    void ImplementMove(AtkContext atk);
  }
}
