using System;
using System.Collections.Generic;
using System.Linq;

namespace RobotControl.Core
{
  public class DataProcessingQueue<T> : DataProcessingQueueBase<T>
    where T : class
  {
    private readonly Action<IEnumerable<T>> action;
    private readonly int interval;
    private readonly Queue<T> items = new Queue<T>();
    private DateTime nextAction = DateTime.Now;

    public DataProcessingQueue(Action<IEnumerable<T>> action, int interval = 0) 
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
        action?.Invoke(new[] { item });
      }
      else
      {
        items.Enqueue(item);
      }
    }

    protected override int GetTimeout()
    {
      if (interval > 0)
      {
        var timeOut = (int)(nextAction - DateTime.Now).TotalMilliseconds;
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
        if (DateTime.Now >= nextAction && items.Any())
        {
          action?.Invoke(items);
          items.Clear();
          nextAction = DateTime.Now.AddMilliseconds(interval);
        }
      }
    }
  }
}