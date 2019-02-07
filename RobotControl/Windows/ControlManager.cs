using RobotControl.Core;
using RobotControl.Messages;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RobotControl.Windows
{
  public class ControlManager : Singleton<ControlManager>
  {
    private readonly object lockObject = new object();
    private readonly IList<Control> listeners = new List<Control>();
    private readonly DataProcessingQueue<DataPackage> messageQueue;
    private readonly IList<DisposableBase> dataSupply = new List<DisposableBase>();

    public ControlManager()
    {
      messageQueue = new DataProcessingQueue<DataPackage>(x => PostMessage(x), 20);
      dataSupply.Add(new MessageListener((s) => messageQueue.Enqueue(new MessagePackage(s))));
    }

    public void RegisterListener(Control listener)
    {
      lock (lockObject)
      {
        if (listener != null && !listeners.Contains(listener))
        {
          listeners.Add(listener);
        }
      }
    }

    public void UnregisterListener(Control listener)
    {
      lock (lockObject)
      {
        if (listeners.Contains(listener))
        {
          listeners.Remove(listener);
        }
      }
    }

    protected override void TearDown()
    {
      foreach (var disposable in dataSupply)
      {
        disposable.Dispose();
      }

      messageQueue?.Dispose();
      base.TearDown();
    }

    private void PostMessage(IEnumerable<DataPackage> dataPackages)
    {
    }
  }
}