namespace RobotControl.Windows
{
  public class MessagePackage : DataPackage
  {
    public MessagePackage(string message)
    {
      Message = message;
    }

    public string Message { get; }
  }
}
