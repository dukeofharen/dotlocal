using Ducode.Local.Business.Implementations;
using Ducode.Local.Resources;
using Ducode.Local.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Text;

namespace Ducode.Local.Business.Tests.Implementations
{
	[TestClass]
	public class WebManagerFacts
	{
		private StringBuilder _console;

		private Mock<INetworkService> _networkServiceMock;
		private Mock<IWebServer> _webServerMock;
		private Mock<IConsoleWriter> _consoleWriterMock;
		private Mock<IFileService> _fileServiceMock;

		private int _port = 9123;
		private string _rootPath = @"C:\tmp";
		private string _username = "username";
		private string _password = "password";
		private string _defaultFile = "index.html";

		[TestInitialize]
		public void Init()
		{
			_console = new StringBuilder();

			//INetworkService
			_networkServiceMock = new Mock<INetworkService>();

			//IWebServer
			_webServerMock = new Mock<IWebServer>();
			_webServerMock.Setup(m => m.CreateServer(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>()));

			//IConsoleWriter
			_consoleWriterMock = new Mock<IConsoleWriter>();
			_consoleWriterMock.Setup(m => m.WriteLine(It.IsAny<string>())).Callback<string>(s => _console.AppendLine(s));
			_consoleWriterMock.Setup(m => m.ReadLine()).Returns(string.Empty);

			//IFileService
			_fileServiceMock = new Mock<IFileService>();
		}

		#region Serve
		[TestMethod]
		public void WebManager_Serve_PortUnavailable()
		{
			//arrange
			var service = CreateManager();
			_networkServiceMock.Setup(m => m.PortAvailable(_port)).Returns(false);

			//act
			service.Serve(_port, string.Empty, string.Empty, false, false, string.Empty, string.Empty);

			//assert
			Assert.IsTrue(_console.Contains(string.Format(Strings.port_in_use, _port)));
		}

		[TestMethod]
		public void WebManager_Serve_DirectoryDoesntExist()
		{
			//arrange
			var service = CreateManager();
			_fileServiceMock.Setup(m => m.DirectoryExists(_rootPath)).Returns(false);

			//act
			service.Serve(_port, _rootPath, string.Empty, false, false, string.Empty, string.Empty);

			//assert
			Assert.IsTrue(_console.Contains(string.Format(Strings.web_server_root_invalid, _rootPath)));
		}

		[TestMethod]
		public void WebManager_Serve_UsernameSetPasswordNotSet()
		{
			//arrange
			var service = CreateManager();

			//act
			service.Serve(_port, string.Empty, string.Empty, false, false, _username, string.Empty);

			//assert
			Assert.IsTrue(_console.Contains(Strings.fill_in_username_and_password));
		}

		[TestMethod]
		public void WebManager_Serve_UsernameNotSetPasswordSet()
		{
			//arrange
			var service = CreateManager();

			//act
			service.Serve(_port, string.Empty, string.Empty, false, false, string.Empty, _password);

			//assert
			Assert.IsTrue(_console.Contains(Strings.fill_in_username_and_password));
		}

		[TestMethod]
		public void WebManager_Serve_HappyFlow_DefaultFile()
		{
			//arrange
			var service = CreateManager();
			_networkServiceMock.Setup(m => m.PortAvailable(_port)).Returns(true);
			_fileServiceMock.Setup(m => m.DirectoryExists(_rootPath)).Returns(true);

			//act
			service.Serve(_port, _rootPath, _defaultFile, false, false, string.Empty, string.Empty);

			//assert
			Assert.IsTrue(_console.Contains(string.Format(Strings.default_file, _defaultFile)));
		}

		[TestMethod]
		public void WebManager_Serve_HappyFlow_EnableDirectoryListing()
		{
			//arrange
			var service = CreateManager();
			_networkServiceMock.Setup(m => m.PortAvailable(_port)).Returns(true);
			_fileServiceMock.Setup(m => m.DirectoryExists(_rootPath)).Returns(true);

			//act
			service.Serve(_port, _rootPath, string.Empty, true, false, string.Empty, string.Empty);

			//assert
			Assert.IsTrue(_console.Contains(Strings.directory_listing_enabled));
		}

		[TestMethod]
		public void WebManager_Serve_HappyFlow_DisableDirectoryListing()
		{
			//arrange
			var service = CreateManager();
			_networkServiceMock.Setup(m => m.PortAvailable(_port)).Returns(true);
			_fileServiceMock.Setup(m => m.DirectoryExists(_rootPath)).Returns(true);

			//act
			service.Serve(_port, _rootPath, string.Empty, false, false, string.Empty, string.Empty);

			//assert
			Assert.IsTrue(_console.Contains(Strings.directory_listing_disabled));
		}

		[TestMethod]
		public void WebManager_Serve_HappyFlow_BasicAuthentication()
		{
			//arrange
			var service = CreateManager();
			_networkServiceMock.Setup(m => m.PortAvailable(_port)).Returns(true);
			_fileServiceMock.Setup(m => m.DirectoryExists(_rootPath)).Returns(true);

			//act
			service.Serve(_port, _rootPath, string.Empty, false, false, _username, _password);

			//assert
			Assert.IsTrue(_console.Contains(Strings.basic_auth_enabled));
		}

		[TestMethod]
		public void WebManager_Serve_HappyFlow_EnableLogging()
		{
			//arrange
			var service = CreateManager();
			_networkServiceMock.Setup(m => m.PortAvailable(_port)).Returns(true);
			_fileServiceMock.Setup(m => m.DirectoryExists(_rootPath)).Returns(true);

			//act
			service.Serve(_port, _rootPath, string.Empty, false, true, string.Empty, string.Empty);

			//assert
			Assert.IsTrue(_console.Contains(Strings.logging_enabled));
		}

		[TestMethod]
		public void WebManager_Serve_HappyFlow_DisableLogging()
		{
			//arrange
			var service = CreateManager();
			_networkServiceMock.Setup(m => m.PortAvailable(_port)).Returns(true);
			_fileServiceMock.Setup(m => m.DirectoryExists(_rootPath)).Returns(true);

			//act
			service.Serve(_port, _rootPath, string.Empty, false, false, string.Empty, string.Empty);

			//assert
			Assert.IsTrue(_console.Contains(Strings.logging_disabled));
		}

		[TestMethod]
		public void WebManager_Serve_HappyFlow_HappyFlow()
		{
			//arrange
			var service = CreateManager();
			_networkServiceMock.Setup(m => m.PortAvailable(_port)).Returns(true);
			_fileServiceMock.Setup(m => m.DirectoryExists(_rootPath)).Returns(true);

			//act
			service.Serve(_port, _rootPath, _defaultFile, true, true, _username, _password);

			//assert
			Assert.IsTrue(_console.Contains(string.Format(Strings.web_server_root, _rootPath)));
			Assert.IsTrue(_console.Contains(Strings.press_any_key));
		}
		#endregion

		#region Private methods
		private IWebManager CreateManager()
		{
			return new WebManager(_networkServiceMock.Object, _webServerMock.Object, _consoleWriterMock.Object, _fileServiceMock.Object);
		}
		#endregion
	}
}
