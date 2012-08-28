class FocusEnergy(StatusMoveE):
    def Act(self, a):
        if a.Attacker.OnboardPokemon.AddCondition('FocusEnergy'):
            a.Attacker.AddReportPm('EnFocusEnergy')
        else:
            self.FailAll(a)
M(FocusEnergy(116))

class LockOn(StatusMoveE):
    def Act(self, a):
        der = a.Target.Defender
        c = Condition()
        c.By = a.Attacker
        c.Turn = a.Controller.TurnNumber + 1
        if der.OnboardPokemon.AddCondition('NoGuard', c):
            a.Attacker.AddReportPm('LockOn', der, None)
        else:
            self.Fail(der)
M(LockOn(170)) #mind reader
M(LockOn(199))

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
            self.Fail(der)
M(Encore(227))

class Taunt(StatusMoveE):
    def Act(self, a):
        der = a.Target.Defender
        if der.OnboardPokemon.AddCondition('Taunt', 3):
            der.AddReportPm('EnTaunt', None, None)
        else:
            self.Fail(der)
M(Taunt(269))

class AquaRing(StatusMoveE):
    def Act(self, a):
        aer = a.Attacker
        if aer.OnboardPokemon.AddCondition('AquaRing', None):
            aer.AddReportPm('EnAquaRing', None, None)
        else:
            self.FailAll(a)
M(AquaRing(392))

class MagnetRise(StatusMoveE):
    def Act(self, a):
        aer = a.Attacker.OnboardPokemon
        if not aer.HasCondition('Ingrain') and aer.AddCondition('MagnetRise', a.Controller.TurnNumber + 5):
            aer.AddReportPm('EnMagnetRise', None, None)
        else:
            self.FailAll(a)
M(MagnetRise(393))

class Soak(StatusMoveE):
    def CalculateTargets(self, a):
        StatusMoveE.CalculateTargets(self, a)
        if a.Target != None:
            der = a.Target.Defender
            if Items.PlatedArceus(der.Pokemon) or (der.OnboardPokemon.Type1 == BattleType.Water and der.OnboardPokemon.Type2 == BattleType.Invalid):
                a.SetTargets(Array.CreateInstance(DefContext, 0))
    def Act(self, a):
        a.Target.Defender.OnboardPokemon.Type1 = BattleType.Water
        a.Target.Defender.OnboardPokemon.Type2 = BattleType.Invalid
        a.Target.Defender.AddReportPm('TypeChange', BattleType.Water)
M(Soak(487))

class ShellSmash(StatusMoveE):
    def Act(self, a):
        a.Attacker.ChangeLv7D(a.Attacker, True, 2, 2, -1, -1, 2)
M(ShellSmash(504))