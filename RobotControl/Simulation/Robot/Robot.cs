using System;

namespace RobotControl.Simulation.Robot
{
  public class Robot : SimulationElement, IRobot, IMessageReceiver
  {
    private const double ROBOT_WIDTH = 124.0;
    private const double WHEEL_RADIUS = 33.2;
    private const double WHEEL_WIDTH = 25.0;
    private const int ENCODER_POINTS = 20;

    private RobotGeometry robotGeometry = new RobotGeometry(ROBOT_WIDTH, WHEEL_RADIUS, WHEEL_WIDTH, ENCODER_POINTS);
    private Route route;
    public int? leftEncoderPoints = null;
    public int? rightEncoderPoints = null;



    public Robot()
      : base(0.0, 0.0, 0.0)
    {
    }

    public Robot(double x, double y, double angle) 
      : base(x, y, angle)
    {
    }

    public Route Route
    {
      get { return route; }
    }

    public RobotGeometry RobotGeometry
    {
      get { return robotGeometry; }
    }

    Point2D IPositionAwareItem.Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Angle2D Angle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public void AddMovement(double distance, double angle) 
    {
      if (route == null) 
      {
        route = new Route(position.X, position.Y, position.Angle);
      }

      route.Movements.Add(new Movement(distance, angle));
    }
        
    public void MessageReceived(string message, object[] parameters) 
    {
      const int FORWARD = 2;
      const int BACKWARD = 3;
      const int MAXVALUE = 255;

      double oneHoleDistance = (RobotCalculator.WheelRadius * 2.0 * Math.PI) / (double)RobotCalculator.EncoderHoles;
      int leftDirection;
      int leftDistance;
      int rightDirection;
      int rightDistance;

      if (object.Equals(message, "DIST")) 
      {
        if (
          parameters.Length >= 4 &&
          int.TryParse(parameters[0] as string, out leftDirection) &&
          int.TryParse(parameters[1] as string, out leftDistance) &&
          int.TryParse(parameters[2] as string, out rightDirection) &&
          int.TryParse(parameters[3] as string, out rightDistance))
        {
          if (!leftEncoderPoints.HasValue) 
          {
            leftEncoderPoints = leftDistance;
          }

          if (!rightEncoderPoints.HasValue)
          {
            rightEncoderPoints = rightDistance;
          }
                    
          if (leftDistance != leftEncoderPoints || rightDistance != rightEncoderPoints)
          {
            var leftDifference = CalcDifference(leftEncoderPoints.Value, leftDistance, leftDirection);
            var rightDifference = CalcDifference(rightEncoderPoints.Value, rightDistance, rightDirection);
            WheelMove(leftDifference * oneHoleDistance, rightDifference * oneHoleDistance);
            //CalcEstimatedPoint(leftDifference * oneHoleDistance, rightDifference * oneHoleDistance);
            leftEncoderPoints = leftDistance;
            rightEncoderPoints = rightDistance;

            this.NeedsRedraw = true;
          }
        }
      }
    }

    private int CalcDifference(int oldValue, int newValue, int direction) 
    {
      const int FORWARD = 2;
      const int BACKWARD = 3;
      const int MAXVALUE = 255;

      var result = newValue - oldValue;
      var bOverflow = (direction == FORWARD && result < 0) || (direction == BACKWARD && result > 0);
      if (bOverflow)
      {
        result = direction == FORWARD ? MAXVALUE + result + 1 : result - MAXVALUE - 1;
      }
      
      return result;
    }
    
    public void WheelMove(int left, int right) 
    { 
    }
    
    public void WheelMove(double left, double right) 
    {
      var angleInRadians = (left - right) / (RobotCalculator.RobotRadius * 2F);
      var centerDistance = (left + right) / 2.0;

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
        vectorAngle = DegreesToRadians(90.0);
      }

      vectorAngle -= DegreesToRadians(this.position.Angle);
      var deltaX = vector * Math.Cos(vectorAngle);
      var deltaY = vector * Math.Sin(vectorAngle);
          
      if (angleInRadians * centerDistance >= 0.0)
      {
        position.Y -= (float)deltaY;
        position.X += (float)deltaX;
      }
      else
      {
        position.Y += (float)deltaY;
        position.X -= (float)deltaX;
      }

      position.Angle += (float)RadiansToDegrees(angleInRadians);
      //NeedsRedraw = true;
    }
  }
}
