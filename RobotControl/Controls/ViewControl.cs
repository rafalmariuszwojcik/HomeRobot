using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace RobotControl.Controls
{
  public class ViewControl : ScrollableControl
  {
    private Size autoScrollMinSize;

    public ViewControl()
    {
      DoubleBuffered = true;
      AutoScroll = false;
      HorizontalScroll.Minimum = 0;
      HorizontalScroll.Maximum = 100;
      HorizontalScroll.Visible = true;
    }

    public override bool AutoScroll
    {
      get { return base.AutoScroll; }
      set { base.AutoScroll = false; }
    }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new Size AutoScrollMinSize
    {
      get { return autoScrollMinSize; }

      set
      {
        autoScrollMinSize = value;
        BeginInvoke(new Action(() => AdjustFormScrollbars(true)));
      }
    }

    //
    // http://msdn.microsoft.com/en-us/library/system.windows.forms.scrollbar.maximum.aspx
    //
    protected override void AdjustFormScrollbars(bool displayScrollbars)
    {
      if (IsHandleCreated && ClientSize.Width > 0 && ClientSize.Height > 0)
      {
        AdjustHorizontalScroll();
        AdjustVerticalScroll();
      }
    }

    protected override void OnScroll(ScrollEventArgs se)
    {
      base.OnScroll(se);
      /*
      if (se.ScrollOrientation == ScrollOrientation.HorizontalScroll)
      {
        Origin = new Point2D(se.NewValue - (3779 / 2), Origin.Y);
      }
      */
    }

    private void AdjustHorizontalScroll()
    {
      if (IsHandleCreated && ClientSize.Width > 0 && autoScrollMinSize.Width > 0)
      {
        HorizontalScroll.Minimum = 0;
        HorizontalScroll.Maximum = autoScrollMinSize.Width;
        HorizontalScroll.LargeChange = ClientSize.Width;
        HorizontalScroll.SmallChange = ClientSize.Width / 10;
        HorizontalScroll.Visible = true;
        HorizontalScroll.Enabled = true;
      }
      else
      {
        HorizontalScroll.Visible = false;
        HorizontalScroll.Enabled = false;
      }
    }

    private void AdjustVerticalScroll()
    {
      AdjustScroll(VerticalScroll, IsHandleCreated && ClientSize.Height > 0 && autoScrollMinSize.Height > 0, autoScrollMinSize.Height, ClientSize.Height);
    }

    private void AdjustScroll(ScrollProperties scroll, bool visible, int max, int size)
    {
      //var v = IsHandleCreated && 
      if (visible)
      {
        scroll.Minimum = 0;
        scroll.Maximum = max;
        scroll.LargeChange = size;
        scroll.SmallChange = size / 10;
      }

      scroll.Visible = scroll.Enabled = visible;
    }
  }
}
