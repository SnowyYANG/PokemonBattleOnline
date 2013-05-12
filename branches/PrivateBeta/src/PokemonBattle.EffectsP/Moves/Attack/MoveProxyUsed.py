class FakeOut(AttackMoveE):
    def NotFail(self, a):
        for m in a.Attacker.Moves:
            if m.HasUsed:
                return False
        return True
M(FakeOut(252))

class LastResort(AttackMoveE):
    def NotFail(self, a):
        for m in a.Attacker.Moves:
            if m.Type.Id != 387 and not m.HasUsed:
                return False
        return True
M(LastResort(387))