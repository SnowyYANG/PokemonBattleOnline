class ClearSmog(AttackMoveE):
    def ImplementEffect(self, d):
        d.Defender.OnboardPokemon.SetLv7D(0, 0, 0, 0, 0, 0, 0)
        d.Defender.AddReportPm('7DReset')
        AttackMoveE.ImplementEffect(self, d)
M(ClearSmog(499))