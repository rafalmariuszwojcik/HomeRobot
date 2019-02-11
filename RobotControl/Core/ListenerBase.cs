using System;
using System.Collections.Generic;
using RobotControl.Communication;

namespace RobotControl.Core
{
  public abstract class ListenerBase<T> : DisposableBase, IListener<T>
    where T : class
  {
    private readonly Action<IEnumerable<T>> action;

    public ListenerBase(Action<IEnumerable<T>> action)
    {
      this.action = action;
    }

    public void DataReceived(IChannel channel, IEnumerable<T> data)
    {
      action?.Invoke(data);
    }
  }
}
