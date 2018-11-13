using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotControl.Drawing
{
  public static class DrawingFactory
  {
    static DrawingFactory()
    {
    }

    public static IDrawElement GetDrawingInstance(Simulation.ISimulationItem simulationItem)
    {
      return null;
    }
  }
}
