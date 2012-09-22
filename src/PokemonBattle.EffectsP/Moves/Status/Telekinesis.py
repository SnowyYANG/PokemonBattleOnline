class Telekinesis(StatusMoveE):
    def Act(self, a):
        der = a.Target.Defender.OnboardPokemon
        if der.PokemonType.Number == 50 or der.Pokemontype.Number == 51:
            der.AddReportPm('NoEffect')
        else:
            if der.AddCondition('Telekinesis', a.Controller.TurnNumber + 2):
                der.AddReportPm('EnTelekinesis')
            else:
                self.FailAll(a)
M(Telekinesis(477))