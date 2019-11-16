using System;

namespace RobotControl.Simulation.Robot
{
  public enum Encoder
  {
    Left = 0,
    Right = 1,
  }

  public class PositionCalculator
  {
    private readonly double robotWidth;
    private readonly double wheelRadius;
    private readonly int encoderPoints;
    private readonly double oneHoleDistance;
    private readonly SignalData[] encoders = new SignalData[(int)Encoder.Right + 1] { null, null };

    public PositionCalculator(double robotWidth, double wheelRadius, int encoderPoints)
    {
      this.robotWidth = robotWidth;
      this.wheelRadius = wheelRadius;
      this.encoderPoints = encoderPoints;
      oneHoleDistance = (wheelRadius * 2.0 * Math.PI) / encoderPoints;
    }

    public void Signal(Encoder encoder, int count, long milis)
    {
      if (encoders[(int)encoder] != null)
      {
        var timeDelta = (milis - encoders[(int)encoder].Milis) / 1000.0;
        var distance = (count - encoders[(int)encoder].Count) * oneHoleDistance;
        var speed = distance / timeDelta;

      }

      encoders[(int)encoder] = new SignalData(count, milis);
    }

    private class SignalData
    {
      internal SignalData(int count, long milis)
      {
        Count = count;
        Milis = milis;
      }

      internal int Count { get; private set; }
      internal long Milis { get; private set; }
    }
  }
}
