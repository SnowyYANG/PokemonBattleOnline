class MoldBreaker(AbilityE):
    def __new__(cls, id, logKey):
        return AbilityE.__new__(cls, id)
    def __init__(self, id, logKey):
        AbilityE.__init__(self, id)
        self.log = logKey
    def Attach(self, pm):
        self.Raise(pm)
        pm.Controller.ReportBuilder.Add(self.log, pm)
A(MoldBreaker(79, 'MoldBreaker'))
A(MoldBreaker(148, 'Teravolt'))
A(MoldBreaker(154, 'Turboblaze'))

class WeatherAbility(AbilityE):
    def __new__(cls, id, weather):
        return AbilityE.__new__(cls, id)
    def __init__(self, id, weather):
        AbilityE.__init__(self, id)
        self.weather = weather
    def Attach(self, pm):
        self.Raise(pm)
        pm.Controller.Weather = weather
A(WeatherAbility(23, Weather.HeavyRain)) #drizzle
A(WeatherAbility(24, Weather.IntenseSunlight)) #drought
A(WeatherAbility(126, Weather.Hailstorm)) #snow warning
A(WeatherAbility(109, Weather.Sandstorm)) #sand stream

class Intimidate(AbilityE):
    def Attach(self, pm):
        pms = []
        for p in pm.Controller.Board[1-pm.Pokemon.TeamId].GetPokemons(pm.OnboardPokemon.X - 1, pm.OnboardPokemon.X + 1):
            if p.CanChangeLv7D(pm, False, StatType.Atk, -1):
                pms.append(p)
        if len(pms) > 0:
            self.Raise(pm)
            for p in pms:
                p.ChangeLv7D(pm, False, -1,0,0,0,0,0,0)
A(Intimidate(61))

class Unnerve(AbilityE):
    def Attach(self, pm):
        self.Raise(pm)
A(Unnerve(147))

class Download(AbilityE):
    def Attach(self, pm):
        pms = pm.Controller.Board[1-pm.Pokemon.TeamId].GetPokemons(pm.OnboardPokemon.X - 1, pm.OnboardPokemon.X + 1)
        if pms.Count() != 0:
            p = pms[pm.Controller.GetRandomInt(0, pms.Count() - 1)]
            self.Raise() #unneccesary to check CanChangeLv7D
            if p.OnboardPokemon.Static.Def > p.OnboardPokemon.Static.SpDef:
                pm.ChangeLv7D(pm, False, 0,0,1,0,0,0,0)
            else:
                pm.ChangeLv7D(pm, False, 1,0,0,0,0,0,0)
A(Download(22))

class SlowStart(AbilityE):
    def Attach(self, pm):
        pm.OnboardPokemon.SetCondition('SlowStart', pm.Controller.TurnNumber + 5)
        self.Raise(pm)
        pm.Controller.Add('EnSlowStart', pm)
A(SlowStart(123))
            
        