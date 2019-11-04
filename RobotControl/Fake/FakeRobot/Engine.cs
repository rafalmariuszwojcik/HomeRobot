using System.Diagnostics;

namespace RobotControl.Fake.FakeRobot
{
  /// <summary>Fake engine class.</summary>
  /// <remarks>Used to simulate robot's engine.</remarks>
  public class Engine
  {
    /// <summary>The maximum speed in encoder signals per second.</summary>
    private const int MAX_SPEED = 100;

    /// <summary>The one second. Indicates number of internal engine signals per one second.</summary>
    private const int ONE_SECOND = 1000;

    /// <summary>The internal time counter.</summary>
    private static readonly Stopwatch stopwatch = new Stopwatch();

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
      get => speed;

      set
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

    /// <summary>
    /// Gets the current distance of engine.
    /// </summary>
    /// <returns></returns>
    public double GetDistance() 
    {
      CalculateDistance();
      return distance;
    }
    
    /// <summary>
    /// Calculates the current full distance of the engine.
    /// </summary>
    private void CalculateDistance()
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
    }
  }
}