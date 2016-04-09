using System;
using System.Net.Sockets;

namespace Ducode.Local.Business.Implementations
{
	public class NetworkService : INetworkService
	{
		public bool PortAvailable(int port)
		{
			using (TcpClient tcpClient = new TcpClient())
			{
				try
				{
					tcpClient.Connect("127.0.0.1", port);
					return false;
				}
				catch (Exception)
				{
					return true;
				}
			}
		}
	}
}
