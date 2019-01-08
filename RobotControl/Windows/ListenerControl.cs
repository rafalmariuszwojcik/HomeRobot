using RobotControl.Command;
using RobotControl.Communication;
using RobotControl.Core;
using System.Windows.Forms;

namespace RobotControl.Windows
{
  public class ListenerControl : DisposableBase, IListener<string>, IListener<ICommand>
  {
    private readonly Control control;

    public ListenerControl(Control control)
    {
      this.control = control;
    }

    public void MessageReceived(IChannel channel, string data)
    {
      ProcessMessage(channel, data);
    }

    public void MessageReceived(IChannel channel, ICommand data)
    {
      ProcessMessage(channel, data);
    }

    protected override void Dispose(bool disposing)
    {
      ;
    }

    private void ProcessMessage<T>(IChannel channel, T data) where T: class
    {
      if (data == null)
      {
        return;
      }

      if (control.InvokeRequired)
      {
        //control.BeginInvoke(new Action<object, ICommand>(CommandReceived), new[] { sender, command });
      }
      else
      {
        //CommandReceived(sender, command);
      }
    }
  }
}