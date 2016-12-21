//_______________________________________________________________
//  Title   : FormMain
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
using System.IO;
using System.Xml;
using System.Text;
using CAS.DNNDataBase.UsersManagement.Properties;

namespace CAS.DNNDataBase.UsersManagement
{
  public partial class FormMain : Form
  {

    public FormMain()
    {
      InitializeComponent();
    }
    private void Form1_Load(object sender, EventArgs e)
    {
      // TODO: This line of code loads data into the 'databaseDNNDataSet.DNNRegisteredUsers' table. You can move, or remove it, as needed.
      this.dNNRegisteredUsersTableAdapter.Fill(this.databaseDNNDataSet.DNNRegisteredUsers);
      // TODO: This line of code loads data into the 'kontaktyDataSet.Kontakty' table. You can move, or remove it, as needed.
      //this.kontaktyTableAdapter.Fill( this.kontaktyDataSet.Kontakty );
      // TODO: This line of code loads data into the 'databaseDNNDataSet.dnn1_OnyakNEOptIns' table. You can move, or remove it, as needed.
      this.dnn1_OnyakNEOptInsTableAdapter.Fill(this.databaseDNNDataSet.dnn1_OnyakNEOptIns);
      // TODO: This line of code loads data into the 'databaseDNNDataSet.dnn1_OnyakNECampaign' table. You can move, or remove it, as needed.
      this.dnn1_OnyakNECampaignTableAdapter.Fill(this.databaseDNNDataSet.dnn1_OnyakNECampaign);
    }

    #region metody
    private void WriteXML()
    {
      FileStream myFileStream = new FileStream(saveFileDialog1.FileName.ToString(), System.IO.FileMode.Create, FileAccess.ReadWrite);
      //Create an XmlTextWriter with the fileStream.
      using (XmlTextWriter myXmlWriter = new XmlTextWriter(myFileStream, System.Text.Encoding.GetEncoding(1250)))
      {
        myXmlWriter.WriteStartDocument();
        myXmlWriter.WriteProcessingInstruction("xml-stylesheet", "type=\"text/xsl\" href=\"arkusz_stylu.xsl\"");
        myXmlWriter.Formatting = Formatting.Indented;
        kontakty.WriteXml(myXmlWriter);
        myXmlWriter.Close();
      }
    }
    private void ReadXML()
    {
      FileStream myFileStream = new FileStream(openFileDialog1.FileName.ToString(), FileMode.Open, FileAccess.Read);
      //Create an XmlTextReader with the fileStream.
      using (XmlTextReader myXmlReader = new XmlTextReader(myFileStream))
        kontakty.ReadXml(myXmlReader);
    }
    private static string GetAndMoveNextElement(ref string inputstring)
    {
      string ret;
      int pos = inputstring.IndexOf(",");
      ret = inputstring.Substring(0, pos);
      inputstring = inputstring.Remove(0, pos + 1);
      return ret;
    }
    private static string ReadFileCSV(string filename)
    {
      using (StreamReader plik = new StreamReader(filename))
      {
        //,System.Text.Encoding.Default);
        string ret = plik.ReadToEnd();
        return ret;
      }
    }
    private static string GetAndMoveNextElementTXT(ref string inputstring)
    {
      string ret = "0";
      do
      {
        int pos = inputstring.IndexOf("@");
        if (pos > 0)
        {
          ret = inputstring.Substring(0, pos + 1);
          if (ret.LastIndexOf(" ") > 0)
          {
            int lastSP = ret.LastIndexOf(" ");
            inputstring = inputstring.Remove(0, lastSP);
          }
          int SP = inputstring.IndexOf(" ");
          ret = inputstring.Substring(0, SP);
          ret = ret.TrimEnd();
          inputstring = inputstring.Remove(0, SP);
          inputstring = inputstring.TrimStart();
        }
        else
        {
          int leng = inputstring.Length;
          inputstring = inputstring.Remove(0, leng);
        }
      }
      while (ret == "");
      return ret;
    }
    private static string PrepareForCSVProcessing(string inputstring)
    {
      string ret;
      int pos = inputstring.IndexOf("\r\n");
      ret = inputstring.Remove(0, pos + 2);
      ret = ret.Replace("\"\r\n", "\",");
      ret = ret.Replace("\r\n", "");
      return ret;
    }
    #endregion

