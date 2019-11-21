using System.IO.Ports;

namespace RobotControl.Communication.Serial
{
  public class SerialChannel : ChannelBase<SerialPort, string, SerialConfiguration>
  {
    public SerialChannel(SerialConfiguration configuration) : base(configuration)
    {
    }

    public SerialChannel() : base(new SerialConfiguration())
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