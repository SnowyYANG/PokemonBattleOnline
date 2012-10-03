class TypeItem(ItemE):
    def __new__(cls, id, type):
        return ItemE.__new__(cls, id)
    def __init__(self, id, type):
        self.Type = type
    def PowerModifier(self, atk):
        if atk.Type == self.Type:
            return 0x1333
        return 0x1000
I(TypeItem(11, BattleType.Bug)) #silverpowder
I(TypeItem(17, BattleType.Steel)) #metal coat
I(TypeItem(20, BattleType.Ground)) #soft sand
I(TypeItem(21, BattleType.Rock)) #hard stone
I(TypeItem(22, BattleType.Grass)) #miracle seed
I(TypeItem(23, BattleType.Dark)) #blackglasses
I(TypeItem(24, BattleType.Fighting)) #black belt
I(TypeItem(25, BattleType.Electric)) #magnet
I(TypeItem(26, BattleType.Water)) #mystic water
I(TypeItem(27, BattleType.Flying)) #sharp beak
I(TypeItem(28, BattleType.Poison)) #poison barb
I(TypeItem(29, BattleType.Ice)) #nevermeltice
I(TypeItem(30, BattleType.Ghost)) #spell tag
I(TypeItem(31, BattleType.Psychic)) #twistedspoon
I(TypeItem(32, BattleType.Fire)) #charcoal
I(TypeItem(33, BattleType.Dragon)) #dragon fang
I(TypeItem(34, BattleType.Normal)) #silk scarf
I(TypeItem(36, BattleType.Water)) #sea incense
I(TypeItem(75, BattleType.Fire)) #flame plate
I(TypeItem(76, BattleType.Water)) #splash plate
I(TypeItem(77, BattleType.Electric)) #zap plate
I(TypeItem(78, BattleType.Grass)) #meadow plate
I(TypeItem(79, BattleType.Ice)) #icicle plate
I(TypeItem(80, BattleType.Fighting)) #fist plate
I(TypeItem(81, BattleType.Poison)) #toxic plate
I(TypeItem(82, BattleType.Ground)) #earth plate
I(TypeItem(83, BattleType.Flying)) #sky plate
I(TypeItem(84, BattleType.Psychic)) #mind plate
I(TypeItem(85, BattleType.Bug)) #insect plate
I(TypeItem(86, BattleType.Rock)) #stone plate
I(TypeItem(87, BattleType.Ghost)) #spooky plate
I(TypeItem(88, BattleType.Dragon)) #draco plate
I(TypeItem(89, BattleType.Dark)) #dread plate
I(TypeItem(90, BattleType.Steel)) #iron plate
I(TypeItem(91, BattleType.Psychic)) #odd incense
I(TypeItem(92, BattleType.Rock)) #rock incense
I(TypeItem(94, BattleType.Water)) #wave incense
I(TypeItem(95, BattleType.Grass)) #rose incense

class Orb(ItemE):
    def __new__(cls, id, pm, type):
        return ItemE.__new__(cls, id)
    def __init__(self, id, pm, type):
        self.Pm = pm
        self.Type = type
    def PowerModifier(self, atk):
        if atk.Attacker.Pokemon.Forme.Type.Number == self.Pm and (atk.Type == BattleType.Dragon or atk.Type == self.Type):
            return 0x1333
        return 0x1000
I(Orb(1, 487, BattleType.Ghost))
I(Orb(2, 483, BattleType.Steel))
I(Orb(3, 484, BattleType.Water))

class MuscleBand(ItemE):
    def __new__(cls, id, category):
        return ItemE.__new__(cls, id)
    def __init__(self, id, category):
        self.Category = category
    def PowerModifier(self, atk):
        if atk.Move.Category == self.Category:
            return 0x1199
        return 0x1199
I(MuscleBand(43, MoveCategory.Physical))
I(MuscleBand(44, MoveCategory.Special)) #wise glasses