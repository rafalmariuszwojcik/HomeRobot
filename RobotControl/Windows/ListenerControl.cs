using RobotControl.Command;
using RobotControl.Communication;
using RobotControl.Core;
using RobotControl.Messages;
using System;
using System.Windows.Forms;

namespace RobotControl.Windows
{
  public class ListenerControl : DisposableBase, IMessageListener, ICommandListener 
  {
    private readonly Control control;

    public ListenerControl(Control control)
    {
      this.control = control;
      MessageManager.Instance.RegisterListener(this);
      CommandManager.Instance.RegisterListener(this);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        CommandManager.Instance.UnregisterListener(this);
        MessageManager.Instance.UnregisterListener(this);
      }
    }

    public event EventHandler<MessageEventArgs> OnMessageReceived;
    public event EventHandler<CommandEventArgs> OnCommandReceived;
      
    private void ProcessMessage<T>(IChannel channel, T data, Action<IChannel, T> action) where T: class
    {
      if (data == null)
      {
        return;
      }

      if (control.InvokeRequired)
      {
        control.BeginInvoke(action, new[] { (object)channel, data });
      }
      else
      {
        action(channel, data);
      }
    }

    private void InvokeOnMessageReceived(IChannel channel, string data)
    {
      OnMessageReceived?.Invoke(this, new MessageEventArgs(channel, data));
    }

    private void InvokeOnCommandReceived(IChannel channel, ICommand command)
    {
      OnCommandReceived?.Invoke(this, new CommandEventArgs(channel, command));
    }

    void IListener<string>.MessageReceived(IChannel channel, string data)
    {
      ProcessMessage(channel, data, (c, d) => InvokeOnMessageReceived(c, d));
    }

    void IListener<ICommand>.MessageReceived(IChannel channel, ICommand data)
    {
      ProcessMessage(channel, data, (c, d) => InvokeOnCommandReceived(c, d));
    }
  }

  public abstract class MessageEventBaseArgs : EventArgs
  {
    public MessageEventBaseArgs(IChannel channel)
    {
      Channel = channel;
    }

    public IChannel Channel { get; }
  }

  public class MessageEventArgs : MessageEventBaseArgs
  {
    public MessageEventArgs(IChannel channel, string message) : base(channel)
    {
      Message = message;
    }

    public string Message { get; }
  }

  public class CommandEventArgs : MessageEventBaseArgs
  {
    public CommandEventArgs(IChannel channel, ICommand command) : base(channel)
    {
      Command = command;
    }

    public ICommand Command { get; }
  }
}