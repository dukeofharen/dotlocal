namespace Ducode.Local.Business
{
	public interface IApplicationManager
	{
		string Version { get; }
		string GetDefaultFolder();
	}
}
