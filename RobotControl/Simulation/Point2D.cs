namespace RobotControl.Simulation
{
  public struct Point2D
  {
    private readonly Length x;
    private readonly Length y;

    public Point2D(double x, double y, MeasurementUnit unit = MeasurementUnit.Milimeter)
    {
      this.x = new Length(x, unit);
      this.y = new Length(y, unit);
    }

    public double X
    {
      get { return x.Value; }
    }

    public double Y
    {
      get { return y.Value; }
    }

    public MeasurementUnit Unit
    {
      get { return x.Unit; }
    }

    public Point2D ConvertTo(MeasurementUnit unit)
    {
      return new Point2D(x.ConvertTo(unit).Value, y.ConvertTo(unit).Value, unit);
    }
  }
}