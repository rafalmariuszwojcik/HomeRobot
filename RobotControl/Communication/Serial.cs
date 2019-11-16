using System.IO.Ports;

namespace RobotControl.Communication
{
  public class Serial : ChannelBaseEx<SerialPort, string, SerialConfiguration>
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

    protected override void InternalSend(SerialPort channel, string data)
    {
      throw new System.NotImplementedException();
    }
    /*
public override void InternalSend(SerialPort channel, ICommand[] commands)
{
 //channel.Write(data);
}

public override void InternalSend(SerialPort channel, string data)
{
 channel.Write(data);
}*/
  }
}