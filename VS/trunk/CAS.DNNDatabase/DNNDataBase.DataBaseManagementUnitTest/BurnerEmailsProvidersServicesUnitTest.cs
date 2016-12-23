
using CAS.DNNDataBase.DataBaseManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Mail;

namespace CAS.DNNDataBase.DataBaseManagementUnitTest
{
  [TestClass]
  public class BurnerEmailsProvidersServicesUnitTest
  {
    [TestMethod]
    public void GetProvidersTestMethod1()
    {
      Assert.IsTrue(BurnerEmailsProvidersServices.IsItRegisteredProvider("0815.ru"));
      Assert.IsTrue(BurnerEmailsProvidersServices.IsItRegisteredProvider("yentzscholarship.xyz"));
    }
    [TestMethod]
    public void GetProvidersMailAddressTestMethod1()
    {
      Assert.IsTrue(BurnerEmailsProvidersServices.IsItRegisteredProvider(new MailAddress ("abscde@0815.ru")));
      Assert.IsTrue(BurnerEmailsProvidersServices.IsItRegisteredProvider(new MailAddress("1234@yentzscholarship.xyz")));
    }
    [TestMethod]
    [ExpectedException(typeof(FormatException))]
    public void GetProvidersWrongMailAddressTestMethod1()
    {
      Assert.IsTrue(BurnerEmailsProvidersServices.IsItRegisteredProvider(new MailAddress("---0815.ru")));
    }
  }
}
