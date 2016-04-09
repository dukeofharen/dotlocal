namespace Ducode.Local.Extensions
{
	public static class CommandLineExtensions
	{
		public static TOptions ParseArguments<TOptions>(this CommandLine.Parser parser, string[] args) where TOptions : new()
		{
			TOptions options = new TOptions();
			parser.ParseArguments(args, options);
			return options;
		}
	}
}
