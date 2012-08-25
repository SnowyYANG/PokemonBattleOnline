class RockyHelmet(ItemE):
    def Attacked(self, d):
        if d.AtkContext.Move.AdvancedFlags.NeedTouch:
            d.AtkContext.Attacker.EffectHurtByOneNth(6, 'RockyHelmet', 0, 0)
I(RockyHelmet(104))

class AirBalloon(ItemE):
    def Attacked(self, d):
        d.Defender.Pokemon.Item = None
        d.Defender.AddReportPm('DeBalloon', None, None)
I(AirBalloon(105))

class EnigmaBerry(ItemE):
    def Attacked(self, d):
        if d.EffectRevise > 0:
            d.Defender.HpRecoverByOneNth(4, 'ItemRecover', 188, True)
I(EnigmaBerry(188))            