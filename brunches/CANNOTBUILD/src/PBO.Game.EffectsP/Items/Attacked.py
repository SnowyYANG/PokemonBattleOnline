class RockyHelmet(ItemE):
    def Attacked(self, d):
        if d.AtkContext.Move.Flags.NeedTouch:
            d.AtkContext.Attacker.EffectHurtByOneNth(6, 'RockyHelmet', 0, 0)
I(RockyHelmet(104))

class AirBalloon(ItemE):
    def Attacked(self, d):
        d.Defender.RemoveItem()
        d.Defender.AddReportPm('DeBalloon', None, None)
I(AirBalloon(105))
print 'WARNING: AirBalloon is wrong, consider substitute'

class EnigmaBerry(ItemE):
    def Attacked(self, d):
        if d.EffectRevise > 0:
            d.Defender.HpRecoverByOneNth(4, False, 'ItemRecover', 188, True)
I(EnigmaBerry(188))