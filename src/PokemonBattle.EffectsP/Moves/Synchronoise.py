class Synchronoise(AttackMoveE):
    def HasEffect(self, d):
        d1 = d.Defender.OnboardPokemon.Type1
        d2 = d.Defender.OnboardPokemon.Type2
        a = d.AtkContext.Attacker.OnboardPokemon.Type1
        if a == d1 or a1 == d2:
            return True;
        a = d.AtkContext.Attacker.OnboardPokemon.Type2
        return a != BattleType.Invalid and (a == d1 or a == d2)
M(Synchronoise(485))