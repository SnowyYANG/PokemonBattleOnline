class Rage(AttackMoveE):
    def Execute(self, pm, flag):
        AttackMoveE.Execute(self, pm, flag)
        pm.OnboardPokemon.SetCondition('Rage')
M(Rage(99))