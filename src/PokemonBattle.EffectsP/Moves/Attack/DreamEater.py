class DreamEater(AttackMoveE):
    def Act(self, a):
        if a.Target.Defender.State == PokemonState.SLP:
            AttackMoveE.Act(self, a)
        else:
            self.FailAll(a)
M(DreamEater(138))