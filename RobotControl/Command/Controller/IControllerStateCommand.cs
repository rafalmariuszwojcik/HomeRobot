using System.Drawing;

namespace RobotControl.Command.Controller
{
  /// <summary>
  /// Manual controller state command interface.
  /// </summary>
  /// <seealso cref="RobotControl.Command.Controller.IControllerCommand" />
  public interface IControllerStateCommand : IControllerCommand
  {
    /// <summary>
    /// Gets the left thumb position.
    /// </summary>
    Point LeftThumb { get; }

    /// <summary>
    /// Gets the right thumb position.
    /// </summary>
    Point RightThumb { get; }

    /// <summary>
    /// Gets the left trigger position.
    /// </summary>
    int LeftTrigger { get; }

    /// <summary>
    /// Gets the right trigger position.
    /// </summary>
    int RightTrigger { get; }
  }
}