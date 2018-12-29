using RobotControl.Messages;
using System;
using WeifenLuo.WinFormsUI.Docking;

namespace RobotControl.Forms
{
  public class BaseView : DockContent, IMessageListener
  {
    public BaseView()
    {
      Text = GetType().Name;
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

    protected virtual void MessageReceived(string message)
    {
    }

    void IMessageListener.MessageReceived(string message)
    {
      message = !string.IsNullOrWhiteSpace(message) ? message.Trim() : null;
      if (string.IsNullOrWhiteSpace(message))
      {
        return;
      }

      if (InvokeRequired)
      {
        Invoke(new Action<string>(MessageReceived), new[] { message });
      }
      else
      {
        MessageReceived(message);
      }
    }
  }
}