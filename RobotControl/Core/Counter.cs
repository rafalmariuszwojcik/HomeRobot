using System;
using System.Diagnostics;

namespace RobotControl.Core
{
  public class Counter : DataProcessingQueue<object>
  {
    private readonly object lockSignal = new object();
    private readonly Stopwatch stopwatch = new Stopwatch();
    private double signalsPerSecond;

    public Counter()
      : base(null, 100) // update 10 times per second.
    {
    }

    public void Signal()
    {
      lock (lockSignal)
      {
        if (!stopwatch.IsRunning)
        {
          stopwatch.Start();
          return;
        }

        var elapsed = stopwatch.Elapsed.TotalMilliseconds;
        signalsPerSecond = elapsed >= 0.0001 ? (1000.0 * 1.0) / elapsed : 0.0;
        stopwatch.Restart();
      }
    }

    public double SignalsPerSecond
    {
      get
      {
        return signalsPerSecond;
      }
    }

    protected override void Work()
    {
      ;
    }
  }
}
