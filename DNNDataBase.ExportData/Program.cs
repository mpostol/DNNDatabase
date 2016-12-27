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
using CAS.DNNDataBase.ExportData.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CAS.DNNDataBase.ExportData
{
  public class Program
  {

    public static void Main(string[] args)
    {
      Task<IEnumerable<DNNDataClassesDataContext.UserEmail>> _listOfEmailsTask = null;
      string[] _commandLine = Environment.GetCommandLineArgs();
      try
      {
        CommandLineCommands _commandToDo = CommandLineCommands.GetCommands(_commandLine);
        DateTime _startDate = DateTime.ParseExact(_commandToDo.CommandsList[CommandLineCommands.CommandsSet.CreateRecentEmailsList].Item2, Settings.Default.DateFormat, CultureInfo.InvariantCulture);
        _listOfEmailsTask = GetEmailAsync(_startDate);
        _listOfEmailsTask.Wait();
        IEnumerable<string> _listOfEmails = _listOfEmailsTask.Result.Select<DNNDataClassesDataContext.UserEmail, string>(x => x.Email).FilterEmails(new Progress<int>());
        string _finalList = String.Join<string>(";", _listOfEmails.ToArray<string>());
        Console.WriteLine();
        Console.WriteLine("Final list of emails:");
        Console.WriteLine(_finalList);
        Console.WriteLine();
        Settings.Default.LastDate = DateTime.Today.ToString(Settings.Default.DateFormat);
        string _fileName = $"{Settings.Default.LastDate}_adresy_strona_commserver.txt";
        File.WriteAllText(_fileName, _finalList);
        Console.WriteLine($"Final list of emails registered since {_startDate.ToString(Settings.Default.DateFormat)} saved to file {_fileName}");
        Settings.Default.Save();
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
    private class CommandLineCommands
    {
      internal Dictionary<CommandsSet, Tuple<CommandsSet, string>> CommandsList { get; private set; } = null;
      private Tuple<CommandsSet, string>[] _defaultCommands = new Tuple<CommandsSet, string>[]
        {
          new Tuple<CommandsSet, string>(CommandsSet.CreateRecentEmailsList, Settings.Default.LastDate),
          new Tuple<CommandsSet, string>(CommandsSet.AnalyzeLinks, String.Empty),
        };
      internal enum CommandsSet { CreateRecentEmailsList, AnalyzeLinks };
      internal static CommandLineCommands GetCommands(string[] commandLine)
      {
        CommandLineCommands _ret = new CommandLineCommands();
        foreach (string _arg in commandLine)
          if (_arg.ToLower().StartsWith("/d"))
            _ret.CommandsList[CommandsSet.CreateRecentEmailsList] = new Tuple<CommandsSet, string>(CommandsSet.CreateRecentEmailsList, _arg.Substring(2));
          else if (_arg.ToLower().StartsWith("/l"))
            _ret.CommandsList[CommandsSet.AnalyzeLinks] = new Tuple<CommandsSet, string>(CommandsSet.AnalyzeLinks, _arg.Substring(2));
        return _ret;
      }
      private CommandLineCommands()
      {
        CommandsList = _defaultCommands.ToDictionary<Tuple<CommandsSet, string>, CommandsSet>(x => x.Item1);
      }
    }
  }
}
