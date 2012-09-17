class HalfSpeed(ItemE):
    def ADSModifiers(self, pm, stat):
        if stat == StatType.Speed:
            return 0x800
        return 0x1000
I(HalfSpeed(6)) #macho brace
I(HalfSpeed(55)) #iron ball
I(HalfSpeed(66)) #power bracer
I(HalfSpeed(67)) #power belt
I(HalfSpeed(68)) #power lens
I(HalfSpeed(69)) #power band
I(HalfSpeed(70)) #power anklet
I(HalfSpeed(71)) #power weight

class ChoiceItem(ItemE):
    def __new__(cls, id, stat):
        return ItemE.__new__(cls, id)
    def __init__(self, id, stat):
        self.Stat = stat
    def Attach(self, pm):
        pm.OnboardPokemon.RemoveCondition('ChoiceItem')
    def ADSModifiers(self, pm, stat):
        if stat == self.Stat:
            return 0x1800
        return 0x1000
I(ChoiceItem(9, StatType.Atk)) #choice band
I(ChoiceItem(64, StatType.Speed)) #choice scarf
I(ChoiceItem(74, StatType.SpAtk)) #choice specs

class SoulDew(ItemE):
    def ADSModifier(self, pm, stat):
        n = pm.Pokemon.PokemonType.Number
        if (n == 380 or n == 381) and (stat == StatType.SpAtk or stat == StatType.SpDef):
            return 0x1800
        return 0x1000
I(SoulDew(12))

class DeapSea(ItemE):
    def __new__(cls, id, stat):
        return ItemE.__new__(cls, id)
    def __init__(self, id, stat):
        self.Stat = stat
    def ADSModifier(self, pm, stat):
        if pm.Pokemon.PokemonType.Number == 366 and stat == self.Stat:
            return 0x2000
        return 0x1000
I(DeapSea(13, StatType.SpAtk)) #deepseatooth
I(DeapSea(14, StatType.SpDef)) #deepseascale

class LightBall(ItemE):
    def ADSModifier(self, pm, stat):
        if pm.Pokemon.PokemonType.Number == 25 and (stat == StatType.Atk or stat == StatType.SpAtk):
            return 0x2000
        return 0x1000
I(LightBall(19))

class ThickClub(ItemE):
    def ADSModifier(self, pm, stat):
        n = pm.Pokemon.PokemonType.Number
        if (n == 104 or n == 105) and stat == StatType.Atk:
            return 0x2000
        return 0x1000
I(ThickClub(40))

class DittoItem(ItemE):
    def __new__(cls, id, stat):
        return ItemE.__new__(cls, id)
    def __init__(self, id, stat):
        self.Stat = stat
    def ADSModifier(self, pm, stat):
        if stat == self.Stat and pm.Pokemon.PokemonType.Number == 132 and not pm.OnboardPokemon.HasCondition('Transform'):
            return 0x2000
        return 0x1000
I(DittoItem(39, StatType.Def)) #metal powder
I(DittoItem(51, StatType.Speed)) #quick powder

class Eviolite(ItemE):
    def ADSModifier(self, pm, stat):
        #if stat == StatType.Def or stat == StatType.SpDef:
        #    return 0x1800
        return 0x1000
print 'WARNING: Items\\ADSModifier.py Eviolite'
I(Eviolite(102))