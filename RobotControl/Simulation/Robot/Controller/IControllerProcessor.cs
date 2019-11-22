using RobotControl.Command.Controller;

namespace RobotControl.Simulation.Robot.Controller
{
  /// <summary>
  /// Controller's commands processor.
  /// </summary>
  public interface IControllerProcessor
  {
    /// <summary>
    /// Calculates robot's engines power.
    /// </summary>
    /// <param name="controllerCommand">The controller command.</param>
    /// <param name="leftEnginePower">The left engine power.</param>
    /// <param name="rightEnginePower">The right engine power.</param>
    void CalculateEnginesPower(IControllerStateCommand controllerCommand, out int leftEnginePower, out int rightEnginePower);
  }
}
