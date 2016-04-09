using Ducode.Local.Business;
using Ducode.Local.Business.Implementations;
using Ducode.Local.Implementations;
using Microsoft.Practices.Unity;

namespace Ducode.Local.IOC
{
	public class Container
	{
		private static IUnityContainer _container;
		public static IUnityContainer Instance
		{
			get
			{
				if(_container == null)
				{
					Initialize();
				}
				return _container;
			}
		}

		private static void Initialize()
		{
			_container = new UnityContainer();

			//business
			_container.RegisterType<INetworkService, NetworkService>();
			_container.RegisterType<IWebManager, WebManager>();
			_container.RegisterType<IWebServer, WebServer>();
			_container.RegisterType<IConsoleWriter, ConsoleWriter>();
			_container.RegisterType<IFileService, FileService>();
			_container.RegisterType<IApplicationManager, ApplicationManager>();
		}
	}
}
