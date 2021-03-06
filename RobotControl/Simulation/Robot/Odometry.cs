﻿using RobotControl.Command;
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
      : base((s, d) => ((Odometry)s).PostData(d), 100)
    {
      this.action = action;
    }

    private void PostData(IEnumerable<IEncoderCommand> data)
    {
      //var copy = new List<long>(data.Select(x => x.Milis));
      //for (var i = 0; i < data.Count() - 1; i++)
      //{
      //  copy[i] = copy[i + 1] - copy[i];
      //}

      //list.Add(new Tuple<int, IList<long>>(data.Count(), copy));

      //if (data.Any(x => x.Distance >= 99))
      //{
      //}

      var dlItems = data.Where(x => x.Index == 0);//.FirstOrDefault();//?.Max(x => x.Distance);
      var drItems = data.Where(x => x.Index == 1);//.FirstOrDefault();//?.Max(x => x.Distance);
      //var dl = dlItems.Any() ? dlItems.Max(x => Math.Abs(x.Distance)) : 0.0;
      //var dr = drItems.Any() ? drItems.Max(x => Math.Abs(x.Distance)) : 0.0;

      var dl = dlItems.Any() ? dlItems.Last().Distance : 0.0;
      var dr = drItems.Any() ? drItems.Last().Distance : 0.0;

      //var totalMilliseconds = (long)new TimeSpan(DateTime.Now.Ticks).TotalMilliseconds;
      //var result = OdometryHelper.Calculate(totalMilliseconds, data);

      //if (result.Dl > 0.1 || result.Dr > 0.1)
      //{
      //  action?.Invoke(result.Dl, result.Dr);
      //}
      action?.Invoke(dl, dr);
    }
  }
}
