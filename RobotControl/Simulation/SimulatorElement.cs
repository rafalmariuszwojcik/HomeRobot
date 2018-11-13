using System;

namespace RobotControl.Simulation
{
  public abstract class SimulationElement : ISimulationItem
  {
    public bool NeedsRedraw { get; set; }

    protected readonly SimulationPoint position;

    public SimulationElement(double x, double y, double angle)
    {
      position = new SimulationPoint(x, y, angle);
    }

    public SimulationPoint Position
    {
      get { return position; }
    }

    public ISimulation Simulation => throw new NotImplementedException();

    public static double RadiansToDegrees(double radians)
    {
      return (radians * 180) / (Math.PI);
    }

    public static double DegreesToRadians(double degrees)
    {
      return (degrees * Math.PI) / 180;
    }
  }
}
