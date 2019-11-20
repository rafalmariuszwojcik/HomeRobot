using System;

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
    /// Initializes a new instance of the <see cref="ControllerVibrationCommand" /> struct.
    /// </summary>
    /// <param name="engineId">The engine identifier.</param>
    /// <param name="motorSpeed">The motor speed.</param>
    public ControllerVibrationCommand(ControllerVibrationEngineId engineId, int motorSpeed)
    {
      EngineId = engineId;
      MotorSpeed = Math.Min(Math.Max(motorSpeed, MIN_SPEED), MAX_SPEED);
    }

    /// <summary>
    /// Gets the vibration engine identifier.
    /// </summary>
    public ControllerVibrationEngineId EngineId { get; private set; }

    /// <summary>
    /// Gets the motor speed.
    /// </summary>
    /// <remarks>In percentage (0; +100).</remarks>
    public int MotorSpeed { get; private set; }
  }
}