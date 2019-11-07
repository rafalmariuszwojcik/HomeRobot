namespace RobotControl.Command
{
  /// <summary>
  /// Double wheel differential Robot move command.
  /// </summary>
  /// <seealso cref="RobotControl.Command.ICommand" />
  public struct RobotMoveCommand : ICommand
  {
    /// <summary>
    /// Gets or sets the left wheel distance.
    /// </summary>
    [CommandParameter(0)]
    public double LeftDistance { get; set; }

    /// <summary>
    /// Gets or sets the right wheel distance.
    /// </summary>
    [CommandParameter(1)]
    public double RightDistance { get; set; }
  }
}
