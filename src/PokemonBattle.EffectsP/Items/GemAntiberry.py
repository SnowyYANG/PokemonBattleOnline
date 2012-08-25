class Gem(ItemE):
    def __new__(cls, id, type):
        return ItemE.__new__(cls, id)
    def __init__(self, id, type):
        self.Type = type
    def PowerModifier(self, atk):
        if atk.Type == self.Type:
            atk.Attacker.UsingItem = True
            return 0x1800
        return 0x1000
    def RaiseImplement(self, pm, key):
        pm.Controller.ReportBuilder.Add(UseItem('Gem', pm, self.Item.Id, pm.AtkContext.Move.Id))
I(Gem(112, BattleType.Fire))
I(Gem(113, BattleType.Water))
I(Gem(114, BattleType.Electric))
I(Gem(115, BattleType.Grass))
I(Gem(116, BattleType.Ice))
I(Gem(117, BattleType.Fighting))
I(Gem(118, BattleType.Poison))
I(Gem(119, BattleType.Ground))
I(Gem(120, BattleType.Flying))
I(Gem(121, BattleType.Psychic))
I(Gem(122, BattleType.Bug))
I(Gem(123, BattleType.Rock))
I(Gem(124, BattleType.Ghost))
I(Gem(125, BattleType.Dragon))
I(Gem(126, BattleType.Dark))
I(Gem(127, BattleType.Steel))
I(Gem(128, BattleType.Normal))

class AntiBerry(ItemE):
    def __new__(cls, id, type):
        return ItemE.__new__(cls, id)
    def __init__(self, id, type):
        self.Type = type
    def DamageFinalModifier(self, d):
        if d.AtkContext.Type == self.Type and d.EffectRevise > 0:
            d.Defender.UsingItem = True
            return 0x800
        return 0x1000
    def RaiseImplement(self, pm, key):
        pm.Controller.ReportBuilder.Add(UseItem('AntiBerry', pm, self.Item.Id, 0))
I(AntiBerry(164, BattleType.Fire)) #occa berry
I(AntiBerry(165, BattleType.Water)) #passho berry
I(AntiBerry(166, BattleType.Electric)) #wacan berry
I(AntiBerry(167, BattleType.Grass)) #rindo berry
I(AntiBerry(168, BattleType.Ice)) #yache berry
I(AntiBerry(169, BattleType.Fighting)) #chople berry
I(AntiBerry(170, BattleType.Poison)) #kebia berry
I(AntiBerry(171, BattleType.Ground)) #shuca berry
I(AntiBerry(172, BattleType.Flying)) #coba berry
I(AntiBerry(173, BattleType.Psychic)) #payapa berry
I(AntiBerry(174, BattleType.Bug)) #tanga berry
I(AntiBerry(175, BattleType.Rock)) #charti berry
I(AntiBerry(176, BattleType.Ghost)) #kasib berry
I(AntiBerry(177, BattleType.Dragon)) #haban berry
I(AntiBerry(178, BattleType.Dark)) #colbur berry
I(AntiBerry(179, BattleType.Steel)) #babiri berry

class ChilanBerry(ItemE):
    def DamageFinalModifier(d):
        if d.AtkContext.Type == BattleType.Normal:
            d.Defender.UsingItem = True
            return 0x800
        return 0x1000
    def RaiseImplement(self, pm, key):
        pm.Controller.ReportBuilder.Add(UseItem('AntiBerry', pm, self.Item.Id, 0))
I(ChilanBerry(180))