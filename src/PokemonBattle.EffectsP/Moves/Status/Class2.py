class Growth(StatusMoveE):
    def Act(self, a):
        aer = a.Attacker
        if a.Controller.Weather == Weather.IntenseSunlight:
            c = 2
        else:
            c = 1
        aer.ChangeLv7D(aer, StatType.Atk, c, True)
        aer.ChangeLv7D(aer, StatType.SpAtk, c, True)
M(Growth(74))

class DefenseCurl(StatusMoveE):
    def Act(self, a):
        StatusMoveE.Act(self, a)
        a.Attacker.OnboardPokemon.SetCondition('DefenseCurl')
M(DefenseCurl(111))

class Captivate(StatusMoveE): 
    def HasEffect(self, d):
        dg = d.Defender.OnboardPokemon.Gender
        ag = d.AtkContext.Attacker.OnboardPokemon.Gender
        return dg == PokemonGender.Male and ag == PokemonGender.Female or dg == PokemonGender.Female and ag == PokemonGender.Male
M(Captivate(445))