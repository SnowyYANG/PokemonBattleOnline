class Ct1(ItemE):
    def CtLvRevise(self, pm):
        return 1
I(Ct1(16)) #scope lens
I(Ct1(96)) #razor claw

class Ct2(ItemE):
    def __new__(cls, id, pm):
        return ItemE.__new__(cls, id)
    def __init__(self, id, pm):
        self.Pm = pm
    def CtLvRevise(self, pm):
        if pm.Pokemon.Form.Type.Number == self.Pm:
            return 2
I(Ct2(38, 113)) #lucky punch
I(Ct2(41, 83)) #stick