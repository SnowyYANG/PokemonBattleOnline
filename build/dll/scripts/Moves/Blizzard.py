class Blizzard(AttackMoveE): 
    def GetAccuracyBase(self, a):
        if a.Controller.GetAvailableWeather() == Weather.Hailstorm:
            return 0x65
        return MoveE.GetAccuracyBase(self, a)
M(Blizzard(59))