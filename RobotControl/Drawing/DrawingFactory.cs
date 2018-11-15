using RobotControl.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RobotControl.Drawing
{
  public static class DrawingFactory
  {
    private static readonly IDictionary<Type, Func<ISimulationItem, IDrawElement>> cache = new Dictionary<Type, Func<ISimulationItem, IDrawElement>>();
    private static readonly IDictionary<Type, Func<ISimulationItem, IDrawElement>> elements = new Dictionary<Type, Func<ISimulationItem, IDrawElement>>();

    static DrawingFactory()
    {
      InitializeDrawingFactory();
    }

    public static IDrawElement GetDrawingInstance(ISimulationItem simulationItem)
    {
      var type = simulationItem.GetType();
      if (cache.ContainsKey(type))
      {
        return cache[type](simulationItem);
      }

      foreach (var key in elements.Keys)
      {
        if (type.GetInterfaces().Contains(key))
        {
          var action = elements[key];
          cache.Add(type, action);
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