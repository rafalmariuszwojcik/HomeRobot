using RobotControl.Core;

namespace RobotControl.Communication.Controller
{
  public class Keyboard : WorkerBase
  {
    /// <summary>
    /// The update timeout.
    /// </summary>
    /// <remarks>Update keyboard state 100 times per one second.</remarks>
    private const int UPDATE_TIMEOUT = 10;

    /// <summary>
    /// Initializes a new instance of the <see cref="Keyboard"/> class.
    /// </summary>
    /// <param name="workTimeout"></param>
    public Keyboard() 
      : base(UPDATE_TIMEOUT)
    {
    }

    /// <summary>
    /// Internal work code.
    /// </summary>
    protected override void WorkInternal()
    {
      //Keyboard.IsKeyDown()
    }
  }
}