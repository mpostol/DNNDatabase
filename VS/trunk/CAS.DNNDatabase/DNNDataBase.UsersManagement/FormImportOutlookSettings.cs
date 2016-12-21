
using System;
using System.Windows.Forms;

namespace CAS.DNNDataBase.UsersManagement
{
  public partial class FormImportOutlookSettings: Form
  {
    internal bool ok=false;
    public FormImportOutlookSettings()
    {
      InitializeComponent();
    }

    private void button_ok_Click( object sender, EventArgs e )
    {
      ok = true;
      this.Close();
    }

    private void button_cancel_Click( object sender, EventArgs e )
    {
      ok = false;
      this.Close();
    }
  }
}