using System.Windows.Forms;

namespace RobotControl.Windows.Controls
{
  public abstract class BaseControl : UserControl
  {
    public BaseControl()
    {
      ControlManager.Instance.RegisterListener(this);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        ControlManager.Instance.UnregisterListener(this);
      }

      base.Dispose(disposing);
    }
  }
}