using RobotControl.Command;
using System.IO.Ports;

namespace RobotControl.Communication
{
  public class Serial : ChannelBase<SerialPort, SerialConfiguration>
  {
    public Serial(SerialConfiguration configuration) : base(configuration)
    {
    }

    public Serial() : base(new SerialConfiguration())
    {
    }

    private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
      SerialPort sp = (SerialPort)sender;
      var data = sp.ReadExisting();
      OnDataReceived(data);
    }

    protected override SerialPort InternalOpen(SerialConfiguration configuration)
    {
      var port = new SerialPort(configuration.Port)
      {
        BaudRate = configuration.BaudRate
      };

      port.DataReceived += Port_DataReceived;
      port.Open();
      return port;
    }

    protected override void InternalClose(SerialPort channel)
    {
      if (channel.IsOpen)
      {
        channel.Close();
      }
    }

    public override void InternalSend(SerialPort channel, ICommand[] commands)
    {
      //channel.Write(data);
    }
  }
}