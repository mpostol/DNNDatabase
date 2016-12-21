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
namespace CAS.DNNDataBase.UsersManagement
{


  partial class kontaktyDataSet
  {
    public partial class KontaktyRow
    {
      internal bool Match(string kraj, string kategoria, bool krajinnynizwybrany)
      {
        bool kraj_match = false;
        bool kategoria_match = false;
        if (this.IsAdresemailNull()) return false;
        if (this.IsAdressubowykrajNull() && kraj != "") return false;
        if ((this.IsAdressubowykrajNull() && kraj == "") ||
          (this.Adressubowykraj.ToLower().Contains(kraj.ToLower()) && !krajinnynizwybrany) ||
          (!this.Adressubowykraj.ToLower().Contains(kraj.ToLower()) && krajinnynizwybrany)
          )
          kraj_match = true;
        if (this.IsKategorieNull() && kategoria != "") return false;
        if ((this.IsKategorieNull() && kategoria == "") || this.Kategorie.ToLower().Contains(kategoria.ToLower()))
          kategoria_match = true;
        return kategoria_match && kraj_match;
      }
      internal string GetImie()
      {
        string ret = "";
        if (!this.IsTytuNull())
          ret += this.Tytu + " ";
        if (!this.IsImięNull())
          ret += this.Imię + " ";
        if (!this.IsDrugieimięNull())
          ret += this.Drugieimię + " ";
        return ret;
      }
      internal string GetNazwisko()
      {
        string ret = "";
        if (!this.IsNazwiskoNull())
          ret += this.Nazwisko;
        return ret;
      }
    }
  }
}
