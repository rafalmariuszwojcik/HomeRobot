using NUnit.Framework;
using RobotControl.Fake.FakeRobot;

namespace RobotControlTest.FakeRobot
{
  /// <summary>
  /// Fake engine tests.
  /// </summary>
  public class EngineTest
  {
    /// <summary>
    /// Setups this instance.
    /// </summary>
    [SetUp]
    public void Setup()
    {
    }

    /// <summary>
    /// Starts the engine.
    /// </summary>
    /// <remarks>Starts engine and check speed counting.</remarks>
    [Test]
    public void StartEngine()
    {
      const int SPEED = 10;
      var engine = new Engine();
      //using (var engine = new Engine()) 
      //{
      //engine.Speed = SPEED;

      //var d = engine.GetDistance();
      //var x = d;
      //Thread.Sleep(500);
      //var e = engine.GetDistance();

      //for (var i = 0; i < 100; i++) 
      //{
      //var signaled = engine.SignalEvent.WaitOne(1000);
      //Assert.IsTrue(signaled);
      //if (engine.CurrentSpeed > 0.0) 
      //{
      //Assert.IsTrue(Math.Abs(engine.CurrentSpeed - SPEED) <= (SPEED * 0.2));
      //}
      //}
      //}
    }
  }
}
