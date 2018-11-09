using System.ComponentModel;

namespace RobotControl.Communication
{
  [DefaultPropertyAttribute("Name")]
  public abstract class ConfigurationBase : IConfiguration
  {
    public string Name { get; set; }
  }
}
