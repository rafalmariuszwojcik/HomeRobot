using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotControl.Command.Robot
{
  /// <summary>
  /// Engine identifier.
  /// </summary>
  public enum EngineId 
  {
    /// <summary>
    /// The left engine.
    /// </summary>
    Left,

    /// <summary>
    /// The right engine.
    /// </summary>
    Right,
  }
  
  /// <summary>
  /// Robot engine state report command.
  /// </summary>
  public interface IRobotEngineStateCommand : IRobotCommand
  {
    /// <summary>
    /// Gets the engine identifier.
    /// </summary>
    EngineId EngineId { get; }

    /// <summary>
    /// Gets the speed.
    /// </summary>
    /// <remarks>range (-MAX; MAX)</remarks>
    int Speed { get; }

    /// <summary>
    /// Gets the maximum speed.
    /// </summary>
    /// <remarks>Absolute value of maximum engine's speed.</remarks>
    int MaxSpeed { get; }
  }
}
