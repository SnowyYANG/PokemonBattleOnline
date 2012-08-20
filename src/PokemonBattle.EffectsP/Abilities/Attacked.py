class t_a(AbilityE):
    def Attacked(self, d):
        if d.AtkContext.Move.AdvancedFlags.NeedTouch:
            self.TA(d)

class Aftermath(t_a):
    def TA(self, d):
        if d.Defender.Hp == 0 and d.AtkContext.Attacker.CanEffectHurt:
            for pm in d.AtkContext.Controller.OnboardPokemons:
                if pm.Ability.Id == 20:
                    return
            self.Raise(d.Defender)
            d.AtkContext.Attacker.EffectHurtByOneNth(4, 'Hurt', 0, 0)
A(Aftermath(2))

class AngerPoint(t_a):
    def TA(self, d):
        if d.IsCt and d.Defender.CanChangeLv7D(d.Defender, False, StatType.Atk, 12):
            self.Raise(d.Defender)
            d.Defender.ChangeLv7D(d.Defender, False, 12, 0, 0, 0, 0, 0, 0)
A(AngerPoint(5))

class WeakArmor(AbilityE):
    def Attacked(self, d):
        der = d.Defender
        if d.AtkContext.Move.Category == MoveCategory.Physical and (der.CanChangeLv7D(der, False, StatType.Atk, 1) or der.CanChangeLv7D(der, False, StatType.Def, -1)):
            self.Raise(der)
            der.ChangeLv7D(der, False, 1, -1, 0, 0, 0, 0, 0)
A(WeakArmor(11))

class CursedBody(AbilityE):
    def Attacked(self, d):
        if d.AtkContext.Controller.RandomHappen(30) and d.AtkContext.Attacker.CanAddState(d.Defender, AttachedState.Disable, False):
            self.Raise(d.Defender)
            d.AtkContext.Attacker.AddState(d.Defender, AttachedState.Disable, False)
A(CursedBody(15))

class EffectSpore(t_a):
    def TA(self, d):
        a = d.AtkContext
        if a.Controller.RandomHappen(10):
            i = a.Controller.GetRandomInt(0, 2)
            if i == 0:
                state = AttachedState.Paralysis
            else:
                if i == 1:
                    state = AttachedState.Sleep
                else:
                    state = AttachedState.Poison
            if a.Attacker.CanAddState(d.Defender, state, False):
                self.Raise(d.Defender)
                a.Attacker.AddState(d.Defender, state, False)
A(EffectSpore(29))

class Justified(AbilityE):
    def Attacked(self, d):
        der = d.Defender
        if d.AtkContext.Move.Type == BattleType.Dark and der.CanChangeLv7D(der, False, StatType.Def, 1):
            self.Raise(der)
            der.ChangeLv7D(der, False, 0, 1, 0, 0, 0, 0, 0)
A(Justified(44))

class Mummy(t_a):
    def TA(self, d):
        ab = d.AtkContext.Attacker.Ability.Id
        if ab != 82 and ab != 83:
            self.Raise(d.Defender)
            d.AtkContext.Attacker.ChangeAbility(83)
A(Mummy(83))

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
A(AttackedAddState(19, AttachedState.Infatuation)) #cute charm
A(AttackedAddState(33, AttachedState.Burn)) #flame body
A(AttackedAddState(97, AttachedState.Poison)) #poison point
A(AttackedAddState(132, AttachedState.Paralysis)) #static

class Rattled(AbilityE):
    def Attacked(self, d):
        type = d.AtkContext.Move.Type
        der = d.Defender
        if (type == BattleType.Dark or type == BattleType.Ghost or type == BattleType.Bug) and der.CanChangeLv7D(der, False, StatType.Speed, 1):
            self.Raise(der)
            der.ChangeLv7D(der, False, 0, 0, 0, 0, 1, 0, 0)
A(Rattled(114))

class RoughSkin(t_a):
    def TA(self, d):
        if d.AtkContext.Attacker.CanEffectHurt:
            self.Raise(d.Defender)
            d.AtkContext.Attacker.EffectHurtByOneNth(8, 'Hurt', 0, 0)
A(RoughSkin(107))
A(RoughSkin(134)) #iron barbs