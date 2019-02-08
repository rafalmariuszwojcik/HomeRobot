using RobotControl.Core;
using System;
using System.Collections.Generic;

namespace RobotControl.Messages
{
  public class MessageListener : ListenerBase<string>, IMessageListener
  {
    public MessageListener(Action<IEnumerable<string>> action) : base(action)
    {
      MessageManager.Instance.RegisterListener(this);
    }

    protected override void Dispose(bool disposing)
    {
      MessageManager.Instance.UnregisterListener(this);
    }
  }
}