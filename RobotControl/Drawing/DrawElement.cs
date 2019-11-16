using System.Drawing;

namespace RobotControl.Drawing
{
  public abstract class DrawElement<T> : DrawElementBase where T : Simulation.ISimulationItem
  {
    public DrawElement(T simulationItem) : base(simulationItem)
    {
    }

    protected sealed override void InternalPaint(Simulation.ISimulationItem simulationItem, Graphics g)
    {
      DrawItem((T)simulationItem, g);
    }

    protected abstract void DrawItem(T item, Graphics g);
  }
}