using Microsoft.Owin;
using System;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Ducode.Local.Middleware
{
	public class BasicAuthMiddleware : OwinMiddleware
	{
		private readonly string _username;
		private readonly string _password;

		public BasicAuthMiddleware(OwinMiddleware next, string username, string password) : base(next)
		{
			_username = username;
			_password = password;
		}

		public override async Task Invoke(IOwinContext context)
		{
			var response = context.Response;
			var request = context.Request;

			if(string.IsNullOrEmpty(_username) && string.IsNullOrEmpty(_password))
			{
				await Next.Invoke(context);
				return;
			}

			var header = request.Headers["Authorization"];
			if (!string.IsNullOrEmpty(header))
			{
				var authHeader = AuthenticationHeaderValue.Parse(header);
				if ("Basic".Equals(authHeader.Scheme, StringComparison.OrdinalIgnoreCase))
				{
					var parameter = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter));
					var parts = parameter.Split(':');

					var username = parts[0];
					var password = parts[1];

					if(username != _username || password != _password)
					{
						Forbidden(context.Response);
						return;
					}
				}
			}
			else
			{
				Forbidden(context.Response);
				return;
			}

			await Next.Invoke(context);
		}

		private void Forbidden(IOwinResponse response)
		{
			response.Headers.Add("WWW-Authenticate", new[] { "Basic" });
			response.StatusCode = 401;
			response.ReasonPhrase = "Not Authorized";
		}
	}
}
