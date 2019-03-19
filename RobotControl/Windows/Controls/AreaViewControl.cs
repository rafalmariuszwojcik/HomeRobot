using System;
using System.Collections.Generic;
using System.Drawing;
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
    private int viewZoom = 25;//100;
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

    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);

      counter?.Signal();
      var transState = e.Graphics.Save();
      try
      {
        e.Graphics.PageUnit = GraphicsUnit.Millimeter;
        e.Graphics.ScaleTransform(DrawScale, DrawScale);
        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        e.Graphics.TranslateTransform((float)Origin.X, (float)Origin.Y);
        DrawGrid(e.Graphics);
        DrawOrigin(e.Graphics);
        DrawSimulation(e.Graphics);
      }
      finally
      {
        e.Graphics.Restore(transState);
      }

      var drawFont = new Font("Arial", 16);
      var drawBrush = new SolidBrush(Color.Green);
      e.Graphics.DrawString($"FPS: {counter?.SignalsPerSecond.ToString("0.0")}", drawFont, drawBrush, new Point(0, 0));
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
      var rect = Simulation.SimulationArea.Area.ConvertTo(MeasurementUnit.Milimeter);
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