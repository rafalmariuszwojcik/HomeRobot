using RobotControl.Command;
using RobotControl.Fake.FakeRobot;
using System;

namespace RobotControl.Communication
{
  public class Fake : ChannelBase<FakeRobot, FakeConfiguration>
  {
    public Fake(FakeConfiguration configuration) : base(configuration)
    {
    }

    protected override FakeRobot InternalOpen(FakeConfiguration configuration)
    {
      var robot = new FakeRobot();
      //robot.Start();
      return robot;
    }

    protected override void InternalClose(FakeRobot channel)
    {
      //channel.Stop();
    }

    public override void InternalSend(FakeRobot channel, ICommand[] commands)
    {
      throw new NotImplementedException();
    }

    public override void InternalSend(FakeRobot channel, string data)
    {
      throw new NotImplementedException();
    }
  }
}