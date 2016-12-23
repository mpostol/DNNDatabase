//_______________________________________________________________
//  Title   : DNNDataClassesDataContext
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAS.DNNDataBase.DataBaseManagement
{
  public partial class DNNDataClassesDataContext
  {
    /// <summary>
    /// Struct UserEmail - captures details for user selection
    /// </summary>
    public struct UserEmail
    {
      public string UserName;
      public bool Registered;
      public string Email;
      public string Company;
    }
    public async static Task<IEnumerable<UserEmail>> GetEmailAsync(DateTime fromDate)
    {
      Func<IEnumerable<UserEmail>> _asyncTask = () =>
      {
        IEnumerable<UserEmail> _myList = null;
        using (DNNDataClassesDataContext _newContext = new DNNDataClassesDataContext())
        {
          IEnumerable<UserEmail>_myQuery =
            (from user in _newContext.dnn1_Users
             join portal in _newContext.dnn1_UserPortals on user.UserID equals portal.UserId
             join profile in _newContext.dnn1_UserProfiles on user.UserID equals profile.UserID
             where (user.CreatedOnDate >= fromDate && portal.PortalId == 1 && profile.PropertyValue.Length > 0)
             select new UserEmail()
             {
               UserName = $"{user.FirstName} {user.LastName}",
               Email = user.Email,
               Registered = profile.PropertyDefinitionID == 44,
               Company = profile.PropertyValue
             });
          _myList = _myQuery.Distinct<UserEmail>(new Comparer()).ToList<UserEmail>();
        };
        return _myList;
      };
      return await Task.Run<IEnumerable<UserEmail>>(_asyncTask);
    }
    /// <summary>
    /// Class Comparer.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEqualityComparer{CAS.DNNDataBase.DataBaseManagement.DNNDataClassesDataContext.UserEmail}" />
    private class Comparer : IEqualityComparer<UserEmail>
    {
      /// <summary>
      /// Determines whether the specified objects are equal.
      /// </summary>
      /// <param name="x">The first object of type <paramref name="T" /> to compare.</param>
      /// <param name="y">The second object of type <paramref name="T" /> to compare.</param>
      /// <returns>true if the specified objects are equal; otherwise, false.</returns>
      public bool Equals(UserEmail x, UserEmail y)
      {
        return x.Email.Equals(y.Email);
      }
      /// <summary>
      /// Returns a hash code for this instance.
      /// </summary>
      /// <param name="obj">The <see cref="T:System.Object" /> for which a hash code is to be returned.</param>
      /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
      /// <exception cref="System.NotImplementedException"></exception>
      public int GetHashCode(UserEmail obj)
      {
        return obj.GetHashCode();
      }
    }
  }
}

