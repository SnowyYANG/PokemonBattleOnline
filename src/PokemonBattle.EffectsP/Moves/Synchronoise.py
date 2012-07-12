class Synchronoise(AttackMoveE):
    def HasEffect(self, d):
        d1 = d.Defender.OnboardPokemon.Type1
        d2 = d.Defender.OnboardPokemon.Type2
        a = d.AtkContext.Attacker.OnboardPokemon.Type1
        print 'test1'
        if a == d1 or a == d2:
            return True
        print 'test2'
        a = d.AtkContext.Attacker.OnboardPokemon.Type2
        print a != BattleType.Invalid and (a == d1 or a == d2)
        return a != BattleType.Invalid and (a == d1 or a == d2)
M(Synchronoise(485))