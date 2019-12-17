using RobotControl.Core;
using System;
using System.Collections.Generic;

namespace RobotControl.Communication
{
  public abstract class ChannelBase2<T, D, C> : DisposableBase, IChannel2<D, C>
    where T : IDisposable
    where D : class
    where C : IConfiguration
  {
    public bool Active { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public string Name => throw new NotImplementedException();

    public IConfiguration Configuration => throw new NotImplementedException();

    public event EventHandler<IDataReceivedEventArgs> DataReceived;

    public bool Send(IChannelMessage<D> data)
    {
      throw new NotImplementedException();
    }

    public bool Send(IEnumerable<IChannelMessage<D>> data)
    {
      throw new NotImplementedException();
    }

    public bool Send(IChannelMessage data)
    {
      throw new NotImplementedException();
    }

    public bool Send(IEnumerable<IChannelMessage> data)
    {
      throw new NotImplementedException();
    }
  }

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
    protected ChannelBase(C configuration)
    {
      Configuration = configuration;
    }

    event EventHandler<IDataReceivedEventArgs<D>> IChannel<D, C>.DataReceived
    {
      add
      {
        throw new NotImplementedException();
      }

      remove
      {
        throw new NotImplementedException();
      }
    }

    event EventHandler<IDataReceivedEventArgs> IChannel.DataReceived
    {
      add
      {
        //throw new NotImplementedException();
      }

      remove
      {
        //throw new NotImplementedException();
      }
    }

    /// <summary>
    /// Gets name of the channel.
    /// </summary>
    public string Name { get => GetType().Name; }

    /// <summary>
    /// Gets the channel's configuration.
    /// </summary>
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

    bool IChannel.Active { get; set; }

    string IChannel.Name => "sss";

    IConfiguration IChannel.Configuration => throw new NotImplementedException();

    /// <summary>
    /// Sends data through the channel.
    /// </summary>
    /// <param name="data">The data.</param>
    public bool Send(D data)
    {
      lock (lockData)
      {
        if (Active)
        {
          InternalSend(channel, data);
          return true;
        }

        return false;
      }
    }

    /// <summary>
    /// Sends data through the channel.
    /// </summary>
    /// <param name="data">The data.</param>
    public bool Send(IEnumerable<D> data)
    {
      foreach (var item in data)
      {
        if (!Send(item))
        {
          return false;
        }
      }

      return true;
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
    }

    /// <summary>
    /// Called when data received from channel.
    /// </summary>
    /// <param name="data">The data.</param>
    protected void OnDataReceived(IEnumerable<D> data)
    {
      foreach (var dataItem in data) 
      {
        OnDataReceived(dataItem);
      }
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

    public bool Send(IChannelMessage data)
    {
      throw new NotImplementedException();
    }

    bool IChannel<D, C>.Send(D data)
    {
      throw new NotImplementedException();
    }

    bool IChannel<D, C>.Send(IEnumerable<D> data)
    {
      throw new NotImplementedException();
    }

    bool IChannel.Send(IChannelMessage data)
    {
      throw new NotImplementedException();
    }
  }
}