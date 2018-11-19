namespace RobotControl.Simulation
{
  public struct Origin
  {
    public Origin(Length x, Length y)
    {
      X = x;
      Y = y;
    }

    public Length X { get; }
    public Length Y { get; }
  }
}
