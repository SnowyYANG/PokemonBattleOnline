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

    public static bool ValidateLv(int value)
    {
      return value > 0 && value <= 100;
    }

    public static bool ValidateAbility(IPokemonCustomInfo pm)
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

    public static bool ValidateMoves(IPokemonCustomInfo pm)
    {
      return pm.MoveIds.Count() <= 4;
    }

    public static bool Validate(IPokemonCustomInfo pm)
    {
      return true;
      return
        pm.Form != null &&
        ValidateAbility(pm) &&
        ValidateEv(pm.Ev) &&
        ValidateLv(pm.Lv) &&
        ValidateIv(pm.Iv) &&
        ValidateMoves(pm);
    }
  }
}
