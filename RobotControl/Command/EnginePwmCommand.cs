namespace RobotControl.Command
{
  public struct EnginePwmCommand : IEngineCommand
  {
    public int Index { get; set; }
    public int Pwm { get; set; }
  }
}
