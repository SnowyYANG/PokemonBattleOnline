class DreamEater(AttackMoveE):
    def HasEffect(self, d):
        return MoveE.HasEffect(self, d) and d.Defender.State == PokemonState.SLP
M(DreamEater(138))

class SuckerPunch(AttackMoveE):
    def NotFail(self, a):
        if MoveE.NotFail(self, a):
            der = a.Target.Defender
            mp = der.SelectedMove
            return der.LastMoveTurn != der.Controller.TurnNumber and mp != None and mp.Move.Type.Category != MoveCategory.Status
        return False
M(SuckerPunch(389))

class Synchronoise(AttackMoveE):
    def HasEffect(self, d):
        d1 = d.Defender.OnboardPokemon.Type1
        d2 = d.Defender.OnboardPokemon.Type2
        a = d.AtkContext.Attacker.OnboardPokemon.Type1
        if a == d1 or a == d2:
            return True
        a = d.AtkContext.Attacker.OnboardPokemon.Type2
        return a != BattleType.Invalid and (a == d1 or a == d2)
M(Synchronoise(485))

