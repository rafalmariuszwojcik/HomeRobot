namespace RobotControl.Command
{
  public interface IEncoderCommand : ICommand
  {
    int Index { get; set; }
    int Distance { get; set; }
    long Milis { get; set; }
    int EngineState { get; set; }
  }
}
