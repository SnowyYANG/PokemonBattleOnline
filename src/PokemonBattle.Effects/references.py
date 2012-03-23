import clr
clr.AddReferenceToFile('Tactic.dll')
clr.AddReferenceToFile('PokemonBattle.Data.dll')
clr.AddReferenceToFile('PokemonBattle.Game.dll')

from LightStudio.PokemonBattle.Data import *
from LightStudio.PokemonBattle.Game import *
from LightStudio.PokemonBattle.Game.Interactive import *
from LightStudio.PokemonBattle.Game.Interactive.GameEvents import *