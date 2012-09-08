class DreamEater(AttackMoveE):
    def Act(self, a):
        if a.Target.Defender.State == PokemonState.Sleeping:
            AttackMoveE.Act(self, a)
        else:
            self.FailAll(a)
M(DreamEater(138))