using RobotControl.Command;
using RobotControl.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RobotControl.Simulation.Robot
{
  public class Odometry : DataProcessingQueue<IEncoderCommand>
  {
    private IList<int> list = new List<int>();

    public Odometry(Action<double, double> action)
      : base((s, d) => ((Odometry)s).PostData(d), 250)
    {
    }

    private void PostData(IEnumerable<IEncoderCommand> data)
    {
      list.Add(data.Count());
      if (data.Any(x => x.Distance >= 99))
      {
      }

    }
  }
}
