using System;
using System.Collections.Generic;

namespace RobotControl.Simulation
{
  public interface ISimulationItem
  {
    ISimulationItem Parent { get; }
    IEnumerable<ISimulationItem> Items { get; }
    bool State { get; }
    void Add(ISimulationItem item);
    void ResetState();
    void SetState();
    event EventHandler OnStateSet;
  }
}