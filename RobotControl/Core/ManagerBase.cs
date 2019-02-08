using RobotControl.Communication;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RobotControl.Core
{
  public abstract class ManagerBase<S, T, M> : Singleton<S>, IManager<T, M>
    where S : Singleton<S>
    where T : class
    where M : class
  {
    private readonly object lockObject = new object();
    private readonly IList<T> listeners = new List<T>();
    private readonly DataProcessingQueue<M> messageQueue;
    private readonly IDictionary<Type, ListenerInfo> types = new Dictionary<Type, ListenerInfo>();

    protected ManagerBase(int interval)
    {
      messageQueue = new DataProcessingQueue<M>(x => PostMessage(null, x), interval);
    }

    public void MessageReceived(object sender, M message)
    {
      messageQueue.Enqueue(message);
    }

    public void RegisterListener(T listener)
    {
      lock (lockObject)
      {
        if (listener != null && !listeners.Contains(listener))
        {
          listeners.Add(listener);
        }
      }
    }

    public void UnregisterListener(T listener)
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
      messageQueue?.Dispose();
      base.TearDown();
    }

    protected virtual void SendData(T listener, Action action)
    {
      action?.Invoke();
    }

    private void PostMessage(IChannel channel, IEnumerable<M> data)
    {
      List<T> listenersToProcess;
      lock (lockObject)
      {
        listenersToProcess = new List<T>(listeners);
      }

      foreach (var listener in listenersToProcess)
      {
        ProcessListener(listener, data);
      }
    }

    private void ProcessListener(T listener, IEnumerable<M> data)
    {
      if (listener == null || data == null || !data.Any())
      {
        return;
      }

      var listenerInfo = GetListenerInfo(listener);
      foreach (var intf in listenerInfo.Interfaces)
      {
        var dataToSend = (IList)Activator.CreateInstance(intf.PackageType);
        foreach (var item in data.Where(x => x.GetType().Equals(intf.DataType)))
        {
          dataToSend.Add(item);
        }

        var action = new Action(() => intf.Method.Invoke(listener, new object[] { null, dataToSend }));
        SendData(listener, action);


        /*
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
        */
      }
    }

    private ListenerInfo GetListenerInfo(T listener)
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
            var method = intf.GetMethod("MessageReceived");
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