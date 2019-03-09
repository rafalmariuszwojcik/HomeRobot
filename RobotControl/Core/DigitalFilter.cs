using System;

namespace RobotControl.Core
{
  public class DigitalFilter
  {
    private readonly int level;

    public DigitalFilter(int level)
    {
      if (level < 1)
      {
        throw new ArgumentOutOfRangeException();
      }

      this.level = level;
    }

    public double Input
    {
      set
      {
        Output = ((Output * (level - 1)) + value);
        Output = Output / level;
      }
    }

    public double Output { get; private set; } = 0.0;
  }
}
