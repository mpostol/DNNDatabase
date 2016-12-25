//_______________________________________________________________
//  Title   : Entry point for the DNNDataBase.ExportData
//  System  : Microsoft VisualStudio 2015 / C#
//  $LastChangedDate:  $
//  $Rev: $
//  $LastChangedBy: $
//  $URL: $
//  $Id:  $
//
//  Copyright (C) 2016, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

using CAS.DNNDataBase.DataBaseManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CAS.DNNDataBase.ExportData
{
  public class Program
  {

    public static void Main(string[] args)
    {
      Task<IEnumerable<DNNDataClassesDataContext.UserEmail>> _listOfEmailsTask = GetEmailAsync();
      try
      {
        _listOfEmailsTask.Wait();
        IEnumerable<DNNDataClassesDataContext.UserEmail> _listOfEmails = _listOfEmailsTask.Result;
        string _finalList = String.Join<string>(";", _listOfEmails.Select<DNNDataClassesDataContext.UserEmail, string>(_email => _email.Email).ToArray<string>());
        Console.WriteLine();
        Console.WriteLine("Final list of emails:");
        Console.WriteLine(_finalList);
        Console.WriteLine();
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
    private static async Task<IEnumerable<DNNDataClassesDataContext.UserEmail>> GetEmailAsync()
    {
      Task<IEnumerable<DNNDataClassesDataContext.UserEmail>> _emails = DNNDataClassesDataContext.GetEmailAsync(DateTime.Now - TimeSpan.FromDays(10));
      while (!_emails.IsCompleted)
      {
        Thread.Sleep(1000);
        Console.Write(".");
      }
      return await _emails;
    }
  }
}
