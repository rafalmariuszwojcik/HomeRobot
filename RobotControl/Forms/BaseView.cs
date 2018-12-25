using WeifenLuo.WinFormsUI.Docking;

namespace RobotControl.Forms
{
  public class BaseView : DockContent
  {
    public BaseView()
    {
      Text = GetType().Name;
    }
  }
}
