using System.Collections.Generic;
using RobotControl.Core;

namespace RobotControl.Communication
{
  public class CommunicationManager : Singleton<CommunicationManager>, ICommunicationManager
  {
    private readonly IList<IChannel> items = new List<IChannel>();
    public IEnumerable<IChannel> Items => items;
        
  }
}