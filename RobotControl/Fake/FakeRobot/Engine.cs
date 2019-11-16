using System;
using System.Diagnostics;

namespace RobotControl.Fake.FakeRobot
{
  /// <summary>
  /// Engine state structure.
  /// </summary>
  public struct EngineState
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="EngineState" /> struct.
    /// </summary>
    /// <param name="distance">The distance.</param>
    /// <param name="signaled">if set to <c>true</c> state of the engine has been changed.</param>
    /// <param name="milis">The milis.</param>
    public EngineState(double distance, bool signaled, long milis)
    {
      Distance = distance;
      Signaled = signaled;
      Milis = milis;
    }

    /// <summary>
    /// Gets the distance.
    /// </summary>
    public double Distance { get; private set; }

    /// <summary>
    /// Gets a value indicating whether this <see cref="EngineState"/> is signaled (state of the engine has been changed).
    /// </summary>
    public bool Signaled { get; private set; }

    /// <summary>
    /// Gets the last state timestamp in miliseconds.
    /// </summary>
    public long Milis { get; private set; }
  }

  /// <summary>Fake engine class.</summary>
  /// <remarks>Used to simulate robot's engine.</remarks>
  public class Engine
  {
    /// <summary>The maximum speed in encoder signals per second.</summary>
    private const int MAX_SPEED = 60;

    /// <summary>The one second. Indicates number of internal engine signals per one second.</summary>
    private const int ONE_SECOND = 1000;

    /// <summary>The internal time counter.</summary>
    private static readonly Stopwatch stopwatch = new Stopwatch();

    /// <summary>
    /// The synchronization object.
    /// </summary>
    private readonly object lockData = new object();

    /// <summary>
    /// The current speed in encoder signals per second.
    /// </summary>
    private int speed;

    /// <summary>
    /// The current full distance.
    /// </summary>
    private double distance;

    /// <summary>
    /// The last signal encoder timestamp.
    /// </summary>
    private long? lastSignalMilis;

    /// <summary>
    /// Initializes the <see cref="Engine"/> class.
    /// </summary>
    /// <remarks>Start static timer for all <c>Engine</c> instances.</remarks>
    static Engine()
    {
      stopwatch.Start();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Engine"/> class.
    /// </summary>
    public Engine()
    {
      CalculateDistance();
    }

    /// <summary>Gets or sets the speed.</summary>
    /// <value>The speed.</value>
    public int Speed
    {
      get
      {
        lock (lockData)
        {
          return speed;
        }
      }

      private set
      {
        lock (lockData)
        {
          if (speed != value)
          {
            value = value <= MAX_SPEED ? value : MAX_SPEED;
            value = value >= -MAX_SPEED ? value : -MAX_SPEED;
            speed = value;
            CalculateDistance();
          }
        }
      }
    }

    /// <summary>
    /// Sets the engine power in percent (-100;100).
    /// </summary>
    /// <remarks>-100: max power, backward rotation.</remarks>
    /// <remarks>0: stop.</remarks>
    /// <remarks>100: max power, forward rotation.</remarks>
    public double Power
    {
      set
      {
        value = value > 100.0 ? 100.0 : value;
        value = value < -100.0 ? -100.0 : value;
        Speed = Convert.ToInt32(Math.Round(value * MAX_SPEED / 100.0));
      }
    }

    /// <summary>
    /// Gets the current state of engine.
    /// </summary>
    /// <returns></returns>
    public EngineState GetEngineState()
    {
      return CalculateDistance();
    }

    /// <summary>
    /// Calculates the current full distance of the engine.
    /// </summary>
    private EngineState CalculateDistance()
    {
      lock (lockData)
      {
        var leg = 0.0;
        var currentMilis = stopwatch.ElapsedMilliseconds;
        if (lastSignalMilis.HasValue)
        {
          var timeDelta = (double)(currentMilis - lastSignalMilis.Value) / ONE_SECOND;
          leg = speed * timeDelta;
        }

        lastSignalMilis = currentMilis;
        distance += leg;
        return new EngineState(distance, leg > 0.0 || leg < 0.0, currentMilis);
      }
    }
  }
}