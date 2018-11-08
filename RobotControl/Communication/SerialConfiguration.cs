namespace RobotControl.Communication
{
  public class SerialConfiguration : ConfigurationBase
  {
    public string Port { get; set; }

    public int BaudRate { get; set; }
  }
}
