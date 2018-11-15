using RobotControl.Simulation;
using System.Drawing;

namespace RobotControl.Drawing
{
  public abstract class DrawElementBase : IDrawElement
  {
    private ISimulationItem SimulationItem { get; }

    public DrawElementBase(ISimulationItem simulationItem)
    {
      SimulationItem = simulationItem;
    }

    public void Paint(Graphics g)
    {
      var transState = g.Save();
      try
      {
        if (SimulationItem is IPositionAwareItem positionSupport)
        {
          g.TranslateTransform((float)positionSupport.Position.X, (float)positionSupport.Position.Y);
        }

        if (SimulationItem is IAngleAwareItem angleSupport)
        {
          g.RotateTransform((float)angleSupport.Angle.Value);
        }

        InternalPaint(SimulationItem, g);
      }
      finally
      {
        g.Restore(transState);
      }
    }

    protected abstract void InternalPaint(Simulation.ISimulationItem simulationItem, Graphics g);
  }
}