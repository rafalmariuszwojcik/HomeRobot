using RobotControl.Simulation;
using RobotControl.Simulation.Robot;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace RobotControl.Drawing
{
  public class RouteDraw : DrawElement<Simulation.Robot.IRobot>
  {
    private readonly Route route;
    private readonly RobotGeometry robotGeometry;

    public RouteDraw(Route route, RobotGeometry robotGeometry)
      : base(new PointF(route != null ? (float)route.Start.X : 0.0F, route != null ? (float)route.Start.Y : 0.0F), route != null ? (float)route.Start.Angle : 0.0F)
    {
      this.route = route;
      this.robotGeometry = robotGeometry;
    }

    protected override void InternalPaint(IRobot simulationItem, Graphics g)
    {
      DrawRoute(g);
    }

    private void DrawRoute(Graphics g)
    {
      if (route == null)
      {
        return;
      }

      foreach (var movement in route.Movements)
      {
        var movementCalculation = robotGeometry.CalculateMovement(movement.Distance, movement.Angle);
        DrawMovement(g, movementCalculation);
        g.TranslateTransform((float)movementCalculation.Vector.X, -(float)movementCalculation.Vector.Y);
        g.RotateTransform((float)movementCalculation.RotationInDegrees);
      }
    }

    private void DrawMovement(Graphics g, MovementCalculation movementCalculation)
    {
      var radius = (float)movementCalculation.Radius;
      var rotation = (float)movementCalculation.RotationInDegrees;
      var distance = (float)movementCalculation.Distance;
      var center = new PointF((!float.IsInfinity(radius) ? radius : 0.0F), 0.0F);
      var centerPen = new Pen(Color.FromArgb(255, 0, 200, 0)) { Width = 2, DashStyle = DashStyle.Dot };
      DrawMovement(g, centerPen, center, radius, rotation, distance);
    }

    private void DrawMovement(Graphics g, Pen pen, PointF center, float radius, float rotation, float distance)
    {
      var startAngle = radius > 0 ? 180.0F : 0.0F;
      if (!float.IsInfinity(radius))
      {
        g.DrawArc(pen, center.X - Math.Abs(radius), center.Y - Math.Abs(radius), Math.Abs(radius * 2.0F), Math.Abs(radius * 2.0F), startAngle, rotation);
      }
      else
      {
        g.DrawLine(pen, center.X, center.Y, center.X, center.Y - distance);
      }
    }
  }
}
