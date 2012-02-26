using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace LightStudio.PokemonBattle.PBO.UIElements
{
  public class SimpleCommand : ICommand
  {
    public event EventHandler CanExecuteChanged;
    Action command;

    public SimpleCommand(Action command)
    {
      if (command == null) this.command = delegate { };
      else this.command = command;
    }

    bool ICommand.CanExecute(object parameter)
    { return true; }

    public void Execute(object parameter)
    {
      command();
    }
  }

  public class MenuCommand : ICommand
  {
    public object Header
    { get; private set; }

    public object Icon
    { get; set; }

    private bool _isEnabled;
    public bool IsEnabled
    {
      get
      {
        return _isEnabled;
      }
      set
      {
        if (_isEnabled != value)
        {
          _isEnabled = value;
          OnCanExecuteChanged();
        }
      }
    }

    private Action action;

    public MenuCommand(object header, Action commandAction)
    {
      this.Header = header;
      this.action = commandAction;
      this.IsEnabled = true;
    }

    public void Execute(object parameter)
    {
      UIDispatcher.Invoke(action);
    }

    public bool CanExecute(object parameter)
    {
      return IsEnabled;
    }

    public event EventHandler CanExecuteChanged = delegate { };
    private void OnCanExecuteChanged()
    {
      UIDispatcher.Invoke(CanExecuteChanged, this, EventArgs.Empty);
    }
  }
}
