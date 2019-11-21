using RobotControl.Communication.Controller;
using RobotControl.Communication.Fake;
using RobotControl.Communication.Serial;
using System.ComponentModel;
using System.Xml.Serialization;

namespace RobotControl.Communication
{
  [DefaultProperty("Name")]
  [XmlInclude(typeof(SerialConfiguration))]
  [XmlInclude(typeof(FakeConfiguration))]
  [XmlInclude(typeof(ControllerConfiguration))]
  public abstract class ConfigurationBase : IConfiguration
  {
    public string Name { get; set; }
  }
}