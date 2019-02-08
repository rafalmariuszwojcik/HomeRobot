using RobotControl.Communication;
using RobotControl.Core;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels;
using System.Text;

namespace RobotControl.Windows.Views
{
  public partial class OutputView : BaseView, IListenerControl<MessagePackage>
  {
    const int MAX_BUFFER_LENGHT = 4096;
    const int REFRESH_FREQUENCY = 50; // Hz
    const int REFRESH_MILIS = 1000 / REFRESH_FREQUENCY;
    private StringBuilder buffer = new StringBuilder(MAX_BUFFER_LENGHT, MAX_BUFFER_LENGHT);

    public OutputView()
    {
      InitializeComponent();
    }

    //DateTime next = DateTime.Now.AddMilliseconds(REFRESH_MILIS);

    protected override void MessageReceived(object sender, string message)
    {
      /*
      if (message == null)
      {
        return;
      }

      var freePlace = MAX_BUFFER_LENGHT - buffer.Length;
      if (freePlace < message.Length)
      {
        //buffer.
        buffer.Remove(0, Math.Min(message.Length - freePlace, buffer.Length));
        freePlace = MAX_BUFFER_LENGHT - buffer.Length;
      }

      buffer.Append(message.Length > MAX_BUFFER_LENGHT ? message.Substring(message.Length - MAX_BUFFER_LENGHT, MAX_BUFFER_LENGHT) : message);

      if (DateTime.Now > next)
      {
        uxOutputText.Text = buffer.ToString();
        next = DateTime.Now.AddMilliseconds(REFRESH_MILIS);
      }
      */



      //uxOutputText.AppendText(message);
    }

    public void MessageReceived(Communication.IChannel channel, IEnumerable<MessagePackage> data)
    {
      foreach (var item in data)
      {
        ProcessMessages(null, item.Message);
      }

      uxOutputText.Text = buffer.ToString();
    }

    private void ProcessMessages(object sender, string message)
    {
      if (message == null)
      {
        return;
      }

      var freePlace = MAX_BUFFER_LENGHT - buffer.Length;
      if (freePlace < message.Length)
      {
        //buffer.
        buffer.Remove(0, Math.Min(message.Length - freePlace, buffer.Length));
        freePlace = MAX_BUFFER_LENGHT - buffer.Length;
      }

      buffer.Append(message.Length > MAX_BUFFER_LENGHT ? message.Substring(message.Length - MAX_BUFFER_LENGHT, MAX_BUFFER_LENGHT) : message);

      /*
      if (DateTime.Now > next)
      {
        uxOutputText.Text = buffer.ToString();
        next = DateTime.Now.AddMilliseconds(REFRESH_MILIS);
      }*/
    }
  }
}