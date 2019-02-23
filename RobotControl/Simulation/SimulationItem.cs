using RobotControl.Core;
using System;
using System.Collections.Generic;

namespace RobotControl.Simulation
{
  public abstract class SimulationItem : DisposableBase, ISimulationItem
  {
    protected ISimulationItem parent;
    private readonly IList<ISimulationItem> items = new List<ISimulationItem>();
    protected readonly SimulationPoint position;
    private bool state;
    public event EventHandler OnStateSet;

    public SimulationItem(ISimulationItem parent = null)
    {
      this.parent = parent;
    }

    public SimulationItem(double x, double y, double angle)
    {
      position = new SimulationPoint(x, y, angle);
    }

    public SimulationPoint Position
    {
      get { return position; }
    }

    public static double RadiansToDegrees(double radians)
    {
      return (radians * 180) / (Math.PI);
    }

    public static double DegreesToRadians(double degrees)
    {
      return (degrees * Math.PI) / 180;
    }

    //ISimulation ISimulationItem.Simulation => throw new NotImplementedException();

    bool ISimulationItem.State => state;

    public IEnumerable<ISimulationItem> Items => items;

    public ISimulationItem Parent => throw new NotImplementedException();

    void ISimulationItem.ResetState()
    {
      state = false;
      foreach (var item in items)
      {
        item.ResetState();
      }
    }

    public void SetState()
    {
      if (!state)
      {
        state = true;
        OnStateSet?.Invoke(this, new EventArgs());
        if (parent != null)
        {
          parent.SetState();
        }
      }
    }

    public void Add(ISimulationItem item)
    {
      if (!items.Contains(item) && item is SimulationItem)
      {
        items.Add(item);
        ((SimulationItem)item).parent = this;
      }
    }
  }
}