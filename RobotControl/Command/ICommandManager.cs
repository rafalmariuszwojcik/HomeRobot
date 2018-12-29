using RobotControl.Messages;

namespace RobotControl.Command
{
  public interface ICommandManager
  {
    void RegisterListener(ICommandListener listener);
    void UnregisterListener(ICommandListener listener);
    void CommandReceived(object sender, ICommand command);
  }
}