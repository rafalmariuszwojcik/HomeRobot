using RobotControl.Communication;
using System.Collections.Generic;

namespace RobotControl.Core
{
  /// <summary>
  /// Listener interface.
  /// </summary>
  /// <remarks>Classes implementing this interface are receivers of broadcasted objects.</remarks>
  /// <typeparam name="T">Type of broadcasted objects.</typeparam>
  public interface IListener<T>
    where T : class
  {
    /// <summary>
    /// Data received function.
    /// </summary>
    /// <param name="channel">The source channel.</param>
    /// <param name="data">The data.</param>
    void DataReceived(IChannel channel, IEnumerable<T> data);
  }
}
