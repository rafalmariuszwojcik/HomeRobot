using RobotControl.Core;
using RobotControl.Messages;
using System;
using System.Threading;

namespace RobotControl.Fake.FakeRobot
{
  /// <summary>
  /// The fake robot object to simulate behaviours.
  /// </summary>
  /// <remarks>Double wheel robot simulation.</remarks>
  public class FakeRobot : DisposableBase
  {
    /// <summary>
    /// The simulation timeout.
    /// </summary>
    private const int SIMULATION_TIMEOUT = 25;
    
    /// <summary>
    /// The left engine.
    /// </summary>
    private readonly Engine leftEngine = new Engine();

    /// <summary>
    /// The right engine.
    /// </summary>
    private readonly Engine rightEngine = new Engine();

    /// <summary>
    /// The signal event.
    /// </summary>
    /// <remarks>Used to control (terminate) worker thread.</remarks>
    private readonly AutoResetEvent signal = new AutoResetEvent(false);

    /// <summary>
    /// The worker thread.
    /// </summary>
    /// <remarks>Executes robot's behaviours.</remarks>
    private readonly Thread worker;

    /// <summary>
    /// Initializes a new instance of the <see cref="FakeRobot"/> class.
    /// </summary>
    public FakeRobot()
    {
      worker = new Thread(Simulation);
      Start();
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        Stop();
        DisposeHelper.Dispose(signal);
      }
    }

    /// <summary>
    /// Starts fake robot instance.
    /// </summary>
    /// <remarks>Start executing simulated behaviours.</remarks>
    private void Start()
    {
      if (!worker.IsAlive) 
      {
        worker.Start();
      } 
    }

    /// <summary>
    /// Stops fake robot instance.
    /// </summary>
    /// <remarks>Stops simulation.</remarks>
    private void Stop()
    {
      if (worker.IsAlive) 
      {
        signal.Set();
        worker.Join();
      }
    }

    int i = 0;
    
    /// <summary>
    /// Simulation execution loop.
    /// </summary>
    /// <param name="sync">The synchronize.</param>
    private void Simulation()
    {
      while (true) 
      {
        if (signal.WaitOne(SIMULATION_TIMEOUT)) 
        {
          break;
        }

        try
        {
          var leftEngineInfo = leftEngine.GetEngineState();
          var rightEngineInfo = rightEngine.GetEngineState();

          MessageManager.Instance.DataReceived(this, new[] { $"ENC,1,{i},{0},2;{Environment.NewLine}" });
          i++;

        }
        catch (Exception)
        {
          ;
        }
      }
    }
  }
}