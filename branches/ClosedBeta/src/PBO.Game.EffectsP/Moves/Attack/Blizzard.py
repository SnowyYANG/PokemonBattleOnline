class Blizzard(AttackMoveE): 
    def GetAccuracyBase(self, a):
        if a.Controller.Weather == Weather.Hailstorm:
            return 0x65
        return MoveE.GetAccuracyBase(self, a)
M(Blizzard(59))