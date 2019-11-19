using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotControl.Command.Robot
{
  /// <summary>
  /// Robot's engine state command.
  /// </summary>
  /// <seealso cref="RobotControl.Command.Robot.IRobotEngineStateCommand" />
  public struct RobotEngineStateCommand : IRobotEngineStateCommand
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="RobotEngineStateCommand"/> struct.
    /// </summary>
    /// <param name="engineId">The engine identifier.</param>
    /// <param name="speed">The speed.</param>
    /// <param name="maxSpeed">The maximum speed.</param>
    public RobotEngineStateCommand(EngineId engineId, int speed, int maxSpeed) 
    {
      EngineId = engineId;
      Speed = speed;
      MaxSpeed = maxSpeed;
    }

    /// <summary>
    /// Gets the engine identifier.
    /// </summary>
    public EngineId EngineId { get; private set; }

    /// <summary>
    /// Gets the speed.
    /// </summary>
    public int Speed { get; private set; }

    /// <summary>
    /// Gets the maximum speed.
    /// </summary>
    public int MaxSpeed { get; private set; }
  }
}
