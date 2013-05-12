using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Data
{
  public static class PokemonValidator
  {
    public static bool ValidateName(string name)
    {
      return name == null || name.Length < 11 && !name.Any((c) => c == '\n' || c == '\r' || c == '\t');
    }

    public static bool Shiney(IPokemonData pm, int random)
    {
      return random % 1366 == 0;
    }

    public static bool ValidateLv(IPokemonData pm, int lv)
    {
      return 0 < pm.Lv && pm.Lv <= 100;
    }

    public static bool ValidateAbility(IPokemonData pm)
    {
      return pm.Form.Data.GetAbility(pm.AbilityIndex) != null;
    }

    public static bool ValidateIv(int value)
    {
      return value >= 0 && value <= 31;
    }
    public static bool ValidateIv(I6D iv)
    {
      return ValidateIv(iv.Hp) && ValidateIv(iv.Atk) && ValidateIv(iv.Def) && ValidateIv(iv.SpAtk) && ValidateIv(iv.SpDef) && ValidateIv(iv.Speed);
    }

    public static bool ValidateEv(I6D ev)
    {
      return ev.Hp + ev.Atk + ev.Def + ev.Speed + ev.SpAtk + ev.SpDef <= 510;
    }

    public static bool ValidateMoves(IPokemonData pm)
    {
      return pm.Moves.Count() <= 4;
    }

    public static bool Validate(IPokemonData pm)
    {
      return true;
      return
        pm.Form != null &&
        ValidateAbility(pm) &&
        ValidateEv(pm.Ev) &&
        ValidateLv(pm, pm.Lv) &&
        ValidateIv(pm.Iv) &&
        ValidateMoves(pm);
    }

    public static bool ValueEquals(this I6D a, I6D b)
    {
      return
        a.Hp == b.Hp &&
        a.Atk == b.Atk &&
        a.Def == b.Def &&
        a.SpAtk == b.SpAtk &&
        a.SpDef == b.SpDef &&
        a.Speed == b.Speed;
    }
    /// <summary>
    /// all but Name
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool ValueEquals(this IPokemonData a, IPokemonData b)
    {
      return
        a.AbilityIndex == b.AbilityIndex &&
        a.Ev.ValueEquals(b.Ev) &&
        a.Form == b.Form &&
        a.Gender == b.Gender &&
        a.Happiness == b.Happiness &&
        a.ItemId == b.ItemId &&
        a.Iv.ValueEquals(b.Iv) &&
        a.Lv == b.Lv &&
        a.Moves.ToArray().ArrayEquals(b.Moves.ToArray()) &&
        a.Nature == b.Nature;
    }
  }
}
