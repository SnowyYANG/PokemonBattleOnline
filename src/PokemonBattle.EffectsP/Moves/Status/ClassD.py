class Encore(StatusMoveE):
    class Condition:
        def __init__(self, turn, move):
            self.Turn = turn
            self.Move = move
    def Act(self, a):
        der = a.Target.Defender
        if der.AtkContext != None and der.AtkContext.MoveProxy.PP.Value > 0 and der.OnboardPokemon.SetCondition('Encore', Condition(3, der.AtkContext.MoveProxy.Type)):
            der.AddReportPm('EnEncore', None, None)
        else:
            MoveE.Fail(der)
M(Encore(227))

class Taunt(StatusMoveE):
    def Act(self, a):
        der = a.Target.Defender
        if der.OnboardPokemon.SetCondition('Taunt', 3):
            der.AddReportPm('EnTaunt', None, None)
        else:
            MoveE.Fail(der)
M(Taunt(269))

class AquaRing(StatusMoveE):
    def Act(self, a):
        aer = a.Attacker
        if aer.OnboardPokemon.SetCondition('AquaRing', None):
            aer.AddReportPm('EnAquaRing', None, None)
        else:
            aer.Controller.ReportBuilder.Add('Fail0')
M(AquaRing(392))

class MagnetRise(StatusMoveE):
    def Act(self, a):
        aer = a.Attacker.OnboardPokemon
        if not aer.HasCondition('Ingrain') and aer.SetCondition('MagnetRise', a.Controller.TurnNumber + 5):
            aer.AddReportPm('EnMagnetRise', None, None)
        else:
            a.Controller.ReportBuilder.Add('Fail0')
M(MagnetRise(393))