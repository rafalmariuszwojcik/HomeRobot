using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotControl.Command.Controller
{
  /// <summary>
  /// Manual controller vibration command (GamePad).
  /// </summary>
  /// <seealso cref="RobotControl.Command.Controller.IControllerVibrationCommand" />
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "<Pending>")]
  public struct ControllerVibrationCommand : IControllerVibrationCommand
  {
    /// <summary>
    /// The maximum power value.
    /// </summary>
    public const int MAX_SPEED = 100;

    /// <summary>
    /// The minimum power value.
    /// </summary>
    public const int MIN_SPEED = 0;

    /// <summary>
    /// Initializes a new instance of the <see cref="ControllerVibrationCommand"/> struct.
    /// </summary>
    /// <param name="leftMotorSpeed">The left motor speed.</param>
    /// <param name="rightMotorSpeed">The right motor speed.</param>
    public ControllerVibrationCommand(int leftMotorSpeed, int rightMotorSpeed) 
    {
      LeftMotorSpeed = Math.Min(Math.Max(leftMotorSpeed, MIN_SPEED), MAX_SPEED);
      RightMotorSpeed = Math.Min(Math.Max(rightMotorSpeed, MIN_SPEED), MAX_SPEED);
    }

    /// <summary>
    /// Gets the left motor speed.
    /// </summary>
    /// <remarks>In percentage (0; +100).</remarks>
    public int LeftMotorSpeed { get; private set; }

    /// <summary>
    /// Gets the right motor speed.
    /// </summary>
    /// <remarks>In percentage (0; +100).</remarks>
    public int RightMotorSpeed { get; private set; }
  }
}
