using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using RobotControl.Communication;
using RobotControl.Core;
using RobotControl.Drawing;
using RobotControl.Simulation;

namespace RobotControl.Windows.Controls
{
  public partial class AreaViewControl : ViewControl, IListener<SimulationPackage>
  {
    private int viewZoom = 25;
    private Point2D originPoint = new Point2D(0, 0, MeasurementUnit.Milimeter);
    private Point? previousMousePosition;
    private readonly Counter counter = new Counter();
    
    public AreaViewControl()
    {
      InitializeComponent();
      AutoScrollMinSize = CalcAreaSize();
      AutoScrollPosition = CalcScrollPosition();
      counter.OnChanged += CounterOnChanged;
    }

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        counter?.Dispose();
        components.Dispose();
      }

      base.Dispose(disposing);
    }

    public Point2D Origin
    {
      get { return originPoint; }

      set
      {
        originPoint = CalculateOrigin(value);
        AutoScrollPosition = CalcScrollPosition();
        BeginInvoke(new Action(() => Refresh()));
      }
    }

    public int Zoom
    {
      get { return viewZoom; }

      set
      {
        if (viewZoom != value)
        {
          viewZoom = value;
          originPoint = CalculateOrigin(originPoint);
          AutoScrollMinSize = CalcAreaSize();
          AutoScrollPosition = CalcScrollPosition();
          BeginInvoke(new Action(() => Refresh()));
        }
      }
    }

    private float DrawScale
    {
      get { return Zoom / 100F; }
    }

    private Rect2D ViewRect
    {
      get
      {
        var origin = Origin.ConvertTo(MeasurementUnit.Milimeter);
        var widthMilimeter = new Length(ClientSize.Width / DpiX / DrawScale, MeasurementUnit.Inch).ConvertTo(MeasurementUnit.Milimeter);
        var heightMilimeter = new Length(ClientSize.Height / DpiY / DrawScale, MeasurementUnit.Inch).ConvertTo(MeasurementUnit.Milimeter);
        var rect = new Rect2D(new Point2D(-origin.X, -origin.Y, MeasurementUnit.Milimeter), widthMilimeter, heightMilimeter);
        return rect;
      }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);

      counter?.Signal();
      var transState = e.Graphics.Save();
      try
      {
        e.Graphics.PageUnit = GraphicsUnit.Millimeter;
        e.Graphics.ScaleTransform(DrawScale, DrawScale);
        e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
        e.Graphics.TranslateTransform((float)Origin.X, (float)Origin.Y);
        DrawGrid(e.Graphics);
        DrawOrigin(e.Graphics);
        DrawSimulation(e.Graphics);
      }
      finally
      {
        e.Graphics.Restore(transState);
      }

      var drawFont = new Font("Arial", 16, FontStyle.Bold);
            var drawBrush = new SolidBrush(Color.Green);
      //e.Graphics.DrawString($"FPS: {counter?.SignalsPerSecond.ToString("0.0")}", drawFont, drawBrush, new Point(0, 0));

      

      // assuming g is the Graphics object on which you want to draw the text
      GraphicsPath p = new GraphicsPath();
      p.AddString(
          $"FPS: {counter?.SignalsPerSecond.ToString("0.0")}",             // text to draw
          FontFamily.GenericSansSerif,  // or any other font family
          (int)FontStyle.Bold,      // font style (bold, italic, etc.)
          /*g.DpiY*/96 * /*fontSize*/24 / 72,       // em size
          new Point(0, 0),              // location where to draw text
          new StringFormat());          // set options here (e.g. center alignment)
      e.Graphics.FillPath(new System.Drawing.SolidBrush(System.Drawing.Color.Yellow), p);
      e.Graphics.DrawPath(Pens.Black, p);
      
      // + g.FillPath if you want it filled as well
    }

    protected override void OnScroll(ScrollEventArgs se)
    {
      base.OnScroll(se);
      var startPoint = CalcStartPoint();
      var x = -(HorizontalScroll.Value + startPoint.X);
      var y = -(VerticalScroll.Value + startPoint.Y);
      var xInMilimeter = new Length(x / DpiX / DrawScale, MeasurementUnit.Inch).ConvertTo(MeasurementUnit.Milimeter);
      var yInMilimeter = new Length(y / DpiY / DrawScale, MeasurementUnit.Inch).ConvertTo(MeasurementUnit.Milimeter);
      Origin = new Point2D(xInMilimeter.Value, yInMilimeter.Value);
    }

    private void DrawGrid(Graphics g)
    {
      var viewRect = ViewRect;
      var rect = Simulation.SimulationArea.Area.ConvertTo(MeasurementUnit.Milimeter);

      var l = (float)rect.Left.Value + (float)(Math.Ceiling((viewRect.Left.Value - (float)rect.Left.Value) / 100F) * 100F);
      var t = (float)rect.Top.Value + (float)(Math.Ceiling((viewRect.Top.Value - (float)rect.Top.Value) / 100F) * 100F);
      var r = (float)rect.Right.Value - (float)(Math.Ceiling((viewRect.Right.Value - (float)rect.Right.Value) / 100F) * 100F);
      var b = (float)rect.Bottom.Value - (float)(Math.Ceiling((viewRect.Bottom.Value - (float)rect.Bottom.Value) / 100F) * 100F);

      var pen = new Pen(Color.FromArgb(255, 160, 160, 160)) { DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot };
      var drawFont = new Font("Arial", 32);
      var drawBrush = new SolidBrush(Color.Gray);

      while (t <= b)
      {
        if (t >= viewRect.Top.Value && t <= viewRect.Bottom.Value)
        {
          g.DrawLine(pen, new PointF((float)viewRect.Left.Value, t), new PointF((float)viewRect.Right.Value, t));
          g.DrawString(t.ToString(), drawFont, drawBrush, new PointF(-(float)Origin.ConvertTo(MeasurementUnit.Milimeter).X, t));
        }

        t += 100F;
      }
      
      while (l <= r)
      {
        if (l >= viewRect.Left.Value && l <= viewRect.Right.Value)
        {
          g.DrawLine(pen, new PointF(l, (float)viewRect.Top.Value), new PointF(l, (float)viewRect.Bottom.Value));
          g.DrawString(l.ToString(), drawFont, drawBrush, new PointF(l, -(float)Origin.ConvertTo(MeasurementUnit.Milimeter).Y));
        }

        l += 100F;
      }
    }

    private void DrawOrigin(Graphics g)
    {
      var drawFont = new Font("Arial", 32);
      var drawBrush = new SolidBrush(Color.Green);
      g.DrawString($"{Origin.ConvertTo(MeasurementUnit.Milimeter).X}; {Origin.ConvertTo(MeasurementUnit.Milimeter).Y}", drawFont, drawBrush, new PointF(0, 0));
    }

    private void DrawSimulation(Graphics g)
    {
      foreach (var item in Simulation.Items)
      {
        var draw = DrawingFactory.GetDrawingInstance(item);
        if (draw != null)
        {
          draw.Paint(g);
        }
      }
    }

    private void View_MouseMove(object sender, MouseEventArgs e)
    {
      var position = MousePosition;
      if (e.Button == MouseButtons.Left && previousMousePosition.HasValue)
      {
        var dx = position.X - previousMousePosition.Value.X;
        var dy = position.Y - previousMousePosition.Value.Y;
        Length xInMilimeter;
        Length yInMilimeter;
        xInMilimeter = new Length(dx / DpiX / DrawScale, MeasurementUnit.Inch).ConvertTo(MeasurementUnit.Milimeter);
        yInMilimeter = new Length(dy / DpiY / DrawScale, MeasurementUnit.Inch).ConvertTo(MeasurementUnit.Milimeter);
        Origin = new Point2D(Origin.X + xInMilimeter.Value, Origin.Y + yInMilimeter.Value);
      }

      previousMousePosition = position;
    }

    private Point2D CalculateOrigin(Point2D origin)
    {
      var rect = Simulation.SimulationArea.Area.ConvertTo(MeasurementUnit.Milimeter);
      origin = origin.ConvertTo(MeasurementUnit.Milimeter);
      var x = origin.X;
      var y = origin.Y;
      var widthMilimeter = new Length(ClientSize.Width / DpiX / DrawScale, MeasurementUnit.Inch).ConvertTo(MeasurementUnit.Milimeter);
      var heightMilimeter = new Length(ClientSize.Height / DpiY / DrawScale, MeasurementUnit.Inch).ConvertTo(MeasurementUnit.Milimeter);
      var maxX = -(rect.Right - widthMilimeter).Value;
      var maxY = -(rect.Bottom - heightMilimeter).Value;
      x = (x <= maxX || rect.Width <= widthMilimeter) ? maxX : x;
      y = (y <= maxY || rect.Height <= heightMilimeter) ? maxY : y;
      x = (x >= -rect.Left.Value || rect.Width <= widthMilimeter) ? -rect.Left.Value : x;
      y = (y >= -rect.Top.Value || rect.Height <= heightMilimeter) ? -rect.Top.Value : y;
      return new Point2D(x, y, MeasurementUnit.Milimeter);
    }

    private Size CalcAreaSize()
    {
      var rect = Simulation.SimulationArea.Area.ConvertTo(MeasurementUnit.Inch);
      return new Size((int)Math.Ceiling(rect.Width.Value * DpiX * DrawScale), (int)Math.Ceiling(rect.Height.Value * DpiY * DrawScale));
    }

    private Point CalcStartPoint()
    {
      var rect = Simulation.SimulationArea.Area.ConvertTo(MeasurementUnit.Inch);
      return new Point((int)Math.Ceiling(rect.Left.Value * DpiX * DrawScale), (int)Math.Ceiling(rect.Top.Value * DpiY * DrawScale));
    }

    private Point CalcOriginPoint()
    {
      var origin = originPoint.ConvertTo(MeasurementUnit.Inch);
      return new Point((int)Math.Ceiling(origin.X * DpiX * DrawScale), (int)Math.Ceiling(origin.Y * DpiY * DrawScale));
    }

    private Point CalcScrollPosition()
    {
      var start = CalcStartPoint();
      var origin = CalcOriginPoint();
      var x = -start.X - origin.X;
      var y = -start.Y - origin.Y;
      return new Point(x, y);
    }

    void IListener<SimulationPackage>.DataReceived(IChannel channel, IEnumerable<SimulationPackage> data)
    {
      var simulation = data.FirstOrDefault(x => x.Simulation.Equals(Simulation));
      if (simulation != null)
      {
        Refresh();
      }
    }

    private ISimulation Simulation => SimulationManager.Instance.Simulation;

    private void CounterOnChanged(object sender, EventArgs e)
    {
      ControlHelper.InvokeAction(this, () => Refresh());
    }
  }
}