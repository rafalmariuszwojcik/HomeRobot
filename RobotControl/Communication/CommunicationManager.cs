using System.Collections.Generic;
using System.Configuration;
using RobotControl.Core;
using RobotControl.Configuration;

namespace RobotControl.Communication
{
  public class CommunicationManager : Singleton<CommunicationManager>, ICommunicationManager
  {
    private readonly IList<IChannel> items = new List<IChannel>();
    public IEnumerable<IChannel> Items => items;

    public CommunicationManager()
    {
      Add(new Serial());
      Add(new Serial());
      Add(new Serial());
      Add(new Serial());
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
        items.Remove(channel);
      }
    }

    public void Save()
    {
      var x = new AppSettingsConfigurationRepository();
      x.WriteString("aaa", "bbb", "cccc");
      x.Flush();


      /*
      var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoaming);
      configFile.AppSettings.Settings.Add(new KeyValueConfigurationElement("aaa", "bbb"));
      configFile.Save();
      */
    }

    private void WriteString(string section, string ident, string value)
    {
      //var entry = this.GetEntry(section, ident);
      //if (entry == null)
      //{
        //this.configFile.AppSettings.Settings.Add(new KeyValueConfigurationElement(this.GetSettingsKey(section, ident), value));
     // }
     // else
      //{
        //entry.Value = value;
      //}
    }

  }
}