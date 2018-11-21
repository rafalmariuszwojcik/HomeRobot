namespace RobotControl.Simulation
{
  public struct Length
  {
    private const double cmToInch = 0.393700787;
    private const double mmToInch = cmToInch / 10.0;
    private const double meterToInch = cmToInch * 100.0;
    private const double inchToMeter = 1.0 / meterToInch;
    private const double inchToCm = 1.0 / cmToInch;
    private const double inchToMm = 1.0 / mmToInch;

    private static readonly double[,] convert = new double[(int)MeasurementUnit.Inch + 1, (int)MeasurementUnit.Inch + 1]
    {
      { 1.0, 100.0, 1000.0, meterToInch},
      { 0.01, 1.0, 100.0, cmToInch},
      { 0.001, 0.01, 1.0, mmToInch},
      { inchToMeter, inchToCm, inchToMm, 1.0},
    };

    public Length(double value, MeasurementUnit unit)
    {
      Value = value;
      Unit = unit;
    }

    public double Value { get; }
    public MeasurementUnit Unit { get; }

    public static Length operator +(Length left, Length right)
    {
      return new Length(left.Value + right.ConvertTo(left.Unit).Value, left.Unit);
    }

    public static Length operator -(Length left, Length right)
    {
      return new Length(left.Value - right.ConvertTo(left.Unit).Value, left.Unit);
    }

    public static bool operator <(Length left, Length right)
    {
      return Comparison(left, right) < 0;
    }

    public static bool operator >(Length left, Length right)
    {
      return Comparison(left, right) > 0;
    }

    public static bool operator <=(Length left, Length right)
    {
      return Comparison(left, right) <= 0;
    }

    public static bool operator >=(Length left, Length right)
    {
      return Comparison(left, right) >= 0;
    }

    public static bool operator ==(Length left, Length right)
    {
      return Comparison(left, right) == 0;
    }

    public static bool operator !=(Length left, Length right)
    {
      return Comparison(left, right) != 0;
    }

    public Length ConvertTo(MeasurementUnit unit)
    {
      if (Unit == unit)
      {
        return this;
      }

      return new Length(Value * convert[(int)Unit, (int)unit], unit);
    }

    private static int Comparison(Length left, Length right)
    {
      right = right.ConvertTo(left.Unit);
      if (left.Value < right.Value)
      {
        return -1;
      }
      else if (left.Value == right.Value)
      {
        return 0;
      }
      else if (left.Value > right.Value)
      {
        return 1;
      }

      return 0;
    }

    public override bool Equals(object obj)
    {
      if (!(obj is Length))
      {
        return false;
      }

      var length = (Length)obj;
      return Value == length.Value && Unit == length.Unit;
    }

    public override int GetHashCode()
    {
      var hashCode = -177567199;
      hashCode = hashCode * -1521134295 + Value.GetHashCode();
      hashCode = hashCode * -1521134295 + Unit.GetHashCode();
      return hashCode;
    }
  }
}