class Add5TurnCondition(StatusMoveE):
    def __new__(cls, id, condition):
        return StatusMoveE.__new__(cls, id)
    def __init__(self, id, condition):
        self.Condition = condition
    def Act(self, a):
        team = a.Attacker.Pokemon.TeamId
        if a.Controller.Board[team].AddCondition(self.Condition, a.Controller.TurnNumber + 4):
            a.Controller.ReportBuilder.Add('En' + self.Condition, team)
        else:
            self.FailAll(a)
M(Add5TurnCondition(54, 'Mist')) #mist
M(Add5TurnCondition(113, 'LightScreen')) #light screen
M(Add5TurnCondition(115, 'Reflect')) #reflect
M(Add5TurnCondition(219, 'Safegurad')) #safeguard
M(Add5TurnCondition(381, 'LuckyChant')) #luck chant

class EntryHazards(StatusMoveE):
    def __new__(cls, id, log):
        return StatusMoveE.__new__(cls, id)
    def __init__(self, id, log):
        self.Log = log
    def Act(self, a):
        team = 1 - a.Attacker.Pokemon.TeamId
        a.Controller.Board[team].AddEntryHazards(self.Move)
        a.Controller.ReportBuilder.Add(self.Log, team)
M(EntryHazards(191, 'EnSpikes'))
M(EntryHazards(390, 'EnToxicSpikes'))
M(EntryHazards(446, 'EnStealthRock'))

class Tailwind(StatusMoveE):
    def Act(self, a):
        team = a.Attacker.Pokemon.TeamId
        if a.Controller.Board[team].AddCondition('Tailwind', a.Controller.TurnNumber + 4):
            a.Controller.ReportBuilder.Add('EnTailwind', team)
        else:
            self.FailAll(a)
M(Tailwind(366))