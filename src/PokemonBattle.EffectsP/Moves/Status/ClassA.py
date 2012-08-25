class WeatherMove(StatusMoveE):
    def __new__(cls, id, weather, item):
        return StatusMoveE.__new__(cls, id)
    def __init__(self, id, weather, item):
        self.Weather = weather
        self.Item = item
    def Act(self, atk):
        if atk.Controller.Board.Weather == self.Weather:
            self.Fail(atk, None)
        atk.Controller.Weather = self.Weather
        turn = atk.Controller.TurnNumber
        if (atk.Attacker.Item.Id == self.Item):
            turn += 8
        else:
            turn += 5
        atk.Controller.Board.SetCondition('Weather', turn)
M(WeatherMove(201, Weather.Sandstorm, 60))
M(WeatherMove(240, Weather.HeavyRain, 62))
M(WeatherMove(241, Weather.IntenseSunlight, 61))
M(WeatherMove(258, Weather.Hailstorm, 59))

class Haze(StatusMoveE):
    def Act(self, atk):
        for pm in atk.Controller.OnboardPokemons:
            pm.OnboardPokemon.Lv5D.Set5D(0,0,0,0,0)
            pm.OnboardPokemon.AccuracyLv = 0
            pm.OnboardPokemon.EvasionLv = 0
        atk.Controller.ReportBuilder.Add('Haze')
M(Haze(114))

class TrickRoom(StatusMoveE):
    def Act(self, atk):
        if atk.Controller.Board.SetCondition('TrickRoom', 5):
            atk.Controller.ReportBuilder.Add("EnTrickRoom", atk.Attacker)
        else:
            atk.Controller.Board.RemoveCondition('TrickRoom')
            atk.Controller.ReportBuilder.Add("DeTrickRoom")
M(TrickRoom(433))
