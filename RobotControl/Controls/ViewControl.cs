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
        BeginInvoke(new Action(() => 
        {
          AdjustFormScrollbars(true);
        }));
        
      }
    }

    protected override void AdjustFormScrollbars(bool displayScrollbars)
    {
      HorizontalScroll.Minimum = 0;
      HorizontalScroll.Maximum = autoScrollMinSize.Width;
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
  }
}
