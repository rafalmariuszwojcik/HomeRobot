using RobotControl.Command;
using RobotControl.Core;
using System;
using System.Collections.Generic;

namespace RobotControl.Simulation.Robot
{
  public class Odometry : DataProcessingQueue<IEncoderCommand>
  {
    public Odometry(Action<IEnumerable<IEncoderCommand>> action, int interval = 0) : base(action, interval)
    {
    }
  }
}
