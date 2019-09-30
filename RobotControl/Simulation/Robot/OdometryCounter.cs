using RobotControl.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotControl.Simulation.Robot
{
  internal struct OdometryResult
  {
    public OdometryResult(double dl, double dr)
    {
      this.Dl = dl;
      this.Dr = dr;
    }

    public double Dl { get; }
    public double Dr { get; }
  }

  internal static class OdometryCounter
  {
    public static OdometryResult Calculate(long currentMilis, IEnumerable<IEncoderCommand> data)
    {


      return new OdometryResult(0, 0);
    }  
  }
}
