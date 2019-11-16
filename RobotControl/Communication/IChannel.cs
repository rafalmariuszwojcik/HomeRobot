using RobotControl.Command;
using System;
using System.Collections.Generic;

namespace RobotControl.Communication
{
  /// <summary>
  /// Data received event argument.
  /// </summary>
  /// <typeparam name="T">Type of received data.</typeparam>
  public interface IDataReceivedEventArgsEx<D>
    where D: class
  {
    /// <summary>
    /// Gets the received data.
    /// </summary>
    D Data { get; }
  }

  public interface IChanellExBase : IDisposable
  {
    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="IChanellEx{T, C}"/> is active (opened).
    /// </summary>
    /// <value>
    /// <c>true</c> if active; otherwise, <c>false</c>.
    /// </value>
    bool Active { get; set; }

    /// <summary>
    /// Gets name of the chanell.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the configuration.
    /// </summary>
    IConfiguration Configuration { get; }
  }
  
  /// <summary>
  /// Communication chanell interface.
  /// </summary>
  /// <typeparam name="D">Type of chanell data.</typeparam>
  /// <typeparam name="C">Chanell's configuration type.</typeparam>
  /// <seealso cref="System.IDisposable" />
  public interface IChanellEx<D, C> : IChanellExBase
    where D: class
    where C: IConfiguration
  {
    /// <summary>
    /// Gets the chanell's configuration.
    /// </summary>
    //C Configuration { get; }

    /// <summary>
    /// Sends data through the channel.
    /// </summary>
    /// <param name="data">The data.</param>
    void Send(D data);

    /// <summary>
    /// Sends data through the channel.
    /// </summary>
    /// <param name="data">The data.</param>
    void Send(IEnumerable<D> data);

    /// <summary>
    /// Occurs when incoming data received.
    /// </summary>
    event EventHandler<IDataReceivedEventArgsEx<D>> DataReceived;
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

  public interface IChannel_old<U> : IChannel where U: ConfigurationBase
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

  public class DataReceivedEventArgsEx<D> : EventArgs, IDataReceivedEventArgsEx<D>
    where D : class
  {
    public DataReceivedEventArgsEx(D data)
    {
      Data = data;
    }

    public D Data { get; }
  }

  public interface IConfiguration
  {
    string Name { get; set; }
  }
}