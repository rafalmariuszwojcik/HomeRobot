using RobotControl.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotControl.Communication
{
  public class Controller : IChanell2<IControllerCommand, ConfigurationBase>
  {
    public bool Active { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public string Name => throw new NotImplementedException();

    public ConfigurationBase Configuration => throw new NotImplementedException();

    public event EventHandler<IDataReceivedEventArgs2<IControllerCommand>> DataReceived;

    public void Close()
    {
      throw new NotImplementedException();
    }

    public void Dispose()
    {
      throw new NotImplementedException();
    }

    public void Open()
    {
      throw new NotImplementedException();
    }

    public void Send(IControllerCommand data)
    {
      throw new NotImplementedException();
    }
  }
}
