using Ducode.Local.Business;
using Ducode.Local.Resources;
using Microsoft.Owin;
using System.Threading.Tasks;

namespace Ducode.Local.Middleware
{
	public class XPoweredByMiddleware : OwinMiddleware
	{
		private readonly IApplicationManager _applicationManager;

		public XPoweredByMiddleware(OwinMiddleware next, IApplicationManager applicationManager) : base(next)
		{
			_applicationManager = applicationManager;
		}

		public async override Task Invoke(IOwinContext context)
		{
			string version = _applicationManager.Version;
			context.Response.Headers.Add(Vars.xpoweredby, new string[] { string.Format(Vars.xpoweredby_value, version) });

			await Next.Invoke(context);
		}
	}
}
