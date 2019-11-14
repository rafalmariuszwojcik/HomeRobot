namespace RobotControl.Command
{
  /// <summary>
  /// Manual controller command (GamePad).
  /// </summary>
  /// <seealso cref="RobotControl.Command.IControllerCommand" />
  public struct ControllerCommand : IControllerCommand
  {
    public float X { get; set; }
    public float Y { get; set; }

    public float RX { get; set; }
    public float RY { get; set; }

  }
}
