using RobotControl.Command;
using RobotControl.Communication;
using RobotControl.Core;
using System;
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
      ProcessMessage(channel, data, (c, d) => InvokeOnMessageReceived(c, d));
    }

    public void MessageReceived(IChannel channel, ICommand data)
    {
      ProcessMessage(channel, data, (c, d) => InvokeOnCommandReceived(c, d));
    }

    public event EventHandler<MessageEventArgs> OnMessageReceived;
    public event EventHandler<CommandEventArgs> OnCommandReceived;

    protected override void Dispose(bool disposing)
    {
      ;
    }

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