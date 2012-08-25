class CompareValueItem(ItemE):
    def __new__(cls, id, value):
        return ItemE.__new__(cls, id)
    def __init__(self, id, value):
        self.Value = value
    def CompareValue(self, pm):
        return self.Value
I(CompareValueItem(56, -1)) #lagging tail
I(CompareValueItem(93, -1)) #full incense

class QuickClaw(ItemE):
    def Raise(self, pm, key):
        pm.AddReportPm('QuickItem', 7)
    def CompareValue(self, pm):
        if pm.Controller.RandomHappen(20):
            pm.UsingItem = True
            return 1
        return 0
I(QuickClaw(7))

class CustapBerry(ItemE):
    def Raise(self, pm, key):
        pm.AddReportPm('QuickItem', 190)
    def CompareValue(self, pm):
        if pm.Hp << 2 < pm.Pokemon.Hp.Origin or Abilities.Gluttony(pm):
            pm.UsingItem = True
            return 1
        return 0
I(CustapBerry(190))