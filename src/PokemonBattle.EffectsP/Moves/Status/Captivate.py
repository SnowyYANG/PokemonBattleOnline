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
            if d.OnboardPokemon.Gender != g:
                self.Fail(d)
            else:
                targets.append(d)
        a.SetTargets(targets)
M(Captivate(445))