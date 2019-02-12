using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RobotControl.Core
{
  public abstract class DataProcessingQueueBase<T> : DisposableBase 
    where T : class
  {
    private readonly object lockData = new object();
    private readonly CancellationTokenSource tokenSource;
    private readonly CancellationToken token;
    private readonly ManualResetEvent signal;
    private readonly Queue<T> data = new Queue<T>();
    private Task worker;
    private bool isDisposing = false;

    public DataProcessingQueueBase()
    {
      tokenSource = new CancellationTokenSource();
      token = tokenSource.Token;
      signal = new ManualResetEvent(true);
    }

    public void Enqueue(IEnumerable<T> items)
    {
      lock (lockData)
      {
        if (items != null && items.Any() && !isDisposing)
        {
          foreach (var item in items)
          {
            data.Enqueue(item);
          }
          
          signal.Set();
        }
      }
    }

    protected void Start()
    {
      worker = Task.Factory.StartNew(() => ProcessData(), token);
    }

    protected override void Dispose(bool disposing)
    {
      lock (lockData)
      {
        isDisposing = true;
      }

      tokenSource?.Cancel();
      Signal();
      worker?.Wait();
      worker?.Dispose();
      signal?.Dispose();
      tokenSource?.Dispose();
    }

    protected virtual int GetTimeout()
    {
      return Timeout.Infinite;
    }

    protected abstract void ProcessItem(T item);

    protected virtual void Work()
    {
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
        signal.WaitOne(GetTimeout());
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

          try
          {
            if (item != null)
            {
              ProcessItem(item);
            }

            Work();
          }
          catch (Exception)
          {
            ;
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