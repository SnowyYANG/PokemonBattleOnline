class Pursuit(AttackMoveE):
    def BuildAtkContext(self, pm):
        atk = MoveE.BuildAtkContext(self, pm)
        if pm.OnboardPokemon.HasCondition('Pursuiting'):
            pm.OnboardPokemon.RemoveCondition('Pursuiting')
            atk.IgnorePostEffectItem = True
        return atk
    def CalculateBasePower(self, d):
        if d.AtkContext.IgnorePostEffectItem:
            d.BasePower = 80
        else:
            d.BasePower = 40
M(Pursuit(228))

class Facade(AttackMoveE):
    def PowerModifier(self, d):
        s = d.AtkContext.Attacker.State
        if s != PokemonState.Normal and s != PokemonState.FRZ:
            return 0x2000
        return 0x1000
M(Facade(263))

class WeatherBall(AttackMoveE):
    def CalculateBasePower(self, d):
        if d.Defender.Controller.Weather == Weather.Normal:
            d.BasePower = 50
        else:
            d.BasePower = 100
M(WeatherBall(311))

class Brine(AttackMoveE):
    def PowerModifier(self, d):
        if d.Defender.Hp << 1 <= d.Defender.Pokemon.Hp.Origin:
            return 0x2000
        return 0x1000
M(Brine(362))

class Payback(AttackMoveE):
    def CalculateBasePower(self, d):
        if d.Defender.LastMoveTurn == d.Defender.Controller.TurnNumber:
            d.BasePower = 100
        else:
            d.BasePower = 50
M(Payback(371))

class Assurance(AttackMoveE):
    def CalculateBasePower(self, d):
        if d.Defender.OnboardPokemon.HasCondition('Assurance'):
            d.BasePower = 100
        else:
            d.BasePower = 50
M(Assurance(372))

class Venoshock(AttackMoveE):
    def PowerModifier(self, d):
        if d.Defender.State == PokemonState.PSN or d.Defender.State == PokemonState.BadlyPSN:
            return 0x2000
        return 0x1000
M(Venoshock(474))

class Retaliate(AttackMoveE):
    def PowerModifier(self, d):
        c = d.Defender.Controller
        if d.AtkContext.Attacker.Tile.Field.GetCondition[int]("FaintTurn") == c.TurnNumber - 1:
            return 0x2000
        return 0x1000
M(Retaliate(514))

