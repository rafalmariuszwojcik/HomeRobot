using System;
using System.IO.Ports;

namespace RobotControl.Communication
{
  public class Serial : IChanel
  {
    bool disposed = false;

    private SerialPort port;

    public event EventHandler<IDataReceivedEventArgs> DataReceived;

    public void Close()
    {
      if (port != null)
      {
        if (port.IsOpen)
        {
          port.Close();
        }
        
        port.Dispose();
        port = null;
      }
    }

    public void Open()
    {
      Close();
      try
      {
        port = new SerialPort("COM1");
        port.BaudRate = 76800;
        port.Open();
        port.DataReceived += Port_DataReceived;
      }
      catch (Exception)
      {
        Close();
        throw;
      }
    }

    private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
      SerialPort sp = (SerialPort)sender;
      var data = sp.ReadExisting();
      DataReceived?.Invoke(this, new DataReceivedEventArgs(data));
    }

    public void Send(string data)
    {
      throw new NotImplementedException();
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
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