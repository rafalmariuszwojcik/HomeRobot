using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotControl.Simulation
{
  public interface IPositionAwareItem
  {
    Point2D Position { get; }
  }
}
