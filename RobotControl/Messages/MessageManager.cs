using RobotControl.Core;
using System.Collections.Generic;

namespace RobotControl.Messages
{
  public class MessageManager : Singleton<MessageManager>, IMessageManager
  {
    private readonly IList<IMessageListener> listeners = new List<IMessageListener>();

    public void RegisterListener(IMessageListener listener)
    {
      if (listener != null && !listeners.Contains(listener))
      {
        listeners.Add(listener);
      }
    }

    public void UnregisterListener(IMessageListener listener)
    {
      if (listeners.Contains(listener))
      {
        listeners.Remove(listener);
      }
    }

    public void MessageReceived(string message)
    {
      foreach (var listener in listeners)
      {
        listener.MessageReceived(message);
      }
    }
  }
}