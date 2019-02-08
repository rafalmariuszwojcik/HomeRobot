namespace RobotControl.Command
{
  public struct EngineSpeedCommand : IEngineCommand
  {
    [CommandParameter(0)]
    public int Index { get; set; }

    [CommandParameter(1)]
    public int Speed { get; set; }

    [CommandParameter(2)]
    public int AvgSpeed { get; set; }

    [CommandParameter(3)]
    public int PWM { get; set; }

    [CommandParameter(4)]
    public int Distance { get; set; }
  }
}