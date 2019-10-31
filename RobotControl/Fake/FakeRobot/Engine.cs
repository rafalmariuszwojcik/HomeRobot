using RobotControl.Core;
using System;
using System.Diagnostics;
using System.Threading;

namespace RobotControl.Fake.FakeRobot
{
  /// <summary>Fake engine class.</summary>
  /// <remarks>Used to simulate robot's engine.</remarks>
  /// <seealso cref="RobotControl.Core.DisposableBase" />
  public class Engine : DisposableBase
  {
    /// <summary>The maximum speed in encoder signals per second.</summary>
    private const int MAX_SPEED = 100;

    /// <summary>The one second. Indicates number of internal engine signals per one second.</summary>
    private const int ONE_SECOND = 1000;

    /// <summary>The internal time counter.</summary>
    private static readonly Stopwatch stopwatch = new Stopwatch();

    /// <summary>The timer.</summary>
    /// <remarks>Generates fake engine's encoder signals.</remarks>
    private readonly Timer timer;

    /// <summary>
    /// The encoder signal event.
    /// </summary>
    private readonly AutoResetEvent signalEvent;

    private int speed;
    private int distance;
    private long? lastSignalMilis;
    private long? oneSignalMilis;
    private double currentSpeed;

    static Engine() 
    {
      stopwatch.Start();
    }    
    
    public Engine()
    {
      //stopwatch.Start();
      //work = new Thread(new ParameterizedThreadStart(DoWork));
      //work.Start(cts.Token);
      timer = new Timer(TimerProc);
      signalEvent = new AutoResetEvent(true);
    }

    /// <summary>
    /// Gets the signal event.
    /// </summary>
    /// <remarks>Signals engine encoder event.</remarks>
    /// <value>
    /// The signal event.
    /// </value>
    public EventWaitHandle SignalEvent => signalEvent;

    /// <summary>Gets or sets the speed.</summary>
    /// <value>The speed.</value>
    public int Speed
    {
      get => speed;

      set
      {
        if (speed != value)
        {
          value = value <= MAX_SPEED ? value : MAX_SPEED;
          value = value >= -MAX_SPEED ? value : -MAX_SPEED;
          speed = value;
          ChangeSpeed(Math.Abs(speed));
        }
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        timer?.Dispose();
        //Stop();
        /*
        if (cts != null)
        {
          cts.Dispose();
          cts = null;
        }
        */
      }
    }

    private void DoWork(object obj)
    {
      CancellationToken ct = (CancellationToken)obj;
      while (!ct.IsCancellationRequested)
      {
        //Thread.SpinWait(50000);
        Thread.Sleep(10);
      }
    }

    private void Stop()
    {
      /*
      stopwatch.Stop();
      if (cts != null && work != null)
      {
        cts.Cancel();
        work.Join();
      }
      */
    }




    /// <summary>
    /// Timer procedure.
    /// </summary>
    /// <param name="state">
    /// The state object.
    /// </param>
    private void TimerProc(object state)
    {
      Signal();
    }

    /// <summary>Calculate engine current speed.</summary>
    private void Signal() 
    {
      var currentMilis = stopwatch.ElapsedMilliseconds;
      if (lastSignalMilis.HasValue) 
      {
        var timeDelta = currentMilis - lastSignalMilis.Value;
        currentSpeed = ONE_SECOND / (double)timeDelta;
        oneSignalMilis = timeDelta;
      }

      lastSignalMilis = currentMilis;
      signalEvent.Set();
    }
    
    /// <summary>Changes the speed.</summary>
    /// <remarks>Updates timer freqency.</remarks>
    /// <param name="newSpeed">The new speed in encoder signals per second.</param>
    private void ChangeSpeed(int newSpeed)
    {
      var period = newSpeed > 0 ? Convert.ToInt32(Math.Round(1000.0 / newSpeed)) : Timeout.Infinite;
      timer.Change(0, period);
    }
  }
}
