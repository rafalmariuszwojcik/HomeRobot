using System;
using RobotControl.Command.Controller;

namespace RobotControl.Simulation.Robot.Controller
{
  /// <summary>
  /// Left thumb only robot engines controll class.
  /// </summary>
  /// <remarks>All engines are controlled only by using left thumb on game pad.</remarks>
  /// <seealso cref="RobotControl.Simulation.Robot.Controller.IControllerProcessor" />
  public class LeftThumbControllerProcessor : IControllerProcessor
  {
    /// <summary>
    /// Calculates robot's engines power.
    /// </summary>
    /// <param name="controllerCommand">The controller command.</param>
    /// <param name="leftEnginePower">The left engine power.</param>
    /// <param name="rightEnginePower">The right engine power.</param>
    /// <exception cref="NotImplementedException"></exception>
    public void CalculateEnginesPower(IControllerStateCommand controllerCommand, out int leftEnginePower, out int rightEnginePower)
    {
      var powerL = Math.Abs(controllerCommand.LeftThumb.Y);
      var directionL = Math.Sign(controllerCommand.LeftThumb.Y);
      var powerR = powerL;
      var directionR = directionL;

      var turn = controllerCommand.LeftThumb.X;

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
  }
}
