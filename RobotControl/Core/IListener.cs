using RobotControl.Communication;
using System.Collections.Generic;

namespace RobotControl.Core
{
  public interface IListener<T>
    where T : class
  {
    void DataReceived(IChannel channel, IEnumerable<T> data);
  }
}
