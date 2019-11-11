using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RobotControl.Core
{
  /// <summary>
  /// Base worker class.
  /// </summary>
  /// <remarks>Used to do jobs in separate thread.</remarks>
  /// <seealso cref="RobotControl.Core.DisposableBase" />
  public abstract class WorkerBase : DisposableBase
  {
    /// <summary>
    /// The work timeout in miliseconds.
    /// </summary>
    private readonly int workTimeout = Timeout.Infinite;

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
    /// Initializes a new instance of the <see cref="WorkerBase"/> class.
    /// </summary>
    public WorkerBase(int workTimeout)
    {
      this.workTimeout = workTimeout;
      worker = new Thread(Work);
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
    /// Internal work code.
    /// </summary>
    /// <remarks>Must be implemented in descendants.</remarks>
    protected abstract void WorkInternal();

    /// <summary>
    /// Starts worker instance.
    /// </summary>
    /// <remarks>Start executing worker code.</remarks>
    private void Start()
    {
      if (!worker.IsAlive)
      {
        worker.Start();
      }
    }

    /// <summary>
    /// Stops worker instance.
    /// </summary>
    /// <remarks>Stops worker code.</remarks>
    private void Stop()
    {
      if (worker.IsAlive)
      {
        signal.Set();
        worker.Join();
      }
    }

    /// <summary>
    /// Work execution loop.
    /// </summary>
    private void Work()
    {
      while (true)
      {
        if (signal.WaitOne(workTimeout))
        {
          break;
        }

        try
        {
          WorkInternal();
        }
        catch (Exception)
        {
          ;
        }
      }
    }
  }
}