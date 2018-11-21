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

    public Length ConvertTo(MeasurementUnit unit)
    {
      if (Unit == unit)
      {
        return this;
      }

      return new Length(Value * convert[(int)Unit, (int)unit], unit);
    }

    /*
     
    public static bool operator <(Employee emp1, Employee emp2){

        return Comparison(emp1, emp2) < 0;

    }

    public static bool operator >(Employee emp1, Employee emp2){

        return Comparison(emp1, emp2) > 0;

    }

    public static bool operator ==(Employee emp1, Employee emp2){

        return Comparison(emp1, emp2) == 0;

    }

    public static bool operator !=(Employee emp1, Employee emp2){

        return Comparison(emp1, emp2) != 0;

    }

    public override bool Equals(object obj){

        if (!(obj is Employee)) return false;

        return this == (Employee)obj;

    }

    public static bool operator <=(Employee emp1, Employee emp2){

        return Comparison(emp1, emp2) <= 0;

    }

    public static bool operator >=(Employee emp1, Employee emp2){

        return Comparison(emp1, emp2) >= 0;

    }

    public static int Comparison(Employee emp1, Employee emp2){

        if (emp1.JobGrade < emp2.JobGrade)

            return -1;

        else if (emp1.JobGrade == emp2.JobGrade)

            return 0;

        else if (emp1.JobGrade > emp2.JobGrade)

            return 1;

        return 0;

    }
     */
  }
}