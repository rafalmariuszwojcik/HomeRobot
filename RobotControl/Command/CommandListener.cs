using RobotControl.Core;
using System;

namespace RobotControl.Command
{
  public class CommandListener : ListenerBase<ICommand>, ICommandListener
  {
    public CommandListener(Action<ICommand> action) : base(action)
    {
      CommandManager.Instance.RegisterListener(this);
    }

    protected override void Dispose(bool disposing)
    {
      CommandManager.Instance.UnregisterListener(this);
    }
  }
}
