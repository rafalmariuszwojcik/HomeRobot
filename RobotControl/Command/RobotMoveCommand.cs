namespace RobotControl.Command
{
  public struct RobotMoveCommand : ICommand
  {
    [CommandParameter(0)]
    public int LeftDirection { get; set; }

    [CommandParameter(1)]
    public double LeftDistance { get; set; }

    [CommandParameter(2)]
    public int RightDirection { get; set; }

    [CommandParameter(3)]
    public double RightDistance { get; set; }
  }
}
