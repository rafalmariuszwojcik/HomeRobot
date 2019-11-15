using RobotControl.Command;
using RobotControl.Core;
using RobotControl.Messages;
using System;

namespace RobotControl.Communication
{
  /// <summary>
  /// Base class for communication changels.
  /// </summary>
  /// <typeparam name="T">Chanell type.</typeparam>
  /// <typeparam name="D">Chanlell's data type.</typeparam>
  /// <typeparam name="C">Configuration type.</typeparam>
  /// <seealso cref="RobotControl.Core.DisposableBase" />
  /// <seealso cref="RobotControl.Communication.IChanellEx{D, C}" />
  public abstract class ChannelBaseEx<T, D, C> : DisposableBase, IChanellEx<D, C>
    where T : IDisposable
    where D : class
    where C : IConfiguration
  {
    /// <summary>
    /// The channel object.
    /// </summary>
    private T channel;

    /// <summary>
    /// Occurs when incoming data received.
    /// </summary>
    public event EventHandler<IDataReceivedEventArgsEx<D>> DataReceived;

    /// <summary>
    /// Gets name of the chanell.
    /// </summary>
    public string Name { get => GetType().Name; }

    /// <summary>
    /// Gets the chanell's configuration.
    /// </summary>
    public C Configuration { get; }

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="T:RobotControl.Communication.IChanellEx`2" /> is active (opened).
    /// </summary>
    /// <value>
    /// <c>true</c> if active; otherwise, <c>false</c>.
    /// </value>
    public bool Active
    {
      get { return channel != null; }
      
      set
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

    /// <summary>
    /// Opens the chanell.
    /// </summary>
    public void Open()
    {
      Close();
      try
      {
        channel = InternalOpen(Configuration);
      }
      catch (Exception)
      {
        Close();
        throw;
      }
    }

    /// <summary>
    /// Closes the chanell.
    /// </summary>
    public void Close()
    {
      if (channel != null)
      {
        InternalClose(channel);
        channel.Dispose();
        channel = default;
      }
    }

    /// <summary>
    /// Sends data through the chanell.
    /// </summary>
    /// <param name="data">The data.</param>
    public void Send(D data)
    {
      if (channel == null)
      {
        throw new NotImplementedException();
      }

      InternalSend(channel, data);
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        Close();
      }
    }

    /// <summary>
    /// Internal open communication chanell method.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <returns>Reference to communication object.</returns>
    protected abstract T InternalOpen(C configuration);

    /// <summary>
    /// Internal close communication chanell.
    /// </summary>
    /// <param name="channel">The channel.</param>
    protected abstract void InternalClose(T channel);

    /// <summary>
    /// Internal send data through the chanell.
    /// </summary>
    /// <param name="channel">The channel.</param>
    /// <param name="commands">The commands.</param>
    public abstract void InternalSend(T channel, D data);
  }


  public abstract class ChannelBase<T, U> : DisposableBase, IChannel<U>
    where T: IDisposable
    where U: ConfigurationBase
  {
    private T channel;
    private readonly U configuration;
    public string Name { get => GetType().Name; }
    public IConfiguration Configuration => configuration;

    public bool Active
    {
      get { return channel != null; }
      set
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

    public event EventHandler<DataReceivedEventArgs> DataReceived;

    public ChannelBase(U configuration)
    {
      this.configuration = configuration;
    }

    public void Open()
    {
      Close();
      try
      {
        channel = InternalOpen(configuration);
      }
      catch (Exception)
      {
        Close();
        throw;
      }
    }

    public void Close()
    {
      if (channel != null)
      {
        InternalClose(channel);
        channel.Dispose();
        channel = default(T);
      }
    }

    public void Send(ICommand[] commands)
    {
      if (channel == null)
      {
        throw new NotImplementedException();
      }

      InternalSend(channel, commands);
    }

    public void Send(string data)
    {
      if (channel == null)
      {
        throw new NotImplementedException();
      }

      InternalSend(channel, data);
    }

    /// <summary>
    /// Internal open communication chanel method.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <returns>Reference to communication object.</returns>
    protected abstract T InternalOpen(U configuration);

    protected abstract void InternalClose(T channel);
    public abstract void InternalSend(T channel, ICommand[] commands);
    public abstract void InternalSend(T channel, string data);
    
    protected void OnDataReceived(string data)
    {
      DataReceived?.Invoke(this, new DataReceivedEventArgs(data));
      MessageManager.Instance.DataReceived(this, new[] { data });
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        Close();
      }
    }
  }
}