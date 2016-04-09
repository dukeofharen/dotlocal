using System;

namespace Ducode.Local.Business
{
	public interface IWebServer
	{
		IDisposable CreateServer(string baseAddress, bool enableDirectoryListing, string rootPath, string defaultFile, bool enableLogging, string username, string password);
	}
}
