namespace RobotControl.Command
{
  /// <summary>
  /// Manual controller command (GamePad).
  /// </summary>
  /// <seealso cref="RobotControl.Command.ICommand" />
  public interface IControllerCommand : ICommand
  {
    float X { get; set; }
    float Y { get; set; }

    float RX { get; set; }
    float RY { get; set; }
  }
}
