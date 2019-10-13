using RobotControl.Simulation.Robot;

namespace RobotControl.Drawing
{
  public interface IRobotDrawGeometry : IRobotGeometry
  {
    double WheelWidth { get; }
    double Radius { get; }
  }
}
