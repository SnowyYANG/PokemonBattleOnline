import clr
clr.AddReferenceToFile('Tactic.dll')
clr.AddReferenceToFile('PokemonBattle.Data.dll')
clr.AddReferenceToFile('PokemonBattle.Game.dll')

from LightStudio.PokemonBattle.Data import *
from LightStudio.PokemonBattle.Game import *
from LightStudio.PokemonBattle.Game.GameEvents import *
from LightStudio.PokemonBattle.Game.Host import *
from LightStudio.PokemonBattle.Game.Host.Sp import *

def M(e):
    EffectsService.Register.Overloads[IMoveE](e)
def A(e):
    EffectsService.Register.Overloads[IAbilityE](e)
def I(e):
    EffectsService.Register.Overloads[IItemE](e)