using System;
using System.Diagnostics;

namespace RobotControl.Core
{
  public class Counter : DataProcessingQueue<object>
  {
    private readonly object lockSignal = new object();
    private readonly Stopwatch stopwatch = new Stopwatch();
    private DigitalFilter signalsPerSecond = new DigitalFilter(4);
    private double? lastElapsed;
    //private double avg_spd;
    //private int fLevel = 5;

    /// <summary>
    /// Common counter class. Used to count signals per second.
    /// </summary>
    /// <param name="timeout">Counter update frequency (delay in miliseconds).</param>
    public Counter(int timeout = 1000)
      : base(null, timeout) // update every one second.
    {
    }

    /// <summary>
    /// Signals per second has changed by background task.
    /// </summary>
    public event EventHandler OnChanged;

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
        lastElapsed = elapsed;
        signalsPerSecond.Input = elapsed >= 0.0001 ? (1000.0 * 1.0) / elapsed : 0.0;
        stopwatch.Restart();
      }
    }

    public double SignalsPerSecond
    {
      get
      {
        lock (lockSignal)
        {
          return signalsPerSecond.Output;
        }
      }
    }

    protected override void DoWork()
    {
      base.DoWork();
      //Signal();
      //OnChanged?.Invoke(this, new EventArgs());
    }

        /*
    protected override void Work()
    {
      var changed = false;
      lock (lockSignal)
      {
        //Signal();
        //if (stopwatch.IsRunning && lastElapsed.HasValue)
        //{
        //  var elapsed = stopwatch.Elapsed.TotalMilliseconds;
        //  if (elapsed > lastElapsed.Value)
        //  {
        //    signalsPerSecond = elapsed >= 0.0001 ? (1000.0 * 1.0) / elapsed : 0.0;
        //    lastElapsed = elapsed;
        //    changed = true;
        //  }
        //}
      }

      //if (changed)
      {
        OnChanged?.Invoke(this, new EventArgs());
      }
    }
    */
  }
}
