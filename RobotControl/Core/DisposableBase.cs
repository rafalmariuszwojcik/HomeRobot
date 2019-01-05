using System;

namespace RobotControl.Core
{
  public abstract class DisposableBase : IDisposable
  {
    private bool disposed = false;

    public DisposableBase()
    {
    }

    ~DisposableBase()
    {
      InternalDispose(false);
    }

    public void Dispose()
    {
      InternalDispose(true);
      GC.SuppressFinalize(this);
    }

    protected abstract void Dispose(bool disposing);

    private void InternalDispose(bool disposing)
    {
      if (disposed)
      {
        return;
      }

      if (disposing)
      {
        Dispose(disposing);
      }

      disposed = true;
    }
  }
}