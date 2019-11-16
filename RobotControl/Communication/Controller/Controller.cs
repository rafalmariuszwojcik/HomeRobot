using RobotControl.Command;
using System;

namespace RobotControl.Communication.Controller
{
  public class Controller : ChannelBase<GamePad, IControllerCommand, ControllerConfiguration>
  {
    public Controller(ControllerConfiguration configuration) : base(configuration)
    {
    }

    protected override void InternalSend(GamePad channel, IControllerCommand data)
    {
      ;
    }

    protected override void InternalClose(GamePad channel)
    {
      ;
    }

    protected override GamePad InternalOpen(ControllerConfiguration configuration)
    {
      return new GamePad();
    }
  }
}
