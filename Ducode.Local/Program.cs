using Ducode.Local.IOC;
using Ducode.Local.Models;
using Ducode.Local.Extensions;
using Microsoft.Practices.Unity;
using System;
using Ducode.Local.Business;
using Ducode.Local.Resources;

namespace Ducode.Local
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				About();

				var applicationManager = Container.Instance.Resolve<IApplicationManager>();

				var options = CommandLine.Parser.Default.ParseArguments<CMDOptions>(args);
				if (options.HelpScreenShown)
				{
					return;
				}

				int port = 9000;
				if (options.Port.HasValue)
				{
					port = options.Port.Value;
				}

				string rootPath = applicationManager.GetDefaultFolder();
				if (!string.IsNullOrEmpty(options.RootPath))
				{
					rootPath = options.RootPath;
				}

				string defaultFile = options.DefaultFile;

				bool enableDirectoryListing = true;
				if (options.EnableDirectoryListing.HasValue)
				{
					enableDirectoryListing = options.EnableDirectoryListing.Value;
				}

				bool enableLogging = true;
				if (options.EnableLogging.HasValue)
				{
					enableLogging = options.EnableLogging.Value;
				}

				string username = options.Username;
				string password = options.Password;

				Container.Instance.Resolve<IWebManager>().Serve(port, rootPath, defaultFile, enableDirectoryListing, enableLogging, username, password);
			}
			catch(Exception e)
			{
				Console.WriteLine(e);
				Console.WriteLine(Strings.press_any_key);
				Console.ReadKey();
			}
		}

		private static void About()
		{
			var applicationManager = Container.Instance.Resolve<IApplicationManager>();
			Console.WriteLine(Strings.dotlocal_letters);
			Console.WriteLine(Strings.copyright);
			Console.WriteLine(string.Format(Strings.dotlocal, applicationManager.Version));
			Console.WriteLine();
		}
	}
}
