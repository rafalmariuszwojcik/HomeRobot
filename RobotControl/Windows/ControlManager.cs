using RobotControl.Command;
using RobotControl.Core;
using RobotControl.Messages;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace RobotControl.Windows
{
  public class ControlManager : ManagerBase<ControlManager, Control, DataPackage>
  {
    public ControlManager()
      : base(20)
    {
      Disposables.Add(new MessageListener((s) => DataReceived(null, new MessagePackage(s.FirstOrDefault()))));
      Disposables.Add(new CommandListener((s) => DataReceived(null, new CommandPackage(s.FirstOrDefault()))));
    }

    protected override void SendData(Control listener, Action action)
    {
      if (listener.InvokeRequired)
      {
        try
        {
          listener.Invoke(action);
        }
        catch (ObjectDisposedException)
        {
          ;
        }
        catch (InvalidAsynchronousStateException)
        {
          ;
        }
      }
      else
      {
        base.SendData(listener, action);
      }
    }
  }
}