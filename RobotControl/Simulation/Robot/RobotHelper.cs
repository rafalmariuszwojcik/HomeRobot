﻿using System;

namespace RobotControl.Simulation.Robot
{
  public static class RobotHelper
  {
    /// <summary>
    /// Calculates the movement.
    /// </summary>
    /// <remarks>Low precision method, can be used only if wheel movements are wery small (close to zero).</remarks>
    /// <remarks>In this case <see cref="s"/> variable can be considered as straight line in calculation triangle (sin, cos).</remarks>
    /// <param name="startPoint">The start point.</param>
    /// <param name="leftEncoder">The left encoder.</param>
    /// <param name="rightEncoder">The right encoder.</param>
    /// <param name="geometry">The geometry.</param>
    /// <returns></returns>
    public static SimulationPoint CalculateMovement(SimulationPoint startPoint, double leftEncoder, double rightEncoder, IRobotGeometry geometry)
    {
      const double ZERO_ANGLE = 90.0;
      var teta0 = SimulationHelper.DegreesToRadians(startPoint.Angle - ZERO_ANGLE);
      var leftDistance = leftEncoder * geometry.OnePointDistance;
      var rightDistance = rightEncoder * geometry.OnePointDistance;

      var s = (leftDistance + rightDistance) / 2.0;
      var teta = ((leftDistance - rightDistance) / geometry.Width) + teta0;
      var x = s * Math.Cos(teta) + startPoint.X;
      var y = s * Math.Sin(teta) + startPoint.Y;

      return new SimulationPoint(x, y, SimulationHelper.RadiansToDegrees(teta) + ZERO_ANGLE);
    }
  }
}