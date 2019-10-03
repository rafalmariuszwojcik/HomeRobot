using System;

namespace RobotControl.Simulation.Robot
{
  public static class RobotHelper
  {
    public static SimulationPoint CalculateMovement(SimulationPoint startPoint, double leftWheel, double rightWheel)
    {
      var angleInRadians = (leftWheel - rightWheel) / (RobotCalculator.RobotRadius * 2F);
      var centerDistance = (leftWheel + rightWheel) / 2.0;

      double vector;
      double vectorAngle;
      if (angleInRadians != 0.0 && centerDistance != 0.0)
      {
        var r = centerDistance / angleInRadians;
        var vectorX = r - (r * Math.Cos(angleInRadians));
        var vectorY = r * Math.Sin(angleInRadians);
        vector = Math.Sqrt(Math.Pow(vectorX, 2) + Math.Pow(vectorY, 2));
        vectorAngle = (Math.Atan(vectorY / vectorX));
      }
      else
      {
        vector = centerDistance;
        vectorAngle = SimulationHelper.DegreesToRadians(90.0);
      }

      vectorAngle -= SimulationHelper.DegreesToRadians(startPoint.Angle);
      var deltaX = vector * Math.Cos(vectorAngle);
      var deltaY = vector * Math.Sin(vectorAngle);

      var result = new SimulationPoint(startPoint.X, startPoint.Y, startPoint.Angle + SimulationHelper.RadiansToDegrees(angleInRadians));

      if (angleInRadians * centerDistance >= 0.0)
      {
        result.Y -= deltaY;
        result.X += deltaX;
      }
      else
      {
        result.Y += deltaY;
        result.X -= deltaX;
      }

      return result;
    }
  }
}