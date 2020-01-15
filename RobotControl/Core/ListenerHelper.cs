using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RobotControl.Core
{
  /// <summary>
  /// Helper class to support data listeners objects.
  /// </summary>
  /// <seealso cref="RobotControl.Core.Singleton{RobotControl.Core.ListenerHelper}" />
  public class ListenerHelper : Singleton<ListenerHelper>
  {
    /// <summary>
    /// The listener information lock object.
    /// </summary>
    private readonly object listenerInfoLockObject = new object();

    /// <summary>
    /// The recognized types cache.
    /// </summary>
    private readonly IDictionary<Type, ListenerInfo> types = new Dictionary<Type, ListenerInfo>();

    /// <summary>
    /// Prepares list of actions to execute on listener to send all data to it.
    /// </summary>
    /// <param name="listener">The listener.</param>
    /// <param name="data">The data.</param>
    /// <returns>List of actions to call.</returns>
    public IEnumerable<Action> DataReceivedActions(IListener listener, IEnumerable<object> data) 
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

    /// <summary>
    /// Discover all <see cref="IListener{T}"/> interfaces for instance.
    /// </summary>
    /// <param name="listener">The listener.</param>
    /// <returns>Listener information.</returns>
    private ListenerInfo GetListenerInfo(IListener listener)
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

    /// <summary>
    /// Data listener information.
    /// </summary>
    private class ListenerInfo
    {
      /// <summary>
      /// Gets or sets the interfaces.
      /// </summary>
      /// <remarks>List of all <see cref="IListener{T}" interfaces supported by type./></remarks>
      internal IList<InterfaceInfo> Interfaces { get; set; }
    }

    /// <summary>
    /// Discovered data listener interface information.
    /// </summary>
    private class InterfaceInfo
    {
      /// <summary>
      /// Gets or sets the type of the data.
      /// </summary>
      internal Type DataType { get; set; }

      /// <summary>
      /// Gets or sets the type of the package.
      /// </summary>
      /// <remarks>Package is a generic <see cref="List{T}"/> of <see cref="DataType"/> items.</remarks>
      internal Type PackageType { get; set; }

      /// <summary>
      /// Gets or sets the method.
      /// </summary>
      /// <remarks>Reference to interface <see cref="IListener{T}.DataReceived(Communication.IChannel, IEnumerable{T})"/> method.</remarks>
      internal MethodInfo Method { get; set; }
    }
  }
}