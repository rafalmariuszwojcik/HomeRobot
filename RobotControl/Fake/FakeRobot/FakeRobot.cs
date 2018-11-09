using System;

namespace RobotControl.Fake.FakeRobot
{
  public class FakeRobot : IDisposable
  {
    bool disposed = false;
    readonly Engine leftEngine = new Engine();
    readonly Engine rightEngine = new Engine();

    ~FakeRobot()
    {
      Dispose(false);
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (disposed)
      {
        return;
      }

      if (disposing)
      {
        ; // dispose resources here.
      }

      disposed = true;
    }
  }
}
