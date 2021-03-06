﻿namespace RobotControl.Messages
{
  public interface IMessageManager
  {
    void RegisterListener(IMessageListener listener);
    void UnregisterListener(IMessageListener listener);
    void MessageReceived(object sender, string message);
  }
}
