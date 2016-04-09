using Microsoft.Owin.StaticFiles.ContentTypes;
using System;

namespace Ducode.Local.Providers
{
	public class CustomContentProvider : FileExtensionContentTypeProvider
	{
		public CustomContentProvider()
		{
			
		}

		public void HandleMimeFile(string contents)
		{
			var lines = contents.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
			if(lines != null && lines.Length > 0)
			{
				foreach(var line in lines)
				{
					var parts = line.Split(new char[] { '|' });
					if(parts.Length == 2)
					{
						var extensions = parts[0].Split(new char[] { ',' });
						foreach(var extension in extensions)
						{
							if (Mappings.ContainsKey(extension))
							{
								Mappings.Remove(extension);
							}
							Mappings.Add(extension, parts[1]);
						}
					}
				}
			}
		}
	}
}
