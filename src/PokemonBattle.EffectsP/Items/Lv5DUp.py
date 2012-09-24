def item_changelv5d(pm, stat, change):
    change = pm.CanChangeLv7D(pm, stat, change, False)
    if change == 0:
        return
    i = pm.Pokemon.Item.Id
    if change == 1:
        e = RemoveItem('Item7DUp1', pm, i, stat)
    else:
        if change == 2:
            e = RemoveItem('Item7DUp2', pm, i, stat)
        else:
            if change > 0:
                e = RemoveItem('Item7DUp3', pm, i, stat)
            else:
                if change == -1:
                    e = RemoveItem('7DDown1', pm, stat)
                else:
                    if change == -2:
                        e = RemoveItem('7DDown2', pm, stat)
                    else:
                        e = RemoveItem('7DDown3', pm, stat)
    pm.OnboardPokemon.ChangeLv7D(stat, change)
    pm.ConsumeItem()
    pm.Controller.ReportBuilder.Add(e)

class Up1Berry(ItemE):
    def __new__(cls, id, stat):
        return ItemE.__new__(cls, id)
    def __init__(self, id, stat):
        self.Stat = stat
    def e(self, pm):
        if Abilities.Gluttony(pm):
            item_changelv5d(pm, self.Stat, 1)
    def Attach(self, pm):
        Up1Berry.e(self, pm)
    def HpChanged(self, pm):
        Up1Berry.e(self, pm)
I(Up1Berry(181, StatType.Atk)) #liechi berry
I(Up1Berry(182, StatType.Def)) #ganlon berry
I(Up1Berry(183, StatType.Speed)) #salac berry
I(Up1Berry(184, StatType.SpAtk)) #petaya berry
I(Up1Berry(185, StatType.SpDef)) #apicot berry

class StarfBerry(ItemE):
    def e(self, pm):
        if Abilities.Gluttony(pm):
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
            if n != 0:
                item_changelv5d(pm, ss[pm.Controller.GetRandomInt(0, n - 1)], 2)
    def Attach(self, pm):
        self.e(pm)
    def HpChanged(self, pm):
        self.e(pm)
I(StarfBerry(187))

class AttackedUpItem(ItemE):
    def __new__(cls, id, type, stat):
        return ItemE.__new__(cls, id)
    def __init__(self, id, type, stat):
        self.Type = type
        self.Stat = stat
    def Attacked(self, d):
        if d.AtkContext.Type == self.Type:
            item_changelv5d(d.Defender, self.Stat, 1)
I(AttackedUpItem(109, BattleType.Water, StatType.SpAtk)) #absorb bulb
I(AttackedUpItem(110, BattleType.Electric, StatType.Atk)) #cell battery