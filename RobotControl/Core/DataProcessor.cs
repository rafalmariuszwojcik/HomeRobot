using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RobotControl.Core
{
  public class DataProcessor<T> : DisposableBase
    where T: class
  {
    private readonly object lockData = new object();
    private readonly CancellationTokenSource tokenSource;
    private readonly CancellationToken token;
    private readonly Task worker;
    private readonly ManualResetEvent signal;
    private readonly Queue<T> data = new Queue<T>();
    private readonly Action<T> action;

    public DataProcessor(Action<T> action)
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
        data.Enqueue(item);
        signal.Set();
      }
    }

    protected override void Dispose(bool disposing)
    {
      tokenSource?.Cancel();
      signal?.Set();
      worker?.Wait();
      worker?.Dispose();
      signal?.Dispose();
      tokenSource?.Dispose();
    }

    private void ProcessData()
    {
      T item = null;
      token.ThrowIfCancellationRequested();
      while (true)
      {
        signal.WaitOne();
        do
        {
          lock (lockData)
          {
            item = data.Any() ? data.Dequeue() : null;
          }

          if (item != null)
          {
            action(item);
          }

          if (token.IsCancellationRequested)
          {
            token.ThrowIfCancellationRequested();
          }
        }
        while (item != null);

        lock (lockData)
        {
          if (!data.Any())
          {
            signal.Reset();
          }
        }
      }
    }
  }
}