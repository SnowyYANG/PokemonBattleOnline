class DeAbnormalState(AttackMoveE):
    def __new__(cls, id, state):
        return AttackMoveE.__new__(cls, id)
    def __init__(self, id, state):
        self.State = state
    def CalculateBasePower(self, d):
        if d.Defender.State == self.State:
            d.BasePower = 120
        else:
            d.BasePower = 60
    def ImplementEffect(self, d):
        if d.AtkContext.Attacker.Tile != None and d.Defender.State == self.State:
            d.Defender.DeAbnormalState(False)
M(DeAbnormalState(265, PokemonState.Paralyzed)) #smelling salt
M(DeAbnormalState(358, PokemonState.Sleeping)) #wake-up slap