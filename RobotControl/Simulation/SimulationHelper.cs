using System;

namespace RobotControl.Simulation
{
  public static class SimulationHelper
  {
    public static double RadiansToDegrees(double radians)
    {
      return (radians * 180) / (Math.PI);
    }

    public static double DegreesToRadians(double degrees)
    {
      return (degrees * Math.PI) / 180;
    }
  }
}
