class Blizzard(AttackMoveE):
    
    def __new__(cls):
        return AttackMoveE.__new__(cls, 59)
    
    """description of class"""
    def CanHit(self, d):
        if d.Defender.Controller.GetAvailableWeather() == Weather.Hailstorm:
            return True
        return MoveE.CanHit(d)


M(Blizzard())