using System.ComponentModel;
using System.Xml.Serialization;

namespace RobotControl.Communication
{
  [DefaultProperty("Name")]
  [XmlInclude(typeof(SerialConfiguration))]
  [XmlInclude(typeof(FakeConfiguration))]
  public abstract class ConfigurationBase : IConfiguration
  {
    public string Name { get; set; }
  }
}