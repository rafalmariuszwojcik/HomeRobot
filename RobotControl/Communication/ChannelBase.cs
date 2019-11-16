using RobotControl.Core;
using System;
using System.Collections.Generic;

namespace RobotControl.Communication
{
  /// <summary>
  /// Base class for communication changels.
  /// </summary>
  /// <typeparam name="T">Channel type.</typeparam>
  /// <typeparam name="D">Channel's data type.</typeparam>
  /// <typeparam name="C">Configuration type.</typeparam>
  /// <seealso cref="RobotControl.Core.DisposableBase" />
  /// <seealso cref="RobotControl.Communication.IChannel{D, C}" />
  public abstract class ChannelBase<T, D, C> : DisposableBase, IChannel<D, C>
    where T : IDisposable
    where D : class
    where C : IConfiguration
  {
    /// <summary>
    /// The lock data object.
    /// </summary>
    private readonly object lockData = new object();

    /// <summary>
    /// The channel object.
    /// </summary>
    private T channel;

    /// <summary>
    /// Occurs when incoming data received.
    /// </summary>
    public event EventHandler<IDataReceivedEventArgs<D>> DataReceived;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChannelBaseEx{T, D, C}"/> class.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    public ChannelBase(C configuration)
    {
      Configuration = configuration;
    }

    /// <summary>
    /// Gets name of the channel.
    /// </summary>
    public string Name { get => GetType().Name; }

    /// <summary>
    /// Gets the channel's configuration.
    /// </summary>
    //public C Configuration { get; private set; }
    public IConfiguration Configuration { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="T:RobotControl.Communication.IChannel" /> is active (opened).
    /// </summary>
    /// <value>
    /// <c>true</c> if active; otherwise, <c>false</c>.
    /// </value>
    public bool Active
    {
      get
      {
        lock (lockData)
        {
          return channel != null;
        }
      }

      set
      {
        lock (lockData)
        {
          if (Active != value)
          {
            if (value)
            {
              Open();
            }
            else
            {
              Close();
            }
          }
        }
      }
    }

    /// <summary>
    /// Sends data through the channel.
    /// </summary>
    /// <param name="data">The data.</param>
    public void Send(D data)
    {
      lock (lockData)
      {
        if (!Active)
        {
          throw new NotImplementedException();
        }

        InternalSend(channel, data);
      }
    }

    /// <summary>
    /// Sends data through the channel.
    /// </summary>
    /// <param name="data">The data.</param>
    public void Send(IEnumerable<D> data)
    {
      foreach (var item in data)
      {
        Send(item);
      }
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        Active = false;
      }
    }

    /// <summary>
    /// Called when data received from channel.
    /// </summary>
    /// <param name="data">The data.</param>
    protected void OnDataReceived(D data)
    {
      DataReceived?.Invoke(this, new DataReceivedEventArgs<D>(data));
      //MessageManager.Instance.DataReceived(this, new[] { data });
    }

    /// <summary>
    /// Internal open communication channel method.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <returns>Reference to communication object.</returns>
    protected abstract T InternalOpen(C configuration);

    /// <summary>
    /// Internal close communication channel.
    /// </summary>
    /// <param name="channel">The channel.</param>
    protected abstract void InternalClose(T channel);

    /// <summary>
    /// Internal send data through the channel.
    /// </summary>
    /// <param name="channel">The channel.</param>
    /// <param name="commands">The commands.</param>
    protected abstract void InternalSend(T channel, D data);

    /// <summary>
    /// Opens the channel.
    /// </summary>
    private void Open()
    {
      Close();
      try
      {
        channel = InternalOpen((C)Configuration);
      }
      catch (Exception)
      {
        Close();
        throw;
      }
    }

    /// <summary>
    /// Closes the channel.
    /// </summary>
    private void Close()
    {
      if (channel != null)
      {
        InternalClose(channel);
        channel.Dispose();
        channel = default;
      }
    }
  }
}