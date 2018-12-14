using System;

namespace RobotControl.Communication
{
  public class CommunicationManager : ICommunicationManager
  {
    private static readonly object syncRoot = new object();
    private static volatile CommunicationManager instance;

    public CommunicationManager()
    {
      //SingletonTearDown.Instance.TearDown += TearDownInstance;
    }

    public static ICommunicationManager Instance
    {
      get
      {
        if (instance == null)
        {
          lock (syncRoot)
          {
            if (instance == null)
            {
              instance = new CommunicationManager();
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