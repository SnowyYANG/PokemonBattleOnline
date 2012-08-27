class LockOn(StatusMoveE):
    def Act(self, a):
        der = a.Target.Defender
        c = Condition()
        c.By = a.Attacker
        c.Turn = a.Controller.TurnNumber + 1
        if der.OnboardPokemon.AddCondition('NoGuard', c):
            a.Attacker.AddReportPm('LockOn', der, None)
        else:
            MoveE.Fail(der)
M(LockOn(170)) #mind reader
M(LockOn(199))

class Protect(StatusMoveE):
    def NotFail(self, a):
        return a.Controller.OnboardPokemons[a.Controller.OnboardPokemons.Count - 1] != a.Attacker and self.CheckContinuousUseNotFail(a)
    def Act(self, a):
        a.Attacker.OnboardPokemon.SetTurnCondition('Protect')
M(Protect(182))

class BellyDrum(StatusMoveE):
    def NotFail(self, a):
        return aer.OnboardPokemon.Lv5D.Atk != 6 and a.Attacker.Hp > a.Attacker.Pokemon.Hp.Origin >> 1
    def Act(self, a):
        aer = a.Attacker
        aer.Hp -= aer.Pokemon.Hp.Origin >> 1
        aer.OnboardPokemon.ChangeLv7D(StatType.Atk, 12)
        aer.Controller.ReportBuilder.Add(HpChange(aer, 'BellyDrum', 0, 0))
M(BellyDrum(187))

class Encore(StatusMoveE):
    def Act(self, a):
        der = a.Target.Defender
        c = Condition()
        c.Turn = 3
        c.Move = der.AtkContext.MoveProxy.Type
        if der.AtkContext != None and der.AtkContext.MoveProxy.PP.Value > 0 and der.OnboardPokemon.AddCondition('Encore', c):
            der.AddReportPm('EnEncore', None, None)
        else:
            MoveE.Fail(der)
M(Encore(227))

class Taunt(StatusMoveE):
    def Act(self, a):
        der = a.Target.Defender
        if der.OnboardPokemon.AddCondition('Taunt', 3):
            der.AddReportPm('EnTaunt', None, None)
        else:
            MoveE.Fail(der)
M(Taunt(269))

class AquaRing(StatusMoveE):
    def Act(self, a):
        aer = a.Attacker
        if aer.OnboardPokemon.AddCondition('AquaRing', None):
            aer.AddReportPm('EnAquaRing', None, None)
        else:
            FailAll(a)
M(AquaRing(392))

class MagnetRise(StatusMoveE):
    def Act(self, a):
        aer = a.Attacker.OnboardPokemon
        if not aer.HasCondition('Ingrain') and aer.AddCondition('MagnetRise', a.Controller.TurnNumber + 5):
            aer.AddReportPm('EnMagnetRise', None, None)
        else:
            FailAll(a)
M(MagnetRise(393))