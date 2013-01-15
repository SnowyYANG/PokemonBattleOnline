import clr
clr.AddReferenceToFile('Tactic.dll')
clr.AddReferenceToFile('PokemonBattle.Data.dll')
clr.AddReferenceToFile('PokemonBattle.Game.dll')

from System import Array
from PokemonBattleOnline.Data import *
from PokemonBattleOnline.Game import *
from PokemonBattleOnline.Game.GameEvents import *
from PokemonBattleOnline.Game.Host import *
from PokemonBattleOnline.Game.Host.Sp import *

def M(e):
    EffectsService.Register.Overloads[MoveE](e)
def A(e):
    EffectsService.Register.Overloads[AbilityE](e)
def I(e):
    EffectsService.Register.Overloads[ItemE](e)