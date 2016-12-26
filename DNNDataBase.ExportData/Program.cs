//_______________________________________________________________
//  Title   : Entry point for the DNNDataBase.ExportData
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

using CAS.DNNDataBase.DataBaseManagement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace CAS.DNNDataBase.ExportData
{
  public class Program
  {

    public static void Main(string[] args)
    {
      Task<IEnumerable<DNNDataClassesDataContext.UserEmail>> _listOfEmailsTask = null;
      try
      {
        DateTime _startDate = DateTime.ParseExact("161116", m_DateFormat, System.Globalization.CultureInfo.InvariantCulture);
        _listOfEmailsTask = GetEmailAsync(_startDate);
        _listOfEmailsTask.Wait();
        IEnumerable<string> _listOfEmails = _listOfEmailsTask.Result.Select<DNNDataClassesDataContext.UserEmail, string>(x => x.Email).FilterEmails( new Progress<int>());
        string _finalList = String.Join<string>(";", _listOfEmails.ToArray<string>());
        Console.WriteLine();
        Console.WriteLine("Final list of emails:");
        Console.WriteLine(_finalList);
        Console.WriteLine();
        string _fileName = $"{XmlConvert.ToString(DateTime.Today, m_DateFormat)}_adresy_strona_commserver.txt";
        File.WriteAllText(_fileName, _finalList);
        Console.WriteLine($"Final list of emails registered since {_startDate.ToString(m_DateFormat)} saved to file {_fileName}");
      }
      catch (AggregateException _ex)
      {
        foreach (Exception _item in _ex.InnerExceptions)
          Console.WriteLine(_listOfEmailsTask.Exception.ToString());
      }
      catch (Exception _ge)
      {
        Console.WriteLine(_ge.ToString());
      }
      Console.Write("Press enter to close the window:");
      Console.ReadLine();
    }
    private readonly static string m_DateFormat = "yyMMdd";
    private static async Task<IEnumerable<DNNDataClassesDataContext.UserEmail>> GetEmailAsync(DateTime startDate)
    {
      Task<IEnumerable<DNNDataClassesDataContext.UserEmail>> _emails = DNNDataClassesDataContext.GetEmailAsync(startDate);
      while (!_emails.IsCompleted)
      {
        Thread.Sleep(1000);
        Console.Write(".");
      }
      return await _emails;
    }
    private class Progress<T> : IProgress<T>
    {
      public void Report(T value)
      {
        Console.WriteLine($"Filtered out {value} emails");
      }
    }
  }
}
