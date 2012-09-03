class m_continuoususe(StatusMoveE):
    def __new__(cls, id, condition):
        return StatusMoveE.__new__(cls, id)
    def __init__(self, id, condition):
        self.Condition = condition
    def NotFail(self, a):
        if a.Controller.OnboardPokemons[a.Controller.OnboardPokemons.Count - 1] == a.Attacker:
            return False
        o = a.Attacker.OnboardPokemon
        c = o.GetCondition('LastMove')
        if c != None and c.Move == self.Move:
            count = o.GetCondition[int]('ContinuousUse')
            if a.Controller.GetRandomInt(0, 0xffff - 1) < 0xffff >> count:
                o.SetCondition('ContinuousUse', count + 1)
                return True
            return False
        o.SetCondition('ContinuousUse', 1)
        return True
    def FailAll(self, a):
        a.Attacker.OnboardPokemon.RemoveCondition('ContinuousUse')

class SelfProtect(m_continuoususe):
    def Act(self, a):
        a.Attacker.OnboardPokemon.SetTurnCondition(self.Condition)
        a.Attacker.AddReportPm('En' + self.Condition)
M(SelfProtect(182, 'Protect')) #protect
M(SelfProtect(197, 'Protect')) #detect
M(SelfProtect(203, 'Endure')) #endure

class TeamProtect(m_continuoususe):
    def Act(self, a):
        team = a.Attacker.Pokemon.TeamId
        a.Controller.Board[team].SetTurnCondition(self.Condition)
        a.Controller.ReportBuilder.Add('En' + self.Condition, team)
M(TeamProtect(469, 'WideGuard')) #wide guard
M(TeamProtect(501, 'QuickGuard')) #quick guard
