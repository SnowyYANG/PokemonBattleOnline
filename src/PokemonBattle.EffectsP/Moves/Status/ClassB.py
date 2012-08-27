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