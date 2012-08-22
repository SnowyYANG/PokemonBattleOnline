class NoEffectWithLv7DUp(AbilityE):
    def __new__(cls, id, type, stat):
        return AbilityE.__new__(cls, id)
    def __init__(self, id, type, stat):
        AbilityE.__init__(self, id)
        self.Type = type
        self.Stat = stat
    def CanImplement(self, d):
        if d.AtkContext.Type == self.Type:
            der = d.Defender
            self.Raise(der)
            if not der.ChangeLv7D(der, self.Stat, 1):
                der.AddReportPm('NoEffect', None, None)
            return False
        return True
A(NoEffectWithLv7DUp(48, BattleType.Grass, StatType.Atk)) #sap sipper
A(NoEffectWithLv7DUp(68, BattleType.Electric, StatType.SpAtk)) #lightningrod
A(NoEffectWithLv7DUp(80, BattleType.Electric, StatType.Speed)) #motor drive
A(NoEffectWithLv7DUp(137, BattleType.Water, StatType.SpAtk)) #storm drain

class NoEffectWithAbsorb(AbilityE):
    def __new__(cls, id, type):
        return AbilityE.__new__(cls, id)
    def __init__(self, id, type):
        AbilityE.__init__(self, id)
        self.Type = type
    def CanImplement(self, d):
        if d.AtkContext.Type == self.Type:
            der = d.Defender
            self.Raise(der)
            if der.FullHp:
                der.AddReportPm('NoEffect', None, None)
            else:
                der.HpRecoverByOneNth(4)
            return False
        return True
A(NoEffectWithAbsorb(159, BattleType.Electric)) #volt absorb
A(NoEffectWithAbsorb(160, BattleType.Water)) #water absorb

class Soundproof(AbilityE):
    def CanImplement(self, d):
        if d.AtkContext.Move.AdvancedFlags.IsSound:
            self.Raise(d.Defender)
            d.Defender.AddReportPm('NoEffect', None, None)
            return False
        return True
A(Soundproof(129))

class WonderGuard(AbilityE):
    def CanImplement(self, d):
        type = d.AtkContext.Type
        if type == BattleType.Flying or type == BattleType.Stone or type == BattleType.Ghost or type == BattleType.Fire or type == BattleType.Dark:
            return True
        self.Raise(d.Defender)
        d.Defender.AddReportPm('NoEffect', None, None)
        return False
A(WonderGuard(164))

class Telepathy(AbilityE):
    def CanImplement(self, d):
        if d.AtkContext.Move.Category != MoveCategory.Status and d.AtkContext.Attacker.Pokemon.TeamId == d.Defender.Pokemon.TeamId:
            self.Raise(d.Defender)
            d.Defender.AddReportPm('NoEffect', None, None)
            return False
        return True
A(Telepathy(146))