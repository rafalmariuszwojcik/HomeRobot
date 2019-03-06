using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RobotControl.Core
{
  public class Counter : DataProcessingQueue<object>
  {
    private readonly Stopwatch stopwatch = new Stopwatch();
    private DateTime? lastSignal;
    private readonly object signalsPerSecondLock = new object();
    private double signalsPerSecond;

    public Counter()
      : base((s, d) => ((Counter)s).Update(d), 100) // update 10 times per second.
    {
    }

    public void Signal()
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

    public double SignalsPerSecond
    {
      get
      {
        lock (signalsPerSecondLock)
        {
          return signalsPerSecond;
        }
      }
    }

    private void Update(IEnumerable<object> data)
    {
      var now = DateTime.Now;
      if (lastSignal.HasValue)
      {
        TimeSpan span = now - lastSignal.Value;
        var ms = span.TotalMilliseconds;
        lock (signalsPerSecondLock)
        {
          signalsPerSecond = ms >= 0.01 ? (1000.0 * data.Count()) / ms : 0.0;
        }
      }

      lastSignal = now;
    }
  }
}
