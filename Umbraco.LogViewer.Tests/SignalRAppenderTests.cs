using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using log4net.Core;
using Microsoft.AspNet.SignalR;
using NUnit.Framework;

namespace Umbraco.LogViewer.Tests
{
    [TestFixture]
    public class SignalRAppenderTests : IConnection
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private Action<ConnectionMessage> messageHandler;

        [TestFixtureSetUp]
        public static void FixtureSetup()
        {
            XmlConfigurator.Configure(new FileInfo(@"..\..\app.config"));
        }

        [SetUp]
        public void Setup()
        {
            SignalRAppenderConnection.GetConnection = () => this;
        }

        [TearDown]
        public void Teardown()
        {
            SignalRAppenderConnection.Reset();
        }

        [Test]
        public void Append_BroadcastsMessageDto()
        {
            ConnectionMessage message;
            messageHandler = (m) => message = m;

            var expectedDto = new LogDto
            {
                Level = Level.Info.DisplayName,
                TimeStamp = DateTime.Now.ToString("G"),
                Logger = "Umbraco.LogViewer.Tests.SignalRAppenderTests",
                Message = "Hey there!",
                Thread = "Runner thread"
            };

            Log.Info("Hey there!");

            Assert.AreEqual(expectedDto, message.Value);
        }

        public Task Send(ConnectionMessage message)
        {
            messageHandler(message);
            return new Task(() => { });
        }

        public string DefaultSignal { get; private set; }
    }
}
