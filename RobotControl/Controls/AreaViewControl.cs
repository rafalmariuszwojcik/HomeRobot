using System.Drawing;
using System.Windows.Forms;
using RobotControl.Drawing;
using RobotControl.Simulation;
using RobotControl.Simulation.Robot;

namespace RobotControl.Controls
{
  public partial class AreaViewControl : UserControl
  {
    private int zoom = 100;
    private Origin origin = new Origin(new Length(0, MeasurementUnit.Milimeter), new Length(0, MeasurementUnit.Milimeter));
    private Point? previousMousePosition;
    private ISimulation simulation = new Simulation.Simulation();
    
    public AreaViewControl()
    {
      InitializeComponent();
      this.VScroll = true;
      this.VerticalScroll.Minimum = 10;
      this.VerticalScroll.Minimum = 100;
      this.VerticalScroll.Visible = true;




      DoubleBuffered = true;
      simulation.Items.Add(new Robot(0, 0, 75));
    }

    public Origin Origin
    {
      get { return origin; }

      set
      {
        origin = CalculateOrigin(value);
        Refresh();
      }
    }

    public int Zoom
    {
      get { return zoom; }

      set
      {
        if (zoom != value)
        {
          zoom = value;
          origin = CalculateOrigin(origin);
          Refresh();
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
      var pageWidthInMilimeters = (Width / (e.Graphics.DpiX / 2.54)) * 10;
      var pageHeightInMilimeters = (Height / (e.Graphics.DpiX / 2.54)) * 10;
      // e.Graphics.TranslateTransform((float)(pageWidthInMilimeters / 2 / scale), (float)(pageHeightInMilimeters / 2 / scale));
      //e.Graphics.TranslateTransform((float)origin.X.Value, (float)origin.Y.Value);
      SetOrigin(e.Graphics);

      //e.Graphics.TranslateTransform(1,1, System.Drawing.Drawing2D.MatrixOrder.Append)


      //var draw = new RobotDraw(new PointF(0, 0), 19, new Simulation.Robot.RobotGeometry(100, 30, 20, 20));
      //draw.Paint(e.Graphics);
      DrawGrid(e.Graphics);
      DrawSimulation(e.Graphics);
    }

    private void SetOrigin(Graphics g)
    {
      g.TranslateTransform((float)origin.X.Value, (float)origin.Y.Value);
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
        g.DrawString(y.ToString(), drawFont, drawBrush, new PointF(-(float)origin.X.ConvertTo(MeasurementUnit.Milimeter).Value, y));
        y += 100F;
      }
            
      y = (float)rect.Top.Value;
      while (x <= r)
      {
        g.DrawLine(pen, new PointF(x, y), new PointF(x, b));
        g.DrawString(x.ToString(), drawFont, drawBrush, new PointF(x, -(float)origin.Y.ConvertTo(MeasurementUnit.Milimeter).Value));
        x += 100F;
      }
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

        Origin = new Origin(origin.X + xInMilimeter, origin.Y + yInMilimeter);
      }

      previousMousePosition = position;
    }

    private Origin CalculateOrigin(Origin origin)
    {
      var rect = simulation.SimulationArea.Area.ConvertTo(MeasurementUnit.Milimeter);
      var x = origin.X.ConvertTo(MeasurementUnit.Milimeter).Value;
      var y = origin.Y.ConvertTo(MeasurementUnit.Milimeter).Value;

      using (var g = CreateGraphics())
      {
        var widthMilimeter = new Length(ClientSize.Width / g.DpiX / DrawScale, MeasurementUnit.Inch).ConvertTo(MeasurementUnit.Milimeter);
        var heightMilimeter = new Length(ClientSize.Height / g.DpiY / DrawScale, MeasurementUnit.Inch).ConvertTo(MeasurementUnit.Milimeter);
        var maxX = -(rect.Right - widthMilimeter).Value;
        var maxY = -(rect.Bottom - heightMilimeter).Value;
        x = (x <= maxX || rect.Width.Value <= widthMilimeter.Value) ? maxX : x;
        y = (y <= maxY || rect.Height.Value <= heightMilimeter.Value) ? maxY : y;
        x = (x >= -rect.Left.Value || rect.Width.Value <= widthMilimeter.Value) ? -rect.Left.Value : x;
        y = (y >= -rect.Top.Value || rect.Height.Value <= heightMilimeter.Value) ? -rect.Top.Value : y;
      }

      return new Origin(new Length(x, MeasurementUnit.Milimeter), new Length(y, MeasurementUnit.Milimeter));
    }
  }
}