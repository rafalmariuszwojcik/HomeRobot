using System;

namespace RobotControl.Simulation
{
  public abstract class SimulationElement : ISimulationItem
  {
    protected readonly SimulationPoint position;

    public SimulationElement(double x, double y, double angle)
    {
      position = new SimulationPoint(x, y, angle);
    }

    public SimulationPoint Position
    {
      get { return position; }
    }

    public static double RadiansToDegrees(double radians)
    {
      return (radians * 180) / (Math.PI);
    }

    public static double DegreesToRadians(double degrees)
    {
      return (degrees * Math.PI) / 180;
    }

    protected bool StateChanged { get; set; }

    ISimulation ISimulationItem.Simulation => throw new NotImplementedException();

    bool ISimulationItem.StateChanged => StateChanged;
    
    void ISimulationItem.ResetState()
    {
      StateChanged = false;
    }
  }
}
