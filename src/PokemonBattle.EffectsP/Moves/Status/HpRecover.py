class Moonlight(StatusMoveE):
    def Act(self, a):
        aer = a.Attacker
        if aer.CanHpRecover(True):
            hp = aer.Pokemon.Hp.Origin
            w = a.Controller.Weather
            if w == Weather.IntenseSunlight:
                hp = hp * 2 / 3
            else:
                if w == Weather.Normal:
                    hp >>= 1
                else:
                    hp >>= 2
            aer.AddReportPm('Moonlight')
            aer.HpRecover(hp)
        else:
            a.AllFail = True
M(Moonlight(236))

class Roost(StatusMoveE):
    def Act(self, a):
        StatusMoveE.Act(self, a)
        a.Attacker.OnboardPokemon.SetTurnCondition('Roost')
M(Roost(355))