using System;
using SharpDX.XInput;

namespace RobotControl
{
  public class XInputController
  {
    Controller controller;
    Gamepad gamepad;
    public bool connected = false;
    public int deadband = 2500;
    public System.Drawing.PointF leftThumb, rightThumb = new System.Drawing.PointF(0, 0);
    public float leftTrigger, rightTrigger;

    public XInputController()
    {
      controller = new Controller(UserIndex.One);
      connected = controller.IsConnected;
    }

    // Call this method to update all class values
    public void Update()
    {
      if (!connected)
        return;

      gamepad = controller.GetState().Gamepad;

      

      leftThumb.X = (Math.Abs((float)gamepad.LeftThumbX) < deadband) ? 0 : (float)gamepad.LeftThumbX / short.MinValue * -100;
      leftThumb.Y = (Math.Abs((float)gamepad.LeftThumbY) < deadband) ? 0 : (float)gamepad.LeftThumbY / short.MaxValue * 100;
      rightThumb.X = (Math.Abs((float)gamepad.RightThumbX) < deadband) ? 0 : (float)gamepad.RightThumbX / short.MaxValue * 100;
      rightThumb.Y = (Math.Abs((float)gamepad.RightThumbY) < deadband) ? 0 : (float)gamepad.RightThumbY / short.MaxValue * 100;

      leftTrigger = gamepad.LeftTrigger;
      rightTrigger = gamepad.RightTrigger;
    }
  }
}
