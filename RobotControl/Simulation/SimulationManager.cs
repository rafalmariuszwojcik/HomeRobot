using RobotControl.Command;
using RobotControl.Communication;
using RobotControl.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RobotControl.Simulation
{
  public class SimulationManager : ManagerBase<SimulationManager, IListener<ISimulation>, ISimulation>, ICommandListener
  {
    private readonly object lockObject = new object();
    private readonly IList<ISimulation> items = new List<ISimulation>();

    public SimulationManager()
    {
      var simulation = new Simulation();
      var robot = new Robot.Robot(0, 0, 0);
      simulation.OnStateSet += (s, e) =>
      {
        BroadcastData(null, new[] { simulation });
      };

      simulation.Add(robot);
      Add(simulation);

      CommandManager.Instance.RegisterListener(this);
    }

    public ISimulation Simulation
    {
      get
      {
        lock (lockObject)
        {
          return items.FirstOrDefault();
        }
      }
    }

    public void Add(ISimulation simulation)
    {
      lock (lockObject)
      {
        if (!items.Contains(simulation))
        {
          items.Add(simulation);
        }
      }
    }

    public void Remove(ISimulation simulation)
    {
      lock (lockObject)
      {
        if (items.Contains(simulation))
        {
          items.Remove(simulation);
        }
      }
    }

    protected override void TearDown()
    {
      CommandManager.Instance.UnregisterListener(this);
      base.TearDown();
    }

    void IListener<ICommand>.DataReceived(IChannel channel, IEnumerable<ICommand> data)
    {
      IList<ISimulation> list;
      lock (lockObject)
      {
        list = new List<ISimulation>(items);
      }

      Parallel.ForEach(list, x =>
        {
          x.ResetState();
          (x as ICommandListener)?.DataReceived(channel, data);
        }
      );
    }
  }
}