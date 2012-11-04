class Judgment(AttackMoveE):
    def CalculateType(self, a):
        a.Type = Items.PlateType(a.Attacker.Pokemon.Item)
M(Judgment(449))

class FusionFlare(AttackMoveE):
    def PowerModifier(self, d):
        d.Defender.Controller.Board.SetTurnCondition('FusionFlare')
        if d.Defender.Controller.Board.HasCondition('FusionBolt'):
            return 0x2000
        return 0x1000
M(FusionFlare(558))

class FusionBolt(AttackMoveE):
    def PowerModifier(self, d):
        d.Defender.Controller.Board.SetTurnCondition('FusionBolt')
        if d.Defender.Controller.Board.HasCondition('FusionFlare'):
            return 0x2000
        return 0x1000
M(FusionBolt(559))

class TechnoBlast(AttackMoveE):
    def CalculateType(self, a):
        i = a.Attacker.Item.Id
        if i == 98: #douse drive
            a.Type = BattleType.Water
        else:
            if i == 99: #shock drive
                a.Type = BattleType.Electric
            else:
                if i == 100: #burn drive
                    a.Type = BattleType.Fire
                else:
                    if i == 101: #chill drive
                        a.Type = BattleType.Ice
                    else:
                        a.Type = BattleType.Normal
M(TechnoBlast(546))