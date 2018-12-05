using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace RobotControl.Controls
{
  public class ViewControl : ScrollableControl
  {
    private Size autoScrollMinSize;
    private Point autoScrollPosition;

    public ViewControl()
    {
      DoubleBuffered = true;
      AutoScroll = false;
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

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new Point AutoScrollPosition
    {
      get { return autoScrollPosition; }

      set
      {
        autoScrollPosition.X = value.X >= 0 ? (value.X <= autoScrollMinSize.Width ? value.X : autoScrollMinSize.Width) : 0;
        autoScrollPosition.Y = value.Y >= 0 ? (value.Y <= autoScrollMinSize.Height ? value.Y : autoScrollMinSize.Height) : 0;
        BeginInvoke(new Action(() => 
        {
          HorizontalScroll.Value = autoScrollPosition.X;
          VerticalScroll.Value = autoScrollPosition.Y;
        }));
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

    private static void AdjustScroll(ScrollProperties scroll, int max, int size)
    {
      var visible = max > size;
      if (visible)
      {
        scroll.Minimum = 0;
        scroll.Maximum = max;
        scroll.LargeChange = size;
        scroll.SmallChange = size / 10;
      }

      scroll.Visible = scroll.Enabled = visible;
    }

    private void AdjustHorizontalScroll()
    {
      AdjustScroll(HorizontalScroll, autoScrollMinSize.Width, ClientSize.Width);
    }

    private void AdjustVerticalScroll()
    {
      
      AdjustScroll(VerticalScroll, autoScrollMinSize.Height, ClientSize.Height);
    }
  }
}