using RobotControl.Command;
using System.Collections.Generic;
using System.Linq;

namespace RobotControl.Simulation.Robot
{
  public struct OdometryResult
  {
    public OdometryResult(double dl, double dr)
    {
      this.Dl = dl;
      this.Dr = dr;
    }

    public double Dl { get; }
    public double Dr { get; }
  }

  public static class OdometryCounter
  {
    public static OdometryResult Calculate(long currentMilis, IEnumerable<IEncoderCommand> data)
    {
      var leftSignals = data.Where(x => x.Index == 0).OrderByDescending(x => x.Distance).ToList();
      var rightSignals = data.Where(x => x.Index == 1).OrderByDescending(x => x.Distance).ToList();

      var leftDistance = leftSignals.Any() ? leftSignals.First().Distance : 0;
      var rightDistance = rightSignals.Any() ? rightSignals.First().Distance : 0;

      var dl = 0;
      var timeL = 0L;
      var speedL = 0.0;

      var dr = 0;
      var timeR = 0L;
      var speedR = 0.0;

      if (leftSignals.Skip(1).Any())
      {
        dl = leftSignals.First().Distance - leftSignals.Skip(1).Select(x => x.Distance).First();
        timeL = leftSignals.First().Milis - leftSignals.Skip(1).Select(x => x.Milis).First();
        speedL = (double)dl / (double)timeL;
      }

      if (rightSignals.Skip(1).Any())
      {
        dr = rightSignals.First().Distance - rightSignals.Skip(1).Select(x => x.Distance).First();
        timeR = rightSignals.First().Milis - rightSignals.Skip(1).Select(x => x.Milis).First();
        speedR = (double)dr / (double)timeR;
      }

      var deltaL = (double)leftDistance + (speedL * (currentMilis - leftSignals.First().Milis));
      var deltaR = (double)rightDistance + (speedR * (currentMilis - rightSignals.First().Milis));
                    
      return new OdometryResult(deltaL, deltaR);
    }  
  }
}
