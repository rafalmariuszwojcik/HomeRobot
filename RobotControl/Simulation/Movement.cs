namespace RobotControl.Simulation
{
  public class Movement
  {
    private readonly double distance;
    private readonly double angle;

    public Movement(double distance, double angle)
    {
      this.distance = distance;
      this.angle = angle;
    }

    public double Distance
    {
      get { return distance; }
    }

    public double Angle
    {
      get { return angle; }
    }
  }
}
