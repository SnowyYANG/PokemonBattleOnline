class ForceSwitch(AttackMoveE):
    def ImplementEffect(self, d):
        AttackMoveE.ImplementEffect(self, d)
        MoveE.ForceSwitch(d)
M(ForceSwitch(509)) #circle throw
M(ForceSwitch(525)) #dragon tail