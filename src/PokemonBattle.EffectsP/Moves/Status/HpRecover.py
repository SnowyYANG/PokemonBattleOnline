class Roost(StatusMoveE):
    def Act(self, a):
        StatusMoveE.Act(self, a)
        a.Attacker.OnboardPokemon.SetTurnCondition('Roost')
M(Roost(355))