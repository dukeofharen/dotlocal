using System.Text;

namespace Ducode.Local.Utilities
{
	public static class StringBuilderExtensions
	{
		public static bool Contains(this StringBuilder builder, string input)
		{
			return builder.ToString().Contains(input);
		}
	}
}
