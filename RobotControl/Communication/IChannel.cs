using System;

namespace RobotControl.Communication
{
  public interface IChannel : IDisposable
  {
    void Open();
    void Close();
    void Send(string data);
    event EventHandler<IDataReceivedEventArgs> DataReceived;
  }

  public interface IDataReceivedEventArgs
  {
    string Data { get; }
  }

  public class DataReceivedEventArgs : IDataReceivedEventArgs
  {
    string data;

    public DataReceivedEventArgs(string data)
    {
      this.data = data;
    }

    public string Data
    {
      get { return data; }
    }
  }
}
