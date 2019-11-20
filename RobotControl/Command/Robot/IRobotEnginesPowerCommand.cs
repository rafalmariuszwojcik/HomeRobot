namespace RobotControl.Command.Robot
{
  /// <summary>
  /// Set robot's engines power command.
  /// </summary>
  /// <seealso cref="RobotControl.Command.Robot.IRobotCommand" />
  public interface IRobotEnginesPowerCommand : IRobotCommand
  {
    /// <summary>
    /// Gets or sets the left engine power.
    /// </summary>
    /// <remarks>In percentage (-100; +100).</remarks>
    int LeftEnginePower { get; }

    /// <summary>
    /// Gets or sets the right engine power.
    /// </summary>
    /// <remarks>In percentage (-100; +100).</remarks>
    int RightEnginePower { get; }
  }
}
