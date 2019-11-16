namespace RobotControl.Command
{
  public interface ICommandManager
  {
    void RegisterListener(ICommandListener listener);
    void UnregisterListener(ICommandListener listener);
  }
}