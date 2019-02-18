using NUnit.Framework;
using RobotControl.Simulation.Robot;

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
      var robot = new Robot();
      Assert.Pass();
    }
  }
}