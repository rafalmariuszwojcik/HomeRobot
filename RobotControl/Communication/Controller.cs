using RobotControl.Command;
using System;

namespace RobotControl.Communication
{
  public class Controller : ChannelBase<GamePad, IControllerCommand, ConfigurationBase>
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
