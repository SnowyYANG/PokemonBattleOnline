class WeatherAbility(AbilityE):
    def __new__(cls, id, weather):
        return AbilityE.__new__(cls, id)
    def __init__(self, id, weather):
        AbilityE.__init__(self, id)
        self.Weather = weather
    def Attach(self, pm):
        if pm.Controller.Weather != self.Weather:
            self.Raise(pm)
            pm.Controller.Weather = self.Weather
A(WeatherAbility(2, Weather.HeavyRain)) #drizzle
A(WeatherAbility(70, Weather.IntenseSunlight)) #drought
A(WeatherAbility(45, Weather.Sandstorm)) #sand stream
A(WeatherAbility(117, Weather.Hailstorm)) #snow warning

class Intimidate(AbilityE):
    def Attach(self, pm):
        self.Raise(pm)
        for p in pm.Controller.Board[1 - pm.Pokemon.TeamId].GetPokemons(pm.OnboardPokemon.X - 1, pm.OnboardPokemon.X + 1):
            p.ChangeLv7D(pm, StatType.Atk, -1, True, None)
A(Intimidate(22))

class Trace(AbilityE):
    def Attach(self, pm):
        pms = []
        for p in pm.Controller.Board[1 - pm.Pokemon.TeamId].GetPokemons(pm.OnboardPokemon.X - 1, pm.OnboardPokemon.X + 1):
            if Abilities.Trace(p.OnboardPokemon.Ability):
                pms.append(p)
        n = len(pms)
        if n > 0:
            self.Raise(pm)
            target = pms[pm.Controller.GetRandomInt(0, n - 1)]
            pm.ChangeAbility(target.OnboardPokemon.Ability, 'Trace', target.Id)
A(Trace(36))

class SimpleAttachRaise(AbilityE):
    def __new__(cls, id, logKey):
        return AbilityE.__new__(cls, id)
    def __init__(self, id, logKey):
        AbilityE.__init__(self, id)
        self.log = logKey
    def Attach(self, pm):
        self.Raise(pm)
        pm.AddReportPm(self.log, None, None)
A(SimpleAttachRaise(46, 'Pressure'))
A(SimpleAttachRaise(104, 'MoldBreaker'))
A(SimpleAttachRaise(163, 'Turboblaze'))
A(SimpleAttachRaise(164, 'Teravolt'))

class Download(AbilityE):
    def Attach(self, pm): #a ability can be copy, so check CanChangeLv7D
        sa = pm.CanChangeLv7D(pm, StatType.SpAtk, 1, False) != 0
        a = pm.CanChangeLv7D(pm, StatType.Atk, 1, False) != 0
        stats = []
        for p in pm.Controller.Board[1 - pm.Pokemon.TeamId].GetPokemons(pm.OnboardPokemon.X - 1, pm.OnboardPokemon.X + 1):
            if p.OnboardPokemon.FiveD.Def > p.OnboardPokemon.FiveD.SpDef:
                if sa:
                    stats.append(StatType.SpAtk)
            else:
                if a:
                    stats.append(StatType.Atk)  
        n = len(stats)
        if n != 0:
            self.Raise(pm)
            pm.ChangeLv7D(pm, stats[pm.Controller.GetRandomInt(0, n - 1)], 1, False, None)
A(Download(88))

class Anticipation(AbilityE):
    def Attach(self, pm):
        for p in pm.Controller.Board[1 - pm.Pokemon.TeamId].GetPokemons(pm.OnboardPokemon.X - 1, pm.OnboardPokemon.X + 1):
            for m in p.Moves:
                if m.Type.Class == MoveInnerClass.OHKO or BattleTypeHelper.EffectRevise(m.Type.Type, pm.OnboardPokemon.Type1, pm.OnboardPokemon.Type2) > 0:
                    self.Raise(pm)
                    pm.AddReportPm('Anticipation', None, None)
                    return
A(Anticipation(107))

class SlowStart(AbilityE):
    def Attach(self, pm):
        pm.OnboardPokemon.SetCondition('SlowStart', pm.Controller.TurnNumber + 5)
        self.Raise(pm)
        pm.AddReportPm('EnSlowStart', None, None)
A(SlowStart(112))

