using System;
using System.Collections.Generic;
using System.Linq;

namespace RobotControl.Drawing
{
  public static class DrawingFactory
  {
    private static readonly IDictionary<Type, IDrawElement> elements = new Dictionary<Type, IDrawElement>();

    static DrawingFactory()
    {
      InitializeDrawingFactory();
    }

    public static IDrawElement GetDrawingInstance(Simulation.ISimulationItem simulationItem)
    {
      return elements.Keys.Any(x => x.Equals(simulationItem.GetType())) ? elements[simulationItem.GetType()] : null;
    }

    private static void InitializeDrawingFactory()
    {
      elements.Add(typeof(Simulation.Robot.Robot), new RobotDraw());
    }
  }
}