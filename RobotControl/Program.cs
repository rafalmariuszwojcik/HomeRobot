using RobotControl.Core;
using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace RobotControl
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
#if DEBUG
      // Add this; Change the Locales(En-US): Done.
      Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
#endif
      Application.ApplicationExit += (sender, e) => SingletonTearDown.Instance.DoEvent();
      Application.Run(new formRobotControl());
    }
  }
}
