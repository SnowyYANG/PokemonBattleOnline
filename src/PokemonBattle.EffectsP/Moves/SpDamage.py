class m_spd(AttackMoveE):
    def CalculateAtk(self, d):
        """"""

class FixedDamage(m_spd):
    def __new__(cls, id, damage):
        return AttackMoveE.__new__(cls, id)
    def __init__(self, id, damage):
        AttackMoveE.__init__(self, id)
        self.Damage = damage
    def CalculateDef(self, d):
        d.Damage = self.Damage
M(FixedDamage(49, 20)) #sonicboom
M(FixedDamage(82, 40)) #dragon rage

class Psywave(m_spd):
    def CalculateDef(self, d):
        d.Damage = d.Defender.Controller.GetRandomInt(50, 150) * d.AtkContext.Attacker.Pokemon.Lv / 100
        if d.Damage == 0:
            d.Damage = 1
M(Psywave(149))

class NightShade(m_spd):
    def CalculateDef(self, d):
        d.Damage = d.AtkContext.Attacker.Pokemon.Lv
M(NightShade(101))
M(NightShade(69)) #Seismic Toss

class Endeavor(m_spd):
    def CalculateDef(self, d):
        d.Damage = d.Defender.Hp - d.AtkContext.Attacker.Hp
        if d.Damage < 0:
            d.Damage = 0
M(Endeavor(283))

class FinalGambit(m_spd):
    def CalculateDef(self, d):
        d.Damage = d.AtkContext.Attacker.Hp
M(FinalGambit(515))