using RobotControl.Communication;
using System.Collections.Generic;

namespace RobotControl.Core
{
  public interface IListener<T>
    where T : class
  {
    void MessageReceived(IChannel channel, IEnumerable<T> data);
  }
}
