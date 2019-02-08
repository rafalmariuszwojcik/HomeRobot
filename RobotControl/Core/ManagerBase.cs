using RobotControl.Communication;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RobotControl.Core
{
  public abstract class ManagerBase<T, M> : Singleton<ManagerBase<T, M>>
    where T: IListener<M> 
    where M: class
  {
    private readonly object lockObject = new object();
    private readonly IList<T> listeners = new List<T>();
    private readonly DataProcessingQueue<M> messageQueue;

    public ManagerBase()
    {
      messageQueue = new DataProcessingQueue<M>(x => PostMessage(null, x));
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

    public void MessageReceived(IChannel channel, M data)
    {
      messageQueue.Enqueue(data);
    }

    protected override void TearDown()
    {
      messageQueue?.Dispose();
      base.TearDown();
    }

    private void PostMessage(IChannel channel, IEnumerable<M> data)
    {
      foreach (var dataItem in data)
      {
        lock (lockObject)
        {
          Parallel.ForEach(listeners, (listener) => {
            listener.MessageReceived(channel, dataItem);
          });
        }
      }
    }
  }
}