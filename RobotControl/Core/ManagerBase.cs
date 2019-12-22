using RobotControl.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RobotControl.Core
{
  public abstract class ManagerBase<S, T, M> : Singleton<S>, IManager<T, M>
    where S : Singleton<S>
    where T : class
    where M : class
  {
    private readonly object lockObject = new object();
    private readonly IList<T> listeners = new List<T>();
    private readonly DataProcessingQueue<M> messageQueue;

    protected ManagerBase(int interval = 0)
    {
      messageQueue = new DataProcessingQueue<M>((s, x) => PostData(null, x), interval);
    }

    protected IList<IDisposable> Disposables { get; } = new List<IDisposable>();

    public void BroadcastData(object sender, IEnumerable<M> data)
    {
      messageQueue.Enqueue(data);
    }

    public void BroadcastData(object sender, M data)
    {
      messageQueue.Enqueue(data);
    }

    public void RegisterListener(T listener)
    {
      lock (lockObject)
      {
        if (listener != null && !listeners.Contains(listener))
        {
          listeners.Add(listener);
        }
      }
    }

    public void UnregisterListener(T listener)
    {
      lock (lockObject)
      {
        if (listeners.Contains(listener))
        {
          listeners.Remove(listener);
        }
      }
    }

    protected override void TearDown()
    {
      foreach (var disposable in Disposables)
      {
        disposable.Dispose();
      }

      messageQueue?.Dispose();
      base.TearDown();
    }

    protected virtual void SendData(T listener, Action action)
    {
      action?.Invoke();
    }

    protected virtual IEnumerable<Action> SendDataActions(T listener, IEnumerable<M> data)
    {
      var result = new List<Action>();
      if (listener is IListener<M> listenerInterface)
      {
        result.Add(new Action(() => listenerInterface.DataReceived(null, data)));
      }

      return result;
    }

    private void PostData(IChannel channel, IEnumerable<M> data)
    {
      List<T> listenersToProcess;
      lock (lockObject)
      {
        listenersToProcess = new List<T>(listeners);
      }

      Parallel.ForEach(listenersToProcess, x => ProcessListener(x, data));
    }

    private void ProcessListener(T listener, IEnumerable<M> data)
    {
      if (listener == null || data == null || !data.Any())
      {
        return;
      }

      var actions = SendDataActions(listener, data);
      Parallel.ForEach(actions, x => SendData(listener, x));
    }
  }
}