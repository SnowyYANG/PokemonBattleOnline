﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Windows.Media.Imaging;

namespace LightStudio.PokemonBattle.Data
{
  public static class GameDataService
  {
    private const string ROM_FILE = "\\rom.dat";
    private const string IMAGE_FILE = "\\image.zip";

    private static ImageService image;
    public static RomData Rom
    { get; private set; }

    public static void Load(string path)
    {
      Rom = RomData.Load(path + ROM_FILE);
      image = new ImageService(path + IMAGE_FILE);
    }
    public static void Unload()
    {
      image.Dispose();
    }

    public static IEnumerable<Ability> Abilities
    { get { return Rom.Abilities; } }
    public static IEnumerable<Item> Items
    { get { return Rom == null ? null : Rom.Items; } }
    public static IEnumerable<PokemonType> Pokemons
    { get { return Rom == null ? null : Rom.Pokemons; } }
    public static IEnumerable<MoveType> Moves
    { get { return Rom.Moves; } }

    public static PokemonType GetPokemon(int number)
    {
      return Rom.GetPokemonType(number);
    }
    public static PokemonForm GetPokemon(int number, int form)
    {
      return Rom.GetPokemonForm(number, form);
    }
    public static MoveType GetMove(int moveId)
    {
      return Rom.GetMoveType(moveId);
    }
    public static Ability GetAbility(int abilityId)
    {
      return Rom.GetAbility(abilityId);
    }
    public static Item GetItem(int itemId)
    {
      return Rom.GetItem(itemId);
    }
    public static int? GetPreEvolution(int number)
    {
      return Rom.GetPreEvolution(number);
    }
    public static IEnumerable<int> GetEvolutions(int number)
    {
      return Rom.GetEvolutions(number);
    }

    public static BitmapImage GetPokemonIcon(PokemonForm form, PokemonGender gender)
    {
      return image.GetPokemonIcon(form, gender);
    }
    public static BitmapImage GetPokemonFront(PokemonForm form, PokemonGender gender, bool shiny)
    {
      return image.GetPokemonFront(form, gender, shiny);
    }
    public static BitmapImage GetPokemonBack(PokemonForm form, PokemonGender gender, bool shiny)
    {
      return image.GetPokemonBack(form, gender, shiny);
    }
    public static BitmapImage GetSpFront(string id)
    {
      return image.GetSpFront(id);
    }
    public static BitmapImage GetSpBack(string id)
    {
      return image.GetSpBack(id);
    }
    public static BitmapImage GetAvatar(int id)
    {
      return image.GetAvatar(id);
    }
  }
}