    #region event handlers

    #region buttons
    private void buttonOpen_Click(object sender, EventArgs e)
    {
      openFileDialog1.FileName = "statystyki";
      openFileDialog1.DefaultExt = "xml";
      openFileDialog1.Filter = "XML files (*.xml)|*.xml";
      openFileDialog1.InitialDirectory = "c:\\";
      if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
        ReadXML();
    }
    private void buttonClean_Click(object sender, EventArgs e)
    {
      kontakty.clean();
    }
    private void buttonSave_Click(object sender, EventArgs e)
    {
      saveFileDialog1.FileName = "statystyki";
      saveFileDialog1.DefaultExt = "xml";
      saveFileDialog1.Filter = "XML files (*.xml)|*.xml";
      saveFileDialog1.InitialDirectory = "c:\\";
      if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
        WriteXML();
    }
    private void buttonGenerujCSV_Click(object sender, EventArgs e)
    {
      saveFileDialog1.FileName = "queriesCSV";
      saveFileDialog1.DefaultExt = ".sql";
      saveFileDialog1.Filter = "TXT files (*.sql)|*.sql";
      saveFileDialog1.InitialDirectory = "c:\\";
      if (this.saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        string email;
        string name = "";
        string kraj = "";
        string metoda = "email";
        string kategoria;
        string czy_otrzym = "tak";
        StreamWriter f = new StreamWriter(saveFileDialog1.FileName.ToString());
        string use = string.Format("use dnn;");
        f.WriteLine(use);
        string createTab = String.Format("CREATE TABLE CAS_kontakty (CustID int identity(1,1) PRIMARY KEY, mail nvarchar (50) not null unique,metoda nvarchar(10),jezyk nvarchar (3),otrzym nvarchar(3),nazw_imie nvarchar(200),grupa nvarchar(20),data_wstawienia smalldatetime );");
        f.WriteLine(createTab);
        foreach (kontaktycsv.osobycsv1Row i in kontaktycsv.osobycsv1.Rows)
        {
          email = i._Adres_e_mail.ToString();
          email = email.ToLower();
          if (email.Contains(".pl") || email.Contains("-pl") || email.Contains("aiut") || email.Contains("sieradz") || email.Contains("michal")
              || email.Contains("joanna") || email.Contains("polish") || email.Contains("polska") || email.Contains("hubert") || email.Contains("monika")
              || email.Contains("andrzej") || email.Contains("tomasz") || email.Contains("zbigniew") || email.Contains("piotr") || email.Contains("ryszard")
              || email.Contains("wojciech") || email.Contains("waldemar") || email.Contains("poczta") || email.Contains("tadeusz") || email.Contains("pec")
              || email.Contains("pawel") || email.Contains("jacek") || email.Contains("mariusz") || email.Contains("dariusz"))
          {
            kraj = "pl";
          }
          else
          {
            kraj = "";
          };
          kategoria = "Outlook_" + i.kategoria.ToString();
          string query = String.Format("INSERT INTO CAS_kontakty VALUES(\'{0}\',\'{1}\',\'{2}\',\'{3}\',\'{4}\','{5}',getdate());", email, metoda, kraj, czy_otrzym, name, kategoria);
          f.WriteLine(query);
        };
        string select = string.Format("Select * from CAS_kontakty;");
        f.WriteLine(select);
        f.Close();
      }
    }
    private void buttonOpenTXT_Click(object sender, EventArgs e)
    {
      openFileDialog1.FileName = "kontakty";
      openFileDialog1.DefaultExt = "txt";
      openFileDialog1.Filter = "TXT files (*.txt)|*.txt| MBOX files(*.mbox)|*.mbox";
      openFileDialog1.InitialDirectory = "c:\\";
      if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
      {
        StreamReader plik = new StreamReader(openFileDialog1.FileName);//,System.Text.Encoding.Default);
        string ret = plik.ReadToEnd();
        ret = ret.Replace("\r\n", " ");
        ret = ret.Replace("@CAS_001", " ");
        ret = ret.Replace("<", " ");
        ret = ret.Replace(">", " ");
        ret = ret.Replace("]", " ");
        ret = ret.Replace("[", " ");
        ret = ret.Replace("=", " ");
        ret = ret.Replace("'", " ");
        ret = ret.Replace(":", " ");
        int duplikaty = 0;
        int ogolna_ilosc = 0;
        //MessageBox.Show(ret.Length.ToString());
        while (ret.Length > 1)
        {
          string adres_email = GetAndMoveNextElementTXT(ref ret);
          ogolna_ilosc++;
          adres_email = adres_email.Replace("\"", "");
          adres_email = adres_email.Replace(";", "");
          adres_email = adres_email.Replace(",", "");
          adres_email = adres_email.Replace("..", "");
          adres_email = adres_email.Replace("...", "");
          int indeks = openFileDialog1.FileName.LastIndexOf("\\");
          string kategoria = "nie_dostali_maila";
          try
          {
            kontaktycsv.osobycsv1.Addosobycsv1Row(adres_email, kategoria);
          }
          catch
          {
            duplikaty++;
          };
        }
        int ilosc = ogolna_ilosc - duplikaty;
        MessageBox.Show(duplikaty.ToString() + " ilosc wykrytych duplikatow maili");
        MessageBox.Show(ogolna_ilosc.ToString() + " ogolna ilosc znalezionych maili");
        MessageBox.Show(ilosc.ToString() + " ilosc osob do ktorych nie dotar³ mail");
      }
    }
    private void buttonPlikBazy_Click(object sender, EventArgs e)
    {
      saveFileDialog1.FileName = "queriesXML";
      saveFileDialog1.DefaultExt = ".sql";
      saveFileDialog1.Filter = "TXT files (*.sql)|*.sql";
      saveFileDialog1.InitialDirectory = "c:\\";
      if (this.saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        string email;
        string name;
        string kraj;
        string recive;
        string met;
        int index = 1;
        StreamWriter f = new StreamWriter(saveFileDialog1.FileName.ToString());
        string use = string.Format("use dnn;");
        f.WriteLine(use);
        string createTab = String.Format("CREATE TABLE CAS_kontakty (CustID int identity(1,1) PRIMARY KEY, mail nvarchar (50) not null unique,metoda nvarchar(10),jezyk nvarchar (3),otrzym nvarchar(3),nazw_imie nvarchar(200),grupa nvarchar(20),data_wstawienia smalldatetime );");
        f.WriteLine(createTab);
        foreach (kontakty.osobaRow i in kontakty.osoba.Rows)
        {
          email = i.email.ToString();
          met = i.metodakomunikacji.ToString();
          kraj = i.kraj.ToString();
          recive = i.wouldliketoreceive.ToString();
          name = i.imie_i_nazwisko.ToString();
          string query = String.Format("INSERT INTO CAS_kontakty VALUES(\'{0}\',\'{1}\',\'{2}\',\'{3}\',\'{4}\','CAS_WWW',getdate());", email, met, kraj, recive, name);
          f.WriteLine(query);
          index++;
        };
        string select = string.Format("Select * from CAS_kontakty");
        f.WriteLine(select);
        f.Close();
      }
    }
    private void buttonOpenCSV_Click(object sender, EventArgs e)
    {
      openFileDialog1.FileName = "kontakty";
      openFileDialog1.DefaultExt = "csv";
      openFileDialog1.Filter = "CSV files (*.csv)|*.csv";
      openFileDialog1.InitialDirectory = "c:\\";
      if (this.openFileDialog1.ShowDialog() != DialogResult.OK)
        return;
      string file = ReadFileCSV(openFileDialog1.FileName.ToString());
      file = PrepareForCSVProcessing(file);
      while (file.Length > 1)
      {
        string adres_email = GetAndMoveNextElement(ref file);
        adres_email = adres_email.Replace("\"", "");
        int indeks = openFileDialog1.FileName.LastIndexOf("\\");
        string kategoria = openFileDialog1.FileName.ToString();
        kategoria = kategoria.Substring(indeks + 1);
        indeks = kategoria.IndexOf(".");
        kategoria = kategoria.Remove(indeks);
        kontaktycsv.osobycsv1.Addosobycsv1Row(adres_email, kategoria);
        //string tytul = GetAndMoveNextElement(ref file);
        //string imie = GetAndMoveNextElement(ref file);
        //string nazwisko = GetAndMoveNextElement(ref file);
        //string jezyk = GetAndMoveNextElement(ref file);
        //string adres_email2 = GetAndMoveNextElement(ref file);
        //string adres_email3 = GetAndMoveNextElement(ref file); 
        //string drugie_imie = GetAndMoveNextElement(ref file);
        //string Sufiks = GetAndMoveNextElement(ref file);
        //string Firma = GetAndMoveNextElement(ref file);
        //string Dzia³ = GetAndMoveNextElement(ref file);
        //string Stanowisko = GetAndMoveNextElement(ref file);
        //string Adres_sluzbowy_ulica = GetAndMoveNextElement(ref file);
        //string Adres_sluzbowy_ulica2 = GetAndMoveNextElement(ref file);
        //string Adres_sluzbowy_ulica3 = GetAndMoveNextElement(ref file);
        //string Adres_sluzowy_miejscowosc = GetAndMoveNextElement(ref file);
        //string Adres_sluzbowy_wojewodztwo = GetAndMoveNextElement(ref file);
        //string Adres_sluzbowy_kod_pocztowy = GetAndMoveNextElement(ref file);
        //string Adres_sluzbowy_kraj = GetAndMoveNextElement(ref file);
        //string Adres_domowy_ulica = GetAndMoveNextElement(ref file);
        //string Adres_domowy_ulica2 = GetAndMoveNextElement(ref file);
        //string Adres_domowy_ulica3 = GetAndMoveNextElement(ref file);
        //string Adres_domowy_miejscowosc = GetAndMoveNextElement(ref file);
        //string Aders_domowy_wojew = GetAndMoveNextElement(ref file);
        //string Adres_domowy_kod_poczt = GetAndMoveNextElement(ref file);
        //string Adres_domowy_kraj = GetAndMoveNextElement(ref file);
        //string Inny_Adres_domowy_ulica = GetAndMoveNextElement(ref file);
        //string Inny_Adres_domowy_ulica2 = GetAndMoveNextElement(ref file);
        //string Inny_Adres_domowy_ulica3 = GetAndMoveNextElement(ref file);
        //string Inny_Adres_domowy_miejscowosc = GetAndMoveNextElement(ref file);
        //string Inny_Aders_domowy_wojew = GetAndMoveNextElement(ref file);
        //string Inny_Adres_domowy_kod_poczt = GetAndMoveNextElement(ref file);
        //string Inny_Adres_domowy_kraj = GetAndMoveNextElement(ref file);
        //string Tel_asystenta = GetAndMoveNextElement(ref file);
        //string Fax_sluzbowy = GetAndMoveNextElement(ref file);
        //string tel_sluzbowy = GetAndMoveNextElement(ref file);
        //string tel_sluzbowy2 = GetAndMoveNextElement(ref file);
        //string Wywo³anie_zwrotne = GetAndMoveNextElement(ref file);
        //string Telefon_w_samochodzie = GetAndMoveNextElement(ref file);
        //string telefon_do_firmy = GetAndMoveNextElement(ref file);
        //string Fax_domowy = GetAndMoveNextElement(ref file);
        //string tel_domowy = GetAndMoveNextElement(ref file);
        //string tel_domowy2 = GetAndMoveNextElement(ref file);
        //string isdn = GetAndMoveNextElement(ref file);
        //string Telefon_komórkowy = GetAndMoveNextElement(ref file);
        //string inny_faks = GetAndMoveNextElement(ref file);
        //string inny_telefon = GetAndMoveNextElement(ref file);
        //string pager = GetAndMoveNextElement(ref file);
        //string tel_podstaw = GetAndMoveNextElement(ref file);
        //string radiotelefon = GetAndMoveNextElement(ref file);
        //string tel_tty_tdd = GetAndMoveNextElement(ref file);
        //string teleks = GetAndMoveNextElement(ref file);

        //string typ_email = GetAndMoveNextElement(ref file);
        //string nazwa_email = GetAndMoveNextElement(ref file);

        //string typ_email2 = GetAndMoveNextElement(ref file);
        //string nazwa_email2 = GetAndMoveNextElement(ref file);

        //string typ_email3 = GetAndMoveNextElement(ref file);
        //string nazwa_email3 = GetAndMoveNextElement(ref file);
        //string charakter_informacyjny = GetAndMoveNextElement(ref file);
        //string dzieci = GetAndMoveNextElement(ref file);
        //string hobby = GetAndMoveNextElement(ref file);
        //string imie_nazw_asyst = GetAndMoveNextElement(ref file);
        //string inf_rozlicz = GetAndMoveNextElement(ref file);
        //string inicjaly = GetAndMoveNextElement(ref file);
        //string internet_info = GetAndMoveNextElement(ref file);

        //string kategorie = GetAndMoveNextElement(ref file);
        //string konto = GetAndMoveNextElement(ref file);
        //string lokalizacja = GetAndMoveNextElement(ref file);
        //string lokalizacja_biura = GetAndMoveNextElement(ref file);
        //string menadzer = GetAndMoveNextElement(ref file);
        //string notatki = GetAndMoveNextElement(ref file);
        //string nr_ewid = GetAndMoveNextElement(ref file);
        //string osoba_polecajaca = GetAndMoveNextElement(ref file);
        //string pesel = GetAndMoveNextElement(ref file);
        //string plec = GetAndMoveNextElement(ref file);
        //string priorytet = GetAndMoveNextElement(ref file);
        //string prywatne = GetAndMoveNextElement(ref file);
        //string przebieg = GetAndMoveNextElement(ref file);
        //string rocznica = GetAndMoveNextElement(ref file);
        //string serwe_adresowy = GetAndMoveNextElement(ref file);
        //string skrytka_pocztowa = GetAndMoveNextElement(ref file);
        //string slowa_kluczowe = GetAndMoveNextElement(ref file);
        //string strona_www = GetAndMoveNextElement(ref file);
        //string urodziny = GetAndMoveNextElement(ref file);
        //string uzytkownik1 = GetAndMoveNextElement(ref file);
        //string uzytkownik2 = GetAndMoveNextElement(ref file);
        //string uzytkownik3 = GetAndMoveNextElement(ref file);
        //string uzytkownik4 = GetAndMoveNextElement(ref file);
        //string wspolmalzonek = GetAndMoveNextElement(ref file);
        //string zawod = GetAndMoveNextElement(ref file);
        //kontaktycsv.osobycsv.AddosobycsvRow(tytul, imie, drugie_imie, nazwisko, Sufiks, Firma, Dzia³, Stanowisko,
        //    Adres_sluzbowy_ulica, Adres_sluzbowy_ulica2, Adres_sluzbowy_ulica3, Adres_sluzowy_miejscowosc, Adres_sluzbowy_wojewodztwo,
        //    Adres_sluzbowy_kod_pocztowy, Adres_sluzbowy_kraj, Adres_domowy_ulica, Adres_domowy_ulica2, Adres_domowy_ulica3,
        //    Adres_domowy_miejscowosc, Aders_domowy_wojew, Adres_domowy_kod_poczt, Adres_sluzbowy_kraj,
        //    Inny_Adres_domowy_ulica, Inny_Adres_domowy_ulica2, Inny_Adres_domowy_ulica3, Inny_Adres_domowy_miejscowosc,
        //    Inny_Aders_domowy_wojew, Inny_Adres_domowy_kod_poczt, Inny_Adres_domowy_kraj, Tel_asystenta, Fax_sluzbowy, tel_sluzbowy, tel_sluzbowy2,
        //    Wywo³anie_zwrotne, Telefon_w_samochodzie, telefon_do_firmy, Fax_domowy, tel_domowy, tel_domowy2, isdn, Telefon_komórkowy, inny_faks,
        //    inny_telefon, pager, tel_podstaw, radiotelefon, tel_tty_tdd, teleks, adres_email, typ_email, nazwa_email, adres_email2, typ_email2, nazwa_email2,
        //    adres_email3, typ_email3, nazwa_email3, charakter_informacyjny, dzieci, hobby, imie_nazw_asyst, inf_rozlicz, inicjaly,
        //    internet_info, jezyk, kategorie, konto, lokalizacja, lokalizacja_biura, menadzer, notatki,
        //    nr_ewid, osoba_polecajaca, pesel, plec, priorytet, prywatne, przebieg, rocznica, serwe_adresowy, skrytka_pocztowa, slowa_kluczowe,
        //    strona_www, urodziny, uzytkownik1, uzytkownik2, uzytkownik3, uzytkownik4, wspolmalzonek, zawod);

      }
    }
    private void buttonGenerujTXT_Click(object sender, EventArgs e)
    {
      saveFileDialog1.FileName = "lista";
      saveFileDialog1.DefaultExt = ".txt";
      saveFileDialog1.Filter = "TXT files (*.txt)|*.txt";
      saveFileDialog1.InitialDirectory = "c:\\";
      if (this.saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        string email;
        using (StreamWriter f = new StreamWriter(saveFileDialog1.FileName.ToString()))
        {
          foreach (kontaktycsv.osobycsv1Row i in kontaktycsv.osobycsv1.Rows)
          {
            email = i._Adres_e_mail.ToString();
            f.WriteLine(email);
          };
        }
      }
    }
    #endregion

