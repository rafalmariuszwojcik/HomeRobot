using RobotControl.Command.Controller;
using System;

namespace RobotControl.Simulation.Robot.Controller
{
  /// <summary>
  /// Base class for processing controller commands.
  /// </summary>
  /// <seealso cref="RobotControl.Simulation.Robot.Controller.IControllerProcessor" />
  public abstract class ControllerProcessorBase : IControllerProcessor
  {
    /// <summary>
    /// Calculates robot's engines power.
    /// </summary>
    /// <param name="controllerCommand">The controller command.</param>
    /// <param name="leftEnginePower">The left engine power.</param>
    /// <param name="rightEnginePower">The right engine power.</param>
    public void CalculateEnginesPower(IControllerStateCommand controllerCommand, out int leftEnginePower, out int rightEnginePower)
    {
      var powerL = Math.Abs(GetY(controllerCommand));
      var directionL = Math.Sign(GetY(controllerCommand));
      var powerR = Math.Abs(GetY(controllerCommand));
      var directionR = Math.Sign(GetY(controllerCommand));
      var turn = GetX(controllerCommand);

      powerR -= (turn > 0 ? turn : 0);
      powerR = powerR < 0 ? 0 : powerR;

      powerL -= (turn < 0 ? -turn : 0);
      powerL = powerL < 0 ? 0 : powerL;

      if (!(powerL > 0 || powerR > 0))
      {
        powerL = powerR = Math.Abs(turn);
        directionL = Math.Sign(turn);
        directionR = Math.Sign(-turn);
      }

      leftEnginePower = powerL * directionL;
      rightEnginePower = powerR * directionR;
    }

    /// <summary>
    /// Gets the X axis power.
    /// </summary>
    /// <param name="controllerCommand">The controller command.</param>
    /// <returns>X axis power (-100; 100).</returns>
    protected abstract int GetX(IControllerStateCommand controllerCommand);

    /// <summary>
    /// Gets the Y axis power.
    /// </summary>
    /// <param name="controllerCommand">The controller command.</param>
    /// <returns>Y axis power (-100; 100).</returns>
    protected abstract int GetY(IControllerStateCommand controllerCommand);
  }
}
