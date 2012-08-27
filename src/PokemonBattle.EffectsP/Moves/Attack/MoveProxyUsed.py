class FakeOut(AttackMoveE):
    def NotFail(self, a):
        for m in a.Attacker.Moves:
            if m.HasUsed:
                return False
        return True
M(FakeOut(252))