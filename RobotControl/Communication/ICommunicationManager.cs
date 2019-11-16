using System.Collections.Generic;

namespace RobotControl.Communication
{
  public interface ICommunicationManager
  {
    IEnumerable<IChannel> Items { get; }
    void Add(IChannel channel);
    void Remove(IChannel channel);
    void Save();
  }
}
