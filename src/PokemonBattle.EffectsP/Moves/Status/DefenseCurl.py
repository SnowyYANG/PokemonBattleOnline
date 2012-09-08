class DefenseCurl(StatusMoveE):
    def Act(self, a):
        StatusMoveE.Act(self, a)
        a.Attacker.OnboardPokemon.SetCondition('DefenseCurl')
M(DefenseCurl(111))