using RobotControl.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotControl.Communication
{
  public class Controller : ChannelBaseEx<GamePad, IControllerCommand, ConfigurationBase>
  {
    public Controller(ConfigurationBase configuration) : base(configuration)
    {
    }

    protected override void InternalSend(GamePad channel, IControllerCommand data)
    {
      throw new NotImplementedException();
    }

    protected override void InternalClose(GamePad channel)
    {
      throw new NotImplementedException();
    }

    protected override GamePad InternalOpen(ConfigurationBase configuration)
    {
      throw new NotImplementedException();
    }
  }
}
