using RobotControl.Communication;
using System.Collections.Generic;

namespace RobotControl.Core
{
  public abstract class ManagerBase<T, M> : Singleton<ManagerBase<T, M>>
    where T: IListener 
    where M: class
  {
    private readonly object lockObject = new object();
    private readonly IList<T> listeners = new List<T>();

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

    public abstract void MessageReceived(IChannel channel, M data);
  }
}