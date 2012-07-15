class FixedDamage(AttackMoveE):
    def __new__(cls, id, damage):
        return AttackMoveE.__new__(cls, id)
    def __init__(self, id, damage):
        AttackMoveE.__init__(self, id)
        self.Damage = damage
    def CalculateDamage(self, d):
        d.Damage = self.Damage
M(FixedDamage(49, 20)) #sonicboom
M(FixedDamage(82, 40)) #dragon rage

class Psywave(AttackMoveE):
    def CalculateDamage(self, d):
        d.Damage = d.Defender.Controller.GetRandomInt(50, 150) * d.AtkContext.Attacker.Pokemon.Lv / 100
        if d.Damage == 0:
            d.Damage = 1
M(Psywave(149))

class NightShade(AttackMoveE):
    def CalculateDamage(self, d):
        d.Damage = d.AtkContext.Attacker.Pokemon.Lv
M(NightShade(101))
M(NightShade(69)) #Seismic Toss

class Endeavor(AttackMoveE):
    def CalculateDamage(self, d):
        d.Damage = d.Defender.Hp - d.AtkContext.Attacker.Hp
        if d.Damage < 0:
            d.Damage = 0
M(Endeavor(283))

class FinalGambit(AttackMoveE):
    def CalculateDamage(self, d):
        d.Damage = d.AtkContext.Attacker.Hp
M(FinalGambit(515))

class SuperFang(AttackMoveE):
    def Calculate(self, a):
        a.Target.Damage = a.Target.Defender.Hp >> 1;
        if a.Target.Damage == 0:
            a.Target.Damage = 1
M(SuperFang(162))