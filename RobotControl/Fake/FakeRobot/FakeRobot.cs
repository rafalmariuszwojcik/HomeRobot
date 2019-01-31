using RobotControl.Core;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace RobotControl.Fake.FakeRobot
{
  public class FakeRobot : DisposableBase
  {
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

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (cts != null)
        {
          cts.Dispose();
          cts = null;
        }
      }
    }

    private void Loop(CancellationToken token)
    {
      while (!token.IsCancellationRequested)
      {
        //Task.Delay(10);
      }
    }
  }
}