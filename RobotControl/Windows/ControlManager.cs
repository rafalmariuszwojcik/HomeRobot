using RobotControl.Command;
using RobotControl.Core;
using RobotControl.Messages;
using RobotControl.Simulation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace RobotControl.Windows
{
  public class ControlManager : ManagerBase<ControlManager, Control, DataPackage>
  {
    const int UPDATE_FREQUENCY = 20; // refresh frequency = 50Hz.

    private readonly object listenerInfoLockObject = new object();
    private readonly IDictionary<Type, ListenerInfo> types = new Dictionary<Type, ListenerInfo>();

    public ControlManager()
      : base(UPDATE_FREQUENCY)
    {
      Disposables.Add(new MessageListener((s) => DataReceived(null, s.Select(x => new MessagePackage(x)))));
      Disposables.Add(new CommandListener((s) => DataReceived(null, s.Select(x => new CommandPackage(x)))));
      Disposables.Add(new SimulationListener((s) => DataReceived(null, s.Select(x => new SimulationPackage(x)))));
    }

    protected override void SendData(Control listener, Action action)
    {
      if (!ControlHelper.InvokeAction(listener, action))
      {
        base.SendData(listener, action);
      }
    }

    protected override IEnumerable<Action> SendDataActions(Control listener, IEnumerable<DataPackage> data)
    {
      var result = new List<Action>();
      var listenerInfo = GetListenerInfo(listener);
      foreach (var intf in listenerInfo.Interfaces)
      {
        var dataToSend = (IList)Activator.CreateInstance(intf.PackageType);
        foreach (var item in data.Where(x => x.GetType().Equals(intf.DataType) || x.GetType().GetInterfaces().Contains(intf.DataType)))
        {
          dataToSend.Add(item);
        }

        var action = new Action(() => intf.Method.Invoke(listener, new object[] { null, dataToSend }));
        result.Add(action);
      }

      return result;
    }

    private ListenerInfo GetListenerInfo(Control listener)
    {
      lock (listenerInfoLockObject)
      {
        var listenerType = listener.GetType();
        if (!types.ContainsKey(listenerType))
        {
          var listenerInfo = new ListenerInfo { Interfaces = new List<InterfaceInfo>() };
          var interfaces = listenerType.GetInterfaces().Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IListener<>));
          foreach (var intf in interfaces)
          {
            var dataType = intf.GetGenericArguments()?.Count() == 1 ? intf.GetGenericArguments()[0] : null;
            if (dataType != null)
            {
              var packageType = typeof(List<>).MakeGenericType(dataType);
              var method = intf.GetMethod(nameof(IListener<object>.DataReceived));
              if (packageType != null && method != null)
              {
                listenerInfo.Interfaces.Add(new InterfaceInfo { DataType = dataType, PackageType = packageType, Method = method });
              }
            }
          }

          types.Add(listenerType, listenerInfo);
          return listenerInfo;
        }

        return types[listenerType];
      }
    }

    private class ListenerInfo
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