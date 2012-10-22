class Rage(AttackMoveE):
    def Execute(self, pm, event, flag):
        AttackMoveE.Execute(self, pm, event, flag)
        pm.OnboardPokemon.SetTurnCondition('Rage')
M(Rage(99))