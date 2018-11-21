using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotControl.Simulation
{
  public interface ISimulationArea
  {
    Rect2D Area { get; }

    //Length W { get; }
    //Length L { get; }
  }
}