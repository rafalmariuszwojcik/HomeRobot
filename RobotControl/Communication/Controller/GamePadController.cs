using RobotControl.Command;
using RobotControl.Command.Controller;
using RobotControl.Command.Robot;
using RobotControl.Core;
using System;
using System.Collections.Generic;

namespace RobotControl.Communication.Controller
{
  /// <summary>
  /// Manual controller communication channel. Supports robot controling by GamePad device.
  /// </summary>
  /// <seealso cref="RobotControl.Communication.ChannelBase{RobotControl.Communication.Controller.GamePad, RobotControl.Command.Controller.IControllerCommand, RobotControl.Communication.Controller.ControllerConfiguration}" />
  public class GamePadController : ChannelBase<GamePad, IControllerCommand, ControllerConfiguration>
  {
    /// <summary>
    /// The command listener.
    /// </summary>
    /// <remarks>Process incoming commands.</remarks>
    private readonly CommandListener commandListener;

    /// <summary>
    /// Initializes a new instance of the <see cref="GamePadController"/> class.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    public GamePadController(ControllerConfiguration configuration) : base(configuration)
    {
      commandListener = new CommandListener(x => ProcessIncomingCommands(x));
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      if (disposing)
      {
        DisposeHelper.Dispose(commandListener);
      }
    }

    /// <summary>
    /// Internal send data through the channel.
    /// </summary>
    /// <param name="channel">The channel.</param>
    /// <param name="data"></param>
    protected override void InternalSend(GamePad channel, IControllerCommand data)
    {
      if (data is IControllerVibrationCommand controllerVibrationCommand)
      {
        channel.SetVibration(
          controllerVibrationCommand.EngineId == ControllerVibrationEngineId.Left ? controllerVibrationCommand.MotorSpeed : (int?)null,
          controllerVibrationCommand.EngineId == ControllerVibrationEngineId.Right ? controllerVibrationCommand.MotorSpeed : (int?)null
        );
      }
    }

    /// <summary>
    /// Internal close communication channel.
    /// </summary>
    /// <param name="channel">The channel.</param>
    protected override void InternalClose(GamePad channel)
    {
      ;
    }

    /// <summary>
    /// Internal open communication channel method.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <returns>
    /// Reference to communication object.
    /// </returns>
    protected override GamePad InternalOpen(ControllerConfiguration configuration)
    {
      var gamePad = new GamePad();
      gamePad.StateChanged += GamePad_StateChanged;
      return gamePad;
    }

    /// <summary>
    /// Handles the StateChanged event of the GamePad control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="IGamePadStateChangedEventArgs"/> instance containing the event data.</param>
    private void GamePad_StateChanged(object sender, IGamePadStateChangedEventArgs e)
    {
      var cmd = new List<ICommand>
      {
        new ControllerStateCommand(e.LeftThumb, e.RightThumb, e.LeftTrigger, e.RightTrigger)
      };

      CommandManager.Instance.BroadcastData(this, cmd);
    }

    /// <summary>
    /// Processes the incoming commands.
    /// </summary>
    /// <param name="commands">The commands.</param>
    private void ProcessIncomingCommands(IEnumerable<ICommand> commands)
    {
      foreach (var command in commands)
      {
        if (command is IRobotEngineStateCommand robotEngineStateCommand)
        {
          Send(new ControllerVibrationCommand(
            robotEngineStateCommand.EngineId == EngineId.Left ? ControllerVibrationEngineId.Left : ControllerVibrationEngineId.Right,
            Math.Abs(Convert.ToInt32(Math.Round(((double)robotEngineStateCommand.Speed / (double)robotEngineStateCommand.MaxSpeed) * 100.0))))
          );
        }
      }
    }
  }
}