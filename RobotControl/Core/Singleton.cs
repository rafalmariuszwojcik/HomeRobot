using System;

namespace RobotControl.Core
{
  /// <summary>
  /// Base singleton class.
  /// </summary>
  /// <typeparam name="T">Type of singleton class.</typeparam>
  public abstract class Singleton<T> where T : Singleton<T>
  {
    /// <summary>
    /// Threads synchronization object.
    /// </summary>
    private static readonly object syncObject = new object();

    /// <summary>
    /// The singleton instance.
    /// </summary>
    private static volatile T instance;

    /// <summary>
    /// Initializes a new instance of the <see cref="Singleton{T}"/> class.
    /// </summary>
    public Singleton()
    {
      if (GetType() != typeof(SingletonTearDown))
      {
        SingletonTearDown.Instance.TearDownEvent += TearDownInstance;
      }
    }

    /// <summary>
    /// Gets the singleton instance.
    /// </summary>
    public static T Instance
    {
      get
      {
        if (instance == null)
        {
          lock (syncObject)
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

    /// <summary>
    /// Tears down the instance.
    /// </summary>
    protected virtual void TearDown()
    {
    }

    /// <summary>
    /// Tears down the instance.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    private static void TearDownInstance(object sender, EventArgs e)
    {
      lock (syncObject)
      {
        if (instance != null)
        {
          instance.TearDown();
        }
      }
    }
  }
}