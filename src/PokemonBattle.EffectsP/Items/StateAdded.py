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
I(DeStateBerry(129, PokemonState.Paralyzed)) #cheri berry
I(DeStateBerry(130, PokemonState.Sleeping)) #chesto berry
I(DeStateBerry(132, PokemonState.Burned)) #rawst berry
I(DeStateBerry(133, PokemonState.Frozen)) #aspear berry

class PechaBerry(ItemE):
    def e(self, pm):
        if pm.State == PokemonState.Poisoned or pm.State == PokemonState.BadlyPoisoned:
            pm.DeAbnormalState(True)
    def Attach(self, pm):
        PechaBerry.e(self, pm)
    def StateAdded(self, pm, by, state):
        if state == AttachedState.Poison:
            PechaBerry.e(self, pm)
I(PechaBerry(131))

class PersimBerry(ItemE):
    def e(self, pm):
        if pm.OnboardPokemon.HasCondition('Confused'):
            pm.ConsumeItem()
            pm.OnboardPokemon.RemoveCondition('Confused')
            pm.Controller.ReportBuilder.Add(RemoveItem('ItemDeConfused', pm, 136, 0))
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
        if state == AttachedState.Infatuation:
            by.AddState(pm, AttachedState.Infatuation, False, 0, 'ItemEnInfatuation', 57)
I(DestinyKnot(57))