using RobotControl.Command;
using RobotControl.Core;
using RobotControl.Messages;
using System;

namespace RobotControl.Communication
{
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