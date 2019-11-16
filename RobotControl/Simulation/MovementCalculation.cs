using System;

namespace RobotControl.Simulation
{
  public struct MovementCalculation
  {
    private double radius;
    private double distance;
    private double leftWheelDistance;
    private double rightWheelDistance;
    private double rotation;
    private Vector vector;

    public MovementCalculation(
      double radius,
      double distance,
      double leftWheelDistance,
      double rightWheelDistance,
      double rotation,
      Vector vector)
    {
      this.radius = radius;
      this.distance = distance;
      this.leftWheelDistance = leftWheelDistance;
      this.rightWheelDistance = rightWheelDistance;
      this.rotation = rotation;
      this.vector = vector;
    }

    public double Radius
    {
      get { return radius; }
    }

    public double Distance
    {
      get { return distance; }
    }

    public double LeftWheelDistance
    {
      get { return leftWheelDistance; }
    }

    public double RightWheelDistance
    {
      get { return rightWheelDistance; }
    }

    public double RotationInRadians
    {
      get { return rotation; }
    }

    public double RotationInDegrees
    {
      get { return (rotation * 180) / (Math.PI); }
    }

    public Vector Vector
    {
      get { return vector; }
    }

    public static MovementCalculation Empty
    {
      get { return new MovementCalculation(0.0, 0.0, 0.0, 0.0, 0.0, new Vector()); }
    }
  }
}
