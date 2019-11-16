using System.Collections.Generic;

namespace RobotControl.Communication
{
  public interface ICommunicationManager
  {
    IEnumerable<IChanellExBase> Items { get; }
    void Add(IChanellExBase channel);
    void Remove(IChanellExBase channel);
    void Save();
  }
}
