import clr
clr.AddReferenceToFile('Tactic.dll')
clr.AddReferenceToFile('PokemonBattle.Data.dll')
clr.AddReferenceToFile('PokemonBattle.Game.dll')

from LightStudio.PokemonBattle.Data import *
from LightStudio.PokemonBattle.Game import *
from LightStudio.PokemonBattle.Interactive import *
from LightStudio.PokemonBattle.Interactive.GameEvents import *

def M(e):
    GameService.Register.Overloads[IMoveE](e)
def A(e):
    GameService.Register.Overloads[IAbilityE](e)
def I(e):
    if e == None:
        print 'e'
    GameService.Register.Overloads[IItemE](e)