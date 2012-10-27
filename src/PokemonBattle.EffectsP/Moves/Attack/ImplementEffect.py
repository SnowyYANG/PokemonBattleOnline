class ClearSmog(AttackMoveE):
    def ImplementEffect(self, d):
        d.Defender.OnboardPokemon.SetLv7D(0, 0, 0, 0, 0, 0, 0)
        d.Defender.AddReportPm('7DReset')
        AttackMoveE.ImplementEffect(self, d)
M(ClearSmog(499))

class AttackSwitch(AttackMoveE):
    def MoveEnding(self, a):
        AttackMoveE.MoveEnding(self, a)
        tile = a.Attacker.Tile
        if tile != None:
            a.Controller.Withdraw(a.Attacker, 'SelfWithdraw', True)
            a.Controller.PauseForSendoutInput(tile)
M(AttackSwitch(369))#u-turn
M(AttackSwitch(521))#volt switch