using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace CAS.DNNDataBase.DataBaseManagement
{
  public static class BurnerEmailsProvidersServices
  {
    /// <summary>
    /// Determines whether the the specified domain is registered.
    /// </summary>
    /// <param name="domain">The domain to check if it is registered.</param>
    /// <returns><c>true</c> if the domain is registered; otherwise, <c>false</c>.</returns>
    public static bool IsItRegisteredProvider(string domain)
    {
      return m_RegisteredProviders.Value.Contains(domain);
    }
    public static bool IsItRegisteredProvider(this MailAddress address)
    {
      return m_RegisteredProviders.Value.Contains(address.Host);
    }
    public static IEnumerable<string> FilterEmails(this IEnumerable<string> emails, IProgress<int> progress)
    {
      List<string> _ret = new List<string>();
      foreach (string _address in emails)
        try
        {
          if (!IsItRegisteredProvider(new MailAddress(_address)))
            _ret.Add(_address);
        }
        catch { }
      progress.Report(emails.Count<string>() - _ret.Count());
      return _ret;
    }
    private static Lazy<List<string>> m_RegisteredProviders = new Lazy<List<String>>(() => GetProviders());
    private static List<string> GetProviders()
    {
      String[] _list = Properties.Resources.BurnerEmailsProviders.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
      List<string> _listProviders = new List<string>(_list);
      _listProviders.Sort();
      return _listProviders;
    }

  }
}
