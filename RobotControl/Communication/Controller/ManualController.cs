using RobotControl.Command;
using System;
using System.Collections.Generic;

namespace RobotControl.Communication.Controller
{
  public class ManualController : ChannelBase<GamePad, IControllerCommand, ControllerConfiguration>
  {
    public ManualController(ControllerConfiguration configuration) : base(configuration)
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
      var gamePad = new GamePad();
      gamePad.StateChanged += GamePad_StateChanged;
      return gamePad;
    }

    private void GamePad_StateChanged(object sender, IGamePadStateChangedEventArgs e)
    {
      var cmd = new List<ICommand>();
      cmd.Add(new ControllerCommand { X = e.LeftThumb.X, Y = e.LeftThumb.Y, RX = e.RightThumb.X });
      CommandManager.Instance.DataReceived(this, cmd);
    }
  }
}
