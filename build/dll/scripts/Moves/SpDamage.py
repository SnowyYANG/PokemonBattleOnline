class m_spd(AttackMoveE):
    def Calculate(self, d):
        print 'test1'
        if type(d) is DefContext:
            print 'test2'
            self.D(d)

class FixedDamage(m_spd):
    def __new__(cls, id, damage):
        return AttackMoveE.__new__(cls, id)
    def __init__(self, id, damage):
        AttackMoveE.__init__(self, id)
        self.Damage = damage
    def D(self, d):
        d.Damage = self.Damage
M(FixedDamage(49, 20)) #sonicboom
M(FixedDamage(82, 40)) #dragon rage

class Psywave(m_spd):
    def D(self, d):
        d.Damage = d.Defender.Controller.GetRandomInt(50, 150) * d.AtkContext.Attacker.Pokemon.Lv / 100
        if d.Damage == 0:
            d.Damage = 1
M(Psywave(149))

class NightShade(m_spd):
    def D(self, d):
        print 'test3', d.AtkContext.Attacker.Pokemon.Lv
        d.Damage = d.AtkContext.Attacker.Pokemon.Lv
M(NightShade(101))