using System;

namespace RobotControl.Simulation
{
  public class RobotCalculatorResult
  {
    public double r { get; set; }
    public double dl { get; set; }
    public double dr { get; set; }
    public double distance { get; set; }
    public int _dl { get; set; }
    public int _dr { get; set; }
    public int curr_dl { get; set; }
    public int curr_dr { get; set; }
  }

  public static class RobotCalculator
  {
    const double Pi2 = Math.PI * 2.0;
    public const int EncoderHoles = 20;
    public const double WheelRadius = 66.4 / 2.0;
    public const double RobotRadius = 124.0 / 2.0;
    public const double WheelWidth = 25.0;

    public static RobotCalculatorResult CalcWheelsDistances(int distance, int angle)
    {
      // left and right distance in milimeters.
      double r = 0.0;
      double dl;
      double dr;

      if (angle > 0.0 || angle < 0.0)
      {
        r = (360.0 * Math.Abs(distance)) / (Pi2 * angle);
        dl = ((angle * Pi2 * (r + RobotRadius)) / 360.0) * (distance < 0 ? -1.0 : 1.0);
        dr = ((angle * Pi2 * (r - RobotRadius)) / 360.0) * (distance < 0 ? -1.0 : 1.0);
      }
      else
      {
        r = double.PositiveInfinity;
        dl = distance;
        dr = distance;
      }

      // one hole distance in milimeters.
      double oneHoleDistance = (WheelRadius * Pi2) / (double)EncoderHoles;

      var result = new RobotCalculatorResult { r = r, dl = dl, dr = dr, distance = distance };
      result._dl = (int)(Math.Round(dl / oneHoleDistance));
      result._dr = (int)(Math.Round(dr / oneHoleDistance));

      return result;
    }
  }
}
