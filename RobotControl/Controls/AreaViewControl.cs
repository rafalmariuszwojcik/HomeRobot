using System.Collections.Generic;
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
    private ISimulation simulation = new Simulation.Simulation();
    //private IList<SimulationElement> elements = new List<SimulationElement>();

    public AreaViewControl()
    {
      InitializeComponent();
      DoubleBuffered = true;
      simulation.Items.Add(new Robot(100, 100, 90));
      
      
    }

    public int Zoom
    {
      get { return zoom; }

      set
      {
        if (zoom != value)
        {
          zoom = value;
          Refresh();
        }
      }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);
      var scale = 0.2F;// zoom / 100F;
      e.Graphics.PageUnit = GraphicsUnit.Millimeter;
      e.Graphics.ScaleTransform(scale, scale);
      e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
      var pageWidthInMilimeters = (Width / (e.Graphics.DpiX / 2.54)) * 10;
      var pageHeightInMilimeters = (Height / (e.Graphics.DpiX / 2.54)) * 10;
      // e.Graphics.TranslateTransform((float)(pageWidthInMilimeters / 2 / scale), (float)(pageHeightInMilimeters / 2 / scale));
      e.Graphics.TranslateTransform((float)(-50), (float)(-50));

      //e.Graphics.TranslateTransform(1,1, System.Drawing.Drawing2D.MatrixOrder.Append)


      //var draw = new RobotDraw(new PointF(0, 0), 19, new Simulation.Robot.RobotGeometry(100, 30, 20, 20));
      //draw.Paint(e.Graphics);
      DrawGrid(e.Graphics);
      DrawSimulation(e.Graphics);
    }

    private void DrawGrid(Graphics g)
    {
      var area = simulation.SimulationArea;
      var xSize = (float)area.W.ConvertTo(MeasurementUnit.Milimeter).Value;
      var ySize = (float)area.L.ConvertTo(MeasurementUnit.Milimeter).Value;
      var x = 0F;
      var y = 0F;

      var pen = new Pen(Color.FromArgb(255, 160, 160, 160));
      pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
      
      while (y < ySize)
      {
        g.DrawLine(pen, new PointF(0, y), new PointF(xSize, y));
        var drawFont = new System.Drawing.Font("Arial", 32);
        var drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
        g.DrawString(y.ToString(), drawFont, drawBrush, new PointF(0, y));
        y += 100F;
      }

      while (x < xSize)
      {
        g.DrawLine(pen, new PointF(x, 0), new PointF(x, ySize));
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
  }
}
