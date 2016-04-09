using Ducode.Local.Business;
using System;

namespace Ducode.Local.Implementations
{
	public class ConsoleWriter : IConsoleWriter
	{
		public string ReadLine()
		{
			return Console.ReadLine();
		}

		public void WriteLine(string input)
		{
			Console.WriteLine(input);
		}
	}
}
