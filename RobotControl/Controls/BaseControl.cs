using RobotControl.Messages;
using System;
using System.Windows.Forms;

namespace RobotControl.Controls
{
  public class BaseControl : UserControl, IMessageListener
  {
    public BaseControl()
    {
      MessageManager.Instance.RegisterListener(this);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        MessageManager.Instance.UnregisterListener(this);
      }

      base.Dispose(disposing);
    }

    protected virtual void MessageReceived(object sender, string message)
    {
    }

    void IMessageListener.MessageReceived(object sender, string message)
    {
      message = !string.IsNullOrWhiteSpace(message) ? message.Trim() : null;
      if (string.IsNullOrWhiteSpace(message))
      {
        return;
      }

      if (InvokeRequired)
      {
        BeginInvoke(new Action<object, string>(MessageReceived), new[] { sender, message });
      }
      else
      {
        MessageReceived(sender, message);
      }
    }
  }
}