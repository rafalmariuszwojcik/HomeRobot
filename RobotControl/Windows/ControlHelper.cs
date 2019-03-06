using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace RobotControl.Windows
{
  public static class ControlHelper
  {
    public static bool InvokeAction(Control control, Action action)
    {
      if (control.InvokeRequired)
      {
        try
        {
          control.Invoke(action);
        }
        catch (ObjectDisposedException)
        {
          ;
        }
        catch (InvalidAsynchronousStateException)
        {
          ;
        }

        return true;
      }
      else
      {
        return false;
      }
    }
  }
}
