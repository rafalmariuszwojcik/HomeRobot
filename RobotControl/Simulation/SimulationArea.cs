using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotControl.Simulation
{
  public class SimulationArea : ISimulationArea
  {
    public SimulationArea(Length w, Length l)
    {
      W = w;
      L = l;
    }

    public Length W { get; }
    public Length L { get; }
  }
}
