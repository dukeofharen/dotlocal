namespace Ducode.Local.Business
{
	public interface IWebManager
	{
		void Serve(int port, string rootPath, string defaultFile, bool enableDirectoryListing, bool enableLogging, string username, string password);
	}
}
