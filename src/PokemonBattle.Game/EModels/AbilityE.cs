﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game
{
  public interface AbilityE
  {
    void Debut();
    void Attacked();
    void Lv8DChanged();
    void StateChanged();
    void KO();
  }
}