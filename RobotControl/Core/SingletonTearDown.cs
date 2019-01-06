using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RobotControl.Core
{
  public class SingletonTearDown : Singleton<SingletonTearDown>
  {
    private bool tearDownInProgress;
    private readonly LinkedList<EventHandler<EventArgs>> tearDownEvents = new LinkedList<EventHandler<EventArgs>>();

    public SingletonTearDown()
    {
    }

    public event EventHandler<EventArgs> TearDownEvent
    {
      add
      {
        if (tearDownInProgress)
        {
          throw new InvalidOperationException("Registration of a singleton instance during SingletonTearDown.DoEvent(). Maybe some singletons depend on each other!");
        }

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
            catch (Exception e)
            {
              ;
            }
          }
        });

/*
        while (!task.IsCompleted)
        {
          // We call DoEvents here, so that pending invokes on the main thread can be executed.
          // E.g. some objects want to access a service (e.g. to unregister from notification events)
          // and getting a service is only possible through the main thread.
          Application.DoEvents();
        }
        */
        tearDownEvents.Clear();
      }
      finally
      {
        tearDownInProgress = false;
      }
    }
  }
}
