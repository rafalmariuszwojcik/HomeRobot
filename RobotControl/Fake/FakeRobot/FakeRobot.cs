using RobotControl.Core;
using System;

namespace RobotControl.Fake.FakeRobot
{
  /// <summary>
  /// Fake robot state structure.
  /// </summary>
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "<Pending>")]
  public struct FakeRobotState
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="FakeRobotState"/> struct.
    /// </summary>
    /// <param name="leftEngineState">State of the left engine.</param>
    /// <param name="rightEngineState">State of the right engine.</param>
    public FakeRobotState(EngineState leftEngineState, EngineState rightEngineState)
    {
      LeftEngineState = leftEngineState;
      RightEngineState = rightEngineState;
    }

    /// <summary>
    /// Gets the state of the left engine.
    /// </summary>
    public EngineState LeftEngineState { get; private set; }

    /// <summary>
    /// Gets the state of the right engine.
    /// </summary>
    public EngineState RightEngineState { get; private set; }
  }

  /// <summary>
  /// The fake robot object to simulate behaviours.
  /// </summary>
  /// <remarks>Double wheel robot simulation.</remarks>
  public class FakeRobot : WorkerBase
  {
    /// <summary>
    /// The simulation timeout.
    /// </summary>
    private const int SIMULATION_TIMEOUT = 10;

    /// <summary>
    /// Initializes a new instance of the <see cref="FakeRobot"/> class.
    /// </summary>
    public FakeRobot() : base(SIMULATION_TIMEOUT)
    {
    }

    /// <summary>
    /// Gets the left engine.
    /// </summary>
    public Engine LeftEngine { get; } = new Engine();

    /// <summary>
    /// Gets the right engine.
    /// </summary>
    public Engine RightEngine { get; } = new Engine();

    /// <summary>
    /// Occurs when robot state changed.
    /// </summary>
    public event EventHandler<FakeRobotState> OnRobotState;

    /// <summary>
    /// Simulate robot moving.
    /// </summary>
    protected override void WorkInternal()
    {
      var leftEngineState = LeftEngine.GetEngineState();
      var rightEngineState = RightEngine.GetEngineState();
      if (leftEngineState.Signaled || rightEngineState.Signaled)
      {
        OnRobotState?.Invoke(this, new FakeRobotState(leftEngineState, rightEngineState));
      }
    }
  }
}