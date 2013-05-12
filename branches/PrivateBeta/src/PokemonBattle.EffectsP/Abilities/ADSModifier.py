class PurePower(AbilityE):
    def AModifier(self, atk):
        if atk.Move.Category == MoveCategory.Physical:
            return 0x2000
        return 0x1000
A(PurePower(37)) #huge power
A(PurePower(74))

class PlusMinus(AbilityE):
    def AModifier(self, atk):
        if atk.Move.Category == MoveCategory.Special:
            for p in atk.Attacker.Tile.Field.Pokemons:
                if p != atk.Attacker:
                    a = p.Ability.Id
                    if a == 57 or a == 58:
                        return 0x1800
        return 0x1000
A(PlusMinus(57)) #plus
A(PlusMinus(58)) #minus

class Guts(AbilityE):
    def AModifier(self, atk):
        if atk.Attacker.State != PokemonState.Normal and atk.Move.Category == MoveCategory.Physical:
            return 0x1800
        return 0x1000
A(Guts(62))

class One3rdHp(AbilityE):
    def __new__(cls, id, type):
        return AbilityE.__new__(cls, id)
    def __init__(self, id, type):
        self.Type = type
    def AModifier(self, atk):
        if atk.Type == self.Type and atk.Attacker.Hp * 3 <= atk.Attacker.Pokemon.Hp.Origin:
            return 0x1800
        return 0x1000
A(One3rdHp(65, BattleType.Grass)) #overgrow
A(One3rdHp(66, BattleType.Fire)) #blaze
A(One3rdHp(67, BattleType.Water)) #torrent
A(One3rdHp(68, BattleType.Bug)) #swarm

class SolarPower(AbilityE):
    def AModifier(self, atk):
        if atk.Controller.Weather == Weather.IntenseSunlight and atk.Move.Category == MoveCategory.Special:
            return 0x1800
        return 0x1000
A(SolarPower(94))

class Defeatist(AbilityE):
    def AModifier(self, atk):
        if atk.Attacker.Hp * 2 < atk.Attacker.Pokemon.Hp.Origin:
            return 0x800
        return 0x1000
A(Defeatist(129))

class WeatherSpeedup(AbilityE):
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

class Unburden(AbilityE):
    def SModifier(self, pm):
        if pm.Pokemon.Item == None and pm.OnboardPokemon.HasCondition('HadItem'):
            return 0x2000
        return 0x1000
A(Unburden(84))

class QuickFeet(AbilityE):
    def SModifier(self, pm):
        if pm.State != PokemonState.Normal:
            return 0x1800
        return 0x1000
A(QuickFeet(95))

