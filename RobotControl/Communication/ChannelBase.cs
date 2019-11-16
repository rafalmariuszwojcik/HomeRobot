using RobotControl.Command;
using RobotControl.Core;
using RobotControl.Messages;
using System;
using System.Collections.Generic;

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
    public event EventHandler<IDataReceivedEventArgsEx<D>> DataReceived;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChannelBaseEx{T, D, C}"/> class.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    public ChannelBaseEx(C configuration)
    {
      Configuration = configuration;
    }

    /// <summary>
    /// Gets name of the chanell.
    /// </summary>
    public string Name { get => GetType().Name; }

    /// <summary>
    /// Gets the chanell's configuration.
    /// </summary>
    //public C Configuration { get; private set; }
    public IConfiguration Configuration { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="T:RobotControl.Communication.IChanellEx`2" /> is active (opened).
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
    /// Sends data through the chanell.
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

    protected void OnDataReceived(D data)
    {
      DataReceived?.Invoke(this, new DataReceivedEventArgsEx<D>(data));
      //MessageManager.Instance.DataReceived(this, new[] { data });
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
    protected abstract void InternalSend(T channel, D data);

    /// <summary>
    /// Opens the chanell.
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
    /// Closes the chanell.
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


  public abstract class ChannelBase_old<T, U> : DisposableBase, IChannel_old<U>
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

    public ChannelBase_old(U configuration)
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