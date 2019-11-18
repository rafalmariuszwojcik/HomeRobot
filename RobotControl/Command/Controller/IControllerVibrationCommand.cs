using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotControl.Command.Controller
{
  /// <summary>
  /// Manual controller vibration command interface.
  /// </summary>
  /// <seealso cref="RobotControl.Command.Controller.IControllerCommand" />
  public interface IControllerVibrationCommand : IControllerCommand
  {
    /// <summary>
    /// Gets the left motor speed.
    /// </summary>
    /// <remarks>In percentage (0; +100).</remarks>
    int LeftMotorSpeed { get; }

    /// <summary>
    /// Gets the right motor speed.
    /// </summary>
    /// <remarks>In percentage (0; +100).</remarks>
    int RightMotorSpeed { get; }
  }
}
