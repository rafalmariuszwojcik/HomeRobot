namespace RobotControl.Command
{
  public struct EncoderCommand : IEncoderCommand
  {
    [CommandParameter(0)]
    public int Index { get; set; }

    [CommandParameter(1)]
    public double Distance { get; set; }

    [CommandParameter(2)]
    public long Milis { get; set; }

    [CommandParameter(3)]
    public int EngineState { get; set; }
  }
}
