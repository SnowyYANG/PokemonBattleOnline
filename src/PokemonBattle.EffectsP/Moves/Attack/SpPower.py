class RollOut(AttackMoveE):
    def CalculateBasePower(self, d):
        i = 5 - d.AtkContext.Attachment
        if d.AtkContext.Attacker.OnboardPokemon.HasCondition('DefenseCurl'):
            i += 1
        d.BasePower = 30 * (1 << i)
M(RollOut(205))
M(RollOut(301))

class Present(AttackMoveE):
    def Execute(self, pm):
        random = pm.Controller.GetRandomInt(0, 99)
        pm.BuildAtkContext(self.Move)
        if random < 20:
            a = 0
        else:
            if random <  60:
                a = 40
            else:
                if random < 90:
                    a = 80
                else:
                    a = 100
        pm.AtkContext.Attachment = a
        MoveE.Execute(self, pm)
    def Act(self, a):
        if a.Attachment == 0:
            a.Target.Defender.HpRecoverByOneNth(4, True)
    def CalculateBasePower(self, d):
        d.BasePower = d.AtkContext.Attachment
M(Present(217))

class Magnitude(AttackMoveE):
    def Execute(self, pm):
        random = pm.Controller.GetRandomInt(0, 99)
        pm.BuildAtkContext(self.Move)
        if random < 5:
            a = 0
        else:
            if random < 16:
                a = 1
            else:
                if random < 35:
                    a = 2
                else:
                    if random < 65:
                        a = 3
                    else:
                        if random < 85:
                            a = 4
                        else:
                            if random < 95:
                                a = 5
                            else:
                                a = 6
        pm.AtkContext.Attachment = a
        pm.Controller.ReportBuilder.Add("Magnitude", 4 + a)
        MoveE.Execute(self, pm)
    def CalculateBasePower(self, d):
        d.BasePower = 10 + 20 * d.AtkContext.Attachment
    def DamageFinalModifier(self, d):
        if d.Defender.OnboardPokemon.CoordY == CoordY.Underground:
            return 0x2000
        return 0x1000
M(Magnitude(222))

class WeatherBall(AttackMoveE):
    def CalculateBasePower(self, d):
        if d.Defender.Controller.Weather == Weather.Normal:
            d.BasePower = 50
        else:
            d.BasePower = 100
M(WeatherBall(311))

class Facade(AttackMoveE):
    def PowerModifier(self, d):
        s = d.AtkContext.Attacker.State
        if s != PokemonState.Normal and s != PokemonState.Frozen:
            return 0x2000
        return 0x1000
M(Facade(263))

class Brine(AttackMoveE):
    def PowerModifier(self, d):
        if d.Defender.Hp << 1 <= d.Defender.Pokemon.Hp.Origin:
            return 0x2000
        return 0x1000
M(Brine(362))

class Payback(AttackMoveE):
    def CalculateBasePower(self, d):
        if d.Defender.LastActTurn == d.Defender.Controller.TurnNumber:
            d.BasePower = 100
        else:
            d.BasePower = 50
M(Payback(371))

class Venoshock(AttackMoveE):
    def PowerModifier(self, d):
        if d.Defender.State == PokemonState.Poisoned or d.Defender.State == PokemonState.BadlyPoisoned:
            return 0x2000
        return 0x1000
M(Venoshock(474))

class Retaliate(AttackMoveE):
    def PowerModifier(self, d):
        c = d.Defender.Controller
        if c.Board[d.Defender.Pokemon.TeamId].GetCondition[int]("FaintTurn") == c.TurnNumber - 1:
            return 0x2000
        return 0x1000
M(Retaliate(514))