class WeatherMove(StatusMoveE):
    def __new__(cls, id, weather, item):
        return StatusMoveE.__new__(cls, id)
    def __init__(self, id, weather, item):
        self.Weather = weather
        self.Item = item
    def NotFail(self, atk):
        return atk.Controller.Board.Weather != self.Weather
    def Act(self, atk):
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
        c = atk.Controller
        if c.Board.AddCondition('TrickRoom', c.TurnNumber + 5):
            c.ReportBuilder.Add("EnTrickRoom", atk.Attacker)
        else:
            c.Board.RemoveCondition('TrickRoom')
            c.ReportBuilder.Add("DeTrickRoom")
M(TrickRoom(433))
