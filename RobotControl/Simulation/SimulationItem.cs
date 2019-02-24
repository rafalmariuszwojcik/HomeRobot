using RobotControl.Core;
using System;
using System.Collections.Generic;

namespace RobotControl.Simulation
{
  public abstract class SimulationItem : DisposableBase, ISimulationItem
  {
    protected ISimulationItem parent;
    private readonly IList<ISimulationItem> items = new List<ISimulationItem>();
    public event EventHandler OnStateSet;

    public SimulationItem(ISimulationItem parent = null)
    {
      this.parent = parent;
    }

    public bool State { get; private set; }
    public IEnumerable<ISimulationItem> Items => items;
    public ISimulationItem Parent => parent;
    
    public void ResetState()
    {
      State = false;
      foreach (var item in items)
      {
        item.ResetState();
      }
    }

    public void SetState()
    {
      if (!State)
      {
        State = true;
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

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        ;
      }
    }

    protected static double RadiansToDegrees(double radians)
    {
      return (radians * 180) / (Math.PI);
    }

    protected static double DegreesToRadians(double degrees)
    {
      return (degrees * Math.PI) / 180;
    }
  }
}