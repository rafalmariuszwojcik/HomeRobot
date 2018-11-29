using System;
using System.Windows.Forms;

namespace RobotControl.Controls
{
  public class ScrollControl : Control
  {
    private ScrollBar hScrollBar;
    private ScrollBar vScrollBar;

    public ScrollControl()
    {
      this.hScrollBar = new UpdatingHScrollBar();
      this.hScrollBar.Top = this.Height - this.hScrollBar.Height;
      this.hScrollBar.Left = 0;
      this.hScrollBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

      this.vScrollBar = new UpdatingVScrollBar();
      this.vScrollBar.Top = 0;
      this.vScrollBar.Left = this.Width - this.vScrollBar.Width;
      this.vScrollBar.Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;

      this.hScrollBar.Width = this.Width - this.vScrollBar.Width;
      this.vScrollBar.Height = this.Height - this.hScrollBar.Height;

      this.Controls.Add(this.hScrollBar);
      this.Controls.Add(this.vScrollBar);
    }

    private class UpdatingVScrollBar : VScrollBar
    {
      protected override void OnValueChanged(EventArgs e)
      {
        base.OnValueChanged(e);
        // setting the scroll position programmatically shall raise Scroll
        this.OnScroll(new ScrollEventArgs(ScrollEventType.EndScroll, this.Value));
      }
    }

    private class UpdatingHScrollBar : HScrollBar
    {
      protected override void OnValueChanged(EventArgs e)
      {
        base.OnValueChanged(e);
        // setting the scroll position programmatically shall raise Scroll
        this.OnScroll(new ScrollEventArgs(ScrollEventType.EndScroll, this.Value));
      }
    }
  }
}
