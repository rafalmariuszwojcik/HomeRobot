using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;

namespace RobotControl.Windows
{
  public interface IListenerControl<T>
    where T : class
  {
    void MessageReceived(IChannel channel, IEnumerable<T> data);
  }
}
