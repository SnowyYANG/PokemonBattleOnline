class OrganBerry(ItemE):
    def __new__(cls, id, hp):
        return ItemE.__new__(cls, id)
    def __init__(self, id, hp):
        self.Hp = hp
    def e(self, pm):
        if pm.Hp << 1 < pm.Pokemon.Hp.Origin and pm.CanHpRecover:
            pm.HpRecover(self.Hp, 'ItemRecover', 135, True)
    def Attach(self, pm):
        OrganBerry.e(self, pm)
    def HpChanged(self, pm):
        OrganBerry.e(self, pm)
I(OrganBerry(135, 10)) #organ berry
I(OrganBerry(194, 20)) #berry juice

class SitrusBerry(ItemE):
    def e(pm):
        if pm.Hp << 1 < pm.Pokemon.Hp.Origin and pm.CanHpRecover:
            pm.HpRecoverByOneNth(4, 'ItemRecover', 138, True)
    def Attach(self, pm):
        SitrusBerry.e(pm)
    def HpChanged(self, pm):
        SitrusBerry.e(pm)
I(SitrusBerry(138))