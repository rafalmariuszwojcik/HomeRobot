using System;

namespace RobotControl.Simulation.Robot
{
  public static class RobotHelper
  {
    public static SimulationPoint CalculateMovement(SimulationPoint startPoint, double leftEncoder, double rightEncoder, IRobotGeometry geometry)
    {
      var teta0 = SimulationHelper.DegreesToRadians(startPoint.Angle - 90.0);
      var leftDistance = leftEncoder * geometry.OnePointDistance;
      var rightDistance = rightEncoder * geometry.OnePointDistance;
                  
      var s = (rightDistance + leftDistance) / 2.0;
      var teta = ((rightDistance - leftDistance) / geometry.Width) + teta0;
      var x = s * Math.Cos(teta) + startPoint.X;
      var y = s * Math.Sin(teta) + startPoint.Y;

      return new SimulationPoint(x, y, SimulationHelper.RadiansToDegrees(teta) + 90.0);
    }
  }
}