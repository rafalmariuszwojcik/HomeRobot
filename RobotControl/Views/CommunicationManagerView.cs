using RobotControl.Communication;

namespace RobotControl.Views
{
  public partial class CommunicationManagerView : Forms.BaseView
  {
    public CommunicationManagerView()
    {
      InitializeComponent();
      var items = CommunicationManager.Instance.Items;
      dataGridView1.DataSource = items;
      CommunicationManager.Instance.Save();



      //var appSettings = ConfigurationManager.AppSettings;

      //Properties.Settings.Default.


    }
  }
}
