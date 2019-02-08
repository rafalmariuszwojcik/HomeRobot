using RobotControl.Core;
using System;
using System.Collections.Generic;

namespace RobotControl.Command
{
  public class CommandListener : ListenerBase<ICommand>, ICommandListener
  {
    public CommandListener(Action<IEnumerable<ICommand>> action) : base(action)
    {
      CommandManager.Instance.RegisterListener(this);
    }

    protected override void Dispose(bool disposing)
    {
      CommandManager.Instance.UnregisterListener(this);
    }
  }
}
