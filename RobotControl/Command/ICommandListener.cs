using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotControl.Command
{
  public interface ICommandListener
  {
    void CommandReceived(object sender, ICommand command);
  }
}
