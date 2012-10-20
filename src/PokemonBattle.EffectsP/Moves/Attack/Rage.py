class Rage(AttackMoveE):
    def Execute(self, pm, event):
        AttackMoveE.Execute(self, pm, event)
        pm.OnboardPokemon.SetTurnCondition('Rage')
M(Rage(99))