    #region menu
    private void outlookAccessDNNToolStripMenuItem_Click(object sender, EventArgs e)
    {
      try
      {
        FormImportOutlookSettings settings = new FormImportOutlookSettings();
        settings.ShowDialog();
        if (settings.ok)
        {
          //wyszukujemy wlasciwa kampanie
          int campaign_id = databaseDNNDataSet.dnn1_OnyakNECampaign.FindId(settings.textBox_kampania.Text);
          if (campaign_id < 0)
            throw new Exception("Kampanii nie znaleziono");
          //robimy wlasciwy import
          foreach (kontaktyDataSet.KontaktyRow row in kontaktyDataSet.Kontakty)
            if (row.Match(settings.textBox_kraj.Text, settings.textBox_kategoria.Text, settings.checkBox_innyniz.Checked))
              databaseDNNDataSet.dnn1_OnyakNEOptIns.UpdateOrAddEntry(campaign_id, row.Adresemail, row.GetImie(), row.GetNazwisko());
          dnn1_OnyakNEOptInsTableAdapter.Update(databaseDNNDataSet.dnn1_OnyakNEOptIns);
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.ToString());
      }
    }
    private void xMLDNNToolStripMenuItem_Click(object sender, EventArgs e)
    {
      try
      {
        FormImportStatystykiSettings _settings = new FormImportStatystykiSettings();
        _settings.ShowDialog();
        if (!_settings.ok)
          return;
        //wyszukujemy wlasciwa kampanie
        int campaign_id = databaseDNNDataSet.dnn1_OnyakNECampaign.FindId(_settings.textBox_kampania.Text);
        if (campaign_id < 0)
        {
          MessageBox.Show($"The campain {_settings.textBox_kampania.Text} doesn't exist.");
          return;
        }
        //robimy wlasciwy import
        foreach (kontakty.osobaRow row in kontakty.osoba)
          if (row.Match(_settings.textBox_kraj.Text))
            databaseDNNDataSet.dnn1_OnyakNEOptIns.UpdateOrAddEntry(campaign_id, row.email, row.GetImie(), row.GetNazwisko());
        dnn1_OnyakNEOptInsTableAdapter.Update(databaseDNNDataSet.dnn1_OnyakNEOptIns);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.ToString());
      }
    }
    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.Close();
    }
    private void tXTListDNNToolStripMenuItem_Click(object sender, EventArgs e)
    {
      try
      {
        FormImportTXTListSettings _settings = new FormImportTXTListSettings();
        _settings.ShowDialog();
        if (!_settings.ok)
          return;
        //wyszukujemy wlasciwa kampanie
        int campaign_id = databaseDNNDataSet.dnn1_OnyakNECampaign.FindId(_settings.textBox_kampania.Text);
        if (campaign_id < 0)
        {
          MessageBox.Show($"The campain {_settings.textBox_kampania.Text} doesn't exist.");
          return;
        }
        //teraz otworzymy plik i dodamy wszystkie tagi:
        StreamReader plik = new StreamReader(_settings.textBox_file.Text, System.Text.Encoding.Default);
        string plikzawartosc = plik.ReadToEnd();
        plik.Close();
        string Tagname = "";
        while (plikzawartosc.Length > 0)
        {
          int pos = plikzawartosc.IndexOf("\r\n");
          if (pos >= 0)
          {
            Tagname = plikzawartosc.Substring(0, pos);
            plikzawartosc = plikzawartosc.Remove(0, pos + 2);
          }
          else
          {
            if (plikzawartosc.Length > 0)
            {
              Tagname = plikzawartosc;
              plikzawartosc = "";
            }
          }
          Tagname = Tagname.Trim();
          if (Tagname.Length > 0) //only if the line is not empty
          {
            //dodajemy taga:
            databaseDNNDataSet.dnn1_OnyakNEOptIns.UpdateOrAddEntry(campaign_id, Tagname, "", "");
          }
        }
        dnn1_OnyakNEOptInsTableAdapter.Update(databaseDNNDataSet.dnn1_OnyakNEOptIns);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.ToString());
      }
    }
    private void changeOutlookAttachToolStripMenuItem_Click(object sender, EventArgs e)
    {
      string file = String.Empty;
      OpenFileDialog _dialog = new OpenFileDialog();
      _dialog.Filter = "DataBase files|*.mdb";
      if (_dialog.ShowDialog() != DialogResult.OK)
        return;
      file = _dialog.FileName;
      this.kontaktyTableAdapter.Connection.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + file;
      this.kontaktyTableAdapter.Fill(this.kontaktyDataSet.Kontakty);
    }
    private void button_save_Click(object sender, EventArgs e)
    {
      string _caption = "Onyak data table";
      if (MessageBox.Show("Are you sure to update Onyak data table ?", _caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
        return;
      int _result = dnn1_OnyakNEOptInsTableAdapter.Update(databaseDNNDataSet.dnn1_OnyakNEOptIns);
      MessageBox.Show($"done result: {_result}", _caption);
    }
    private void dataGridView5_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    /// <summary>
    /// Handles the Click event of the dNNRegisteredUsersDNNToolStripMenuItem control. 
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void dNNRegisteredUsersDNNToolStripMenuItem_Click(object sender, EventArgs e)
    {
      try
      {
        FormImportDNNRegisteredUsersSettings _settings = new FormImportDNNRegisteredUsersSettings();
        _settings.ShowDialog();
        if (!_settings.ok)
          return;
        int campaign_id = databaseDNNDataSet.dnn1_OnyakNECampaign.FindId(_settings.textBox_kampania.Text);
        if (campaign_id < 0)
        {
          MessageBox.Show($"The campain {_settings.textBox_kampania.Text} doesn't exist.");
          return;
        }
        foreach (DatabaseDNNDataSet.DNNRegisteredUsersRow row in databaseDNNDataSet.DNNRegisteredUsers)
          if (row.IsUserFrom(_settings.textBox_kraj.Text, _settings.checkBox_innyniz.Checked))
            databaseDNNDataSet.dnn1_OnyakNEOptIns.UpdateOrAddEntry(campaign_id, row.Email, row.FirstName, row.LastName);
        dnn1_OnyakNEOptInsTableAdapter.Update(databaseDNNDataSet.dnn1_OnyakNEOptIns);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.ToString());
      }
    }
    /// <summary>
    /// Unsubscribe users using text list of emails contained in the selected file.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
    private void unsubscribeTXTListToolStripMenuItem_Click(object sender, EventArgs e)
    {
      try
      {
        OpenFileDialog _fileOpenDialog = new OpenFileDialog();
        if (_fileOpenDialog.ShowDialog() != DialogResult.OK)
          return;
        string _content;
        using (StreamReader _file = new StreamReader(_fileOpenDialog.FileName, Encoding.Default))
          _content = _file.ReadToEnd();
        string _tagName = "";
        int _count = 0;
        while (_content.Length > 0)
        {
          int pos = _content.IndexOf("\r\n");
          if (pos >= 0)
          {
            _tagName = _content.Substring(0, pos);
            _content = _content.Remove(0, pos + 2);
          }
          else
          {
            if (_content.Length > 0)
            {
              _tagName = _content;
              _content = "";
            }
          }
          _tagName = _tagName.Trim();
          if (_tagName.Length > 0) //only if the line is not empty
          {
            if (databaseDNNDataSet.dnn1_OnyakNEOptIns.UnsubscribeUserByEmailFromAllCampaigns(_tagName))
              _count++;
          }
        }
        dnn1_OnyakNEOptInsTableAdapter.Update(databaseDNNDataSet.dnn1_OnyakNEOptIns);
        MessageBox.Show($"Number of unsubscribed items: {_count}", "Campaigns");
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.ToString());
      }
    }
    #endregion    

    #endregion

  }
}