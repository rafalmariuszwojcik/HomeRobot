using RobotControl.Command;
using RobotControl.Windows;
using System.Windows.Forms;

namespace RobotControl.Windows.Controls
{
  public abstract class BaseControl : UserControl
  {
    private readonly ListenerControl listener;

    public BaseControl()
    {
      listener = new ListenerControl(this);
      listener.OnMessageReceived += (s, e) => MessageReceived(s, e.Message);
      listener.OnCommandReceived += (s, e) => CommandReceived(s, e.Command);
      ControlManager.Instance.RegisterListener(this);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        ControlManager.Instance.UnregisterListener(this);
        listener?.Dispose();
      }

      base.Dispose(disposing);
    }

    protected virtual void MessageReceived(object sender, string message)
    {
    }

    protected virtual void CommandReceived(object sender, ICommand message)
    {
    }
  }
}