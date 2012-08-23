class Gem(ItemE):
    def __new__(cls, id, type):
        return ItemE.__new__(cls, id, 'Gem')
    def __init__(self, id, type):
        self.Type = type
    def PowerModifier(self, atk):
        if atk.Type == self.Type:
            atk.RaiseItem = True
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
