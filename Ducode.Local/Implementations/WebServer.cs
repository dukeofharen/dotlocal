using Ducode.Local.Business;
using Ducode.Local.Middleware;
using Ducode.Local.Providers;
using Ducode.Local.Resources;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.StaticFiles;
using Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Ducode.Local.Implementations
{
	public class WebServer : IWebServer
	{
		private readonly IConsoleWriter _consoleWriter;
		private readonly IApplicationManager _applicationManager;

		public WebServer(IConsoleWriter consoleWriter, IApplicationManager applicationManager)
		{
			_consoleWriter = consoleWriter;
			_applicationManager = applicationManager;
		}

		public IDisposable CreateServer(string baseAddress, bool enableDirectoryListing, string rootPath, string defaultFile, bool enableLogging, string username, string password)
		{
			return WebApp.Start(baseAddress, builder =>
			{
				//custom middleware
				builder.Use(typeof(XPoweredByMiddleware), _applicationManager);
				if (enableLogging)
				{
					builder.Use(typeof(LoggerMiddleware), _consoleWriter);
				}

				if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
				{
					builder.Use(typeof(BasicAuthMiddleware), username, password);
				}

				var options = new FileServerOptions()
				{
					EnableDirectoryBrowsing = enableDirectoryListing,
					RequestPath = new PathString(),
					FileSystem = new PhysicalFileSystem(rootPath),
					EnableDefaultFiles = !string.IsNullOrEmpty(defaultFile)
				};
				options.StaticFileOptions.ServeUnknownFileTypes = true;

				var contentProvider = new CustomContentProvider();
				contentProvider.HandleMimeFile(Files.mime_types);
				string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().FullName);
				string file = Path.Combine(path, "mimeTypes.txt");
				if (File.Exists(file))
				{
					var mimeContents = File.ReadAllText(file);
					contentProvider.HandleMimeFile(mimeContents);
				}
				options.StaticFileOptions.ContentTypeProvider = contentProvider;

				if (!string.IsNullOrEmpty(defaultFile))
				{
					options.DefaultFilesOptions.DefaultFileNames = new List<string>() { defaultFile };
				}
				builder.UseFileServer(options);
			});
		}
		
	}
}
