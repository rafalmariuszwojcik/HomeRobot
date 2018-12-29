using RobotControl.Command;
using RobotControl.Messages;
using System;
using WeifenLuo.WinFormsUI.Docking;

namespace RobotControl.Views
{
  public class BaseView : DockContent, IMessageListener, ICommandListener
  {
    private readonly bool listenMessages = true;
    private readonly bool listenCommands = true;

    public BaseView()
    {
      Text = GetType().Name;
      MessageManager.Instance.RegisterListener(this);
      CommandManager.Instance.RegisterListener(this);
    }

    public BaseView(bool listenMessages, bool listenCommands) : this()
    {
      this.listenMessages = listenMessages;
      this.listenCommands = listenCommands;
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

    protected virtual void CommandReceived(object sender, ICommand command)
    {
    }

    void ICommandListener.CommandReceived(object sender, ICommand command)
    {
      if (!listenCommands || command == null)
      {
        return;
      }

      if (InvokeRequired)
      {
        BeginInvoke(new Action<object, ICommand>(CommandReceived), new[] { sender, command });
      }
      else
      {
        CommandReceived(sender, command);
      }
    }

    void IMessageListener.MessageReceived(object sender, string message)
    {
      if (!listenMessages)
      {
        return;
      }

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