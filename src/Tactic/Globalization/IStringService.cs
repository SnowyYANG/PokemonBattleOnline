using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.Tactic.Globalization
{
  public interface IStringService
  {
    IDomainStringService GetDomainService(string domain);
    string Language { get; set; }
  }
}
