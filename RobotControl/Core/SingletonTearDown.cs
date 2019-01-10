using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RobotControl.Core
{
  public class SingletonTearDown : Singleton<SingletonTearDown>
  {
    private bool tearDownInProgress;
    private readonly LinkedList<EventHandler<EventArgs>> tearDownEvents = new LinkedList<EventHandler<EventArgs>>();

    public event EventHandler<EventArgs> TearDownEvent
    {
      add
      {
        if (tearDownInProgress)
        {
          throw new InvalidOperationException("Registration of a singleton instance during SingletonTearDown.DoEvent(). Maybe some singletons depend on each other!");
        }

        //tearDownEvents.AddLast(value);
        tearDownEvents.AddFirst(value);
      }

      remove
      {
        tearDownEvents.Remove(value);
      }
    }

    public void DoEvent()
    {
      tearDownInProgress = true;
      try
      {
        var task = Task.Run(() =>
        {
          foreach (var handler in tearDownEvents.ToList())
          {
            try
            {
              handler(this, EventArgs.Empty);
            }
            catch (Exception)
            {
              #if DEBUG
              throw;
              #endif
            }
          }
        });

        while (!task.IsCompleted)
        {
          Application.DoEvents();
        }

        tearDownEvents.Clear();
      }
      finally
      {
        tearDownInProgress = false;
      }
    }
  }
}