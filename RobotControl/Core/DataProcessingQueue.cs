using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RobotControl.Core
{
  public class DataProcessingQueue<T> : DataProcessingQueueBase<T>
    where T : class
  {
    private readonly Action<object, IEnumerable<T>> action;
    private readonly int interval;
    private readonly Queue<T> items = new Queue<T>();
    private readonly Stopwatch stopwatch = new Stopwatch();

    public DataProcessingQueue(Action<object, IEnumerable<T>> action, int interval = 0)
      : base()
    {
      this.action = action;
      this.interval = interval;
      stopwatch.Start();
      Start();
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      if (disposing)
      {
        stopwatch.Stop();
      }
    }

    protected long ElapsedMilliseconds { get; private set; }

    protected override void ProcessItem(T item)
    {
      if (interval <= 0)
      {
        action?.Invoke(this, new[] { item });
      }
      else
      {
        if (!items.Any())
        {
          stopwatch.Restart();
        }

        items.Enqueue(item);
      }
    }

    protected override int GetTimeout()
    {
      if (interval > 0)
      {
        var timeOut = interval - (int)stopwatch.ElapsedMilliseconds;
        timeOut = timeOut <= 0 ? 1 : timeOut;
        timeOut = timeOut > interval ? interval : timeOut;
        return timeOut;
      }
      else
      {
        return base.GetTimeout();
      }
    }

    protected override void Work()
    {
      if (interval > 0)
      {
        if (stopwatch.ElapsedMilliseconds >= interval)
        {
          // Restart must be called before DoWork, to have more precise execution frequency.
          ElapsedMilliseconds = stopwatch.ElapsedMilliseconds;
          stopwatch.Restart();
          DoWork();
        }
      }
    }

    protected virtual void DoWork()
    {
      if (items.Any())
      {
        action?.Invoke(this, items);
        items.Clear();
      }
    }
  }
}