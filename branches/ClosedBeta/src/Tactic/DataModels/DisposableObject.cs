using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline
{
  public abstract class DisposableObject : IDisposable
  {
    private object disposeLock;

    protected DisposableObject()
    {
      disposeLock = new object();
    }

    private volatile bool _isDisposed;
    protected bool IsDisposed
    { get { return _isDisposed; } }

    public void Dispose()
    {
      Dispose(true);

      GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
      if (IsDisposed) return;
      lock (disposeLock)
      {
        if (IsDisposed) return;
        _isDisposed = true;
      }
      if (disposing) DisposeManagedResources();
      DisposeUnmanagedResources();
    }

    protected virtual void DisposeManagedResources()
    { }

    protected virtual void DisposeUnmanagedResources()
    { }


    ~DisposableObject()
    {
      Dispose(false);
    }

  }
}
