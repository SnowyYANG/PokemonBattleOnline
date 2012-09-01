class Rage(AttackMoveE):
    def Execute(self, pm, event):
        AttackMoveE.Execute(self, pm, event)
        if not pm.AtkContext.FailAll:
            pm.OnboardPokemon.SetTurnCondition('Rage')
M(Rage(99))