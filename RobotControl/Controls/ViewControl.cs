using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace RobotControl.Controls
{
  public class ViewControl : ScrollableControl
  {
    private const int WM_HSCROLL = 0x114;
    private const int WM_VSCROLL = 0x115;

    private Size autoScrollMinSize;
    private Point autoScrollPosition;

    public ViewControl()
    {
      VScroll = true;
      HScroll = true;
      AutoScroll = false;
      DoubleBuffered = true;
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
        BeginInvoke(new Action(() => UpdateScrollPosition()));
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

      if (se.Type == ScrollEventType.EndScroll)
      {
        var x = 1;
        var y = x;
      }

      SetDisplayRectLocation(0, se.NewValue);

      return;

      if (se.ScrollOrientation == ScrollOrientation.HorizontalScroll)
      {
        autoScrollPosition.X = se.NewValue;
      }
      else if (se.ScrollOrientation == ScrollOrientation.VerticalScroll)
      {
        autoScrollPosition.Y = se.NewValue;
      }

      BeginInvoke(new Action(() => UpdateScrollPosition()));
    }

    protected override void WndProc(ref Message msg)
    {
      if (msg.HWnd == this.Handle)
      {
        switch (msg.Msg)
        {

          case WM_VSCROLL:
            if (msg.LParam != IntPtr.Zero)
            {
              break; //do the base.WndProc
            }
            try
            {
              int oldVscrollPos = VerticalScroll.Value;
              int newVscrollPos = oldVscrollPos;
              ScrollEventType type = getScrollEventType(msg.WParam);
              switch (type)
              {
                case ScrollEventType.SmallDecrement:
                  newVscrollPos = oldVscrollPos - VerticalScroll.SmallChange;
                  break;
                case ScrollEventType.SmallIncrement:
                  newVscrollPos = oldVscrollPos + VerticalScroll.SmallChange;
                  break;
                case ScrollEventType.LargeDecrement:
                  newVscrollPos = oldVscrollPos - VerticalScroll.LargeChange;
                  break;
                case ScrollEventType.LargeIncrement:
                  newVscrollPos = oldVscrollPos + VerticalScroll.LargeChange;
                  break;
                case ScrollEventType.First:
                  newVscrollPos = VerticalScroll.Minimum;
                  break;
                case ScrollEventType.Last:
                  newVscrollPos = VerticalScroll.Maximum;
                  break;
                case ScrollEventType.ThumbTrack:
                  newVscrollPos = HiWord((int)msg.WParam);
                  break;
                case ScrollEventType.ThumbPosition:
                  newVscrollPos = HiWord((int)msg.WParam);
                  break;
              }
              if (newVscrollPos < VerticalScroll.Minimum)
              {
                newVscrollPos = VerticalScroll.Minimum;
              }
              if (newVscrollPos > VerticalScroll.Maximum)
              {
                newVscrollPos = VerticalScroll.Maximum;
              }
              if (oldVscrollPos != newVscrollPos)
              {
                VerticalScroll.Value = newVscrollPos;
                if (type != ScrollEventType.EndScroll)
                {
                  ScrollEventArgs arg = new ScrollEventArgs(type, oldVscrollPos, newVscrollPos, ScrollOrientation.VerticalScroll);
                  this.OnScroll(arg);
                  Invalidate();
                }
              }
            }
            catch (Exception)
            {
            }
            return;

          case WM_HSCROLL:
            if (msg.LParam != IntPtr.Zero)
            {
              break; //do the base.WndProc
            }
            try
            {
              int oldHscrollPos = HorizontalScroll.Value;
              int newHscrollPos = oldHscrollPos;
              ScrollEventType type = getScrollEventType(msg.WParam);
              switch (type)
              {
                case ScrollEventType.SmallDecrement:
                  newHscrollPos = oldHscrollPos - HorizontalScroll.SmallChange;
                  break;
                case ScrollEventType.SmallIncrement:
                  newHscrollPos = oldHscrollPos + HorizontalScroll.SmallChange;
                  break;
                case ScrollEventType.LargeDecrement:
                  newHscrollPos = oldHscrollPos - HorizontalScroll.LargeChange;
                  break;
                case ScrollEventType.LargeIncrement:
                  newHscrollPos = oldHscrollPos + HorizontalScroll.LargeChange;
                  break;
                case ScrollEventType.First:
                  newHscrollPos = HorizontalScroll.Minimum;
                  break;
                case ScrollEventType.Last:
                  newHscrollPos = HorizontalScroll.Maximum;
                  break;
                case ScrollEventType.ThumbTrack:
                  newHscrollPos = HiWord((int)msg.WParam);
                  break;
                case ScrollEventType.ThumbPosition:
                  newHscrollPos = HiWord((int)msg.WParam);
                  break;
              }
              if (newHscrollPos < HorizontalScroll.Minimum)
              {
                newHscrollPos = HorizontalScroll.Minimum;
              }
              if (newHscrollPos > HorizontalScroll.Maximum)
              {
                newHscrollPos = HorizontalScroll.Maximum;
              }
              if (oldHscrollPos != newHscrollPos)
              {
                HorizontalScroll.Value = newHscrollPos;
                ScrollEventArgs arg = new ScrollEventArgs(type, oldHscrollPos, newHscrollPos, ScrollOrientation.HorizontalScroll);
                if (type != ScrollEventType.EndScroll)
                {
                  this.OnScroll(arg);
                  Invalidate();
                }
              }
            }
            catch (Exception)
            {
            }
            return;

          default:
            break;
        }
      }
      base.WndProc(ref msg);
    }

    private static void AdjustScroll(ScrollProperties scroll, int max, int size, int additionalScrollSize)
    {
      var visible = max > size;
      if (visible)
      {
        scroll.Minimum = 0;
        scroll.Maximum = max - additionalScrollSize;
        scroll.LargeChange = size;
        scroll.SmallChange = size / 10;
      }

      scroll.Visible = scroll.Enabled = visible;
    }

    private void AdjustHorizontalScroll()
    {
      AdjustScroll(HorizontalScroll, autoScrollMinSize.Width, ClientSize.Width, autoScrollMinSize.Height > ClientSize.Height ? SystemInformation.VerticalScrollBarWidth : 0);
    }

    private void AdjustVerticalScroll()
    {
      
      AdjustScroll(VerticalScroll, autoScrollMinSize.Height, ClientSize.Height, autoScrollMinSize.Width > ClientSize.Width ? SystemInformation.HorizontalScrollBarHeight : 0);
    }

    private void UpdateScrollPosition()
    {
      HorizontalScroll.Value = autoScrollPosition.X;
      VerticalScroll.Value = autoScrollPosition.Y;
    }

    private const int SB_LINEUP = 0;
    private const int SB_LINEDOWN = 1;
    private const int SB_PAGEUP = 2;
    private const int SB_PAGEDOWN = 3;
    private const int SB_THUMBPOSITION = 4;
    private const int SB_THUMBTRACK = 5;
    private const int SB_TOP = 6;
    private const int SB_BOTTOM = 7;
    private const int SB_ENDSCROLL = 8;

    private int getSBFromScrollEventType(ScrollEventType type)
    {
      int result = -1;
      switch (type)
      {
        case ScrollEventType.SmallDecrement:
          result = SB_LINEUP;
          break;
        case ScrollEventType.SmallIncrement:
          result = SB_LINEDOWN;
          break;
        case ScrollEventType.LargeDecrement:
          result = SB_PAGEUP;
          break;
        case ScrollEventType.LargeIncrement:
          result = SB_PAGEDOWN;
          break;
        case ScrollEventType.ThumbTrack:
          result = SB_THUMBTRACK;
          break;
        case ScrollEventType.First:
          result = SB_TOP;
          break;
        case ScrollEventType.Last:
          result = SB_BOTTOM;
          break;
        case ScrollEventType.ThumbPosition:
          result = SB_THUMBPOSITION;
          break;
        case ScrollEventType.EndScroll:
          result = SB_ENDSCROLL;
          break;
        default:
          break;
      }
      return result;
    }

    private ScrollEventType getScrollEventType(System.IntPtr wParam)
    {
      ScrollEventType result = 0;
      switch (LoWord((int)wParam))
      {
        case SB_LINEUP:
          result = ScrollEventType.SmallDecrement;
          break;
        case SB_LINEDOWN:
          result = ScrollEventType.SmallIncrement;
          break;
        case SB_PAGEUP:
          result = ScrollEventType.LargeDecrement;
          break;
        case SB_PAGEDOWN:
          result = ScrollEventType.LargeIncrement;
          break;
        case SB_THUMBTRACK:
          result = ScrollEventType.ThumbTrack;
          break;
        case SB_TOP:
          result = ScrollEventType.First;
          break;
        case SB_BOTTOM:
          result = ScrollEventType.Last;
          break;
        case SB_THUMBPOSITION:
          result = ScrollEventType.ThumbPosition;
          break;
        case SB_ENDSCROLL:
          result = ScrollEventType.EndScroll;
          break;
        default:
          result = ScrollEventType.EndScroll;
          break;
      }
      return result;
    }

    static int MakeLong(int LoWord, int HiWord)
    {
      return (HiWord << 16) | (LoWord & 0xffff);
    }

    static IntPtr MakeLParam(int LoWord, int HiWord)
    {
      return (IntPtr)((HiWord << 16) | (LoWord & 0xffff));
    }

    static int HiWord(int number)
    {
      if ((number & 0x80000000) == 0x80000000)
        return (number >> 16);
      else
        return (number >> 16) & 0xffff;
    }

    static int LoWord(int number)
    {
      return number & 0xffff;
    }
  }
}