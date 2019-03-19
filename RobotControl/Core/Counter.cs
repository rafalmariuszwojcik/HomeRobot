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
    private bool disableSignal;
    private bool zero;

    /// <summary>
    /// Common counter class. Used to count signals per second.
    /// </summary>
    /// <param name="timeout">Counter update frequency (delay in miliseconds).</param>
    public Counter(int timeout = 100)
      : base(null, timeout) // update every 0.1 sec.
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
        if (disableSignal)
        {
          return;
        }

        if (!stopwatch.IsRunning)
        {
          stopwatch.Start();
          return;
        }

        var elapsed = stopwatch.Elapsed.TotalMilliseconds;
        stopwatch.Restart();
        Calculate(elapsed);
      }
    }

    public double SignalsPerSecond
    {
      get
      {
        lock (lockSignal)
        {
          zero = signalsPerSecond.Output <= 0.0;
          return signalsPerSecond.Output;
        }
      }
    }

    protected override void DoWork()
    {
      base.DoWork();
      bool down;
      double elapsed;
      lock (lockSignal)
      {
        elapsed = stopwatch.Elapsed.TotalMilliseconds;
        down = lastElapsed.HasValue && elapsed > lastElapsed * 1.2;
      }
        
      if (down)
      {
        bool update;
        lock (lockSignal)
        {
          disableSignal = true;
          Calculate(elapsed);
          update = !zero;
        }
        try
        {
          if (update)
          {
            OnChanged?.Invoke(this, new EventArgs());
          }
        }
        finally
        {
          lock (lockSignal)
          {
            disableSignal = false;
          }
        }
      }
    }

    private void Calculate(double elapsed)
    {
      lock (lockSignal)
      {
        signalsPerSecond.Input = elapsed >= 0.0001 ? (1000.0 * 1.0) / elapsed : 0.0;
        lastElapsed = elapsed;
      }
    }
  }
}