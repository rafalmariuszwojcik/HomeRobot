using RobotControl.Command;
using RobotControl.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RobotControl.Simulation.Robot
{
  public class Odometry : DataProcessingQueue<IEncoderCommand>
  {
    private readonly Action<double, double> action;
    private IList<Tuple<int, IList<long>>> list = new List<Tuple<int, IList<long>>>();

    public Odometry(Action<double, double> action)
      : base((s, d) => ((Odometry)s).PostData(d), 50)
    {
      this.action = action;
    }

    private void PostData(IEnumerable<IEncoderCommand> data)
    {
      var copy = new List<long>(data.Select(x => x.Milis));
      for (var i = 0; i < data.Count() - 1; i++)
      {
        copy[i] = copy[i + 1] - copy[i];
      }

      list.Add(new Tuple<int, IList<long>>(data.Count(), copy));
      
      if (data.Any(x => x.Distance >= 99))
      {
      }

      var dl = data.Where(x => x.Index == 0).Max(x => x.Distance);
      var dr = data.Where(x => x.Index == 1).Max(x => x.Distance);
      action?.Invoke(dl, dr);

    }
  }
}
