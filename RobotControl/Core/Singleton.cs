using System;

namespace RobotControl.Core
{
  public class Singleton<T> where T: class
  {
    private static readonly object syncRoot = new object();
    private static volatile T instance;

    public Singleton()
    {
      //SingletonTearDown.Instance.TearDown += TearDownInstance;
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

    private static void TearDownInstance(object sender, EventArgs e)
    {
      instance = null;
    }
  }
}