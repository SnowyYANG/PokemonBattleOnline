using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.Tactic.DataModels
{
  public abstract class GameElementProxy<TController, TModel>
  {
    protected readonly TController Controller;
    protected readonly TModel Model;

    protected GameElementProxy(TController controller, TModel model)
    {
      Controller = controller;
      Model = model;
    }
  }
}
