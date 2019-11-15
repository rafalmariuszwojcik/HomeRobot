using RobotControl.Command;
using System;

namespace RobotControl.Communication
{
  public interface IDataReceivedEventArgs2<T>
    where T: class
  {
    T Data { get; }
  }

  public interface IChanell2<T, C> : IDisposable 
    where T: class
    where C: IConfiguration
  {
    void Open();
    void Close();
    void Send(T data);
    event EventHandler<IDataReceivedEventArgs2<T>> DataReceived;
    bool Active { get; set; }
    string Name { get; }
    C Configuration { get; }
  }

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