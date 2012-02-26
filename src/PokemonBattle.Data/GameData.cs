using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics.Contracts;
using System.Windows.Media;

namespace LightStudio.PokemonBattle.Data
{
  internal class GameData
  {
    private Dictionary<int, PokemonType> pokemons;
    private Dictionary<int, MoveType> moves;
    private Dictionary<int, Terrain> terrains;
    private Dictionary<int, Ability> abilities;
    private Dictionary<int, Item> items;
    private Dictionary<int, Rule> rules;

    public GameData(string identifier)
    {
      this.Identifier = identifier;

      pokemons = new Dictionary<int, PokemonType>();
      moves = new Dictionary<int, MoveType>();
      terrains = new Dictionary<int, Terrain>();
      abilities = new Dictionary<int, Ability>();
      items = new Dictionary<int, Item>();
      rules = new Dictionary<int, Rule>();
    }

    public string Identifier
    { get; private set; }

    public IEnumerable<PokemonType> PokemonTypes
    {
      get
      {
        return pokemons.Values;
      }
    }
    public IEnumerable<MoveType> MoveTypes
    {
      get
      {
        return moves.Values;
      }
    }
    public IEnumerable<Ability> Abilities
    {
      get
      {
        return abilities.Values;
      }
    }
    public IEnumerable<Item> Items
    {
      get
      {
        return items.Values;
      }
    }
    public IEnumerable<Rule> Rules
    {
      get
      {
        return rules.Values;
      }
    }
    public IEnumerable<Terrain> Terrains
    {
      get
      {
        return terrains.Values;
      }
    }

    #region AddXXX
    /// <summary>
    /// existing SpriteType with the same id will be overwritten
    /// </summary>
    public void AddPokemonType(PokemonType pokemon)
    {
      Contract.Requires(pokemon != null);
      pokemons[pokemon.Id] = pokemon;
    }

    /// <summary>
    /// existing SkillType with the same id will be overwritten
    /// </summary>
    public void AddMoveType(MoveType move)
    {
      Contract.Requires(move != null);
      moves[move.Id] = move;
    }

    /// <summary>
    /// existing Ability with the same id will be overwritten
    /// </summary>
    public void AddAbility(Ability ability)
    {
      Contract.Requires(ability != null);
      abilities[ability.Id] = ability;
    }

    /// <summary>
    /// existing Item with the same id will be overwritten
    /// </summary>
    public void AddItem(Item item)
    {
      Contract.Requires(item != null);
      items[item.Id] = item;
    }

    ///// <summary>
    ///// existing Mode with the same id will be overwritten
    ///// </summary>
    //public void AddMode(INamedDynamicObject mode)
    //{
    //  Contract.Requires(mode != null);
    //  modes[mode.Id] = mode;
    //}

    /// <summary>
    /// existing Rule with the same id will be overwritten
    /// </summary>
    public void AddRule(Rule rule)
    {
      Contract.Requires(rule != null);
      rules[rule.Id] = rule;
    }

    /// <summary>
    /// existing Terrain with the same id will be overwritten
    /// </summary>
    public void AddTerrain(Terrain terrain)
    {
      Contract.Requires(terrain != null);
      terrains[terrain.Id] = terrain;
    }
    #endregion

    #region AddXXXs
    public void AddPokemonTypes(IEnumerable<PokemonType> pokemons)
    {
      if (pokemons == null)
        return;
      foreach (var sprite in pokemons)
        AddPokemonType(sprite);
    }
    public void AddMoveTypes(IEnumerable<MoveType> moves)
    {
      if (moves == null)
        return;
      foreach (var skill in moves)
        AddMoveType(skill);
    }
    public void AddAbilities(IEnumerable<Ability> abilities)
    {
      if (abilities == null)
        return;
      foreach (var ability in abilities)
        AddAbility(ability);
    }
    public void AddItems(IEnumerable<Item> items)
    {
      if (items == null)
        return;
      foreach (var item in items)
        AddItem(item);
    }
    //public void AddModes(IEnumerable<INamedDynamicObject> modes)
    //{
    //  if (modes == null)
    //    return;
    //  foreach (var mode in modes)
    //    AddMode(mode);
    //}
    public void AddRules(IEnumerable<Rule> rules)
    {
      if (rules == null)
        return;
      foreach (var rule in rules)
        AddRule(rule);
    }
    public void AddTerrains(IEnumerable<Terrain> terrains)
    {
      if (terrains == null)
        return;
      foreach (var terrain in terrains)
        AddTerrain(terrain);
    }
    #endregion
    
    #region GetXXX
    public PokemonType GetPokemonType(int spriteId)
    {
      return pokemons.ValueOrDefault(spriteId);
    }

    public MoveType GetMoveType(int skillId)
    {
      return moves.ValueOrDefault(skillId);
    }

    public Ability GetAbility(int abilityId)
    {
      return abilities.ValueOrDefault(abilityId);
    }

    public Item GetItem(int itemId)
    {
      return items.ValueOrDefault(itemId);
    }

    public Terrain GetTerrain(int terrainId)
    {
      return terrains.ValueOrDefault(terrainId);
    }

    //public INamedDynamicObject GetMode(int modeId)
    //{
    //  return modes.ValueOrDefault(modeId);
    //}

    public Rule GetRule(int ruleId)
    {
      return rules.ValueOrDefault(ruleId);
    }
    #endregion
  }
}
