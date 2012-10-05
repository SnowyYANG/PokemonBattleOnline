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
A(One3rdHp(67, BattleType.Water)) #torrent
A(One3rdHp(68, BattleType.Bug)) #swarm
A(One3rdHp(65, BattleType.Grass)) #overgrow
A(One3rdHp(66, BattleType.Fire)) #blaze

class Guts(a_a):
    def AModifier(self, atk, sp):
        if atk.Attacker.State != PokemonState.Normal and not sp:
            return 0x1800
        return 0x1000
A(Guts(62))

class Defeatist(a_a):
    def AModifier(self, atk, sp):
        if atk.Attacker.Hp * 2 < atk.Attacker.Pokemon.Hp.Origin:
            return 0x800
        return 0x1000
A(Defeatist(129))

class PurePower(AbilityE):
    def ADSModifier(self, pm, stat):
        if stat == StatType.Atk:
            return 0x2000
        return 0x1000
A(PurePower(74))
A(PurePower(37)) #huge power

class MarvelScale(AbilityE):
    def ADSModifier(self, pm, stat):
        if stat == StatType.Def and pm.State != PokemonState.Normal:
            return 0x1800
        return 0x1000
A(MarvelScale(63))

class PlusMinus(AbilityE):
    def ADSModifier(self, pm, stat):
        if stat == StatType.SpAtk:
            for p in pm.Controller.Board[pm.Pokemon.TeamId].Pokemons:
                if p != pm:
                    a = p.Ability.Id
                    if a == 57 or a == 58:
                        return 0x1800
        return 0x1000
A(PlusMinus(58)) #minus
A(PlusMinus(57)) #plus

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
A(WeatherSpeedup(34, Weather.IntenseSunlight)) #chlorophyll
A(WeatherSpeedup(146, Weather.Sandstorm)) #sand rush
A(WeatherSpeedup(33, Weather.HeavyRain)) #swift swim

class QuickFeet(a_s):
    def SModifier(self, pm):
        if pm.State != PokemonState.Normal:
            return 0x1800
        return 0x1000
A(QuickFeet(95))

class Unburden(a_s):
    def SModifier(self, pm):
        if pm.Pokemon.Item == None and pm.OnboardPokemon.HasCondition('HadItem'):
            return 0x2000
        return 0x1000
A(Unburden(84))

class SolarPower(AbilityE):
    def ADSModifier(self, pm, stat):
        if stat == StatType.SpAtk and pm.Controller.Weather == Weather.IntenseSunlight:
            return 0x1800
        return 0x1000
A(SolarPower(94))