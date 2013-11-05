using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.PBO.Converters
{
  class BattleTypeString : Converter<BattleType>
  {
    public static readonly BattleTypeString C = new BattleTypeString();
    protected override object Convert(BattleType value)
    {
      return GameString.Current.BattleType(value);
    }
  }
  class NatureString : Converter<PokemonNature>
  {
    public static readonly NatureString C = new NatureString();
    protected override object Convert(PokemonNature value)
    {
      return GameString.Current.Nature(value);
    }
  }
  class PokemonSpeciesString : Converter<int>
  {
    public static readonly PokemonSpeciesString C = new PokemonSpeciesString();
    protected override object Convert(int value)
    {
      return GameString.Current.Pokemon(value);
    }
  }
  class PokemonFormString : Converter<PokemonForm>
  {
    public static readonly PokemonFormString C = new PokemonFormString();
    protected override object Convert(PokemonForm value)
    {
      return GameString.Current.Pokemon(value.Species.Number, value.Index);
    }
  }
  class MoveString : Converter<int>
  {
    public static MoveString C = new MoveString();
    protected override object Convert(int value)
    {
      return GameString.Current.Move(value);
    }
  }
  class MoveDString : Converter<int>
  {
    public static MoveDString C = new MoveDString();
    protected override object Convert(int value)
    {
      return GameString.Current.MoveD(value);
    }
  }
  class AbilityString : Converter<int>
  {
    public static readonly AbilityString C = new AbilityString();
    protected override object Convert(int value)
    {
      return GameString.Current.Ability(value);
    }
  }
  class AbilityDString : Converter<int>
  {
    public static readonly AbilityDString C = new AbilityDString();
    protected override object Convert(int value)
    {
      return GameString.Current.AbilityD(value);
    }
  }
  class ItemString : Converter<int>
  {
    public static ItemString C = new ItemString();
    protected override object Convert(int value)
    {
      return GameString.Current.Item(value);
    }
  }
}
