using RobotControl.Command;
using System;

namespace RobotControl.Communication
{
  public interface IChannel : IDisposable
  {
    void Open();
    void Close();
    void Send(ICommand[] commands);
    void Send(string data);
    event EventHandler<DataReceivedEventArgs> DataReceived;
    bool Active { get; set; }
    string Name { get; }
    IConfiguration Configuration { get; }
  }

  public interface IChannel<U> : IChannel where U: ConfigurationBase
  {
  }

  public interface IDataReceivedEventArgs
  {
    string Data { get; }
  }

  public class DataReceivedEventArgs : EventArgs, IDataReceivedEventArgs
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