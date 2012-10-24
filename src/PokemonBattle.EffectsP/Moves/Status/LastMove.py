def last_moveproxy(pm):
    if pm.AtkContext != None and pm.AtkContext.MoveProxy != None:
        for m in pm.Moves:
            if m.Type == pm.AtkContext.MoveProxy.Type:
                if m.PP > 0:
                    return m
                break
    return None

class Spite(StatusMoveE):
    def Act(self, a):
        der = a.Target.Defender
        move = last_moveproxy(der)
        if move == None:
            self.FailAll(a)
        else:
            fp = move.PP
            move.PP -= 4
            a.Controller.ReportBuilder.Add(PPChange('Spite', move, fp))
M(Spite(180))

class Encore(StatusMoveE):
    def Act(self, a):
        der = a.Target.Defender
        move = last_moveproxy(der)
        if move == None or move.Type.Id == 102 or move.Type.Id == 144 or move.Type.Id == 227:
            self.FailAll(a)
        else:
            c = Condition()
            c.Turn = 3
            c.Move = move.Type
            if der.OnboardPokemon.AddCondition('Encore', c):
                der.AddReportPm('EnEncore', None, None)
            else:
                self.FailAll(a)
M(Encore(227))
