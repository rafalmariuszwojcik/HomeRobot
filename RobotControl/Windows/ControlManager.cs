using RobotControl.Core;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RobotControl.Windows
{
  public class ControlManager : Singleton<ControlManager>
  {
    private readonly object lockObject = new object();
    private readonly IList<Control> listeners = new List<Control>();

    public void RegisterListener(Control listener)
    {
      lock (lockObject)
      {
        if (listener != null && !listeners.Contains(listener))
        {
          listeners.Add(listener);
        }
      }
    }

    public void UnregisterListener(Control listener)
    {
      lock (lockObject)
      {
        if (listeners.Contains(listener))
        {
          listeners.Remove(listener);
        }
      }
    }

    protected override void TearDown()
    {
      base.TearDown();
    }
  }
}