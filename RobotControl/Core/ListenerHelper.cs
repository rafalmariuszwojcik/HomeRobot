using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RobotControl.Core
{
  public class ListenerHelper : Singleton<ListenerHelper>
  {
    private readonly object listenerInfoLockObject = new object();
    private readonly IDictionary<Type, ListenerInfo> types = new Dictionary<Type, ListenerInfo>();

    private ListenerInfo GetListenerInfo(object listener)
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
