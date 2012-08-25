def item_changelv5d(pm, stat, change):
    if Abilities.Gluttony(pm):
        change = pm.CanChangeLv7D(pm, stat, change, False)
        if change == 0:
            return
        i = pm.Pokemon.Item.Id
        if change == 1:
            e = UseItem('Item7DUp1', pm, i)
        else:
            if change == 2:
                e = UseItem('Item7DUp2', pm, i)
            else:
                if change > 0:
                    e = UseItem('Item7DUp3', pm, i)
                else:
                    if change == -1:
                        e = UseItem('7DDown1', pm)
                    else:
                        if change == -2:
                            e = UseItem('7DDown2', pm)
                        else:
                            e = UseItem('7DDown3', pm)
        pm.OnboardPokemon.ChangeLv7D(stat, change)
        pm.ConsumeItem()
        pm.Controller.ReportBuilder.Add(e)

class Up1Berry(ItemE):
    def __new__(cls, id, stat):
        return ItemE.__new__(cls, id)
    def __init__(self, id, stat):
        self.Stat = stat
    def e(self, pm):
        item_changelv5d(pm, self.Stat, 1)
        pm.ConsumeItem()
    def Attach(self, pm):
        e(self, pm)
    def HpChanged(self, pm):
        e(self, pm)
I(Up1Berry(181, StatType.Atk)) #liechi berry
I(Up1Berry(182, StatType.Def)) #ganlon berry
I(Up1Berry(183, StatType.Speed)) #salac berry
I(Up1Berry(184, StatType.SpAtk)) #petaya berry
I(Up1Berry(185, StatType.SpDef)) #apicot berry

class StarfBerry(ItemE):
    def e(pm):
        ss = []
        if pm.CanChangeLv7D(pm, StatType.Atk, 2, False) != 0:
            ss.append(StatType.Atk)
        if pm.CanChangeLv7D(pm, StatType.Def, 2, False) != 0:
            ss.append(StatType.Def)
        if pm.CanChangeLv7D(pm, StatType.SpAtk, 2, False) != 0:
            ss.append(StatType.SpAtk)
        if pm.CanChangeLv7D(pm, StatType.SpDef, 2, False) != 0:
            ss.append(StatType.SpDef)
        if pm.CanChangeLv7D(pm, StatType.Speed, 2, False) != 0:
            ss.append(StatType.Speed)
        n = len(ss)
        if i != 0:
            item_changelv5d(pm, ss[pm.Controller.GetRandomInt(0, n - 1)], 2)
    def Attach(self, pm):
        e(pm)
    def HpChanged(self, pm):
        e(pm)
I(StarfBerry(187))

class AttackedUpItem(ItemE):
    def __new__(cls, id, type, stat):
        return ItemE.__new__(cls, id)
    def __init__(self, id, type, stat):
        self.Type = type
        self.Stat = stat
    def Attacked(self, d):
        if d.AtkContext.Type == self.Type and d.Defender.CanChangeLv7D(d.Defender, self.Stat, 1, False) != 0:
            item_changelv5d(d.Defender, self.Stat, 1)
I(AttackedUpItem(109, BattleType.Water, StatType.SpAtk)) #absorb bulb
I(AttackedUpItem(110, BattleType.Electric, StatType.Atk)) #cell battery