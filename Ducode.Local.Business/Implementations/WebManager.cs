using Ducode.Local.Resources;

namespace Ducode.Local.Business.Implementations
{
	public class WebManager : IWebManager
	{
		private readonly INetworkService _networkService;
		private readonly IWebServer _webServer;
		private readonly IConsoleWriter _consoleWriter;
		private readonly IFileService _fileService;

		public WebManager(INetworkService networkService, IWebServer webServer, IConsoleWriter consoleWriter, IFileService fileService)
		{
			_networkService = networkService;
			_webServer = webServer;
			_consoleWriter = consoleWriter;
			_fileService = fileService;
		}

		public void Serve(int port, string rootPath, string defaultFile, bool enableDirectoryListing, bool enableLogging, string username, string password)
		{
			string baseAddress = string.Format(Strings.url_format, port);
			bool valid = true;

			if (!_networkService.PortAvailable(port))
			{
				valid = false;
				_consoleWriter.WriteLine(string.Format(Strings.port_in_use, port));
			}
			if (!_fileService.DirectoryExists(rootPath))
			{
				valid = false;
				_consoleWriter.WriteLine(string.Format(Strings.web_server_root_invalid, rootPath));
			}

			if (string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) ||
				!string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password))
			{
				valid = false;
				_consoleWriter.WriteLine(Strings.fill_in_username_and_password);
			}

			if (!valid)
			{
				_consoleWriter.WriteLine(Strings.press_any_key);
				_consoleWriter.ReadLine();
			}
			else
			{
				using (_webServer.CreateServer(baseAddress, enableDirectoryListing, rootPath, defaultFile, enableLogging, username, password))
				{
					_consoleWriter.WriteLine(string.Format(Strings.listening_at_port, port));

					if (!string.IsNullOrEmpty(defaultFile))
					{
						_consoleWriter.WriteLine(string.Format(Strings.default_file, defaultFile));
					}

					if (enableDirectoryListing)
					{
						_consoleWriter.WriteLine(Strings.directory_listing_enabled);
					}
					else
					{
						_consoleWriter.WriteLine(Strings.directory_listing_disabled);
					}

					if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
					{
						_consoleWriter.WriteLine(Strings.basic_auth_enabled);
					}

					if (enableLogging)
					{
						_consoleWriter.WriteLine(Strings.logging_enabled);
					}
					else
					{
						_consoleWriter.WriteLine(Strings.logging_disabled);
					}

					_consoleWriter.WriteLine(string.Format(Strings.web_server_root, rootPath));
					_consoleWriter.WriteLine(Strings.press_any_key);
					_consoleWriter.ReadLine();
				}
			}
		}
	}
}
