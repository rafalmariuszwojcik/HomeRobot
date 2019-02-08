using RobotControl.Communication;
using RobotControl.Core;
using RobotControl.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotControl.Command
{
  public class CommandManager : Singleton<CommandManager>, ICommandManager, IMessageListener
  {
    private readonly object lockListeners = new object();
    private readonly IList<ICommandListener> listeners = new List<ICommandListener>();
    private readonly StringBuilder incomingData = new StringBuilder();
    private readonly DataProcessingQueue<ICommand> commandQueue;

    public CommandManager()
    {
      commandQueue = new DataProcessingQueue<ICommand>(x => CommandReceived(null, x));
      MessageManager.Instance.RegisterListener(this);
    }

    protected override void TearDown()
    {
      MessageManager.Instance.UnregisterListener(this);
      commandQueue?.Dispose();
      base.TearDown();
    }

    public void RegisterListener(ICommandListener listener)
    {
      lock (lockListeners)
      {
        if (listener != null && !listeners.Contains(listener))
        {
          listeners.Add(listener);
        }
      }
    }

    public void UnregisterListener(ICommandListener listener)
    {
      lock (lockListeners)
      {
        if (listeners.Contains(listener))
        {
          listeners.Remove(listener);
        }
      }
    }

    private void CommandReceived(object sender, IEnumerable<ICommand> commands)
    {
      foreach (var command in commands)
      {
        lock (lockListeners)
        {
          Parallel.ForEach(listeners, (listener) => {
            listener.MessageReceived((IChannel)sender, command);
          });
        }
      }
    }

    private void MessageReceived(IChannel channel, string message)
    {
      if (string.IsNullOrWhiteSpace(message))
      {
        return;
      }

      incomingData.Append(message.ToUpper().Replace("\r", string.Empty).Replace("\n", string.Empty));
      var data = incomingData.ToString();
      var index = data.LastIndexOf(';');
      if (index >= 0)
      {
        data = data.Substring(0, index + 1);
        incomingData.Remove(0, index + 1);
        var commandLines = data.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var commandLine in commandLines)
        {
          var commandParts = commandLine.Split(',');
          if (commandParts.Any())
          {
            var commandId = commandParts[0];
            var commandParameters = commandParts.Length > 1 ? new string[commandParts.Length - 1] : null;
            if (commandParameters != null)
            {
              Array.Copy(commandParts, 1, commandParameters, 0, commandParameters.Length);
            }

            var command = CommandFactory.CreateCommand(commandId, commandParameters);
            if (command != null)
            {
              commandQueue.Enqueue(command);
            }
          }
        }
      }
    }

    void IListener<string>.MessageReceived(IChannel channel, string data)
    {
      MessageReceived(channel, data);
    }

    /*
    void IMessageListener.MessageReceived(object sender, string message)
    {
      MessageReceived(sender, message);
    }
    */
  }
}