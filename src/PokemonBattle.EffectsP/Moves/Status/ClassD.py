class AquaRing(StatusMoveE):
    def Act(self, a):
        aer = a.Attacker
        if aer.OnboardPokemon.HasCondition('AquaRing'):
            aer.Controller.ReportBuilder.Add('Fail0')
        else:
            a.Attacker.OnboardPokemon.SetCondition('AquaRing', None)
            aer.AddReportPm('EnAquaRing', None, None)
M(AquaRing(392))