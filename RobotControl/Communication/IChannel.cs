using System;
using System.Collections.Generic;

namespace RobotControl.Communication
{
  /// <summary>
  /// Channel configuration interface.
  /// </summary>
  public interface IConfiguration
  {
    /// <summary>
    /// Gets or sets configuration's name.
    /// </summary>
    string Name { get; set; }
  }

  /// <summary>
  /// Data received event argument.
  /// </summary>
  /// <typeparam name="T">Type of received data.</typeparam>
  public interface IDataReceivedEventArgs<D>
    where D : class
  {
    /// <summary>
    /// Gets the received data.
    /// </summary>
    D Data { get; }
  }

  public interface IChannel : IDisposable
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
  public interface IChannel<D, C> : IChannel
    where D : class
    where C : IConfiguration
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
    event EventHandler<IDataReceivedEventArgs<D>> DataReceived;
  }

  /// <summary>
  /// Data received event data.
  /// </summary>
  /// <typeparam name="D"></typeparam>
  /// <seealso cref="System.EventArgs" />
  /// <seealso cref="RobotControl.Communication.IDataReceivedEventArgs{D}" />
  public class DataReceivedEventArgs<D> : EventArgs, IDataReceivedEventArgs<D>
    where D : class
  {
    public DataReceivedEventArgs(D data)
    {
      Data = data;
    }

    public D Data { get; }
  }
}