//_______________________________________________________________
//  Title   : FormImportDNNRegisteredUsersSettings
//  System  : Microsoft VisualStudio 2015 / C#
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2016, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

using System;
using System.Windows.Forms;

namespace CAS.DNNDataBase.UsersManagement
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
      Close();
    }
    private void button_cancel_Click( object sender, EventArgs e )
    {
      ok = false;
      Close();
    }
  }
}