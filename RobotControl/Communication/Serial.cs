using System;
using System.IO.Ports;

namespace RobotControl.Communication
{
  public class Serial : ChannelBase<SerialPort>
  {
    private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
      SerialPort sp = (SerialPort)sender;
      var data = sp.ReadExisting();
      OnDataReceived(data);
    }

    protected override SerialPort InternalOpen()
    {
      var port = new SerialPort("COM1");
      port.BaudRate = 76800;
      port.Open();
      port.DataReceived += Port_DataReceived;
      return port;
    }

    protected override void InternalClose(SerialPort channel)
    {
      if (channel.IsOpen)
      {
        channel.Close();
      }
    }

    public override void InternalSend(SerialPort channel, string data)
    {
      channel.Write(data);
    }
  }
}