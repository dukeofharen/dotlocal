using Ducode.Local.Business;
using System.Reflection;
using System;

namespace Ducode.Local.Implementations
{
	public class ApplicationManager : IApplicationManager
	{
		public string Version
		{
			get
			{
				return Assembly.GetExecutingAssembly().GetName().Version.ToString();
			}
		}

		public string GetDefaultFolder()
		{
			return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
		}
	}
}
