using Ducode.Local.Business;
using Ducode.Local.Implementations;
using Ducode.Local.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ducode.Local.Tests
{
	[TestClass]
	public class IntegrationTests
	{
		private StringBuilder _console;
		private static string _version = "0.2.3.4";
		private static string _urlFormat = "http://localhost:{0}";
		private static string _defaultFile = "index.html";
		private static string _username = "username";
		private static string _password = "password";
		private static string _baseUrl;
		private static string _rootFolder;

		private Mock<IConsoleWriter> _consoleWriterMock;
		private Mock<IApplicationManager> _applicationManagerMock;
		private IDisposable _webServer;

		[TestInitialize]
		public void Init()
		{
			_console = new StringBuilder();
			_baseUrl = string.Format(_urlFormat, FreeTcpPort());
			_rootFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().FullName);

			//IConsoleWriter
			_consoleWriterMock = new Mock<IConsoleWriter>();
			_consoleWriterMock.Setup(m => m.WriteLine(It.IsAny<string>())).Callback<string>(s => _console.AppendLine(s));
			_consoleWriterMock.Setup(m => m.ReadLine()).Returns(string.Empty);

			//IApplicationManager
			_applicationManagerMock = new Mock<IApplicationManager>();
			_applicationManagerMock.Setup(m => m.Version).Returns(_version);
		}

		[TestCleanup]
		public void Cleanup()
		{
			_webServer.Dispose();
		}

		#region XPoweredByMiddleware
		[TestMethod]
		public async Task Integration_XPoweredByMiddleware_HappyFlow()
		{
			//arrange
			var server = CreateServer();
			_webServer = CreateServer().CreateServer(_baseUrl, true, _rootFolder, _defaultFile, false, string.Empty, string.Empty);

			//act / assert
			using (var client = new HttpClient())
			using (var response = await client.GetAsync(_baseUrl))
			{
				Assert.IsNotNull(response);
				Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
				IEnumerable<string> values = null;
				response.Headers.TryGetValues("X-Powered-By", out values);
				Assert.IsNotNull(values);
				Assert.IsTrue(values.First().Contains(string.Format("DotLocal {0}", _version)));
			}
		}
		#endregion

		#region LoggerMiddleware
		[TestMethod]
		public async Task Integration_LoggerMiddleware_HappyFlow()
		{
			//arrange
			var server = CreateServer();
			_webServer = CreateServer().CreateServer(_baseUrl, true, _rootFolder, _defaultFile, true, string.Empty, string.Empty);

			//act / assert
			using (var client = new HttpClient())
			using (var response = await client.GetAsync(_baseUrl))
			{
				Assert.IsNotNull(response);
				Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
				Assert.IsTrue(_console.Contains(_baseUrl));
			}
		}
		#endregion

		#region BasicAuthMiddleware
		[TestMethod]
		public async Task Integration_BasicAuthMiddleware_NoAuthorizationHeaderSent()
		{
			//arrange
			var server = CreateServer();
			_webServer = CreateServer().CreateServer(_baseUrl, true, _rootFolder, _defaultFile, true, _username, _password);

			using (var client = new HttpClient())
			using (var response = await client.GetAsync(_baseUrl))
			{
				Assert.IsNotNull(response);
				Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
			}
		}

		[TestMethod]
		public async Task Integration_BasicAuthMiddleware_WrongUsernameAndPassword()
		{
			//arrange
			var server = CreateServer();
			_webServer = CreateServer().CreateServer(_baseUrl, true, _rootFolder, _defaultFile, true, _username, _password);

			using (var client = new HttpClient())
			{
				var request = new HttpRequestMessage();
				request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("wrongusername:wrongpassword")));
				request.RequestUri = new Uri(_baseUrl);
				using (var response = await client.SendAsync(request))
				{
					Assert.IsNotNull(response);
					Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
				}
			}
		}

		[TestMethod]
		public async Task Integration_BasicAuthMiddleware_HappyFlow()
		{
			//arrange
			var server = CreateServer();
			_webServer = CreateServer().CreateServer(_baseUrl, true, _rootFolder, _defaultFile, true, _username, _password);

			using (var client = new HttpClient())
			{
				var request = new HttpRequestMessage();
				request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", _username, _password))));
				request.RequestUri = new Uri(_baseUrl);
				using (var response = await client.SendAsync(request))
				{
					Assert.IsNotNull(response);
					Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
				}
			}
		}
		#endregion

		#region Private methods
		private IWebServer CreateServer()
		{
			return new WebServer(_consoleWriterMock.Object, _applicationManagerMock.Object);
		}

		private static int FreeTcpPort()
		{
			var listener = new TcpListener(IPAddress.Loopback, 0);
			listener.Start();
			var port = ((IPEndPoint)listener.LocalEndpoint).Port;
			listener.Stop();
			return port;
		}
		#endregion
	}
}
