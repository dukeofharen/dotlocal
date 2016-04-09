using System.IO;

namespace Ducode.Local.Business.Implementations
{
	public class FileService : IFileService
	{
		public bool DirectoryExists(string directoryPath)
		{
			return Directory.Exists(directoryPath);
		}
	}
}
