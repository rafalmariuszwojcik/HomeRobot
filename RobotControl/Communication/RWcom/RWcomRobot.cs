using RobotControl.Command.Robot;
using RobotControl.Core;
using System;
using System.Collections.Generic;

namespace RobotControl.Communication.RWcom
{
  /// <summary>
  /// Communication channel with robots of type RWcom.
  /// </summary>
  /// <seealso cref="RobotControl.Communication.ChannelBase{RobotControl.Communication.RWcom.CommandEncoder, RobotControl.Command.Robot.IRobotCommand, RobotControl.Communication.IConfiguration}" />
  /// <seealso cref="RobotControl.Core.IListener{RobotControl.Command.Robot.IRobotCommand}" />
  /// <seealso cref="RobotControl.Core.IListener{System.String}" />
  public class RWcomRobot : ChannelBase<RWcomTranslator, IRobotCommand, IConfiguration>, IListener<IRobotCommand>, IListener<string>
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="RWcomRobot"/> class.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    public RWcomRobot(IConfiguration configuration) : base(configuration)
    {
    }

    /// <summary>
    /// Internal close communication channel.
    /// </summary>
    /// <param name="channel">The channel.</param>
    protected override void InternalClose(RWcomTranslator channel)
    {
      ;
    }

    /// <summary>
    /// Internal open communication channel method.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <returns>
    /// Reference to communication object.
    /// </returns>
    protected override RWcomTranslator InternalOpen(IConfiguration configuration)
    {
      return new RWcomTranslator();
    }

    protected override void InternalSend(RWcomTranslator channel, IRobotCommand data)
    {
      throw new NotImplementedException();
    }

    void IListener<IRobotCommand>.DataReceived(IChannel channel, IEnumerable<IRobotCommand> data)
    {
      if (Active) 
      {
       // this.OnDataReceived((IRobotCommand)null);
      }
    }

    void IListener<string>.DataReceived(IChannel channel, IEnumerable<string> data)
    {
      if (Active)
      {
      }
    }
  }
}
