using RobotControl.Command;
using RobotControl.Core;
using RobotControl.Messages;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RobotControl.Fake.FakeRobot
{
  /// <summary>
  /// Fake robot state structure.
  /// </summary>
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
    /// The left engine.
    /// </summary>
    private readonly Engine leftEngine = new Engine();

    /// <summary>
    /// The right engine.
    /// </summary>
    private readonly Engine rightEngine = new Engine();

    /// <summary>
    /// The command listener.
    /// </summary>
    /// <remarks>Process incoming commands.</remarks>
    private readonly CommandListener commandListener;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="FakeRobot"/> class.
    /// </summary>
    public FakeRobot() : base(SIMULATION_TIMEOUT)
    {
      commandListener = new CommandListener(x => ProcessCommands(x));
    }

    /// <summary>
    /// Occurs when robot state changed.
    /// </summary>
    public event EventHandler<FakeRobotState> OnRobotState;

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      if (disposing)
      {
        DisposeHelper.Dispose(commandListener);
      }
    }

    /// <summary>
    /// Simulate robot moving.
    /// </summary>
    protected override void WorkInternal()
    {
      var leftEngineState = leftEngine.GetEngineState();
      var rightEngineState = rightEngine.GetEngineState();
      if (leftEngineState.Signaled || rightEngineState.Signaled)
      {
        OnRobotState?.Invoke(this, new FakeRobotState(leftEngineState, rightEngineState));
      }
    }

    /// <summary>
    /// Processes the commands.
    /// </summary>
    /// <remarks>Execute incoming commands on <see cref=""/>FakeRobot</see> instance.</remarks>
    /// <param name="commands">The commands.</param>
    private void ProcessCommands(IEnumerable<ICommand> commands) 
    {
      Parallel.ForEach(commands, x =>
        {
          if (x is ControllerCommand controllerCommand) 
          {
            SetPower(controllerCommand);
          }
        }
      );
    }

    /// <summary>
    /// Sets engine's power.
    /// </summary>
    /// <param name="controllerCommand">The controller command.</param>
    private void SetPower(ControllerCommand controllerCommand) 
    {
      var powerL = Math.Abs(controllerCommand.Y);
      var directionL = Math.Sign(controllerCommand.Y);
      var powerR = powerL;
      var directionR = directionL;

      var turn = controllerCommand.X;

      powerR -= (turn > 0.0 ? turn : 0.0F);
      powerR = powerR < 0.0 ? 0.0F : powerR;

      powerL -= (turn < 0.0 ? -turn : 0.0F);
      powerL = powerL < 0.0 ? 0.0F : powerL;

      if (!(powerL > 0.0 || powerR > 0.0))
      {
        powerL = powerR = Math.Abs(turn);
        directionL = Math.Sign(turn);
        directionR = Math.Sign(-turn);
      }

      leftEngine.Power = powerL * directionL;
      rightEngine.Power = powerR * directionR;
    }
  }
}