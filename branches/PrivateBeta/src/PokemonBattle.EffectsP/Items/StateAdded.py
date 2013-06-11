class DeStateBerry(ItemE):
    def __new__(cls, id, state):
        return ItemE.__new__(cls, id)
    def __init__(self, id, state):
        self.State = state
    def e(self, pm):
        if pm.State == self.State:
            pm.DeAbnormalState(True)
    def Attach(self, pm):
        DeStateBerry.e(self, pm)
    def StateAdded(self, pm, by, state):
        DeStateBerry.e(self, pm)
I(DeStateBerry(129, PokemonState.PAR)) #cheri berry
I(DeStateBerry(130, PokemonState.SLP)) #chesto berry
I(DeStateBerry(132, PokemonState.BRN)) #rawst berry
I(DeStateBerry(133, PokemonState.FRZ)) #aspear berry

class PechaBerry(ItemE):
    def e(self, pm):
        if pm.State == PokemonState.PSN or pm.State == PokemonState.BadlyPSN:
            pm.DeAbnormalState(True)
    def Attach(self, pm):
        PechaBerry.e(self, pm)
    def StateAdded(self, pm, by, state):
        if state == AttachedState.PSN:
            PechaBerry.e(self, pm)
I(PechaBerry(131))

class PersimBerry(ItemE):
    def e(self, pm):
        if pm.OnboardPokemon.RemoveCondition('Confuse'):
            pm.ConsumeItem()
            pm.Controller.ReportBuilder.Add(RemoveItem('ItemDeConfuse', pm, 136, 0))
    def Attach(self, pm):
        PerisimBerry.e(self, pm)
    def StateAdded(self, pm, by, state):
        if state == AttachedState.Confusion:
            PerisimBerry.e(self, pm)
I(PersimBerry(136))

class LumBerry(ItemE):
    def e(self, pm):
        if pm.State != PokemonState.Normal:
            pm.DeAbnormalState(True)
    def Attach(self, pm):
        LumBerry.e(self, pm)
    def StateAdded(self, pm, by, state):
        LumBerry.e(self, pm)
I(LumBerry(137))

class DestinyKnot(ItemE):
    def StateAdded(self, pm, by, state):
        if state == AttachedState.Attract:
            by.AddState(pm, AttachedState.Attract, False, 0, 'ItemEnAttract', 57)
I(DestinyKnot(57))