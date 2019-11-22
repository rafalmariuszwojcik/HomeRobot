using RobotControl.Command.Controller;

namespace RobotControl.Simulation.Robot.Controller
{
  /// <summary>
  /// Left thumb only robot engines controll class.
  /// </summary>
  /// <seealso cref="RobotControl.Simulation.Robot.Controller.ControllerProcessorBase" />
  /// <remarks>
  /// All engines are controlled only by using left thumb on game pad.
  /// </remarks>
  /// <seealso cref="RobotControl.Simulation.Robot.Controller.IControllerProcessor" />
  public class LeftThumbControllerProcessor : ControllerProcessorBase
  {
    /// <summary>
    /// Gets the X axis power.
    /// </summary>
    /// <param name="controllerCommand">The controller command.</param>
    /// <returns>
    /// X axis power (-100; 100).
    /// </returns>
    protected override int GetX(IControllerStateCommand controllerCommand)
    {
      return controllerCommand.LeftThumb.X;
    }

    /// <summary>
    /// Gets the Y axis power.
    /// </summary>
    /// <param name="controllerCommand">The controller command.</param>
    /// <returns>
    /// Y axis power (-100; 100).
    /// </returns>
    protected override int GetY(IControllerStateCommand controllerCommand)
    {
      return controllerCommand.LeftThumb.Y;
    }
  }
}
