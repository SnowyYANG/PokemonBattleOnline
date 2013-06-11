class Telekinesis(StatusMoveE):
    def Act(self, a):
        der = a.Target.Defender.OnboardPokemon
        if der.Form.Type.Number == 50 or der.Form.Type.Number == 51:
            der.AddReportPm('NoEffect')
        else:
            if der.AddCondition('Telekinesis', a.Controller.TurnNumber + 2):
                der.AddReportPm('EnTelekinesis')
            else:
                a.FailAll()
M(Telekinesis(477))