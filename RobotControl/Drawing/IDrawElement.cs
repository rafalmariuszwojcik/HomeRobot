using System.Drawing;

namespace RobotControl.Drawing
{
  public interface IDrawElement
  {
    void Paint(Simulation.ISimulationItem simulationItem, Graphics g);
  }
}
