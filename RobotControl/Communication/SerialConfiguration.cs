using System.Xml.Serialization;

namespace RobotControl.Communication
{
  public class SerialConfiguration : ConfigurationBase
  {
    public SerialConfiguration()
    {
      Port = "COM3";
      BaudRate = 115200;
    }

    public string Port { get; set; }
    public int BaudRate { get; set; }
  }
}
