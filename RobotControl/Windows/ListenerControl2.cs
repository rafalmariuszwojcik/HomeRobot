using RobotControl.Communication;
using RobotControl.Core;
using RobotControl.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RobotControl.Windows
{
  public class ListenerControl2<T> : IListener<T> where T : class
  {
    private readonly Control control;

    public ListenerControl2(Control control)
    {
      this.control = control;
      //MessageManager.Instance.RegisterListener(this);
      //CommandManager.Instance.RegisterListener(this);
    }

    void IListener<T>.MessageReceived(IChannel channel, T data)
    {
      ;
    }
  }
}
