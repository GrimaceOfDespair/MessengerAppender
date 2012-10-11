using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using log4net;

namespace MessengerAppender.Tests
{
  public class CommunicationTests
  {
    public static int WaitFor = 30000;
    public static int Polling = 2500;

    [SetUp]
    public void SetUp()
    {
    }

    [TearDown]
    public void TearDown()
    {
    }

    [Test]
    public void IssueMessage()
    {
      log4net.Config.XmlConfigurator.Configure(new FileInfo("log4net.config"));
      ILog log = LogManager.GetLogger("MessengerAppender.Tests");

      var liveAppender = (LiveAppender)LogManager
        .GetAllRepositories()
        .SelectMany(appender => appender.GetAppenders())
        .First(appender => appender is LiveAppender);

      Assert.That(() => liveAppender.Messenger.Connected, Is.True.After(WaitFor, Polling), "LiveAppender should have connected");
      Assert.That(() => liveAppender.Contacts.Any(), Is.True.After(WaitFor, Polling), "LiveAppender should have loaded contacts");
    }
  }
}
