class LansatBerry(ItemE):
    def e(self, pm):
        if Abilities.Gluttony(pm) and pm.OnboardPokemon.AddCondition('FocusEnergy'):
            pm.Controller.ReportBuilder.Add(RemoveItem('ItemEnFocusEnergy', pm, 186))
            pm.ConsumeItem()
    def Attach(self, pm):
        LansatBerry.e(self, pm)
    def HpChanged(self, pm):
        LansatBerry.e(self, pm)
I(LansatBerry(186))