using Ducode.Local.Business;
using Microsoft.Owin;
using System;
using System.Threading.Tasks;

namespace Ducode.Local.Middleware
{
	public class LoggerMiddleware : OwinMiddleware
	{
		private readonly IConsoleWriter _consoleWriter;

		public LoggerMiddleware(OwinMiddleware next, IConsoleWriter consoleWriter) : base(next)
		{
			_consoleWriter = consoleWriter;
		}

		public async override Task Invoke(IOwinContext context)
		{
			_consoleWriter.WriteLine(string.Format("{0}: {1} ({2})", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), context.Request.Uri, context.Request.RemoteIpAddress));
			await Next.Invoke(context);
		}
	}
}
