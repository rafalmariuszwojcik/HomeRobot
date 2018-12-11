using System.Windows.Forms;

namespace RobotControl.Controls
{
  public class ManualScrollableControl : ScrollableControl
  {
    private const int WM_HSCROLL = 0x114;
    private const int WM_VSCROLL = 0x115;
  }
}
