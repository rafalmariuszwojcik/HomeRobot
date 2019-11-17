using RobotControl.Core;
using SharpDX.XInput;
using System;
using System.Drawing;

namespace RobotControl.Communication.Controller
{
  /// <summary>
  /// Game pad state changed event argument.
  /// </summary>
  public interface IGamePadStateChangedEventArgs
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
  
  public class GamePad : WorkerBase
  {
    /// <summary>
    /// The simulation timeout.
    /// </summary>
    /// <remarks>Update controller state 100 times per one second.</remarks>
    private const int UPDATE_TIMEOUT = 10;

    /// <summary>
    /// The deadband value.
    /// </summary>
    private const int DEADBAND = 2500;

    /// <summary>
    /// The controller object.
    /// </summary>
    private readonly SharpDX.XInput.Controller controller;

    /// <summary>
    /// Indicates, is the controller connected.
    /// </summary>
    private readonly bool connected = false;

    /// <summary>
    /// The left thumb and right thumb position.
    /// </summary>
    private Point leftThumb, rightThumb = new Point();

    /// <summary>
    /// The left and right trigger state.
    /// </summary>
    private int leftTrigger, rightTrigger;

    /// <summary>
    /// Occurs when game pad state has changed.
    /// </summary>
    public event EventHandler<IGamePadStateChangedEventArgs> StateChanged;

    /// <summary>
    /// Initializes a new instance of the <see cref="GamePad"/> class.
    /// </summary>
    public GamePad()
      : base(UPDATE_TIMEOUT)
    {
      controller = new SharpDX.XInput.Controller(UserIndex.One);
      connected = controller.IsConnected;
    }

    /// <summary>
    /// Internal work code.
    /// </summary>
    protected override void WorkInternal()
    {
      if (!connected) 
      {
        return;
      }

      var signal = false;
      var gamepad = controller.GetState().Gamepad;

      var leftThumb = new Point
      {
        X = (Math.Abs((double)gamepad.LeftThumbX) < DEADBAND) ? 0 : Convert.ToInt32(Math.Round((double)gamepad.LeftThumbX / short.MinValue * -100.0)),
        Y = (Math.Abs((double)gamepad.LeftThumbY) < DEADBAND) ? 0 : Convert.ToInt32(Math.Round((double)gamepad.LeftThumbY / short.MaxValue * 100.0))
      };

      if (!this.leftThumb.Equals(leftThumb)) 
      {
        this.leftThumb = leftThumb;
        signal = true;
      }

      var rightThumb = new Point
      {
        X = (Math.Abs((double)gamepad.RightThumbX) < DEADBAND) ? 0 : Convert.ToInt32(Math.Round((double)gamepad.RightThumbX / short.MinValue * -100.0)),
        Y = (Math.Abs((double)gamepad.RightThumbY) < DEADBAND) ? 0 : Convert.ToInt32(Math.Round((double)gamepad.RightThumbY / short.MaxValue * 100.0))
      };

      if (!this.rightThumb.Equals(rightThumb))
      {
        this.rightThumb = rightThumb;
        signal = true;
      }

      if (leftTrigger != gamepad.LeftTrigger) 
      {
        leftTrigger = gamepad.LeftTrigger;
        signal = true;
      }

      if (rightTrigger != gamepad.RightTrigger) 
      {
        rightTrigger = gamepad.RightTrigger;
        signal = true;
      }

      if (signal) 
      {
        OnStateChanged();
      }
    }

    /// <summary>
    /// Call state changed event.
    /// </summary>
    private void OnStateChanged()
    {
      StateChanged?.Invoke(this, new GamePadStateChangedEventArgs(leftThumb, rightThumb, leftTrigger, rightTrigger));
    }
  }

  /// <summary>
  /// Game pad changed event arguments.
  /// </summary>
  /// <seealso cref="System.EventArgs" />
  /// <seealso cref="RobotControl.Communication.Controller.IGamePadStateChangedEventArgs" />
  public class GamePadStateChangedEventArgs : EventArgs, IGamePadStateChangedEventArgs
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="GamePadStateChangedEventArgs"/> class.
    /// </summary>
    /// <param name="leftThumb">The left thumb.</param>
    /// <param name="rightThumb">The right thumb.</param>
    /// <param name="leftTrigger">The left trigger.</param>
    /// <param name="rightTrigger">The right trigger.</param>
    public GamePadStateChangedEventArgs(Point leftThumb, Point rightThumb, int leftTrigger, int rightTrigger) 
    {
      LeftThumb = new Point(leftThumb.X, leftThumb.Y);
      RightThumb = new Point(rightThumb.X, rightThumb.Y);
      LeftTrigger = leftTrigger;
      RightTrigger = rightTrigger;
    }

    /// <summary>
    /// Gets the left thumb position.
    /// </summary>
    public Point LeftThumb { get; private set; }

    /// <summary>
    /// Gets the right thumb position.
    /// </summary>
    public Point RightThumb { get; private set; }

    /// <summary>
    /// Gets the left trigger position.
    /// </summary>
    public int LeftTrigger { get; private set; }

    /// <summary>
    /// Gets the right trigger position.
    /// </summary>
    public int RightTrigger { get; private set; }
  }
}