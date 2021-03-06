﻿using RobotControl.Command;
using WeifenLuo.WinFormsUI.Docking;

namespace RobotControl.Windows.Views
{
  public class BaseView : DockContent
  {
    //private readonly ListenerControl listener;

    public BaseView()
    {
      //listener = new ListenerControl(this);
      //listener.OnMessageReceived += (s, e) => MessageReceived(s, e.Message);
      //listener.OnCommandReceived += (s, e) => CommandReceived(s, e.Command);
      Text = GetType().Name;
      ControlManager.Instance.RegisterListener(this);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        ControlManager.Instance.UnregisterListener(this);
        //listener?.Dispose();
      }

      base.Dispose(disposing);
    }

    protected virtual void MessageReceived(object sender, string message)
    {
    }

    protected virtual void CommandReceived(object sender, ICommand command)
    {
    }
  }
}