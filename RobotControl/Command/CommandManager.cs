using RobotControl.Core;
using RobotControl.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RobotControl.Command
{
  public class CommandManager : Singleton<CommandManager>, ICommandManager, IMessageListener
  {
    private readonly object lockObject = new object();
    private readonly IList<ICommandListener> listeners = new List<ICommandListener>();
    private readonly StringBuilder incomingData = new StringBuilder();

    public CommandManager()
    {
      MessageManager.Instance.RegisterListener(this);
    }

    protected override void TearDown()
    {
      MessageManager.Instance.UnregisterListener(this);
      base.TearDown();
    }

    public void RegisterListener(ICommandListener listener)
    {
      lock (lockObject)
      {
        if (listener != null && !listeners.Contains(listener))
        {
          listeners.Add(listener);
        }
      }
    }

    public void UnregisterListener(ICommandListener listener)
    {
      lock (lockObject)
      {
        if (listeners.Contains(listener))
        {
          listeners.Remove(listener);
        }
      }
    }

    public void CommandReceived(object sender, ICommand command)
    {
      lock (lockObject)
      {
        Parallel.ForEach(listeners, (listener) => {
          listener.CommandReceived(sender, command);
        });
      }
    }

    void IMessageListener.MessageReceived(object sender, string message)
    {
      if (string.IsNullOrWhiteSpace(message))
      {
        return;
      }

      incomingData.Append(message.ToUpper());
      var data = incomingData.ToString();
      var index = data.LastIndexOf(';');
      if (index >= 0)
      {
        data = data.Substring(0, index + 1);
        incomingData.Remove(0, index + 1);
        var commandLines = data.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var commandLine in commandLines)
        {


        }
      }
    }
  }
}