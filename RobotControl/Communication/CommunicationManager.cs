using RobotControl.Command;
using RobotControl.Command.Controller;
using RobotControl.Communication.Controller;
using RobotControl.Communication.Fake;
using RobotControl.Communication.Serial;
using RobotControl.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace RobotControl.Communication
{
  /// <summary>
  /// Global communication manager.
  /// </summary>
  public class CommunicationManager : ManagerBase<CommunicationManager, IListener, IChannelMessage>, ICommunicationManager
  {
    private readonly IDictionary<Type, Func<ConfigurationBase, IChannel>> channelFromConfiguration = new Dictionary<Type, Func<ConfigurationBase, IChannel>>()
    {
      { typeof(SerialConfiguration), new Func<ConfigurationBase, IChannel>(x => new SerialChannel((SerialConfiguration)x)) },
      { typeof(FakeConfiguration), new Func<ConfigurationBase, IChannel>(x => new FakeChannel((FakeConfiguration)x)) },
      { typeof(ControllerConfiguration), new Func<ConfigurationBase, IChannel>(x => new GamePadController((ControllerConfiguration)x)) },
    };

    /// <summary>
    /// Internal list of channels.
    /// </summary>
    private readonly IList<IChannel> channels = new List<IChannel>();

    /// <summary>
    /// Gets available channels.
    /// </summary>
    public IEnumerable<IChannel> Items => channels;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommunicationManager"/> class.
    /// </summary>
    public CommunicationManager()
    {
    }

    /// <summary>
    /// Tears down the instance.
    /// </summary>
    /// <remarks>Close all channels.</remarks>
    protected override void TearDown()
    {
      base.TearDown();
      while (channels.Any())
      {
        Remove(channels[0]);
      }
    }

    /// <summary>
    /// Adds the specified channel.
    /// </summary>
    /// <param name="channel">The channel.</param>
    public void Add(IChannel channel)
    {
      if (!channels.Contains(channel))
      {
        channels.Add(channel);
        channel.DataReceived += DataReceivedFromChannel;
        RegisterListener(channel as IListener);
      }
    }

    /// <summary>
    /// Removes the specified channel.
    /// </summary>
    /// <param name="channel">The channel.</param>
    public void Remove(IChannel channel)
    {
      if (channels.Contains(channel))
      {
        UnregisterListener(channel as IListener);
        channel.DataReceived -= DataReceivedFromChannel;
        channel.Active = false;
        channel.Dispose();
        channels.Remove(channel);
      }
    }

    public void Load()
    {
      var serializer = new XmlSerializer(typeof(CommunicationManagerConfiguration));
      using (var reader = new StreamReader(@"c:\temp\CommunicationManager.xml"))
      {
        var configuration = (CommunicationManagerConfiguration)serializer.Deserialize(reader);
        while (channels.Any())
        {
          Remove(channels[0]);
        }

        foreach (var cfg in configuration.Configurations)
        {
          Add(channelFromConfiguration[cfg.GetType()](cfg));
        }
      }
    }

    public void Save()
    {
      var list = channels.Select(x => x.Configuration).Cast<ConfigurationBase>().ToArray();
      //var ser = new XmlSerializer(typeof(ConfigurationBase[]));


      var xy = new CommunicationManagerConfiguration(list);
      var ser = new XmlSerializer(typeof(CommunicationManagerConfiguration));
      using (var writer = new StreamWriter(@"c:\temp\CommunicationManager.xml"))
      {
        ser.Serialize(writer, xy);
      }



      /*
      var x = new AppSettingsConfigurationRepository();
      x.WriteString("aaa", "bbb", "cccc");
      x.Flush();
      */

      /*
      var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoaming);
      configFile.AppSettings.Settings.Add(new KeyValueConfigurationElement("aaa", "bbb"));
      configFile.Save();
      */
    }

    public class CommunicationManagerConfiguration
    {
      public CommunicationManagerConfiguration()
      {
      }

      public CommunicationManagerConfiguration(ConfigurationBase[] configurations)
      {
        Configurations = configurations;
      }

      public ConfigurationBase[] Configurations { get; set; }
    }

    /// <summary>
    /// Sends the data actions.
    /// </summary>
    /// <param name="listener">The listener.</param>
    /// <param name="data">The data.</param>
    /// <returns></returns>
    protected override IEnumerable<Action> SendDataActions(IListener listener, IEnumerable<IChannelMessage> data)
    {
      var result = ListenerHelper.Instance.DataReceivedActions(listener, data);
      return result;
    }

    /// <summary>
    /// Data received from channel.
    /// </summary>
    /// <param name="sender">The sender object.</param>
    /// <param name="e">The <see cref="IDataReceivedEventArgs"/> instance containing the event data.</param>
    private void DataReceivedFromChannel(object sender, IDataReceivedEventArgs e) 
    {
      BroadcastData(this, e.Message);
      if (e.Message is IChannelMessage<IControllerCommand> command) 
      {
        var data = new List<ICommand>();
        data.Add(command.Data);
        CommandManager.Instance.BroadcastData(this, data);
      }
    }
  }
}