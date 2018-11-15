using RobotControl.Simulation.Robot;
using System.Drawing;

namespace RobotControl.Drawing
{
  public class RobotDraw : DrawElement<IRobot>
  {
    public RobotDraw(IRobot simulationItem) : base(simulationItem)
    {
    }

    protected override void DrawItem(IRobot simulationItem, Graphics g)
    {
      DrawRobot(simulationItem, g);
    }

    private void DrawRobot(IRobot robot, Graphics g)
    {
      var penR1 = new Pen(Color.FromArgb(255, 160, 160, 160));
      penR1.Width = 4;
      g.DrawLine(penR1, 0, 0, (float)robot.Geometry.Radius, 0);

      var penR2 = new Pen(Color.FromArgb(255, 180, 180, 180));
      penR2.Width = 4;
      g.DrawLine(penR2, 0, 0, -(float)robot.Geometry.Radius, 0);

      var penR3 = new Pen(Color.FromArgb(255, 200, 200, 200));
      penR3.Width = 2;
      g.DrawEllipse(
        penR3,
        -(float)robot.Geometry.Radius,
        -(float)robot.Geometry.Radius,
        (float)robot.Geometry.Radius * 2,
        (float)robot.Geometry.Radius * 2);

      var brushR3 = new SolidBrush(Color.FromArgb(64, 200, 200, 200));
      g.FillEllipse(
        brushR3,
        -(float)robot.Geometry.Radius,
        -(float)robot.Geometry.Radius,
        (float)robot.Geometry.Radius * 2,
        (float)robot.Geometry.Radius * 2);

      var penR4 = new Pen(Color.FromArgb(255, 0, 0, 0));
      penR4.Width = 2;
      g.DrawRectangle(
        penR4,
        -(float)robot.Geometry.Radius - ((float)robot.Geometry.WheelWidth / 2.0F),
        -(float)robot.Geometry.WheelRadius,
        (float)robot.Geometry.WheelWidth,
        (float)robot.Geometry.WheelRadius * 2.0F);
      g.DrawRectangle(
        penR4,
        (float)robot.Geometry.Radius - ((float)robot.Geometry.WheelWidth / 2.0F),
        -(float)robot.Geometry.WheelRadius,
        (float)robot.Geometry.WheelWidth,
        (float)robot.Geometry.WheelRadius * 2.0F);

      var penR5 = new Pen(Color.FromArgb(255, 255, 0, 0));
      penR5.Width = 4;
      g.DrawLine(penR5, 0, 20, 0, -20);
      g.DrawLine(penR5, 0, -20, -10, -10);
      g.DrawLine(penR5, 0, -20, 10, -10);
    }
  }
}