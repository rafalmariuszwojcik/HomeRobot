using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RobotControl.Core
{
  public class DataProcessingQueue<T> : DisposableBase
    where T: class
  {
    private readonly object lockData = new object();
    private readonly CancellationTokenSource tokenSource;
    private readonly CancellationToken token;
    private readonly Task worker;
    private readonly ManualResetEvent signal;
    private readonly Queue<T> data = new Queue<T>();
    private readonly Action<T> action;

    public DataProcessingQueue(Action<T> action)
    {
      tokenSource = new CancellationTokenSource();
      token = tokenSource.Token;
      signal = new ManualResetEvent(true);
      this.action = action;
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

    private void Signal()
    {
      lock (lockData)
      {
        signal?.Set();
      }
    }

    private void ProcessData()
    {
      T item = null;
      if (token.IsCancellationRequested)
      {
        return;
      }

      while (true)
      {
        signal.WaitOne();
        do
        {
          lock (lockData)
          {
            item = data.Any() ? data.Dequeue() : null;
            if (item == null)
            {
              signal.Reset();
            }
          }

          if (item != null && action != null)
          {
            action(item);
          }

          if (token.IsCancellationRequested)
          {
            return;
          }
        }
        while (item != null);
      }
    }
  }
}