using System;

namespace RobotControl.Core
{
  public abstract class Singleton<T> where T: Singleton<T>
  {
    private static readonly object syncRoot = new object();
    private static volatile T instance;

    public Singleton()
    {
      if (GetType() != typeof(SingletonTearDown))
      {
        SingletonTearDown.Instance.TearDownEvent += TearDownInstance;
      }
    }

    public static T Instance
    {
      get
      {
        if (instance == null)
        {
          lock (syncRoot)
          {
            if (instance == null)
            {
              instance = Activator.CreateInstance<T>();
            }
          }
        }

        return instance;
      }
    }

    protected virtual void TearDown()
    {
    }

    private static void TearDownInstance(object sender, EventArgs e)
    {
      lock (syncRoot)
      {
        if (instance != null)
        {
          instance.TearDown();
        }
      }
    }
  }
}