class a_a(AbilityE):
    def ADSModifier(self, pm, stat):
        if stat == StatType.Atk or stat == StatType.SpAtk:
            return self.AModifier(pm.AtkContext, pm.AtkContext.Move.Category == MoveCategory.Special)
        return 0x1000

class One3rdHp(a_a):
    def __new__(cls, id, type):
        return AbilityE.__new__(cls, id)
    
    def __init__(self, id, type):
        self.Type = type
    
    def AModifier(self, atk, sp):
        if atk.Type == self.Type and atk.Attacker.Hp * 3 <= atk.Attacker.Pokemon.Hp.Origin:
            return 0x1800
        return 0x1000
A(One3rdHp(151, BattleType.Water)) #torrent
A(One3rdHp(141, BattleType.Bug)) #swarm
A(One3rdHp(89, BattleType.Grass)) #overgrow
A(One3rdHp(10, BattleType.Fire)) #blaze

class Guts(a_a):
    def AModifier(self, atk, sp):
        if atk.Attacker.State != PokemonState.Normal and not sp:
            return 0x1800
        return 0x1000
A(Guts(41))

class Defeatist(a_a):
    def AModifier(self, atk, sp):
        if atk.Attacker.Hp * 2 < atk.Attacker.Pokemon.Hp.Origin:
            return 0x800
        return 0x1000
A(Defeatist(31))

class PurePower(AbilityE):
    def ADSModifier(self, pm, stat):
        if stat == StatType.Atk:
            return 0x2000
        return 0x1000
A(PurePower(100))
A(PurePower(50)) #huge power

class MarvelScale(AbilityE):
    def ADSModifier(self, pm, stat):
        if stat == StatType.Def and pm.State != PokemonState.Normal:
            return 0x1800
        return 0x1000
A(MarvelScale(75))

class PlusMinus(AbilityE):
    def ADSModifier(self, pm, stat):
        if stat == StatType.SpAtk:
            for p in pm.Controller.Board[pm.Pokemon.TeamId].Pokemons:
                if p != pm:
                    a = p.Ability.Id
                    if a == 76 or a == 94:
                        return 0x1800
        return 0x1000
A(PlusMinus(76)) #minus
A(PlusMinus(94)) #plus

class a_s(AbilityE):
    def __new__(cls, id):
        return AbilityE.__new__(cls, id)
    def ADSModifier(self, pm, stat):
        if stat == StatType.Speed:
            return self.SModifier(pm)
        return 0x1000

class WeatherSpeedup(a_s):
    def __new__(cls, id, weather):
        return AbilityE.__new__(cls, id)
    def __init__(self, id, weather):
        self.Weather = weather
    def SModifier(self, pm):
        if pm.Controller.Weather == self.Weather:
            return 0x2000
        return 0x1000
A(WeatherSpeedup(12, Weather.IntenseSunlight)) #chlorophyll
A(WeatherSpeedup(111, Weather.Sandstorm)) #sand rush
A(WeatherSpeedup(142, Weather.HeavyRain)) #swift swim

class QuickFeet(a_s):
    def SModifier(self, pm):
        if pm.State != PokemonState.Normal:
            return 0x1800
        return 0x1000
A(QuickFeet(101))

class Unburden(a_s):
    def SModifier(self, pm):
        if pm.Pokemon.Item == None and pm.OnboardPokemon.HasCondition('Unburden'):
            return 0x2000
        return 0x1000
A(Unburden(156))