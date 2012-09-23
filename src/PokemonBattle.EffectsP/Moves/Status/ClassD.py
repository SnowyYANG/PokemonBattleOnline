class PmAddState(StatusMoveE):
    def Act(self, a):
        if a.Attacker.OnboardPokemon.AddCondition(self.Condition):
            a.Attacker.AddReportPm('En' + self.Condition)
        else:
            self.FailAll(a)
M(PmAddState(116, 'FocusEnergy'))
M(PmAddState(286, 'Imprison'))

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

class KOedCondition(StatusMoveE):
    def __new__(cls, id, condition):
        return StatusMoveE.__new__(cls, id)
    def __init__(self, id, condition):
        self.Condition = condition
    def Act(self, a):
        a.Attacker.Tile.SetCondition(self.Condition)
        a.Attacker.AddReportPm('En' + self.Condition)
M(KOedCondition(194, 'DestinyBond'))
M(KOedCondition(288, 'Grudge'))

class PsychUp(StatusMoveE):
    def Act(self, a):
        der = a.Target.Defender.OnboardPokemon
        lv5d = der.Lv5D
        a.Attacker.OnboardPokemon.SetLv7D(lv5d.Atk, lv5d.SpAtk, lv5d.Def, lv5d.SpDef, lv5d.Speed, der.AccuracyLv, der.EvasionLv)
        a.Attacker.AddReportPm('PsychUp', a.Target.Defender)
M(PsychUp(244))

class Memento(StatusMoveE):
    def Act(self, a):
        a.Attacker.Pokemon.SetHp(0)
        a.Attacker.CheckFaint()
        a.Target.Defender.ChangeLv7D(a.Attacker, True, -2, 0, -2, 0, 0, 0, 0)
M(Memento(262))

class Charge(StatusMoveE):
    def Act(self, a):
        StatusMoveE.Act(self, a)
        a.Attacker.OnboardPokemon.SetCondition('Charge', a.Controller.TurnNumber + 1)
        a.Attacker.AddReportPm('Charge')
M(Charge(268))

class Taunt(StatusMoveE):
    def Act(self, a):
        der = a.Target.Defender
        if der.OnboardPokemon.AddCondition('Taunt', 3):
            der.AddReportPm('EnTaunt', None, None)
        else:
            self.Fail(der)
M(Taunt(269))

class Wish(StatusMoveE):
    def Act(self, a):
        c = Condition()
        c.Turn = a.Controller.TurnNumber + 1
        c.Int = a.Attacker.Pokemon.Hp.Origin >> 1
        if not a.Attacker.Tile.AddCondition('Wish', c):
            self.FailAll(a)
M(Wish(273))

class MagicCoat(StatusMoveE):
    def Act(self, a):
        a.Attacker.OnboardPokemon.SetTurnCondition('MagicCoat')
        a.Attacker.AddReportPm('EnMagicCoat')
M(MagicCoat(277))

class Refresh(StatusMoveE):
    def NotFail(self, a):
        return a.Attacker.State != PokemonState.Normal
    def Act(self, a):
        a.Attacker.DeAbnormalState(False)
M(Refresh(287))

class Camouflage(StatusMoveE):
    def NotFail(self, a):
        return not (a.Attacker.OnboardPokemon.Type1 == TerrainExtension.GetBattleType(a.Controller.GameSettings.Terrain) and a.Attacker.OnboardPokemon.Type2 == BattleType.Invalid)
    def Act(self, a):
        t = TerrainExtension.GetBattleType(a.Controller.GameSettings.Terrain)
        a.Attacker.OnboardPokemon.Type1 = t
        a.Attacker.OnboardPokemon.Type2 = BattleType.Invalid
        a.Attacker.AddReportPm('TypeChange', t)
M(Camouflage(293))

class HealingWish(StatusMoveE):
    def __new__(cls, id, condition):
        return StatusMoveE.__new__(cls, id)
    def __init__(self, id, condition):
        self.Condition = condition
    def NotFail(self, a):
        return a.Attacker.Pokemon.Owner.PmsAlive > GameModeExtensions.OnboardPokemonsPerPlayer(a.Controller.GameSettings.Mode)
    def Act(self, a):
        a.Attacker.Tile.SetTurnCondition(self.Condition)
        a.Attacker.Pokemon.SetHp(0)
        a.Attacker.CheckFaint()
M(HealingWish(361, 'HealingWish'))
M(HealingWish(461, 'LunarDance')) #lunar dance

class PsychoShift(StatusMoveE):
    def NotFail(self, a):
        return a.Attacker.State != PokemonState.Normal
    def Act(self, a):
        s = a.Attacker.State
        if s == PokemonState.BadlyPoisoned:
            a.Target.Defender.AddState(a.Attacker, AttachedState.Poison, True, 15)
        else:
            if s == PokemonState.Burned:
                s = AttachedState.Burn
            else:
                if s == PokemonState.Frozen:
                    s = AttachedState.Freeze
                else:
                    if s == PokemonState.Paralyzed:
                        s = AttachedState.Paralysis
                    else:
                        if s == PokemonState.Sleeping:
                            s = AttachedState.Sleep
                        else:
                            a = AttachedState.Poison
            a.Target.Defender.AddState(a.Attacker, s, True)
M(PsychoShift(375))

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
        der = a.Target.Defender
        ab = der.Ability
        if ab.Id != 82 and der.OnboardPokemon.AddCondition('GastroAcid'): #multitype
            der.AddReportPm('EnGastroAcid')
            ab.Detach(der)
        else:
            self.FailAll(a)
M(GastroAcid(380))

class StatSwap(StatusMoveE):
    def __new__(cls, id, log, stats):
        return StatusMoveE.__init__(cls, id)
    def __init__(self, id, log, stats):
        self.Log = log
        self.Stats = stats
    def Act(self, a):
        aer = a.Attacker.OnboardPokemon
        der = a.Target.Defender.OnboardPokemon
        for s in self.Stats:
            t = der.Lv5D.GetStat(s)
            der.SetLv7D(s, aer.Lv5D.GetStat(s))
            aer.SetLv7D(s, t)
        a.Controller.ReportBuilder.Add(self.Log, a.Attacker, a.Target.Defender)
M(StatSwap(384, 'PowerSwap', (StatType.Atk, StatType.SpAtk))) 
M(StatSwap(385, 'GuardSwap', (StatType.Def, StatType.SpDef)))
M(StatSwap(391, 'HeartSwap', (StatType.Atk, StatType.Def, StatType.SpAtk, StatType.SpDef, StatType.Speed, StatType.AccuracyLv, StatType.EvasionLv)))

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
M(Defog(432))

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
    def Act(self, a):
        der = a.Target.Defender
        if Items.PlatedArceus(der.Pokemon) or (der.OnboardPokemon.Type1 == BattleType.Water and der.OnboardPokemon.Type2 == BattleType.Invalid):
            self.FailAll(a)
        else:
            der.OnboardPokemon.Type1 = BattleType.Water
            der.OnboardPokemon.Type2 = BattleType.Invalid
            der.AddReportPm('TypeChange', BattleType.Water)
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
        return not (a.Attacker.Pokemon.Item == None or Items.CantLostItem(a.Attacker.Pokemon))
    def Act(self, a):
        if a.Target.Defender.Pokemon.Item == None:
            i = a.Attacker.Pokemon.Item.Id
            a.Attacker.Pokemon.Item = None
            a.Target.Defender.ChangeItem(i, 'Bestow', a.Attacker)
        else:
            self.FailAll(a)
M(Bestow(516))