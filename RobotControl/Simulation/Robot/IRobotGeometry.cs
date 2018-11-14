namespace RobotControl.Simulation.Robot
{
  public interface IRobotGeometry : IGeometry
  {
    double Radius { get; }
    double Width { get; }
    double WheelRadius { get; }
    double WheelWidth { get; }
    int EncoderPoints { get; }
  }
}