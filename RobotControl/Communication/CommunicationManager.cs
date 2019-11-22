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
  public class CommunicationManager : Singleton<CommunicationManager>, ICommunicationManager
  {
    private readonly IDictionary<Type, Func<ConfigurationBase, IChannel>> channelFromConfiguration = new Dictionary<Type, Func<ConfigurationBase, IChannel>>()
    {
      { typeof(SerialConfiguration), new Func<ConfigurationBase, IChannel>(x => new SerialChannel((SerialConfiguration)x)) },
      { typeof(FakeConfiguration), new Func<ConfigurationBase, IChannel>(x => new FakeChannel((FakeConfiguration)x)) },
      { typeof(ControllerConfiguration), new Func<ConfigurationBase, IChannel>(x => new GamePadController((ControllerConfiguration)x)) },
    };

    private readonly IList<IChannel> items = new List<IChannel>();
    public IEnumerable<IChannel> Items => items;

    public CommunicationManager()
    {
    }

    protected override void TearDown()
    {
      base.TearDown();
      foreach (var item in items)
      {
        item.Active = false;
      }
    }

    public void Add(IChannel channel)
    {
      if (!items.Contains(channel))
      {
        items.Add(channel);
      }
    }

    public void Remove(IChannel channel)
    {
      if (items.Contains(channel))
      {
        channel.Active = false;
        items.Remove(channel);
      }
    }

    public void Load()
    {
      var serializer = new XmlSerializer(typeof(CommunicationManagerConfiguration));
      using (var reader = new StreamReader(@"c:\temp\CommunicationManager.xml"))
      {
        var configuration = (CommunicationManagerConfiguration)serializer.Deserialize(reader);
        while (items.Any())
        {
          Remove(items[0]);
        }

        foreach (var cfg in configuration.Configurations)
        {
          Add(channelFromConfiguration[cfg.GetType()](cfg));
        }
      }
    }

    public void Save()
    {
      var list = items.Select(x => x.Configuration).Cast<ConfigurationBase>().ToArray();
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
  }
}