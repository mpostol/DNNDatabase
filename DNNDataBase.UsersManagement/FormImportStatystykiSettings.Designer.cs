namespace import
{
  partial class FormImportStatystykiSettings
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose( bool disposing )
    {
      if ( disposing && ( components != null ) )
      {
        components.Dispose();
      }
      base.Dispose( disposing );
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.label1 = new System.Windows.Forms.Label();
      this.textBox_kraj = new System.Windows.Forms.TextBox();
      this.textBox_kampania = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.button_ok = new System.Windows.Forms.Button();
      this.button_cancel = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point( 21, 20 );
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size( 34, 13 );
      this.label1.TabIndex = 0;
      this.label1.Text = "jêzyk:";
      // 
      // textBox_kraj
      // 
      this.textBox_kraj.Location = new System.Drawing.Point( 107, 17 );
      this.textBox_kraj.Name = "textBox_kraj";
      this.textBox_kraj.Size = new System.Drawing.Size( 100, 20 );
      this.textBox_kraj.TabIndex = 1;
      this.textBox_kraj.Text = "PL";
      // 
      // textBox_kampania
      // 
      this.textBox_kampania.Location = new System.Drawing.Point( 107, 66 );
      this.textBox_kampania.Name = "textBox_kampania";
      this.textBox_kampania.Size = new System.Drawing.Size( 100, 20 );
      this.textBox_kampania.TabIndex = 1;
      this.textBox_kampania.Text = "CAS NEWS";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point( 21, 69 );
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size( 57, 13 );
      this.label3.TabIndex = 2;
      this.label3.Text = "Kampania:";
      // 
      // button_ok
      // 
      this.button_ok.Location = new System.Drawing.Point( 24, 98 );
      this.button_ok.Name = "button_ok";
      this.button_ok.Size = new System.Drawing.Size( 75, 23 );
      this.button_ok.TabIndex = 3;
      this.button_ok.Text = "OK";
      this.button_ok.UseVisualStyleBackColor = true;
      this.button_ok.Click += new System.EventHandler( this.button_ok_Click );
      // 
      // button_cancel
      // 
      this.button_cancel.Location = new System.Drawing.Point( 107, 98 );
      this.button_cancel.Name = "button_cancel";
      this.button_cancel.Size = new System.Drawing.Size( 75, 23 );
      this.button_cancel.TabIndex = 3;
      this.button_cancel.Text = "Cancel";
      this.button_cancel.UseVisualStyleBackColor = true;
      this.button_cancel.Click += new System.EventHandler( this.button_cancel_Click );
      // 
      // FormImportStatystykiSettings
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size( 276, 133 );
      this.Controls.Add( this.button_cancel );
      this.Controls.Add( this.button_ok );
      this.Controls.Add( this.label3 );
      this.Controls.Add( this.textBox_kampania );
      this.Controls.Add( this.textBox_kraj );
      this.Controls.Add( this.label1 );
      this.Name = "FormImportStatystykiSettings";
      this.Text = "FormImportStatystykiSettings";
      this.ResumeLayout( false );
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label3;
    internal System.Windows.Forms.TextBox textBox_kraj;
    internal System.Windows.Forms.TextBox textBox_kampania;
    private System.Windows.Forms.Button button_ok;
    private System.Windows.Forms.Button button_cancel;
  }
}