using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotControl.Drawing
{
  public interface IDrawElement
  {
    void Paint(Simulation.ISimulationItem simulationItem, Graphics g);
  }
}
