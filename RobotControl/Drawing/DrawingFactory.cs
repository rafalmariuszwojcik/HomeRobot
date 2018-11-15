using RobotControl.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RobotControl.Drawing
{
  public static class DrawingFactory
  {
    private static readonly IDictionary<Type, Func<ISimulationItem, IDrawElement>> elements = new Dictionary<Type, Func<ISimulationItem, IDrawElement>>();

    static DrawingFactory()
    {
      InitializeDrawingFactory();
    }

    public static DrawElementBase GetDrawingInstance(Simulation.ISimulationItem simulationItem)
    {
      return null;
      //return elements.Keys.Any(x => x.Equals(simulationItem.GetType())) ? elements[simulationItem.GetType()] : null;
    }

    private static void InitializeDrawingFactory()
    {
      //elements.Add()

      //elements.Add(Simulation.Robot.IRobot, new Func<ISimulationItem, IDrawElement>(x => null));
    }
  }
}