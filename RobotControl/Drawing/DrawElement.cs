using System.Drawing;

namespace RobotControl.Drawing
{
  public abstract class DrawElement<T> : DrawElementBase where T: Simulation.ISimulationItem
  {
    public bool NeedsRedraw { get; protected set; }
    protected PointF position;
    protected float angle;

    public DrawElement()
    {
    }

    public DrawElement(PointF position, float angle)
    {
      this.position = position;
      this.angle = angle;
    }

    public void Paint(Simulation.ISimulationItem simulationItem, Graphics g)
    {
      var transState = g.Save();
      try
      {
        g.TranslateTransform(position.X, position.Y);
        g.RotateTransform(angle);
        InternalPaint((T)simulationItem, g);
      }
      finally
      {
        NeedsRedraw = false;
        g.Restore(transState);
      }
    }

    protected abstract void InternalPaint(T simulationItem, Graphics g);
  }
}