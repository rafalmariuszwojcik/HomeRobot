using RobotControl.Command;
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
      var cm = CommandManager.Instance;
      Application.Run(new formRobotControl());
    }
  }
}
