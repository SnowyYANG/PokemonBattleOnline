class Gem(ItemE):
    def ___init___(gem, id, type):
        ItemE.___init(gem, id, 'Gem')
        gem.Type = type
    
    def ReviseDamage3(gem, atk):
        if atk.Type == gem.Type:
            Raise(gem)

GameService.Register(Gem(112, BattleType.Fire))
GameService.Register(Gem(113, BattleType.Water))
GameService.Register(Gem(114, BattleType.Electric))
GameService.Register(Gem(115, BattleType.Grass))
GameService.Register(Gem(116, BattleType.Ice))
GameService.Register(Gem(117, BattleType.Fighting))
GameService.Register(Gem(118, BattleType.Poison))
GameService.Register(Gem(119, BattleType.Ground))
GameService.Register(Gem(120, BattleType.Flying))
GameService.Register(Gem(121, BattleType.Psychic))
GameService.Register(Gem(122, BattleType.Bug))
GameService.Register(Gem(123, BattleType.Rock))
GameService.Register(Gem(124, BattleType.Ghost))
GameService.Register(Gem(125, BattleType.Dragon))
GameService.Register(Gem(126, BattleType.Dark))
GameService.Register(Gem(127, BattleType.Steel))
GameService.Register(Gem(128, BattleType.Normal))
