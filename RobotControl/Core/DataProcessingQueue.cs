using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RobotControl.Core
{
  public class DataProcessingQueue<T> : DisposableBase
    where T : class
  {
    private readonly object lockData = new object();
    private readonly CancellationTokenSource tokenSource;
    private readonly CancellationToken token;
    private readonly Task worker;
    private readonly ManualResetEvent signal;
    private readonly Queue<T> data = new Queue<T>();
    private readonly Action<T> action;
    private readonly Action<IEnumerable<T>> listAction;
    private readonly int minInterval;
    private DateTime nextAction = DateTime.Now;

    private enum ProcessDataMode
    {
      Single,
      All,
    }

    private DataProcessingQueue()
    {
      tokenSource = new CancellationTokenSource();
      token = tokenSource.Token;
      signal = new ManualResetEvent(true);
    }

    public DataProcessingQueue(Action<T> action) : this()
    {
      this.action = action;
      worker = Task.Run(() => ProcessData(), token);
    }

    public DataProcessingQueue(Action<IEnumerable<T>> action, int minInterval = 0) : this()
    {
      listAction = action;
      this.minInterval = minInterval;
      worker = Task.Run(() => ProcessData(), token);
    }

    public void Enqueue(T item)
    {
      lock (lockData)
      {
        if (item != null)
        {
          data.Enqueue(item);
          signal.Set();
        }
      }
    }

    protected override void Dispose(bool disposing)
    {
      tokenSource?.Cancel();
      Signal();
      worker?.Wait();
      worker?.Dispose();
      signal?.Dispose();
      tokenSource?.Dispose();
    }

    private ProcessDataMode Mode => listAction != null ? ProcessDataMode.All : ProcessDataMode.Single;

    private void Signal()
    {
      lock (lockData)
      {
        signal?.Set();
      }
    }

    private void ProcessData()
    {
      var timeOut = Timeout.Infinite;
      T item = null;
      var items = new Queue<T>();
      if (token.IsCancellationRequested)
      {
        return;
      }

      while (true)
      {
        signal.WaitOne(timeOut);
        var now = DateTime.Now;
        if (minInterval > 0 && now < nextAction)
        {
          lock (lockData)
          {
            timeOut = (int)(nextAction - now).TotalMilliseconds;
            timeOut = timeOut <= 0 ? 1 : timeOut;
            timeOut = timeOut > minInterval ? minInterval : timeOut;
            signal.Reset();
            continue;
          }
        }
                     
        items.Clear();
        do
        {
          lock (lockData)
          {
            do
            {
              item = data.Any() ? data.Dequeue() : null;
              if (Mode == ProcessDataMode.All && item != null)
              {
                items.Enqueue(item);
              }
            }
            while (Mode == ProcessDataMode.All && item != null);

            if (item == null)
            {
              signal.Reset();
            }
          }

          if (Mode == ProcessDataMode.All && items.Any())
          {
            listAction(items);
          }
          else if (item != null && action != null)
          {
            action(item);
          }

          if (token.IsCancellationRequested)
          {
            return;
          }
        }
        while (item != null);

        if (minInterval > 0)
        {
          nextAction = DateTime.Now.AddMilliseconds(minInterval);
        }
      }
    }
  }
}