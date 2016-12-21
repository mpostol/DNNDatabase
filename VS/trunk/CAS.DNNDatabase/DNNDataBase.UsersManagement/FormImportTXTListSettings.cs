using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace import
{
  public partial class FormImportTXTListSettings: Form
  {
    internal bool ok=false;
    public FormImportTXTListSettings()
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

    private void button_browse_Click( object sender, EventArgs e )
    {
      if ( openFileDialog.ShowDialog( this ) == DialogResult.OK )
      {
        this.textBox_file.Text = openFileDialog.FileName;
      }
    }
  }
}