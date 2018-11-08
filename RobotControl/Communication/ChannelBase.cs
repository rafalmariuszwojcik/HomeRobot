using System;

namespace RobotControl.Communication
{
  public abstract class ChannelBase<T, U> : IChannel<U>
    where T: IDisposable
    where U: ConfigurationBase
  {
    bool disposed = false;

    private T channel;

    private U configuration;

    public event EventHandler<IDataReceivedEventArgs> DataReceived;

    public ChannelBase(U configuration)
    {
      this.configuration = configuration;
    }

    ~ChannelBase()
    {
      Dispose(false);
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

    public void Send(string data)
    {
      if (channel == null)
      {
        throw new NotImplementedException();
      }

      InternalSend(channel, data);
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected abstract T InternalOpen(U configuration);

    protected abstract void InternalClose(T channel);

    public abstract void InternalSend(T channel, string data);

    protected void OnDataReceived(string data)
    {
      DataReceived?.Invoke(this, new DataReceivedEventArgs(data));
    }

    protected virtual void Dispose(bool disposing)
    {
      if (disposed)
      {
        return;
      }

      if (disposing)
      {
        Close();
      }

      disposed = true;
    }
  }
}