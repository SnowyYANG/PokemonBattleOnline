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
    def PostEffect(self, d):
        if d.Defender.State == self.State:
            d.Defender.DeAbnormalState(False)
M(DeAbnormalState(265, PokemonState.PAR)) #smelling salt
M(DeAbnormalState(358, PokemonState.SLP)) #wake-up slap