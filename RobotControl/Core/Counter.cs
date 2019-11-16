using System;

namespace RobotControl.Core
{
  public class Counter : DataProcessingQueue<object>
  {
    const int UPDATE_FREQUENCY = 1000; // update every 1 sec.

    private readonly object lockSignal = new object();
    private int signals;
    private double signalsPerSecond;

    /// <summary>
    /// Common counter class. Used to count signals per second.
    /// </summary>
    /// <param name="timeout">Counter update frequency (delay in miliseconds).</param>
    public Counter()
      : base(null, UPDATE_FREQUENCY)
    {
    }

    /// <summary>
    /// Signals per second has changed by background task.
    /// </summary>
    public event EventHandler OnChanged;

    public double SignalsPerSecond
    {
      get
      {
        lock (lockSignal)
        {
          return signalsPerSecond;
        }
      }
    }

    public void Signal()
    {
      lock (lockSignal)
      {
        signals++;
      }
    }

    protected override void DoWork()
    {
      base.DoWork();
      Calculate(ElapsedMilliseconds);
    }

    private void Calculate(double elapsed)
    {
      var zero = false;
      lock (lockSignal)
      {
        var sps = signals / (elapsed / 1000.0);
        zero = sps <= 0.1 && signalsPerSecond > 0.1;
        signalsPerSecond = sps;
        signals = zero ? -1 : 0;
      }

      if (zero)
      {
        OnChanged?.Invoke(this, new EventArgs());
      }
    }
  }
}