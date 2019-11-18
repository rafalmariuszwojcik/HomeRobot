using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotControl.Command.Robot
{
  /// <summary>
  /// Set robot's engines power command.
  /// </summary>
  /// <seealso cref="RobotControl.Command.Robot.IRobotEnginesPowerCommand" />
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "<Pending>")]
  public struct RobotEnginesPowerCommand : IRobotEnginesPowerCommand
  {
    /// <summary>
    /// The maximum power value.
    /// </summary>
    public const int MAX_POWER = 100;

    /// <summary>
    /// The minimum power value.
    /// </summary>
    public const int MIN_POWER = -100;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="RobotEnginesPowerCommand"/> struct.
    /// </summary>
    /// <param name="leftEnginePower">The left engine power.</param>
    /// <param name="rightEnginePower">The right engine power.</param>
    public RobotEnginesPowerCommand(int leftEnginePower, int rightEnginePower) 
    {
      LeftEnginePower = Math.Min(Math.Max(leftEnginePower, MIN_POWER), MAX_POWER);
      RightEnginePower = Math.Min(Math.Max(rightEnginePower, MIN_POWER), MAX_POWER);
    }
    
    /// <summary>
    /// Gets the left engine power.
    /// </summary>
    /// <remarks>In percentage (-100; +100).</remarks>
    public int LeftEnginePower { get; private set; }

    /// <summary>
    /// Gets the right engine power.
    /// </summary>
    /// <remarks>In percentage (-100; +100).</remarks>
    public int RightEnginePower { get; private set; }
  }
}
