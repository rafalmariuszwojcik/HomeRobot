using RobotControl.Core;
using SharpDX.XInput;

namespace RobotControl.Communication.Controller
{
  public class GamePad : WorkerBase
  {
    /// <summary>
    /// The controller object.
    /// </summary>
    private readonly SharpDX.XInput.Controller controller;

    /// <summary>
    /// Indicates, is the controller connected.
    /// </summary>
    private readonly bool connected = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="GamePad"/> class.
    /// </summary>
    public GamePad()
      : base(10)
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
    }
  }
}
