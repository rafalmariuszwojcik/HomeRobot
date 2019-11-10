using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotControl.Command
{
  public struct ThumbCommand : ICommand
  {
    public float X { get; set; }
    public float Y { get; set; }
  }
}
