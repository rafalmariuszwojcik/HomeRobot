namespace RobotControl.Command
{
  public interface IEngineCommand : ICommand
  {
    int Index { get; set; }
  }
}
