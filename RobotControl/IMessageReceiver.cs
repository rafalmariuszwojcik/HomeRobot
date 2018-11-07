using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotControl
{
  public interface IMessageReceiver
  {
    void MessageReceived(string message, object[] parameters);
  }
}
