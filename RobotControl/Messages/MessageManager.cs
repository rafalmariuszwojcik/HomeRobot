using RobotControl.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RobotControl.Messages
{
  public class MessageManager : Singleton<MessageManager>, IMessageManager
  {
    private readonly object lockObject = new object();
    private readonly IList<IMessageListener> listeners = new List<IMessageListener>();

    public void RegisterListener(IMessageListener listener)
    {
      lock (lockObject)
      {
        if (listener != null && !listeners.Contains(listener))
        {
          listeners.Add(listener);
        }
      }
    }

    public void UnregisterListener(IMessageListener listener)
    {
      lock (lockObject)
      {
        if (listeners.Contains(listener))
        {
          listeners.Remove(listener);
        }
      }
    }

    public void MessageReceived(object sender, string message)
    {
      lock (lockObject)
      {
        Parallel.ForEach(listeners, (listener) => {
          listener.MessageReceived(sender, message);
        });
      }
    }
  }
}