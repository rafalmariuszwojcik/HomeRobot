using RobotControl.Command;
using RobotControl.Communication;
using RobotControl.Core;
using System;
using System.Collections.Generic;

namespace RobotControl.Simulation.Robot
{
  public class Robot : SimulationItem, IRobot, ICommandListener
  {
    private const double ROBOT_WIDTH = 124.0;
    private const double WHEEL_RADIUS = 33.2;
    private const double WHEEL_WIDTH = 25.0;
    private const int ENCODER_POINTS = 20;

    private readonly SimulationPoint position;
    private readonly Odometry odometry;
    private RobotGeometry robotGeometry = new RobotGeometry(ROBOT_WIDTH, WHEEL_RADIUS, WHEEL_WIDTH, ENCODER_POINTS);
    private Route route;
    public int? leftEncoderPoints = null;
    public int? rightEncoderPoints = null;
    private MovementStartPoint startPoint;
    
    public Robot()
      : this(0.0, 0.0, 0.0)
    {
    }

    public Robot(double x, double y, double angle) 
      : base()
    {
      position = new SimulationPoint(x, y, angle);
      odometry = new Odometry(new Action<double, double>((dl, dr) => 
        {
          MessageReceived(2, dl, 2, dr);
        }
      ));
    }

    public Route Route
    {
      get { return route; }
    }

    Point2D IPositionAwareItem.Position => new Point2D(position.X, position.Y);
    Angle2D IAngleAwareItem.Angle => new Angle2D { Value = position.Angle };

    public IRobotGeometry Geometry => robotGeometry;

    public void AddMovement(double distance, double angle) 
    {
      if (route == null) 
      {
        route = new Route(position.X, position.Y, position.Angle);
      }

      route.Movements.Add(new Movement(distance, angle));
    }

    public void MessageReceived(int leftDirection, double leftDistance, int rightDirection, double rightDistance)
    {
      if (startPoint == null)
      {
        startPoint = new MovementStartPoint(position.X, position.Y, position.Angle, 0.0, 0.0);
        //startPoint = new MovementStartPoint(position.X, position.Y, position.Angle, leftDistance, rightDistance);
      }

      double oneHoleDistance = (RobotCalculator.WheelRadius * 2.0 * Math.PI) / (double)RobotCalculator.EncoderHoles;
      var leftDifference = leftDistance - startPoint.LeftEncoder;
      var rightDifference = rightDistance - startPoint.RightEncoder;
      if (leftDifference == 0 && rightDifference == 0)
      {
        return;
      }

      /*
      if (leftDifference < 0.0)
      {
        leftDifference = 0.0;
      }

      if (rightDifference < 0.0)
      {
        rightDifference = 0.0;
      }
      */

      //var newPoint = CalculateMovement(startPoint, (double)leftDifference * oneHoleDistance, (double)rightDifference * oneHoleDistance);
      var geometry = new RobotGeometry(124.0, 66.4 / 2.0, 20);
      var newPoint = RobotHelper.CalculateMovement(startPoint, leftDifference, rightDifference, geometry);
      //var newPoint = CalculateMovement(startPoint, leftDifference, rightDifference);

      if (newPoint.X > position.X)
      {
        var x = 1;
        var y = x;
      }

      position.X = newPoint.X;
      position.Y = newPoint.Y;
      position.Angle = newPoint.Angle;
      //if (startPoint.IsExpired)
      {
        startPoint = new MovementStartPoint(newPoint.X, newPoint.Y, newPoint.Angle, leftDistance, rightDistance);
      }

      SetState();
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
    
    public void WheelMove(double left, double right) 
    {
      var angleInRadians = (left - right) / (RobotCalculator.RobotRadius * 2F);
      var centerDistance = (left + right) / 2.0;

      double vector;
      double vectorAngle;
      if (angleInRadians != 0.0 /*&& centerDistance != 0.0*/)
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

      vectorAngle -= DegreesToRadians(position.Angle);
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
    }

    public SimulationPoint CalculateMovement(SimulationPoint startPoint, double leftWheel, double rightWheel)
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
        vectorAngle = DegreesToRadians(90.0);
      }

      vectorAngle -= DegreesToRadians(startPoint.Angle);
      var deltaX = vector * Math.Cos(vectorAngle);
      var deltaY = vector * Math.Sin(vectorAngle);

      var result = new SimulationPoint(startPoint.X, startPoint.Y, startPoint.Angle + RadiansToDegrees(angleInRadians));
      
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

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
      }
    }

    private void ProcessCommand(ICommand command)
    {
      if (command is RobotMoveCommand cmd)
      {
        var list = new List<IEncoderCommand>();
        list.Add(new EncoderCommand { Index = 0, Distance = cmd.LeftDistance });
        list.Add(new EncoderCommand { Index = 1, Distance = cmd.RightDistance });
        odometry.Enqueue(list);
        
        //MessageReceived(cmd.LeftDirection, (int)cmd.LeftDistance, cmd.RightDirection, (int)cmd.RightDistance);
      }
      else if (command is IEncoderCommand encoderCommand)
      {
        odometry.Enqueue(new[] { encoderCommand });
      }
    }

    void IListener<ICommand>.DataReceived(IChannel channel, IEnumerable<ICommand> data)
    {
      foreach (var command in data)
      {
        ProcessCommand(command);
      }
    }

    /*
    public void MessageReceived(int leftDirection, int leftDistance, int rightDirection, int rightDistance)
    {
      if (startPoint == null)
      {
        startPoint = new MovementStartPoint(position.X, position.Y, position.Angle, leftDistance, rightDistance);
      }

      double oneHoleDistance = (RobotCalculator.WheelRadius * 2.0 * Math.PI) / (double)RobotCalculator.EncoderHoles;
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
        StateChanged = true;
      }
    }
    */

    private class MovementStartPoint : SimulationPoint
    {
      //private readonly DateTime expiration;

      public MovementStartPoint(double x, double y, double angle, double leftEncoder, double rightEncoder)
        : base(x, y, angle)
      {
        //expiration = DateTime.Now.AddMilliseconds(1);
        LeftEncoder = leftEncoder;
        RightEncoder = rightEncoder;
      }

      /*
      public bool IsExpired
      {
        get { return DateTime.Now > expiration; }
      }
      */

      public double LeftEncoder { get; private set; }
      public double RightEncoder { get; private set; }
    }
  }
}