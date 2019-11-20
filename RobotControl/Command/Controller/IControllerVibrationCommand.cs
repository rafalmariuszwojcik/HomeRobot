namespace RobotControl.Command.Controller
{
  /// <summary>
  /// Engine identifier type.
  /// </summary>
  public enum ControllerVibrationEngineId
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
  /// Manual controller vibration command interface.
  /// </summary>
  /// <seealso cref="RobotControl.Command.Controller.IControllerCommand" />
  public interface IControllerVibrationCommand : IControllerCommand
  {
    /// <summary>
    /// Gets the engine identifier.
    /// </summary>
    ControllerVibrationEngineId EngineId { get; }

    /// <summary>
    /// Gets the motor speed.
    /// </summary>
    /// <remarks>In percentage (0; +100).</remarks>
    int MotorSpeed { get; }
  }
}
