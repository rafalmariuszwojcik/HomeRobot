using RobotControl.Command.Controller;

namespace RobotControl.Simulation.Robot.Controller
{
  /// <summary>
  /// Two thumbs robot engines controll class.
  /// </summary>
  /// <seealso cref="RobotControl.Simulation.Robot.Controller.ControllerProcessorBase" />
  /// <remarks>
  /// Forward and backward power is controlled by the left thumb, direction is controlled by the right thumb.
  /// Rotation possible by using right thumb only (when left thumb in neutral position).
  /// </remarks>
  /// <seealso cref="RobotControl.Simulation.Robot.Controller.IControllerProcessor" />
  public class TwoThumbsControllerProcessor : ControllerProcessorBase
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
      return controllerCommand.RightThumb.X;
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