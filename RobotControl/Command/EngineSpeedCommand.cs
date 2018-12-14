namespace RobotControl.Command
{
  public struct EngineSpeedCommand : IEngineCommand
  {
    public int Index { get; set; }
    public int Speed { get; set; }
  }
}