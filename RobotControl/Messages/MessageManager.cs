using RobotControl.Communication;
using RobotControl.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RobotControl.Messages
{
  public class MessageManager : Singleton<MessageManager>, IMessageManager
  {
    private readonly object lockObject = new object();
    private readonly IList<IMessageListener> listeners = new List<IMessageListener>();
    private readonly DataProcessingQueue<string> messageQueue;

    public MessageManager()
    {
      messageQueue = new DataProcessingQueue<string>(x => PostMessage(null, x));
    }

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
      messageQueue.Enqueue(message);
    }
    
    protected override void TearDown()
    {
      messageQueue?.Dispose();
      base.TearDown();
    }

    private void PostMessage(IChannel channel, string message)
    {
      lock (lockObject)
      {
        Parallel.ForEach(listeners, (listener) => {
          listener.MessageReceived(channel, message);
        });
      }
    }
  }
}