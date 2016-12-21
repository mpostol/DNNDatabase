//<summary>
//  Title   : Name of Application
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    <Author> - <date>:
//    <description>
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.com.pl
//  http://www.cas.eu
//</summary>

using System.Data;
using System.Text.RegularExpressions;

namespace CAS.DNNDataBase.UsersManagement
{


  partial class kontakty
  {
    partial class osobaRow
    {
      internal bool Match(string jezyk)
      {
        bool kraj_match = false;
        if (this.IsemailNull() || email == "")
          return false;
        if (this.IslangNull() && jezyk != "")
          return false;
        if (this.wouldliketoreceive.ToLower() == "no" || this.wouldliketoreceive.ToLower() == "nie")
          if ((this.IslangNull() && jezyk == "") || this.lang.ToLower().Contains(jezyk.ToLower()))
            kraj_match = true;
        return kraj_match;
      }
      internal string GetImie()
      {
        string ret = "";
        try
        {
          if (!this.Isimie_i_nazwiskoNull())
            ret = this.imie_i_nazwisko.Split(' ')[0];
        }
        catch
        { }
        return ret;
      }
      internal string GetNazwisko()
      {
        string ret = "";
        try
        {
          if (!this.Isimie_i_nazwiskoNull())
            ret = this.imie_i_nazwisko.Split(' ')[1];
        }
        catch
        { }
        return ret;
      }
    }
    internal void clean()
    {
      //int wiersz = 0;
      string pattern = @"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]+\.([a-z]" + @"[a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$";
      foreach (kontakty.osobaRow i in osoba.Rows)
      {
        //wiersz++;
        foreach (DataColumn j in osoba.Columns)
        {

          i[j] = i[j].ToString().TrimEnd();
          i[j] = i[j].ToString().TrimStart();
          i[j] = i[j].ToString().ToLower();
          i[j] = i[j].ToString().Replace("\'", " ");
          if (!i[1].ToString().Contains("@"))
            i[1] = "";
          if (i[1].ToString().Contains("@"))
          {
            Match match = Regex.Match(i[1].ToString().Trim(), pattern, RegexOptions.IgnoreCase);
            if (!match.Success)
              i[1] = "";
          }
        }
      }
    }

  }
}
