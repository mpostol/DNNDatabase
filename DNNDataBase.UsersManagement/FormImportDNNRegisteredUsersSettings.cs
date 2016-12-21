using System;
using System.Windows.Forms;

namespace import
{
  public partial class FormImportDNNRegisteredUsersSettings: Form
  {
    internal bool ok=false;
    public FormImportDNNRegisteredUsersSettings()
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