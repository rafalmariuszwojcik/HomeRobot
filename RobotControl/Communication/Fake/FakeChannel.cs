using RobotControl.Command;
using RobotControl.Command.Robot;
using RobotControl.Core;
using RobotControl.Fake.FakeRobot;
using System.Collections.Generic;

namespace RobotControl.Communication.Fake
{
  /// <summary>
  /// Communication channel with <see cref="FakeRobot"/>, used for behaviours simulation without real robot body.
  /// </summary>
  /// <seealso cref="RobotControl.Communication.ChannelBase{RobotControl.Fake.FakeRobot.FakeRobot, RobotControl.Command.ICommand, RobotControl.Communication.Fake.FakeConfiguration}" />
  public class FakeChannel : ChannelBase<FakeRobot, ICommand, FakeConfiguration>
  {
    /// <summary>
    /// The command listener.
    /// </summary>
    /// <remarks>Process incoming commands.</remarks>
    private readonly CommandListener commandListener;

    /// <summary>
    /// Initializes a new instance of the <see cref="FakeChannel"/> class.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    public FakeChannel(FakeConfiguration configuration) : base(configuration)
    {
      commandListener = new CommandListener(x => ProcessCommands(x));
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
    /// Internal open communication chanel method.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <returns>
    /// Reference to communication object.
    /// </returns>
    protected override FakeRobot InternalOpen(FakeConfiguration configuration)
    {
      var robot = new FakeRobot();
      robot.OnRobotState += OnRobotState;
      return robot;
    }

    /// <summary>
    /// Internal close communication channel.
    /// </summary>
    /// <param name="channel">The channel.</param>
    protected override void InternalClose(FakeRobot channel)
    {
    }

    /// <summary>
    /// Called when robot state changed.
    /// </summary>
    /// <param name="sender">The sender object.</param>
    /// <param name="e">Fake robot current state.</param>
    private void OnRobotState(object sender, FakeRobotState e)
    {
      var commands = new List<ICommand>() { new RobotMoveCommand { LeftDistance = e.LeftEngineState.Distance, RightDistance = e.RightEngineState.Distance } };
      commands.Add(new EngineSpeedCommand() { Index = 0, Speed = e.LeftEngineState.Speed, AvgSpeed = e.LeftEngineState.Speed });
      commands.Add(new EngineSpeedCommand() { Index = 1, Speed = e.RightEngineState.Speed, AvgSpeed = e.RightEngineState.Speed });
      CommandManager.Instance.BroadcastData(this, commands);
    }

    /// <summary>
    /// Internal send data through the channel.
    /// </summary>
    /// <param name="channel">The channel.</param>
    /// <param name="data"></param>
    protected override void InternalSend(FakeRobot channel, ICommand data)
    {
      if (data is IRobotEnginesPowerCommand enginesPowerCommand) 
      {
        channel.LeftEngine.Power = enginesPowerCommand.LeftEnginePower;
        channel.RightEngine.Power = enginesPowerCommand.RightEnginePower;
      }
    }

    /// <summary>
    /// Processes the commands.
    /// </summary>
    /// <remarks>Execute incoming commands on <see cref=""/>FakeRobot</see> instance.</remarks>
    /// <param name="commands">The commands.</param>
    private void ProcessCommands(IEnumerable<ICommand> commands)
    {
      Send(commands);
    }
  }
}