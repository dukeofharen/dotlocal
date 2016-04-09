using CommandLine;
using CommandLine.Text;

namespace Ducode.Local.Models
{
	public class CMDOptions
	{
		[Option('p', "port", HelpText = "The port number which should be used.", Required = false)]
		public int? Port { get; set; }

		[Option('r', "root", HelpText = "The root path of the web folder.", Required = false)]
		public string RootPath { get; set; }

		[Option('d', "defaultFile", HelpText = "The default file that will be served to the user on the root URL.", Required = false)]
		public string DefaultFile { get; set; }

		[Option('l', "enableListing", HelpText = "Whether directory listing should be enabled or not.", Required = false)]
		public bool? EnableDirectoryListing { get; set; }

		[Option('o', "enableLogging", HelpText = "Whether the logging of calls should be written to the console.", Required = false)]
		public bool? EnableLogging { get; set; }

		[Option('u', "username", HelpText = "The username that should be used for optional basic authentication.", Required = false)]
		public string Username { get; set; }

		[Option('w', "password", HelpText = "The password that should be used for optional basic authentication.", Required = false)]
		public string Password { get; set; }

		[HelpOption]
		public string GetUsage()
		{
			return HelpText.AutoBuild(this, (HelpText current) =>
			{
				current.Heading = " ";
				current.Copyright = " ";
				HelpScreenShown = true;
				HelpText.DefaultParsingErrorsHandler(this, current);
			});
		}

		public bool HelpScreenShown { get; set; }
	}
}
