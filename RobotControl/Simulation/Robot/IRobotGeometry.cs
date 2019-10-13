namespace RobotControl.Simulation.Robot
{
  public interface IRobotGeometry : IGeometry
  {
    double Width { get; }
    double WheelRadius { get; }
    int EncoderPoints { get; }
    double OnePointDistance { get; }
    double WheelWidth { get; }
    double Radius { get; }
  }
}