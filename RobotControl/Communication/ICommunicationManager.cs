using System.Collections.Generic;

namespace RobotControl.Communication
{
  public interface ICommunicationManager
  {
    IEnumerable<IChannel> Items { get; }
  }
}
