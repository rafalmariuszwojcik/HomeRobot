using RobotControl.Core;
using RobotControl.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RobotControl.Command
{
  public class CommandManager : Singleton<CommandManager>, ICommandManager, IMessageListener
  {
    private readonly object lockObject = new object();
    private readonly IList<ICommandListener> listeners = new List<ICommandListener>();

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
      ;
    }
  }
}