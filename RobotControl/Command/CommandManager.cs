using RobotControl.Core;
using RobotControl.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RobotControl.Command
{
  public class CommandManager : Singleton<CommandManager>, ICommandManager, IMessageListener
  {
    private readonly object lockListeners = new object();
    private readonly object lockCommands = new object();
    private readonly IList<ICommandListener> listeners = new List<ICommandListener>();
    private readonly Queue<ICommand> commands = new Queue<ICommand>();
    private readonly StringBuilder incomingData = new StringBuilder();
    private readonly CancellationTokenSource tokenSource;
    private readonly CancellationToken token;
    private readonly Task commandWorker;
    private readonly ManualResetEvent signal;
    
    public CommandManager()
    {
      tokenSource = new CancellationTokenSource();
      token = tokenSource.Token;
      signal = new ManualResetEvent(true);
      commandWorker = Task.Run(() => ProcessCommands(), token);
      MessageManager.Instance.RegisterListener(this);
    }

    protected override void TearDown()
    {
      MessageManager.Instance.UnregisterListener(this);

      if (tokenSource != null)
      {
        tokenSource.Cancel();
        signal.Set();
        if (commandWorker != null)
        {
          commandWorker.Wait();
        }
        
        tokenSource.Dispose();
        signal.Dispose();
      }

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

    private void CommandReceived(object sender, ICommand command)
    {
      lock (lockListeners)
      {
        Parallel.ForEach(listeners, (listener) => {
          listener.CommandReceived(sender, command);
        });
      }
    }

    private void MessageReceived(object sender, string message)
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
              lock (lockCommands)
              {
                commands.Enqueue(command);
                signal.Set();
              }
            }
          }
        }
      }
    }

    private void ProcessCommands()
    {
      ICommand command = null;
      token.ThrowIfCancellationRequested();
      while (true)
      {
        signal.WaitOne();
        do
        {
          lock (lockCommands)
          {
            command = commands.Any() ? commands.Dequeue() : null;
          }

          if (command != null)
          {
            CommandReceived(null, command);
          }

          if (token.IsCancellationRequested)
          {
            token.ThrowIfCancellationRequested();
          }
        }
        while (command != null);
        signal.Reset();
      }
    }

    void IMessageListener.MessageReceived(object sender, string message)
    {
      MessageReceived(sender, message);
    }
  }
}