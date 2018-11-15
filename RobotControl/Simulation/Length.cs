using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotControl.Simulation
{
  public struct Length
  {
    public Length(double value, MeasurementUnit unit)
    {
      Value = value;
      Unit = unit;
    }

    public double Value { get; }
    public MeasurementUnit Unit { get; }

    public Length ConvertTo(MeasurementUnit unit)
    {
      return new Length();
    }
  }
}
