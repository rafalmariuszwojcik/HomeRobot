using RobotControl.Command;
using RobotControl.Fake.FakeRobot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RobotControl.Communication
{
  public class Fake : ChannelBase<FakeRobot, FakeConfiguration>
  {
    public Fake(FakeConfiguration configuration) : base(configuration)
    {
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

    protected override void InternalClose(FakeRobot channel)
    {
    }

    public override void InternalSend(FakeRobot channel, ICommand[] commands)
    {
      throw new NotImplementedException();
    }

    public override void InternalSend(FakeRobot channel, string data)
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Called when robot state changed.
    /// </summary>
    /// <param name="sender">The sender object.</param>
    /// <param name="e">Fake robot current state.</param>
    private void OnRobotState(object sender, FakeRobotState e)
    {
      var commands = new List<ICommand>() { new RobotMoveCommand { LeftDistance = e.LeftEngineState.Distance, RightDistance = e.RightEngineState.Distance} };
      commands.Add(new EngineSpeedCommand() { Index = 0, AvgSpeed = 30 });
      CommandManager.Instance.DataReceived(this, commands);
    }
  }
}