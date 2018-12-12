using System;
using System.Drawing;
using System.Windows.Forms;
using RobotControl.Drawing;
using RobotControl.Simulation;
using RobotControl.Simulation.Robot;

namespace RobotControl.Controls
{
  public partial class AreaViewControl : ViewControl
  {
    private int viewZoom = 100;
    private Point2D originPoint = new Point2D(0, 0, MeasurementUnit.Milimeter);
    private Point? previousMousePosition;
    private ISimulation simulation = new Simulation.Simulation();

    public AreaViewControl()
    {
      InitializeComponent();
      simulation.Items.Add(new Robot(0, 0, 75));
      AutoScrollMinSize = CalcAreaSize();
      AutoScrollPosition = CalcScrollPosition();
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

    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);
      e.Graphics.PageUnit = GraphicsUnit.Millimeter;
      e.Graphics.ScaleTransform(DrawScale, DrawScale);
      e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
      e.Graphics.TranslateTransform((float)Origin.X, (float)Origin.Y);
      DrawGrid(e.Graphics);
      DrawOrigin(e.Graphics);
      DrawSimulation(e.Graphics);
    }

    protected override void OnScroll(ScrollEventArgs se)
    {
      base.OnScroll(se);
      var startPoint = CalcStartPoint();
      var x = -(HorizontalScroll.Value + startPoint.X);
      var y = -(VerticalScroll.Value + startPoint.Y);
      using (var g = CreateGraphics())
      {
        var xInMilimeter = new Length(x / g.DpiX / DrawScale, MeasurementUnit.Inch).ConvertTo(MeasurementUnit.Milimeter);
        var yInMilimeter = new Length(y / g.DpiY / DrawScale, MeasurementUnit.Inch).ConvertTo(MeasurementUnit.Milimeter);
        Origin = new Point2D(xInMilimeter.Value, yInMilimeter.Value);
      }
    }

    private void DrawGrid(Graphics g)
    {
      var rect = simulation.SimulationArea.Area.ConvertTo(MeasurementUnit.Milimeter);
      var x = (float)rect.Left.Value;
      var y = (float)rect.Top.Value;
      var r = (float)rect.Right.Value;
      var b = (float)rect.Bottom.Value;
      
      var pen = new Pen(Color.FromArgb(255, 160, 160, 160)) { DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot };
      var drawFont = new Font("Arial", 32);
      var drawBrush = new SolidBrush(Color.Gray);

      while (y <= b)
      {
        g.DrawLine(pen, new PointF(x, y), new PointF(r, y));
        g.DrawString(y.ToString(), drawFont, drawBrush, new PointF(-(float)Origin.ConvertTo(MeasurementUnit.Milimeter).X, y));
        y += 100F;
      }
            
      y = (float)rect.Top.Value;
      while (x <= r)
      {
        g.DrawLine(pen, new PointF(x, y), new PointF(x, b));
        g.DrawString(x.ToString(), drawFont, drawBrush, new PointF(x, -(float)Origin.ConvertTo(MeasurementUnit.Milimeter).Y));
        x += 100F;
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
      foreach (var item in simulation.Items)
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

        using (var g = CreateGraphics())
        {
          xInMilimeter = new Length(dx / g.DpiX / DrawScale, MeasurementUnit.Inch).ConvertTo(MeasurementUnit.Milimeter);
          yInMilimeter = new Length(dy / g.DpiY / DrawScale, MeasurementUnit.Inch).ConvertTo(MeasurementUnit.Milimeter);
        }

        Origin = new Point2D(Origin.X + xInMilimeter.Value, Origin.Y + yInMilimeter.Value);
      }

      previousMousePosition = position;
    }

    private Point2D CalculateOrigin(Point2D origin)
    {
      var rect = simulation.SimulationArea.Area.ConvertTo(MeasurementUnit.Milimeter);
      origin = origin.ConvertTo(MeasurementUnit.Milimeter);
      var x = origin.X;
      var y = origin.Y;

      using (var g = CreateGraphics())
      {
        var widthMilimeter = new Length(ClientSize.Width / g.DpiX / DrawScale, MeasurementUnit.Inch).ConvertTo(MeasurementUnit.Milimeter);
        var heightMilimeter = new Length(ClientSize.Height / g.DpiY / DrawScale, MeasurementUnit.Inch).ConvertTo(MeasurementUnit.Milimeter);
        var maxX = -(rect.Right - widthMilimeter).Value;
        var maxY = -(rect.Bottom - heightMilimeter).Value;
        x = (x <= maxX || rect.Width <= widthMilimeter) ? maxX : x;
        y = (y <= maxY || rect.Height <= heightMilimeter) ? maxY : y;
        x = (x >= -rect.Left.Value || rect.Width <= widthMilimeter) ? -rect.Left.Value : x;
        y = (y >= -rect.Top.Value || rect.Height <= heightMilimeter) ? -rect.Top.Value : y;
      }

      return new Point2D(x, y, MeasurementUnit.Milimeter);
    }

    private Size CalcAreaSize()
    {
      using (var g = CreateGraphics())
      {
        var rect = simulation.SimulationArea.Area.ConvertTo(MeasurementUnit.Inch);
        return new Size((int)Math.Ceiling(rect.Width.Value * g.DpiX * DrawScale), (int)Math.Ceiling(rect.Height.Value * g.DpiY * DrawScale));
      }
    }

    private Point CalcStartPoint()
    {
      using (var g = CreateGraphics())
      {
        var rect = simulation.SimulationArea.Area.ConvertTo(MeasurementUnit.Inch);
        return new Point((int)Math.Ceiling(rect.Left.Value * g.DpiX * DrawScale), (int)Math.Ceiling(rect.Top.Value * g.DpiY * DrawScale));
      }
    }

    private Point CalcOriginPoint()
    {
      using (var g = CreateGraphics())
      {
        var origin = originPoint.ConvertTo(MeasurementUnit.Inch);
        return new Point((int)Math.Ceiling(origin.X * g.DpiX * DrawScale), (int)Math.Ceiling(origin.Y * g.DpiY * DrawScale));
      }
    }

    private Point CalcScrollPosition()
    {
      var start = CalcStartPoint();
      var origin = CalcOriginPoint();
      var x = -start.X - origin.X;
      var y = -start.Y - origin.Y;
      return new Point(x, y);
    }
  }
}