using RobotControl.Core;
using RobotControl.Messages;
using System;
using System.Diagnostics;
using System.Threading;

namespace RobotControl.Fake.FakeRobot
{
  public enum EngineState { UNKNOWN = 0, STOP = 1, FORWARD = 2, BACKWARD = 3 };

  public class Engine : DisposableBase
  {
    private const int MAX_SPEED = 100;
    private const int ONE_SECOND = 1000;

    private static readonly Stopwatch stopwatch = new Stopwatch();
    private readonly Timer timer;
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
    }

    public int Speed
    {
      get => speed;

      set
      {
        if (speed != value)
        {
          speed = value;
          ChangeSpeed(speed);
        }
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
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


    

    private void TimerProc(object state)
    {
      var currentMilis = stopwatch.ElapsedMilliseconds;
      if (lastSignalMilis.HasValue) 
      {
        var timeDelta = currentMilis - lastSignalMilis.Value;
        currentSpeed = ONE_SECOND / (double)timeDelta;
        oneSignalMilis = timeDelta;
      }

      lastSignalMilis = currentMilis;



      var t = (Timer)state;
      
      
      //stopwatch.ElapsedMilliseconds



      //var totalMilliseconds = (long)new TimeSpan(DateTime.Now.Ticks).TotalMilliseconds;
      //MessageManager.Instance.DataReceived(this, new[] { $"ENC,1,{distance},{totalMilliseconds},2;{Environment.NewLine}" });
      //MessageManager.Instance.DataReceived(this, new[] { $"ENC,0,{distance},{totalMilliseconds},2;{Environment.NewLine}" });
      //distance++;
    }

    private void ChangeSpeed(int newSpeed)
    {
      var period = newSpeed > 0 ? Convert.ToInt32(Math.Round(1000.0 / newSpeed)) : Timeout.Infinite;
      timer.Change(0, period);
    }
  }
}
