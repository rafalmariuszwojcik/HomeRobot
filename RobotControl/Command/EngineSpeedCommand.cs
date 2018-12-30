namespace RobotControl.Command
{
  public struct EngineSpeedCommand : IEngineCommand
  {
    [CommandParameter(0)]
    public int Index { get; set; }

    [CommandParameter(1)]
    public int Speed { get; set; }
  }
}