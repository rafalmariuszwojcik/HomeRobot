﻿namespace RobotControl.Simulation
{
  public struct Rect2D
  {
    public Rect2D(double width, double height, MeasurementUnit unit = MeasurementUnit.Milimeter)
    {
      TopLeft = new Point2D(-width / 2F, -height / 2F, unit);
      BottomRight = new Point2D(width / 2F, height / 2F, unit);
    }

    public Point2D TopLeft { get; }

    public Point2D BottomRight { get; }

    public Length Left => new Length(TopLeft.X, TopLeft.Unit);

    public Length Top => new Length(TopLeft.Y, TopLeft.Unit);

    public Length Right => new Length(BottomRight.X, BottomRight.Unit);

    public Length Bottom => new Length(BottomRight.Y, BottomRight.Unit);

    public Length Width => Right - Left;

    public Length Height => Bottom - Top;
  }
}