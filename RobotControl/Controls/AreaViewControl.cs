using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RobotControl.Drawing;
using RobotControl.Simulation;

namespace RobotControl.Controls
{
  public partial class AreaViewControl : UserControl
  {
    private IList<SimulationElement> elements = new List<SimulationElement>();

    public AreaViewControl()
    {
      InitializeComponent();
      DoubleBuffered = true;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);
      const float SCALE = 0.1F;
      var scale = 0.1F;
      e.Graphics.PageUnit = GraphicsUnit.Millimeter;
      e.Graphics.ScaleTransform(scale, scale);
      e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
      var pageWidthInMilimeters = (Width / (e.Graphics.DpiX / 2.54)) * 10;
      var pageHeightInMilimeters = (Height / (e.Graphics.DpiX / 2.54)) * 10;
      e.Graphics.TranslateTransform((float)(pageWidthInMilimeters / 2 / scale), (float)(pageHeightInMilimeters / 2 / scale));


      var draw = new RobotDraw(new PointF(0, 0), 19, new Simulation.RobotGeometry(100, 30, 20, 20));
      draw.Paint(e.Graphics);
    }

    private void DrawGrid(Graphics g)
    {
    }
  }
}
