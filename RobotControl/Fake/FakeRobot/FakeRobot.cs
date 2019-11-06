using RobotControl.Core;
using System.Threading;

namespace RobotControl.Fake.FakeRobot
{
  /// <summary>
  /// The fake robot object to simulate behaviours.
  /// </summary>
  /// <remarks>Double wheel robot simulation.</remarks>
  public class FakeRobot : DisposableBase
  {
    /// <summary>
    /// The left engine.
    /// </summary>
    private readonly Engine leftEngine = new Engine();

    /// <summary>
    /// The right engine.
    /// </summary>
    private readonly Engine rightEngine = new Engine();

    /// <summary>
    /// The signal event.
    /// </summary>
    /// <remarks>Used to control (terminate) worker thread.</remarks>
    private readonly AutoResetEvent signal = new AutoResetEvent(false);

    /// <summary>
    /// The worker thread.
    /// </summary>
    /// <remarks>Executes robot's behaviours.</remarks>
    private readonly Thread worker;

    /// <summary>
    /// Initializes a new instance of the <see cref="FakeRobot"/> class.
    /// </summary>
    public FakeRobot()
    {
      worker = new Thread(new ParameterizedThreadStart(myMethod));
      Start();
    }

    private void myMethod(object sync) 
    {
      Thread.Sleep(100);
    }

    /// <summary>
    /// Starts fake robot instance.
    /// </summary>
    /// <remarks>Start executing simulated behaviours.</remarks>
    public void Start()
    {
      if (!worker.IsAlive) 
      {
        worker.Start(null);
      } 
    }

    public void Stop()
    {
      worker.Join();
    /*  
    if (cts != null)
      {
        cts.Cancel();
        task.Wait();
      }*/
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        Stop();
        DisposeHelper.Dispose(signal); 
      }
    }

    /// <summary>
    /// Fake robot main thread loop.
    /// </summary>
    /// <param name="token">The cancelation token.</param>
    private void Loop(CancellationToken token)
    {
    /*  
    var waitHandles = new[] { leftEngine.SignalEvent, rightEngine.SignalEvent };
      while (true) 
      {
        var index = WaitHandle.WaitAny(waitHandles, 100);
        if (index != WaitHandle.WaitTimeout) 
        {
          switch (index) 
          {
            case 0:
              break;

            case 1:
              break;
          }
        }

        if (token.IsCancellationRequested)
        {
          return;
        }
      }*/
    }
  }
}