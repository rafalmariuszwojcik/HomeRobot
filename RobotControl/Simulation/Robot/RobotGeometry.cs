using System;

namespace RobotControl.Simulation.Robot
{
  public class RobotGeometry : IRobotGeometry
  {
    // distance between wheels (two wheels differential robot).
    private readonly double robotWidth;

    // wheel radius.
    private readonly double wheelRadius;

    // number of encodes signals per one wheel rotation.
    private readonly int encoderPoints;

    // wheel width in milimeters.
    private readonly double wheelWidth;

    public RobotGeometry(double robotWidth, double wheelRadius, double wheelWidth, int encoderPoints)
    {
      this.robotWidth = robotWidth;
      this.wheelRadius = wheelRadius;
      this.wheelWidth = wheelWidth;
      this.encoderPoints = encoderPoints;
    }

    public double Radius
    {
      get { return robotWidth / 2.0; }
    }

    public double WheelRadius
    {
      get { return wheelRadius; }
    }

    public double WheelWidth
    {
      get { return wheelWidth; }
    }

    public double Width
    {
      get { return robotWidth; }
    }

    public int EncoderPoints
    {
      get { return encoderPoints; }
    }

    public MovementCalculation CalculateMovement(double distance, double angle)
    {
      // movement radius in degrees.
      double r;

      // left and right distance in milimeters.
      double dl;
      double dr;
      Vector vector;

      if (angle != 0.0)
      {
        r = (360.0 * /*Math.Abs*/(distance)) / (2.0 * Math.PI * angle);
        dl = ((angle * 2.0 * Math.PI * (r + Radius)) / 360.0);// * (distance < 0 ? -1.0 : 1.0);
        dr = ((angle * 2.0 * Math.PI * (r - Radius)) / 360.0);// * (distance < 0 ? -1.0 : 1.0);
        var angleInRadians = (angle * Math.PI) / 180;
        vector = new Vector(r - (r * Math.Cos(angleInRadians)), r * Math.Sin(angleInRadians));
      }
      else
      {
        r = double.PositiveInfinity;
        dl = distance;
        dr = distance;
        vector = new Vector(0.0, distance);
      }
      
      return new MovementCalculation(r, distance, dl, dr, (dl - dr) / robotWidth, vector);
    }
  }
}