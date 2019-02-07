using System;
using RobotControl.Communication;

namespace RobotControl.Core
{
  public abstract class ListenerBase<T> : DisposableBase, IListener<T>
    where T : class
  {
    private readonly Action<T> action;

    public ListenerBase(Action<T> action)
    {
      this.action = action;
    }

    public void MessageReceived(IChannel channel, T data)
    {
      action?.Invoke(data);
    }
  }
}
