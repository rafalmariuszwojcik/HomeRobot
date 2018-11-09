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
      return new FakeRobot();
    }

    protected override void InternalClose(FakeRobot channel)
    {
      throw new NotImplementedException();
    }

    public override void InternalSend(FakeRobot channel, string data)
    {
      throw new NotImplementedException();
    }
  }
}
