class WonderSkin(AbilityE):
    def AccuracyModifier(self, d):
        if d.AtkContext.Move.Category == MoveCategory.Status:
            return 0x999
        return 0x1000
A(WonderSkin(147))

class SandVeil(AbilityE):
    def AccuracyModifier(self, d):
        if d.AtkContext.Controller.Weather == Weather.Sandstorm:
            return 0xccc
        return 0x1000
A(SandVeil(8))

class SnowCloak(AbilityE):
    def AccuracyModifier(self, d):
        if d.AtkContext.Controller.Weather == Weather.Hailstorm:
            return 0xccc
        return 0x1000
A(SnowCloak(81))

class TangledFeet(AbilityE):
    def AccuracyModifier(self, d):
        if d.Defender.OnboardPokemon.HasCondition('Confuse'):
            return 0xccc
        return 0x1000
A(TangledFeet(77))
