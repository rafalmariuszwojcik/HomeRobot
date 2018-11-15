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

    public static IDrawElement GetDrawingInstance(ISimulationItem simulationItem)
    {
      foreach (var key in elements.Keys)
      {
        if (simulationItem.GetType().GetInterfaces().Contains(key))
        {
          var action = elements[key];
          return action(simulationItem);
        }
      }

      return null;
    }

    private static void InitializeDrawingFactory()
    {
      elements.Add(typeof(Simulation.Robot.IRobot), new Func<ISimulationItem, IDrawElement>(x => new RobotDraw((Simulation.Robot.IRobot)x)));
    }
  }
}