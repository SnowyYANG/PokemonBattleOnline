class Telekinesis(StatusMoveE):
    def Act(self, a):
        der = a.Target.Defender.OnboardPokemon
        if der.PokemonType.Number == 50 or der.Pokemontype.Number == 51 or der.HasCondition('Telekinesis'):
            self.FailAll(a)
        else:
            der.SetCondition('Telekinesis', a.Controller.TurnNumber + 2)
M(Telekinesis(477))