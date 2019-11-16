namespace RobotControl.Command
{
  public struct EngineAvgSpeedCommand : IEngineCommand
  {
    public int Index { get; set; }
    public int AvgSpeed { get; set; }
  }
}