using System.Drawing;

namespace RobotControl.Command.Controller
{
  /// <summary>
  /// Manual controller state command (GamePad).
  /// </summary>
  /// <seealso cref="RobotControl.Command.IControllerCommand" />
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "<Pending>")]
  public struct ControllerStateCommand : IControllerStateCommand
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ControllerStateCommand"/> struct.
    /// </summary>
    /// <param name="leftThumb">The left thumb.</param>
    /// <param name="rightThumb">The right thumb.</param>
    /// <param name="leftTrigger">The left trigger.</param>
    /// <param name="rightTrigger">The right trigger.</param>
    public ControllerStateCommand(Point leftThumb, Point rightThumb, int leftTrigger, int rightTrigger)
    {
      LeftThumb = leftThumb;
      RightThumb = rightThumb;
      LeftTrigger = leftTrigger;
      RightTrigger = rightTrigger;
    }

    /// <summary>
    /// Gets the left thumb.
    /// </summary>
    public Point LeftThumb { get; private set; }

    /// <summary>
    /// Gets the right thumb.
    /// </summary>
    public Point RightThumb { get; private set; }

    /// <summary>
    /// Gets the left trigger.
    /// </summary>
    public int LeftTrigger { get; private set; }

    /// <summary>
    /// Gets the right trigger.
    /// </summary>
    public int RightTrigger { get; private set; }
  }
}