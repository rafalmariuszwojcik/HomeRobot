using NUnit.Framework;
using RobotControl.Simulation.Robot;
using System;
using System.Threading.Tasks;

namespace Tests
{
  public class RobotTests
  {
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
      const double ROBOT_WIDTH = 124.0;
      const double WHEEL_RADIUS = 33.2;
      const int ENCODER_POINTS = 20;

      var calculator = new PositionCalculator(ROBOT_WIDTH, WHEEL_RADIUS, ENCODER_POINTS);

      var robot = new Robot();
      var start = DateTime.Now;
      for (var i = 1; i <= 10; i++)
      {
        calculator.Signal(Encoder.Left, i, (long)(DateTime.Now - start).TotalMilliseconds);
        Task.Delay(10).Wait();
        calculator.Signal(Encoder.Right, i, (long)(DateTime.Now - start).TotalMilliseconds);
        Task.Delay(90).Wait();
      }
      
      Assert.Pass();
    }
  }
}