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
  /// Channel message interface.
  /// </summary>
  public interface IChannelMessage
  {
    /// <summary>
    /// Gets the sender.
    /// </summary>
    IChannel Sender { get; }

    /// <summary>
    /// Gets the receivers.
    /// </summary>
    IEnumerable<IChannel> Receivers { get; }
  }

  /// <summary>
  /// Channel message interface.
  /// </summary>
  /// <typeparam name="D">Type of message data.</typeparam>
  public interface IChannelMessage<D> : IChannelMessage
    where D : class
  {
    /// <summary>
    /// Gets the data.
    /// </summary>
    D Data { get; }
  }

  /// <summary>
  /// Data received event argument.
  /// </summary>
  public interface IDataReceivedEventArgs 
  {
    /// <summary>
    /// Gets the message.
    /// </summary>
    IChannelMessage Message { get; }
  }

  /// <summary>
  /// Data received event argument.
  /// </summary>
  /// <typeparam name="T">Type of received data.</typeparam>
  public interface IDataReceivedEventArgs<D> : IDataReceivedEventArgs
    where D : class
  {
    /// <summary>
    /// Gets the message.
    /// </summary>
    new IChannelMessage<D> Message { get; }
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

    /// <summary>
    /// Sends the specified data.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <returns><c>True</c> if data was sent successfully.</returns>
    bool Send(IChannelMessage data);

    /// <summary>
    /// Occurs when data received from channel.
    /// </summary>
    event EventHandler<IDataReceivedEventArgs> DataReceived;
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
  }

  /// <summary>
  /// Channel message class.
  /// </summary>
  /// <seealso cref="RobotControl.Communication.IChannelMessage" />
  public class ChannelMessage : IChannelMessage
  {
    /// <summary>
    /// The message receivers.
    /// </summary>
    private readonly IEnumerable<IChannel> receivers = new List<IChannel>();

    /// <summary>
    /// Initializes a new instance of the <see cref="ChannelMessage"/> class.
    /// </summary>
    /// <param name="sender">The sender.</param>
    public ChannelMessage(IChannel sender) 
    {
      Sender = sender;
    }

    /// <summary>
    /// Gets the message sender.
    /// </summary>
    public IChannel Sender { get; private set; }

    /// <summary>
    /// Gets the receivers.
    /// </summary>
    public IEnumerable<IChannel> Receivers => receivers;
  }

  /// <summary>
  /// Channel message class.
  /// </summary>
  /// <typeparam name="D"></typeparam>
  /// <seealso cref="RobotControl.Communication.IChannelMessage" />
  public class ChannelMessage<D> : ChannelMessage, IChannelMessage<D>
    where D : class
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ChannelMessage{D}"/> class.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="data">The data.</param>
    public ChannelMessage(IChannel sender, D data)
      : base(sender)
    {
      Data = data;
    }

    /// <summary>
    /// Gets the data.
    /// </summary>
    public D Data { get; private set; }
  }

  /// <summary>
  /// Data received event argument class.
  /// </summary>
  /// <seealso cref="System.EventArgs" />
  /// <seealso cref="RobotControl.Communication.IDataReceivedEventArgs" />
  public class DataReceivedEventArgs : EventArgs, IDataReceivedEventArgs
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="DataReceivedEventArgs"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    public DataReceivedEventArgs(IChannelMessage message)
    {
      Message = message;
    }

    /// <summary>
    /// Gets the message.
    /// </summary>
    public IChannelMessage Message { get; private set; }
  }

  /// <summary>
  /// Data received event data.
  /// </summary>
  /// <typeparam name="D"></typeparam>
  /// <seealso cref="System.EventArgs" />
  /// <seealso cref="RobotControl.Communication.IDataReceivedEventArgs{D}" />
  public class DataReceivedEventArgs<D> : DataReceivedEventArgs, IDataReceivedEventArgs<D>
    where D : class
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="DataReceivedEventArgs{D}"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="data">The data.</param>
    public DataReceivedEventArgs(IChannelMessage<D> message)
      :base(message)
    {
    }

    /// <summary>
    /// Gets the message.
    /// </summary>
    public new IChannelMessage<D> Message => (IChannelMessage<D>)base.Message;
  }
}