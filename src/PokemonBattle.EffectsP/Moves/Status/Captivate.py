class Captivate(StatusMoveE):          
    def CalculateTargets(self, a):
        g = a.Attacker.OnboardPokemon.Gender
        if g == PokemonGender.None:
            pm.Controller.ReportBuilder.Add('Fail0')
        else:
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