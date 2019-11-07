namespace RobotControl.Command
{
  public interface IEncoderCommand : ICommand
  {
    int Index { get; set; }
    double Distance { get; set; }
    long Milis { get; set; }
    int EngineState { get; set; }
  }
}
