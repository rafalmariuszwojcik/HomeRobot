namespace RobotControl.Simulation
{
  public class SimulationPoint
  {
    private double x;
    private double y;
    private double angle;

    public SimulationPoint(double x, double y, double angle) 
    {
      this.x = x;
      this.y = y;
      this.angle = angle;
    }

    public double X 
    {
      get { return x; }
      set { x = value; }
    }

    public double Y
    {
      get { return y; }
      set { y = value; }
    }

    public double Angle
    {
      get { return angle; }
      set { angle = value; }
    }
  }
}
