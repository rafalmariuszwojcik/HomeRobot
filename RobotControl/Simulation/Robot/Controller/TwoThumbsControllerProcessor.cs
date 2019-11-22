using RobotControl.Command.Controller;

namespace RobotControl.Simulation.Robot.Controller
{
  /// <summary>
  /// Two thumbs robot engines controll class.
  /// </summary>
  /// <remarks>
  /// Forward and backward power is controlled by the left thumb, direction is controlled by the right thumb.
  /// Rotation possible by using right thumb only (when left thumb in neutral position).
  /// </remarks>
  /// <seealso cref="RobotControl.Simulation.Robot.Controller.IControllerProcessor" />
  public class TwoThumbsControllerProcessor : IControllerProcessor
  {
    /// <summary>
    /// Calculates robot's engines power.
    /// </summary>
    /// <param name="controllerCommand">The controller command.</param>
    /// <param name="leftEnginePower">The left engine power.</param>
    /// <param name="rightEnginePower">The right engine power.</param>
    public void CalculateEnginesPower(IControllerStateCommand controllerCommand, out int leftEnginePower, out int rightEnginePower)
    {
      leftEnginePower = 0;
      rightEnginePower = 0;
    }
  }
}
