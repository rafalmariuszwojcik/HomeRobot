using System.Drawing;

namespace RobotControl.Drawing
{
  public abstract class DrawElement : IDrawElement
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
        InternalPaint(g);
      }
      finally
      {
        NeedsRedraw = false;
        g.Restore(transState);
      }
    }

    protected abstract void InternalPaint(Graphics g);
  }
}