using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace import
{
  public partial class FormImportStatystykiSettings: Form
  {
    internal bool ok=false;
    public FormImportStatystykiSettings()
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