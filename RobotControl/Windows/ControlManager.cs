using RobotControl.Command;
using RobotControl.Core;
using RobotControl.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace RobotControl.Windows
{
  public class ControlManager : Singleton<ControlManager>
  {
    private readonly object lockObject = new object();
    private readonly IList<Control> listeners = new List<Control>();
    private readonly DataProcessingQueue<DataPackage> messageQueue;
    private readonly IList<DisposableBase> dataSupply = new List<DisposableBase>();
    private readonly IDictionary<Type, ControlInfo> types = new Dictionary<Type, ControlInfo>();

    public ControlManager()
    {
      messageQueue = new DataProcessingQueue<DataPackage>(x => PostMessage(x), 20); // 20 = 50Hz
      dataSupply.Add(new MessageListener((s) => messageQueue.Enqueue(new MessagePackage(s))));
      dataSupply.Add(new CommandListener((s) => messageQueue.Enqueue(new CommandPackage(s))));
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
      List<Control> controls;
      lock (lockObject)
      {
        controls = new List<Control>(listeners);
      }

      foreach (var control in controls)
      {
        ProcessControl(control, dataPackages);
      }
    }

    private void ProcessControl(Control control, IEnumerable<DataPackage> dataPackages)
    {
      if (control == null || dataPackages == null || !dataPackages.Any())
      {
        return;
      }

      var controlInfo = GetControlInfo(control);
      foreach (var intf in controlInfo.Interfaces)
      {
        var data = (IList)Activator.CreateInstance(intf.PackageType);
        foreach (var item in dataPackages.Where(x => x.GetType().Equals(intf.DataType)))
        {
          data.Add(item);
        }

        if (control.InvokeRequired)
        {
          try
          {
            control.Invoke(new Action(() => intf.Method.Invoke(control, new object[] { null, data })));
          }
          catch (ObjectDisposedException)
          {
            ;
          }
          catch (InvalidAsynchronousStateException)
          {
            ;
          }
        }
        else
        {
          intf.Method.Invoke(control, new object[] { null, data });
        }
      }
    }

    private ControlInfo GetControlInfo(Control control)
    {
      var controlType = control.GetType();
      if (!types.ContainsKey(controlType))
      {
        var controlInfo = new ControlInfo { Interfaces = new List<InterfaceInfo>() };
        var interfaces = controlType.GetInterfaces().Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IListenerControl<>));
        foreach (var intf in interfaces)
        {
          var dataType = intf.GetGenericArguments()?.Count() == 1 ? intf.GetGenericArguments()[0] : null;
          if (dataType != null)
          {
            var packageType = typeof(List<>).MakeGenericType(dataType);
            var method = intf.GetMethod("MessageReceived");
            if (packageType != null && method != null)
            {
              controlInfo.Interfaces.Add(new InterfaceInfo { DataType = dataType, PackageType = packageType, Method = method });
            }
          }
        }

        types.Add(controlType, controlInfo);
        return controlInfo;
      }

      return types[controlType];
    }

    private class ControlInfo
    {
      internal IList<InterfaceInfo> Interfaces { get; set; }
    }

    private class InterfaceInfo
    {
      internal Type DataType { get; set; }
      internal Type PackageType { get; set; }
      internal MethodInfo Method { get; set; }
    }
  }
}