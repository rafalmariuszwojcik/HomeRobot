namespace RobotControl
{
  public interface IMessageReceiver
  {
    void MessageReceived(string message, object[] parameters);
  }
}
