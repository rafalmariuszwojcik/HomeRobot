using System;
using System.Collections.Generic;
using System.Linq;

namespace RobotControl.Drawing
{
  public static class DrawingFactory
  {
    private static readonly IDictionary<Type, DrawElementBase> elements = new Dictionary<Type, DrawElementBase>();

    static DrawingFactory()
    {
      InitializeDrawingFactory();
    }

    public static DrawElementBase GetDrawingInstance(Simulation.ISimulationItem simulationItem)
    {
      return elements.Keys.Any(x => x.Equals(simulationItem.GetType())) ? elements[simulationItem.GetType()] : null;
    }

    private static void InitializeDrawingFactory()
    {
      elements.Add(typeof(Simulation.Robot.Robot), new RobotDraw());
    }
  }
}