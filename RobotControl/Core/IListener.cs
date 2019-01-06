using RobotControl.Communication;

namespace RobotControl.Core
{
  public interface IListener<T>
    where T : class
  {
    void MessageReceived(IChannel channel, T data);
  }
}
