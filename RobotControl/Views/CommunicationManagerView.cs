using RobotControl.Communication;

namespace RobotControl.Views
{
  public partial class CommunicationManagerView : Forms.BaseView
  {
    public CommunicationManagerView()
    {
      InitializeComponent();
      var items = CommunicationManager.Instance.Items;
      
      //CommunicationManager.Instance.Save();
      CommunicationManager.Instance.Load();

      

      dataGridView1.RowHeadersVisible = false;
      dataGridView1.ColumnHeadersVisible = false;
      dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
      dataGridView1.AllowUserToResizeRows = false;
      dataGridView1.DataSource = items;




      //var appSettings = ConfigurationManager.AppSettings;

      //Properties.Settings.Default.


    }
  }
}
