class Technician(AbilityE):
    def PowerModifier(self, target):
        if target.BasePower <= 60:
            return 0x1800
        return 0x1000
A(Technician(145))

class FlareBoost(AbilityE):
    def PowerModifier(self, target):
        if target.AtkContext.Move.Category == MoveCategory.Special and target.AtkContext.Attacker.State == PokemonState.BRN:
            return 0x1800
        return 0x1000
A(FlareBoost(45))

class Analytic(AbilityE):
    def PowerModifier(self, target):
        turn = target.AtkContext.Attacker.LastActTurn
        for pm in target.AtkContext.Controller.OnboardPokemons:
            if pm.LastActTurn != turn:
                return 0x1000
        return 0x14cd
A(Analytic(4))

class Reckless(AbilityE):
    def PowerModifier(self, target):
        move = target.AtkContext.Move
        if move.HurtPercentage < 0 or move.Id == 26 or move.Id == 136:
            return 0x1333
        return 0x1000
A(Reckless(103))

class IronFist(AbilityE):
    def PowerModifier(self, target):
        if target.AtkContext.Move.AdvancedFlags.IsFist:
            return 0x1333
        return 0x1000
A(IronFist(62))

class ToxicBoost(AbilityE):
    def PowerModifier(self, target):
        state = target.AtkContext.Attacker.State
        if (state == PokemonState.PSN or state == PokemonState.BadlyPSN) and target.AtkContext.Move.Category == MoveCategory.Physical:
            return 0x1800
        return 0x1000
A(ToxicBoost(98))

class Rivalry(AbilityE):
    def PowerModifier(self, target):
        u = target.AtkContext.Attacker.OnboardPokemon.Gender
        t = target.Defender.OnboardPokemon.Gender
        if u == PokemonGender.None or t == PokemonGender.None:
            return 0x1000
        if u == t:
            return 0xc00
        return 0x1400
A(Rivalry(105))

class SandForce(AbilityE):
    def PowerModifier(self, target):
        type = target.AtkContext.Move.Type
        if (type == BattleType.Rock or type == BattleType.Ground or type == BattleType.Steel) and target.AtkContext.Controller.Weather == Weather.Sandstorm:
            return 0x14cd
        return 0x1000
A(SandForce(110))