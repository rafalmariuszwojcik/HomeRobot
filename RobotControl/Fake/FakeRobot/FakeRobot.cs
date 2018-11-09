using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace RobotControl.Fake.FakeRobot
{
  public class FakeRobot : IDisposable
  {
    private bool disposed = false;
    private readonly Engine leftEngine = new Engine();
    private readonly Engine rightEngine = new Engine();
    private readonly Task task;
    private readonly ConcurrentQueue<int> inputQueue = new ConcurrentQueue<int>();
    private readonly ConcurrentQueue<int> outputQueue = new ConcurrentQueue<int>();
    private CancellationTokenSource cts = new CancellationTokenSource();

    public FakeRobot()
    {
      task = new Task(() => Loop(cts.Token));
    }

    ~FakeRobot()
    {
      Dispose(false);
    }

    public void Start()
    {
      task.Start();
    }

    public void Stop()
    {
      if (cts != null)
      {
        cts.Cancel();
        task.Wait();
      }
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
        if (cts != null)
        {
          cts.Dispose();
          cts = null;
        }
      }

      disposed = true;
    }

    private void Loop(CancellationToken token)
    {
      while (!token.IsCancellationRequested)
      {
        Task.Delay(10);
      }
    }
  }
}