class t_a(AbilityE):
    def Attacked(self, d):
        if d.AtkContext.Move.AdvancedFlags.NeedTouch:
            self.TA(d)

class Aftermath(t_a):
    def TA(self, d):
        if d.Defender.Hp == 0 and d.AtkContext.Attacker.CanEffectHurt:
            for pm in d.AtkContext.Controller.OnboardPokemons:
                if pm.Ability.Id == 6:
                    return
            self.Raise(d.Defender)
            d.AtkContext.Attacker.EffectHurtByOneNth(4, 'Hurt', 0, 0)
A(Aftermath(106))

class AngerPoint(t_a):
    def TA(self, d):
        der = d.Defender
        if d.IsCt and der.OnboardPokemon.Lv5D.Atk != 6:
            self.Raise(der)
            der.ChangeLv7D(der, StatType.Atk, 12, False, 'AngerPoint')
A(AngerPoint(83))

class WeakArmor(AbilityE):
    def Attacked(self, d):
        der = d.Defender
        if d.AtkContext.Move.Category == MoveCategory.Physical and (der.CanChangeLv7D(der, StatType.Speed, 1, False) or der.CanChangeLv7D(der, StatType.Def, -1, False)):
            self.Raise(der)
            der.ChangeLv7D(der, False, 0, -1, 0, 0, 1, 0, 0)
A(WeakArmor(133))

class CursedBody(AbilityE):
    def Attacked(self, d):
        if d.AtkContext.Controller.RandomHappen(30) and d.AtkContext.Attacker.CanAddState(d.Defender, AttachedState.Disable, False):
            self.Raise(d.Defender)
            d.AtkContext.Attacker.AddState(d.Defender, AttachedState.Disable, False, 4)
A(CursedBody(130))

class EffectSpore(t_a):
    def TA(self, d):
        a = d.AtkContext
        if a.Controller.RandomHappen(10):
            i = a.Controller.GetRandomInt(0, 2)
            if i == 0:
                state = AttachedState.PAR
            else:
                if i == 1:
                    state = AttachedState.SLP
                else:
                    state = AttachedState.PSN
            if a.Attacker.CanAddState(d.Defender, state, False):
                self.Raise(d.Defender)
                a.Attacker.AddState(d.Defender, state, False)
A(EffectSpore(27))

class Justified(AbilityE):
    def Attacked(self, d):
        der = d.Defender
        if d.AtkContext.Move.Type == BattleType.Dark and der.CanChangeLv7D(der, StatType.Atk, 1, False):
            self.Raise(der)
            der.ChangeLv7D(der, False, 1, 0, 0, 0, 0, 0, 0)
A(Justified(154))

class Mummy(t_a):
    def TA(self, d):
        ab = d.AtkContext.Attacker.OnboardPokemon.Ability
        if ab != 82 and ab != 83: #multitype
            self.Raise(d.Defender)
            d.AtkContext.Attacker.AddReportPm('Mummy', None, None)
            d.AtkContext.Attacker.ChangeAbility(83)
A(Mummy(152))

class AttackedAddState(t_a):
    def __new__(cls, id, state):
        return AbilityE.__new__(cls, id)
    def __init__(self, id, state):
        AbilityE.__init__(self, id)
        self.State = state
    def TA(self, d):
        if d.AtkContext.Attacker.CanAddState(d.Defender, self.State, False) and d.AtkContext.Controller.RandomHappen(30):
            self.Raise(d.Defender)
            d.AtkContext.Attacker.AddState(d.Defender, self.State, False, 0)
A(AttackedAddState(56, AttachedState.Attract)) #cute charm
A(AttackedAddState(49, AttachedState.BRN)) #flame body
A(AttackedAddState(38, AttachedState.PSN)) #poison point
A(AttackedAddState(9, AttachedState.PAR)) #static

class Rattled(AbilityE):
    def Attacked(self, d):
        type = d.AtkContext.Move.Type
        der = d.Defender
        if (type == BattleType.Dark or type == BattleType.Ghost or type == BattleType.Bug) and der.CanChangeLv7D(der, StatType.Speed, 1, False):
            self.Raise(der)
            der.ChangeLv7D(der, False, 0, 0, 0, 0, 1, 0, 0)
A(Rattled(155))

class RoughSkin(t_a):
    def TA(self, d):
        if d.AtkContext.Attacker.CanEffectHurt:
            self.Raise(d.Defender)
            d.AtkContext.Attacker.EffectHurtByOneNth(8, 'Hurt', 0, 0)
A(RoughSkin(24))
A(RoughSkin(160)) #iron barbs

class WickedThief(t_a):
    def TA(self, d):
        der = d.Defender
        aer = d.AtkContext.Attacker
        if der.Pokemon.Item == None and aer.CanLostItem:
            i = aer.Pokemon.Item.Id
            aer.RemoveItem()
            der.ChangeItem(i)
            self.Raise(der)
            der.Controller.ReportBuilder.Add('WickedThief', aer.Id, i)
A(WickedThief(124))