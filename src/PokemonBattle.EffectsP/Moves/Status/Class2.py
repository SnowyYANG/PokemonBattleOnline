class Growth(StatusMoveE):
    def Act(self, a):
        aer = a.AtkContext.Attacker
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
    def NotFail(self, a):
        return a.Attacker.OnboardPokemon.Gender != PokemonGender.None
    def CalculateTargets(self, a):
        g = a.Attacker.OnboardPokemon.Gender
        if g == PokemonGender.Male:
            g = PokemonGender.Female
        else:
            g = PokemonGender.Male
        MoveE.CalculateTargets(self, a)
        targets = []
        for d in a.Targets:
            if d.Defender.OnboardPokemon.Gender != g:
                self.Fail(d)
            else:
                if Abilities.RaiseAbility(d.Defender, 12):
                    d.Defender.AddReportPm('NoEffect')
                else:
                    targets.append(d)
        a.SetTargets(targets)
M(Captivate(445))