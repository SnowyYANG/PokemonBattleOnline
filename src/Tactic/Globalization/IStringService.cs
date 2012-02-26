using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.Tactic.Globalization
{
  public interface IStringService
  {
    // Methods
    IDomainStringService GetDomainService(string domain);

    // Properties
    IDomainStringService this[string domain] { get; }
    string Language { get; set; }
  }
}
