using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotControl.Fake.FakeRobot
{
  public enum EngineState { UNKNOWN = 0, STOP = 1, FORWARD = 2, BACKWARD = 3 };

  public class Engine
  {
    private volatile EngineState state;
  }
}
