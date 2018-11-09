using RobotControl.Command;
using System;

namespace RobotControl.Communication
{
  public interface IChannel<U> : IDisposable
    where U: ConfigurationBase
  {
    void Open();
    void Close();
    void Send(ICommand[] commands);
    event EventHandler<IDataReceivedEventArgs> DataReceived;
  }

  public interface IDataReceivedEventArgs
  {
    string Data { get; }
  }

  public class DataReceivedEventArgs : IDataReceivedEventArgs
  {
    public DataReceivedEventArgs(string data)
    {
      Data = data;
    }

    public string Data { get; }
  }

  public interface IConfiguration
  {
    string Name { get; set; }
  }
}
