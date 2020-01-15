using RobotControl.Communication;
using RobotControl.Core;
using RobotControl.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotControl.Command
{
  /// <summary>
  /// Global commands manager.
  /// </summary>
  /// <seealso cref="RobotControl.Core.ManagerBase{RobotControl.Command.CommandManager, RobotControl.Command.ICommandListener, RobotControl.Command.ICommand}" />
  public class CommandManager : ManagerBase<CommandManager, ICommandListener, ICommand>, IListener, IListener<ICommand>
  {
    private readonly StringBuilder incomingData = new StringBuilder();

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandManager"/> class.
    /// </summary>
    public CommandManager()
      : base()
    {
      Disposables.Add(new MessageListener((s) => MessageReceived(null, s.FirstOrDefault())));
      CommunicationManager.Instance.RegisterListener(this);
    }

    /// <summary>
    /// Tears down.
    /// </summary>
    protected override void TearDown()
    {
      CommunicationManager.Instance.UnregisterListener(this);
      base.TearDown();
    }

    void IListener<ICommand>.DataReceived(IChannel channel, IEnumerable<ICommand> data)
    {
      BroadcastData(this, data);
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
              BroadcastData(channel, new[] { command });
            }
          }
        }
      }
    }
  }
}