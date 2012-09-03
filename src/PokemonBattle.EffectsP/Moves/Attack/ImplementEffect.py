class ClearSmog(AttackMoveE):
    def ImplementEffect(self, d):
        d.Defender.OnboardPokemon.ResetLv7D()
        d.Defender.AddReportPm('7DReset')
        AttackMoveE.ImplementEffect(self, d)
M(ClearSmog(499))