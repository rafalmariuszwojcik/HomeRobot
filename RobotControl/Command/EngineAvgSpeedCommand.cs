using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotControl.Command
{
  public struct EngineAvgSpeedCommand : IEngineCommand
  {
    public int Index { get; set; }
    public int AvgSpeed { get; set; }
  }
}