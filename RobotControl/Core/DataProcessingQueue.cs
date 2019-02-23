using System;
using System.Collections.Generic;
using System.Linq;

namespace RobotControl.Core
{
  public class DataProcessingQueue<T> : DataProcessingQueueBase<T>
    where T : class
  {
    private readonly Action<object, IEnumerable<T>> action;
    private readonly int interval;
    private readonly Queue<T> items = new Queue<T>();
    private readonly object nextActionLock = new object();
    private DateTime nextAction = DateTime.Now;

    public DataProcessingQueue(Action<object, IEnumerable<T>> action, int interval = 0) 
      : base()
    {
      this.action = action;
      this.interval = interval;
      Start();
    }

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
          AddDelay();
        }

        items.Enqueue(item);
      }
    }

    protected override int GetTimeout()
    {
      if (interval > 0)
      {
        var timeOut = GetNextTimeout();
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
        if (CanDoWork())
        {
          if (items.Any())
          {
            action?.Invoke(this, items);
            items.Clear();
          }

          AddDelay();
        }
      }
    }

    private void AddDelay()
    {
      lock (nextActionLock)
      {
        nextAction = DateTime.Now.AddMilliseconds(interval);
      }
    }

    private bool CanDoWork()
    {
      lock (nextActionLock)
      {
        return DateTime.Now >= nextAction;
      }
    }

    private int GetNextTimeout()
    {
      lock (nextActionLock)
      {
        return (int)(nextAction - DateTime.Now).TotalMilliseconds;
      }
    }
  }
}