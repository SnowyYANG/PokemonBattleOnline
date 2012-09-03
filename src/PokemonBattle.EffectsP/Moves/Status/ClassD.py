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
        return a.Attacker.OnboardPokemon.Lv5D.Atk != 6 and a.Attacker.Hp > a.Attacker.Pokemon.Hp.Origin >> 1
    def Act(self, a):
        aer = a.Attacker
        aer.Pokemon.SetHp(aer.Hp - (aer.Pokemon.Hp.Origin >> 1))
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

class PowerTrick(StatusMoveE):
    def Act(self, a):
        s = a.Attacker.OnboardPokemon.Static
        atk = s.Atk
        s.Atk = s.Def
        s.Def = atk
        a.Attacker.AddReportPm('PowerTrick')
M(PowerTrick(379))

class GastroAcid(StatusMoveE):
    def Act(self, a):
        der = a.Target.Defender.OnboardPokemon
        if der.HasCondition('GastroAcid') or der.Ability == 82: #multitype
            self.FailAll()
        else:
            a.Target.Defender.AddReportPm('EnGastroAcid')
            der.Ability.Detach(der)
            der.SetCondition('GastroAcid')
M(GastroAcid(380))
        
class PowerSwap(StatusMoveE):
    def Act(self, a):
        aer = a.Attacker.OnboardPokemon
        der = a.Target.Defender.OnboardPokemon
        aa = aer.Lv5D.Atk
        asa = aer.Lv5D.SpAtk
        aer.SetLv7D(der.Lv5D.Atk, None, der.Lv5D.SpAtk, None, None, None, None)
        der.SetLv7D(aa, None, asa, None, None, None, None)
        a.Controller.ReportBuilder.Add('PowerSwap', a.Attacker, a.Target.Defender)
M(PowerSwap(384))

class GuardSwap(StatusMoveE):
    def Act(self, a):
        aer = a.Attacker.OnboardPokemon
        der = a.Target.Defender.OnboardPokemon
        ad = aer.Lv5D.Def
        asd = aer.Lv5D.SpDef
        aer.SetLv7D(None, der.Lv5D.Def, None, der.Lv5D.SpDef, None, None, None)
        der.SetLv7D(None, ad, None, asd, None, None, None)
        a.Controller.ReportBuilder.Add('GuardSwap', a.Attacker, a.Target.Defender)
M(GuardSwap(385))

class HeartSwap(StatusMoveE):
    def Act(self, a):
        aer = a.Attacker.OnboardPokemon
        der = a.Target.Defender.OnboardPokemon
        aa = aer.Lv5D.Atk
        ad = aer.Lv5D.Def
        asa = aer.Lv5D.SpAtk
        asd = aer.Lv5D.SpDef
        aspeed = aer.Lv5D.Speed
        aacc = aer.AccuracyLv
        aeva = aer.EvasionLv
        aer.SetLv7D(der.Lv5D.Atk, der.Lv5D.Def, der.Lv5D.SpAtk, der.Lv5D.SpDef, der.Lv5D.Speed, der.AccuracyLv, der.EvasionLv)
        der.SetLv7D(aa, ad, asa, asd, aspeed, aacc, aeva)
        a.Controller.ReportBuilder.Add('HeartSwap', a.Attacker, a.Target.Defender)
M(HeartSwap(391))

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

class Defog(StatusMoveE):
    def Act(self, a):
        r = a.Controller.ReportBuilder
        a.Target.Defender.ChangeLv7D(a.Attacker, StatType.Evasion, -1, True)
        t = a.Target.Defender.Pokemon.TeamId
        f = a.Controller.Board[t]
        f.DeEntryHazards(r, t)
        if f.HasCondition('Reflect'):
            f.RemoveCondition('Reflect')
            r.Add('DeReflect', t)
        if f.HasCondition('LightScreen'):
            f.RemoveCondition('LightScreen')
            r.Add('DeLightScreen', t)
        if f.HasCondition('Mist'):
            f.RemoveCondition('Mist')
            r.Add('DeMist', t)
        if f.HasCondition('Safeguard'):
            f.RemoveCondition('Safeguard')
            r.Add('DeSafeguard', t)
M(DeFog(432))

class GuardSplit(StatusMoveE):
    def Act(self, a):
        aer = a.Attacker.OnboardPokemon
        der = a.Target.Defender.OnboardPokemon
        aer.Static.Def = der.Static.Def = (aer.Static.Def + der.Static.Def) >> 1
        aer.Static.SpDef = der.Static.SpDef = (aer.Static.SpDef + der.Static.SpDef) >> 1
        a.Controller.ReportBuilder.Add('GuardSplit', a.Attacker, a.Target.Defender)
M(GuardSplit(470))

class PowerSplit(StatusMoveE):
    def Act(self, a):
        aer = a.Attacker.OnboardPokemon
        der = a.Target.Defender.OnboardPokemon
        aer.Static.Atk = der.Static.Atk = (aer.Static.Atk + der.Static.Atk) >> 1
        aer.Static.SpAtk = der.Static.SpAtk = (aer.Static.SpAtk + der.Static.SpAtk) >> 1
        a.Controller.ReportBuilder.Add('PowerSplit', a.Attacker, a.Target.Defender)
M(PowerSplit(471))

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

class ReflectType(StatusMoveE):
    def Act(self, a):
        aer = a.Attacker.OnboardPokemon
        t1 = a.Target.Defender.OnboardPokemon.Type1
        t2 = a.Target.Defender.OnboardPokemon.Type2
        if aer.Type1 == t1 and aer.Type2 == t2:
            self.FailAll(a)
        else:
            aer.Type1 = t1
            aer.Type2 = t2
            aer.AddReportPm('ReflectType', a.Target.Defender)
M(ReflectType(513))

class Bestow(StatusMoveE):
    def NotFail(self, a):
        return a.Attacker.Pokemon.Item != None and a.Attacker.CanLostItem
    def Act(self, a):
        if a.Target.Defender.Pokemon.Item == None:
            self.FailAll(a)
        else:
            i = a.Attacker.Pokemon.Item.Id
            a.Attacker.Pokemon.Item = None
            a.Target.Defender.ChangeItem(i, 'Bestow', a.Attacker)
M(Bestow(516))