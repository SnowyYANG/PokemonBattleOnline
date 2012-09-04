class Haze(StatusMoveE):
    def Act(self, atk):
        for pm in atk.Controller.OnboardPokemons:
            pm.OnboardPokemon.ResetLv7D()
        atk.Controller.ReportBuilder.Add('Haze')
M(Haze(114))

class CantSelectWithdraw(StatusMoveE):
    def Act(self, a):
        if a.Target.Defender.OnboardPokemon.AddCondition('CantSelectWithdraw', a.Attacker):
            a.Target.Defender.AddReportPm('CantSelectWithdraw')
        else:
            self.FailAll(a)
M(CantSelectWithdraw(169)) #spider web
M(CantSelectWithdraw(212)) #mean look
M(CantSelectWithdraw(335)) #block

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
        if (atk.Attacker.Item.Id == self.Item):
            turn = 7
        else:
            turn = 4
        atk.Controller.Board.SetCondition('Weather', atk.Controller.TurnNumber + turn)
M(WeatherMove(201, Weather.Sandstorm, 60)) #sandstorm
M(WeatherMove(240, Weather.HeavyRain, 62)) #rain dance
M(WeatherMove(241, Weather.IntenseSunlight, 61)) #sunny day
M(WeatherMove(258, Weather.Hailstorm, 59)) #hail

class Sport(StatusMoveE):
    def __new__(cls, id, condition):
        return StatusMoveE.__new__(cls, id)
    def __init__(self, id, condition):
        self.Condition = condition
    def Act(self, a):
        if a.Attacker.OnboardPokemon.AddCondition(self.Condition):
            a.Controller.ReportBuilder.Add(self.Condition)
        else:
            self.FailAll(a)
M(Sport(300, 'MudSport'))
M(Sport(346, 'WaterSport'))

class TrickRoom(StatusMoveE):
    def Act(self, atk):
        c = atk.Controller
        if c.Board.AddCondition('TrickRoom', c.TurnNumber + 4):
            c.ReportBuilder.Add('EnTrickRoom', atk.Attacker)
        else:
            c.Board.RemoveCondition('TrickRoom')
            c.ReportBuilder.Add('DeTrickRoom')
M(TrickRoom(433))

class MagicRoom(StatusMoveE):
    def Act(self, atk):
        c = atk.Controller
        if c.Board.AddCondition('MagicRoom', c.TurnNumber + 4):
            c.ReportBuilder.Add('EnMagicRoom', atk.Attacker)
        else:
            c.Board.RemoveCondition('MagicRoom')
            c.ReportBuilder.Add('DeMagicRoom')
M(MagicRoom(478))