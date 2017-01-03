
using CAS.DNNDataBase.DataBaseManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CAS.DNNDataBase.DataBaseManagementUnitTest
{
  [TestClass]
  public class DNNDataClassesDataContextUnitTest
  {
    [TestMethod]
    public void GetEmailAsyncTestMethod()
    {
      IEnumerable<DNNDataClassesDataContext.UserEmail> _listOfUsers = DNNDataClassesDataContext.GetEmailAsync(DateTime.Now - TimeSpan.FromDays(40)).Result;
      Assert.IsTrue(_listOfUsers.Count() > 0);
    }
  }
}
