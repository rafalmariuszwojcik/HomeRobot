using RobotControl.Core;
using System;

namespace RobotControl.Messages
{
  public class MessageListener : ListenerBase<string>, IMessageListener
  {
    public MessageListener(Action<string> action) : base(action)
    {
      MessageManager.Instance.RegisterListener(this);
    }

    protected override void Dispose(bool disposing)
    {
      MessageManager.Instance.UnregisterListener(this);
    }
  }
}