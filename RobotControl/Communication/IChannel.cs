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

  public interface IChannelMessage
  {
    IChannel Sender { get; }
    IEnumerable<IChannel> Receivers { get; }
  }

  public interface IChannelMessage<D> : IChannelMessage
    where D : class
  {
    D Data { get; }
  }

  public interface IDataReceivedEventArgs 
  {
    IChannelMessage Message { get; }
  }

  

  /// <summary>
  /// Data received event argument.
  /// </summary>
  /// <typeparam name="T">Type of received data.</typeparam>
  public interface IDataReceivedEventArgs<D>// : IDataReceivedEventArgs
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
    /// Gets or sets a value indicating whether this <see cref="IChannel"/> is active (opened).
    /// </summary>
    /// <value>
    /// <c>true</c> if active; otherwise, <c>false</c>.
    /// </value>
    bool Active { get; set; }

    /// <summary>
    /// Gets name of the channel.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the configuration.
    /// </summary>
    IConfiguration Configuration { get; }
  }

  public interface IChannel2 : IDisposable 
  {
    bool Active { get; set; }
    string Name { get; }
    IConfiguration Configuration { get; }
    bool Send(IChannelMessage data);
    bool Send(IEnumerable<IChannelMessage> data);
    event EventHandler<IDataReceivedEventArgs> DataReceived;
  }

  public interface IChannel2<D, C> : IChannel2
    where D : class
    where C : IConfiguration
  {
    bool Send(IChannelMessage<D> data);
    bool Send(IEnumerable<IChannelMessage<D>> data);
  }

  /// <summary>
  /// Communication channel interface.
  /// </summary>
  /// <typeparam name="D">Type of channel data.</typeparam>
  /// <typeparam name="C">Channel's configuration type.</typeparam>
  /// <seealso cref="System.IDisposable" />
  public interface IChannel<D, C> : IChannel
    where D : class
    where C : IConfiguration
  {
    /// <summary>
    /// Sends data through the channel.
    /// </summary>
    /// <param name="data">The data.</param>
    bool Send(D data);

    /// <summary>
    /// Sends data through the channel.
    /// </summary>
    /// <param name="data">The data.</param>
    bool Send(IEnumerable<D> data);

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