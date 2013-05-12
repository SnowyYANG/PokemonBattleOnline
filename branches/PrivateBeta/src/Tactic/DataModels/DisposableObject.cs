using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio
{
  public abstract class DisposableObject : IDisposable
  {
    private object disposeLock;

    protected DisposableObject()
    {
      disposeLock = new object();
    }

    protected bool IdDisposed { get; private set; }

    public void Dispose()
    {
      Dispose(true);

      GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
      if (IdDisposed)
        return;
      lock (disposeLock)
      {
        if (IdDisposed)
          return;
        IdDisposed = true;
      }

      if (disposing)
      {
        DisposeManagedResources();
      }
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
