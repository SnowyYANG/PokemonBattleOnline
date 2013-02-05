class HalfSpeed(ItemE):
    def SModifier(self, pm):
        return 0x800
I(HalfSpeed(6)) #macho brace
I(HalfSpeed(55)) #iron ball
I(HalfSpeed(66)) #power bracer
I(HalfSpeed(67)) #power belt
I(HalfSpeed(68)) #power lens
I(HalfSpeed(69)) #power band
I(HalfSpeed(70)) #power anklet
I(HalfSpeed(71)) #power weight

class QuickPowder(ItemE):
    def SModifier(self, pm):
        if pm.Pokemon.Form.Type.Number == 132 and not pm.OnboardPokemon.HasCondition('Transform'):
            return 0x2000
        return 0x1000
I(QuickPowder(51))

class ChoiceScarf(ItemE):
    def SModifier(self, pm):
        return 0x1800
I(ChoiceScarf(64))

class ChoiceItem(ItemE):
    def __new__(cls, id, cat):
        return ItemE.__new__(cls, id)
    def __init__(self, id, cat):
        self.Category = cat
    def AModifier(self, a):
        if a.Move.Category == self.Category:
            return 0x1800
        return 0x1000
I(ChoiceItem(9, MoveCategory.Physical)) #choice band
I(ChoiceItem(74, MoveCategory.Special)) #choice specs

class SoulDew(ItemE):
    def AModifier(self, a):
        n = a.Attacker.Pokemon.Form.Type.Number
        if (n == 380 or n == 381) and a.Move.Category == MoveCategory.Special:
            return 0x1800
        return 0x1000
    def DModifier(self, d):
        n = d.Defender.Pokemon.Form.Type.Number
        if (n == 380 or n == 381) and d.AtkContext.Move.Category == MoveCategory.Special:
            return 0x1800
        return 0x1000
I(SoulDew(12))

class DeapSeaTooth(ItemE):
    def AModifier(self, a):
        if a.Attacker.Pokemon.Form.Type.Number == 366 and a.Move.Category == MoveCategory.Special:
            return 0x2000
        return 0x1000
I(DeapSeaTooth(13)) #deepseatooth

class DeapSeaScale(ItemE):
    def DModifier(self, d):
        if d.Defender.Pokemon.Form.Type.Number == 366 and d.AtkContext.Move.Category == MoveCategory.Special:
            return 0x2000
        return 0x1000
I(DeapSeaScale(14))

class LightBall(ItemE):
    def AModifier(self, a):
        if a.Attacker.Pokemon.Form.Type.Number == 25:
            return 0x2000
        return 0x1000
I(LightBall(19))

class MetalPowder(ItemE):
    def DModifier(self, d):
        if d.AtkContext.Move.Category == MoveCategory.Physical and d.Defender.Pokemon.Form.Type.Number == 132 and not d.Defender.OnboardPokemon.HasCondition('Transform'):
            return 0x2000
        return 0x1000
I(MetalPowder(39))

class ThickClub(ItemE):
    def AModifier(self, a):
        n = a.Attacker.Pokemon.Form.Type.Number
        if (n == 104 or n == 105) and a.Move.Category == MoveCategory.Physical:
            return 0x2000
        return 0x1000
I(ThickClub(40))