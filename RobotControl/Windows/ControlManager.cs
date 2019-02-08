using RobotControl.Command;
using RobotControl.Core;
using RobotControl.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace RobotControl.Windows
{
  public class ControlManager : ManagerBase<ControlManager, Control, DataPackage>
  {
    private readonly IList<DisposableBase> dataSupply = new List<DisposableBase>();

    public ControlManager()
      : base(20)
    {
      dataSupply.Add(new MessageListener((s) => MessageReceived(null, new MessagePackage(s.FirstOrDefault()))));
      dataSupply.Add(new CommandListener((s) => MessageReceived(null, new CommandPackage(s.FirstOrDefault()))));
    }

    protected override void TearDown()
    {
      foreach (var disposable in dataSupply)
      {
        disposable.Dispose();
      }

      base.TearDown();
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