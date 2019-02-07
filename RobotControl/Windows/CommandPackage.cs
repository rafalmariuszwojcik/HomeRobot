using RobotControl.Command;

namespace RobotControl.Windows
{
  public class CommandPackage : DataPackage
  {
    public CommandPackage(ICommand command)
    {
      Command = command;
    }

    public ICommand Command { get; }
  }
}
