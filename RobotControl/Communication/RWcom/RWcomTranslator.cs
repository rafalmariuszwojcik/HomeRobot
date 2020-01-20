using RobotControl.Command.Robot;
using RobotControl.Core;

namespace RobotControl.Communication.RWcom
{
  public class RWcomTranslator : DisposableBase
  {
    /// <summary>
    /// Converts commands to string.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <returns></returns>
    public string CommandToString(IRobotCommand command) 
    {
      return string.Empty;
    }

    /// <summary>
    /// Converts strings to command.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <returns></returns>
    public IRobotCommand StringToCommand(string data) 
    {
      return null;
    }
    
    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing) 
      { 
      }
    }
  }
}
